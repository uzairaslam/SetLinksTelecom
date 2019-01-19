using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SetLinksTelecom.Data
{
    public static class HtmlHelpers
    {
        public static string IsActiveLink(this HtmlHelper html, string controllers = "", string actions = "", string cssClass = "selected")
        {
            var routeData = html.ViewContext.RouteData;

            var routeAction = routeData.Values["action"].ToString();
            var routeController = routeData.Values["controller"].ToString();

            return (controllers == routeController && actions == routeAction) ? "active" : "";
        }
    }
}