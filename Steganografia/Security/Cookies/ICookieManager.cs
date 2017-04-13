using Steganografia.Security.Entities;
using System.Web;

namespace Steganografia.Security.Cookies
{
    public interface ICookieManager
    {
        void AddAuthenticationCookie(int userId, HttpContextBase httpContext);
        AppPrincipal ValidateRequestCookie(HttpContextBase httpContext);
    }
}