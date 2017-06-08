using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Steganografia.ViewModels.Home;
using Steganografia.Models.Conversations;
using Steganografia.EntityFramework;
using Steganografia.Models.Users;
using System.Web.Mvc;
using System.IO;
using System.Data.Entity;

namespace Steganografia.Services.Conversations
{
	public class ConversationService : IConversationService
	{
		private readonly IRepository<Conversation> _conversationRepository;
		private readonly IRepository<Message> _messageRepository;
		private readonly IRepository<User> _userRepository;
		private readonly IHiddenMessageService _hiddenMessageService;
		public ConversationService() : this(new RepositoryBase<Conversation>(), new RepositoryBase<Message>(), new RepositoryBase<User>(), new HiddenMessageService())
		{

		}

		public ConversationService(IRepository<Conversation> conversationRepository, IRepository<Message> messageRepository, IRepository<User> userRepository, IHiddenMessageService hiddenMessageService)
		{
			_conversationRepository = conversationRepository;
			_messageRepository = messageRepository;
			_userRepository = userRepository;
			_hiddenMessageService = hiddenMessageService;
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
				.Select(message => new ConversationMessageViewModel
				{
					Id = message.Id,
					Content = message.Content,
					CreatedByUser = message.CreatedByUser.LastName + " " + message.CreatedByUser.FirstName,
					CreatedDate = message.CreatedDate,
					CreatedByUserId = message.CreatedByUserId
				}).ToList();
		}

		public IEnumerable<ConversationMessageViewModel> GetNewestMessages(int conversationId, int? lastMessageId)
		{
			var query = _messageRepository.AsNoTracking()
				.Where(x => x.ConversationId == conversationId);

			if (lastMessageId.HasValue)
			{
				query = query.Where(x => x.Id > lastMessageId.Value);
			}

			return query.OrderBy(x => x.CreatedDate)
				.Select(message => new ConversationMessageViewModel
				{
					Id = message.Id,
					Content = message.Content,
					CreatedByUser = message.CreatedByUser.LastName + " " + message.CreatedByUser.FirstName,
					CreatedDate = message.CreatedDate,
					CreatedByUserId = message.CreatedByUserId
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
				Users = _userRepository.AsQueryable().Where(x => userIds.Contains(x.Id) || (x.Id == userId)).ToList()
			};
			_conversationRepository.Add(conversation);
		}

		public bool UsersExists(List<int> userIds)
		{
			return _userRepository.AsQueryable().Where(x => userIds.Contains(x.Id)).Count() == userIds.Count();
		}

		public bool ConversationForUserGroupExists(List<int> userIds, int userId)
		{
			var allUserIds = userIds.Union(new List<int> { userId }).ToList();
			var cos = _conversationRepository.AsNoTracking()
				.Include(x => x.Users).ToList();
			bool result = false;
			foreach (var item in cos)
			{
				var list = item.Users.Select(x => x.Id);
				result = list.All(x => allUserIds.Contains(x));
				result = result && list.Count() == allUserIds.Count();
				if (result)
					break;
			}
			return result;
		}

		public IEnumerable<SelectListItem> GetAllUsersExceptAsSelectListItems(int userId)
		{
			return _userRepository.AsNoTracking()
				.Where(x => x.Id != userId)
				.ToList()
				.Select(x => new SelectListItem
				{
					Text = x.UserName,
					Value = x.Id.ToString()
				});
		}

		public ConversationMessageViewModel CreateMessage(int conversatonId, string content, int userId)
		{
			content = _hiddenMessageService.FindPaternAndHideInPictureText(content);
			var message = new Message(content, conversatonId, userId);
			message = _messageRepository.Add(message);
			var userToSendThisMessage = _conversationRepository.AsQueryable()
				.Where(x => x.Id == conversatonId)
				.SelectMany(x => x.Users.Select(u => u.Id))
				.Where(x => x != userId)
				.ToList()
				.Select(x => new UserUnreadMessage(x, message.Id, userId));
			message.UsersWhichDidNotReadMessage = userToSendThisMessage.ToList();
			_messageRepository.SaveOrUpdate();
			return ConvertToConversationMessageViewModel(_messageRepository.AsNoTracking().FirstOrDefault(x => x.Id == message.Id));
		}

		private static IQueryable<Conversation> IsMemberOfConversation(IQueryable<Conversation> query, int userId)
		{
			return query.Where(c => c.Users.Any(u => u.Id == userId) || (c.CreatedByUserId == userId));
		}

		private static ConversationMessageViewModel ConvertToConversationMessageViewModel(Message message)
		{
			return new ConversationMessageViewModel
			{
				Id = message.Id,
				Content = message.Content,
				CreatedByUser = message.CreatedByUser.LastName + " " + message.CreatedByUser.FirstName,
				CreatedDate = message.CreatedDate,
				CreatedByUserId = message.CreatedByUserId
			};
		}

		public string DecryptFromEmoticon(Stream inputStream, string password)
		{
			return _hiddenMessageService.DecryptFromEmoticon(inputStream, password);
		}

	}
}