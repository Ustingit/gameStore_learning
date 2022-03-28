using System;
using System.Collections.Generic;
using System.Data.Entity;
using GameStore.StoreDomain.Entities;

namespace GameStore.StoreDomain.Concrete
{
	public class EFDbContext : DbContext
	{
		public DbSet<Game> Games { get; set; }
	}
}
