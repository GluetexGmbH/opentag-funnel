using Microsoft.Extensions.Logging;
using OpenTAG.Funnel.Parts.GetStarted.View;
using OpenTAG.Funnel.Parts.Main.View;
using OpenTAG.Funnel.Services.Interfaces;

namespace OpenTAG.Funnel.Parts.Splash;

public partial class Splash : ContentPage
{
    private readonly ILogger<Splash> logger;
    private readonly ISettingsService settingsService;
    private readonly INavigationService navigationService;
    
    public Splash(ISettingsService settingsService, INavigationService navigationService, ILogger<Splash> logger)
    {
        this.settingsService = settingsService;
        this.navigationService = navigationService;
        this.logger = logger;

        InitializeComponent();
        Init();
    }

    private async void Init()
    {
        try
        {
            string? jwtToken = await settingsService.GetSettingAsync("JwtToken");
            string? templateId = await settingsService.GetSettingAsync("TemplateId");
            
            if (string.IsNullOrWhiteSpace(jwtToken) || string.IsNullOrWhiteSpace(templateId))
            {
                await navigationService.NavigateToAsync<GetStartedPage>();
            } 
            else
            {
                await navigationService.NavigateToAsync<MainView>();
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error during splash screen initialization");
            await DisplayAlert("Error", "An error occurred while initializing the application. Please try again.", "OK");
            Application.Current?.Quit();
        }
    }
}