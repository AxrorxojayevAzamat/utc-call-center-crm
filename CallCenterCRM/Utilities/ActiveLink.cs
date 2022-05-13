using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Specialized;
using System.Web;

namespace CallCenterCRM.Utilities
{
    public static class Utilities
    {
        public static string IsActive(this IHtmlHelper htmlHelper, string controller, string action)
        {
            string[] onePageActions = { "Details", "Edit", "Delete", "Create" };
            string[] linkActions = { "Index", "AppsList", "SendOrg", "Delayed", "Selected", "RejectedOrg",
                "RejectedMod", "Rejected", "Edited", "Branches", "AnswersList" };
            string[] listActions = { "Index", "AppsList", "AnswersList" };

            RouteData routeData = htmlHelper.ViewContext.RouteData;
            QueryString qs = htmlHelper.ViewContext.HttpContext.Request.QueryString;
            string? actionName = new BuildQueryParams(qs).GetObject().actionName;
            string routeAction = routeData.Values["action"].ToString();
            string routeController = routeData.Values["controller"].ToString();
            bool equelsActionName = actionName != null ? actionName == action : Array.Exists(listActions, element => element == action);

            var returnActive = Array.Exists(onePageActions, element => element == routeAction)
                ? controller == routeController && Array.Exists(linkActions, element => element == action) && equelsActionName
                : controller == routeController && action == routeAction;

            return returnActive ? "active" : "";
        }
    }

    public class BuildQueryParams
    {
        private QueryString _queryString;
        public int? authorId { get; set; }
        public int? recipentId { get; set; }
        public string? actionName { get; set; }

        public BuildQueryParams(QueryString queryString)
        {
            _queryString = queryString;
        }

        public BuildQueryParams GetObject()
        {
            NameValueCollection dict = HttpUtility.ParseQueryString(_queryString.Value);
            string json = JsonConvert.SerializeObject(dict.Cast<string>().ToDictionary(k => k, v => dict[v]));

            return JsonConvert.DeserializeObject<BuildQueryParams>(json);
        }
    }
}
