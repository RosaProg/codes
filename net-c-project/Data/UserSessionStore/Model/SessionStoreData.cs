using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPrima.UserSessionStore.Model
{
    /// <summary>
    /// Defines the session data
    /// </summary>
    public class SessionStoreData
    {
        /// <summary>
        /// Gets or sets the unique key for the session
        /// </summary>
        [Key]
        [MaxLength(450)]
        public string SessionKey { get; set; }

        /// <summary>
        /// Gets or sets data string for the session
        /// </summary>
        [Required]
        public string SessionData { get; set; }
    }
}
