using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using GameStore.Infrastructure.Binders;
using GameStore.StoreDomain.Entities;

namespace GameStore
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            ModelBinders.Binders.Add(typeof(Cart), new CartModelBinder());
        }
    }
}
