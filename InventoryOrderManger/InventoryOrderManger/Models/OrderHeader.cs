using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace InventoryOrderManger.Models
{
    public class OrderHeader : BaseModel
    {
        [PrimaryKey, AutoIncrement]
        public int OrderID { get; set; }
        public string OrderNo { get; set; }
        public string CustomerName { get; set; }
        public decimal OrderTotalPrice { get; set; }


    }
}
