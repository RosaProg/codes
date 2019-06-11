using DSPrima.WcfUserSession.ClientSession;
using PCHI.WcfServices.API.PCHIServices.InterfaceProxies.Users;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExternalAuthenticationWebsite.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Url to the index: http://[host]/Home/Index?path=Admin/Patient?patientId=[patientId]
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public ActionResult Index(string path)
        {

            UserClient uc = new UserClient();
            uc.Ping();
            if (WcfUserClientSession.Current.Config == null)
            {
                string username = System.Web.HttpContext.Current.User.Identity.Name;
                var result = uc.Login(username, null);
                if (!result.Succeeded)
                {
                    ViewBag.ErrorMessage = result.ErrorMessages;
                    return this.View();
                }

                var resultRoles = uc.UserHasMultipleRoles(username);
                if (resultRoles.Strings.Count > 1)
                {
                    if (resultRoles.Strings.Contains("Practitioner")) uc.SelectRole(username, "Practitioner");
                    else if (resultRoles.Strings.Contains("Administrator")) uc.SelectRole(username, "Administrator");
                    else if (resultRoles.Strings.Contains("Telephonist")) uc.SelectRole(username, "Telephonist");
                    else if (resultRoles.Strings.Contains("Researcher")) uc.SelectRole(username, "Researcher");
                    else
                    {
                        ViewBag.ErrorMessage = "You are not authorized to use Windows Authentication";                        
                    }
                }
            }
            Uri baseUri = new Uri(ConfigurationManager.AppSettings["PCHIWebsiteUrl"]);
            Uri destination = null;
            if (string.IsNullOrWhiteSpace(path))
            {
                destination = new Uri(baseUri, "Admin/Index");
            }
            else
            {
                destination = new Uri(baseUri, path);
            }
            
            return this.Redirect(destination.AbsoluteUri);
        }
    }
}