using IOManager.ViewModels;

namespace IOManager.Views;

public partial class OnlineSyncPage : ContentPage
{
	public OnlineSyncPage(OnlineSyncViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}