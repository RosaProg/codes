using PCHI.Model.Security;
using PCHI.Model.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebsiteSupportLibrary.Models.Attributes
{
    /// <summary>
    /// Defines a Functionality Permission attribute that checks if the current user has any of the functionalities specified.
    /// If no any of the functionalities are specified, the user is redirected to another page
    /// </summary>
    public class AnyRole : RoleCheckAttrribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnyFunctionalityPermissionAttrribute"/> class
        /// Allows for verification if the user has any of the specified attributes present.
        /// If the Functionality verification fails, the user is redirect to a default action and controller.
        /// </summary>
        /// <param name="role">The role to check for</param>
        /// <param name="roles">Any additional rolesto check for</param>
        public AnyRole(string role, params string[] roles)
            : this(null, role, roles)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnyFunctionalityPermissionAttrribute"/> class
        /// Allows for verification if the user has any of the specified attributes present.
        /// If the Functionality verification fails, the user is redirect to the Action and Controller specified.
        /// If actionResult is null, the user is redirected to a default page.        
        /// </summary>
        /// <param name="actionResult">The actionresult redirect to if the user doesn't have the required functionality (Can be null)</param>
        /// <param name="role">The role to check for</param>
        /// <param name="roles">Any additional rolesto check for</param>
        public AnyRole(ActionResult actionResult, string role, params string[] roles)
            : base(true, actionResult, role, roles)
        {
        }

        /// <summary>
        /// Executed before the action is being executed
        /// Checks if the SelectedRoles can be found in the list of allowed roles
        /// </summary>
        /// <param name="filterContext">The ActionExecutingContext to use</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool redirect = false;
            if (DSPrima.WcfUserSession.ClientSession.WcfUserClientSession.Current.Config == null)
            {
                redirect = true;
            }
            else
            {
                var sessionData = DSPrima.WcfUserSession.ClientSession.WcfUserClientSession.Current.Config.SessionData<PCHI.Model.Security.ClientSessionDetails>();
                if (!this.roles.Contains(sessionData.SelectedRole.ToUpper())) redirect = true;
            }

            if (redirect)
            {
                filterContext.Result = this.GetRedirect();
            }

            base.OnActionExecuting(filterContext);
        }
    }
}