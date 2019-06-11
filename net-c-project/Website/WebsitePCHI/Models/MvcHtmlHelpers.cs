using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Website.Models
{
    public static class MvcHtmlHelpers
    {
        /// <summary>
        /// Create an a html tag with the specified image inside of a div
        /// It does a check against the provided roles to see if the current user is allowed to see this item. 
        /// If one or more roles have been specified but there is no session, or there is no match, an empty MvcHtmlString is returned.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="text">Text that appears on the link</param>
        /// <param name="image">URL of the image to display</param>
        /// <param name="action">Action to execute</param>
        /// <param name="controller">Controller where the action belongs</param>
        /// <param name="routeValues">Aditional parameteres to the URL (QueryString)</param>        
        /// <param name="roles">The roles the user must have set on of in <see cref="DSPrima.WcfUserSession.ClientSession.WcfUserClientSession.Current.Config.SessionData<PCHI.Model.Security.ClientSessionDetails>().SelectedRole"/> in order to see this Menu item. If none has been specified, no role check is performed and the menu item is shown as well</param>
        /// <returns>The HTML tag created</returns>
        public static MvcHtmlString MenuItem(this HtmlHelper htmlHelper, string text, string image, string action, string controller, object routeValues = null, params string[] roles)
        {
            if (roles != null && roles.Length > 0)
            {
                if (DSPrima.WcfUserSession.ClientSession.WcfUserClientSession.Current.Config == null) return new MvcHtmlString(string.Empty);
                var sessionData = DSPrima.WcfUserSession.ClientSession.WcfUserClientSession.Current.Config.SessionData<PCHI.Model.Security.ClientSessionDetails>();
                if (!roles.Contains(sessionData.SelectedRole)) return new MvcHtmlString(string.Empty);
            }

            var li = new TagBuilder("li");
            var routeData = htmlHelper.ViewContext.RouteData;
            var currentAction = routeData.GetRequiredString("action");
            var currentController = routeData.GetRequiredString("controller");

            li.InnerHtml = @"<a href=""" + new UrlHelper(htmlHelper.ViewContext.RequestContext).Action(action, controller, routeValues) + @""">
                <div class=""tab-icon" + (string.Equals(currentAction, action, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(currentController, controller, StringComparison.OrdinalIgnoreCase) ? " tab-current" : string.Empty) + @""">
                    " + (!string.IsNullOrWhiteSpace(image) ? @"<img src=""/Content/Images/" + image + @""" width=""60"" height=""60"" alt="""" />" : string.Empty) + @"<br />
                    " + text + @"
                </div>
            </a>";

            return MvcHtmlString.Create(li.ToString());
        }

        public static MvcHtmlString MenuSeparator(this HtmlHelper htmlHelper, string text)
        {
            var li = new TagBuilder("li");            
            li.InnerHtml = @"<div class=""tab-icon"" style=""background-color:transparent;color:black;height:5px;""><strong>" + text + @"</strong></div>";
            return MvcHtmlString.Create(li.ToString());
        }
    }
}

