using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPrima.WcfUserSession.Model
{
    /// <summary>
    /// Holds the secured client session data for storing
    /// </summary>
    public class SecuredClientsessionData
    {
        /// <summary>
        /// Gets or sets the secured data string
        /// </summary>
        public string SecuredData { get; set; }

        /// <summary>
        /// Gets or sets the last time this data was accessed
        /// </summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Gets or sets the session timeout in minute to ensure automatic cleaning.
        /// </summary>
        public int SessionTimeoutInMinutes { get; set; }
    }
}
