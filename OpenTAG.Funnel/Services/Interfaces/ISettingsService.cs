namespace OpenTAG.Funnel.Services.Interfaces;

public interface ISettingsService
{
    Task<string?> GetSettingAsync(string key);
    
    Task SaveSettingAsync(string key, string? value);
}