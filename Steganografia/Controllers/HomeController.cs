using Steganografia.Services.Conversations;
using Steganografia.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Steganografia.Controllers
{
	public class HomeController : BaseController
	{
		private readonly IConversationService _conversationService;

		public HomeController() : this(new ConversationService())
		{

		}

		public HomeController(IConversationService conversationService)
		{
			_conversationService = conversationService;
		}

		[HttpGet]
		public ActionResult Index()
		{
			return View(_conversationService.GetConversationForUser(User.Identity.Id));
		}

		[HttpGet]
		public ActionResult Messages(int id)
		{
			if (!_conversationService.UserIsAMemberOfConversation(id, User.Identity.Id))
			{
				return new HttpNotFoundResult();
			}
			ViewBag.ConversatonId = id;
			return PartialView("ConversationMessages", _conversationService.GetMessagesForConversation(id));
		}

		[HttpGet]
		public ActionResult NewMessages(int id, int? lastMessageId)
		{
			if (!_conversationService.UserIsAMemberOfConversation(id, User.Identity.Id))
			{
				return new HttpNotFoundResult();
			}
			return PartialView("ConversationMessagesList", _conversationService.GetNewestMessages(id, lastMessageId));
		}

		[HttpGet]
		public ActionResult Create()
		{
			ViewData["UsersSelectListItems"] = _conversationService.GetAllUsersExceptAsSelectListItems(User.Identity.Id);
			return View(new CreateConversationViewModel());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(CreateConversationViewModel model)
		{
			ValidateCreateConversationViewModel(model);

			if (ModelState.IsValid)
			{
				_conversationService.CreateConversation(model.Name, model.UserIds, User.Identity.Id);
				return RedirectToAction(nameof(Index));
			}
			ViewData.Add("UsersSelectListItems", _conversationService.GetAllUsersExceptAsSelectListItems(User.Identity.Id));
			return View(model);
		}

		[HttpGet]
		public ActionResult CreateMessage(int id)
		{
			return PartialView(new CreateMessageViewModel { ConversatonId = id });
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult CreateMessage(CreateMessageViewModel createMessageViewModel)
		{
			//Validate, if users belongs to conversation
			if (ModelState.IsValid)
			{
				var message = _conversationService.CreateMessage(createMessageViewModel.ConversatonId, createMessageViewModel.Message, User.Identity.Id);
				return PartialView("ConversationMessage", message);
			}
			Response.StatusCode = (int)HttpStatusCode.BadRequest;
			return PartialView(createMessageViewModel);
		}

		[HttpPost]
		public string Decrypt(string DecryptionPassword, HttpPostedFileBase Emoticon)
		{

			string result = _conversationService.DecryptFromEmoticon(Emoticon.InputStream, DecryptionPassword);
			return result;
		}

		private void ValidateCreateConversationViewModel(CreateConversationViewModel model)
		{
			if (model.UserIds == null)
			{
				return;
			}
			if (model.UserIds.Contains(User.Identity.Id))
			{
				ModelState.AddModelError(nameof(CreateConversationViewModel.UserIds), "You can not talk with yourself... 4ever alone :(");
			}

			if (!model.UserIds.Any())
			{
				ModelState.AddModelError(nameof(CreateConversationViewModel.UserIds), "You have to provide at least one user.");
			}

			if (!_conversationService.UsersExists(model.UserIds))
			{
				ModelState.AddModelError(nameof(CreateConversationViewModel.UserIds), "Providen users do not exists.");
			}

			if (_conversationService.ConversationForUserGroupExists(model.UserIds, User.Identity.Id))
			{
				ModelState.AddModelError(nameof(CreateConversationViewModel.UserIds), "Conversation for this group of users already exists.");
			}
		}
	}
}