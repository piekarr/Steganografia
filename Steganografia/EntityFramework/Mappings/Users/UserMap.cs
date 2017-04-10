using Steganografia.Models.Users;

namespace Steganografia.EntityFramework.Mappings.Users
{
    public class UserMap : EntityBaseMap<User>
    {
        public UserMap()
        {
            Property(x => x.UserName);
            Property(x => x.Password);
            Property(x => x.FirstName);
            Property(x => x.LastName);
        }
    }
}