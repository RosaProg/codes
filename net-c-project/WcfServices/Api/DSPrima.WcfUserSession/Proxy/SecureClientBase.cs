using DSPrima.WcfUserSession.Behaviours;
using DSPrima.WcfUserSession.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Web;

namespace DSPrima.WcfUserSession.Proxy
{
    /// <summary>
    /// Provides a ClientBase parent class that can be used to automatically handle the WCF Session communication
    /// When used from within a website the HTTPContext.Current.User instance is automatically populated and an authentication cookie is created.
    /// This is done using <see cref="DSPrima.WcfUserSession.Behaviours.WcfUserSessionBehaviour"/>
    /// </summary>
    /// <typeparam name="TChannel">The channel type to use</typeparam>
    public class SecureClientBase<TChannel> : ClientBase<TChannel>, IDisposable where TChannel : class
    {
        /// <summary>
        /// The name of the Cookie that holds all the session data
        /// </summary>
        private const string CookieName = "Set-Cookie";

        /// <summary>
        /// The OperationContextScope to use to read the headers
        /// </summary>
        protected OperationContextScope scope;

        // TODO Remove
        /*
        /// <summary>
        /// Holds the SecurityCookie value that has been set by the user Application.
        /// </summary>
        private string securityString = null;
        */

        /// <summary>
        /// Gets the headers belonging to the last call
        /// </summary>
        public UserSessionConfiguration GetConfiguration
        {
            get
            {
                return OperationContext.Current.IncomingMessageHeaders.GetHeader<DSPrima.WcfUserSession.Model.UserSessionConfiguration>("SecurityConfig", "s");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecureClientBase&lt;TChannel&gt;" /> class
        /// </summary>
        public SecureClientBase()
        {
            this.ChannelFactory.Endpoint.Behaviors.Add(new DSPrima.WcfUserSession.Behaviours.WcfUserSessionBehaviour());
        }

        // TODO remove
        /*
        /// <summary>
        /// Gets or sets the security string cookie.
        /// Before making the first call after creating an Instance of this clas this HAS to be set on the outgoing context if User security is required
        /// On subsequent calls of the same instance this is automatically filled in.
        /// If FormAuthentication is being used and the WcfUserSessionBehaviour is active (which it is when using this class as a base class) this is automatically filled in and thus doesn't need to be set.
        /// </summary>
        public string SecurityCookie
        {
            get
            {
                return this.GetConfiguration.SessionId;
            }

            set
            {
                this.securityString = value;

                if (OperationContext.Current != null)
                {
                    HttpRequestMessageProperty requestProperty = new HttpRequestMessageProperty();
                    requestProperty.Headers[HttpRequestHeader.Cookie] = CookieName + "=" + value;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestProperty;
                }
            }
        }
         */

        /// <summary>
        /// Gets or sets the private instance of the request header
        /// </summary>
        private RequestHeader requestHeader;

        /// <summary>
        /// Gets or sets the Request Header. When set, the Header is automatically added to the Message as soon as possible. 
        /// </summary>
        public RequestHeader RequestHeader
        {
            get
            {
                return this.requestHeader;
            }

            set
            {
                this.requestHeader = value;
                if (OperationContext.Current != null)
                {
                    var typedHeader = new MessageHeader<RequestHeader>(this.requestHeader);
                    var untypedHeader = typedHeader.GetUntypedHeader(WcfUserSessionBehaviour.HeaderName, WcfUserSessionBehaviour.HeaderNamespace);
                    OperationContext.Current.OutgoingMessageHeaders.RemoveAll(WcfUserSessionBehaviour.HeaderName, WcfUserSessionBehaviour.HeaderNamespace);
                    OperationContext.Current.OutgoingMessageHeaders.Add(untypedHeader);
                }
            }
        }

        /// <summary>
        /// Gets the Display name for the currently logged in user
        /// </summary>
        public string DisplayName
        {
            get
            {
                return this.GetConfiguration.UserDisplayName;
            }
        }

        /// <summary>
        /// Gets the list of Roles the user contains
        /// </summary>
        public string[] Roles
        {
            get
            {
                return this.GetConfiguration.RoleNames != null ? this.GetConfiguration.RoleNames.ToArray() : new string[0];
            }
        }

        /// <summary>
        /// Returns a new channel to the service.
        /// </summary>
        /// <returns>A channel of the type of the service contract.</returns>
        protected override TChannel CreateChannel()
        {
            TChannel proxy = base.CreateChannel();
            this.scope = new OperationContextScope((IContextChannel)proxy);

            // TODO remove
            /*
            if (!string.IsNullOrWhiteSpace(this.securityString))
            {
                this.SecurityCookie = this.securityString;
            }*/

            if (this.requestHeader != null)
            {
                this.RequestHeader = this.requestHeader;
            }

            return proxy;
        }

        /// <summary>
        /// Disposes of the internal data
        /// </summary>
        public void Dispose()
        {
            this.scope.Dispose();
        }
    }
}