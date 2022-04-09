using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using GameStore.Controllers;
using GameStore.Infrastructure.Abstract;
using GameStore.Models.Auth;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GameStore.Tests
{
	[TestClass]
	public class AdminSecurityTests
	{
		[TestMethod]
		public void Can_Login_With_Valid_Credentials()
		{
			//arrange
			var mock = new Mock<IAuthProvider>();
			mock.Setup(x => x.Authenticate("admin", "12345")).Returns(true);

			// Организация - создание модели представления
			// с правильными учетными данными
			var loginModel = new LoginViewModel()
			{
				UserName = "admin",
				Password = "12345"
			};

			var controller = new AccountController(mock.Object);

			//action
			var result = controller.Login(loginModel, "/MyURL");

			//assert
			Assert.IsInstanceOfType(result, typeof(RedirectResult));
			Assert.AreEqual("/MyURL", ((RedirectResult)result).Url);
		}

		[TestMethod]
		public void Cannot_Login_With_Invalid_Credentials()
		{
			//arrange
			var mock = new Mock<IAuthProvider>();
			mock.Setup(x => x.Authenticate("badUser", "badPass")).Returns(false);

			// Организация - создание модели представления
			// с некорректными учетными данными
			LoginViewModel model = new LoginViewModel
			{
				UserName = "badUser",
				Password = "badPass"
			};

			var controller = new AccountController(mock.Object);

			//action
			var result = controller.Login(model, "/MyURL");

			//assert
			Assert.IsInstanceOfType(result, typeof(ViewResult));
			Assert.IsFalse(((ViewResult)result).ViewData.ModelState.IsValid);
		}
	}
}
