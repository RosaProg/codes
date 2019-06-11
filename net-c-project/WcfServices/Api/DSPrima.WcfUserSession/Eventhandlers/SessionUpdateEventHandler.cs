using DSPrima.WcfUserSession.Interfaces;
using DSPrima.WcfUserSession.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPrima.WcfUserSession.Eventhandlers
{
    /// <summary>
    /// The event handlers that is executed each time the current session data is updated.
    /// </summary>
    /// <param name="sessionId">The id of the current session</param>
    /// <param name="user">The new/updated user data</param>
    /// <param name="header">The header that was passed on with the request</param>
    public delegate void SessionUpdateEventHandler(string sessionId, IUser user, RequestHeader header);
}
