using System;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
	public class AccountController : Controller
    {
		public AccountController()
		{

		}

        public IActionResult AccessDenied(string? ReturnUrl)
        {
            return View();
        }
    }
}

