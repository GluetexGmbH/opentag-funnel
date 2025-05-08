using OpenTAG.Funnel.Parts.GetStarted.ViewModels;

namespace OpenTAG.Funnel.Parts.GetStarted.View;

public partial class GetStartedPage : ContentPage
{
    public GetStartedPage(InitViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}