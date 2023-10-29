using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace IOManager.ViewModels
{
	public partial class OrderLineCreateEditViewModel : ObservableObject
	{
		readonly Action<OrderLineCreateEditViewModel> qtyChangeAction;

		public OrderLineCreateEditViewModel(Action<OrderLineCreateEditViewModel> qtyChangeAction)
		{
			this.qtyChangeAction = qtyChangeAction;
		}
		public string ItemName { get; set; }

		[ObservableProperty]
		[NotifyPropertyChangedFor(nameof(Total))]
		int qty;

		[ObservableProperty]
		[NotifyPropertyChangedFor(nameof(Total))]
		decimal price;

		public decimal Total => Price * Qty;

		[RelayCommand]
		void QtyDec()
		{
			Qty--;
		}

		[RelayCommand]
		void QtyInc(object obj)
		{
			Qty++;
		}

		partial void OnPriceChanged(decimal value)
		{
			TotalChange();
		}

		partial void OnQtyChanged(int value)
		{
			TotalChange();
		}

		void TotalChange()
		{
			qtyChangeAction?.Invoke(this);
		}
	}
}
