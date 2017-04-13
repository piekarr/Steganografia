using System.Web.Mvc;
using System.Web.Routing;

namespace Steganografia.Security.Filters
{
    public class CustomAuthorizationFilter : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "SignIn" }));
        }
    }
}