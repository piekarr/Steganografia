using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Steganografia.ViewModels.Home;
using Steganografia.Models.Conversations;
using Steganografia.EntityFramework;
using Steganografia.Models.Users;

namespace Steganografia.Services.Conversations
{
    public class ConversationService : IConversationService
    {
        private readonly IRepository<Conversation> _conversationRepository;
        private readonly IRepository<Message> _messageRepository;
        private readonly IRepository<User> _userRepository;

        public ConversationService() : this(new RepositoryBase<Conversation>(), new RepositoryBase<Message>(), new RepositoryBase<User>())
        {

        }

        public ConversationService(IRepository<Conversation> conversationRepository, IRepository<Message> messageRepository, IRepository<User> userRepository)
        {
            _conversationRepository = conversationRepository;
            _messageRepository = messageRepository;
            _userRepository = userRepository;
        }

        public List<ConversationViewModel> GetConversationForUser(int userId)
        {
            var result = _conversationRepository.AsNoTracking();
            result = IsMemberOfConversation(result, userId);
            return result.OrderByDescending(x => !x.Messages.Any() ? x.CreatedDate : x.Messages
                                                                                    .OrderByDescending(m => m.CreatedDate)
                                                                                    .Select(s => s.CreatedDate)
                    .FirstOrDefault())
                        .Select(x => new ConversationViewModel
                        {
                            Id = x.Id,
                            Name = x.Name,
                            UnreadMessages = x.Messages.Count(m => m.UsersWhichDidNotReadMessage.Any(u => u.UserId == userId))
                        }).ToList();
        }

        public List<ConversationMessageViewModel> GetMessagesForConversation(int conversationId)
        {
            return _messageRepository.AsNoTracking()
                .Where(x => x.ConversationId == conversationId)
                .OrderBy(x => x.CreatedDate)
                .Select(x => new ConversationMessageViewModel
                {
                    Id = x.Id,
                    Content = x.Content,
                    CreatedByUser = x.CreatedByUser.LastName + " " + x.CreatedByUser.FirstName,
                    CreatedDate = x.CreatedDate,
                    CreatedByUserId = x.CreatedByUserId
                }).ToList();
        }

        public bool UserIsAMemberOfConversation(int conversationId, int userId)
        {
            return IsMemberOfConversation(_conversationRepository.AsNoTracking(), userId).Any();
        }

        public void CreateConversation(string name, List<int> userIds, int userId)
        {
            var conversation = new Conversation
            {
                Name = name,
                CreatedByUserId = userId,
                Users= _userRepository.AsQueryable().Where(x => userIds.Contains(x.Id) || (x.Id == userId)).ToList()
            };
            _conversationRepository.Add(conversation);
        }

        public bool UsersExists(List<int> userIds)
        {
            return _userRepository.AsQueryable().Where(x => userIds.Contains(x.Id)).Count() == userIds.Count();
        }

        private static IQueryable<Conversation> IsMemberOfConversation(IQueryable<Conversation> query, int userId)
        {
            return query.Where(c => c.Users.Any(u => u.Id == userId) || (c.CreatedByUserId == userId));
        }
    }
}