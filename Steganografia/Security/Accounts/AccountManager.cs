using Steganografia.EntityFramework;
using Steganografia.Models.Users;
using Steganografia.Security.Cookies;
using System.Linq;
using System.Web;
using System;
using System.Security.Cryptography;

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
            User user = new User(userName, GetHash(password), firstName, lastName);
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
			var hashedPassword = GetHash(password);

			return _userRepository.AsQueryable().Any(x => (x.UserName == userName) && (x.Password == hashedPassword));
        }

        public bool UserNameTaken(string userName)
        {
            return _userRepository.AsNoTracking().Any(x => x.UserName == userName);
        }


		public static string GetHash(string input)
		{
			HashAlgorithm hashAlgorithm = new SHA256CryptoServiceProvider();

			byte[] byteValue = System.Text.Encoding.UTF8.GetBytes(input);

			byte[] byteHash = hashAlgorithm.ComputeHash(byteValue);

			return Convert.ToBase64String(byteHash);
		}

		public void Logout(HttpContextBase httpContext)
		{
			_cookieManager.RemoveCookie(httpContext);
		}
	}
}