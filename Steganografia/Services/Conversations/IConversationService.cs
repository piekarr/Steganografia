using System.Collections.Generic;
using Steganografia.ViewModels.Home;

namespace Steganografia.Services.Conversations
{
    public interface IConversationService
    {
        List<ConversationViewModel> GetConversationForUser(int userId);
        List<ConversationMessageViewModel> GetMessagesForConversation(int conversationId);
        bool UserIsAMemberOfConversation(int conversationId, int userId);
        void CreateConversation(string name, List<int> userIds, int userId);
        bool UsersExists(List<int> userIds);
    }
}