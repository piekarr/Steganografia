using System.Web;

namespace Steganografia.Security
{
    public interface ICookieManager
    {
        HttpCookie GetNewCookie(int userId);
    }
}