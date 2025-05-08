using OpenTAG.Funnel.Parts.GetStarted.View;
using OpenTAG.Funnel.Parts.Main.View;
using OpenTAG.Funnel.Parts.Splash;

namespace OpenTAG.Funnel;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        
        Routing.RegisterRoute(nameof(Splash), typeof(Splash));
        Routing.RegisterRoute(nameof(JwtTokenPage), typeof(JwtTokenPage));
        Routing.RegisterRoute(nameof(TemplateIdPage), typeof(TemplateIdPage));
        Routing.RegisterRoute(nameof(GetStartedPage), typeof(GetStartedPage));
        Routing.RegisterRoute(nameof(MainView), typeof(MainView));
        Routing.RegisterRoute(nameof(ScanView), typeof(ScanView));
    }
}