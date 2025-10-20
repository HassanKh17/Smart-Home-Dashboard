using Microsoft.Extensions.Logging;



namespace SmartHomeDashboardP;

using SmartHomeDashboardP.Views;
using SmartHomeDashboardP.ViewModels;
using SmartHomeDashboardP.Services;



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
        builder.Services.AddSingleton<MainViewModel>();
        builder.Services.AddSingleton<DashboardPage>();
        builder.Services.AddSingleton<SettingsPage>();
        builder.Services.AddSingleton<DeviceSimulationService>();

#endif

        return builder.Build();
	}
}
