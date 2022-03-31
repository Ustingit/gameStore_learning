﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.StoreDomain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameStore.Tests
{
	[TestClass]
	public class CartTests
	{
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
	}
}
