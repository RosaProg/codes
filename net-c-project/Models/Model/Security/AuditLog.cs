using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Security
{
    /// <summary>
    /// Defines a Audit log entry
    /// </summary>
    public class AuditLog
    {
        /// <summary>
        /// Gets or sets the database Id of the event
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the Id of the user that done the event
        /// </summary>
        [MaxLength(128)]
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the IP Address the user is connecting from
        /// </summary>
        public string UserIp { get; set; }

        /// <summary>
        /// Gets or sets the IP Addresses from the client machine (such as the webserver). A server can have multiple IP's which are all stored in here.
        /// </summary>
        public string ClientIps { get; set; }

        /// <summary>
        /// Gets or sets the name of the client machine (such as the webserver)
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// Gets or sets when the event has happened
        /// </summary>
        public DateTime EventDateUTC { get; set; }
                
        /// <summary>
        /// Gets or sets the type of event
        /// </summary>        
        public AuditEventType EventType { get; set; }

        /// <summary>
        /// Gets or sets the string representation of the event type
        /// </summary>
        public string EventTypeName { get; set; }

        /// <summary>
        /// Gets or sets the user action that caused this event
        /// </summary>
        public Actions Action { get; set; }

        /// <summary>
        /// Gets or sets the name of the action that caused this event
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// Gets or sets the table name that has the change
        /// </summary>
        [MaxLength(128)]
        public string ObjectType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the action was a success (true) or not (false)
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the name of the field the Record Id belongs to.
        /// </summary>
        public string FieldName { get; set; }
        
        /// <summary>
        /// Gets or sets the ID of the Record that has been affected.
        /// This can Id is a value in a the field found in FieldName
        /// </summary>
        [MaxLength(250)]
        public string RecordId { get; set; }
        /*
        /// <summary>
        /// Gets or sets the name of the column that has changed
        /// </summary>
        [MaxLength(250)]
        public string ColumnName { get; set; }

        /// <summary>
        /// Gets or sets the New Value
        /// </summary>
        public string Value { get; set; }*/

        /// <summary>
        /// Gets or sets a message belonging to this AuditLog entry. This may be null if this is entry only regarding saving data
        /// </summary>
        public string Message { get; set; }
    }
}
