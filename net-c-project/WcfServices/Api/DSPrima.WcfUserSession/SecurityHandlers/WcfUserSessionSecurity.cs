using DSPrima.Security;
using DSPrima.WcfUserSession.Eventhandlers;
using DSPrima.WcfUserSession.Interfaces;
using DSPrima.WcfUserSession.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using System.Xml;
using System.Xml.Serialization;

namespace DSPrima.WcfUserSession.SecurityHandlers
{
    /// <summary>
    /// Handles the verification of a User session via a WCF connection
    /// </summary>
    public class WcfUserSessionSecurity
    {
        /// <summary>
        /// The name with which to store the WcfUserSessionSecurity instance int he Call Context
        /// </summary>
        private const string CallContextName = "DSPrima.WcfUserSessionSecurity";

        #region Settings

        /// <summary>
        /// Indicates whether or not multiple steps (other then login) to the verification process are needed.        
        /// </summary>     
        private static bool multiStepVerification = false;

        /// <summary>
        /// Gets or sets a value indicating whether or not multiple steps (other then login) to the verification process are needed.
        /// Default is false
        /// </summary>        
        public static bool MultiStepVerification
        {
            get
            {
                return WcfUserSessionSecurity.multiStepVerification;
            }

            set
            {
                WcfUserSessionSecurity.multiStepVerification = value;
            }
        }

        /// <summary>
        /// The timeout in minutes for the session to remain alive
        /// </summary>        
        private static int sessiontimeout = 15;

        /// <summary>
        /// Gets or sets the timeout in minutes for the session to remain alive
        /// </summary>        
        public static int SessionTimeout
        {
            get
            {
                return WcfUserSessionSecurity.sessiontimeout;
            }

            set
            {
                WcfUserSessionSecurity.sessiontimeout = value;
            }
        }

        /// <summary>
        /// Gets or sets the IUserSessionStore instance to user for storing sessions
        /// </summary>
        public static IUserSessionStore SessionStore { get; set; }

        /// <summary>
        /// Gets or sets the IUserManager instance to use for checking if a User exists
        /// </summary>
        public static IUserManager UserManager { get; set; }

        #endregion

        #region Static Functionality

        /// <summary>
        /// This event is executed each time the session is Updated and/or Created.
        /// Passed the Session ID and the User for that session to is
        /// </summary>
        public static event SessionUpdateEventHandler SessionUpdated;

        /// <summary>
        /// Gets or sets the current instance of the WcfUserSessionSecurity Context
        /// </summary>
        public static WcfUserSessionSecurity Current
        {
            get
            {
                object data = CallContext.LogicalGetData(WcfUserSessionSecurity.CallContextName);
                if (data != null) return data as WcfUserSessionSecurity;
                return new WcfUserSessionSecurity();
            }

            set
            {
                CallContext.LogicalSetData(WcfUserSessionSecurity.CallContextName, value);
                if (value != null && WcfUserSessionSecurity.SessionUpdated != null) WcfUserSessionSecurity.SessionUpdated(value.SessionId, value.User, value.RequestHeader);
            }
        }

        /// <summary>
        /// Attempts to login a user with the given username and password.
        /// If successfull a new instance of the WcfUserSessionSecurity class is created with the given User data and the Security String is set
        /// This instance can be retrieved using WcfUserSessionSecurity.Current
        /// If however MultiStepVerification has been set to True on either the service or the User itself, the user is not yet authenticated until <see cref="M:MultiStepVerificationCompleted"/> has been called and the User instance is therefore not accessible yet
        /// </summary>
        /// <param name="username">The username to login with</param>
        /// <param name="password">The password to use</param>
        /// <returns>True if successful, false otherwise</returns>
        public static LoginResult Login(string username, string password)
        {
            IUser user = null;
            LoginResult result = WcfUserSessionSecurity.UserManager.Find(username, password, ref user);
            WcfUserSessionSecurity.Current.RequestHeader.SessionId = null;

            if (result == LoginResult.Success)
            {
                WcfUserSessionSecurity sec = WcfUserSessionSecurity.Current.User != null && WcfUserSessionSecurity.Current.User.UserName.Equals(username, StringComparison.CurrentCultureIgnoreCase) ? WcfUserSessionSecurity.Current : new WcfUserSessionSecurity(user, WcfUserSessionSecurity.Current.RequestHeader);
                sec.InternalUser = user;
                WcfUserSessionSecurity.Current = sec;
                WcfUserSessionSecurity.SessionStore.StoreSession(sec.SessionId, sec.SessionData);
            }

            return result;
        }

        /// <summary>
        /// Verifies if the given SecurityString inside the header is correct and if the user this string belongs to hasn't timed out yet.        
        /// </summary>
        /// <param name="header">The header of the request which holds teh SessionId to verify</param>
        public static void VerifySession(RequestHeader header)
        {
            if (WcfUserSessionSecurity.SessionStore.ValidateSession(header.SessionId))
            {
                WcfUserSessionSecurity sec = new WcfUserSessionSecurity(header);
                WcfUserSessionSecurity.Current = sec;
            }
            else
            {
                WcfUserSessionSecurity.Current = new WcfUserSessionSecurity(header);
            }
        }
        #endregion

        /// <summary>
        /// Gets or sets the User that is currently logged in
        /// </summary>
        private IUser InternalUser { get; set; }

        /// <summary>
        /// Gets the User that is currently logged in and authenticated
        /// If <see cref="WcfSecurity.MultiStepVerification"/> is true, the <see cref="M:MultiStepVerificationCompleted"/> has to be called before this is populated
        /// </summary>
        public IUser User { get { return this.SessionData == null || !this.SessionData.IsAuthenticated ? null : this.InternalUser; } }

        /// <summary>
        /// The security string of this user
        /// </summary>
        private string sessionId = string.Empty;

        /// <summary>
        /// Gets the security string of this user. This string is used to verify if the user actually exists and if session hasn't expired yet.
        /// </summary>
        public string SessionId { get { return this.sessionId; } }

        /// <summary>
        /// Gets or sets the session data for this session
        /// </summary>
        protected SessionData SessionData { get; set; }

        /// <summary>
        /// Gets the Request header for the last request by the client
        /// This header includes additional information regarding the origins of the request
        /// </summary>
        public RequestHeader RequestHeader { get { return this.SessionData.LastRequestHeader; } }

        /// <summary>
        /// Gets a value indicating whether the user or the service requires two step authentication
        /// </summary>
        public bool MultiStepVerificationEnabled { get { return (this.InternalUser != null && this.InternalUser.MultiStepVerificationEnabled) || WcfUserSessionSecurity.MultiStepVerification; } }

        /// <summary>
        /// Prevents a default instance of the <see cref="WcfUserSessionSecurity"/> class from being created
        /// The user and securitystring are not populated
        /// </summary>
        private WcfUserSessionSecurity()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WcfUserSessionSecurity"/> class
        /// This creates a new session for the user
        /// </summary>
        /// <param name="user">The IUser instance to user</param>
        /// <param name="header">The Request header</param>
        internal protected WcfUserSessionSecurity(IUser user, RequestHeader header)
            : this(header)
        {
            if (user != null)
            {
                this.InternalUser = user;
                this.sessionId = MachineKeyEncryption.Encrypt(this.InternalUser.Id);
                this.SessionData.SecurityString = header.SessionId;
                this.SessionData.UserId = this.InternalUser.Id;
                this.SessionData.IsAuthenticated = this.MultiStepVerificationEnabled ? false : true;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WcfUserSessionSecurity"/> class
        /// Continues with a existing session by the user (Or no session if the Session Id if empty)
        /// </summary>
        /// <param name="header">The Request header belonging to this session</param>
        internal protected WcfUserSessionSecurity(RequestHeader header)
        {
            this.sessionId = header.SessionId;
            string userId = MachineKeyEncryption.Decrypt(this.sessionId);
            try
            {                
                this.InternalUser = WcfUserSessionSecurity.UserManager.Find(userId/*ticket.UserData*/);
                this.SessionData = WcfUserSessionSecurity.SessionStore.GetSessionData(this.sessionId);
                this.SessionData.LastRequestHeader = header;

                WcfUserSessionSecurity.SessionStore.UpdateSessionData(this.sessionId, this.SessionData);
            }
            catch (Exception)
            {
                this.InternalUser = null;
                this.SessionData = new SessionData();
                this.SessionData.LastRequestHeader = header;
            }
        }

        /// <summary>
        /// Indicates that the verification sequence has been completed.
        /// This call is only needed if the MultiStepVerification is set to True
        /// If the user name doesn't match the name of the user for this session the authentication will fail silently and therefor not complete.
        /// </summary>
        /// <param name="userName">The name of the user to autorhize</param>
        public void MultiStepVerificationCompleted(string userName)
        {
            if (this.InternalUser.UserName.Equals(userName, StringComparison.CurrentCultureIgnoreCase))
            {
                this.SessionData.IsAuthenticated = true;
                WcfUserSessionSecurity.SessionStore.UpdateSessionData(this.sessionId, this.SessionData);
            }
        }

        /// <summary>
        /// Verifies if the given userName or ID matches the name and/or ID in the current session
        /// This is useful for when Session Authentication hasn't completed yet but you want to verify if the userName or Id you have is valid for the current session
        /// If both are specified (not null, not empty and not whitespace), both must match for the result to be true.
        /// </summary>
        /// <param name="userId">The Id of the user</param>
        /// <param name="userName">The username of the user</param>
        /// <returns>True if the name or Id matches. False if the name doesn't match or there is no user for the current session</returns>
        public bool VerifyNameOrIdWithSession(string userId = null, string userName = null)
        {
            if (this.InternalUser == null) return false;

            bool match1 = true;
            if (!string.IsNullOrWhiteSpace(userName))
            {
                match1 = this.InternalUser.UserName.Equals(userName, StringComparison.CurrentCultureIgnoreCase);
            }

            bool match2 = true;
            if (!string.IsNullOrWhiteSpace(userId))
            {
                match2 = this.InternalUser.Id.Equals(userId, StringComparison.CurrentCulture);
            }

            return match1 == true && match2 == true;
        }

        /// <summary>
        /// Logs out the current session
        /// </summary>
        /// <param name="cleanInternalSessionVariables">If true, the internal session varaibles (User, SessionData) are set to null. If false they are not. The session is always cleared for the next call to the service.</param>
        public void Logout(bool cleanInternalSessionVariables = true)
        {
            WcfUserSessionSecurity.SessionStore.RemoveSession(this.sessionId);
            this.sessionId = null;
            if (cleanInternalSessionVariables)
            {
                this.InternalUser = null;
                this.SessionData = null;
            }
        }

        /// <summary>
        /// Updates the current User to have the given name to display
        /// </summary>
        /// <param name="displayName">The name to display</param>
        public void SetDisplayName(string displayName)
        {
            this.InternalUser.DisplayName = displayName;
        }

        /// <summary>
        /// Adds a role to the internal User
        /// </summary>
        /// <param name="role">The role to add to the list of roles already assigned to the User for this session</param>
        public void AddRole(string role)
        {
            if (!this.InternalUser.RoleNames.Contains(role))
            {
                List<string> roles = this.InternalUser.RoleNames.ToList();
                roles.Add(role);
                this.InternalUser.RoleNames = roles;
            }
        }

        /// <summary>
        /// Replaces the current list of Roles with the list of roles passed on
        /// </summary>
        /// <param name="roles">The list of roles</param>
        public void SetRoles(IEnumerable<string> roles)
        {
            this.InternalUser.RoleNames = roles;
        }

        /// <summary>
        /// Gets or sets the Session Information data
        /// </summary>
        private object SessionInfoData { get; set; }

        /// <summary>
        /// Adds session information data to pass on to the client side
        /// This data is serialized to XML usering an XmlObjectSeriliazer and preserves object References.
        /// </summary>
        /// <param name="data">The data to store and pass on</param>
        public void AddSessionInformation(object data)
        {
            this.SessionInfoData = data;
        }

        /// <summary>
        /// Gets the Session Configuration Data
        /// </summary>
        /// <returns>The session configuration data</returns>
        internal UserSessionConfiguration GetSessionConfig()
        {
            string userDisplayName = string.Empty;
            IEnumerable<string> roleNames = null;
            if (this.User != null)
            {
                userDisplayName = WcfUserSessionSecurity.Current.User.DisplayName;
                if (string.IsNullOrWhiteSpace(userDisplayName)) userDisplayName = WcfUserSessionSecurity.Current.User.UserName;
                roleNames = WcfUserSessionSecurity.Current.User.RoleNames;
            }

            return new UserSessionConfiguration(WcfUserSessionSecurity.Current.SessionId, userDisplayName, WcfUserSessionSecurity.MultiStepVerification, WcfUserSessionSecurity.SessionTimeout, roleNames, this.SessionInfoData);
        }
    }
}