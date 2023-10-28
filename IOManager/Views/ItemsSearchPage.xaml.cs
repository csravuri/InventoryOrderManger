using IOManager.ViewModels;

namespace IOManager.Views;

public partial class ItemsSearchPage : ContentPage
{
	private readonly ItemsSearchViewModel vm;

	public ItemsSearchPage(ItemsSearchViewModel vm)
	{
		InitializeComponent();
		BindingContext = this.vm = vm;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		vm.SearchCommand.Execute(true);
	}

	private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
	{
		vm.SearchCommand.Execute(null);
	}
}