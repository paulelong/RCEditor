using RCEditor.Models;
using RCEditor.Services;

namespace RCEditor.Pages;

public partial class EffectsPage : ContentPage
{
    public EffectsPage()
    {
        InitializeComponent();
        this.BindingContext = this;

        // Use the current patch from the service
        CurrentPatch = PatchService.Instance.CurrentPatch;
        
        // Subscribe to patch changes
        PatchService.Instance.CurrentPatchChanged += OnCurrentPatchChanged;
    }

    private void OnCurrentPatchChanged(object sender, MemoryPatch currentPatch)
    {
        // Update the binding context when the current patch changes
        CurrentPatch = currentPatch;
    }

    // Property that binds to the current patch
    public MemoryPatch CurrentPatch { get; set; }

    private void OnBankTabClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button != null && button.BindingContext is string tabName)
        {
            // Hide all bank panels
            BankAPanel.IsVisible = false;
            BankBPanel.IsVisible = false;
            BankCPanel.IsVisible = false;
            BankDPanel.IsVisible = false;

            // Show the selected bank panel
            switch (tabName)
            {
                case "BankA":
                    BankAPanel.IsVisible = true;
                    break;
                case "BankB":
                    BankBPanel.IsVisible = true;
                    break;
                case "BankC":
                    BankCPanel.IsVisible = true;
                    break;
                case "BankD":
                    BankDPanel.IsVisible = true;
                    break;
            }
        }
    }

    private async void OnEditParametersClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button != null && button.BindingContext is string slotId)
        {
            // Parse the slot ID to determine which bank and slot
            char bankLetter = slotId[0];
            int slotNumber = int.Parse(slotId[1].ToString()) - 1;

            // Get the corresponding effect slot
            EffectSlot slot = null;
            switch (bankLetter)
            {
                case 'A':
                    slot = CurrentPatch.Effects.BankA.Slots[slotNumber];
                    break;
                case 'B':
                    slot = CurrentPatch.Effects.BankB.Slots[slotNumber];
                    break;
                case 'C':
                    slot = CurrentPatch.Effects.BankC.Slots[slotNumber];
                    break;
                case 'D':
                    slot = CurrentPatch.Effects.BankD.Slots[slotNumber];
                    break;
            }

            if (slot != null)
            {
                // In a real app, we would open a parameters editor dialog
                // For now, just display a placeholder message
                await DisplayAlert("Effect Parameters",
                    $"Parameter editor for {bankLetter}{slotNumber + 1}: {slot.Type}\n\nThis dialog would contain sliders and controls for all parameters of this effect type.",
                    "OK");
            }
        }
    }
}