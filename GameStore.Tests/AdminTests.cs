using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using GameStore.Controllers;
using GameStore.StoreDomain.Abstract;
using GameStore.StoreDomain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GameStore.Tests
{
	[TestClass]
	public class AdminTests
	{
		[TestMethod]
		public void Can_Delete_Valid_Games()
		{
			//arrange
			var game = new Game() { GameId = 2, Name = "Игра2" };
			var mock = new Mock<IGameRepository>();
			mock.Setup(x => x.Games).Returns(new List<Game>()
			{
				new Game { GameId = 1, Name = "Игра1"},
				new Game { GameId = 2, Name = "Игра2"},
				new Game { GameId = 3, Name = "Игра3"},
				new Game { GameId = 4, Name = "Игра4"},
				new Game { GameId = 5, Name = "Игра5"}
			});

			var controller = new AdminController(mock.Object);

			//action
			controller.Delete(game.GameId);

			//assert
			// Утверждение - проверка того, что метод удаления в хранилище
			// вызывается для корректного объекта Game
			mock.Verify(x => x.Delete(game.GameId));
		}

		[TestMethod]
		public void Cannot_Save_Invalid_Changes()
		{
			//arrange
			var mock = new Mock<IGameRepository>();
			var controller = new AdminController(mock.Object);
			var game = new Game() { Name = "Test" };
			
			controller.ModelState.AddModelError("error", "error");

			//action
			var result = controller.Edit(game);

			//assert
			// Утверждение - проверка того, что обращение к хранилищу НЕ производится 
			mock.Verify(x => x.Save(It.IsAny<Game>()), Times.Never());

			// Утверждение - проверка типа результата метода
			Assert.IsInstanceOfType(result, typeof(ViewResult));
		}

		[TestMethod]
		public void Can_Save_Valid_Changes()
		{
			//arrange
			var mock = new Mock<IGameRepository>();
			var controller = new AdminController(mock.Object);
			var game = new Game() { Name = "Test" };

			//action
			var result = controller.Edit(game);

			//assert
			// Утверждение - проверка того, что к хранилищу производится обращение
			mock.Verify(x => x.Save(game));

			// Утверждение - проверка типа результата метода
			Assert.IsNotInstanceOfType(result, typeof(ViewResult));
		}

		[TestMethod]
		public void Can_Edit_Game()
		{
			//arrange
			var mock = new Mock<IGameRepository>();
			mock.Setup(x => x.Games).Returns(new List<Game>()
			{
				new Game {GameId = 1, Name = "Игра1"},
				new Game {GameId = 2, Name = "Игра2"},
				new Game {GameId = 3, Name = "Игра3"},
				new Game {GameId = 4, Name = "Игра4"},
				new Game {GameId = 5, Name = "Игра5"}
			});

			var controller = new AdminController(mock.Object);

			//action
			var game1 = controller.Edit(1).ViewData.Model as Game;
			var game2 = controller.Edit(2).ViewData.Model as Game;
			var game3 = controller.Edit(3).ViewData.Model as Game;

			//assert
			Assert.AreEqual(1, game1.GameId);
			Assert.AreEqual(2, game2.GameId);
			Assert.AreEqual(3, game3.GameId);
		}

		[TestMethod]
		public void Cannot_Edit_Nonexistent_Game()
		{
			var mock = new Mock<IGameRepository>();
			mock.Setup(x => x.Games).Returns(new List<Game>()
			{
				new Game { GameId = 1, Name = "Игра1"},
				new Game { GameId = 2, Name = "Игра2"},
				new Game { GameId = 3, Name = "Игра3"},
				new Game { GameId = 4, Name = "Игра4"},
				new Game { GameId = 5, Name = "Игра5"}
			});

			var controller = new AdminController(mock.Object);

			//action
			var game = controller.Edit(6).ViewData.Model as Game;

			//assert
			Assert.IsNull(game);
		}

		[TestMethod]
		public void Index_Contains_All_Games()
		{
			//arrange 
			var mock = new Mock<IGameRepository>();
			mock.Setup(x => x.Games).Returns(new List<Game>()
			{
				new Game { GameId = 1, Name = "Игра1"},
				new Game { GameId = 2, Name = "Игра2"},
				new Game { GameId = 3, Name = "Игра3"},
				new Game { GameId = 4, Name = "Игра4"},
				new Game { GameId = 5, Name = "Игра5"}
			});

			var controller = new AdminController(mock.Object);

			//action
			var result = ((IEnumerable<Game>) controller.Index().ViewData.Model).ToList();

			//assert
			Assert.AreEqual(result.Count(), 5);
			Assert.AreEqual("Игра1", result[0].Name);
			Assert.AreEqual("Игра2", result[1].Name);
			Assert.AreEqual("Игра3", result[2].Name);
		}
	}
}
