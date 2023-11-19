using IOManager.ViewModels;

namespace IOManager.Views;

public partial class OfflineSyncPage : ContentPage
{
	public OfflineSyncPage(OfflineSyncViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}