using DSPrima.WcfUserSession.Interfaces;
using DSPrima.WcfUserSession.Model;
using DSPrima.WcfUserSession.SecurityHandlers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPrima.WcfUserSession.SessionStores
{
    /// <summary>
    /// Defines a SessionStore that stores the session data in RAM memory
    /// </summary>
    public class ServerSessionStore : IUserSessionStore
    {
        /// <summary>
        /// The store the session data is held in.
        /// </summary>
        private static ConcurrentDictionary<string, SessionData> store = new ConcurrentDictionary<string, SessionData>();

        /// <summary>
        /// Stores a given session Key as a valid session
        /// </summary>
        /// <param name="sessionKey">The session Key to store</param>
        /// <param name="data">The session data belonging to this session</param>
        public void StoreSession(string sessionKey, SessionData data)
        {
            if (!ServerSessionStore.store.ContainsKey(sessionKey))
            {
                data.LastSessionAccessTime = DateTime.Now;
                ServerSessionStore.store.TryAdd(sessionKey, data);
            }
        }

        /// <summary>
        /// Validates if a given Session Key is still valid (i.e. not expired) and mark it as still alive if it is still valid
        /// It is up to the Session Store to determine how to do it, by time or something else (most likely time)
        /// </summary>
        /// <param name="sessionKey">The session key to verify</param>
        /// <returns>True if the session key is still valid, false otherwise</returns>
        public bool ValidateSession(string sessionKey)
        {
            if (ServerSessionStore.store.ContainsKey(sessionKey) && ServerSessionStore.store[sessionKey].LastSessionAccessTime.AddMinutes(WcfUserSessionSecurity.SessionTimeout) > DateTime.Now)
            {
                ServerSessionStore.store[sessionKey].LastSessionAccessTime = DateTime.Now;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes the given session from the list
        /// </summary>
        /// <param name="sessionKey">The session Key to remove</param>
        public void RemoveSession(string sessionKey)
        {
            if (ServerSessionStore.store.ContainsKey(sessionKey))
            {
                SessionData data = null;
                ServerSessionStore.store.TryRemove(sessionKey, out data);
            }
        }

        /// <summary>
        /// Updates the SessionData for the given sessionKey
        /// </summary>
        /// <param name="sessionKey">The session Key for which to authenticate the session</param>
        /// <param name="data">The new session data</param>
        public void UpdateSessionData(string sessionKey, SessionData data)
        {
            ServerSessionStore.store[sessionKey] = data;
        }

        /// <summary>
        /// Gets a copy of the session data irresspective of whether the session is validated or not. 
        /// </summary>
        /// <param name="sessionKey">The session key for which to get the session data</param>
        /// <returns>The session data if found or null if not found</returns>
        public SessionData GetSessionData(string sessionKey)
        {
            if (ServerSessionStore.store.Keys.Contains(sessionKey)) return ServerSessionStore.store[sessionKey];
            return null;
        }
    }
}
