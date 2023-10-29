using SQLite;

namespace IOManager.Models
{
	[Table("Item")]
	public class ItemModel
	{
		[PrimaryKey]
		public Guid Id { get; set; } = Guid.NewGuid();

		[MaxLength(100), Unique]
		public string ItemName { get; set; }

		public decimal WholeSalePrice { get; set; }

		public decimal RetailSalePrice { get; set; }

		public decimal? PurchasePrice { get; set; }

		public int? StockQuantity { get; set; }

		public string Description { get; set; }

		public string ImagePath { get; set; }
	}
}
