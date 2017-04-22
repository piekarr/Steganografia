using Steganografia.Models.Conversations;

namespace Steganografia.EntityFramework.Mappings.Conversations
{
    public class MessageMap : EntityBaseMap<Message>
    {
        public MessageMap()
        {
            Property(x => x.Content).HasColumnType("NVARCHAR");
            Property(x => x.ConversationId);
            HasRequired(x => x.Conversation).WithMany(x => x.Messages).HasForeignKey(x => x.ConversationId);
        }
    }
}