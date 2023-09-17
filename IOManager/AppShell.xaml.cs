using IOManager.Views;

namespace IOManager;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(ItemCreateEditPage), typeof(ItemCreateEditPage));
		Routing.RegisterRoute(nameof(ItemsSearchPage), typeof(ItemsSearchPage));
		Routing.RegisterRoute(nameof(OrderCreateEditPage), typeof(OrderCreateEditPage));
		Routing.RegisterRoute(nameof(OrdersSearchPage), typeof(OrdersSearchPage));
		Routing.RegisterRoute(nameof(BackupPage), typeof(BackupPage));
		Routing.RegisterRoute(nameof(RestorePage), typeof(RestorePage));
	}
}
