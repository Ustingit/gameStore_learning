using System;
using System.Collections.Generic;
using System.Linq;

namespace GameStore.StoreDomain.Entities
{
	public class Cart
	{
		private List<CartLine> lineCollection = new List<CartLine>();

		public void AddItem(Game game, int quantity)
		{
			var existLine = lineCollection.FirstOrDefault(x => x.Game.GameId == game.GameId);

			if (existLine != null)
			{
				existLine.Quantity += quantity;

				return;
			}

			var newLine = new CartLine()
			{
				Game = game,
				Quantity = quantity
			};
			lineCollection.Add(newLine);
		}

		public void RemoveLine(Game game)
		{
			lineCollection.RemoveAll(x => x.Game.GameId == game.GameId);
		}

		public decimal ComputeTotalValue()
		{
			return lineCollection.Sum(l => l.Game.Price * l.Quantity);
		}

		public void Clear()
		{
			lineCollection.Clear();
		}

		public IEnumerable<CartLine> Lines => lineCollection;
	}
}
