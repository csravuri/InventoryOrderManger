using System;
using System.ComponentModel;

namespace InventoryOrderManger.Models
{
    public class OrderLine : BaseModel, INotifyPropertyChanged
    {
        public Guid OrderID { get; set; }
        public Guid ItemID { get; set; }
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

                OnPropertyChanged(this, new PropertyChangedEventArgs(nameof(ItemTotalPrice)));
            }
        }
    }
}