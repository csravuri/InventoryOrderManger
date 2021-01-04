using System.ComponentModel;
using SQLite;

namespace InventoryOrderManger.Models
{
    public class OrderHeader : BaseModel, INotifyPropertyChanged
    {
        public new event PropertyChangedEventHandler PropertyChanged;

        [PrimaryKey, AutoIncrement]
        public int OrderID { get; set; }
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

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OrderTotalPrice)));
            }
        }
    }
}
