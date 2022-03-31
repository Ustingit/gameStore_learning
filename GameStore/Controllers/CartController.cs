using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GameStore.Models;
using GameStore.StoreDomain.Abstract;
using GameStore.StoreDomain.Entities;

namespace GameStore.Controllers
{
    public class CartController : Controller
    {
	    private IGameRepository repository;

	    public CartController(IGameRepository repo)
	    {
		    repository = repo;
	    }

	    public ViewResult Index(string returnUrl)
	    {
		    return View(new CartIndexViewModel()
		    {
				ReturnUrl = returnUrl,
				Cart = GetCart()
		    });
	    }

	    public RedirectToRouteResult AddToCart(int gameId, string returnUrl)
	    {
		    var existGame = repository.Games.FirstOrDefault(g => g.GameId == gameId);

		    if (existGame != null)
		    {
				GetCart().AddItem(existGame, 1);
		    }

		    return RedirectToAction("Index", new {returnUrl});
	    }

	    public RedirectToRouteResult RemoveFromCart(int gameId, string returnUrl)
	    {
		    var existGame = repository.Games.FirstOrDefault(g => g.GameId == gameId);

		    if (existGame != null)
		    {
				GetCart().RemoveLine(existGame);
		    }

		    return RedirectToAction("Index", new { returnUrl });
	    }

	    private Cart GetCart()
	    {
		    var cart = (Cart) Session["Cart"];

		    if (cart != null)
		    {
			    return cart;
		    }

		    var newCart = new Cart();
		    Session["Cart"] = newCart;

		    return new Cart();
	    }
    }
}