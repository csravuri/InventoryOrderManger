using IOManager.ViewModels;

namespace IOManager.Views;

public partial class OrderCreateEditPage : ContentPage
{
	public OrderCreateEditPage(OrderCreateEditViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}