using System;
using System.Threading.Tasks;
using GameStore.StoreDomain.Entities;

namespace GameStore.StoreDomain.Abstract
{
	public interface IOrderProcessor
	{
		void ProcessOrder(Cart cart, ShippingDetails details);
	}
}
