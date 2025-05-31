using RCEditor.Models;
using System.Collections.ObjectModel;
using System.Xml.Linq; // Add this namespace for XDocument
using System.Text.RegularExpressions; // Add this namespace for Regex

namespace RCEditor.Services
{    public class PatchService
    {
        private static PatchService? _instance;
        public static PatchService Instance => _instance ??= new PatchService();        public event EventHandler<RCEditor.Models.MemoryPatch>? CurrentPatchChanged;
        public event EventHandler<RCEditor.Models.SystemSettings?>? SystemSettingsChanged;
        public event EventHandler<string>? CurrentDirectoryChanged;

        private RCEditor.Models.MemoryPatch _currentPatch;
        private RCEditor.Models.SystemSettings? _systemSettings;
        private string _currentDirectory = string.Empty;
        
        public ObservableCollection<PatchListItem> Patches { get; private set; }

        public RCEditor.Models.MemoryPatch CurrentPatch
        {
            get => _currentPatch;
            set
            {
                if (_currentPatch != value)
                {
                    _currentPatch = value;
                    CurrentPatchChanged?.Invoke(this, _currentPatch);
                }
            }
        }
        
        public RCEditor.Models.SystemSettings? SystemSettings
        {
            get => _systemSettings;
            set
            {
                if (_systemSettings != value)
                {
                    _systemSettings = value;
                    SystemSettingsChanged?.Invoke(this, _systemSettings);
                }
            }
        }
        
        public string CurrentDirectory
        {
            get => _currentDirectory;
            private set
            {
                if (_currentDirectory != value)
                {
                    _currentDirectory = value;
                    CurrentDirectoryChanged?.Invoke(this, _currentDirectory);
                }
            }
        }
        
        public bool IsDirectoryLoaded => !string.IsNullOrEmpty(CurrentDirectory);
        
        // Additional properties for system settings access
        public string SystemFileInfo => SystemSettings?.FileName ?? "No system file found";
        public int SystemMasterLevel => SystemSettings?.Mixer?.MasterOut ?? 100;
        public int SystemReverbLevel => SystemSettings?.Output?.MasterFx?.Reverb ?? 0;
        private PatchService()
        {
            // Initialize with empty collection
            Patches = new ObservableCollection<PatchListItem>();

            // Create a default current patch
            _currentPatch = new RCEditor.Models.MemoryPatch();
        }        
        
        public void CreateNewPatch()
        {
            var newPatch = new RCEditor.Models.MemoryPatch();
            newPatch.Name = $"NEW PATCH {Patches.Count + 1}";

            var newPatchItem = new PatchListItem
            {
                Name = newPatch.Name,
                PatchNumber = Patches.Count + 1
            };

            Patches.Add(newPatchItem);

            // Select the new patch
            SelectPatch(newPatchItem);
        }

        public void SelectPatch(PatchListItem patchItem)
        {
            // Update selection state in the collection
            foreach (var item in Patches)
            {
                item.IsSelected = (item == patchItem);
            }

            // In a real app, we would load the actual patch data from storage
            CurrentPatch.Name = patchItem.Name;
        }
        
        public async Task<bool> SelectPatchAsync(PatchListItem patchItem)
        {
            try
            {
                // Update selection state in the collection
                foreach (var item in Patches)
                {
                    item.IsSelected = (item == patchItem);
                }
                
                // Load the actual patch data from storage if we have a file path
                if (!string.IsNullOrEmpty(patchItem.FilePath) && File.Exists(patchItem.FilePath))
                {
                    var patch = await ParseRc0FileAsync(patchItem.FilePath);
                    if (patch != null)
                    {
                        CurrentPatch = patch;
                        return true;
                    }
                }
                
                // Fallback if we couldn't load the patch
                CurrentPatch.Name = patchItem.Name;
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error selecting patch: {ex.Message}");
                return false;
            }
        }
          public async Task<bool> ImportPatch(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    return false;
                }

                var patch = await ParseRc0FileAsync(filePath);
                if (patch == null)
                {
                    return false;
                }

                // Try to extract patch number and variation from filename
                var fileName = Path.GetFileNameWithoutExtension(filePath);
                int patchNumber = Patches.Count + 1;
                char variation = 'A';
                
                if (fileName.Length >= 10 && fileName.StartsWith("MEMORY"))
                {
                    if (int.TryParse(fileName.Substring(6, 3), out int num) && 
                        char.TryParse(fileName.Substring(9, 1), out char var))
                    {
                        patchNumber = num;
                        variation = var;
                    }
                }
                
                var patchItem = new PatchListItem
                {
                    Name = patch.Name,
                    PatchNumber = patchNumber,
                    Variation = variation,
                    FilePath = filePath
                };
                Patches.Add(patchItem);

                await SelectPatchAsync(patchItem);
                
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error importing patch: {ex.Message}");
                return false;
            }
        }
        
        public async Task<bool> ExportPatch(string filePath)
        {
            try
            {
                // Use the RCEditor.Models.Services.PatchExportService directly with our models
                var exportService = new RCEditor.Models.Services.PatchExportService();
                return await exportService.ExportPatchAsync(CurrentPatch, filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error exporting patch: {ex.Message}");
                return false;
            }
        }        public async Task<bool> LoadPatchDirectory(string directoryPath)
        {
            try
            {
                // Clear existing patches
                Patches.Clear();
                
                // Save the current directory
                CurrentDirectory = directoryPath;
                
                // Load the system settings first
                await LoadSystemSettingsAsync(directoryPath);
                
                // Find all RC0 files that are memory patches (not system files)
                var patchFiles = Directory.EnumerateFiles(directoryPath, "MEMORY*.RC0", SearchOption.AllDirectories);
                if (!patchFiles.Any())
                {
                    // Also check for lowercase extension
                    patchFiles = Directory.EnumerateFiles(directoryPath, "MEMORY*.rc0", SearchOption.AllDirectories);
                }
                
                // Keep track of patch numbers for sorting
                var patchItems = new List<PatchListItem>();
                
                foreach (var file in patchFiles)
                {
                    // Extract patch number and variation from filename
                    var fileName = Path.GetFileNameWithoutExtension(file);
                    if (fileName.Length >= 10 && fileName.StartsWith("MEMORY"))
                    {
                        if (int.TryParse(fileName.Substring(6, 3), out int patchNumber) && 
                            char.TryParse(fileName.Substring(9, 1), out char variation))
                        {
                            var patch = await ParseRc0FileAsync(file);
                            if (patch != null)
                            {
                                var patchItem = new PatchListItem
                                {
                                    Name = patch.Name,
                                    PatchNumber = patchNumber,
                                    Variation = variation,
                                    FilePath = file
                                };
                                
                                patchItems.Add(patchItem);
                            }
                        }
                    }
                }
                
                // Sort patches by number and variation
                var sortedPatches = patchItems.OrderBy(p => p.PatchNumber).ThenBy(p => p.Variation);
                
                // Add to observable collection
                foreach (var patchItem in sortedPatches)
                {
                    Patches.Add(patchItem);
                }
                
                // Select first patch if available
                if (Patches.Count > 0)
                {
                    await SelectPatchAsync(Patches[0]);
                }
                else
                {
                    // No patches found
                    CurrentPatch = new RCEditor.Models.MemoryPatch();
                }
                
                return Patches.Count > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading patch directory: {ex.Message}");
                return false;
            }
        }
        private async Task LoadSystemSettingsAsync(string directoryPath)
        {
            try
            {
                // Look for SYSTEM1.RC0 file with case insensitive search
                var system1Path = FindSystemFile(directoryPath, "SYSTEM1.RC0");
                
                // If system file found, load it
                if (File.Exists(system1Path))
                {
                    // Create a new SystemSettings instance
                    var settings = new RCEditor.Models.SystemSettings 
                    { 
                        FileName = Path.GetFileName(system1Path)
                    };
                    
                    // Parse the system file content
                    try
                    {
                        // Use the same parsing logic as patches but handle it differently
                        var content = await ParseSystemFileAsync(system1Path);
                        
                        // Set some example values based on the content
                        // In a real implementation, you would extract these from the parsed XML
                        if (content != null)
                        {
                            // Set mixer master out level - default is 100 if not found
                            settings.Mixer.MasterOut = 100;
                            
                            // Set reverb level - default is 0 if not found
                            settings.Output.MasterFx.Reverb = 0;
                            
                            // Look for SYSTEM2.RC0 as well
                            var system2Path = FindSystemFile(directoryPath, "SYSTEM2.RC0");
                            if (File.Exists(system2Path))
                            {
                                // Add info about SYSTEM2.RC0
                                settings.FileName += $", {Path.GetFileName(system2Path)}";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error parsing system file: {ex.Message}");
                        // Continue with default settings even if parsing fails
                    }
                    
                    // Set the SystemSettings property which will trigger the event
                    SystemSettings = settings;
                }
                else
                {
                    SystemSettings = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading system settings: {ex.Message}");
                SystemSettings = null;
            }
              // No need to return Task.CompletedTask in async Task method
        }
        
        public async Task<bool> ImportPatchDirectory(string directoryPath)
        {
            try
            {
                var rc0Files = Directory.GetFiles(directoryPath, "*.rc0", SearchOption.AllDirectories);
                foreach (var file in rc0Files)
                {
                    var patch = await ParseRc0FileAsync(file);
                    if (patch != null)
                    {
                        // Try to extract patch number and variation from filename
                        var fileName = Path.GetFileNameWithoutExtension(file);
                        int patchNumber = Patches.Count + 1;
                        char variation = 'A';
                        
                        if (fileName.Length >= 10 && fileName.StartsWith("MEMORY"))
                        {
                            if (int.TryParse(fileName.Substring(6, 3), out int num) && 
                                char.TryParse(fileName.Substring(9, 1), out char var))
                            {
                                patchNumber = num;
                                variation = var;
                            }
                        }
                        
                        var patchItem = new PatchListItem
                        {
                            Name = patch.Name,
                            PatchNumber = patchNumber,
                            Variation = variation,
                            FilePath = file
                        };

                        Patches.Add(patchItem);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error importing patches: {ex.Message}");
                return false;
            }
        }
        
        // Updated method: if the file is a .rc0 file, use our custom loader.        
        private async Task<RCEditor.Models.MemoryPatch?> ParseRc0FileAsync(string filePath)
        {
            if (Path.GetExtension(filePath).Equals(".rc0", StringComparison.OrdinalIgnoreCase))
            {
                return await LoadCustomRc0FileAsync(filePath);
            }

            try
            {
                var sanitizedXml = PreprocessXml(filePath);
                var doc = XDocument.Parse(sanitizedXml);
                var memElement = doc.Descendants("mem").FirstOrDefault();
                if (memElement == null) return null;

                var patch = new RCEditor.Models.MemoryPatch();
                var nameElement = memElement.Element("NAME");
                if (nameElement != null)
                {
                    var nameChars = nameElement.Elements()
                        .Select(e => (char)int.Parse(e.Value))
                        .ToArray();
                    patch.Name = new string(nameChars).TrimEnd();
                }

                for (int i = 1; i <= 6; i++)
                {
                    var trackElement = memElement.Element($"TRACK{i}");
                    if (trackElement != null)
                    {
                        patch.Tracks[i - 1] = new RCEditor.Models.Track
                        {
                            Level = int.Parse(trackElement.Element("C")?.Value ?? "0"),
                            Pan = int.Parse(trackElement.Element("D")?.Value ?? "0")
                        };
                    }
                }

                return patch;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing RC0 file: {ex.Message}");
                return null;
            }
        }
        
        // Updated custom loader: wrap the content so that only one root exists.
        public Task<RCEditor.Models.MemoryPatch?> LoadCustomRc0FileAsync(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException($"File not found: {filePath}");
                }

                string content = File.ReadAllText(filePath);

                // Replace invalid element names (e.g., <0>, <#>) with valid ones.
                content = Regex.Replace(content, @"<(\d+)>", m => $"<Element{m.Groups[1].Value}>");
                content = Regex.Replace(content, @"<(\#)>", "<ElementHash>");
                content = Regex.Replace(content, @"</(\d+)>", m => $"</Element{m.Groups[1].Value}>");
                content = Regex.Replace(content, @"</(\#)>", "</ElementHash>");

                // Remove any XML declaration.
                content = Regex.Replace(content, @"<\?xml.*?\?>", string.Empty, RegexOptions.Singleline);
                // Wrap the entire content in a dummy root to avoid multiple root errors.
                content = $"<Wrapper>{content}</Wrapper>";

                var doc = XDocument.Parse(content);
                // Locate the <mem> element anywhere in the document.
                var memElement = doc.Descendants("mem").FirstOrDefault();
                if (memElement == null) return Task.FromResult<RCEditor.Models.MemoryPatch?>(null);

                var patch = new RCEditor.Models.MemoryPatch();
                var nameElement = memElement.Element("NAME");
                if (nameElement != null)
                {
                    var nameChars = nameElement.Elements()
                        .Select(e => (char)int.Parse(e.Value))
                        .ToArray();
                    patch.Name = new string(nameChars).TrimEnd();
                }

                for (int i = 1; i <= 6; i++)
                {
                    var trackElement = memElement.Element($"TRACK{i}");
                    if (trackElement != null)
                    {
                        int level = int.Parse(trackElement.Element("C")?.Value ?? "0");
                        int pan = int.Parse(trackElement.Element("D")?.Value ?? "0");
                        patch.Tracks[i - 1] = new RCEditor.Models.Track
                        {
                            Level = level,
                            Pan = pan
                        };
                    }
                }

                return Task.FromResult<RCEditor.Models.MemoryPatch?>(patch);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing custom RC0 file: {ex.Message}");
                return Task.FromResult<RCEditor.Models.MemoryPatch?>(null);
            }
        }

        private string PreprocessXml(string filePath)
        {
            var content = File.ReadAllText(filePath);
            content = Regex.Replace(content, @"<(\d+)>", m => $"<Element{m.Groups[1].Value}>");
            content = Regex.Replace(content, @"<(\#)>", "<ElementHash>");
            content = Regex.Replace(content, @"</(\d+)>", m => $"</Element{m.Groups[1].Value}>");
            content = Regex.Replace(content, @"</(\#)>", "</ElementHash>");
            return content;
        }
        
        private double ExtractTempo(byte[] data)
        {
            // TODO: Implement tempo extraction from byte data
            return 120.0;
        }

        private RCEditor.Models.PlayModeEnum ExtractPlayMode(byte[] data)
        {
            // TODO: Implement play mode extraction from byte data
            return RCEditor.Models.PlayModeEnum.Multi;
        }        private RCEditor.Models.SingleModeSwitchEnum ExtractSingleModeSwitch(byte[] data)
        {
            return RCEditor.Models.SingleModeSwitchEnum.Loop;
        }

        private bool ExtractLoopSync(byte[] data)
        {
            return false;
        }

        private RCEditor.Models.Track ExtractTrackData(byte[] data, int trackIndex)
        {
            var track = new RCEditor.Models.Track();
            return track;
        }

        private RCEditor.Models.EffectBanks ExtractEffectsData(byte[] data)
        {
            return new RCEditor.Models.EffectBanks();
        }

        private RCEditor.Models.RhythmSettings ExtractRhythmSettings(byte[] data)
        {
            return new RCEditor.Models.RhythmSettings();
        }

        private RCEditor.Models.ControlAssignments ExtractControlAssignments(byte[] data)
        {
            return new RCEditor.Models.ControlAssignments();
        }

        private List<RCEditor.Models.AssignSlot> ExtractAssignSlots(byte[] data)
        {
            return new List<RCEditor.Models.AssignSlot>();
        }

        private string FindSystemFile(string directoryPath, string fileName)
        {
            // Check case-sensitive paths first
            var paths = new List<string>
            {
                Path.Combine(directoryPath, "DATA", fileName),
                Path.Combine(directoryPath, fileName)
            };
            
            // Then try lowercase extension
            var lcFileName = Path.GetFileNameWithoutExtension(fileName) + ".rc0";
            paths.Add(Path.Combine(directoryPath, "DATA", lcFileName));
            paths.Add(Path.Combine(directoryPath, lcFileName));
            
            // Return the first path that exists
            return paths.FirstOrDefault(File.Exists) ?? paths[0]; // Default to first path if none exists
        }
        
        private async Task<XDocument> ParseSystemFileAsync(string filePath)
        {
            try
            {
                string content = await File.ReadAllTextAsync(filePath);
                
                // Apply the same XML preprocessing as for patches
                content = Regex.Replace(content, @"<(\d+)>", m => $"<Element{m.Groups[1].Value}>");
                content = Regex.Replace(content, @"<(\#)>", "<ElementHash>");
                content = Regex.Replace(content, @"</(\d+)>", m => $"</Element{m.Groups[1].Value}>");
                content = Regex.Replace(content, @"</(\#)>", "</ElementHash>");
                
                // Remove any XML declaration
                content = Regex.Replace(content, @"<\?xml.*?\?>", string.Empty, RegexOptions.Singleline);
                
                // Wrap the entire content in a dummy root
                content = $"<Wrapper>{content}</Wrapper>";
                
                return XDocument.Parse(content);
            }
            catch (Exception ex)
            {                Console.WriteLine($"Error parsing system file XML: {ex.Message}");
                return new XDocument(new XElement("Wrapper"));
            }
        }
    }
}