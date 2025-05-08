using OpenTAG.Funnel.Parts.GetStarted.ViewModels;

namespace OpenTAG.Funnel.Parts.GetStarted.View;

public partial class JwtTokenPage : ContentPage
{
    public JwtTokenPage(InitViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}