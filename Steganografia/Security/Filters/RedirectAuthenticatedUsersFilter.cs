using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace Steganografia.Security.Filters
{
    public class RedirectAuthenticatedUsersFilterAttribute : ActionFilterAttribute, IActionFilter
    {
        private string _controllerName;
        private string _actionName;

        public RedirectAuthenticatedUsersFilterAttribute(string controllerName, string actionName)
        {
            _controllerName = controllerName;
            _actionName = actionName;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = _controllerName, action = _actionName }));
            }
        }
    }
}