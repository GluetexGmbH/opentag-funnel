using System.IdentityModel.Tokens.Jwt;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OpenTAG.Funnel.Parts.GetStarted.View;
using OpenTAG.Funnel.Parts.Main.View;
using OpenTAG.Funnel.Services.Interfaces;

namespace OpenTAG.Funnel.Parts.GetStarted.ViewModels;

   public partial class InitViewModel : BaseViewModel
    {
        private readonly INavigationService navigationService;
        private readonly ISettingsService settingsService;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsJwtValid))]
        private string? jwtToken;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsTemplateIdValid))]
        private string? templateId;

        public bool IsJwtValid => ValidateJwt();
        public bool IsTemplateIdValid => ValidateTemplateId();

        public InitViewModel(INavigationService navigationService, ISettingsService settingsService)
        {
            this.navigationService = navigationService;
            this.settingsService = settingsService;
        }

        private bool ValidateJwt()
        {
            if (string.IsNullOrEmpty(JwtToken))
                return false;
            
            try
            {
                var handler = new JwtSecurityTokenHandler();
                handler.ReadJwtToken(JwtToken);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool ValidateTemplateId()
        {
            return !string.IsNullOrEmpty(TemplateId) && Guid.TryParse((string?) TemplateId, out _);
        }

        [RelayCommand]
        private async Task NavigateToJwtPage()
        {
            await navigationService.NavigateToAsync<JwtTokenPage>();
        }

        [RelayCommand]
        private async Task NavigateToTemplatePage()
        {
            if (!IsJwtValid)
                return;

            await navigationService.NavigateToAsync<TemplateIdPage>();
        }

        [RelayCommand]
        private async Task FinishInitialization()
        {
            if (!IsTemplateIdValid)
                return;

            await settingsService.SaveSettingAsync("JwtToken", JwtToken);
            await settingsService.SaveSettingAsync("TemplateId", TemplateId);
            
            await navigationService.NavigateToAsync<MainView>();
        }
    }
