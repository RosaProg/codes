using PCHI.Model.Security;
using PCHI.Model.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebsiteSupportLibrary.Models.Attributes
{
    /// <summary>
    /// Defines a Functionality Permission attribute
    /// </summary>
    public abstract class RoleCheckAttrribute : ActionFilterAttribute
    {
        /// <summary>
        /// The list of specified roles in Upper Case formate
        /// </summary>
        protected List<string> roles;

        /// <summary>
        /// The action name to redirect to
        /// </summary>
        private ActionResult actionResult;

        /// <summary>
        /// IF true, check for any of the functionalities, if false check for all the functionalities
        /// </summary>
        private bool CheckForAnyAttribute = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleCheckAttrribute"/> class
        /// Allows for verification if the user has any or all the specified attributes present.
        /// If the Functionality verification fails, the user is redirect to the Action and Controller specified.
        /// If action is null, the user is redirected to a default page.
        /// If action is not null but the controller is null the user is redirect to an action with the same controller
        /// </summary>
        /// <param name="checkForAnyAttribute">If true, Specifies to check for any of the specified roles. If false, the user must have ALL the roles specified </param>
        /// <param name="actionResult">The actionresult redirect to if the user doesn't have the required functionality (Can be null)</param>
        /// <param name="role">The role to check for</param>
        /// <param name="roles">Any additional roles to check for</param>
        public RoleCheckAttrribute(bool checkForAnyAttribute, ActionResult actionResult, string role, params string[] roles)
        {
            this.CheckForAnyAttribute = checkForAnyAttribute;
            this.roles = new List<string>();
            if (roles != null) this.roles.AddRange(roles.Select(r => r.ToUpper()));
            this.roles.Add(role.ToUpper());
            this.actionResult = actionResult;
        }

        /// <summary>
        /// Executed before the action is being executed
        /// Sub classes should override this function and call this to call the base class to order the redirect
        /// </summary>
        /// <param name="filterContext">The ActionExecutingContext to use</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            /*
            bool redirect = false;
            if (HttpContext.Current.User == null) redirect = true;
            else
            {
                var sessionData = DSPrima.WcfUserSession.ClientSession.WcfUserClientSession.Current.Config.SessionData<PCHI.Model.Security.ClientSessionDetails>();
                if (CheckForAnyAttribute)
                {
                    redirect = true;
                    foreach (string r in roles)
                    {
                        if (HttpContext.Current.User.IsInRole(r.ToString()))
                        {
                            redirect = false;
                            break;
                        }
                    }
                }
                else
                {
                    foreach (string r in roles)
                    {
                        if (!HttpContext.Current.User.IsInRole(r.ToString())) redirect = true;
                    }
                }
            }

            if (redirect)
            {
                filterContext.Result = this.GetRedirect();
            }*/

            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// Gets a redirect action based upon the action name and controller name
        /// If actionname is null, a default redirect is returned
        /// If the controllername is null, a redirect to within the same controller is used
        /// </summary>
        /// <param name="actionName">The action name. </param>
        /// <param name="controllerName">The controller name</param>
        /// <returns>A redirect ActionResult </returns>
        protected ActionResult GetRedirect()
        {
            if (this.actionResult != null) return this.actionResult;

            RouteValueDictionary routeValueDictionary = new RouteValueDictionary();

            routeValueDictionary["action"] = "Login";
            routeValueDictionary["controller"] = "Access";

            return new RedirectToRouteResult(routeValueDictionary);
        }
    }
}