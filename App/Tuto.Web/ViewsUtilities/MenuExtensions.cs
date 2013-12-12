using System;
using System.Web.Mvc;

namespace Tuto.Web.ViewsUtilities
{
    public static class MenuExtensions
    {
        public static MvcHtmlString MenuItem(this HtmlHelper htmlHelper, string name, string description, string icon, string action, string controller)
        {
            var menuItemBuilder = new TagBuilder("li");

            var routeData = htmlHelper.ViewContext.ParentActionViewContext.RouteData;
            var currentAction = routeData.GetRequiredString("action");
            var currentController = routeData.GetRequiredString("controller");

            if (string.Equals(currentAction, action, StringComparison.OrdinalIgnoreCase) && string.Equals(currentController, controller, StringComparison.OrdinalIgnoreCase))
            {
                menuItemBuilder.AddCssClass("active");
            }

            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

            var linkBuilder = new TagBuilder("a");
            linkBuilder.Attributes.Add("href", urlHelper.Action(action, controller));
            linkBuilder.Attributes.Add("title", description);

            var iconBuilder = new TagBuilder("span");
            iconBuilder.AddCssClass(icon);

            linkBuilder.InnerHtml = iconBuilder.ToString() + name;
            menuItemBuilder.InnerHtml = linkBuilder.ToString();

            return MvcHtmlString.Create(menuItemBuilder.ToString());
        }
    }
}