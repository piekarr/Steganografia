using Steganografia.Models.Sessions;

namespace Steganografia.EntityFramework.Mappings.Sessions
{
    public class SessionMap : EntityBaseMap<Session>
    {
        public SessionMap()
        {
            Property(x => x.SesssionId);
            Property(x => x.UserId);
            Property(x => x.Expires);
            HasRequired(x => x.User).WithMany().HasForeignKey(x => x.UserId);
        }
    }
}