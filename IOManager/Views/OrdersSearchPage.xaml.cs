using IOManager.ViewModels;

namespace IOManager.Views;

public partial class OrdersSearchPage : ContentPage
{
	public OrdersSearchPage(OrdersSearchViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}