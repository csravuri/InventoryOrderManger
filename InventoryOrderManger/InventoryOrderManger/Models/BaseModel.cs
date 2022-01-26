using System;
using System.ComponentModel;
using SQLite;

namespace InventoryOrderManger.Models
{
    public class BaseModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [PrimaryKey]
        public Guid ID { get; set; } = Guid.Empty;

        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        private bool _isSelected { get; set; }

        [Ignore]
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                OnPropertyChanged(this, new PropertyChangedEventArgs(nameof(IsSelected)));
            }
        }

        protected void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(sender, e);
        }
    }
}