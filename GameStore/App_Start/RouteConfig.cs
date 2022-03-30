﻿using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace GameStore
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
				name: null,
                url: "Page{page}",
                defaults: new { controller = "Game", action = "List" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Game", action = "List", id = UrlParameter.Optional }
            );
        }
    }
}
