﻿using System;
using System.Collections.Generic;
using GameStore.StoreDomain.Entities;

namespace GameStore.Models
{
	public class GamesListViewModel
	{
		public IEnumerable<Game> Games { get; set; }

		public PagingInfo PagingInfo { get; set; }
	}
}