using RCEditor.Models;
using RCEditor.Services;

namespace RCEditor.Pages;

public partial class TracksPage : ContentPage
{
    public TracksPage()
    {
        InitializeComponent();
        this.BindingContext = this;

        // Use the current patch from the service
        CurrentPatch = PatchService.Instance.CurrentPatch;
          // Subscribe to patch changes
        PatchService.Instance.CurrentPatchChanged += OnCurrentPatchChanged;
    }

    private void OnCurrentPatchChanged(object? sender, RCEditor.Models.MemoryPatch currentPatch)
    {
        // Update the binding context when the current patch changes
        CurrentPatch = currentPatch;
    }

    // Property that binds to the current patch
    public RCEditor.Models.MemoryPatch? CurrentPatch { get; set; }

    private void OnTrackTabClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button != null && button.BindingContext is string tabName)
        {
            // Hide all track panels
            Track1Panel.IsVisible = false;
            Track2Panel.IsVisible = false;
            Track3Panel.IsVisible = false;
            Track4Panel.IsVisible = false;
            Track5Panel.IsVisible = false;
            Track6Panel.IsVisible = false;
            
            // Show the selected track panel
            switch (tabName)
            {
                case "Track1":
                    Track1Panel.IsVisible = true;
                    break;
                case "Track2":
                    Track2Panel.IsVisible = true;
                    break;
                case "Track3":
                    Track3Panel.IsVisible = true;
                    break;
                case "Track4":
                    Track4Panel.IsVisible = true;
                    break;
                case "Track5":
                    Track5Panel.IsVisible = true;
                    break;
                case "Track6":
                    Track6Panel.IsVisible = true;
                    break;
            }
        }
    }
}