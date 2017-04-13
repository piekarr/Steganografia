using Steganografia.Security.Cookies;
using System.Threading;
using System.Web.Mvc.Filters;

namespace Steganografia.Security.Filters
{
    public class CustomAuthenticationFilter : IAuthenticationFilter
    {
        private readonly ICookieManager _cookieManager;

        public CustomAuthenticationFilter() : this(new CookieManager())
        {

        }

        public CustomAuthenticationFilter(ICookieManager cookieManager)
        {
            _cookieManager = cookieManager;
        }

        public void OnAuthentication(AuthenticationContext filterContext)
        {
            if (!filterContext.Principal.Identity.IsAuthenticated)
            {
                var appPrincipal = _cookieManager.ValidateRequestCookie(filterContext.HttpContext);
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