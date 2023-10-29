using SQLite;

namespace IOManager.Models
{
	[Table("OrderHeader")]
	public class OrderHeaderModel
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		public string OrderNo { get; set; }

		[MaxLength(100)]
		public string CustomerName { get; set; }

		public decimal Total { get; set; }

		public int LinesCount { get; set; }
	}
}
