using Steganografia.Models.Users;

namespace Steganografia.Models.Conversations
{
    public class UserUnreadMessage : EntityBase
    {
        public virtual int UserId { get; set; }
        public virtual User User { get; set; }
        public virtual int MessageId { get; set; }
        public virtual Message Message { get; set; }
    }
}