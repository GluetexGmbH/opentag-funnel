namespace OpenTAG.Funnel.Services.Interfaces;

public interface INavigationService
{
    Task NavigateToAsync<T>(IDictionary<string, object> parameters = null) where T : Page;
    
    Task GoBackAsync();
}