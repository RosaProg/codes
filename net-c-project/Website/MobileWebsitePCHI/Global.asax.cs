using DSPrima.WcfUserSession.ClientSession;
using DSPrima.WcfUserSession.SessionStores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace MobileWebsitePCHI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            WcfUserClientSession.SessionStore = new ClientSessionStore(null, "MobileSessions.dat");
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                System.Web.HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(new FormsIdentity(ticket), ticket.UserData.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries));
                WcfUserClientSession.LoadSession();
            }
        }
    }
}
