using System.Text;
using System.Text.RegularExpressions;
using RCEditor.Models;

namespace RC600Dump.Services
{
    public class PatchReader
    {
        private static readonly Regex GroupTagRegex = new Regex(@"<([A-Z0-9]+)>(.*?)</\1>", RegexOptions.Compiled | RegexOptions.Singleline);
        private static readonly Regex ParameterTagRegex = new Regex(@"<([A-Z#0-9]+)>([^<]*)</\1>", RegexOptions.Compiled);

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
            // Parse effects from the ifxContent using the same tag-based approach
            foreach (Match groupMatch in GroupTagRegex.Matches(ifxContent))
            {
                string bankTag = groupMatch.Groups[1].Value;
                string effectContent = groupMatch.Groups[2].Value;
                
                // Determine which bank this is for
                EffectSlot.BankType bankType = GetEffectBankFromName(bankTag);
                string bankKey = bankType.ToString();
                
                // Skip if we can't determine the bank
                if (bankType == EffectSlot.BankType.None)
                    continue;
                
                // Get the effect slot from the appropriate bank
                var effectBank = patch.InputFX.Banks[bankKey];
                var effectSlot = effectBank.Slots[1]; // Assume first slot for now
                
                // Set category to InputFX
                effectSlot.SlotCategory = EffectSlot.Category.InputFX;
                effectSlot.Bank = bankType;
                
                // Parse parameters for this effect
                var parameters = new Dictionary<string, int>();
                foreach (Match paramMatch in ParameterTagRegex.Matches(effectContent))
                {
                    string tag = paramMatch.Groups[1].Value;
                    string value = paramMatch.Groups[2].Value;
                    
                    // Parse integer value
                    if (int.TryParse(value, out int intValue))
                    {
                        parameters[tag] = intValue;
                    }
                }
                
                // Process effect parameters
                foreach (var param in parameters)
                {
                    switch (param.Key)
                    {
                        case "SW":
                            effectSlot.Enabled = param.Value == 1;
                            break;
                        case "TYPE":
                            effectSlot.EffectType = param.Value;
                            effectSlot.EffectName = GetEffectTypeFromId(param.Value);
                            break;
                        default:
                            // Add to parameters dictionary, converting to double
                            effectSlot.Parameters[param.Key] = param.Value;
                            break;
                    }
                }
            }
        }
        
        private void ProcessTrackFXSection(MemoryPatch patch, string tfxContent)
        {
            // Parse effects from the tfxContent using the same tag-based approach
            foreach (Match groupMatch in GroupTagRegex.Matches(tfxContent))
            {
                string bankTag = groupMatch.Groups[1].Value;
                string effectContent = groupMatch.Groups[2].Value;
                
                // Handle the complex RC600 tag structure (AA, AB, etc.)
                EffectSlot.BankType bankType = EffectSlot.BankType.None;
                int slotIndex = 0;
                
                // First check for the simple A, B, C, D bank tags
                if (bankTag.Length == 1 && char.IsLetter(bankTag[0]))
                {
                    // Single letter tags like A, B, C, D represent bank settings
                    bankType = GetEffectBankFromSingleLetter(bankTag);
                }
                // Then check for two-letter bank/slot combinations like AA, AB, AC, etc.
                else if (bankTag.Length == 2 && char.IsLetter(bankTag[0]) && char.IsLetter(bankTag[1]))
                {
                    // First letter represents the bank (A, B, C, D)
                    char bankLetter = bankTag[0];
                    // Second letter represents the slot (A, B, C, D = 0, 1, 2, 3)
                    char slotLetter = bankTag[1];
                    
                    bankType = GetEffectBankFromSingleLetter(bankLetter.ToString());
                    slotIndex = slotLetter - 'A';
                }
                // Then check for specific effect type tags like AA_CHORUS, AA_REVERB, etc.
                else if (bankTag.Length > 2 && bankTag.Contains('_'))
                {
                    // Format is like AA_CHORUS, where AA is the bank/slot and CHORUS is the effect type
                    string bankSlotPart = bankTag.Split('_')[0];
                    
                    if (bankSlotPart.Length == 2 && char.IsLetter(bankSlotPart[0]) && char.IsLetter(bankSlotPart[1]))
                    {
                        // First letter represents the bank (A, B, C, D)
                        char bankLetter = bankSlotPart[0];
                        // Second letter represents the slot (A, B, C, D = 0, 1, 2, 3)
                        char slotLetter = bankSlotPart[1];
                        
                        bankType = GetEffectBankFromSingleLetter(bankLetter.ToString());
                        slotIndex = slotLetter - 'A';
                        
                        // The rest of the tag (after the underscore) is the effect type name
                        string effectTypeName = bankTag.Substring(bankTag.IndexOf('_') + 1);
                    }
                }
                
                // Skip if we can't determine the bank or if it's not a valid effect tag
                if (bankType == EffectSlot.BankType.None || slotIndex < 0 || slotIndex >= 4)
                    continue;
                
                string bankKey = bankType.ToString();
                
                // Make sure the bank exists in the TrackFX container
                if (!patch.TrackFX.Banks.ContainsKey(bankKey))
                    continue;
                
                // Get the effect bank
                var effectBank = patch.TrackFX.Banks[bankKey];
                
                // Make sure the slot index is valid
                if (slotIndex >= effectBank.Slots.Count)
                    continue;
                
                // Get the effect slot
                var effectSlot = effectBank.Slots[slotIndex];
                
                // Set category to TrackFX
                effectSlot.SlotCategory = EffectSlot.Category.TrackFX;
                effectSlot.Bank = bankType;
                
                // Parse parameters for this effect
                var parameters = new Dictionary<string, int>();
                foreach (Match paramMatch in ParameterTagRegex.Matches(effectContent))
                {
                    string tag = paramMatch.Groups[1].Value;
                    string value = paramMatch.Groups[2].Value;
                    
                    // Parse integer value
                    if (int.TryParse(value, out int intValue))
                    {
                        parameters[tag] = intValue;
                    }
                }
                
                // Process effect parameters
                foreach (var param in parameters)
                {
                    switch (param.Key)
                    {
                        case "SW":
                            effectSlot.Enabled = param.Value == 1;
                            break;
                        case "TYPE":
                            effectSlot.EffectType = param.Value;
                            effectSlot.EffectName = GetEffectTypeFromId(param.Value);
                            break;
                        default:
                            // Add to parameters dictionary, converting to double
                            effectSlot.Parameters[param.Key] = param.Value;
                            break;
                    }
                }
            }
        }
        
        private EffectSlot.BankType GetEffectBankFromSingleLetter(string letter)
        {
            switch (letter.ToUpper())
            {
                case "A":
                    return EffectSlot.BankType.A;
                case "B":
                    return EffectSlot.BankType.B;
                case "C":
                    return EffectSlot.BankType.C;
                case "D":
                    return EffectSlot.BankType.D;
                default:
                    return EffectSlot.BankType.None;
            }
        }
        
        private EffectSlot.BankType GetEffectBankFromName(string bankName)
        {
            // First check if it's a single letter bank
            if (bankName.Length == 1 && char.IsLetter(bankName[0]))
            {
                return GetEffectBankFromSingleLetter(bankName);
            }
            
            // Then check for old-style names (FX1, BANK1, etc.)
            switch (bankName.ToUpper())
            {
                case "FX1":
                case "BANK1":
                    return EffectSlot.BankType.A;
                case "FX2":
                case "BANK2":
                    return EffectSlot.BankType.B;
                case "FX3":
                case "BANK3":
                    return EffectSlot.BankType.C;
                case "FX4":
                case "BANK4":
                    return EffectSlot.BankType.D;
                default:
                    // For complex tags like AA, AB, etc., just return None
                    // These will be handled in ProcessTrackFXSection
                    return EffectSlot.BankType.None;
            }
        }
        
        private string GetControlSourceName(int sourceValue)
        {
            // Implementation to map source value to name
            // This is a placeholder
            return $"Source_{sourceValue}";
        }
        
        private string GetControlTargetName(int targetValue)
        {
            // Implementation to map target value to name
            // This is a placeholder
            return $"Target_{targetValue}";
        }
        
        private string GetEffectTypeFromId(int effectTypeId)
        {
            // Implementation to map effect type ID to name
            // This is a placeholder
            return $"Effect_{effectTypeId}";
        }
    }
}