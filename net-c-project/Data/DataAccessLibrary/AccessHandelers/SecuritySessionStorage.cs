using PCHI.DataAccessLibrary.Context;
using PCHI.Model.Security;
using PCHI.Model.Users;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PCHI.DataAccessLibrary.AccessHandelers
{
    /// <summary>
    /// Provides storage support for the Security Session
    /// </summary>
    public class SecuritySessionStorage
    {
        /// <summary>
        /// Holds the internal data store
        /// </summary>
        private ConcurrentDictionary<string, SessionDetails> internalStore = new ConcurrentDictionary<string, SessionDetails>();

        /// <summary>
        /// Holds the DB Update Queue
        /// </summary>
        private Queue<SessionDetails> updatesToStore = new Queue<SessionDetails>();

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
        /// Initializes a new instance of the <see cref="SecuritySessionStorage"/> class
        /// </summary>
        public SecuritySessionStorage()
        {
            using (MainDatabaseContext context = new MainDatabaseContext())
            {
                this.internalStore = new ConcurrentDictionary<string, SessionDetails>(context.SessionStore.ToDictionary(s => s.SessionId, s => s));
            }

            this.databaseUpdater = new TaskFactory().StartNew(this.UpdateDatabase);

            this.houseKeeper = new System.Timers.Timer();
            this.houseKeeper.Interval = 60000; // 1 minute
            this.houseKeeper.AutoReset = true;
            this.houseKeeper.Elapsed += this.HouseKeeper_Elapsed;
            this.houseKeeper.Start();
        }

        /// <summary>
        /// Executed when the HouseKeeper timer is elapsed.
        /// Goes through the internal store, marks any out of date session data for removing
        /// </summary>
        /// <param name="sender">The sender of the event, this is ignored</param>
        /// <param name="e">The event arguments, this is ignored</param>
        private void HouseKeeper_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            List<SessionDetails> data = this.internalStore.Select(s => new SessionDetails() { SessionId = s.Key, Role = s.Value.Role, LastAccess = s.Value.LastAccess }).ToList();
            foreach (SessionDetails session in data)
            {
                try
                {
                    if (!this.IsValid(session))
                    {
                        this.RemoveSession(session.SessionId);
                    }
                }
                catch (Exception)
                {
                    this.RemoveSession(session.SessionId);
                }
            }
        }

        /// <summary>
        /// Updates the database based upon SessionDetails in the UpdatesToStore queue. 
        /// If the SessioStoreData.SessionData entry is null, the data is removed.
        /// Runs in a loop until either Token.IsCancellationRequested is true.
        /// Will finish updating the database when we are disposing and there are still updates to store
        /// </summary>
        private void UpdateDatabase()
        {
            while (!this.token.IsCancellationRequested || (this.disposing && this.updatesToStore.Count > 0))
            {
                MainDatabaseContext context = new MainDatabaseContext();

                int count = 0;
                if (this.updatesToStore.Count > 0)
                {
                    while (this.updatesToStore.Count > 0 && count < 100)
                    {
                        count++;
                        SessionDetails session = this.updatesToStore.Dequeue();
                        if (session.Role == null)
                        {
                            if (context.SessionStore.Any(s => s.SessionId == session.SessionId))
                            {
                                context.SessionStore.Local.Where(s => s.SessionId == session.SessionId).ToList().ForEach(s => context.Entry(s).State = System.Data.Entity.EntityState.Deleted);
                            }
                            else
                            {
                                // Crashed on duplicate key error
                                context.Entry(session).State = System.Data.Entity.EntityState.Deleted;
                            }
                        }
                        else
                        {
                            SessionDetails newSession = new SessionDetails();
                            newSession.SessionId = session.SessionId;
                            newSession.Role = session.Role;
                            newSession.LastAccess = session.LastAccess;                            

                            if (context.SessionStore.Local.Any(s => s.SessionId == newSession.SessionId) && context.Entry(context.SessionStore.Local.Where(s => s.SessionId == newSession.SessionId).Single()).State != System.Data.Entity.EntityState.Deleted)
                            {
                                SessionDetails details = context.SessionStore.Local.Where(s => s.SessionId == newSession.SessionId).Single();
                                details.Role = newSession.Role;                                
                                details.LastAccess = newSession.LastAccess;
                            }
                            else if (context.SessionStore.Any(s => s.SessionId == newSession.SessionId))
                            {
                                context.Entry(newSession).State = System.Data.Entity.EntityState.Modified;
                            }
                            else
                            {
                                context.Entry(newSession).State = System.Data.Entity.EntityState.Added;
                            }                            
                        }
                    }

                    if (count > 0)
                    {
                        try
                        {
                            context.SaveChanges();
                        }
                        catch (DbEntityValidationException e)
                        {
                            string errorResult = string.Empty;
                            foreach (var eve in e.EntityValidationErrors)
                            {
                                errorResult += "Entity of type \" " + eve.Entry.Entity.GetType().Name + "\" in state \"" + eve.Entry.State + "\" has the following validation errors: \n";
                                foreach (var ve in eve.ValidationErrors)
                                {
                                    errorResult += "- Property: \"" + ve.PropertyName + "\", Error: \"" + ve.ErrorMessage + "\" \n";
                                }
                            }

                            throw new DbEntityValidationException(errorResult, e);
                        }
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
        /// <param name="sessionId">The sessionId to store</param>
        /// <param name="role">The session Data to store</param>
        private void UpdateStorage(string sessionId, string role)
        {
            if (!this.internalStore.ContainsKey(sessionId))
            {
                SessionDetails session = new SessionDetails();
                session.SessionId = sessionId;
                session.Role = role;
                session.LastAccess = DateTime.Now;
                this.internalStore.TryAdd(sessionId, session);
            }
            else
            {
                this.internalStore[sessionId].Role = role;
                this.internalStore[sessionId].LastAccess = DateTime.Now;
            }

            this.updatesToStore.Enqueue(this.internalStore[sessionId]);
        }

        /// <summary>
        /// Removes the session key from internal and DB storage
        /// </summary>
        /// <param name="sessionId">The session key of the session to remove</param>
        private void RemoveFromStorage(string sessionId)
        {
            SessionDetails session = new SessionDetails();
            session.SessionId = sessionId;
            this.updatesToStore.Enqueue(session);
            if (this.internalStore.ContainsKey(sessionId))
            {
                SessionDetails output = null;
                this.internalStore.TryRemove(sessionId, out output);
            }
        }

        /// <summary>
        /// Stores a given session Key as a valid session
        /// </summary>
        /// <param name="sessionId">The session Key to store</param>
        /// <param name="role">The session data belonging to this session</param>
        public void StoreSession(string sessionId, string role)
        {
            this.UpdateStorage(sessionId, role);
        }

        /// <summary>
        /// Validates if a given Session Key is still valid (i.e. not expired) and mark it as still alive if it is still valid
        /// It is up to the Session Store to determine how to do it, by time or something else (most likely time)
        /// </summary>
        /// <param name="sessionId">The session key to verify</param>
        /// <returns>True if the session key is still valid, false otherwise</returns>
        public bool ValidateSession(string sessionId)
        {
            if (this.internalStore.ContainsKey(sessionId))
            {
                this.UpdateStorage(sessionId, this.internalStore[sessionId].Role);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes the given session from the list
        /// </summary>
        /// <param name="sessionId">The session Key to remove</param>
        public void RemoveSession(string sessionId)
        {
            this.RemoveFromStorage(sessionId);
        }

        /// <summary>
        /// Updates the SessionData for the given sessionId
        /// </summary>
        /// <param name="sessionId">The session Key for which to authenticate the session</param>
        /// <param name="role">The new session data</param>
        public void UpdateSessionData(string sessionId, string role)
        {
            if (this.internalStore.ContainsKey(sessionId))
            {
                this.UpdateStorage(sessionId, role);
            }
        }

        /// <summary>
        /// Gets a copy of the session data irresspective of whether the session is validated or not. 
        /// </summary>
        /// <param name="sessionId">The session key for which to get the session data</param>
        /// <returns>The session role if found or null if not found</returns>
        public string Role(string sessionId)
        {
            if (!string.IsNullOrWhiteSpace(sessionId) && this.internalStore.ContainsKey(sessionId))
            {
                this.ValidateSession(sessionId);
                return this.internalStore[sessionId].Role;
            }

            return null;
        }

        /// <summary>
        /// Checks if the current session data is still valid. Used for automatic deletion after 24 hours after the last access
        /// </summary>
        /// <param name="details">The session details to check</param>
        /// <returns>True if still valid, false otherwise</returns>
        private bool IsValid(SessionDetails details)
        {
            return details.LastAccess > DateTime.Now.AddDays(-1);
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
