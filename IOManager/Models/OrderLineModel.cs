using SQLite;

namespace IOManager.Models
{
	[Table("OrderLine")]
	public class OrderLineModel
	{
		public int OrderId { get; set; }

		public string ItemName { get; set; }

		public decimal Price { get; set; }

		public int Qty { get; set; }

		public decimal Total => Price * Qty;
	}
}
