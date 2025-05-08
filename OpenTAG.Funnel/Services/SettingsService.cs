using System.Text.Json;
using OpenTAG.Funnel.Services.Interfaces;

namespace OpenTAG.Funnel.Services;

public class SettingsService : ISettingsService
{
    private const string SettingsFileName = "settings.json";
    
    public async Task<string?> GetSettingAsync(string key)
    {
        if (string.IsNullOrEmpty(key))
            return null;

        // iOS and MacCatalyst support SecureStorage only with a valid Entitlements file
        if (DeviceInfo.Current.Platform == DevicePlatform.iOS || DeviceInfo.Current.Platform == DevicePlatform.MacCatalyst)
        {
            string basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string settingsFilePath = Path.Combine(basePath, SettingsFileName);

            if (!File.Exists(settingsFilePath))
                return null;
            
            string json = await File.ReadAllTextAsync(settingsFilePath);
            var settings = JsonSerializer.Deserialize<Dictionary<string, string?>>(json);
                
            if (settings != null && settings.TryGetValue(key, out string? value))
            {
                return value;
            }

            return null;
        }
        
        return await SecureStorage.GetAsync(key);
    }

    public async Task SaveSettingAsync(string key, string? value)
    {
        if (string.IsNullOrEmpty(key))
            return;
        
        // iOS and MacCatalyst support SecureStorage only with a valid Entitlements file
        if (DeviceInfo.Current.Platform == DevicePlatform.iOS || DeviceInfo.Current.Platform == DevicePlatform.MacCatalyst)
        {
            string basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            Directory.CreateDirectory(basePath);
            string settingsFilePath = Path.Combine(basePath, SettingsFileName);
            
            Dictionary<string, string?> settings = [];
            if (File.Exists(settingsFilePath))
            {
                string json = await File.ReadAllTextAsync(settingsFilePath);
                settings = JsonSerializer.Deserialize<Dictionary<string, string?>>(json) ?? [];
            }
                
            settings[key] = value;
                
            string settingsJson = JsonSerializer.Serialize(settings);
            await File.WriteAllTextAsync(settingsFilePath, settingsJson);
                
            return;
        }
        
        await SecureStorage.SetAsync(key, value);
    }
}