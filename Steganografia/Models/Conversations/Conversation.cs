using Steganografia.Models.Users;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Steganografia.Models.Conversations
{
    public class Conversation : EntityBase
    {
        public virtual string Name { get; set; }
        public virtual ICollection<Message> Messages { get; set; } = new Collection<Message>();
        public virtual ICollection<User> Users { get; set; } = new Collection<User>();
    }
}