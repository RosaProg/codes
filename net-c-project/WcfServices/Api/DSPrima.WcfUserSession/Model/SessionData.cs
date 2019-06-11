using DSPrima.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DSPrima.WcfUserSession.Model
{
    /// <summary>
    /// Defines the data for a session
    /// </summary>
    public sealed class SessionData
    {
        /// <summary>
        /// Gets or sets the Security String belonging to this Session
        /// </summary>
        public string SecurityString { get; set; }
        
        /// <summary>
        /// Gets or sets the userId of this session
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Session has completed it's authentication process
        /// </summary>
        public bool IsAuthenticated { get; set; }

        /// <summary>
        /// Gets or sets the start time of the session
        /// </summary>
        public DateTime SessionStartTime { get; set; }

        /// <summary>
        /// Gets or sets the last time this sessionw as accessed. This can be used to identify if a session has been expired
        /// </summary>
        public DateTime LastSessionAccessTime { get; set; }

        /// <summary>
        /// Gets or sets the Request header of the last request
        /// </summary>
        public RequestHeader LastRequestHeader { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionData"/> class
        /// </summary>
        public SessionData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionData"/> class
        /// </summary>
        /// <param name="securityString">The security string for this session</param>
        /// <param name="userId">The user Id this session belong to</param>
        /// <param name="isAuthenticated">Whether or not this session has completed it's authentication process</param>
        public SessionData(string securityString, string userId, bool isAuthenticated)
        {
            this.SecurityString = securityString;
            this.UserId = userId;
            this.IsAuthenticated = isAuthenticated;
            this.SessionStartTime = DateTime.Now;
            this.LastSessionAccessTime = DateTime.Now;
        }

        /// <summary>
        /// Encryps the SessionData into a single string for safe storage
        /// </summary>
        /// <param name="data">The SessionData instance to encrypt</param>
        /// <returns>An encrypted representation of the SessionData</returns>
        public static string Encrypt(SessionData data)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(data.GetType());
            StringWriter textWriter = new StringWriter();
            xmlSerializer.Serialize(textWriter, data);
            textWriter.Flush();
            return MachineKeyEncryption.Encrypt(textWriter.ToString());
        }

        /// <summary>
        /// Decrypts the given string into a SessionData object
        /// </summary>
        /// <param name="encryptedString">The encrypted string to decrypt</param>
        /// <returns>The SessionData instance</returns>
        public static SessionData Decrypt(string encryptedString)
        {
            string decrypted = MachineKeyEncryption.Decrypt(encryptedString);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(SessionData));
            StringReader reader = new StringReader(decrypted);
            return xmlSerializer.Deserialize(reader) as SessionData;
        }
    }
}
