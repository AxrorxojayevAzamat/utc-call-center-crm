using Microsoft.AspNetCore.Mvc.Rendering;

namespace CallCenterCRM.Utilities
{
    public static class Utilities
    {
        public static string IsActive(this IHtmlHelper htmlHelper, string controller, string action)
        {
            var routeData = htmlHelper.ViewContext.RouteData;

            var routeAction = routeData.Values["action"].ToString();
            var routeController = routeData.Values["controller"].ToString();

            var returnActive = (routeAction == "Details" || routeAction == "Edit" || routeAction == "Delete" || routeAction == "Create")
                ? controller == routeController && (action == "Index" || action == "AppsList" || action == "SendOrg" )
                : controller == routeController && action == routeAction;

            return returnActive ? "active" : "";
        }
    }
}
