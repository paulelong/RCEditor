using RCEditor.Models;
using System.Collections.ObjectModel;
using System.Xml.Linq; // Add this namespace for XDocument
using System.Text.RegularExpressions; // Add this namespace for Regex

namespace RCEditor.Services
{    public class PatchService
    {
        private static PatchService? _instance;
        public static PatchService Instance => _instance ??= new PatchService();

        public event EventHandler<RCEditor.Models.MemoryPatch>? CurrentPatchChanged;

        private RCEditor.Models.MemoryPatch _currentPatch;        public ObservableCollection<PatchListItem> Patches { get; private set; }

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

        private PatchService()
        {
            // Initialize with some dummy data
            Patches = new ObservableCollection<PatchListItem>();

            // Create some sample patches
            for (int i = 1; i <= 10; i++)
            {
                Patches.Add(new PatchListItem
                {
                    Name = $"Patch {i}",
                    PatchNumber = i
                });
            }

            // Create a default current patch
            _currentPatch = new RCEditor.Models.MemoryPatch();
        }        public void CreateNewPatch()
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
        }        public async Task<bool> ImportPatch(string filePath)
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

                var patchItem = new PatchListItem
                {
                    Name = patch.Name,
                    PatchNumber = Patches.Count + 1
                };
                Patches.Add(patchItem);

                SelectPatch(patchItem);
                CurrentPatch = patch;

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error importing patch: {ex.Message}");
                return false;
            }
        }        public async Task<bool> ExportPatch(string filePath)
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
                        var patchItem = new PatchListItem
                        {
                            Name = patch.Name,
                            PatchNumber = Patches.Count + 1
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
        }        // Updated method: if the file is a .rc0 file, use our custom loader.
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
        }        // Updated custom loader: wrap the content so that only one root exists.
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
        }        private double ExtractTempo(byte[] data)
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
    }
}