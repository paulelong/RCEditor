using RCEditor.Models;
using System.Collections.ObjectModel;
using System.Xml.Linq; // Add this namespace for XDocument
using System.Text.RegularExpressions; // Add this namespace for Regex

namespace RCEditor.Services
{
    public class PatchService
    {
        private static PatchService _instance;
        public static PatchService Instance => _instance ??= new PatchService();

        public event EventHandler<MemoryPatch> CurrentPatchChanged;

        private MemoryPatch _currentPatch;

        public ObservableCollection<PatchListItem> Patches { get; private set; }

        public MemoryPatch CurrentPatch
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
            _currentPatch = new MemoryPatch();
        }

        public void CreateNewPatch()
        {
            var newPatch = new MemoryPatch();
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
                // First, convert our local MemoryPatch to the Models.MemoryPatch format
                var modelsPatch = ConvertToModelsPatch(CurrentPatch);
                
                // Then use the PatchExportService to write the file
                var exportService = new RCEditor.Models.Services.PatchExportService();
                return await exportService.ExportPatchAsync(modelsPatch, filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error exporting patch: {ex.Message}");
                return false;
            }
        }
        
        // Helper method to convert from our local MemoryPatch model to the RCEditor.Models version
        private RCEditor.Models.MemoryPatch ConvertToModelsPatch(MemoryPatch patch)
        {
            var modelsPatch = new RCEditor.Models.MemoryPatch
            {
                Name = patch.Name
            };
            
            // Convert tracks
            for (int i = 0; i < Math.Min(patch.Tracks.Length, modelsPatch.Tracks.Length); i++)
            {
                var srcTrack = patch.Tracks[i];
                var destTrack = new RCEditor.Models.Track
                {
                    Level = srcTrack.Level,
                    Pan = srcTrack.Pan,
                    Reverse = srcTrack.Reverse,
                    OneShot = srcTrack.OneShot,
                    FXEnabled = srcTrack.FXEnabled,
                    MeasureCount = srcTrack.MeasureCount,
                    BounceIn = srcTrack.BounceIn
                    // Add other properties as needed
                };
                
                modelsPatch.Tracks[i] = destTrack;
            }
            
            // Set basic Rhythm settings
            modelsPatch.Rhythm.Genre = patch.Rhythm.Genre;
            modelsPatch.Rhythm.Pattern = patch.Rhythm.Pattern;
            modelsPatch.Rhythm.Variation = patch.Rhythm.Variation;
              // Set Play settings
            modelsPatch.Play.SingleTrackChange = (RCEditor.Models.SingleTrackChangeEnum)(int)patch.Play.SingleTrackChange;
            modelsPatch.Play.FadeTimeIn = patch.Play.FadeTimeIn;
            modelsPatch.Play.FadeTimeOut = patch.Play.FadeTimeOut;
            
            // Copy AllStartTracks and AllStopTracks arrays
            for (int i = 0; i < Math.Min(patch.Play.AllStartTracks.Length, modelsPatch.Play.AllStartTracks.Length); i++)
            {
                modelsPatch.Play.AllStartTracks[i] = patch.Play.AllStartTracks[i];
                modelsPatch.Play.AllStopTracks[i] = patch.Play.AllStopTracks[i];
            }
            
            // Set Rec settings
            modelsPatch.Rec.RecAction = (RCEditor.Models.RecActionEnum)(int)patch.Rec.RecAction;
            modelsPatch.Rec.QuantizeEnabled = patch.Rec.QuantizeEnabled;
            modelsPatch.Rec.AutoRecEnabled = patch.Rec.AutoRecEnabled;
            modelsPatch.Rec.AutoRecSensitivity = patch.Rec.AutoRecSensitivity;
            modelsPatch.Rec.BounceEnabled = patch.Rec.BounceEnabled;
            modelsPatch.Rec.BounceTrack = patch.Rec.BounceTrack;
            
            // Add more conversions as needed for a complete patch
            
            return modelsPatch;
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
        }

        // Updated method: if the file is a .rc0 file, use our custom loader.
        private async Task<MemoryPatch> ParseRc0FileAsync(string filePath)
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

                var patch = new MemoryPatch();
                var nameElement = memElement.Element("NAME");
                if (nameElement != null)
                {
                    var nameChars = nameElement.Elements()
                        .Select(e => (char)int.Parse(e.Value))
                        .ToArray();
                    patch.Name = new string(nameChars).TrimEnd();
                }

                patch.Tracks = new Track[6];
                for (int i = 1; i <= 6; i++)
                {
                    var trackElement = memElement.Element($"TRACK{i}");
                    if (trackElement != null)
                    {
                        patch.Tracks[i - 1] = new Track
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
        public async Task<MemoryPatch> LoadCustomRc0FileAsync(string filePath)
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
                if (memElement == null) return null;

                var patch = new MemoryPatch();
                var nameElement = memElement.Element("NAME");
                if (nameElement != null)
                {
                    var nameChars = nameElement.Elements()
                        .Select(e => (char)int.Parse(e.Value))
                        .ToArray();
                    patch.Name = new string(nameChars).TrimEnd();
                }

                patch.Tracks = new Track[6];
                for (int i = 1; i <= 6; i++)
                {
                    var trackElement = memElement.Element($"TRACK{i}");
                    if (trackElement != null)
                    {
                        int level = int.Parse(trackElement.Element("C")?.Value ?? "0");
                        int pan = int.Parse(trackElement.Element("D")?.Value ?? "0");
                        patch.Tracks[i - 1] = new Track
                        {
                            Level = level,
                            Pan = pan
                        };
                    }
                }

                return patch;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing custom RC0 file: {ex.Message}");
                return null;
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
            const int tempoOffset = 100;
            return 120.0;
        }

        private PlayModeEnum ExtractPlayMode(byte[] data)
        {
            const int playModeOffset = 120;
            return PlayModeEnum.Multi;
        }

        private SingleModeSwitchEnum ExtractSingleModeSwitch(byte[] data)
        {
            return SingleModeSwitchEnum.Loop;
        }

        private bool ExtractLoopSync(byte[] data)
        {
            return false;
        }

        private Track ExtractTrackData(byte[] data, int trackIndex)
        {
            var track = new Track();
            return track;
        }

        private EffectBanks ExtractEffectsData(byte[] data)
        {
            return new EffectBanks();
        }

        private RhythmSettings ExtractRhythmSettings(byte[] data)
        {
            return new RhythmSettings();
        }

        private ControlAssignments ExtractControlAssignments(byte[] data)
        {
            return new ControlAssignments();
        }

        private List<AssignSlot> ExtractAssignSlots(byte[] data)
        {
            return new List<AssignSlot>();
        }
    }
}