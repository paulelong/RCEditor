using RCEditor.Pages;

namespace RCEditor;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        
        // Register routes for the tab pages
        Routing.RegisterRoute("TracksPage", typeof(TracksPage));
        Routing.RegisterRoute("RhythmPage", typeof(RhythmPage));
        Routing.RegisterRoute("EffectsPage", typeof(EffectsPage));
        Routing.RegisterRoute("ControlsPage", typeof(ControlsPage));
        Routing.RegisterRoute("AboutPage", typeof(AboutPage));
    }
}
