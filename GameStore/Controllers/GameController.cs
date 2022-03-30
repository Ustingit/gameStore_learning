using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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

	    public ViewResult List(int page = 1)
	    {
		    var data = repository.Games.OrderBy(x => x.GameId).Skip((page - 1) * pageSize).Take(pageSize);

			return View(data);
	    }
    }
}