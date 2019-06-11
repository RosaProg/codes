using PCHI.Model.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Security
{
    /// <summary>
    /// Defines the details for a session.
    /// </summary>
    public class SessionDetails
    {
        /// <summary>
        /// Gets or sets The ID of the session
        /// </summary>        
        [Key]
        public string SessionId { get; set; }

        /// <summary>
        /// Gets or sets the Role for the session
        /// </summary>        
        public string Role { get; set; }

        /// <summary>
        /// Gets or sets the last time this session was accessed
        /// </summary>
        public DateTime LastAccess { get; set; }
    }
}
