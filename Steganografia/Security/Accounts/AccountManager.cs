using Steganografia.EntityFramework;
using Steganografia.Models.Users;
using Steganografia.Security.Cookies;
using System.Linq;
using System.Web;
using System;

namespace Steganografia.Security.Accounts
{
    public class AccountManager : IAccountManager
    {
        private readonly IRepository<User> _userRepository;
        private readonly ICookieManager _cookieManager;
        public AccountManager() : this(new RepositoryBase<User>(), new CookieManager())
        {

        }

        public AccountManager(IRepository<User> userRepository, ICookieManager cookieManager)
        {
            _userRepository = userRepository;
            _cookieManager = cookieManager;
        }

        public void CreateAccount(string userName, string password, string firstName, string lastName)
        {
            User user = new User(userName, password, firstName, lastName);
            _userRepository.Add(user);
            _userRepository.SaveOrUpdate();
        }

        public void SignIn(string userName, HttpContextBase httpContextBase)
        {
            var user = _userRepository.AsNoTracking().First(x => x.UserName == userName);
            _cookieManager.AddAuthenticationCookie(user.Id, httpContextBase);
        }

        public bool UserExists(string userName, string password)
        {
            return _userRepository.AsQueryable().Any(x => (x.UserName == userName) && (x.Password == password));
        }

        public bool UserNameTaken(string userName)
        {
            return _userRepository.AsNoTracking().Any(x => x.UserName == userName);
        }
    }
}