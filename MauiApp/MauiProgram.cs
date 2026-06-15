using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TicTacToe.Maui.Services;
using TicTacToe.Maui.ViewModels;
using TicTacToe.Maui.Views;

namespace TicTacToe.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMaui()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream("appsettings.json");
        if (stream is not null)
            builder.Configuration.AddJsonStream(stream);

        var apiBaseUrl = builder.Configuration["ApiBaseUrl"]
            ?? throw new InvalidOperationException("ApiBaseUrl is not configured in appsettings.json.");

        builder.Services.AddHttpClient<GameApiService>(client =>
        {
            client.BaseAddress = new Uri(apiBaseUrl.TrimEnd('/') + '/');
        });

        builder.Services.AddTransient<GameViewModel>();
        builder.Services.AddTransient<GamePage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
