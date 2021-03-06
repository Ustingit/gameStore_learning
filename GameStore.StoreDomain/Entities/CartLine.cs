namespace GameStore.StoreDomain.Entities
{
	public class CartLine
	{
		public  Game Game { get; set; }

		public int Quantity { get; set; }

		public decimal LinePrice => Quantity * Game.Price;
	}
}
