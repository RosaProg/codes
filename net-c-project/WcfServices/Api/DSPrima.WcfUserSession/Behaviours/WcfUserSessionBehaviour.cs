using DSPrima.WcfUserSession.ClientSession;
using DSPrima.WcfUserSession.Model;
using DSPrima.WcfUserSession.SecurityHandlers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Web;
using System.Web.Security;

namespace DSPrima.WcfUserSession.Behaviours
{
    /// <summary>
    /// Defines the behaviour of a Wcf User session 
    /// On the Service side, 
    /// On receiving a request, the class looks for the security string cookie in a cookie called "Set-Cookie". If found, it automatically calls the WcfUserSessionSecurity to verify the cookie and create a current instance.
    /// On sending a request it will automatically  add the s<see cref="UserSessionConfiguration"/> data to the header with the HeaderName "SecurityConfig"
    /// 
    /// On the client side
    /// On receiving a response, if the HttpContext.Current instance is available, it will automatically add the security string cookie and create a user session in it upon receiving of the data.
    /// On sending a request the If the HttpContext.Current instance is available OR if the OperationContext.Current (i.e. a connection is being reused) exists and either one of them contains the security string it will add it as a cookie on the request to the service if it doesn't already exists.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class WcfUserSessionBehaviour : Attribute, IDispatchMessageInspector,
        IClientMessageInspector, IEndpointBehavior, IServiceBehavior
    {
        /// <summary>
        /// Defines the name of the incomming cookie (is always "Set-Cookie" when using .Net)
        /// </summary>
        private const string CookieName = "Set-Cookie";

        /// <summary>
        /// Defines the name of the security string cookie
        /// </summary>
        public const string SecurityStringCookieName = "DSPrima.WCFCookie";

        /// <summary>
        /// Defines the name in which the header is being held
        /// </summary>
        public const string HeaderName = "SecurityConfig";

        /// <summary>
        /// Defines the namespace in which the header is being held
        /// </summary>
        public const string HeaderNamespace = "s";

        /// <summary>
        /// Defines the domain to use for searching the cookie
        /// </summary>
        private const string CookieDomain = "http://www.cookiedomain.com.au";

        #region IDispatchMessageInspector

        /// <summary>
        /// Gets the cookie from the request and passes it on to the WcfUserSessionSecurity class to use
        /// </summary>
        /// <param name="request">The request message</param>
        /// <param name="channel">The channel being used</param>
        /// <param name="instanceContext">The instance context to use</param>
        /// <returns>Null as there is nothing to return</returns>
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            // Retrieve Cookie from Request and set user in current session            
            /*HttpRequestMessageProperty prop = (HttpRequestMessageProperty)OperationContext.Current.IncomingMessageProperties[HttpRequestMessageProperty.Name];
            if (prop != null && prop.Headers[HttpRequestHeader.Cookie] != null)
            {
                CookieContainer cookieContainer = new CookieContainer();
                cookieContainer.SetCookies(new Uri(WcfUserSessionBehaviour.CookieDomain), prop.Headers[HttpRequestHeader.Cookie]);

                if (cookieContainer.GetCookies(new Uri(WcfUserSessionBehaviour.CookieDomain))[WcfUserSessionBehaviour.CookieName] != null)
                    WcfUserSessionSecurity.VerifySecurityString(cookieContainer.GetCookies(new Uri(WcfUserSessionBehaviour.CookieDomain))[WcfUserSessionBehaviour.CookieName].Value);
            }*/

            if (request.Headers.FindHeader(WcfUserSessionBehaviour.HeaderName, WcfUserSessionBehaviour.HeaderNamespace) > -1)
            {
                var header = request.Headers.GetHeader<RequestHeader>(WcfUserSessionBehaviour.HeaderName, WcfUserSessionBehaviour.HeaderNamespace);
                if (header != null)
                {
                    OperationContext context = OperationContext.Current;
                    MessageProperties prop = context.IncomingMessageProperties;
                    RemoteEndpointMessageProperty endpoint =
                        prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
                    header.ClientIp = endpoint.Address;
                    WcfUserSessionSecurity.VerifySession(header); // header.SessionId);
                }
            }
            else
            {
            }

            return null;
        }

        /// <summary>
        /// Gets the currents security string and passes it to the client
        /// </summary>
        /// <param name="reply">The reply message</param>
        /// <param name="correlationState">The correclation state of the message</param>
        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            /*
            string userDisplayName = string.Empty;
            IEnumerable<string> roleNames = null;
            if (WcfUserSessionSecurity.Current.User != null)
            {
                userDisplayName = WcfUserSessionSecurity.Current.User.DisplayName;
                if (string.IsNullOrWhiteSpace(userDisplayName)) userDisplayName = WcfUserSessionSecurity.Current.User.UserName;
                roleNames = WcfUserSessionSecurity.Current.User.RoleNames;
            }
            */

            UserSessionConfiguration config = WcfUserSessionSecurity.Current.GetSessionConfig();
            var typedHeader = new MessageHeader<UserSessionConfiguration>(config);
            var untypedHeader = typedHeader.GetUntypedHeader(WcfUserSessionBehaviour.HeaderName, WcfUserSessionBehaviour.HeaderNamespace);
            reply.Headers.Add(untypedHeader);
        }

        #endregion

        #region IClientMessageInspector

        /// <summary>
        /// Executed before a request is send to the service. Adds the security string cookie to the request if not already there and if it can be found
        /// </summary>
        /// <param name="request">The request message</param>
        /// <param name="channel">The channel being used</param>
        /// <returns>Null as there is nothing to return</returns>
        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            // Add the cookie if it is not already there
            // if (!OperationContext.Current.OutgoingMessageProperties.ContainsKey(HttpRequestMessageProperty.Name) ||
            //  !((HttpRequestMessageProperty)OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name]).Headers[HttpRequestHeader.Cookie].Contains(WcfUserSessionBehaviour.CookieName))
            /*{*/

            string value = string.Empty;

            if (OperationContext.Current != null && OperationContext.Current.IncomingMessageHeaders != null && OperationContext.Current.IncomingMessageHeaders.FindHeader(WcfUserSessionBehaviour.HeaderName, WcfUserSessionBehaviour.HeaderNamespace) > -1)
            {
                var header = OperationContext.Current.IncomingMessageHeaders.GetHeader<UserSessionConfiguration>(WcfUserSessionBehaviour.HeaderName, WcfUserSessionBehaviour.HeaderNamespace);

                if (header != null)
                {
                    value = header.SessionId;
                }
            }
            else if (HttpContext.Current != null && HttpContext.Current.Request.Cookies[WcfUserSessionBehaviour.SecurityStringCookieName] != null)
            {
                value = HttpContext.Current.Request.Cookies[WcfUserSessionBehaviour.SecurityStringCookieName].Value;
            }

            /*
                if (!string.IsNullOrWhiteSpace(value))
                {
                    HttpRequestMessageProperty requestProperty;
                    if (request.Properties.ContainsKey(HttpRequestMessageProperty.Name))
                    {
                        requestProperty = (HttpRequestMessageProperty)request.Properties[HttpRequestMessageProperty.Name];
                    }
                    else
                    {
                        requestProperty = new HttpRequestMessageProperty();
                        request.Properties.Add(HttpRequestMessageProperty.Name, requestProperty);
                    }

                    requestProperty.Headers[HttpRequestHeader.Cookie] = WcfUserSessionBehaviour.CookieName + "=" + value;
                }
            }*/

            RequestHeader requestHeader = new RequestHeader();
            if (request.Headers.FindHeader(WcfUserSessionBehaviour.HeaderName, WcfUserSessionBehaviour.HeaderNamespace) > -1)
            {
                requestHeader = request.Headers.GetHeader<RequestHeader>(WcfUserSessionBehaviour.HeaderName, WcfUserSessionBehaviour.HeaderNamespace);
            }

            if (requestHeader.SessionId == null)
            {
                if (WcfUserClientSession.Current.Config != null)
                {
                    requestHeader.SessionId = WcfUserClientSession.Current.Config.SessionId;
                }
                else if (!string.IsNullOrWhiteSpace(value))
                {
                    requestHeader.SessionId = value;
                }
            }

            string strHostName = System.Net.Dns.GetHostName();
            IPHostEntry ipHostInfo = Dns.GetHostEntry(strHostName);

            requestHeader.ClientName = requestHeader.ClientName != null ? requestHeader.ClientName : strHostName;
            requestHeader.UserIp = requestHeader.UserIp != null ? requestHeader.UserIp : WcfUserClientSession.Current.UserIp != null ? WcfUserClientSession.Current.UserIp : HttpContext.Current != null ? HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] : ipHostInfo.AddressList.Select(a => a.ToString()).Aggregate((s1, s2) => { return s1 + "|" + s2; });

            var typedHeader = new MessageHeader<RequestHeader>(requestHeader);
            var untypedHeader = typedHeader.GetUntypedHeader(WcfUserSessionBehaviour.HeaderName, WcfUserSessionBehaviour.HeaderNamespace);
            request.Headers.RemoveAll(WcfUserSessionBehaviour.HeaderName, WcfUserSessionBehaviour.HeaderNamespace);
            request.Headers.Add(untypedHeader);

            return null;
        }

        /// <summary>
        /// Executed after a reply is received. Adds the security string and cookie if the HttpContext.Current instance is available
        /// </summary>
        /// <param name="reply">The reply of the server</param>
        /// <param name="correlationState">The correlation state</param>
        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            if (HttpContext.Current == null) return;

            var header = reply.Headers.GetHeader<UserSessionConfiguration>(WcfUserSessionBehaviour.HeaderName, WcfUserSessionBehaviour.HeaderNamespace);
            WcfUserClientSession.SetClientSession(header);

            if (header != null && !string.IsNullOrWhiteSpace(header.SessionId))
            {
                HttpContext.Current.Response.Cookies.Add(new HttpCookie(WcfUserSessionBehaviour.SecurityStringCookieName, header.SessionId));
                if (!string.IsNullOrWhiteSpace(header.UserDisplayName))
                {
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                        1,
                        header.UserDisplayName,
                        DateTime.Now, DateTime.Now.AddMinutes(header.Sessiontimeout),
                        false,
                        header.RoleNames == null || header.RoleNames.Count() == 0 ? string.Empty : header.RoleNames.Aggregate((s1, s2) => { return s1 + "," + s2; }),
                        FormsAuthentication.FormsCookiePath);

                    System.Web.HttpContext.Current.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket)));
                    if (HttpContext.Current.User == null || !HttpContext.Current.User.Identity.IsAuthenticated)
                    {
                        HttpContext.Current.User = new GenericPrincipal(new FormsIdentity(ticket), header.RoleNames.ToArray());
                    }
                }
            }
            else
            {
                HttpContext.Current.Response.Cookies.Remove(WcfUserSessionBehaviour.SecurityStringCookieName);
                FormsAuthentication.SignOut();
            }
        }

        #endregion

        #region IEndpointBehavior

        /// <summary>
        /// Validates the endpoint (Nothing is done here)
        /// </summary>
        /// <param name="endpoint">The end point to validate</param>
        public void Validate(ServiceEndpoint endpoint)
        {
        }

        /// <summary>
        /// Implements a modification or extension of the client across an endpoint. (Nothing is done here)
        /// </summary>
        /// <param name="endpoint">The end point</param>
        /// <param name="bindingParameters">The objects that binding elements require to support the behavior.</param>
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        /// <summary>
        /// Implements a modification or extension of the client across an endpoint. 
        /// </summary>
        /// <param name="endpoint">The endpoint that exposes the contract.</param>
        /// <param name="endpointDispatcher">The endpoint dispatcher to be modified or extended.</param>
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            var channelDispatcher = endpointDispatcher.ChannelDispatcher;
            if (channelDispatcher == null) return;
            foreach (var ed in channelDispatcher.Endpoints)
            {
                var inspector = new WcfUserSessionBehaviour();
                ed.DispatchRuntime.MessageInspectors.Add(inspector);
            }
        }

        /// <summary>
        /// Implements a modification or extension of the client across an endpoint. (Nothing is done here)
        /// </summary>
        /// <param name="endpoint">The endpoint that is to be customized.</param>
        /// <param name="clientRuntime">The client runtime to be customized.</param>
        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            var inspector = new WcfUserSessionBehaviour();
            clientRuntime.MessageInspectors.Add(inspector);
        }

        #endregion

        #region IServiceBehaviour

        /// <summary>
        /// Provides the ability to inspect the service host and the service description
        /// to confirm that the service can run successfully.
        /// </summary>
        /// <param name="serviceDescription">The service description.</param>
        /// <param name="serviceHostBase">The service host that is currently being constructed.</param>
        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        /// <summary>
        /// Provides the ability to pass custom data to binding elements to support the
        /// contract implementation.
        /// </summary>
        /// <param name="serviceDescription">The service description of the service.</param>
        /// <param name="serviceHostBase">The host of the service.</param>
        /// <param name="endpoints">The service endpoints.</param>
        /// <param name="bindingParameters">Custom objects to which binding elements have access.</param>
        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }

        /// <summary>
        /// Provides the ability to change run-time property values or insert custom
        /// extension objects such as error handlers, message or parameter interceptors,
        /// security extensions, and other custom extension objects.
        /// </summary>
        /// <param name="serviceDescription">The service description.</param>
        /// <param name="serviceHostBase">The host that is currently being built.</param>
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcher channelDispatcher in serviceHostBase.ChannelDispatchers)
            {
                foreach (var endPointDispatcher in channelDispatcher.Endpoints)
                {
                    endPointDispatcher.DispatchRuntime.MessageInspectors.Add(new WcfUserSessionBehaviour());
                }
            }
        }

        #endregion
    }
}