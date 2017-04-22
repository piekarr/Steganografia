using Steganografia.Models.Conversations;

namespace Steganografia.EntityFramework.Mappings.Conversations
{
    public class ConversationMap : EntityBaseMap<Conversation>
    {
        public ConversationMap()
        {
            Property(x => x.Name);
            HasMany(x => x.Users).WithMany();
        }
    }
}