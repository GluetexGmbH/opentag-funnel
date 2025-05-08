using CommunityToolkit.Mvvm.ComponentModel;

namespace OpenTAG.Funnel.Parts;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    private bool isBusy;
}