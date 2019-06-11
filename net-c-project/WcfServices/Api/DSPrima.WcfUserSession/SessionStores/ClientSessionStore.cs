using DSPrima.Security;
using DSPrima.WcfUserSession.Interfaces;
using DSPrima.WcfUserSession.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DSPrima.WcfUserSession.SessionStores
{
    /// <summary>
    /// Stores the Session Data in RAM Memory but saves it to a hard disk upon disposal
    /// </summary>
    public class ClientSessionStore : IClientSessionStore, IDisposable
    {
        #region static variables
        /// <summary>
        /// The name of the file to save to
        /// </summary>
        private const string FileName = "SessionDetails.dat";

        /// <summary>
        /// Holds the minimum time between saves
        /// </summary>
        private const int MinimumSecondsBetweenSaves = 10;

        /// <summary>
        /// The namespace for saving serializing the SessionDetails store
        /// </summary>
        private const string SessionNameSpace = "ClientSessionDetails";

        /// <summary>
        /// Defines the amount of characters the SessionId substring will be for the Secured Data storage
        /// </summary>
        private const int SessionIdSubstringLength = 10;

        /// <summary>
        /// Holds the path to the persistant Store Location
        /// </summary>
        private string persistantStoreLocation = Path.Combine(Path.GetTempPath(), ClientSessionStore.FileName);

        /// <summary>
        /// Indicates whether the SessionDetails are already being saved to disk.
        /// </summary>
        private static object savingLock = new object();

        /// <summary>
        /// Holds the date and time the SessionDetails were last saved.
        /// </summary>
        private static DateTime lastSaveTime = DateTime.Now.AddSeconds(-MinimumSecondsBetweenSaves);
        #endregion

        #region Instance variables

        /// <summary>
        /// Holds the private session details instance in memory for quick access
        /// </summary>
        private ConcurrentDictionary<string, ClientSessionData> sessionDetails;

        /// <summary>
        /// Gets the Session Details. If the session details don't exist yet, they are loaded from the Persistent File.
        /// </summary>
        private ConcurrentDictionary<string, ClientSessionData> SessionDetails
        {
            get
            {
                if (this.sessionDetails == null)
                {
                    Task readThread = new TaskFactory().StartNew(this.LoadSessionDetails);
                    readThread.Wait();
                }

                return this.sessionDetails;
            }
        }

        /// <summary>
        /// Holds the secured data.
        /// If the sessionDetails do not hold the session data, it may be found here in encrypted form.
        /// </summary>
        private Dictionary<string, List<SecuredClientsessionData>> securedData = new Dictionary<string, List<SecuredClientsessionData>>();

        #endregion

        #region Initializers
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientSessionStore"/> class
        /// </summary>
        public ClientSessionStore()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientSessionStore"/> class
        /// </summary>
        /// <param name="storageLocation">The directory to store the session data. If the path doesn't exist or is null, the Temp folder is users</param>
        /// <param name="fileName">The filename to store the session data. If null or whitespace "SessionDetails.dat" is used instead</param>
        public ClientSessionStore(string storageLocation, string fileName)
        {
            string directory = Path.GetTempPath();
            if (Directory.Exists(storageLocation))
            {
                directory = storageLocation;
            }

            string file = ClientSessionStore.FileName;
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                file = FileName;
            }

            this.persistantStoreLocation = Path.Combine(directory, file);
        }

        #endregion

        #region Interface Functionality

        /// <summary>
        /// Stores a session and the ClientSessionData
        /// </summary>
        /// <param name="sessionId">The session Id to store it under</param>
        /// <param name="data">The session Data to store</param>
        public void StoreSession(string sessionId, Model.ClientSessionData data)
        {
            this.SessionDetails.AddOrUpdate(sessionId, data, (k, v) => { return data; });
            new TaskFactory().StartNew(this.SaveSessionDetails);
        }

        /// <summary>
        /// Retrieves the Client Session Data for the given session ID
        /// </summary>
        /// <param name="sessionId">The Id of the session to retrieve the data for</param>
        /// <returns>The Client Session Data</returns>
        public Model.ClientSessionData GetSessionData(string sessionId)
        {
            if (this.SessionDetails.ContainsKey(sessionId))
            {
                return this.sessionDetails[sessionId];
            }
            else if (this.securedData.ContainsKey(sessionId.Substring(0, ClientSessionStore.SessionIdSubstringLength)))
            {
                List<SecuredClientsessionData> d = this.securedData[sessionId.Substring(0, ClientSessionStore.SessionIdSubstringLength)];
                foreach (SecuredClientsessionData scsd in d)
                {
                    try
                    {
                        // SecuredClientsessionData scsd = this.securedData[sessionId.Substring(0, ClientSessionStore.SessionIdSubstringLength)];
                        string data = scsd.SecuredData;
                        UserSessionConfiguration config = this.Deserialize<UserSessionConfiguration>(MachineKeyEncryption.Decrypt(data, sessionId));
                        ClientSessionData csd = new ClientSessionData() { LastTimeUpdated = scsd.LastUpdated, UserSessionConfiguration = config };

                        this.SessionDetails.AddOrUpdate(sessionId, csd, (k, v) => { return csd; });
                        d.Remove(scsd);
                        return this.sessionDetails[sessionId];
                    }
                    catch (Exception)
                    {
                        // Obviously it wasn't this one
                    }
                }
            }

            return null;
        }
        #endregion

        #region Saving
        /// <summary>
        /// Saves all the session details to the hard disk.
        /// </summary>
        private void SaveSessionDetails()
        {
            if (this.sessionDetails == null) return;
#if !DEBUG
            if ((DateTime.Now - ClientSessionStore.lastSaveTime).TotalSeconds < ClientSessionStore.MinimumSecondsBetweenSaves) return;
#endif
            lock (savingLock)
            {
                ClientSessionStore.lastSaveTime = DateTime.Now;
                Dictionary<string, List<SecuredClientsessionData>> dataTostore = new Dictionary<string, List<SecuredClientsessionData>>();

                // Do some cleanup of the data that is no longer valid.
                DateTime now = DateTime.Now;
                List<string> keysToremove = new List<string>();
                foreach (var data in this.sessionDetails)
                {
                    if (data.Value.LastTimeUpdated.AddMinutes(data.Value.UserSessionConfiguration.Sessiontimeout) < now)
                    {
                        keysToremove.Add(data.Key);
                    }
                    else
                    {
                        string key = data.Key.Substring(0, ClientSessionStore.SessionIdSubstringLength);
                        SecuredClientsessionData sd = new SecuredClientsessionData();
                        sd.SecuredData = MachineKeyEncryption.Encrypt(this.Serialize(data.Value.UserSessionConfiguration), data.Key);
                        sd.LastUpdated = data.Value.LastTimeUpdated;
                        sd.SessionTimeoutInMinutes = data.Value.UserSessionConfiguration.Sessiontimeout;
                        if (!dataTostore.ContainsKey(data.Key)) dataTostore.Add(key, new List<SecuredClientsessionData>());
                        dataTostore[key].Add(sd);
                    }
                }

                foreach (string key in keysToremove)
                {
                    ClientSessionData tmp;
                    this.sessionDetails.TryRemove(key, out tmp);
                }

                keysToremove = new List<string>();
                foreach (var data in this.securedData)
                {
                    foreach (var value in data.Value)
                    {
                        if (value.LastUpdated.AddMinutes(value.SessionTimeoutInMinutes) >= now)
                        {
                            if (!dataTostore.ContainsKey(data.Key))
                            {
                                dataTostore.Add(data.Key, new List<SecuredClientsessionData>());
                            }

                            dataTostore[data.Key].Add(value);
                        }
                        else
                        {
                            keysToremove.Add(data.Key);
                        }
                    }
                }

                foreach (string key in keysToremove)
                {
                    this.securedData.Remove(key);
                }

                FileStream fs = new FileStream(this.persistantStoreLocation, FileMode.Create);
                string xmlData = MachineKeyEncryption.Encrypt(this.Serialize(dataTostore));

                // Construct a BinaryFormatter and use it to write the data to the stream.
                BinaryWriter writer = new BinaryWriter(fs);
                try
                {
                    writer.Write(xmlData);
                }
                catch (SerializationException e)
                {
                    Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                    throw;
                }
                finally
                {
                    fs.Close();
                }
            }
        }

        /// <summary>
        /// Serializes the given object to a string
        /// </summary>
        /// <param name="objectToSerialize">The object to serialize</param>
        /// <returns>The serialized object</returns>
        private string Serialize(object objectToSerialize)
        {
            // Time to save.
            string xmlData = null;
            DataContractSerializer ds = new DataContractSerializer(objectToSerialize.GetType(), ClientSessionStore.SessionNameSpace, string.Empty, null,
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
                    ds.WriteObject(xw, objectToSerialize);
                }

                xmlData = sw.ToString();
            }

            return xmlData;
        }
        #endregion

        #region Loading
        /// <summary>
        /// Loads all the session details from the persistant store file and fills the <see cref="WcfUserClient.sessionDetails"/>
        /// </summary>
        private void LoadSessionDetails()
        {
            lock (ClientSessionStore.savingLock)
            {
                if (this.sessionDetails != null) return;

                this.sessionDetails = new ConcurrentDictionary<string, ClientSessionData>();

                // Open the file containing the data that you want to deserialize.
                FileInfo file = new FileInfo(this.persistantStoreLocation);
                if (!file.Exists || file.Length == 0)
                {
                    this.securedData = new Dictionary<string, List<SecuredClientsessionData>>();
                    return;
                }

                FileStream fs = file.OpenRead(); // new FileStream(WcfUserClientSession.PersistantStoreLocation, FileMode.Open);
                try
                {
                    BinaryReader binReader = new BinaryReader(fs);
                    string xmlData = MachineKeyEncryption.Decrypt(binReader.ReadString());

                    this.securedData = this.Deserialize<Dictionary<string, List<SecuredClientsessionData>>>(xmlData);
                }
                catch (SerializationException e)
                {
                    Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                }
                finally
                {
                    fs.Close();
                }
            }
        }

        /// <summary>
        /// Deserializes the given string to the passed object type
        /// </summary>
        /// <typeparam name="T">The class to deserialize to</typeparam>
        /// <param name="stringToDeserialize">The string to deserialize</param>
        /// <returns>The deserialized object</returns>
        private T Deserialize<T>(string stringToDeserialize) where T : class
        {
            DataContractSerializer ds = new DataContractSerializer(typeof(T), ClientSessionStore.SessionNameSpace, string.Empty, null,
            0x7FFF, // maxItemsInObjectGraph
            false,  // ignoreExtensionDataObject
            true,   // preserveObjectReferences
            null);  // dataContractSurrogate

            TextReader tr = new StringReader(stringToDeserialize);
            XmlReader reader = XmlReader.Create(tr);

            return (T)ds.ReadObject(reader);
        }

        #endregion

        /// <summary>
        /// Disposes of the application. Saves the data one last time to the file
        /// </summary>
        public void Dispose()
        {
            ClientSessionStore.lastSaveTime = DateTime.Now.AddSeconds(-ClientSessionStore.MinimumSecondsBetweenSaves);
            this.SaveSessionDetails();
        }
    }
}
