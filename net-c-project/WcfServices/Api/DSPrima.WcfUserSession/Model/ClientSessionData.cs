using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPrima.WcfUserSession.Model
{
    /// <summary>
    /// Holds the data for storing the UserSessionConfiguration client side along the last datetime it was accessed
    /// </summary>
    public class ClientSessionData
    {
        /// <summary>
        /// Gets or sets the <see cref="UserSessionConfiguration"/> instance
        /// </summary>
        public UserSessionConfiguration UserSessionConfiguration { get; set; }

        /// <summary>
        /// Gets or sets the last time the <see cref="UserSessionConfiguration"/> instance was updated
        /// </summary>
        public DateTime LastTimeUpdated { get; set; }
    }
}
