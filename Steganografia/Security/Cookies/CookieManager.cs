using Steganografia.EntityFramework;
using Steganografia.Models.Sessions;
using Steganografia.Models.Users;
using Steganografia.Security.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Steganografia.Security.Cookies
{
    public class CookieManager : ICookieManager
    {
        private const string COOKIE_NAME = "PiekarSiteAuthenticate";
        private readonly IRepository<Session> _sessionRepository;
        private readonly IRepository<User> _userRepository;
        public CookieManager() : this(new RepositoryBase<Session>(), new RepositoryBase<User>())
        {
        }

        public CookieManager(IRepository<Session> sessionRepository, IRepository<User> userRepository)
        {
            _sessionRepository = sessionRepository;
            _userRepository = userRepository;
        }
        public void AddAuthenticationCookie(int userId, HttpContextBase httpContext)
        {
            var authenticationCookieExpires = DateTime.UtcNow.AddMinutes(15);
            var authenticationCookieValue = Guid.NewGuid();
            _sessionRepository.Add(new Session(authenticationCookieValue, userId, authenticationCookieExpires));
            var authenticationCookie = CreateNewCookie(authenticationCookieValue, authenticationCookieExpires);
            httpContext.Response.Cookies.Add(authenticationCookie);
        }

        public AppPrincipal ValidateRequestCookie(HttpContextBase httpContext)
        {
            Guid sesionToken;
            var authenticationCookie = httpContext.Request.Cookies[COOKIE_NAME];
            if ((authenticationCookie != null) && Guid.TryParse(authenticationCookie.Value, out sesionToken))
            {
                var session = _sessionRepository.AsQueryable().FirstOrDefault(x => (x.SesssionId == sesionToken) && x.IsActive);
                if ((session != null) && (session.Expires >= DateTime.UtcNow))
                {
                    session.Expires = DateTime.UtcNow.AddMinutes(15);
                    _sessionRepository.SaveOrUpdate();
                    httpContext.Response.Cookies.Add(CreateNewCookie(session.SesssionId, session.Expires));
                    var appIdentityAnonymous = _userRepository.AsNoTracking()
                                        .Where(x => x.Id == session.UserId)
                                        .Select(x => new { x.Id, x.UserName }).First();
                    return new AppPrincipal(new AppIdentity(appIdentityAnonymous.Id, appIdentityAnonymous.UserName));
                }
            }
            if (httpContext.Request.IsLocal)
            {
                var admin = _userRepository.AsNoTracking().Where(x => x.UserName == "Admin").First();
                AddAuthenticationCookie(admin.Id, httpContext);
                return new AppPrincipal(new AppIdentity(admin.Id, admin.UserName));
            }
            return null;
        }
        private HttpCookie CreateNewCookie(Guid cookieId, DateTime expires)
        {
            return new HttpCookie(COOKIE_NAME, cookieId.ToString())
            {
                Expires = expires
            };
        }
    }
}