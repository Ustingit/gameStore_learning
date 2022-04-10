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
	public class ImageTests
	{
		[TestMethod]
		public void Can_Retrieve_Image_Data()
		{
			//arrange
			var game = new Game
			{
				GameId = 2,
				Name = "Игра2",
				ImageData = new byte[] { },
				ImageMimeType = "image/png"
			};

			var mock = new Mock<IGameRepository>();
			mock.Setup(x => x.Games).Returns(new List<Game>()
			{
				new Game {GameId = 1, Name = "Игра1"},
				game,
				new Game {GameId = 3, Name = "Игра3"}
			}.AsQueryable());

			var controller = new GameController(mock.Object);

			//action
			var result = controller.GetImage(game.GameId);

			//assert
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(FileResult));
			Assert.AreEqual(game.ImageMimeType, ((FileResult)result).ContentType);
		}

		[TestMethod]
		public void Cannot_Retrieve_Image_Data_For_Invalid_ID()
		{
			//arrange
			Mock<IGameRepository> mock = new Mock<IGameRepository>();
			mock.Setup(m => m.Games).Returns(new List<Game> {
				new Game {GameId = 1, Name = "Игра1"},
				new Game {GameId = 2, Name = "Игра2"}
			}.AsQueryable());
			GameController controller = new GameController(mock.Object);

			//action
			ActionResult result = controller.GetImage(10);

			//assert
			Assert.IsNull(result);
		}
	}
}
