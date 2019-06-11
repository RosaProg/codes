using PCHI.WcfServices.PMS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PCHI.WcfServices.PMS.Models
{
    [XmlInclude(typeof(PatientAppointment))]
    public abstract class MessageDataObject : Message
    {
        /// <summary>
        /// Gets or sets the unique Id of MessageDataObject.
        /// This Id can be a database Id and is used to identify an object that may already exists on the receiving side and needs to be updated.
        /// </summary>
        public string Id { get; set; }
    }
}
