using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using GameStore.Controllers;
using GameStore.Models;
using GameStore.StoreDomain.Abstract;
using GameStore.StoreDomain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GameStore.Tests
{
	[TestClass]
	public class CartTests
	{
		[TestMethod]
		public void Cannot_Checkout_Invalid_ShippingDetails()
		{
			//arrange
			var mock = new Mock<IOrderProcessor>();

			Cart cart = new Cart();
			cart.AddItem(new Game(), 1);

			CartController controller = new CartController(null, mock.Object);

			// Организация — добавление ошибки в модель
			controller.ModelState.AddModelError("error", "error");

			//action
			var result = controller.Checkout(cart, new ShippingDetails());

			//assert

			//проверка, что заказ не передается обработчику
			mock.Verify(x => x.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Never());

			//проверка, что метод вернул стандартное представление
			Assert.AreEqual("", result.ViewName);

			//проверка, что-представлению передана неверная модель
			Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
		}

		[TestMethod]
		public void Cannot_Checkout_Empty_Cart()
		{
			//arrange
			var orderProcessorMock = new Mock<IOrderProcessor>();
			var cart = new Cart();
			var shippingDetails = new ShippingDetails();
			var controller = new CartController(null, orderProcessorMock.Object);

			//action
			var result = controller.Checkout(cart, shippingDetails);

			//assert

			// проверка, что заказ не был передан обработчику 
			orderProcessorMock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Never());

			//проверка, что метод вернул стандартное представление 
			Assert.AreEqual("", result.ViewName);

			//проверка, что-представлению передана неверная модель
			Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
		}

		[TestMethod]
		public void Can_Checkout_And_Submit_Order()
		{
			//arrange
			var mock = new Mock<IOrderProcessor>();

			Cart cart = new Cart();
			cart.AddItem(new Game(), 1);

			var controller = new CartController(null, mock.Object);

			//action
			var result = controller.Checkout(cart, new ShippingDetails());

			//assert

			//проверка, что заказ передан обработчику
			mock.Verify(x => x.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Once());

			//проверка, что метод возвращает представление 
			Assert.AreEqual("Completed", result.ViewName);

			//проверка, что представлению передается допустимая модель
			Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
		}

		[TestMethod]
		public void Can_Add_New_Lines()
		{
			//arrange
			var game1 = new Game { GameId = 1, Name = "Игра1" };
			var game2 = new Game { GameId = 2, Name = "Игра2" };

			//action
			var cart = new Cart();
			cart.AddItem(game1, 1);
			cart.AddItem(game2, 1);

			var result = cart.Lines.ToList();

			//assert
			Assert.AreEqual(result.Count(), 2);
			Assert.AreEqual(result[0].Game, game1);
			Assert.AreEqual(result[1].Game, game2);
		}

		[TestMethod]
		public void Can_Add_Quantity_For_Existing_Lines()
		{
			//arrange
			var cart = new Cart();

			var game1 = new Game { GameId = 1, Name = "Игра1" };
			var game2 = new Game { GameId = 2, Name = "Игра2" };

			//action
			cart.AddItem(game1, 1);
			cart.AddItem(game2, 1);
			cart.AddItem(game1, 5);

			var result = cart.Lines.OrderBy(x => x.Game.GameId).ToList();

			//assert
			Assert.AreEqual(result.Count(), 2);
			Assert.AreEqual(result[0].Quantity, 6);
			Assert.AreEqual(result[1].Quantity, 1);
		}

		[TestMethod]
		public void Can_Remove_Line()
		{
			//arrange
			Game game1 = new Game { GameId = 1, Name = "Игра1" };
			Game game2 = new Game { GameId = 2, Name = "Игра2" };
			Game game3 = new Game { GameId = 3, Name = "Игра3" };

			Cart cart = new Cart();
			cart.AddItem(game1, 1);
			cart.AddItem(game2, 4);
			cart.AddItem(game3, 2);
			cart.AddItem(game2, 1);

			//action
			cart.RemoveLine(game2);

			//assert
			Assert.AreEqual(cart.Lines.Count(c => c.Game == game2), 0);
			Assert.AreEqual(cart.Lines.Count(), 2);
		}

		[TestMethod]
		public void Calculate_Cart_Total()
		{
			//arrange
			Game game1 = new Game { GameId = 1, Name = "Игра1", Price = 100 };
			Game game2 = new Game { GameId = 2, Name = "Игра2", Price = 55 };

			Cart cart = new Cart();

			//action
			cart.AddItem(game1, 1);
			cart.AddItem(game2, 1);
			cart.AddItem(game1, 5);

			var result = cart.ComputeTotalValue();

			//assert
			Assert.AreEqual(result, 655);
		}

		[TestMethod]
		public void Can_Clear_Contents()
		{
			//arrange
			Game game1 = new Game { GameId = 1, Name = "Игра1", Price = 100 };
			Game game2 = new Game { GameId = 2, Name = "Игра2", Price = 55 };

			Cart cart = new Cart();

			//action
			cart.AddItem(game1, 1);
			cart.AddItem(game2, 1);
			cart.AddItem(game1, 5);
			cart.Clear();

			//assert
			Assert.AreEqual(cart.Lines.Count(), 0, "The cart was not cleared");
		}

		/// <summary>
		/// Проверяем добавление в корзину
		/// </summary>
		[TestMethod]
		public void Can_Add_To_Cart()
		{
			//arrange
			var mock = new Mock<IGameRepository>();
			mock.Setup(x => x.Games).Returns(new List<Game>()
			{
				new Game {GameId = 1, Name = "Игра1", Category = "Кат1"},
			}.AsQueryable());

			var cart = new Cart();

			var controller = new CartController(mock.Object, null);

			//action
			controller.AddToCart(cart, 1, null);

			//assert
			Assert.AreEqual(cart.Lines.Count(), 1);
			Assert.AreEqual(cart.Lines.ToList()[0].Game.GameId, 1);
		}

		/// <summary>
		/// После добавления игры в корзину, должно быть перенаправление на страницу корзины
		/// </summary>
		[TestMethod]
		public void Adding_Game_To_Cart_Goes_To_Cart_Screen()
		{
			//arrange
			var mock = new Mock<IGameRepository>();
			mock.Setup(x => x.Games).Returns(new List<Game>()
			{
				new Game {GameId = 1, Name = "Игра1", Category = "Кат1"},
			}.AsQueryable());

			var cart = new Cart();
			var controller = new CartController(mock.Object, null);

			//action
			RedirectToRouteResult result = controller.AddToCart(cart, 2, "myUrl");

			//assert
			Assert.AreEqual(result.RouteValues["action"], "Index");
			Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
		}

		[TestMethod]
		public void Can_View_Cart_Contents()
		{
			//arrange
			Cart cart = new Cart();
			CartController target = new CartController(null, null);

			//action
			var result = (CartIndexViewModel) target.Index(cart, "myUrl").ViewData.Model;

			//assert
			Assert.AreSame(result.Cart, cart);
			Assert.AreEqual(result.ReturnUrl, "myUrl");
		}
	}
}
