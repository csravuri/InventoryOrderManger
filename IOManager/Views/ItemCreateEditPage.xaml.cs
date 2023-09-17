using IOManager.ViewModels;

namespace IOManager.Views;

public partial class ItemCreateEditPage : ContentPage
{
	public ItemCreateEditPage(ItemCreateEditViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}