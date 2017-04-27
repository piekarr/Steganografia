using System.Collections.Generic;

namespace Steganografia.Models.Conversations
{
    public class Message : EntityBase
    {
        public virtual string Content { get; set; }
        public virtual int ConversationId { get; set; }
        public virtual Conversation Conversation { get; set; }
        public virtual ICollection<UserUnreadMessage> UsersWhichDidNotReadMessage { get; set; }

        protected Message() : base()
        {

        }

        public Message(string contet, int conversatioId, int createdByUserId) : base(createdByUserId)
        {
            Content = contet;
            ConversationId = conversatioId;
        }
    }
}