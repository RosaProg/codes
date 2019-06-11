using DSPrima.WcfUserSession.Behaviours;
using DSPrima.WcfUserSession.Interfaces;
using DSPrima.WcfUserSession.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace DSPrima.WcfUserSession.ClientSession
{
    /// <summary>
    /// When data is returned from a Wcf Service with the <see cref="WcfUserSessionSecurity"/> attribute assigned, and the client is having the <see cref="WcfUserSessionBehaviour"/> behaviour assigned to it's endpoint. 
    /// The session data can be found in the <see cref="WcfUserClientSession.Current"/>
    /// This data is persistent in the Temp Directory. If the Temp directory is not available, <see cref="WcfUserClientSession.PersistantStorePath"/> must be set to a proper location with read and write access.
    /// Saves are made at most every 10 seconds.
    /// </summary>
    public class WcfUserClientSession
    {
        /// <summary>
        /// The name with which to store the WcfUserSessionSecurity instance int he Call Context
        /// </summary>
        private const string CallContextName = "DSPrima.WcfUserClientSession";

        #region Static Functionality

        /// <summary>
        /// Holds the reference to the session store
        /// This has to be set on startup of the application or it crashes.
        /// </summary>
        public static IClientSessionStore SessionStore;

        /// <summary>
        /// Gets or sets the current instance of the WcfUserSessionSecurity Context
        /// </summary>
        public static WcfUserClientSession Current
        {
            get
            {
                object data = null;
                if (HttpContext.Current != null)
                {
                    data = HttpContext.Current.Items[WcfUserClientSession.CallContextName];
                }

                if (data == null)
                {
                    data = CallContext.LogicalGetData(WcfUserClientSession.CallContextName);
                }

                if (data != null) return data as WcfUserClientSession;

                return new WcfUserClientSession(null);
            }

            set
            {
                CallContext.LogicalSetData(WcfUserClientSession.CallContextName, value);

                // Setting the HttpContext items as well if available, this is due to the fact that the CallContext may be set on a different thread in Websites and thus data may not be available.
                if (HttpContext.Current != null)
                {
                    if (!HttpContext.Current.Items.Contains(WcfUserClientSession.CallContextName)) HttpContext.Current.Items.Add(WcfUserClientSession.CallContextName, value);
                    else HttpContext.Current.Items[WcfUserClientSession.CallContextName] = value;
                }
            }
        }

        /// <summary>
        /// Sets the client session to be a session with the given User Session Configuration
        /// </summary>
        /// <param name="config">The user session configuration for the current client session</param>
        public static void SetClientSession(UserSessionConfiguration config)
        {
            if (config == null || string.IsNullOrWhiteSpace(config.SessionId)) return;

            ClientSessionData data = new ClientSessionData() { LastTimeUpdated = DateTime.Now, UserSessionConfiguration = config };
            WcfUserClientSession.SessionStore.StoreSession(config.SessionId, data);
            WcfUserClientSession.Current = new WcfUserClientSession(config);
        }

        /// <summary>
        /// Loads a session's data based upon the existing session ID passed back from the user in a cookie.
        /// This is version ONLY works for Websites where the HttpContext.Current is available. For applications that do not have the HttpContext.Current available, use <see cref="M:LoadSession(string sessionId)"/> instead
        /// </summary>
        public static void LoadSession()
        {
            if (HttpContext.Current != null && HttpContext.Current.Request.Cookies[WcfUserSessionBehaviour.SecurityStringCookieName] != null)
            {
                string sessionId = HttpContext.Current.Request.Cookies[WcfUserSessionBehaviour.SecurityStringCookieName].Value;
                WcfUserClientSession.LoadSession(sessionId);
            }
        }

        /// <summary>
        /// Loads a session's data based upon the given Session ID.
        /// Websites and applications with the HttPContext.Current available can use <see cref="M:LoadSession()"/> instead
        /// </summary>
        /// <param name="sessionId">The Id of the session to load</param>
        public static void LoadSession(string sessionId)
        {
            ClientSessionData data = null;
            if ((data = WcfUserClientSession.SessionStore.GetSessionData(sessionId)) != null)
            {
                WcfUserClientSession.SetClientSession(data.UserSessionConfiguration);
            }
        }

        #endregion

        #region Instance Methods
        /// <summary>
        /// Holds the internal Configuration
        /// </summary>
        protected UserSessionConfiguration config;

        /// <summary>
        /// Gets the User Session Configuration
        /// </summary>
        public UserSessionConfiguration Config { get { return this.config; } }

        /// <summary>
        /// Gets or sets the IP of the user that is connecting. If the client is a Website or the user is on the local machine, this does not have to be set
        /// </summary>
        public string UserIp { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WcfUserClientSession"/> class
        /// </summary>
        /// <param name="config">The configuration to set</param>
        public WcfUserClientSession(UserSessionConfiguration config)
        {
            this.config = config;
        }

        #endregion
    }
}
