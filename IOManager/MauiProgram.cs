using IOManager.Database;
using IOManager.ViewModels;
using IOManager.Views;
using Microsoft.Extensions.Logging;

namespace IOManager;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		builder.Services.AddSingleton<HomeViewModel>();
		builder.Services.AddSingleton<HomePage>();

		builder.Services.AddTransient<ItemCreateEditPage>();
		builder.Services.AddTransient<ItemsSearchPage>();
		builder.Services.AddTransient<OrderCreateEditPage>();
		builder.Services.AddTransient<OrdersSearchPage>();
		builder.Services.AddTransient<BackupPage>();
		builder.Services.AddTransient<RestorePage>();
		builder.Services.AddTransient<CustomerCreateEditPage>();
		builder.Services.AddTransient<CustomerSearchPage>();
		builder.Services.AddTransient<OfflineSyncPage>();

		builder.Services.AddTransient<ItemCreateEditViewModel>();
		builder.Services.AddTransient<ItemsSearchViewModel>();
		builder.Services.AddTransient<OrderCreateEditViewModel>();
		builder.Services.AddTransient<OrdersSearchViewModel>();
		builder.Services.AddTransient<BackupViewModel>();
		builder.Services.AddTransient<RestoreViewModel>();
		builder.Services.AddTransient<RestoreViewModel>();

		builder.Services.AddSingleton<DbConnection>();

		return builder.Build();
	}
}
