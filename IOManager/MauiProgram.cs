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

		return builder.Build();
	}
}
