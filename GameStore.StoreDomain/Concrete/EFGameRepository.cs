using System;
using System.Collections.Generic;
using GameStore.StoreDomain.Abstract;
using GameStore.StoreDomain.Entities;

namespace GameStore.StoreDomain.Concrete
{
	public class EFGameRepository : IGameRepository
	{
		EFDbContext context = new EFDbContext();

		public IEnumerable<Game> Games
		{
			get { return context.Games; }
		}

		public void Save(Game game)
		{
			if (game.GameId == 0)
			{
				context.Games.Add(game);
			}
			else
			{
				var dbEntry = context.Games.Find(game.GameId);

				if (dbEntry != null)
				{
					dbEntry.Name = game.Name;
					dbEntry.Description = game.Description;
					dbEntry.Price = game.Price;
					dbEntry.Category = game.Category;
				}
			}

			context.SaveChanges();
		}
	}
}
