using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GameStore.StoreDomain.Abstract;
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
			Mock<IGameRepository> mock = new Mock<IGameRepository>();
			mock.Setup(x => x.Games).Returns(new List<Game>()
			{
				new Game() { Name = "SimCity", Price = 1499 },
				new Game() { Name = "TITANFALL", Price = 2299},
				new Game() { Name = "Battlefield 4", Price = 889.4M } 
			});
			kernel.Bind<IGameRepository>().ToConstant(mock.Object);
		}
	}
}