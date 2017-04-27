using System.Web;

namespace Steganografia.Security.Accounts
{
    public interface IAccountManager
    {
        bool UserExists(string userName, string password);
        void SignIn(string userName, HttpContextBase response);
        bool UserNameTaken(string userName);
        void CreateAccount(string userName, string password, string firstName, string lastName);
    }
}