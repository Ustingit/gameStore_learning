using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GameStore.Infrastructure.Abstract;
using GameStore.Models.Auth;

namespace GameStore.Controllers
{
    public class AccountController : Controller
    {
	    IAuthProvider authProvider;

	    public AccountController(IAuthProvider authProvider)
	    {
		    this.authProvider = authProvider;
	    }

	    public ViewResult Login()
	    {
		    return View();
	    }

		[HttpPost]
	    public ActionResult Login(LoginViewModel model, string returnUrl)
	    {
		    if (ModelState.IsValid)
		    {
			    if (authProvider.Authenticate(model.UserName, model.Password))
			    {
				    return Redirect(returnUrl ?? Url.Action("Index", "Admin"));
			    }

				ModelState.AddModelError("", "Неправильный логин или пароль");

				return View();
			}
		    else
		    {
			    return View();
		    }
	    }
    }
}