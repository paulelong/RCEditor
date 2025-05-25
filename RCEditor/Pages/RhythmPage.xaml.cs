using RCEditor.Models;
using RCEditor.Services;

namespace RCEditor.Pages;

public partial class RhythmPage : ContentPage
{    // Add a property for CurrentPatch
    public RCEditor.Models.MemoryPatch? CurrentPatch { get; private set; }

    public RhythmPage()
    {
        InitializeComponent(); // Ensure the XAML file is properly linked
        this.BindingContext = this;

        // Use the current patch from the service        CurrentPatch = PatchService.Instance.CurrentPatch;

        // Subscribe to patch changes
        PatchService.Instance.CurrentPatchChanged += OnCurrentPatchChanged;
    }

    private void OnCurrentPatchChanged(object? sender, RCEditor.Models.MemoryPatch newPatch)
    {
        CurrentPatch = newPatch;
        OnPropertyChanged(nameof(CurrentPatch)); // Notify the UI of the change
    }

    private void OnImportMidiClicked(object sender, EventArgs e)
    {

    }

    // Other code remains unchanged
}
