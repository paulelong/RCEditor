using RCEditor.Models;
using RCEditor.Services;

namespace RCEditor.Pages;

public partial class ControlsPage : ContentPage
{
    public ControlsPage()
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
    private void OnAddAssignClicked(object sender, EventArgs e)
    {
        if (CurrentPatch == null) return;

        // Create a new assign slot with default values
        var newAssign = new AssignSlot
        {
            Source = "Expression Pedal 1",
            Target = "Track 1 Level",
            ActionMode = ActionModeEnum.Continuous
        };

        // Add it to the collection
        CurrentPatch.Assigns.Add(newAssign);
    }

    private void OnRemoveAssignClicked(object sender, EventArgs e)
    {
        if (CurrentPatch == null) return;
        
        var button = sender as Button;
        if (button != null && button.CommandParameter is AssignSlot assignToRemove)
        {
            CurrentPatch.Assigns.Remove(assignToRemove);
        }
    }
}