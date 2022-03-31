using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GameStore.Models;
using GameStore.StoreDomain.Abstract;

namespace GameStore.Controllers
{
    public class GameController : Controller
    {
		public int pageSize = 4; //public for testable reasons

	    private IGameRepository repository;

	    public GameController(IGameRepository repo)
	    {
		    repository = repo;
	    }

	    public ViewResult List(string category, int page = 1)
	    {
			var model = new GamesListViewModel()
			{
				CurrentCategory = category,
				Games = category == null 
					? repository.Games.OrderBy(x => x.GameId).Skip((page - 1) * pageSize).Take(pageSize)
					: repository.Games.Where(x => x.Category == null || x.Category == category).OrderBy(x => x.GameId).Skip((page - 1) * pageSize).Take(pageSize),
				PagingInfo = new PagingInfo()
				{
					CurrentPage = page,
					ItemsPerPage = pageSize,
					TotalItems = category == null 
						? repository.Games.Count() 
						: repository.Games.Count(game => game.Category == category)
				}
			};

			return View(model);
	    }
    }
}