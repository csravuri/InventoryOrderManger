using IOManager.ViewModels;

namespace IOManager.Views;

public partial class BackupPage : ContentPage
{
	public BackupPage(BackupViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}