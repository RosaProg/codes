using DSPrima.WcfUserSession.Behaviours;
using DSPrima.WcfUserSession.ClientSession;
using DSPrima.WcfUserSession.SessionStores;
using PCHI.WcfServices.API.PCHIServices.InterfaceProxies.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace Website
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            WcfUserClientSession.SessionStore = new ClientSessionStore();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            bool hasAuthCookie = false;
            if(Request.Cookies.AllKeys.Contains(FormsAuthentication.FormsCookieName))
            {
                try
                {
                    HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                    hasAuthCookie = true;
                }
                catch(Exception)
                {
                    hasAuthCookie = false;
                    Request.Cookies.Remove(FormsAuthentication.FormsCookieName);
                }
            }

            if(Request.Cookies.AllKeys.Contains(WcfUserSessionBehaviour.SecurityStringCookieName) &&
                !hasAuthCookie)
            {
                new ServiceDetailsClient().Ping();
            }
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                System.Web.HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(new FormsIdentity(ticket), ticket.UserData.Split(new string [] {","}, StringSplitOptions.RemoveEmptyEntries));
                WcfUserClientSession.LoadSession();
            }
        }

    }
}
