using System.Security.Principal;

namespace Steganografia.Security.Entities
{
    public class AppIdentity : GenericIdentity
    {
        public int Id { get; private set; }
        public AppIdentity(int id, string username) : base(username)
        {
            Id = id;
        }
    }
}