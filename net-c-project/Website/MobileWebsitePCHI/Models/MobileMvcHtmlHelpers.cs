using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MobileWebsitePCHI.Models
{
    /// <summary>
    /// Defines extra Html Helper functionality
    /// </summary>
    public static class MvcHtmlHelpers
    {
        /// <summary>
        /// Create an a html tag for the menu
        /// </summary>
        /// <param name="htmlHelper">The HtmlHelper that is being extended</param>
        /// <param name="text">Text that appears on the link</param>
        /// <param name="image">URL of the image to display</param>
        /// <param name="action">Action to execute</param>
        /// <param name="controller">Controller where the action belongs</param>
        /// <param name="routeValues">Aditional parameteres to the URL (QueryString)</param>        
        /// <returns>The HTML tag created</returns>
        public static MvcHtmlString MenuItem(this HtmlHelper htmlHelper, string text, string image, string action, string controller, object routeValues = null, params string[] roles)
        {
            if (roles != null && roles.Length > 0)
            {
                if (DSPrima.WcfUserSession.ClientSession.WcfUserClientSession.Current.Config == null) return new MvcHtmlString(string.Empty);
                var sessionData = DSPrima.WcfUserSession.ClientSession.WcfUserClientSession.Current.Config.SessionData<PCHI.Model.Security.ClientSessionDetails>();
                if (!roles.Contains(sessionData.SelectedRole)) return new MvcHtmlString(string.Empty);
            }

            var li = new TagBuilder("a");
            li.Attributes.Add("href", new UrlHelper(htmlHelper.ViewContext.RequestContext).Action(action, controller, routeValues));
            li.InnerHtml = @"<li class=""hiddenMenu"">" + text + @"</li>";

            return MvcHtmlString.Create(li.ToString());
        }
    }
}