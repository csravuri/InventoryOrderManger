using IOManager.Views;

namespace IOManager;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(ItemCreateEditPage), typeof(ItemCreateEditPage));
	}
}
