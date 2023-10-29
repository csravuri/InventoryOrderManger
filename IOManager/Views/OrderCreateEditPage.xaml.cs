using IOManager.ViewModels;

namespace IOManager.Views;

public partial class OrderCreateEditPage : ContentPage
{
	readonly OrderCreateEditViewModel vm;

	public OrderCreateEditPage(OrderCreateEditViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
		this.vm = vm;
	}

	private void Entry_TextChanged(object sender, TextChangedEventArgs e)
	{
	}
}