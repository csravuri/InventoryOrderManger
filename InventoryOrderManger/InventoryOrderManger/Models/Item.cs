using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace InventoryOrderManger.Models
{
    public class Item : BaseModel
    {
        [PrimaryKey, AutoIncrement]
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public decimal SellPrice { get; set; }
        public string Description { get; set; }
        public decimal StockQty { get; set; }
        public decimal PurchasePrice { get; set; }
        public string ImagePath { get; set; }

    }
}
