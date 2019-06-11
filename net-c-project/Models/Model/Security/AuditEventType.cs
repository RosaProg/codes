using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.Model.Security
{
    /// <summary>
    /// Describes the type of events
    /// </summary>
    public enum AuditEventType
    {
        /// <summary>
        /// The Event is a ADD of the object
        /// </summary>
        ADD,

        /// <summary>
        /// The event is DELETE of the object
        /// </summary>
        DELETE,

        /// <summary>
        /// The event is a MODIFICATION of the object
        /// </summary>
        MODIFIED,

        /// <summary>
        /// The event is a READ of the object
        /// </summary>
        READ
    }
}
