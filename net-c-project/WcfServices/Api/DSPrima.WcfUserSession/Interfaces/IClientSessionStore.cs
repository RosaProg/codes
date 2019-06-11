using DSPrima.WcfUserSession.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPrima.WcfUserSession.Interfaces
{
    /// <summary>
    /// Defines the storage functionality for the client side.
    /// The storage is responsible for it's own cleanup. 
    ///     The ClientSessionData contains the last time the data was updated 
    ///     The ClientSessionData.UserSessionConfiguration holds the timeout in minutes for the session.
    ///     
    /// These two variables can be used for automatic cleanup.
    /// </summary>
    public interface IClientSessionStore
    {
        /// <summary>
        /// Stores a session and the ClientSessionData
        /// </summary>
        /// <param name="sessionId">The session Id to store it under</param>
        /// <param name="data">The session Data to store</param>
        void StoreSession(string sessionId, ClientSessionData data);

        /// <summary>
        /// Retrieves the Client Session Data for the given session ID
        /// </summary>
        /// <param name="sessionId">The Id of the session to retrieve the data for</param>
        /// <returns>The Client Session Data</returns>
        ClientSessionData GetSessionData(string sessionId);
    }
}
