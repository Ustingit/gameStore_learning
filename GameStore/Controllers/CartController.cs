using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls.WebParts;
using GameStore.Models;
using GameStore.StoreDomain.Abstract;
using GameStore.StoreDomain.Entities;

namespace GameStore.Controllers
{
	/// <summary>
	/// Note about session: Нам нравится использовать средство состояния сеанса в контроллере Cart для хранения и управления объектами Cart, созданными в предыдущей статье, но нас не устраивает способ, которым это должно делаться. Он не вписывается в остальные части модели приложения, которые основаны на параметрах методов действий. Мы не можем провести исчерпывающее модульное тестирование класса CartController до тех пор, пока не построим имитацию параметра Session базового класса, а это означает имитацию класса Controller и множество других вещей, с которыми лучше не связываться.
	/// </summary>
	/// <returns></returns>
	public class CartController : Controller
    {
	    private IGameRepository repository;
	    private IOrderProcessor orderProcessor;

	    public CartController(IGameRepository repo, IOrderProcessor orderProcessor)
	    {
		    repository = repo;
		    this.orderProcessor = orderProcessor;
	    }

	    public ViewResult Index(Cart cart, string returnUrl)
	    {
		    return View(new CartIndexViewModel()
		    {
				ReturnUrl = returnUrl,
				Cart = cart
		    });
	    }

	    public ViewResult Checkout(Cart cart)
	    {
		    return View(new ShippingDetails());
	    }

		[HttpPost]
	    public ViewResult Checkout(Cart cart, ShippingDetails shippingDetails)
	    {
		    if (cart.Lines.Count() == 0)
		    {
				ModelState.AddModelError("", "Извините, ваша корзина пуста!");
		    }

		    if (ModelState.IsValid)
		    {
				orderProcessor.ProcessOrder(cart, shippingDetails);
				cart.Clear();

				return View("Completed");
		    }
		    else
		    {
			    return View(shippingDetails);
		    }
	    }

	    public PartialViewResult Summary(Cart cart)
	    {
		    return PartialView(cart);
	    }

	    public RedirectToRouteResult AddToCart(Cart cart, int gameId, string returnUrl)
	    {
		    var existGame = repository.Games.FirstOrDefault(g => g.GameId == gameId);

		    if (existGame != null)
		    {
				cart.AddItem(existGame, 1);
		    }

		    return RedirectToAction("Index", new {returnUrl});
	    }

	    public RedirectToRouteResult RemoveFromCart(Cart cart, int gameId, string returnUrl)
	    {
		    var existGame = repository.Games.FirstOrDefault(g => g.GameId == gameId);

		    if (existGame != null)
		    {
			    cart.RemoveLine(existGame);
		    }

		    return RedirectToAction("Index", new { returnUrl });
	    }
    }
}