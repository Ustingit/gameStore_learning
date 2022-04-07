using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
