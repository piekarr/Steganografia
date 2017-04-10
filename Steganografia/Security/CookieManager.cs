using Steganografia.EntityFramework;
using Steganografia.Models.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Steganografia.Security
{
    public class CookieManager : ICookieManager
    {
        private const string COOKIE_NAME = "PiekarSiteAuthenticate";
        private readonly IRepository<Session> _sessionRepository;
        public CookieManager() : this(new RepositoryBase<Session>())
        {
        }

        public CookieManager(IRepository<Session> sessionRepository)
        {
            _sessionRepository = sessionRepository;
        }
        public HttpCookie GetNewCookie(int userId)
        {
            var authenticationCookieExpires = DateTime.UtcNow.AddMinutes(15);
            var authenticationCookieValue = Guid.NewGuid();
            _sessionRepository.Add(new Session(authenticationCookieValue, userId, authenticationCookieExpires));
            return new HttpCookie(COOKIE_NAME, authenticationCookieValue.ToString())
            {
                Expires = authenticationCookieExpires
            };
        }
    }
}