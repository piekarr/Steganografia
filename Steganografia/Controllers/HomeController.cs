using Steganografia.Services.Conversations;
using Steganografia.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
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
            return PartialView("ConversationMessages", _conversationService.GetMessagesForConversation(id));
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new CreateConversationViewModel());
        }

        [HttpPost]
        public ActionResult Create(CreateConversationViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (_conversationService.UsersExists(model.UserIds))
                {
                    _conversationService.CreateConversation(model.Name, model.UserIds, User.Identity.Id);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Providen users do not exists.");
                }
            }
            return View(model);
        }
    }
}