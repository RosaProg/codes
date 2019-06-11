using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.PMS.MessageDataLibrary.Model
{
    /// <summary>
    /// Defines a message to be stored
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Gets or sets the Database Id
        /// </summary>        
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the MessageReference
        /// </summary>
        [Index, MaxLength(100)]
        public string MessageReference { get; set; }

        /// <summary>
        /// Gets or sets the message itself
        /// </summary>
        public string MessageText { get; set; }

        /// <summary>
        /// Gets or sets the Date and Time the message was send or received
        /// </summary>
        public DateTime dateTimeOfMessage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this message was send (true) or received (false)
        /// </summary>
        public bool WasSent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Message was successful or not.
        /// </summary>
        public bool Success { get; set; }
    }
}
