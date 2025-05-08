using OpenTAG.Funnel.Services.Interfaces;

namespace OpenTAG.Funnel.Services;

public class NavigationService : INavigationService
{
    public async Task NavigateToAsync<T>(IDictionary<string, object> parameters = null) where T : Page
    {
        await Shell.Current.GoToAsync(typeof(T).Name);
    }
    
    public async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}