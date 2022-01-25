using System.ComponentModel;

namespace InventoryOrderManger.Models
{
    public class OrderHeader : BaseModel, INotifyPropertyChanged
    {
        public string OrderNo { get; set; }
        public string CustomerName { get; set; }
        private decimal _orderTotalPrice { get; set; }

        public decimal OrderTotalPrice
        {
            get
            {
                return _orderTotalPrice;
            }
            set
            {
                _orderTotalPrice = value;
                OnPropertyChanged(this, new PropertyChangedEventArgs(nameof(OrderTotalPrice)));
            }
        }
    }
}