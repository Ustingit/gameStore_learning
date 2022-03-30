using System;
using System.Collections.Generic;
using System.Linq;
using GameStore.Controllers;
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
		public void Can_Paginate_Games()
		{
			// arrange
			Mock<IGameRepository> mock = new Mock<IGameRepository>();
			mock.Setup(x => x.Games).Returns(new List<Game>()
			{
				new Game { GameId = 1, Name = "Игра1"},
				new Game { GameId = 2, Name = "Игра2"},
				new Game { GameId = 3, Name = "Игра3"},
				new Game { GameId = 4, Name = "Игра4"},
				new Game { GameId = 5, Name = "Игра5"}
			});

			GameController controller = new GameController(mock.Object);
			controller.pageSize = 3;

			// act
			IEnumerable<Game> result = (IEnumerable<Game>) controller.List(2).Model;

			// assert
			List<Game> games = result.ToList();

			Assert.IsTrue(games.Count == 2);
			Assert.AreEqual(games[0].Name, "Игра4");
			Assert.AreEqual(games[1].Name, "Игра5");
		}
	}
}
