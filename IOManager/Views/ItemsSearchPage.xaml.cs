using IOManager.ViewModels;

namespace IOManager.Views;

public partial class ItemsSearchPage : ContentPage
{
	public ItemsSearchPage(ItemsSearchViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}