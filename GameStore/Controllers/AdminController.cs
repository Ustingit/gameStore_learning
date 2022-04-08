﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using GameStore.StoreDomain.Abstract;
using GameStore.StoreDomain.Entities;

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

        public ViewResult Create()
        {
	        return View("Edit", new Game());
        }

        public ViewResult Edit(int gameId)
        {
	        var game = repository.Games.FirstOrDefault(x => x.GameId == gameId);

	        return View(game);
        }

        [HttpPost]
        public ActionResult Edit(Game game)
        {
	        if (ModelState.IsValid)
	        {
                repository.Save(game);
                TempData["message"] = string.Format("Изменения в игре \"{0}\" были сохранены", game.Name);

                return RedirectToAction("Index");
	        }

	        return View(game);
        }

        [HttpPost]
        public ActionResult Delete(int gameId)
        {
	        var game = repository.Delete(gameId);

	        if (game != null)
	        {
                TempData["message"] = string.Format("Игра \"{0}\" была удалена", game.Name);
            }

	        return RedirectToAction("Index");
        }
    }
}