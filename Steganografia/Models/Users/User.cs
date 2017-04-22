using Steganografia.Models.Conversations;
using System.Collections.Generic;

namespace Steganografia.Models.Users
{
    public class User : EntityBase
    {
        public virtual string UserName { get; set; }
        public virtual string Password { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }

        protected User()
        {

        }

        public User(string userName, string password, string firstName, string lastName) : base(1000)
        {
            UserName = userName;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}