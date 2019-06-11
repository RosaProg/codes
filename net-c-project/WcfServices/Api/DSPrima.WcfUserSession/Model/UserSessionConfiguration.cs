using DSPrima.WcfUserSession.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DSPrima.WcfUserSession.Model
{
    /// <summary>
    /// The configuration class for the Wcf User Session used for transfering the settings to the client site
    /// </summary>
    [DataContract]
    public class UserSessionConfiguration
    {
        /// <summary>
        /// The name of the namespace to use for serialization of the Session Data
        /// </summary>
        private const string SessionDataNameSpace = "SessionData";
        
        /// <summary>
        /// The internal data object for storing the DeSerialized Data object
        /// </summary>
        private object data;

        /// <summary>
        /// Contains the security string that has to be transfered back via a cookie for the session to remain alive and the user to be authenticated client side
        /// </summary>
        [DataMember]
        public string SessionId = string.Empty;

        /// <summary>
        /// The user name to display if the user is logged in.
        /// </summary>
        [DataMember]
        public string UserDisplayName = string.Empty;

        /// <summary>
        /// Indicates whether or not multiple steps (other then login) to the verification process are needed.        
        /// </summary>     
        [DataMember]
        public bool MultiStepVerification = false;

        /// <summary>
        /// The timeout in minutes for the session to remain alive
        /// </summary>        
        [DataMember]
        public int Sessiontimeout = 15;

        /// <summary>
        /// Contains the list of Roles the user is a member off
        /// </summary>
        [DataMember]
        public IEnumerable<string> RoleNames = null;

        /// <summary>
        /// Gets or sets the Session XML Data
        /// </summary>
        [DataMember]
        internal string SessionXmlData = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserSessionConfiguration"/> class
        /// </summary>
        /// <param name="securityString">The security string of the current session</param>
        /// <param name="userDisplyName">The display name of the user</param>
        /// <param name="multiStepVerification">An indication if multi-step verification is enabled or not</param>
        /// <param name="sessionTimeout">The session time out in minutes</param>
        /// <param name="roleNames">The list of roles the user is a member off</param>
        /// <param name="sessionData">The object containing any data that may be useful for the session on the client side. This data is NOT passed back to the service.</param>
        public UserSessionConfiguration(string securityString, string userDisplyName, bool multiStepVerification, int sessionTimeout, IEnumerable<string> roleNames, object sessionData)
        {
            this.SessionId = securityString;
            this.UserDisplayName = userDisplyName;
            this.MultiStepVerification = multiStepVerification;
            this.Sessiontimeout = sessionTimeout;
            this.RoleNames = roleNames;

            this.SessionData(sessionData);
        }

        /// <summary>
        /// Sets the session data, converts it to XML
        /// </summary>
        /// <param name="data">The additional data instance to store</param>
        internal void SessionData(object data)
        {
            if (data == null)
            {
                this.SessionXmlData = null;
                return;
            }

            DataContractSerializer ds = new DataContractSerializer(data.GetType(), UserSessionConfiguration.SessionDataNameSpace, string.Empty, null,
                0x7FFF, // maxItemsInObjectGraph
                false,  // ignoreExtensionDataObject
                true,   // preserveObjectReferences
                null);  // dataContractSurrogate

            using (StringWriter sw = new StringWriter())
            {
                XmlWriterSettings xws = new XmlWriterSettings()
                {
                    Indent = true,
                    IndentChars = "  ",
                    OmitXmlDeclaration = true,
                    Encoding = Encoding.UTF8
                };
                using (XmlWriter xw = XmlWriter.Create(sw))
                {
                    ds.WriteObject(xw, data);
                }

                this.SessionXmlData = sw.ToString();
            }
        }

        /// <summary>
        /// Retrieves the Session data and returns it
        /// </summary>
        /// <typeparam name="T">The type of object the SessionData is</typeparam>
        /// <returns>The Session Data</returns>
        public T SessionData<T>()
        {
            if (this.data == null && !string.IsNullOrWhiteSpace(this.SessionXmlData))
            {
                XmlObjectSerializer serializer = new DataContractSerializer(typeof(T), UserSessionConfiguration.SessionDataNameSpace, string.Empty, null,
                0x7FFF, // maxItemsInObjectGraph
                false,  // ignoreExtensionDataObject
                true,   // preserveObjectReferences
                null);  // dataContractSurrogate

                TextReader tr = new StringReader(this.SessionXmlData);
                XmlReader reader = XmlReader.Create(tr);
                this.data = serializer.ReadObject(reader);
            }

            return (T)this.data;
        }
    }
}
