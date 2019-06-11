using DSPrima.WcfUserSession.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPrima.WcfUserSession.Interfaces
{
    /// <summary>
    /// Defines the functionality for a UserSessionStore
    /// </summary>
    public interface IUserSessionStore
    {
        /// <summary>
        /// Stores a given session Key as a valid session
        /// </summary>
        /// <param name="sessionKey">The session Key to store</param>
        /// <param name="data">The session data belonging to this session</param>
        void StoreSession(string sessionKey, SessionData data);

        /// <summary>
        /// Validates if a given Session Key is still valid (i.e. not expired) and mark it as still alive if it is still valid
        /// It is up to the Session Store to determine how to do it, by time or something else (most likely time)
        /// </summary>
        /// <param name="sessionKey">The session key to verify</param>
        /// <returns>True if the session key is still valid, false otherwise</returns>
        bool ValidateSession(string sessionKey);

        /// <summary>
        /// Updates the SessionData for the given sessionKey
        /// </summary>
        /// <param name="sessionKey">The session Key for which to authenticate the session</param>
        /// <param name="data">The new session data</param>
        void UpdateSessionData(string sessionKey, SessionData data);

        /// <summary>
        /// Gets a copy of the session data irresspective of whether the session is validated or not. 
        /// </summary>
        /// <param name="sessionKey">The session key for which to get the session data</param>
        /// <returns>The session data if found or null if not found</returns>
        SessionData GetSessionData(string sessionKey);

        /// <summary>
        /// Removes the given session from the list
        /// </summary>
        /// <param name="sessionKey">The session Key to remove</param>
        void RemoveSession(string sessionKey);
    }
}
