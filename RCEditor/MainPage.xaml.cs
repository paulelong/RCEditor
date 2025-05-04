using System.Collections.ObjectModel;
using RCEditor.Models;
using RCEditor.Services;

namespace RCEditor;

public partial class MainPage : ContentPage
{
    private bool _isPatchSelected = false;

    public bool IsPatchSelected 
    { 
        get => _isPatchSelected; 
        set
        {
            _isPatchSelected = value;
            OnPropertyChanged(nameof(IsPatchSelected));
        }
    }

    public MainPage()
    {
        InitializeComponent();
        this.BindingContext = this;

        // Use the shared PatchService
        PatchesCollection.ItemsSource = PatchService.Instance.Patches;
        
        // Set the current patch from the service
        PatchDetailsPanel.BindingContext = PatchService.Instance.CurrentPatch;
        
        // Subscribe to patch changes
        PatchService.Instance.CurrentPatchChanged += OnCurrentPatchChanged;
    }

    private void OnCurrentPatchChanged(object sender, MemoryPatch currentPatch)
    {
        // Update the binding context when the current patch changes
        PatchDetailsPanel.BindingContext = currentPatch;
        IsPatchSelected = true;
    }

    private void OnNewPatchClicked(object sender, EventArgs e)
    {
        // Use the service to create a new patch
        PatchService.Instance.CreateNewPatch();
        IsPatchSelected = true;
    }

    private async void OnImportClicked(object sender, EventArgs e)
    {
        try
        {
            // Show a file picker dialog
            var options = new PickOptions
            {
                PickerTitle = "Select RC-600 Patch File",
                FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.WinUI, new[] { ".rc0" } },
                    { DevicePlatform.macOS, new[] { "rc0" } }
                })
            };

            var result = await FilePicker.PickAsync(options);
            if (result != null)
            {
                // Show loading indicator
                LoadingIndicator.IsRunning = true;
                LoadingIndicator.IsVisible = true;

                try
                {
                    // Use the PatchService to import the file
                    bool success = await PatchService.Instance.ImportPatch(result.FullPath);
                    
                    if (success)
                    {
                        await DisplayAlert("Import", $"Successfully imported patch from {result.FileName}", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Import Failed", $"Could not import patch from {result.FileName}. The file may be corrupted or in an unsupported format.", "OK");
                    }
                }
                finally
                {
                    // Hide loading indicator
                    LoadingIndicator.IsRunning = false;
                    LoadingIndicator.IsVisible = false;
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }

    private async void OnExportClicked(object sender, EventArgs e)
    {
        if (!IsPatchSelected)
        {
            await DisplayAlert("Export", "Please select a patch to export", "OK");
            return;
        }

        // For demonstration only - in a real app, this would export to a .RC0 file
        await DisplayAlert("Export", $"Export of patch '{PatchService.Instance.CurrentPatch.Name}' to RC-600 .RC0 files will be implemented in a future update.", "OK");
    }

    private async void OnExportAllClicked(object sender, EventArgs e)
    {
        // For demonstration only - in a real app, this would export all patches to .RC0 files
        await DisplayAlert("Export All", "Export all patches to RC-600 .RC0 files will be implemented in a future update.", "OK");
    }

    private void OnFilterTextChanged(object sender, TextChangedEventArgs e)
    {
        var searchBar = sender as SearchBar;
        if (searchBar == null || string.IsNullOrWhiteSpace(searchBar.Text))
        {
            // Reset filter
            foreach (var item in PatchService.Instance.Patches)
            {
                item.IsVisible = true;
            }
            return;
        }

        string filter = searchBar.Text.ToLowerInvariant();
        
        // Apply filter
        foreach (var item in PatchService.Instance.Patches)
        {
            item.IsVisible = item.Name.ToLowerInvariant().Contains(filter) || 
                            item.PatchNumber.ToString().Contains(filter);
        }
    }

    private void OnPatchSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count > 0)
        {
            var selectedItem = e.CurrentSelection[0] as PatchListItem;
            if (selectedItem != null)
            {
                // Use the service to select this patch
                PatchService.Instance.SelectPatch(selectedItem);
                IsPatchSelected = true;
            }
        }
        else
        {
            IsPatchSelected = false;
        }
    }

    private void OnDetailTabClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button != null && button.BindingContext is string tabName)
        {
            // Hide all tabs
            GeneralTab.IsVisible = false;
            SingleModeTab.IsVisible = false;
            
            // Show the selected tab
            if (tabName == "GeneralTab")
                GeneralTab.IsVisible = true;
            else if (tabName == "SingleModeTab")
                SingleModeTab.IsVisible = true;
        }
    }
}

