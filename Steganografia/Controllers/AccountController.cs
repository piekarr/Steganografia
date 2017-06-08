using Steganografia.Security;
using Steganografia.Security.Accounts;
using Steganografia.Security.Filters;
using Steganografia.ViewModels.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Steganografia.Controllers
{
	[AllowAnonymous]
	public class AccountController : BaseController
	{
		private readonly IAccountManager _accountManager;

		public AccountController() : this(new AccountManager())
		{
		//web.mck.pk.edu.pl/~aniewiarowski/lab
		//haslo: pai
		}

		public AccountController(IAccountManager accountManager)
		{
			_accountManager = accountManager;
		}

		[HttpGet]
		[RedirectAuthenticatedUsersFilter("Home", "Index")]
		public ActionResult SignIn()
		{
			return View(new SignInUserViewModel());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[RedirectAuthenticatedUsersFilter("Home", "Index")]
		public ActionResult SignIn(SignInUserViewModel signInUser)
		{
			if (ModelState.IsValid)
			{
				if (_accountManager.UserExists(signInUser.UserName, signInUser.Password))
				{
					_accountManager.SignIn(signInUser.UserName, HttpContext);
					return RedirectToAction("Index", "Home");
				}
				else
				{
					ModelState.AddModelError("", "Wrong username or password.");
				}
			}
			return View(signInUser);
		}

		public ActionResult Logout()
		{
			_accountManager.Logout(HttpContext);
			return RedirectToAction(nameof(SignIn));
		}
		[HttpGet]
		[RedirectAuthenticatedUsersFilter("Home", "Index")]
		public ActionResult Register()
		{
			return View(new RegisterUserViewModel());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[RedirectAuthenticatedUsersFilter("Home", "Index")]
		public ActionResult Register(RegisterUserViewModel registerUser)
		{
			ValidateRegisterUserViewModel(registerUser);
			if (ModelState.IsValid)
			{
				_accountManager.CreateAccount(registerUser.UserName, registerUser.Password, registerUser.FirstName, registerUser.LastName);
				_accountManager.SignIn(registerUser.UserName, HttpContext);
				return RedirectToAction("Index", "Home");
			}
			return View(registerUser);
		}

		private void ValidateRegisterUserViewModel(RegisterUserViewModel registerUser)
		{
			if (_accountManager.UserNameTaken(registerUser.UserName))
			{
				ModelState.AddModelError(nameof(RegisterUserViewModel.UserName), "Username is already taken.");
			}
		}
	}
}