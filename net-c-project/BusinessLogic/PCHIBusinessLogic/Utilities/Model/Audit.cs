using PCHI.Model.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.BusinessLogic.Utilities.Model
{
    /// <summary>
    /// Defines the data needed for the Audit Entry
    /// </summary>
    public class Audit
    {
        /// <summary>
        /// Gets or sets the action for this Audit
        /// </summary>
        public Actions Action { get; set; }

        /// <summary>
        /// Gets or sets the type of event
        /// </summary>
        public AuditEventType AuditEventType { get; set; }

        /// <summary>
        /// Gets or sets the type of the object
        /// </summary>
        public string ObjectType { get; set; }

        /// <summary>
        /// Gets or sets the name of the field the Id belongs to
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Gets or sets the object Id (if it exists already)
        /// </summary>
        public string ObjectId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the action was a success
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Audit"/> class
        /// </summary>
        /// <param name="action">The action that causes the audit</param>
        /// <param name="eventType">The type of event</param>
        /// <param name="objectType">The type of Object (use o.GetType().Name)</param>
        /// <param name="fieldName">The name of the field the object identifier can be found</param>
        /// <param name="objectId">The Id of the object.</param>
        /// <param name="success">Indicates whether the action was a success or not</param>
        /// <param name="message">The optional message to add</param>
        public Audit(Actions action, AuditEventType eventType, Type objectType, string fieldName, string objectId, bool success = true, string message = null)
        {
            this.Action = action;
            this.AuditEventType = eventType;
            this.ObjectType = objectType.Name;
            this.FieldName = fieldName;
            this.ObjectId = objectId;
            this.Success = success;
            this.Message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Audit"/> class
        /// </summary>
        /// <param name="action">The action that causes the audit</param>
        /// <param name="eventType">The type of event</param>
        /// <param name="o">The Object. The object type and Id are retrieved from the object instance</param>
        /// <param name="success">Indicates whether the action was a success or not</param>
        /// <param name="message">The optional message to add</param>
        public Audit(Actions action, AuditEventType eventType, object o, bool success = true, string message = null)
        {
            this.Action = action;
            this.AuditEventType = eventType;
            this.ObjectType = o.GetType().Name;

            if (o != null)
            {
                // Get primary key value (If you have more than one key column, this will need to be adjusted)
                var keyNames = o.GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(KeyAttribute), false).Count() > 0 || p.Name == "Id").ToList();

                bool first = true;
                foreach (PropertyInfo kn in keyNames)
                {
                    object v = o.GetType().GetProperty(kn.Name).GetValue(o);
                    this.FieldName += (first ? string.Empty : " , ") + kn.Name;
                    this.ObjectId += (first ? string.Empty : " , ") + (v != null ? v.ToString() : string.Empty);
                    first = false;
                }
            }

            this.Success = success;
            this.Message = message;
        }        
    }
}
