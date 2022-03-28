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
	}
}
