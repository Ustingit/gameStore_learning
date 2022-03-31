using System;
using GameStore.StoreDomain.Entities;

namespace GameStore.Models
{
	public class CartIndexViewModel
	{
		public Cart Cart { get; set; }
		
		public string ReturnUrl { get; set; }
	}
}