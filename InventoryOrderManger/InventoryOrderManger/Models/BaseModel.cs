using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace InventoryOrderManger.Models
{
    public class BaseModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        private bool _isSelected { get; set; }

        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
            }
        }
    }
}
