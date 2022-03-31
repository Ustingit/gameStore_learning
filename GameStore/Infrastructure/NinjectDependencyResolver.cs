using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GameStore.StoreDomain.Abstract;
using GameStore.StoreDomain.Concrete;
using GameStore.StoreDomain.Concrete.Order;
using GameStore.StoreDomain.Entities;
using Moq;
using Ninject;

namespace GameStore.Infrastructure
{
	public class NinjectDependencyResolver : IDependencyResolver
	{
		private IKernel kernel;

		public NinjectDependencyResolver(IKernel kernelParams)
		{
			kernel = kernelParams;
			AddBindings();
		}

		public object GetService(Type serviceType)
		{
			return kernel.TryGet(serviceType);
		}

		public IEnumerable<object> GetServices(Type serviceType)
		{
			return kernel.GetAll(serviceType);
		}
		
		private void AddBindings()
		{
			kernel.Bind<IGameRepository>().To<EFGameRepository>();

			var emailSetting = new EmailSettings()
			{
				WriteAsFile = ConfigurationManager.AppSettings["Email.WriteAsFile"] != null && bool.Parse(ConfigurationManager.AppSettings["Email.WriteAsFile"])
			};

			kernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>().WithConstructorArgument("settings", emailSetting);
		}
	}
}