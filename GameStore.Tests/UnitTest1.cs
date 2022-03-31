using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GameStore.Controllers;
using GameStore.HtmlHelpers;
using GameStore.Models;
using GameStore.StoreDomain.Abstract;
using GameStore.StoreDomain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GameStore.Tests
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void Generate_Category_Specific_Game_Count()
		{
			// arrange
			var mock = new Mock<IGameRepository>();
			mock.Setup(x => x.Games).Returns(new List<Game>()
			{
				new Game { GameId = 1, Name = "Игра1", Category="Cat1"},
				new Game { GameId = 2, Name = "Игра2", Category="Cat2"},
				new Game { GameId = 3, Name = "Игра3", Category="Cat1"},
				new Game { GameId = 4, Name = "Игра4", Category="Cat2"},
				new Game { GameId = 5, Name = "Игра5", Category="Cat3"}
			});

			var controller = new GameController(mock.Object) {pageSize = 3};

			//action
			var res1 = ((GamesListViewModel)controller.List("Cat1").Model).PagingInfo.TotalItems;
			var res2 = ((GamesListViewModel)controller.List("Cat2").Model).PagingInfo.TotalItems;
			var res3 = ((GamesListViewModel)controller.List("Cat3").Model).PagingInfo.TotalItems;
			var resAll = ((GamesListViewModel) controller.List(null).Model).PagingInfo.TotalItems;

			//assert
			Assert.AreEqual(res1, 2);
			Assert.AreEqual(res2, 2);
			Assert.AreEqual(res3, 1);
			Assert.AreEqual(resAll, 5);
		}

		[TestMethod]
		public void Indicates_Selected_Category()
		{
			//arrange
			var mock = new Mock<IGameRepository>();
			mock.Setup(x => x.Games).Returns(new List<Game>()
			{
				new Game {GameId = 1, Name = "Игра1", Category = "Симулятор"},
				new Game {GameId = 2, Name = "Игра2", Category = "Шутер"}
			});

			var controller = new NavController(mock.Object);
			string categoryToSelect = "Шутер";

			//action
			var result = controller.Menu(categoryToSelect).ViewBag.SelectedCategory;

			//assert
			Assert.AreEqual(result, categoryToSelect);
		}

		[TestMethod]
		public void Can_Create_Categories()
		{
			// arrange
			var mock = new Mock<IGameRepository>();
			mock.Setup(x => x.Games).Returns(new List<Game>()
			{
				new Game { GameId = 1, Name = "Игра1", Category="Симулятор"},
				new Game { GameId = 2, Name = "Игра2", Category="Симулятор"},
				new Game { GameId = 3, Name = "Игра3", Category="Шутер"},
				new Game { GameId = 4, Name = "Игра4", Category="RPG"},
			});

			var controller = new NavController(mock.Object);

			// action
			List<string> result = ((IEnumerable<string>)controller.Menu().Model).ToList();

			// assert
			Assert.AreEqual(result.Count(), 3);
			Assert.AreEqual(result[0], "RPG");
			Assert.AreEqual(result[1], "Симулятор");
			Assert.AreEqual(result[2], "Шутер");
		}

		[TestMethod]
		public void Can_Filter_Games()
		{
			// arrange
			var mock = new Mock<IGameRepository>();
			mock.Setup(x => x.Games).Returns(new List<Game>()
			{
				new Game { GameId = 1, Name = "Игра1", Category="Cat1"},
				new Game { GameId = 2, Name = "Игра2", Category="Cat2"},
				new Game { GameId = 3, Name = "Игра3", Category="Cat1"},
				new Game { GameId = 4, Name = "Игра4", Category="Cat2"},
				new Game { GameId = 5, Name = "Игра5", Category="Cat3"}
			});

			var controller = new GameController(mock.Object) {pageSize = 3};

			// action
			var result = ((GamesListViewModel)controller.List("Cat2", 1).Model).Games.ToList();

			// assert
			Assert.AreEqual(result.Count(), 2);
			Assert.IsTrue(result[0].Name == "Игра2" && result[0].Category == "Cat2");
			Assert.IsTrue(result[1].Name == "Игра4" && result[1].Category == "Cat2");
		}

		[TestMethod]
		public void Can_Send_Pagination_View_Model()
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

			var controller = new GameController(mock.Object) {pageSize = 3};

			//action 
			var result = (GamesListViewModel)controller.List(null, 2).Model;

			//assert
			var pageInfo = result.PagingInfo;

			Assert.AreEqual(pageInfo.CurrentPage, 2);
			Assert.AreEqual(pageInfo.ItemsPerPage, 3);
			Assert.AreEqual(pageInfo.TotalItems, 5);
			Assert.AreEqual(pageInfo.TotalPages, 2);
		}

		[TestMethod]
		public void Car_Create_Page_Links()
		{
			// Организация - определение вспомогательного метода HTML - это необходимо
			// для применения расширяющего метода
			HtmlHelper helper = null;

			// arrange
			var pagingInfo = new PagingInfo()
			{
				CurrentPage = 2,
				TotalItems = 28,
				ItemsPerPage = 10
			};

			Func<int, string> pageUrlDelegate = i => "Page" + i;

			//action
			MvcHtmlString result = helper.PageLinks(pagingInfo, pageUrlDelegate);

			//assert
			Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
			                + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
			                + @"<a class=""btn btn-default"" href=""Page3"">3</a>", result.ToString());
		}

		[TestMethod]
		public void Can_Paginate_Games()
		{
			// (arrange)
			Mock<IGameRepository> mock = new Mock<IGameRepository>();
			mock.Setup(m => m.Games).Returns(new List<Game>
			{
				new Game { GameId = 1, Name = "Игра1"},
				new Game { GameId = 2, Name = "Игра2"},
				new Game { GameId = 3, Name = "Игра3"},
				new Game { GameId = 4, Name = "Игра4"},
				new Game { GameId = 5, Name = "Игра5"}
			});
			GameController controller = new GameController(mock.Object);
			controller.pageSize = 3;

			// (act)
			GamesListViewModel result = (GamesListViewModel)controller.List(null, 2).Model;

			// assert
			List<Game> games = result.Games.ToList();
			Assert.IsTrue(games.Count == 2);
			Assert.AreEqual(games[0].Name, "Игра4");
			Assert.AreEqual(games[1].Name, "Игра5");
		}
	}
}
