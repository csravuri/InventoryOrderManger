namespace InventoryOrderManger.Models
{
    public class Item : BaseModel
    {
        public string ItemName { get; set; }
        public decimal WholeSalePrice { get; set; }
        public decimal RetailSalePrice { get; set; }
        public decimal CustomerSellingPrice { get; set; }
        public string Description { get; set; }
        public decimal StockQty { get; set; }
        public decimal PurchasePrice { get; set; }
        public string ImagePath { get; set; }
    }
}