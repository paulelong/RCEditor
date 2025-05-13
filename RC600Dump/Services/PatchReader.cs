using System.Text;
using System.Text.RegularExpressions;
using RCEditor.Models;

namespace RC600Dump.Services
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
            Match memMatch = Regex.Match(content, @"<mem id=""[^""]*"">(.*?)</mem>", RegexOptions.Singleline);
            if (memMatch.Success)
            {
                string memContent = memMatch.Groups[1].Value;
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
            
            // Handle other name-related parameters
            foreach (var param in parameters)
            {
                if (param.Key == "NAME")
                {
                    // This would require getting the text value, not just an int
                    // This is a placeholder - the actual implementation would extract the text
                    patch.Name = "Memory Patch";
                }
                // Process other name-related parameters if needed
            }
        }
        
        private void ProcessPlayGroup(MemoryPatch patch, Dictionary<string, int> parameters)
        {
            // Process each parameter from the PLAY section
            foreach (var param in parameters)
            {
                switch (param.Key)
                {
                    case "S.TRK_CHANGE":
                        patch.Play.SingleTrackChange = (SingleTrackChangeEnum)param.Value;
                        break;
                    case "CURRENT_TRACK":
                        // Handle current track if needed
                        break;
                    case "FADE_TIME_IN":
                        patch.Play.FadeTimeIn = param.Value;
                        break;
                    case "FADE_TIME_OUT":
                        patch.Play.FadeTimeOut = param.Value;
                        break;
                    case "ALL_START_TRK":
                        // Set all start tracks based on the bit mask
                        for (int i = 0; i < 6; i++)
                        {
                            patch.Play.AllStartTracks[i] = ((param.Value >> i) & 1) == 1;
                        }
                        break;
                    case "ALL_STOP_TRK":
                        // Set all stop tracks based on the bit mask
                        for (int i = 0; i < 6; i++)
                        {
                            patch.Play.AllStopTracks[i] = ((param.Value >> i) & 1) == 1;
                        }
                        break;
                    case "LOOP_LENGTH":
                        patch.Play.LoopLength = param.Value;
                        break;
                    case "SPEED_CHANGE":
                        patch.Play.SpeedChange = (SpeedChangeEnum)param.Value;
                        break;
                    case "SYNC_ADJUST":
                        patch.Play.SyncAdjust = (SyncAdjustEnum)param.Value;
                        break;
                    // Add more parameters as needed
                }
            }
        }
        
        private void ProcessRecGroup(MemoryPatch patch, Dictionary<string, int> parameters)
        {
            // Process each parameter from the REC section
            foreach (var param in parameters)
            {
                switch (param.Key)
                {
                    case "REC_ACTION":
                        patch.Rec.RecAction = (RecActionEnum)param.Value;
                        break;
                    case "QUANTIZE":
                        patch.Rec.QuantizeEnabled = param.Value == 1;
                        break;
                    case "AUTO_REC_SW":
                        patch.Rec.AutoRecEnabled = param.Value == 1;
                        break;
                    case "AUTO_REC_SENS":
                        patch.Rec.AutoRecSensitivity = param.Value;
                        break;
                    case "BOUNCE_SW":
                        patch.Rec.BounceEnabled = param.Value == 1;
                        break;
                    case "BOUNCE_TRACK":
                        patch.Rec.BounceTrack = param.Value;
                        break;
                    // Add more parameters as needed
                }
            }
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
            
            // Process each parameter for this track
            foreach (var param in parameters)
            {
                switch (param.Key)
                {
                    case "REVERSE":
                        track.Reverse = param.Value == 1;
                        break;
                    case "1SHOT":
                        track.OneShot = param.Value == 1;
                        break;
                    case "PAN":
                        track.Pan = param.Value - 50; // Convert from 0-100 to -50 to +50
                        break;
                    case "PLAY_LEVEL":
                        track.Level = param.Value;
                        break;
                    case "START_MODE":
                        track.StartMode = (StartModeEnum)param.Value;
                        break;
                    case "STOP_MODE":
                        track.StopMode = (StopModeEnum)param.Value;
                        break;
                    case "DUB_MODE":
                        track.OverdubMode = (OverdubModeEnum)param.Value;
                        break;
                    case "FX":
                        track.FXEnabled = param.Value == 1;
                        break;
                    case "PLAY_MODE":
                        // Handle play mode if needed
                        break;
                    case "MEASURE":
                        track.MeasureCount = param.Value;
                        break;
                    case "LOOP_SYNC_SW":
                        track.LoopSyncSw = param.Value == 1;
                        break;
                    case "LOOP_SYNC_MODE":
                        track.LoopSyncMode = (LoopSyncModeEnum)param.Value;
                        break;
                    case "TEMPO_SYNC_SW":
                        track.TempoSyncSw = param.Value == 1;
                        break;
                    case "TEMPO_SYNC_MODE":
                        track.TempoSyncMode = (TempoSyncModeEnum)param.Value;
                        break;
                    case "TEMPO_SYNC_SPEED":
                        track.TempoSyncSpeed = (TempoSyncSpeedEnum)param.Value;
                        break;
                    case "BOUNCE_IN":
                        track.BounceIn = param.Value == 1;
                        break;
                    // Input routing parameters (MIC, INST, RHYTHM)
                    case "INPUT_MIC":
                        if (param.Value == 1)
                            track.InputRouting.MicIn = InputRouteEnum.Input1;
                        break;
                    case "INPUT_INST":
                        if (param.Value == 1)
                            track.InputRouting.Inst1 = InputRouteEnum.Input2;
                        break;
                    case "INPUT_RHYTHM":
                        if (param.Value == 1)
                            track.InputRouting.Inst2 = InputRouteEnum.Input3;
                        break;
                    // Add any other track parameters as needed
                }
            }
        }
        
        private void ProcessRhythmGroup(MemoryPatch patch, Dictionary<string, int> parameters)
        {
            // Process each parameter from the RHYTHM section
            foreach (var param in parameters)
            {
                switch (param.Key)
                {
                    case "GENRE":
                        patch.Rhythm.Genre = param.Value.ToString();
                        break;
                    case "PATTERN":
                        patch.Rhythm.Pattern = param.Value.ToString();
                        break;
                    case "VARIATION":
                        patch.Rhythm.Variation = (char)('A' + param.Value);
                        break;
                    case "KIT":
                        patch.Rhythm.Kit = param.Value.ToString();
                        break;
                    case "BEAT":
                        patch.Rhythm.Beat = param.Value.ToString();
                        break;
                    case "START_TRIG":
                        patch.Rhythm.StartMode = (RhythmStartTrigEnum)param.Value;
                        break;
                    case "STOP_TRIG":
                        patch.Rhythm.StopMode = (RhythmStopTrigEnum)param.Value;
                        break;
                    case "INTRO_REC":
                        patch.Rhythm.IntroOnRec = param.Value == 1;
                        break;
                    case "INTRO_PLAY":
                        patch.Rhythm.IntroOnPlay = param.Value == 1;
                        break;
                    case "ENDING":
                        patch.Rhythm.Ending = param.Value == 1;
                        break;
                    case "FILL":
                        patch.Rhythm.FillIn = param.Value == 1;
                        break;
                    case "VAR.CHANGE":
                        patch.Rhythm.VariationChangeTiming = (RhythmVariationChangeEnum)param.Value;
                        break;
                    case "LEVEL":
                        // Handle level if needed
                        break;
                    // Add any other rhythm parameters as needed
                }
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
            // Use our new custom parser to get top-level tags
            var sectionTags = ParseTagsRecursively(fxContent);
            
            // First look for the SETUP tag which contains the active bank setting
            if (sectionTags.ContainsKey("SETUP"))
            {
                var setupParams = ParseTagParameters(sectionTags["SETUP"]);
                if (setupParams.ContainsKey("A"))
                {
                    // BANK parameter value will be 0, 1, 2, or 3 corresponding to banks A, B, C, or D
                    int bankValue = setupParams["A"];
                    EffectSlot.BankType activeBank = EffectSlot.BankType.A; // Default
                    
                    switch (bankValue)
                    {
                        case 0:
                            activeBank = EffectSlot.BankType.A;
                            break;
                        case 1:
                            activeBank = EffectSlot.BankType.B;
                            break;
                        case 2:
                            activeBank = EffectSlot.BankType.C;
                            break;
                        case 3:
                            activeBank = EffectSlot.BankType.D;
                            break;
                    }
                    
                    // Set the active bank based on category
                    if (category == EffectSlot.Category.InputFX)
                        patch.InputFX.ActiveBank = activeBank;
                    else
                        patch.TrackFX.ActiveBank = activeBank;
                }
            }
            
            foreach (var tagEntry in sectionTags)
            {
                string bankTag = tagEntry.Key;
                string effectContent = tagEntry.Value;
                
                // Skip the SETUP tag as we already processed it
                if (bankTag == "SETUP")
                    continue;
                
                // Handle the bank/slot structure
                EffectSlot.BankType bankType = EffectSlot.BankType.None;
                int slotIndex = 1;
                
                // First check for the simple A, B, C, D bank tags
                if (bankTag.Length == 1 && char.IsLetter(bankTag[0]))
                {
                    // Single letter tags like A, B, C, D represent bank settings
                    bankType = GetEffectBankFromSingleLetter(bankTag);
                    
                    // Process bank-level settings
                    if (bankType != EffectSlot.BankType.None && banks.ContainsKey(bankType.ToString()))
                    {
                        var bankParams = ParseTagParameters(effectContent);
                        var currentEffectBank = banks[bankType.ToString()];
                        
                        // Process the bank-level parameters
                        foreach (var slot in currentEffectBank.Slots.Values)
                        {
                            // Apply bank-level settings to all slots in this bank
                            // In RC-600 format, parameter A=SW, B=MODE, C=TARGET
                            if (bankParams.ContainsKey("A"))
                            {
                                // Bank-level enable/disable (A=SW)
                                bool bankEnabled = bankParams["A"] == 1;
                                // Only set if the slot doesn't have a specific setting
                                if (!slot.Parameters.ContainsKey("BankEnabled"))
                                    slot.Parameters["BankEnabled"] = bankEnabled ? 1 : 0;
                            }
                            
                            if (bankParams.ContainsKey("B"))
                            {
                                // Bank operation mode (B=MODE)
                                int bankMode = bankParams["B"];
                                if (!slot.Parameters.ContainsKey("BankMode"))
                                    slot.Parameters["BankMode"] = bankMode;
                            }
                            
                            if (bankParams.ContainsKey("C"))
                            {
                                // FX target tracks/inputs (C=TARGET)
                                string fxTarget = GetFXTargetFromId(bankParams["C"]);
                                if (string.IsNullOrEmpty(slot.Target))
                                    slot.Target = fxTarget;
                            }
                        }
                    }
                    
                    // Continue to next tag since we're done with the bank settings
                    continue;
                }
                // Then check for two-letter bank/slot combinations like AA, AB, AC, etc.
                else if (bankTag.Length == 2 && char.IsLetter(bankTag[0]) && char.IsLetter(bankTag[1]))
                {
                    // First letter represents the bank (A, B, C, D)
                    char bankLetter = bankTag[0];
                    // Second letter represents the slot (A, B, C, D = 0, 1, 2, 3)
                    char slotLetter = bankTag[1];
                    
                    bankType = GetEffectBankFromSingleLetter(bankLetter.ToString());
                    slotIndex = slotLetter - 'A' + 1;
                    
                    // Process slot-level settings from double-letter tags
                    if (bankType != EffectSlot.BankType.None && banks.ContainsKey(bankType.ToString()))
                    {
                        var slotParams = ParseTagParameters(effectContent);
                        
                        if (slotParams.ContainsKey("C") && slotIndex >= 1 && slotIndex <= 4)
                        {
                            var slotBank = banks[bankType.ToString()];
                            var currentSlot = slotBank.Slots[slotIndex];
                            
                            // C parameter determines the active effect type for this slot
                            int effectTypeValue = slotParams["C"];
                            string effectName = GetEffectTypeFromId(effectTypeValue);
                            
                            // Store the effect settings first
                            var effectSettings = new EffectSettings
                            {
                                EffectType = effectTypeValue,
                                EffectName = effectName
                            };
                            
                            // Add to AllEffectSettings and set as current effect
                            currentSlot.AllEffectSettings[effectTypeValue] = effectSettings;
                            currentSlot.SwitchToEffect(effectTypeValue, effectName);
                            
                            // Parse other slot parameters if present
                            if (slotParams.ContainsKey("A"))
                            {
                                // Usually SW (on/off) state 
                                currentSlot.Enabled = slotParams["A"] == 1;
                            }
                            
                            if (slotParams.ContainsKey("B"))
                            {
                                // Often mode or other setting
                                currentSlot.Parameters["SlotMode"] = slotParams["B"];
                                effectSettings.Parameters["SlotMode"] = slotParams["B"];
                            }
                            
                            // Continue processing this slot and save values
                            continue;
                        }
                    }
                }
                // Then check for specific effect type tags like AA_CHORUS, AA_REVERB, etc.
                else if (bankTag.Length > 2 && bankTag.Contains('_'))
                {
                    // Format is like AA_CHORUS, where AA is the bank/slot and CHORUS is the effect type
                    string[] parts = bankTag.Split('_');
                    if (parts.Length != 2)
                        continue;
                        
                    string bankSlotPart = parts[0];
                    string effectTypeName = parts[1];
                    
                    if (bankSlotPart.Length == 2 && char.IsLetter(bankSlotPart[0]) && char.IsLetter(bankSlotPart[1]))
                    {
                        // First letter represents the bank (A, B, C, D)
                        char bankLetter = bankSlotPart[0];
                        // Second letter represents the slot (A, B, C, D = 0, 1, 2, 3)
                        char slotLetter = bankSlotPart[1];
                        
                        bankType = GetEffectBankFromSingleLetter(bankLetter.ToString());
                        slotIndex = slotLetter - 'A' + 1;
                        
                        // Get the effect type ID from the name
                        int effectTypeId = GetEffectIdFromName(effectTypeName);
                        
                        // Skip if we couldn't determine the effect type
                        if (effectTypeId < 0)
                            continue;
                        
                        // Process the effect-specific parameters
                        var effectParamTags = ParseTagParameters(effectContent);
                        
                        // Skip if we can't determine the bank or if it's not in a valid range
                        if (bankType == EffectSlot.BankType.None || !banks.ContainsKey(bankType.ToString()) || 
                            slotIndex < 1 || slotIndex > 4)
                            continue;
                        
                        // Get the bank and slot
                        var currentEffectBank = banks[bankType.ToString()];
                        var currentEffectSlot = currentEffectBank.Slots[slotIndex];
                        
                        // Check if we already have settings for this effect type
                        var effectSettings = currentEffectSlot.AllEffectSettings.ContainsKey(effectTypeId) 
                            ? currentEffectSlot.AllEffectSettings[effectTypeId] 
                            : new EffectSettings { 
                                EffectType = effectTypeId, 
                                EffectName = effectTypeName 
                            };
                        
                        // Save the effect parameters
                        foreach (var param in effectParamTags)
                        {
                            effectSettings.Parameters[param.Key] = param.Value;
                        }
                        
                        // Store or update the effect settings
                        currentEffectSlot.AllEffectSettings[effectTypeId] = effectSettings;
                        
                        // If this is the active effect for this slot, update the parameters
                        if (currentEffectSlot.EffectType == effectTypeId)
                        {
                            // Sync parameters to the slot
                            foreach (var param in effectSettings.Parameters)
                            {
                                currentEffectSlot.Parameters[param.Key] = param.Value;
                            }
                        }
                        
                        continue;
                    }
                }
                else
                {
                    // Handle old-style bank names (FX1, BANK1, etc.)
                    // print warning that tag wasn't handled
                    Console.WriteLine($"Warning: Unhandled bank tag format: {bankTag}");

                    continue;
                }
                
                // Skip if we can't determine the bank or if it's not a valid effect tag
                if (bankType == EffectSlot.BankType.None || slotIndex < 0 || slotIndex >= 4)
                    continue;
                
                string bankKey = bankType.ToString();
                
                // Make sure the bank exists in the container
                if (!banks.ContainsKey(bankKey))
                    continue;
                
                // Get the effect bank
                var effectBank = banks[bankKey];
                
                // Make sure the slot index is valid
                if (slotIndex >= effectBank.Slots.Count)
                    continue;
                
                // Get the effect slot
                var effectSlot = effectBank.Slots[slotIndex];
                
                // Set category
                effectSlot.SlotCategory = category;
                effectSlot.Bank = bankType;
                
                // Parse parameters for this effect using our recursive parser
                var paramTags = ParseTagParameters(effectContent);
                
                // Process effect parameters
                int currentEffectType = effectSlot.EffectType;
                
                foreach (var param in paramTags)
                {
                    switch (param.Key)
                    {
                        case "SW":
                            effectSlot.Enabled = param.Value == 1;
                            break;
                        case "TYPE":
                            int newEffectType = param.Value;
                            string newEffectName = GetEffectTypeFromId(newEffectType);
                            
                            // Check if we're changing effect types
                            if (currentEffectType != newEffectType)
                            {
                                // Make sure we have settings for this effect type
                                if (!effectSlot.AllEffectSettings.ContainsKey(newEffectType))
                                {
                                    effectSlot.AllEffectSettings[newEffectType] = new EffectSettings
                                    {
                                        EffectType = newEffectType,
                                        EffectName = newEffectName,
                                        Parameters = new Dictionary<string, int>()
                                    };
                                }
                                
                                // Switch to the new effect type
                                effectSlot.SwitchToEffect(newEffectType, newEffectName);
                            }
                            break;
                        case "SW_MODE":
                            effectSlot.SwitchMode = (SwitchModeEnum)param.Value;
                            break;
                        case "TARGET":
                            effectSlot.Target = GetFXTargetFromId(param.Value);
                            break;
                        default:
                            // Add to parameters dictionary as integer
                            effectSlot.Parameters[param.Key] = param.Value;
                            
                            // Also store in the current effect's settings
                            if (effectSlot.AllEffectSettings.ContainsKey(effectSlot.EffectType))
                            {
                                effectSlot.AllEffectSettings[effectSlot.EffectType].Parameters[param.Key] = param.Value;
                            }
                            break;
                    }
                }
            }
        }
        
        // Helper method to convert effect name to ID
        private int GetEffectIdFromName(string effectName)
        {
            // Convert effect name to uppercase and normalize spaces
            string normalized = effectName.ToUpper().Trim().Replace(" ", "");
            
            // Match against known effect names
            switch (normalized)
            {
                case "LPF": return 0;
                case "BPF": return 1;
                case "HPF": return 2;
                case "PHASER": return 3;
                case "FLANGER": return 4;
                case "SYNTH": return 5;
                case "LOFI": case "LO-FI": return 6;
                case "RADIO": return 7;
                case "RINGMOD": case "RING_MOD": case "RING-MOD": return 8;
                case "G2B": case "GUITARTOBASS": return 9;
                case "SUSTAINER": return 10;
                case "AUTORIFF": case "AUTO_RIFF": case "AUTO-RIFF": return 11;
                case "SLOWGEAR": case "SLOW_GEAR": case "SLOW-GEAR": return 12;
                case "TRANSPOSE": return 13;
                case "PITCHBEND": case "PITCH_BEND": case "PITCH-BEND": return 14;
                case "ROBOT": return 15;
                case "ELECTRIC": return 16;
                case "HRMMANUAL": case "HRM_MANUAL": case "HRM-MANUAL": return 17;
                case "HRMAUTO": case "HRM_AUTO": case "HRM-AUTO": return 18;
                case "VOCODER": return 19;
                case "OSCVOC": case "OSC_VOC": case "OSC-VOC": return 20;
                case "OSCBOT": case "OSC_BOT": case "OSC-BOT": return 21;
                case "PREAMP": case "PRE_AMP": case "PRE-AMP": return 22;
                case "DIST": case "DISTORTION": return 23;
                case "DYNAMICS": return 24;
                case "EQ": return 25;
                case "ISOLATOR": return 26;
                case "OCTAVE": return 27;
                case "AUTOPAN": case "AUTO_PAN": case "AUTO-PAN": return 28;
                case "MANUALPAN": case "MANUAL_PAN": case "MANUAL-PAN": return 29;
                case "STEREOENHANCE": case "STEREO_ENHANCE": case "STEREO-ENHANCE": return 30;
                case "TREMOLO": return 31;
                case "VIBRATO": return 32;
                case "PATTERNSLICER": case "PATTERN_SLICER": case "PATTERN-SLICER": return 33;
                case "STEPSLICER": case "STEP_SLICER": case "STEP-SLICER": return 34;
                case "DELAY": return 35;
                case "PANNINGDELAY": case "PANNING_DELAY": case "PANNING-DELAY": return 36;
                case "REVERSEDELAY": case "REVERSE_DELAY": case "REVERSE-DELAY": return 37;
                case "MODDELAY": case "MOD_DELAY": case "MOD-DELAY": return 38;
                case "TAPEECHO1": case "TAPE_ECHO1": case "TAPE-ECHO1": return 39;
                case "TAPEECHO2": case "TAPE_ECHO2": case "TAPE-ECHO2": return 40;
                case "GRANULARDELAY": case "GRANULAR_DELAY": case "GRANULAR-DELAY": return 41;
                case "WARP": return 42;
                case "TWIST": return 43;
                case "ROLL1": case "ROLL_1": case "ROLL-1": return 44;
                case "ROLL2": case "ROLL_2": case "ROLL-2": return 45;
                case "FREEZE": return 46;
                case "CHORUS": return 47;
                case "REVERB": return 48;
                case "GATEREVERB": case "GATE_REVERB": case "GATE-REVERB": return 49;
                case "REVERSEREVERB": case "REVERSE_REVERB": case "REVERSE-REVERB": return 50;
                case "BEATSCATTER": case "BEAT_SCATTER": case "BEAT-SCATTER": return 51;
                case "BEATREPEAT": case "BEAT_REPEAT": case "BEAT-REPEAT": return 52;
                case "BEATSHIFT": case "BEAT_SHIFT": case "BEAT-SHIFT": return 53;
                case "VINYLFLICK": case "VINYL_FLICK": case "VINYL-FLICK": return 54;
                default: return -1; // Effect not recognized
            }
        }
        
        private string GetFXTargetFromId(int targetId)
        {
            // Map target values to meaningful descriptions based on the RC-600 documentation
            switch (targetId)
            {
                case 0:
                    return "ALL";  // All tracks/inputs
                case 1:
                    return "TRACK 1";
                case 2:
                    return "TRACK 2";
                case 3:
                    return "TRACK 3";
                case 4:
                    return "TRACK 4";
                case 5:
                    return "TRACK 5";
                case 6:
                    return "TRACK 6";
                case 7:
                    return "INPUT MIC";
                case 8:
                    return "INPUT INST";
                case 9:
                    return "INPUT RHYTHM";
                case 10:
                    return "CURRENT TRACK";  // Currently selected track
                default:
                    return $"UNKNOWN TARGET ({targetId})";
            }
        }

        // Helper methods for effect bank handling
        private EffectSlot.BankType GetEffectBankFromSingleLetter(string bankLetter)
        {
            switch (bankLetter.ToUpper())
            {
                case "A": return EffectSlot.BankType.A;
                case "B": return EffectSlot.BankType.B;
                case "C": return EffectSlot.BankType.C;
                case "D": return EffectSlot.BankType.D;
                default: return EffectSlot.BankType.None;
            }
        }

        // Helper method to get effect type name from ID
        private string GetEffectTypeFromId(int effectTypeId)
        {
            switch (effectTypeId)
            {
                case 0: return "LPF";
                case 1: return "BPF";
                case 2: return "HPF";
                case 3: return "PHASER";
                case 4: return "FLANGER";
                case 5: return "SYNTH";
                case 6: return "LO-FI";
                case 7: return "RADIO";
                case 8: return "RING MOD";
                case 9: return "GUITAR TO BASS";
                case 10: return "SUSTAINER";
                case 11: return "AUTO RIFF";
                case 12: return "SLOW GEAR";
                case 13: return "TRANSPOSE";
                case 14: return "PITCH BEND";
                case 15: return "ROBOT";
                case 16: return "ELECTRIC";
                case 17: return "HRM MANUAL";
                case 18: return "HRM AUTO";
                case 19: return "VOCODER";
                case 20: return "OSC VOC";
                case 21: return "OSC BOT";
                case 22: return "PRE AMP";
                case 23: return "DISTORTION";
                case 24: return "DYNAMICS";
                case 25: return "EQ";
                case 26: return "ISOLATOR";
                case 27: return "OCTAVE";
                case 28: return "AUTO PAN";
                case 29: return "MANUAL PAN";
                case 30: return "STEREO ENHANCE";
                case 31: return "TREMOLO";
                case 32: return "VIBRATO";
                case 33: return "PATTERN SLICER";
                case 34: return "STEP SLICER";
                case 35: return "DELAY";
                case 36: return "PANNING DELAY";
                case 37: return "REVERSE DELAY";
                case 38: return "MOD DELAY";
                case 39: return "TAPE ECHO 1";
                case 40: return "TAPE ECHO 2";
                case 41: return "GRANULAR DELAY";
                case 42: return "WARP";
                case 43: return "TWIST";
                case 44: return "ROLL 1";
                case 45: return "ROLL 2";
                case 46: return "FREEZE";
                case 47: return "CHORUS";
                case 48: return "REVERB";
                case 49: return "GATE REVERB";
                case 50: return "REVERSE REVERB";
                case 51: return "BEAT SCATTER";
                case 52: return "BEAT REPEAT";
                case 53: return "BEAT SHIFT";
                case 54: return "VINYL FLICK";
                default: return $"UNKNOWN EFFECT ({effectTypeId})";
            }
        }

        // Helper methods for control assignments
        private string GetControlSourceName(int sourceId)
        {
            switch (sourceId)
            {
                case 0: return "CTL 1";
                case 1: return "CTL 2";
                case 2: return "CTL 3";
                case 3: return "CTL 4";
                case 4: return "CTL 5";
                case 5: return "CTL 6";
                case 6: return "EXP";
                case 7: return "MIDI CC";
                default: return $"UNKNOWN SOURCE ({sourceId})";
            }
        }

        private string GetControlTargetName(int targetId)
        {
            // This is a simplified version - real implementation would have all the possible targets
            switch (targetId)
            {
                case 0: return "TRACK LEVEL";
                case 1: return "TRACK PAN";
                case 2: return "FX PARAMETER";
                case 3: return "RHYTHM LEVEL";
                case 4: return "RHYTHM VARIATION";
                case 5: return "LOOP LENGTH";
                case 6: return "TEMPO";
                default: return $"UNKNOWN TARGET ({targetId})";
            }
        }
    }
}