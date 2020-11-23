using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace InventoryOrderManger.Models
{
    public class OrderLine : BaseModel
    {
        public int OrderID { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public decimal ItemSellPrice { get; set; }
        public decimal ItemOrderQty { get; set; }
        public decimal ItemTotalPrice { get; set; }
    }
}
