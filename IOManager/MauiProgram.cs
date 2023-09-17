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

		builder.Services.AddSingleton<ItemCreateEditPage>();
		builder.Services.AddSingleton<ItemsSearchPage>();
		builder.Services.AddSingleton<OrderCreateEditPage>();
		builder.Services.AddSingleton<OrdersSearchPage>();
		builder.Services.AddSingleton<BackupPage>();
		builder.Services.AddSingleton<RestorePage>();

		builder.Services.AddSingleton<ItemCreateEditViewModel>();
		builder.Services.AddSingleton<ItemsSearchViewModel>();
		builder.Services.AddSingleton<OrderCreateEditViewModel>();
		builder.Services.AddSingleton<OrdersSearchViewModel>();
		builder.Services.AddSingleton<BackupViewModel>();
		builder.Services.AddSingleton<RestoreViewModel>();

		return builder.Build();
	}
}
