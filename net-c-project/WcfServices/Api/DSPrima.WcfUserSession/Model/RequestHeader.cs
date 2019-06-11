using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPrima.WcfUserSession.Model
{
    /// <summary>
    /// Defines the data to be send with a request
    /// </summary>
    public class RequestHeader
    {
        /// <summary>
        /// Gets or sets the session ID
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// Gets or sets the IP address(es) of the machine that does the request.
        /// If there are multiple address they have to be seperated by a pipe "|".
        /// </summary>
        public string ClientIp { get; set; }

        /// <summary>
        /// Gets or sets the name of the machine that does the request
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// Gets or sets the Ip of the user that does the request, can be the same as the machine if the user is on the actual machine.
        /// </summary>
        public string UserIp { get; set; }
    }
}
