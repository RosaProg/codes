using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PCHI.WcfServices.PMS.Models
{
    //Defines a message to be send to or received from the PCHI-PMS service
    public class Message
    {
        /// <summary>
        /// Gets or sets the message reference, this is to indicate a unique message.
        /// If a message is being resent, this reference is to be the same as the message it already send.        
        /// </summary>        
        public string messageReference { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the message is a success (true) and contains data of a failure (false) and contains an error message.
        /// This should only be useful on replies from the service.
        /// </summary>
        public bool success { get; set; }

        /// <summary>
        /// Gets or sets any error messages
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets the XML serialized version of this instance
        /// </summary>
        [XmlIgnore]
        public string Xml
        {
            get
            {
                XmlSerializer serializer = new XmlSerializer(this.GetType());
                StringWriter sw = new StringWriter();
                serializer.Serialize(sw, this);
                return sw.ToString();
            }
        }
        
        /// <summary>
        /// Deserializes an XML string into an Message
        /// </summary>
        /// <typeparam name="T">The type to deserialize it in to</typeparam>
        /// <param name="xml">The xml message to deserialize</param>
        /// <returns>The deserialized message instance</returns>
        public static T Deserialize<T>(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            StringReader reader = new StringReader(xml);
            return (T)serializer.Deserialize(reader);
        }
    }
}
