using System.Collections.ObjectModel;
using Microsoft.Maui.Platform;
using RCEditor.Models;
using RCEditor.Services;
#if WINDOWS
using Microsoft.UI.Xaml;
#endif

namespace RCEditor;

public partial class MainPage : ContentPage
{
    private bool _isPatchSelected = false;
    private bool _isLoading = false;
    private bool _hasDirectoryLoaded = false;
    private bool _hasSystemSettings = false;
    private string _systemFileInfo = "No system file found";
    private int _systemMasterLevel = 100;
    private int _systemReverbLevel = 0;

    public bool IsPatchSelected 
    { 
        get => _isPatchSelected; 
        set
        {
            _isPatchSelected = value;
            OnPropertyChanged(nameof(IsPatchSelected));
        }
    }
    
    public bool IsLoading
    {
        get => _isLoading;
        set
        {
            _isLoading = value;
            OnPropertyChanged(nameof(IsLoading));
        }
    }
    
    public bool HasDirectoryLoaded
    {
        get => _hasDirectoryLoaded;
        set
        {
            _hasDirectoryLoaded = value;
            OnPropertyChanged(nameof(HasDirectoryLoaded));
        }
    }
    
    public bool HasSystemSettings
    {
        get => _hasSystemSettings;
        set
        {
            _hasSystemSettings = value;
            OnPropertyChanged(nameof(HasSystemSettings));
        }
    }
    
    public string SystemFileInfo
    {
        get => _systemFileInfo;
        set
        {
            _systemFileInfo = value;
            OnPropertyChanged(nameof(SystemFileInfo));
        }
    }
    
    public int SystemMasterLevel
    {
        get => _systemMasterLevel;
        set
        {
            _systemMasterLevel = value;
            OnPropertyChanged(nameof(SystemMasterLevel));
        }
    }
    
    public int SystemReverbLevel
    {
        get => _systemReverbLevel;
        set
        {
            _systemReverbLevel = value;
            OnPropertyChanged(nameof(SystemReverbLevel));
        }
    }

    public MainPage()
    {
        InitializeComponent();
        this.BindingContext = this;

        // Use the shared PatchService
        PatchesCollection.ItemsSource = PatchService.Instance.Patches;
        
        // Set the current patch from the service
        PatchDetailsPanel.BindingContext = PatchService.Instance.CurrentPatch;        // Subscribe to patch changes
        PatchService.Instance.CurrentPatchChanged += OnCurrentPatchChanged;
        
        // Register other events
        RegisterEvents();
    }

    private void OnCurrentPatchChanged(object? sender, RCEditor.Models.MemoryPatch currentPatch)
    {
        // Update the binding context when the current patch changes
        //PatchDetailsPanel.BindingContext = currentPatch;
        IsPatchSelected = true;
    }
      private async void OnSelectDirectoryClicked(object sender, EventArgs e)
    {
        try
        {
            // Show a folder picker dialog
            var options = new PickOptions
            {
                PickerTitle = "Select RC-600 Patch Directory"
            };
            
            // For .NET MAUI, we use FilePicker instead of FolderPicker
            // We'll need a workaround to select folders
            string? selectedFolder = await GetFolderPath();
            
            if (!string.IsNullOrEmpty(selectedFolder))
            {
                // Show loading indicator
                IsLoading = true;
                LoadingIndicator.IsVisible = true;
                LoadingIndicator.IsRunning = true;
                
                try
                {
                    // Load patches from the selected directory
                    bool success = await PatchService.Instance.LoadPatchDirectory(selectedFolder);
                    
                    if (success)
                    {
                        HasDirectoryLoaded = true;
                        
                        // Update system settings UI
                        UpdateSystemSettingsUI();
                    }
                    else
                    {
                        await DisplayAlert("Directory Selection", "No patch files found in the selected directory.", "OK");
                        HasDirectoryLoaded = false;
                    }
                }
                finally
                {
                    // Hide loading indicator
                    IsLoading = false;
                    LoadingIndicator.IsVisible = false;
                    LoadingIndicator.IsRunning = false;
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }
    
    private async Task<string?> GetFolderPath()
    {
        // In a real app, this would use platform-specific code to show a folder picker
        // For now, we'll just use a hardcoded path for demonstration
#if WINDOWS
        var folderPicker = new Windows.Storage.Pickers.FolderPicker();
        folderPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
        folderPicker.FileTypeFilter.Add("*");
          // Get the current window handle safely
        var hwnd = IntPtr.Zero;
        if (App.Current?.Windows != null && 
            App.Current.Windows.Count > 0 && 
            App.Current.Windows[0].Handler?.PlatformView != null &&
            App.Current.Windows[0].Handler.PlatformView is MauiWinUIWindow platformView)
        {
            hwnd = platformView.WindowHandle;
        }
        
        // Associate the HWND with the file picker
        WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, hwnd);
          try
        {
            var folder = await folderPicker.PickSingleFolderAsync();
            return folder?.Path;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to open folder picker: {ex.Message}", "OK");
            return null;
        }
#else
        // For other platforms or fallback - show dialog to enter path directly
        string result = await DisplayPromptAsync("Select Directory", 
            "Please enter the path to your RC-600 patch directory:", 
            initialValue: "/path/to/rc600/patches");
        
        return result;
#endif
    }

    private void UpdateSystemSettingsUI()
    {
        var systemSettings = PatchService.Instance.SystemSettings;
        
        if (systemSettings != null)
        {
            HasSystemSettings = true;
            SystemFileInfo = $"System File: {systemSettings.FileName}";
            
            // Access the mixer master output level if available
            SystemMasterLevel = 100; // Default value
            if (systemSettings.Mixer != null)
            {
                SystemMasterLevel = systemSettings.Mixer.MasterOut;
            }
            
            // Access the reverb level if available
            SystemReverbLevel = 0; // Default value
            if (systemSettings.Output?.MasterFx != null)
            {
                SystemReverbLevel = systemSettings.Output.MasterFx.Reverb;
            }
        }
        else
        {
            HasSystemSettings = false;
            SystemFileInfo = "No system file found";
            SystemMasterLevel = 100;
            SystemReverbLevel = 0;
        }
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
    
    private async void OnPatchSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count > 0)
        {
            var selectedItem = e.CurrentSelection[0] as PatchListItem;
            if (selectedItem != null)
            {
                // Show loading indicator
                IsLoading = true;
                LoadingIndicator.IsVisible = true;
                LoadingIndicator.IsRunning = true;
                
                try
                {
                    // Use the service to select this patch with the new async method
                    await PatchService.Instance.SelectPatchAsync(selectedItem);
                    IsPatchSelected = true;
                }
                finally
                {
                    // Hide loading indicator
                    IsLoading = false;
                    LoadingIndicator.IsVisible = false;
                    LoadingIndicator.IsRunning = false;
                }
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

    private void OnCurrentDirectoryChanged(object? sender, EventArgs e)
    {
        // Update the UI or perform actions based on the new directory
        HasDirectoryLoaded = true;
    }
    
    private void OnSystemSettingsChanged(object? sender, RCEditor.Models.SystemSettings? systemSettings)
    {
        if (systemSettings != null)
        {
            // Update system settings properties
            SystemMasterLevel = PatchService.Instance.SystemMasterLevel;
            SystemReverbLevel = PatchService.Instance.SystemReverbLevel;
            SystemFileInfo = PatchService.Instance.SystemFileInfo;

            HasSystemSettings = true;
        }
        else
        {
            HasSystemSettings = false;
        }
    }
    
    public void RegisterEvents()
    {
        // Subscribe to directory changes
        PatchService.Instance.CurrentDirectoryChanged += (sender, dirPath) => 
        {
            HasDirectoryLoaded = !string.IsNullOrEmpty(dirPath);
        };
        
        // Subscribe to system settings changes
        PatchService.Instance.SystemSettingsChanged += (sender, settings) => 
        {
            UpdateSystemSettingsUI();
        };
    }
}

