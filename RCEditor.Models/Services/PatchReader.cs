using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading.Tasks;
using RCEditor.Models;

namespace RCEditor.Models.Services
{
    public class PatchReader
    {
        private static readonly Regex GroupTagRegex = new Regex(@"<([A-Z0-9]+)>(.*?)</\1>", RegexOptions.Compiled | RegexOptions.Singleline);
        private static readonly Regex ParameterTagRegex = new Regex(@"<([A-Z#0-9]+)>([^<]*)</\1>", RegexOptions.Compiled);

        // Custom recursive parser for handling nested XML-like tags
        private Dictionary<string, string> ParseTagsRecursively(string content)
        {
            var result = new Dictionary<string, string>();
            int position = 0;
            
            while (position < content.Length)
            {
                // Find next opening tag
                int tagStart = content.IndexOf('<', position);
                if (tagStart == -1) break;
                
                // Find tag name
                int tagNameEnd = content.IndexOf('>', tagStart);
                if (tagNameEnd == -1) break;
                
                string tagName = content.Substring(tagStart + 1, tagNameEnd - tagStart - 1);
                
                // Skip if this is a closing tag
                if (tagName.StartsWith("/"))
                {
                    position = tagNameEnd + 1;
                    continue;
                }
                
                // Find matching closing tag
                string closingTag = $"</{tagName}>";
                int closingTagPosition = FindMatchingClosingTag(content, tagNameEnd + 1, tagName);
                
                if (closingTagPosition != -1)
                {
                    // Extract content between tags
                    string tagContent = content.Substring(tagNameEnd + 1, 
                        closingTagPosition - tagNameEnd - 1);
                    
                    result[tagName] = tagContent;
                    position = closingTagPosition + closingTag.Length;
                }
                else
                {
                    position = tagNameEnd + 1;
                }
            }
            
            return result;
        }
        
        private int FindMatchingClosingTag(string content, int startPosition, string tagName)
        {
            // Need to track nested levels of the same tag
            int nestedLevel = 1;
            int position = startPosition;
            
            while (position < content.Length && nestedLevel > 0)
            {
                // Find the next opening or closing tag
                int nextOpenTag = content.IndexOf($"<{tagName}>", position);
                int nextCloseTag = content.IndexOf($"</{tagName}>", position);
                
                if (nextCloseTag == -1) return -1; // No matching closing tag
                
                if (nextOpenTag != -1 && nextOpenTag < nextCloseTag)
                {
                    // Found a nested opening tag
                    nestedLevel++;
                    position = nextOpenTag + tagName.Length + 2;
                }
                else
                {
                    // Found a closing tag
                    nestedLevel--;
                    position = nextCloseTag + tagName.Length + 3;
                    
                    if (nestedLevel == 0)
                    {
                        return nextCloseTag;
                    }
                }
            }
            
            return -1; // No matching closing tag found
        }

        // Parse a specific tag's content into parameters
        private Dictionary<string, int> ParseTagParameters(string content)
        {
            var parameters = new Dictionary<string, int>();
            var paramTags = ParseTagsRecursively(content);
            
            foreach (var param in paramTags)
            {
                // Parse integer value
                if (int.TryParse(param.Value, out int intValue))
                {
                    parameters[param.Key] = intValue;
                }
            }
            
            return parameters;
        }

        public async Task<MemoryPatch?> ReadPatchAsync(string filePath)
        {
            string content = await File.ReadAllTextAsync(filePath, Encoding.UTF8);
            
            // Parse the patch data from the XML-like content
            var patch = new MemoryPatch();
            
            // The file has two main sections:
            // 1. <mem> section for memory patch settings
            // 2. <ifx> section for input effects
            // 3. <tfx> section for track effects
            
            // Process the <mem> section
            Match memMatch = Regex.Match(content, @"<mem id=""([^""]*)"">(.*?)</mem>", RegexOptions.Singleline);
            if (memMatch.Success)
            {
                // Extract memory patch ID from the mem tag
                string memIdStr = memMatch.Groups[1].Value;
                if (int.TryParse(memIdStr, out int memId))
                {
                    patch.Id = memId;
                }
                
                string memContent = memMatch.Groups[2].Value;
                // Process each group tag and its content in the mem section
                foreach (Match groupMatch in GroupTagRegex.Matches(memContent))
                {
                    string groupTag = groupMatch.Groups[1].Value;
                    string groupContent = groupMatch.Groups[2].Value;
                    
                    // Parse parameters within the group
                    var parameters = new Dictionary<string, int>();
                    foreach (Match paramMatch in ParameterTagRegex.Matches(groupContent))
                    {
                        string tag = paramMatch.Groups[1].Value;
                        string value = paramMatch.Groups[2].Value;
                        
                        // Parse integer value
                        if (int.TryParse(value, out int intValue))
                        {
                            parameters[tag] = intValue;
                        }
                    }
                    
                    // Process each group based on its tag
                    ProcessMemGroup(patch, groupTag, parameters);
                }
            }
            
            // Process the <ifx> section for input effects
            Match ifxMatch = Regex.Match(content, @"<ifx id=""[^""]*"">(.*?)</ifx>", RegexOptions.Singleline);
            if (ifxMatch.Success)
            {
                string ifxContent = ifxMatch.Groups[1].Value;
                ProcessInputFXSection(patch, ifxContent);
            }
              // Process the <tfx> section for track effects
            Match tfxMatch = Regex.Match(content, @"<tfx id=""[^""]*"">(.*?)</tfx>", RegexOptions.Singleline);
            if (tfxMatch.Success)
            {
                string tfxContent = tfxMatch.Groups[1].Value;
                ProcessTrackFXSection(patch, tfxContent);
            }
            
            // Process the control settings from the content
            // This section extracts and processes all control settings (ICTL, ECTL, etc.)
            patch.ControlSettings = ControlSettingsParser.ParseControlSettings(content);
            
            return patch;
        }
          private void ProcessMemGroup(MemoryPatch patch, string groupTag, Dictionary<string, int> parameters)
        {
            switch (groupTag)
            {
                case "NAME":
                    ProcessNameGroup(patch, parameters);
                    break;
                    
                case "PLAY":
                    ProcessPlayGroup(patch, parameters);
                    break;
                    
                case "REC":
                    ProcessRecGroup(patch, parameters);
                    break;
                    
                case "TRACK1":
                case "TRACK2":
                case "TRACK3":
                case "TRACK4":
                case "TRACK5":
                case "TRACK6":
                    ProcessTrackGroup(patch, groupTag, parameters);
                    break;
                    
                case "MASTER":
                    ProcessMasterGroup(patch, parameters);
                    break;
                    
                case "RHYTHM":
                    ProcessRhythmGroup(patch, parameters);
                    break;
                    
                case "CONTROL":
                    ProcessControlGroup(patch, parameters);
                    break;
                    
                case "ASSIGN":
                    ProcessAssignGroup(patch, parameters);
                    break;
            }
        }
        
        private void ProcessNameGroup(MemoryPatch patch, Dictionary<string, int> parameters)
        {
            // For NAME section, we typically need to handle it differently
            // There might be text fields that aren't just integer parameters
            if (patch.Name == null)
            {
                patch.Name = "Untitled";
            }
            
            // Build name from ASCII values stored in single-letter parameters (A, B, C, etc.)
            var nameChars = new List<KeyValuePair<char, int>>();
            
            // Collect all single-letter parameters which represent characters in the name
            foreach (var param in parameters)
            {
                if (param.Key.Length == 1 && char.IsLetter(param.Key[0]))
                {
                    char position = param.Key[0];
                    nameChars.Add(new KeyValuePair<char, int>(position, param.Value));
                }
            }
            
            // Sort by parameter key (A, B, C, etc.) to get correct character order
            nameChars.Sort((a, b) => a.Key.CompareTo(b.Key));
            
            // Build name from ASCII values
            if (nameChars.Count > 0)
            {
                StringBuilder nameBuilder = new StringBuilder();
                foreach (var charPair in nameChars)
                {
                    // Convert integer value to ASCII character
                    if (charPair.Value >= 32 && charPair.Value <= 126) // Printable ASCII range
                    {
                        nameBuilder.Append((char)charPair.Value);
                    }
                }
                
                string newName = nameBuilder.ToString().Trim();
                if (!string.IsNullOrEmpty(newName))
                {
                    patch.Name = newName;
                }
            }
        }
        
        private void ProcessPlayGroup(MemoryPatch patch, Dictionary<string, int> parameters)
        {
            // Process each parameter from the PLAY section using letter tags
            // A -> Single Track Change
            // B -> Fade Time In (there is no Current Track parameter)
            // C -> Fade Time Out
            // D -> All Start Tracks
            // E -> All Stop Tracks
            // F -> Loop Length
            // G -> Speed Change
            // H -> Sync Adjust
            
            if (parameters.ContainsKey("A"))
            {
                patch.Play.SingleTrackChange = (SingleTrackChangeEnum)parameters["A"];
            }
            
            if (parameters.ContainsKey("B"))
            {
                patch.Play.FadeTimeIn = parameters["B"];
            }
            
            if (parameters.ContainsKey("C"))
            {
                patch.Play.FadeTimeOut = parameters["C"];
            }
            
            if (parameters.ContainsKey("D"))
            {
                // Set all start tracks based on the bit mask
                for (int i = 0; i < 6; i++)
                {
                    patch.Play.AllStartTracks[i] = ((parameters["D"] >> i) & 1) == 1;
                }
            }
            
            if (parameters.ContainsKey("E"))
            {
                // Set all stop tracks based on the bit mask
                for (int i = 0; i < 6; i++)
                {
                    patch.Play.AllStopTracks[i] = ((parameters["E"] >> i) & 1) == 1;
                }
            }
            
            if (parameters.ContainsKey("F"))
            {
                patch.Play.LoopLength = parameters["F"];
            }
            
            if (parameters.ContainsKey("G"))
            {
                patch.Play.SpeedChange = (SpeedChangeEnum)parameters["G"];
            }
              if (parameters.ContainsKey("H"))
            {
                patch.Play.SyncAdjust = (SyncAdjustEnum)parameters["H"];
            }
            
            // Note: 'I' (Tempo) parameter is not saved in PlaySettings
        }
        
        private void ProcessRecGroup(MemoryPatch patch, Dictionary<string, int> parameters)
        {
            // Process each parameter from the REC section using letter tags
            // A -> Rec Action
            // B -> Quantize Enabled
            // C -> Auto Rec Enabled
            // D -> Auto Rec Sensitivity
            // E -> Bounce Enabled
            // F -> Bounce Track
            
            if (parameters.ContainsKey("A"))
            {
                patch.Rec.RecAction = (RecActionEnum)parameters["A"];
            }
            
            if (parameters.ContainsKey("B"))
            {
                patch.Rec.QuantizeEnabled = parameters["B"] == 1;
            }
            
            if (parameters.ContainsKey("C"))
            {
                patch.Rec.AutoRecEnabled = parameters["C"] == 1;
            }
            
            if (parameters.ContainsKey("D"))
            {
                patch.Rec.AutoRecSensitivity = parameters["D"];
            }
            
            if (parameters.ContainsKey("E"))
            {
                patch.Rec.BounceEnabled = parameters["E"] == 1;
            }
            
            if (parameters.ContainsKey("F"))
            {
                patch.Rec.BounceTrack = parameters["F"];
            }
            
            // Add any other rec parameters as needed
        }
          private void ProcessTrackGroup(MemoryPatch patch, string groupTag, Dictionary<string, int> parameters)
        {
            // Extract track number from the tag (e.g., "TRACK1" -> 1)
            int trackNumber = int.Parse(groupTag.Replace("TRACK", ""));
            
            // Adjust for 0-based indexing
            trackNumber -= 1;
            
            // Ensure we're within bounds
            if (trackNumber < 0 || trackNumber >= patch.Tracks.Length)
            {
                return;
            }
            
            var track = patch.Tracks[trackNumber];
            
            // Process each parameter for this track using letter tags
            // A -> Reverse
            // B -> One Shot
            // C -> Pan
            // D -> Play Level
            // E -> Start Mode
            // F -> Stop Mode
            // G -> Overdub Mode
            // H -> FX Enabled
            // I -> Play Mode
            // J -> Measure Count
            // K -> Unknown Parameter K
            // L -> Loop Sync Switch
            // M -> Tempo Sync Switch
            // N -> Tempo Sync Mode
            // O -> Tempo Sync Speed
            // P -> Unknown Parameter P
            // Q -> Input Mic1/Mic2
            // R -> Input Inst1
            // S -> Input Inst2
            // T -> Input Rhythm
            // U -> Unknown Parameter U
            // V -> Unknown Parameter V
            // W -> Bounce In
            // X -> Unknown Parameter X
            // Y -> Loop Sync Mode
            
            if (parameters.ContainsKey("A"))
            {
                track.Reverse = parameters["A"] == 1;
            }
            
            if (parameters.ContainsKey("B"))
            {
                track.OneShot = parameters["B"] == 1;
            }
            
            if (parameters.ContainsKey("C"))
            {
                track.Pan = parameters["C"] - 50; // Convert from 0-100 to -50 to +50
            }
            
            if (parameters.ContainsKey("D"))
            {
                track.Level = parameters["D"];
            }
            
            if (parameters.ContainsKey("E"))
            {
                track.StartMode = (StartModeEnum)parameters["E"];
            }
            
            if (parameters.ContainsKey("F"))
            {
                track.StopMode = (StopModeEnum)parameters["F"];
            }
            
            if (parameters.ContainsKey("G"))
            {
                track.OverdubMode = (OverdubModeEnum)parameters["G"];
            }
              if (parameters.ContainsKey("H"))
            {
                track.FXEnabled = parameters["H"] == 1;
            }
            
            if (parameters.ContainsKey("I"))
            {
                track.PlayMode = (PlayModeEnum)parameters["I"];
            }
            
            if (parameters.ContainsKey("J"))
            {
                track.MeasureCount = parameters["J"];
            }
            
            // K is an unknown parameter
            if (parameters.ContainsKey("K"))
            {
                track.UnknownK = parameters["K"];
            }
            
            if (parameters.ContainsKey("L"))
            {
                track.LoopSyncSw = parameters["L"] == 1;
            }
            
            if (parameters.ContainsKey("M"))
            {
                track.TempoSyncSw = parameters["M"] == 1;
            }
            
            if (parameters.ContainsKey("N"))
            {
                track.TempoSyncMode = (TempoSyncModeEnum)parameters["N"];
            }
            
            if (parameters.ContainsKey("O"))
            {
                track.TempoSyncSpeed = (TempoSyncSpeedEnum)parameters["O"];
            }
              if (parameters.ContainsKey("P"))
            {
                // In the updated documentation, P is described as unknown
                track.UnknownP = parameters["P"];
            }
            
            // R is an unknown parameter in the updated docs
            if (parameters.ContainsKey("R"))
            {
                track.UnknownR = parameters["R"];
            }
            // S is MEASUREB in the updated docs
            if (parameters.ContainsKey("S"))
            {
                track.MeasureCountB = parameters["S"];
            }
            
            // T is an unknown parameter
            if (parameters.ContainsKey("T"))
            {
                track.UnknownT = parameters["T"];
            }
            
            // U is an unknown parameter
            if (parameters.ContainsKey("U"))
            {
                track.UnknownU = parameters["U"];
            }
            
            // V is an unknown parameter
            if (parameters.ContainsKey("V"))
            {
                track.UnknownV = parameters["V"];
            }
            
            // W is BOUNCE IN
            if (parameters.ContainsKey("W"))
            {
                track.BounceIn = parameters["W"] == 1;
            }
            
            // X is an unknown parameter
            if (parameters.ContainsKey("X"))
            {
                track.UnknownX = parameters["X"];
            }
            
            // Y is LOOP SYNC MODE
            if (parameters.ContainsKey("Y"))
            {
                track.LoopSyncMode = (LoopSyncModeEnum)parameters["Y"];
            }              // Handle input routing bit values (Tag Q in the document)
            if (parameters.ContainsKey("Q"))
            {
                int inputMask = parameters["Q"];
                
                // Check each bit position for the inputs
                // Bit 0: MIC1
                track.InputRouting.Mic1Enabled = (inputMask & 0x01) != 0;
                
                // Bit 1: MIC2
                track.InputRouting.Mic2Enabled = (inputMask & 0x02) != 0;
                
                // Bit 2: INST1
                track.InputRouting.Inst1Enabled = (inputMask & 0x04) != 0;
                
                // Bit 3: INST2
                track.InputRouting.Inst2Enabled = (inputMask & 0x08) != 0;
                
                // Bit 4: RHYTHM
                track.InputRouting.RhythmEnabled = (inputMask & 0x10) != 0;
            }
        }
          private void ProcessRhythmGroup(MemoryPatch patch, Dictionary<string, int> parameters)
        {
            // Process each parameter from the RHYTHM section according to RC600Param.md
            // A -> Genre
            // B -> Pattern
            // C -> Variation
            // D -> VAR.CHANGE (Variation Change Timing)
            // E -> Kit
            // F -> Beat
            // G -> Fill
            // H -> Intro Rec
            // I -> Intro Play
            // J -> Ending
            // K -> Stop Trig
            // L -> Start Trig
            // M -> Unknown (preserving for future use)
            
            if (parameters.ContainsKey("A"))
            {
                patch.Rhythm.Genre = parameters["A"].ToString();
            }
            
            if (parameters.ContainsKey("B"))
            {
                // Store both the numeric pattern ID and pattern number (1-based)
                patch.Rhythm.PatternId = parameters["B"] - 1;
                patch.Rhythm.Pattern = (parameters["B"]).ToString();
            }
            
            if (parameters.ContainsKey("C"))
            {
                patch.Rhythm.Variation = (char)('A' + parameters["C"]);
            }
            
            if (parameters.ContainsKey("D"))
            {
                patch.Rhythm.VariationChangeTiming = (RhythmVariationChangeEnum)parameters["D"];
            }
            
            if (parameters.ContainsKey("E"))
            {
                patch.Rhythm.Kit = parameters["E"].ToString();
            }
            
            if (parameters.ContainsKey("F"))
            {
                patch.Rhythm.Beat = parameters["F"].ToString();
            }
            
            if (parameters.ContainsKey("G"))
            {
                patch.Rhythm.FillIn = parameters["G"] == 1;
            }
            
            if (parameters.ContainsKey("H"))
            {
                patch.Rhythm.IntroOnRec = parameters["H"] == 1;
            }
            
            if (parameters.ContainsKey("I"))
            {
                patch.Rhythm.IntroOnPlay = parameters["I"] == 1;
            }
            
            if (parameters.ContainsKey("J"))
            {
                patch.Rhythm.Ending = parameters["J"] == 1;
            }
            
            if (parameters.ContainsKey("K"))
            {
                patch.Rhythm.StopMode = (RhythmStopTrigEnum)parameters["K"];
            }
            
            if (parameters.ContainsKey("L"))
            {
                patch.Rhythm.StartMode = (RhythmStartTrigEnum)parameters["L"];
            }
            
            if (parameters.ContainsKey("M"))
            {
                patch.Rhythm.UnknownM = parameters["M"];
            }
        }
        
        private void ProcessControlGroup(MemoryPatch patch, Dictionary<string, int> parameters)
        {
            // Process general control settings
            foreach (var param in parameters)
            {
                // Store parameter values as needed for your model
                // This would need to be adapted to your specific ControlAssignments structure
            }
        }
        
        private void ProcessAssignGroup(MemoryPatch patch, Dictionary<string, int> parameters)
        {
            // Create a new assignment
            var assign = new AssignSlot();
            
            // Process assignment parameters
            foreach (var param in parameters)
            {
                switch (param.Key)
                {
                    case "SW":
                        // Enable/disable the assignment
                        break;
                    case "SOURCE":
                        assign.Source = GetControlSourceName(param.Value);
                        break;
                    case "TARGET":
                        assign.Target = GetControlTargetName(param.Value);
                        break;
                    case "TARGET_MIN":
                        assign.RangeMin = param.Value;
                        break;
                    case "TARGET_MAX":
                        assign.RangeMax = param.Value;
                        break;
                    // Add other parameters as needed
                }
            }
            
            // Add the assignment to the list
            patch.Assigns.Add(assign);
        }
        
        private void ProcessInputFXSection(MemoryPatch patch, string ifxContent)
        {
            ProcessFXSection(patch, ifxContent, patch.InputFX.Banks, EffectSlot.Category.InputFX);
        }
        
        private void ProcessTrackFXSection(MemoryPatch patch, string tfxContent)
        {
            ProcessFXSection(patch, tfxContent, patch.TrackFX.Banks, EffectSlot.Category.TrackFX);
        }
        
        private void ProcessFXSection(MemoryPatch patch, string fxContent, Dictionary<string, EffectBank> banks, EffectSlot.Category category)
        {
            // Use our custom parser to get top-level tags
            var sectionTags = ParseTagsRecursively(fxContent);
            
            // Process the active bank setting (SETUP tag)
            ProcessActiveBankSetting(patch, sectionTags, category);
            
            // Process top-level bank tags (A, B, C, D)
            ProcessBankTag(patch, sectionTags, "A", EffectSlot.BankType.A, banks, category);
            ProcessBankTag(patch, sectionTags, "B", EffectSlot.BankType.B, banks, category);
            ProcessBankTag(patch, sectionTags, "C", EffectSlot.BankType.C, banks, category);
            ProcessBankTag(patch, sectionTags, "D", EffectSlot.BankType.D, banks, category);
            
            // Process effect slots and their parameters
            foreach (var tagEntry in sectionTags)
            {
                string tagName = tagEntry.Key;
                string tagContent = tagEntry.Value;
                
                // Skip already processed tags
                if (tagName == "SETUP" || tagName == "A" || tagName == "B" || tagName == "C" || tagName == "D")
                    continue;
                
                if (IsSequenceTag(tagName))
                {
                    ProcessSequenceTag(banks, tagName, tagContent);
                }
                else if (IsSlotTag(tagName))
                {
                    ProcessSlotTag(banks, tagName, tagContent);
                }
                else if (IsEffectTag(tagName))
                {
                    ProcessEffectTag(banks, tagName, tagContent);
                }
                else
                {
                    // Unhandled tag format
                    Console.WriteLine($"Warning: Unhandled bank tag format: {tagName}");
                }
            }
        }
        
        // Process the active bank setting from the SETUP tag
        private void ProcessActiveBankSetting(MemoryPatch patch, Dictionary<string, string> sectionTags, EffectSlot.Category category)
        {
            if (sectionTags.ContainsKey("SETUP"))
            {
                var setupParams = ParseTagParameters(sectionTags["SETUP"]);
                if (setupParams.ContainsKey("A"))
                {
                    // BANK parameter value will be 0, 1, 2, or 3 corresponding to banks A, B, C, or D
                    int bankValue = setupParams["A"];
                    EffectSlot.BankType activeBank = GetBankTypeFromValue(bankValue);
                    
                    // Set the active bank based on category
                    if (category == EffectSlot.Category.InputFX)
                        patch.InputFX.ActiveBank = activeBank;
                    else
                        patch.TrackFX.ActiveBank = activeBank;
                }
            }
        }
        
        // Convert numeric bank value to BankType enum
        private EffectSlot.BankType GetBankTypeFromValue(int bankValue)
        {
            switch (bankValue)
            {
                case 0: return EffectSlot.BankType.A;
                case 1: return EffectSlot.BankType.B;
                case 2: return EffectSlot.BankType.C;
                case 3: return EffectSlot.BankType.D;
                default: return EffectSlot.BankType.A; // Default to A
            }
        }
        
        // Check if the tag is a sequence tag (format: AA_LPF_SEQ)
        private bool IsSequenceTag(string tagName)
        {
            return tagName.Contains("_SEQ");
        }
        
        // Check if the tag is a slot tag (format: AA, AB, AC, etc.)
        private bool IsSlotTag(string tagName)
        {
            return tagName.Length == 2 && char.IsLetter(tagName[0]) && char.IsLetter(tagName[1]);
        }
        
        // Check if the tag is an effect tag (format: AA_CHORUS, AA_REVERB, etc.)
        private bool IsEffectTag(string tagName)
        {
            return tagName.Length > 2 && tagName.Contains('_');
        }
          // Process a sequence tag (e.g., AA_LPF_SEQ)
        private void ProcessSequenceTag(Dictionary<string, EffectBank> banks, string tagName, string tagContent)
        {
            // Format is like AA_LPF_SEQ, where AA is the bank/slot and LPF is the effect type
            string[] parts = tagName.Split('_');
            if (parts.Length < 3) // Need at least 3 parts for AA_LPF_SEQ format
                return;
                
            string bankSlotPart = parts[0];
            string effectName = parts[1]; // Extract the effect name (e.g., LPF)
            
            if (bankSlotPart.Length == 2 && char.IsLetter(bankSlotPart[0]) && char.IsLetter(bankSlotPart[1]))
            {
                // Parse bank and slot
                char bankLetter = bankSlotPart[0];
                char slotLetter = bankSlotPart[1];
                
                var bankType = GetEffectBankFromSingleLetter(bankLetter.ToString());
                int slotIndex = slotLetter - 'A' + 1;
                
                // Get the effect type ID from the name
                int effectTypeId = GetEffectIdFromName(effectName);
                
                // Parse sequence parameters
                var seqParams = ParseTagParameters(tagContent);
                
                // Skip if invalid bank or slot
                if (effectTypeId < 0 || bankType == EffectSlot.BankType.None || !banks.ContainsKey(bankType.ToString()) || 
                    slotIndex < 1 || slotIndex > 4)
                    return;
                
                // Get the bank and slot
                var currentEffectBank = banks[bankType.ToString()];
                var currentEffectSlot = currentEffectBank.Slots[slotIndex];
                
                // Process sequence parameters for this specific effect
                ProcessSequenceParameters(currentEffectSlot, effectTypeId, effectName, seqParams);
            }
        }
          // Process sequence parameters for a specific effect
        private void ProcessSequenceParameters(EffectSlot slot, int effectTypeId, string effectName, Dictionary<string, int> seqParams)
        {
            // Get or create effect settings for this effect
            EffectSettings effectSettings;
            
            if (slot.AllEffectSettings.TryGetValue(effectTypeId, out var settings))
            {
                effectSettings = settings;
            }
            else
            {
                // Create new effect settings if it doesn't exist
                effectSettings = new EffectSettings
                {
                    EffectType = effectTypeId,
                    EffectName = effectName
                };
                
                // Add to the slot's all effect settings
                slot.AllEffectSettings[effectTypeId] = effectSettings;
            }
            
            // Map lettered parameters to named sequence parameters
            // A -> SEQ_SW (Sequence on/off)
            if (seqParams.ContainsKey("A"))
                effectSettings.Parameters["SEQ_SW"] = seqParams["A"];
            
            // B -> SEQ_SYNC (Sync with loop start)
            if (seqParams.ContainsKey("B"))
                effectSettings.Parameters["SEQ_SYNC"] = seqParams["B"];
            
            // C -> SEQ_RETRIG (Retrigger when effect is turned on)
            if (seqParams.ContainsKey("C"))
                effectSettings.Parameters["SEQ_RETRIG"] = seqParams["C"];
            
            // D -> SEQ_TARGET (Which parameter is targeted)
            if (seqParams.ContainsKey("D"))
                effectSettings.Parameters["SEQ_TARGET"] = seqParams["D"];
            
            // E -> SEQ_RATE (Cycle speed)
            if (seqParams.ContainsKey("E"))
                effectSettings.Parameters["SEQ_RATE"] = seqParams["E"];
            
            // F -> SEQ_MAX (Maximum steps in sequence)
            if (seqParams.ContainsKey("F"))
                effectSettings.Parameters["SEQ_MAX"] = seqParams["F"];
            
            // Step Values (VAL1-VAL16)
            // G through V -> SEQ_VAL1 through SEQ_VAL16
            char letter = 'G';
            for (int i = 1; i <= 16; i++)
            {
                string letterKey = letter.ToString();
                if (seqParams.ContainsKey(letterKey))
                    effectSettings.Parameters[$"SEQ_VAL{i}"] = seqParams[letterKey];
                
                letter++;
                if (letter > 'V') // Only process up to V (16 steps)
                    break;
            }
            
            // // If this is the currently active effect in the slot, also sync the parameters to the slot
            // if (slot.EffectType == effectTypeId)
            // {
            //     // Copy sequence parameters to the slot for backward compatibility
            //     // This can be removed in the future when all code is updated to read from effect settings
            //     foreach (var param in effectSettings.Parameters.Where(p => p.Key.StartsWith("SEQ_")))
            //     {
            //         slot.Parameters[param.Key] = param.Value;
            //     }
            // }
        }
        
        // Process a slot tag (e.g., AA, AB, AC, etc.)
        private void ProcessSlotTag(Dictionary<string, EffectBank> banks, string tagName, string tagContent)
        {
            // First letter represents the bank (A, B, C, D)
            char bankLetter = tagName[0];
            // Second letter represents the slot (A, B, C, D = 0, 1, 2, 3)
            char slotLetter = tagName[1];
            
            var bankType = GetEffectBankFromSingleLetter(bankLetter.ToString());
            int slotIndex = slotLetter - 'A' + 1;
            
            // Skip if invalid bank or slot
            if (bankType == EffectSlot.BankType.None || !banks.ContainsKey(bankType.ToString()) || 
                slotIndex < 1 || slotIndex > 4)
                return;
                
            var slotParams = ParseTagParameters(tagContent);
            
            if (slotParams.ContainsKey("C"))
            {
                var slotBank = banks[bankType.ToString()];
                var currentSlot = slotBank.Slots[slotIndex];
                
                // C parameter determines the active effect type for this slot
                int effectTypeValue = slotParams["C"];
                string effectName = GetEffectTypeFromId(effectTypeValue);
                
                // Create effect settings
                var effectSettings = new EffectSettings
                {
                    EffectType = effectTypeValue,
                    EffectName = effectName
                };
                
                // Add to AllEffectSettings and set as current effect
                currentSlot.AllEffectSettings[effectTypeValue] = effectSettings;
                currentSlot.SwitchToEffect(effectTypeValue, effectName);
                
                // Process other slot parameters
                ProcessCommonSlotParameters(currentSlot, slotParams, effectSettings);
            }
        }          // Process common slot parameters
        private void ProcessCommonSlotParameters(EffectSlot slot, Dictionary<string, int> parameters, EffectSettings effectSettings)
        {
            // A parameter is usually SW (on/off) state 
            if (parameters.ContainsKey("A"))
            {
                slot.Enabled = parameters["A"] == 1;
                // effectSettings.Parameters["A"] = parameters["A"];
                slot.Parameters["A"] = parameters["A"]; // Store in both places
            }
            
            // Process B, C, D parameters
            if (parameters.ContainsKey("B"))
            {
                // effectSettings.Parameters["B"] = parameters["B"];
                slot.Parameters["B"] = parameters["B"]; // Store in both places
            }
            
            if (parameters.ContainsKey("C"))
            {
                // effectSettings.Parameters["C"] = parameters["C"];
                // C is stored as slot.EffectType, but also keep in parameters for consistency
                slot.Parameters["C"] = parameters["C"]; 
            }
            
            if (parameters.ContainsKey("D"))
            {
                // effectSettings.Parameters["D"] = parameters["D"];
                slot.Parameters["D"] = parameters["D"]; // Store in both places
            }
        }
        
        // Process an effect tag (e.g., AA_CHORUS, AA_REVERB, etc.)
        private void ProcessEffectTag(Dictionary<string, EffectBank> banks, string tagName, string tagContent)
        {
            // Format is like AA_CHORUS or AA_SLOW_GEAR, where AA is the bank/slot and CHORUS or SLOW_GEAR is the effect type
            string[] parts = tagName.Split('_');
            if (parts.Length < 2)
                return;
                
            string bankSlotPart = parts[0];
            // Combine all parts after the first underscore to handle effect names with underscores like SLOW_GEAR
            string effectTypeName = string.Join("_", parts.Skip(1));

            if (bankSlotPart.Length == 2 && char.IsLetter(bankSlotPart[0]) && char.IsLetter(bankSlotPart[1]))
            {
                // Parse bank and slot
                char bankLetter = bankSlotPart[0];
                char slotLetter = bankSlotPart[1];

                var bankType = GetEffectBankFromSingleLetter(bankLetter.ToString());
                int slotIndex = slotLetter - 'A' + 1;

                // Get the effect type ID from the name
                int effectTypeId = GetEffectIdFromName(effectTypeName);

                // Skip if invalid
                if (effectTypeId < 0 || bankType == EffectSlot.BankType.None ||
                    !banks.ContainsKey(bankType.ToString()) || slotIndex < 1 || slotIndex > 4)
                    return;

                // Get the bank and slot
                var currentEffectBank = banks[bankType.ToString()];
                var currentEffectSlot = currentEffectBank.Slots[slotIndex];

                // Get or create effect settings
                var effectSettings = currentEffectSlot.AllEffectSettings.ContainsKey(effectTypeId)
                    ? currentEffectSlot.AllEffectSettings[effectTypeId]
                    : new EffectSettings
                    {
                        EffectType = effectTypeId,
                        EffectName = effectTypeName
                    };

                // Process effect parameters
                var effectParams = ParseTagParameters(tagContent);
                foreach (var param in effectParams)
                {
                    effectSettings.Parameters[param.Key] = param.Value;
                }

                // Store or update the effect settings
                currentEffectSlot.AllEffectSettings[effectTypeId] = effectSettings;

                // Slot parameters are not meant for effect parameters.                  
                // // If this is the active effect for this slot, update the parameters
                // if (currentEffectSlot.EffectType == effectTypeId)
                // {
                //     // Sync parameters to the slot
                //     foreach (var param in effectSettings.Parameters)
                //     {
                //         currentEffectSlot.Parameters[param.Key] = param.Value;
                //     }
                // }
            }
        }
        
        // Helper method to process a bank-level tag (A, B, C, D)
        private void ProcessBankTag(MemoryPatch patch, Dictionary<string, string> sectionTags, string bankTag, 
                                    EffectSlot.BankType bankType, Dictionary<string, EffectBank> banks, 
                                    EffectSlot.Category category)
        {
            if (!sectionTags.ContainsKey(bankTag))
                return;
                
            var bankParams = ParseTagParameters(sectionTags[bankTag]);
            
            // For each bank, the parameters A, B, C represent:
            // A: SW (on/off) - Bank enable state
            
            if (bankParams.ContainsKey("A"))
            {
                // Set bank enable state
                if (banks.ContainsKey(bankType.ToString()))
                {
                    banks[bankType.ToString()].Enabled = bankParams["A"] == 1;
                }
            }
            
            if (bankParams.ContainsKey("B"))
            {
                // Additional bank parameter B
                if (banks.ContainsKey(bankType.ToString()))
                {
                    banks[bankType.ToString()].Parameters["B"] = bankParams["B"];
                }
            }
            
            if (bankParams.ContainsKey("C"))
            {
                // Additional bank parameter C
                if (banks.ContainsKey(bankType.ToString()))
                {
                    banks[bankType.ToString()].Parameters["C"] = bankParams["C"];
                }
            }
        }
        
        // Helper methods
        private EffectSlot.BankType GetEffectBankFromSingleLetter(string letter)
        {
            switch (letter)
            {
                case "A": return EffectSlot.BankType.A;
                case "B": return EffectSlot.BankType.B;
                case "C": return EffectSlot.BankType.C;
                case "D": return EffectSlot.BankType.D;
                default: return EffectSlot.BankType.None;
            }
        }
          private string GetEffectTypeFromId(int effectTypeId)
        {
            // Use the centralized mapping class
            return EffectMappings.GetEffectNameFromId(effectTypeId);
        }        private int GetEffectIdFromName(string effectName)
        {
            // Use the centralized mapping class
            return EffectMappings.GetEffectIdFromName(effectName);
        }
        
        private string GetControlSourceName(int sourceId)
        {
            // Map source ID to source name
            // Example implementation:
            switch(sourceId)
            {
                case 0: return "CTL1";
                case 1: return "CTL2";
                case 2: return "CTL3";
                case 3: return "CTL4";
                case 4: return "EXP1";
                case 5: return "EXP2";
                // Add more mappings as needed
                default: return $"SOURCE_{sourceId}";
            }
        }
        
        private string GetControlTargetName(int targetId)
        {
            // Map target ID to target name
            // Example implementation:
            switch(targetId)
            {
                case 0: return "TRACK1_LEVEL";
                case 1: return "TRACK2_LEVEL";
                case 2: return "TRACK3_LEVEL";
                case 3: return "TRACK4_LEVEL";
                case 4: return "TRACK5_LEVEL";
                case 5: return "TRACK6_LEVEL";
                case 10: return "RHYTHM_LEVEL";
                case 20: return "MASTER_LEVEL";
                // Add more mappings as needed
                default: return $"TARGET_{targetId}";
            }
        }
        
        private void ProcessMasterGroup(MemoryPatch patch, Dictionary<string, int> parameters)
        {
            // Process each parameter from the MASTER section using letter tags
            // A -> Loop Position
            // B -> Loop Length
            // C -> Mode Flag
            // D -> Mode Value
            
            if (parameters.ContainsKey("A"))
            {
                patch.Master.LoopPosition = parameters["A"];
            }
            
            if (parameters.ContainsKey("B"))
            {
                patch.Master.LoopLength = parameters["B"];
            }
            
            if (parameters.ContainsKey("C"))
            {
                patch.Master.ModeFlag = parameters["C"];
            }
            
            if (parameters.ContainsKey("D"))
            {
                patch.Master.ModeValue = parameters["D"];
            }
        }
    }
}
