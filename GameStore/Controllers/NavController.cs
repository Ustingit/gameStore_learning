using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GameStore.StoreDomain.Abstract;

namespace GameStore.Controllers
{
    public class NavController : Controller
    {
	    private IGameRepository repository;

	    public NavController(IGameRepository repo)
	    {
		    repository = repo;
	    }

	    public PartialViewResult Menu(string category = null)
	    {
		    ViewBag.SelectedCategory = category;

		    var categories = repository.Games.Select(g => g.Category).Distinct().OrderBy(c => c);

			return PartialView("FlexMenu", categories);
	    }
    }
}