using System.ComponentModel;
using SQLite;

namespace InventoryOrderManger.Models
{
    public class OrderLine : BaseModel, INotifyPropertyChanged
    {
        public new event PropertyChangedEventHandler PropertyChanged;    
        public int OrderID { get; set; }

        [PrimaryKey, AutoIncrement]
        public int OrderLineID { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public decimal ItemSellPrice { get; set; }
        public decimal ItemOrderQty { get; set; }

        private decimal _itemTotalPrice;
        public decimal ItemTotalPrice 
        {
            get
            {
                return _itemTotalPrice;
            }
            set
            {
                _itemTotalPrice = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ItemTotalPrice)));
            }
        }
    }
}
