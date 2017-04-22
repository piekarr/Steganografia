using Steganografia.Security.Cookies;
using System.Threading;
using System.Web.Mvc.Filters;

namespace Steganografia.Security.Filters
{
    public class CustomAuthenticationFilter : IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            ICookieManager cookieManager = new CookieManager();

            if (!filterContext.Principal.Identity.IsAuthenticated)
            {
                var appPrincipal = cookieManager.ValidateRequestCookie(filterContext.HttpContext);
                if (appPrincipal != null)
                {
                    filterContext.Principal = appPrincipal;
                    Thread.CurrentPrincipal = appPrincipal;
                }
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {

        }
    }
}