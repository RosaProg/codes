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
    /// Defines a Functionality Permission attribute that checks if the current user has all of the functionalities specified.
    /// If no all of the functionalities are specified, the user is redirected to another page
    /// </summary>
    public class AllRoles : RoleCheckAttrribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AllRoles"/> class
        /// Allows for verification if the user has all of the specified attributes present.
        /// If the Functionality verification fails, the user is redirect to the a default page
        /// </summary>
        /// <param name="role">The Role to check for</param>        
        public AllRoles(string role)
            : this(null, role)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AllRoles"/> class
        /// Allows for verification if the user has all of the specified attributes present.
        /// If the Functionality verification fails, the user is redirect to the Action and Controller specified.
        /// If actionResult is null, the user is redirected to a default page.        
        /// </summary>
        /// <param name="actionResult">The actionresult redirect to if the user doesn't have the required functionality (Can be null)</param>
        /// <param name="role">The Role to check for</param>        
        public AllRoles(ActionResult actionResult, string role)
            : base(false, actionResult, role)
        {
        }

        /// <summary>
        /// Executed before the action is being executed
        /// Checks if the SelectedRole matches the first role required
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
                if (this.roles.Count == 0 || sessionData.SelectedRole.ToUpper() != this.roles.FirstOrDefault()) redirect = true;
            }

            if (redirect)
            {
                filterContext.Result = this.GetRedirect();
            }

            base.OnActionExecuting(filterContext);
        }
    }
}