using OpenTAG.Funnel.Parts.Main.ViewModel;

namespace OpenTAG.Funnel.Parts.Main.View;

public partial class MainView : ContentPage
{
    public MainView(ScanViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}