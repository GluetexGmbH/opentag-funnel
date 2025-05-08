using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OpenTAG.Funnel.Parts.GetStarted.View;
using OpenTAG.Funnel.Parts.Main.View;
using OpenTAG.Funnel.Services;
using OpenTAG.Funnel.Services.Interfaces;
using OpenTAG.Funnel.Services.Types;

namespace OpenTAG.Funnel.Parts.Main.ViewModel;

public partial class ScanViewModel : BaseViewModel
{
    private readonly IOpenTagValidator tagValidator;
    private readonly IOpenTagAdapter tagAdapter;
    private readonly INavigationService navigationService;
    private readonly ISettingsService settingsService;

    [ObservableProperty]
    private bool isLoading;
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsScannedIdValid))]
    private string? scannedId;
    
    [ObservableProperty]
    private OpenTagTemplate? loadedTemplate;
    
    [ObservableProperty]
    private OpenTagId? loadedId;
    
    public ScanViewModel(
        IOpenTagValidator tagValidator, INavigationService navigationService, 
        ISettingsService settingsService, IOpenTagAdapter tagAdapter)
    {
        this.tagValidator = tagValidator;
        this.navigationService = navigationService;
        this.settingsService = settingsService;
        this.tagAdapter = tagAdapter;
    }

    public bool IsScannedIdValid => ValidateId();
    
    private bool ValidateId()
    {
        if (string.IsNullOrEmpty(ScannedId))
            return false;
        
        var tagId = tagValidator.GetValidTagId(ScannedId);
        return tagId != null;
    }

    [RelayCommand]
    private async Task Reset()
    {
        await settingsService.SaveSettingAsync("JwtToken", string.Empty);
        await settingsService.SaveSettingAsync("TemplateId", string.Empty);
        
        await navigationService.NavigateToAsync<GetStartedPage>();
    }

    [RelayCommand]
    private async Task Scan()
    {
        IsLoading = true;
        try
        {
            LoadedTemplate = await tagAdapter.LoadTemplate();
            
            if (LoadedTemplate is null)
            {
                await Shell.Current.DisplayAlert("Error", "Failed to load template", "OK");
                LoadedTemplate = null;
                LoadedId = null;
                return;
            }

            LoadedId = await tagAdapter.LoadId(ScannedId);
            
            if (LoadedId is null)
            {
                await Shell.Current.DisplayAlert("Error", "Failed to load ID", "OK");
                LoadedTemplate = null;
                LoadedId = null;
                return;
            }

            await navigationService.NavigateToAsync<ScanView>();   
        }
        finally
        {
            IsLoading = false;
        }
    }
    
    [RelayCommand]
    private async Task BackWithoutSave()
    {
        LoadedTemplate = null;
        LoadedId = null;
        ScannedId = null;
        
        await navigationService.NavigateToAsync<MainView>();
    }
    
    [RelayCommand]
    private async Task Save()
    {
        if (LoadedTemplate is null || LoadedId is null)
            return;

        bool result = await tagAdapter.SaveTag(LoadedTemplate, LoadedId);
        
        if (result)
        {
            await Shell.Current.DisplayAlert("Success", "Tag saved successfully", "OK");
            await BackWithoutSave();
        }
        else
        {
            await Shell.Current.DisplayAlert("Error", "Failed to save tag", "OK");
        }
    }
}