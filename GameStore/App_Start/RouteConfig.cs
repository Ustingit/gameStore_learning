using System;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject.Infrastructure.Language;

namespace GameStore
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(null,
	            "",
	            new
	            {
		            controller = "Game",
		            action = "List",
		            category = (string)null,
		            page = 1
	            }
            );

            routes.MapRoute(
	            name: null,
	            url: "Page{page}",
	            defaults: new { controller = "Game", action = "List", category = (string)null },
	            constraints: new { page = @"\d+" }
            );

            routes.MapRoute(null,
	            "{category}",
	            new { controller = "Game", action = "List", page = 1 }
            );

            routes.MapRoute(null,
	            "{category}/Page{page}",
	            new { controller = "Game", action = "List" },
	            new { page = @"\d+" }
            );

            routes.MapRoute(null, "{controller}/{action}");
        }
    }
}
