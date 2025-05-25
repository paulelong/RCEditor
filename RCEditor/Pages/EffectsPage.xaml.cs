using RCEditor.Models;
using RCEditor.Services;
using System.Text;

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

    private void OnCurrentPatchChanged(object? sender, MemoryPatch currentPatch)
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

    private async void OnEditParametersClicked(object? sender, EventArgs e)
    {
        var button = sender as Button;
        if (button != null && button.BindingContext is string slotId)
        {
            // Parse the slot ID to determine which bank and slot
            char bankLetter = slotId[0];
            int slotNumber = int.Parse(slotId[1].ToString());            // Get the corresponding effect slot
            EffectSlot? slot = null;
            switch (bankLetter)
            {
                case 'A':
                    slot = CurrentPatch.InputFX.Banks["A"].Slots[slotNumber];
                    break;
                case 'B':
                    slot = CurrentPatch.InputFX.Banks["B"].Slots[slotNumber];
                    break;
                case 'C':
                    slot = CurrentPatch.InputFX.Banks["C"].Slots[slotNumber];
                    break;
                case 'D':
                    slot = CurrentPatch.InputFX.Banks["D"].Slots[slotNumber];
                    break;
            }

            if (slot != null)
            {
                // Get current effect settings
                var currentSettings = slot.GetCurrentEffectSettings();
                
                // Show available effects and allow the user to switch between them
                string[] effectOptions = slot.AllEffectSettings.Values
                    .Select(e => e.EffectName)
                    .ToArray();
                    
                // Add option to create a new effect if we don't have all possible effects
                if (effectOptions.Length < 55)
                {
                    effectOptions = effectOptions.Append("Add new effect...").ToArray();
                }
                
                // Show options dialog
                string result = await DisplayActionSheet(
                    $"Effect Settings for Bank {bankLetter} Slot {slotNumber}",
                    "Cancel",
                    null,
                    effectOptions);
                    
                if (result == "Add new effect...")
                {
                    // Here we would show a dialog to select from all available effect types
                    // For now, just show a message
                    await DisplayAlert("Add Effect", 
                        "In a real implementation, this would show a list of all available effects to add to this slot.",
                        "OK");
                }
                else if (result != "Cancel" && result != null)
                {
                    // Find the selected effect in the dictionary
                    var selectedEffect = slot.AllEffectSettings.Values
                        .FirstOrDefault(e => e.EffectName == result);
                        
                    if (selectedEffect != null)
                    {
                        // Switch to the selected effect
                        slot.SwitchToEffect(selectedEffect.EffectType, selectedEffect.EffectName);
                        
                        // Now show the parameters dialog for the selected effect
                        await ShowEffectParametersDialog(slot, selectedEffect);
                    }
                }
                else if (result == "Cancel")
                {
                    // If the user just wanted to edit the current effect without switching
                    await ShowEffectParametersDialog(slot, currentSettings);
                }
            }
        }
    }

    private async Task ShowEffectParametersDialog(EffectSlot slot, EffectSettings settings)
    {
        // In a real app, we would open a parameters editor dialog with all parameters
        // For now, just display a placeholder message with the parameter information
        StringBuilder paramInfo = new StringBuilder();
        paramInfo.AppendLine($"Effect: {settings.EffectName} (ID: {settings.EffectType})");
        paramInfo.AppendLine($"Enabled: {slot.Enabled}");
        paramInfo.AppendLine();
        paramInfo.AppendLine("Parameters:");
        
        foreach (var param in settings.Parameters)
        {
            paramInfo.AppendLine($"- {param.Key}: {param.Value}");
        }
        
        await DisplayAlert("Effect Parameters", paramInfo.ToString(), "OK");
        
        // In a real implementation, we would:
        // 1. Show a custom dialog with sliders/controls for each parameter
        // 2. Allow the user to adjust parameters and save them
        // 3. Update both the slot.Parameters and the settings.Parameters
    }
}