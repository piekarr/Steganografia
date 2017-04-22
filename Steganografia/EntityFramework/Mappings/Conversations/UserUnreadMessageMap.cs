using Steganografia.Models.Conversations;

namespace Steganografia.EntityFramework.Mappings.Conversations
{
    public class UserUnreadMessageMap : EntityBaseMap<UserUnreadMessage>
    {
        public UserUnreadMessageMap()
        {
            Property(x => x.UserId);
            Property(x => x.MessageId);
            HasRequired(x => x.User).WithMany().HasForeignKey(x => x.UserId);
            HasRequired(x => x.Message).WithMany(x => x.UsersWhichDidNotReadMessage).HasForeignKey(x => x.MessageId);
        }
    }
}