using IOManager.ViewModels;

namespace IOManager.Views;

public partial class RestorePage : ContentPage
{
	public RestorePage(RestoreViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}