using System;
using System.Collections.Generic;
using GameStore.StoreDomain.Entities;

namespace GameStore.StoreDomain.Abstract
{
	public interface IGameRepository
	{
		IEnumerable<Game> Games { get; }

		void Save(Game game);
	}
}
