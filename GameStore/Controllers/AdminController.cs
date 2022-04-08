using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GameStore.StoreDomain.Abstract;

namespace GameStore.Controllers
{
    public class AdminController : Controller
    {
	    private IGameRepository repository;

	    public AdminController(IGameRepository repo)
	    {
		    repository = repo;
	    }

        // GET: Admin
        public ViewResult Index()
        {
            return View(repository.Games);
        }

        public ViewResult Edit(int gameId)
        {
	        var game = repository.Games.FirstOrDefault(x => x.GameId == gameId);

	        return View(game);
        }
    }
}