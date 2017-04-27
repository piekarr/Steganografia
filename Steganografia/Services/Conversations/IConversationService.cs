using System.Collections.Generic;
using Steganografia.ViewModels.Home;
using System.Web.Mvc;

namespace Steganografia.Services.Conversations
{
    public interface IConversationService
    {
        List<ConversationViewModel> GetConversationForUser(int userId);
        List<ConversationMessageViewModel> GetMessagesForConversation(int conversationId);
        bool UserIsAMemberOfConversation(int conversationId, int userId);
        void CreateConversation(string name, List<int> userIds, int userId);
        bool UsersExists(List<int> userIds);
        bool ConversationForUserGroupExists(List<int> userIds, int userId);
        IEnumerable<SelectListItem> GetAllUsersExceptAsSelectListItems(int userId);
        ConversationMessageViewModel CreateMessage(int conversatonId, string content, int userId);
    }
}