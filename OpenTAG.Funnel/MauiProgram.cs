using Microsoft.Extensions.Logging;
using OpenTAG.Funnel.Parts.GetStarted.View;
using OpenTAG.Funnel.Parts.GetStarted.ViewModels;
using OpenTAG.Funnel.Parts.Main.View;
using OpenTAG.Funnel.Parts.Main.ViewModel;
using OpenTAG.Funnel.Services;
using OpenTAG.Funnel.Services.Interfaces;

namespace OpenTAG.Funnel;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
           .UseMauiApp<App>()
           .ConfigureFonts(fonts => {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        
        // Add HttpClient 
        builder.Services.AddScoped(_ => new HttpClient());
        
        // Register services
        builder.Services.AddSingleton<INavigationService, NavigationService>();
        builder.Services.AddSingleton<ISettingsService, SettingsService>();
        builder.Services.AddSingleton<IOpenTagAdapter, OpenTagAdapter>();
        builder.Services.AddSingleton<IOpenTagValidator, OpenTagValidator>();
        builder.Services.AddSingleton<IOpenTagMapper, OpenTagMapper>();
        
        // Register view models
        builder.Services.AddSingleton<InitViewModel>();
        builder.Services.AddSingleton<ScanViewModel>();
        
        // Register pages
        builder.Services.AddTransient<GetStartedPage>();
        builder.Services.AddTransient<JwtTokenPage>();
        builder.Services.AddTransient<TemplateIdPage>();
        builder.Services.AddTransient<MainView>();
        builder.Services.AddTransient<ScanView>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}