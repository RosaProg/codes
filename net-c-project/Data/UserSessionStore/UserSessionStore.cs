using DSPrima.UserSessionStore.Context;
using DSPrima.UserSessionStore.Model;
using DSPrima.WcfUserSession.Interfaces;
using DSPrima.WcfUserSession.Model;
using DSPrima.WcfUserSession.SecurityHandlers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DSPrima.UserSessionStore
{
    /// <summary>
    /// Implements a Hybrid Memory/SlqDatabase (Entity framework) method of storing the session data
    /// </summary>
    public class UserSessionStore : IUserSessionStore, IDisposable
    {
        /// <summary>
        /// Holds the internal data store
        /// </summary>
        private ConcurrentDictionary<string, string> internalStore = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// Holds the DB Update Queue
        /// </summary>
        private Queue<SessionStoreData> updatesToStore = new Queue<SessionStoreData>();

        /// <summary>
        /// A timer that when it timesout will do the housekeeping and cleans up old data from RAM
        /// </summary>
        private System.Timers.Timer houseKeeper;

        /// <summary>
        /// The process that stored the updates in the database
        /// </summary>
        private Task databaseUpdater;

        /// <summary>
        /// Used for canceling the running process
        /// </summary>
        private CancellationTokenSource tokenSource = null;

        /// <summary>
        /// Used for canceling the running process, created from the TokenSource
        /// </summary>
        private CancellationToken token;

        /// <summary>
        /// Indicates Dispose() has been called. When true the Database does not accept more updates to store but will finish storing outstanding Updates in the database
        /// </summary>
        private bool disposing = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserSessionStore"/> class
        /// </summary>
        public UserSessionStore()
        {
            using (SessionContext context = new SessionContext())
            {
                this.internalStore = new ConcurrentDictionary<string, string>((from s in context.SessionStore select new { key = s.SessionKey, data = s.SessionData }).ToDictionary(s => s.key, s => s.data));
            }

            this.databaseUpdater = new TaskFactory().StartNew(this.UpdateDatabase);

            this.houseKeeper = new System.Timers.Timer();
            this.houseKeeper.Interval = 60000; // 1 minute
            this.houseKeeper.AutoReset = true;
            this.houseKeeper.Elapsed += this.HouseKeeper_Elapsed;
            this.houseKeeper.Start();
        }

        /// <summary>
        /// Checks if a given time is still within the session timeout as specified by the WcfUserSessionSecurity.SessionTimeout variable
        /// </summary>
        /// <param name="time">The time to check</param>
        /// <returns>True if the time is still valid, false if it has timed out (less then Now)</returns>
        private bool IsValid(DateTime time)
        {
            return time.AddMinutes(WcfUserSessionSecurity.SessionTimeout) > DateTime.Now;
        }

        /// <summary>
        /// Executed when the HouseKeeper timer is elapsed.
        /// Goes through the internal store, marks any out of date session data for removing
        /// </summary>
        /// <param name="sender">The sender of the event, this is ignored</param>
        /// <param name="e">The event arguments, this is ignored</param>
        private void HouseKeeper_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            List<SessionStoreData> data = this.internalStore.Select(s => new SessionStoreData() { SessionKey = s.Key, SessionData = s.Value }).ToList();
            foreach (SessionStoreData s in data)
            {
                try
                {
                    SessionData session = SessionData.Decrypt(s.SessionData);
                    if (!this.IsValid(session.LastSessionAccessTime))
                    {
                        this.RemoveSession(s.SessionKey);
                    }
                }
                catch (Exception)
                {
                    this.RemoveSession(s.SessionKey);
                }
            }
        }

        /// <summary>
        /// Updates the database based upon SessionStoreData in the UpdatesToStore queue. 
        /// If the SessioStoreData.SessionData entry is null, the data is removed.
        /// Runs in a loop until either Token.IsCancellationRequested is true.
        /// Will finish updating the database when we are disposing and there are still updates to store
        /// </summary>
        private void UpdateDatabase()
        {
            while (!this.token.IsCancellationRequested || (this.disposing && this.updatesToStore.Count > 0))
            {
                SessionContext context = new SessionContext();
                int count = 0;
                if (this.updatesToStore.Count > 0)
                {
                    while (this.updatesToStore.Count > 0 && count < 100)
                    {
                        count++;
                        SessionStoreData session = this.updatesToStore.Dequeue();
                        if (string.IsNullOrWhiteSpace(session.SessionData))
                        {
                            if (context.SessionStore.Any(s => s.SessionKey == session.SessionKey))
                            {
                                context.SessionStore.Local.Where(s => s.SessionKey == session.SessionKey).ToList().ForEach(s => context.Entry(s).State = System.Data.Entity.EntityState.Deleted);
                            }
                            else
                            {
                                // Crashed on duplicate key error
                                context.Entry(session).State = System.Data.Entity.EntityState.Deleted;
                            }
                        }
                        else// if (this.internalStore.ContainsKey(session.SessionKey))
                        {
                            if (context.SessionStore.Local.Any(s => s.SessionKey == session.SessionKey) && context.Entry(context.SessionStore.Local.Where(s => s.SessionKey == session.SessionKey).Single()).State != System.Data.Entity.EntityState.Deleted)
                            {
                                context.SessionStore.Local.Where(s => s.SessionKey == session.SessionKey).Single().SessionData = session.SessionData;
                            }
                            else if (context.SessionStore.Any(s => s.SessionKey == session.SessionKey))
                            {
                                context.Entry(session).State = System.Data.Entity.EntityState.Modified;
                            }
                            else
                            {
                                context.Entry(session).State = System.Data.Entity.EntityState.Added;
                            }
                        }
                    }

                    if (count > 0)
                    {
                        context.SaveChanges();
                    }
                }
                else
                {
                    Thread.Sleep(100);
                }
            }
        }

        /// <summary>
        /// Updates the interal storage and the database storage and keeps them in sync
        /// </summary>
        /// <param name="sessionKey">The sessionKey to store</param>
        /// <param name="sessionData">The session Data to store</param>
        private void UpdateStorage(string sessionKey, string sessionData)
        {
            SessionStoreData session = new SessionStoreData();
            session.SessionKey = sessionKey;
            session.SessionData = sessionData;

            if (!this.internalStore.ContainsKey(sessionKey))
            {
                this.internalStore.TryAdd(sessionKey, sessionData);
            }
            else
            {
                this.internalStore[sessionKey] = sessionData;
            }

            this.updatesToStore.Enqueue(session);
        }

        /// <summary>
        /// Removes the session key from internal and DB storage
        /// </summary>
        /// <param name="sessionKey">The session key of the session to remove</param>
        private void RemoveFromStorage(string sessionKey)
        {
            SessionStoreData session = new SessionStoreData();
            session.SessionKey = sessionKey;
            this.updatesToStore.Enqueue(session);
            if (this.internalStore.ContainsKey(sessionKey))
            {
                string output = null;
                this.internalStore.TryRemove(sessionKey, out output);
            }
        }

        /// <summary>
        /// Stores a given session Key as a valid session
        /// </summary>
        /// <param name="sessionKey">The session Key to store</param>
        /// <param name="data">The session data belonging to this session</param>
        public void StoreSession(string sessionKey, SessionData data)
        {
            data.LastSessionAccessTime = DateTime.Now;
            string sessionData = SessionData.Encrypt(data);

            this.UpdateStorage(sessionKey, sessionData);
        }

        /// <summary>
        /// Validates if a given Session Key is still valid (i.e. not expired) and mark it as still alive if it is still valid
        /// It is up to the Session Store to determine how to do it, by time or something else (most likely time)
        /// </summary>
        /// <param name="sessionKey">The session key to verify</param>
        /// <returns>True if the session key is still valid, false otherwise</returns>
        public bool ValidateSession(string sessionKey)
        {
            if (sessionKey != null && this.internalStore.ContainsKey(sessionKey))
            {
                SessionData data = SessionData.Decrypt(this.internalStore[sessionKey]);
                if (this.IsValid(data.LastSessionAccessTime))
                {
                    data.LastSessionAccessTime = DateTime.Now;
                    this.UpdateStorage(sessionKey, SessionData.Encrypt(data));
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Removes the given session from the list
        /// </summary>
        /// <param name="sessionKey">The session Key to remove</param>
        public void RemoveSession(string sessionKey)
        {
            this.RemoveFromStorage(sessionKey);
        }

        /// <summary>
        /// Updates the SessionData for the given sessionKey
        /// </summary>
        /// <param name="sessionKey">The session Key for which to authenticate the session</param>
        /// <param name="data">The new session data</param>
        public void UpdateSessionData(string sessionKey, SessionData data)
        {
            if (this.internalStore.ContainsKey(sessionKey))
            {
                this.UpdateStorage(sessionKey, SessionData.Encrypt(data));
            }
        }

        /// <summary>
        /// Gets a copy of the session data irresspective of whether the session is validated or not. 
        /// </summary>
        /// <param name="sessionKey">The session key for which to get the session data</param>
        /// <returns>The session data if found or null if not found</returns>
        public SessionData GetSessionData(string sessionKey)
        {
            if (this.internalStore.ContainsKey(sessionKey)) return SessionData.Decrypt(this.internalStore[sessionKey]);
            return null;
        }

        /// <summary>
        /// Stops updating to the database, but does not clear the queue. stops the housekeeper as well
        /// </summary>
        public void StopUpdating()
        {
            this.tokenSource.Cancel();
            try
            {
                this.databaseUpdater.Wait();
            }
            catch { } // It doesn't matter if this fails, means it's already disposed.
            this.databaseUpdater = null;
            this.houseKeeper.Stop();
        }

        /// <summary>
        /// Disposes of all data held within this object. 
        /// The function will wait for outstanding data to finish storing in the database.
        /// </summary>
        public void Dispose()
        {
            this.disposing = true;
            this.houseKeeper.Stop();
            this.updatesToStore.Clear();
            this.StopUpdating();
        }
    }
}
