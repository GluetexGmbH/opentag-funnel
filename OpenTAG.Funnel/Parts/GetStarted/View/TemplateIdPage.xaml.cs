using OpenTAG.Funnel.Parts.GetStarted.ViewModels;

namespace OpenTAG.Funnel.Parts.GetStarted.View;

public partial class TemplateIdPage : ContentPage
{
    public TemplateIdPage(InitViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}