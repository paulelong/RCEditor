using System.Text;
using RCEditor.Models;
using RCEditor.Models.Services;

namespace RC600Dump.Services
{
    public class PatchFormatter
    {
        /// <summary>
        /// Stores the count value from the RC0 file
        /// </summary>
        public string Count { get; set; } = "001F"; // Default value
        
        /// <summary>
        /// Formats a bit mask that represents the 6 tracks to a readable string
        /// </summary>
        private string FormatBounceTrackBitMask(int bitMask)
        {
            List<int> selectedTracks = new List<int>();
            
            // Check each bit position (0-5) for the 6 tracks
            for (int i = 0; i < 6; i++)
            {
                // If the bit is set (1), then track i+1 is selected
                if ((bitMask & (1 << i)) != 0)
                {
                    selectedTracks.Add(i + 1);
                }
            }
            
            if (selectedTracks.Count == 0)
            {
                return "None";
            }
            else
            {
                return $"{bitMask} (Tracks: {string.Join(", ", selectedTracks)})";
            }
        }
        
        /// <summary>
        /// Determines if an effect supports the sequence functionality
        /// </summary>
        private bool SupportsSequence(string effectName)
        {
            // Normalize the effect name to make comparison easier
            string normalized = effectName?.ToUpper() ?? "";
            
            // List of effects that support sequencing according to the RC600Param.md documentation
            return normalized.Contains("LPF") ||
                   normalized.Contains("BPF") ||
                   normalized.Contains("HPF") ||
                   normalized.Contains("PHASER") ||
                   normalized.Contains("FLANGER") ||
                   normalized.Contains("SYNTH") ||
                   normalized.Contains("RING MOD") || 
                   normalized.Contains("TRANSPOSE") ||
                   normalized.Contains("PITCH BEND") ||
                   normalized.Contains("OSC BOT") ||
                   normalized.Contains("ISOLATOR") ||
                   normalized.Contains("OCTAVE") ||
                   normalized.Contains("MANUAL PAN") ||
                   normalized.Contains("TREMOLO") ||
                   normalized.Contains("VIBRATO");
        }
        
        /// <summary>
        /// Formats the sequence data for an effect, if present
        /// </summary>
        private void FormatSequenceData(StringBuilder sb, EffectSlot effect)
        {
            // If this effect doesn't support sequences, don't process
            if (!SupportsSequence(effect.EffectName))
                return;
                
            // For effect types that support SEQ, confirm the SEQ data exists
            // by checking for at least one of the specific SEQ parameters
            bool hasSeqData = effect.Parameters.ContainsKey("SEQ_SW") || 
                              effect.Parameters.ContainsKey("SEQ") || 
                              effect.Parameters.ContainsKey("S_SW");
            
            // Additional check - if we have a _SEQ tag for this effect but no data,
            // we shouldn't display the sequence section
            if (!hasSeqData && effect.Parameters.Count > 0)
            {
                // If all SEQ parameters are 0, then there's no meaningful sequence data
                // This happens when the effect has a _SEQ section with all zeros
                bool allZeros = true;
                int seqParamCount = 0;
                
                foreach (var param in effect.Parameters)
                {
                    if (param.Key.StartsWith("SEQ_") || param.Key.StartsWith("S_"))
                    {
                        seqParamCount++;
                        if (param.Value != 0)
                        {
                            allZeros = false;
                            break;
                        }
                    }
                }
                
                // If we found SEQ parameters and they're all zeros, or we didn't find any,
                // then don't show the sequence section
                if ((seqParamCount > 0 && allZeros) || seqParamCount == 0)
                    return;
                    
                // If we have SEQ parameters and not all are zero, then we have data to display
                hasSeqData = seqParamCount > 0;
            }
            
            if (!hasSeqData)
                return;
                
            sb.AppendLine($"        Sequence:");
            
            // Sequence switch (may have different parameter names based on the dump format)
            if (effect.Parameters.TryGetValue("SEQ_SW", out int seqSw) || 
                effect.Parameters.TryGetValue("SEQ", out seqSw) || 
                effect.Parameters.TryGetValue("S_SW", out seqSw))
            {
                sb.AppendLine($"          SW: {(seqSw == 1 ? "ON" : "OFF")}");
            }
            
            // Sync
            if (effect.Parameters.TryGetValue("SEQ_SYNC", out int seqSync) || 
                effect.Parameters.TryGetValue("S_SYNC", out seqSync))
            {
                sb.AppendLine($"          Sync: {(seqSync == 1 ? "ON" : "OFF")}");
            }
            
            // Retrigger
            if (effect.Parameters.TryGetValue("SEQ_RETRIG", out int seqRetrig) || 
                effect.Parameters.TryGetValue("S_RETRIG", out seqRetrig))
            {
                sb.AppendLine($"          Retrigger: {(seqRetrig == 1 ? "ON" : "OFF")}");
            }
            
            // Target parameter 
            if (effect.Parameters.TryGetValue("SEQ_TARGET", out int seqTarget) || 
                effect.Parameters.TryGetValue("S_TARGET", out seqTarget))
            {
                string targetName = GetSequenceTargetName(effect.EffectName, seqTarget);
                sb.AppendLine($"          Target: {targetName}");
            }
            
            // Rate
            if (effect.Parameters.TryGetValue("SEQ_RATE", out int seqRate) || 
                effect.Parameters.TryGetValue("S_RATE", out seqRate) ||
                effect.Parameters.TryGetValue("STEP_RATE", out seqRate))
            {
                string rateText = FormatSequenceRate(seqRate);
                sb.AppendLine($"          Rate: {rateText}");
            }
            
            // Max Steps
            if (effect.Parameters.TryGetValue("SEQ_MAX", out int seqMax) || 
                effect.Parameters.TryGetValue("S_MAX", out seqMax))
            {
                sb.AppendLine($"          Max Steps: {seqMax}");
            }
            
            // Step Values (VAL1-VAL16)
            List<int> stepValues = new List<int>();
            for (int i = 1; i <= 16; i++)
            {
                string valKey = $"SEQ_VAL{i}";
                string altValKey = $"S_VAL{i}";
                
                if (effect.Parameters.TryGetValue(valKey, out int val) || 
                    effect.Parameters.TryGetValue(altValKey, out val))
                {
                    stepValues.Add(val);
                }
            }
            
            if (stepValues.Count > 0)
            {
                sb.AppendLine($"          Step Values: {string.Join(", ", stepValues)}");
            }
        }
        
        /// <summary>
        /// Gets a meaningful name for the sequence target parameter
        /// </summary>
        private string GetSequenceTargetName(string effectName, int targetValue)
        {
            // Different effects have different parameters that can be targeted
            // This is a simplified implementation - a full one would need to know
            // every possible target for each effect type
            string normalized = effectName?.ToUpper() ?? "";
            
            // Common targets for filter effects
            if (normalized.Contains("LPF") || normalized.Contains("BPF") || normalized.Contains("HPF"))
            {
                return targetValue switch
                {
                    0 => "DEPTH",
                    1 => "CUTOFF",
                    _ => $"Unknown ({targetValue})"
                };
            }
            
            // Phaser targets
            if (normalized.Contains("PHASER"))
            {
                return targetValue switch
                {
                    0 => "DEPTH",
                    1 => "RESONANCE",
                    2 => "MANUAL",
                    3 => "D.LEVEL",
                    4 => "E.LEVEL",
                    _ => $"Unknown ({targetValue})"
                };
            }
            
            // Flanger targets
            if (normalized.Contains("FLANGER"))
            {
                return targetValue switch
                {
                    0 => "DEPTH",
                    1 => "RESONANCE",
                    2 => "MANUAL",
                    3 => "D.LEVEL",
                    4 => "E.LEVEL",
                    5 => "SEPARATION",
                    _ => $"Unknown ({targetValue})"
                };
            }
            
            // For other effects, just return the raw value
            return $"Parameter {targetValue}";
        }
        
        /// <summary>
        /// Formats the sequence rate value to a meaningful string
        /// </summary>
        private string FormatSequenceRate(int rateValue)
        {
            // Based on RC-600 Parameter Guide
            if (rateValue == 0) return "OFF";
            if (rateValue == 1) return "4MEAS";
            if (rateValue == 2) return "2MEAS";
            if (rateValue == 3) return "1MEAS";
            if (rateValue == 4) return "♩"; // Quarter note
            if (rateValue == 5) return "♪"; // Eighth note
            if (rateValue == 6) return "♬"; // Sixteenth note
            if (rateValue == 7) return "♬."; // Thirty-second note
            
            // For raw values
            return rateValue.ToString();
        }

        /// <summary>
        /// Formats a single-line summary of a patch
        /// </summary>
        public string FormatPatchSummary(int patchNumber, MemoryPatch patch)
        {
            return $"Patch {patchNumber:D2}: Name={patch.Name}";
        }
        
        /// <summary>
        /// Formats a detailed view of a patch
        /// </summary>
        public string FormatPatchDetails(int patchNumber, char variation, MemoryPatch patch)
        {
            var sb = new StringBuilder();
            
            // Patch header with count value
            sb.AppendLine($"Patch {patchNumber:D2}{variation} - \"{patch.Name}\" (Full Dump):");
            sb.AppendLine($"Count: {Count}");
            sb.AppendLine();
            
            // Basic patch info
            sb.AppendLine($"Name: {patch.Name}");
            
            // Play mode settings
            // string playMode = patch.PlayMode == PlayModeEnum.Multi ? "MULTI" : "SINGLE";
            
            // sb.AppendLine($"PlayMode: {playMode}");
            
            // Master section
            if (patch.Master != null)
            {
                sb.AppendLine();
                sb.AppendLine("Master Settings:");
                sb.AppendLine($"  Loop Position: {patch.Master.LoopPosition}");
                sb.AppendLine($"  Loop Length: {patch.Master.LoopLength}");
                sb.AppendLine($"  Mode Flag: {patch.Master.ModeFlag}");
                sb.AppendLine($"  Mode Value: {patch.Master.ModeValue}");
            }
            
            // Rec settings
            sb.AppendLine();
            sb.AppendLine("Rec Settings:");
            sb.AppendLine($"  Rec Action: {patch.Rec.RecAction}");
            sb.AppendLine($"  Quantize: {(patch.Rec.QuantizeEnabled ? "ON" : "OFF")}");
            sb.AppendLine($"  Auto Rec: {(patch.Rec.AutoRecEnabled ? "ON" : "OFF")}");
            sb.AppendLine($"  Auto Rec Sensitivity: {patch.Rec.AutoRecSensitivity}");
            sb.AppendLine($"  Bounce: {(patch.Rec.BounceEnabled ? "ON" : "OFF")}");
            
            // Format BounceTrack to show which tracks are included in the bounce operation
            string bounceTrackDisplay = FormatBounceTrackBitMask(patch.Rec.BounceTrack);
            sb.AppendLine($"  Bounce Track: {bounceTrackDisplay}");
            
            // Rec settings
            sb.AppendLine();
            sb.AppendLine("Play Settings:");
            sb.AppendLine($"  Single Track Change: {patch.Play.SingleTrackChange}");
            sb.AppendLine($"  Fade Time In: {patch.Play.FadeTimeIn}");
            sb.AppendLine($"  Fade Time Out: {patch.Play.FadeTimeOut}");
            sb.AppendLine($"  All Start Tracks: {string.Join(", ", patch.Play.AllStartTracks.Select((enabled, i) => enabled ? (i+1).ToString() : "").Where(s => !string.IsNullOrEmpty(s)))}");
            sb.AppendLine($"  All Stop Tracks: {string.Join(", ", patch.Play.AllStopTracks.Select((enabled, i) => enabled ? (i+1).ToString() : "").Where(s => !string.IsNullOrEmpty(s)))}");
            sb.AppendLine($"  Loop Length: {(patch.Play.LoopLength == 0 ? "AUTO" : patch.Play.LoopLength.ToString())}");
            sb.AppendLine($"  Speed Change: {patch.Play.SpeedChange}");
            sb.AppendLine($"  Sync Adjust: {patch.Play.SyncAdjust}");
              // Rhythm settings
            sb.AppendLine();
            sb.AppendLine("Rhythm Settings:");
            string genreName = RhythmPatternNameService.GetGenreName(Convert.ToInt32(patch.Rhythm.Genre));
            sb.AppendLine($"  Genre: {genreName}");
              // Get the pattern name using the stored PatternId (0-based)
            string patternName = RhythmPatternNameService.GetPatternNameByGenre(genreName, patch.Rhythm.PatternId);
            sb.AppendLine($"  Pattern: {patch.Rhythm.Pattern} ({patternName})");
            sb.AppendLine($"  Variation: {patch.Rhythm.Variation} ({RhythmPatternNameService.GetVariationDescription(patch.Rhythm.Variation)})");
            
            // Get the kit name using the stored Kit value
            string kitName = RhythmPatternNameService.GetKitName(patch.Rhythm.Kit);
            sb.AppendLine($"  Kit: {patch.Rhythm.Kit} ({kitName})");
            sb.AppendLine($"  Beat: {patch.Rhythm.Beat}");
            sb.AppendLine($"  Start Mode: {patch.Rhythm.StartMode}");
            sb.AppendLine($"  Stop Mode: {patch.Rhythm.StopMode}");
            sb.AppendLine($"  Intro On Rec: {(patch.Rhythm.IntroOnRec ? "ON" : "OFF")}");
            sb.AppendLine($"  Intro On Play: {(patch.Rhythm.IntroOnPlay ? "ON" : "OFF")}");
            sb.AppendLine($"  Ending: {(patch.Rhythm.Ending ? "ON" : "OFF")}");
            sb.AppendLine($"  Fill In: {(patch.Rhythm.FillIn ? "ON" : "OFF")}");
            sb.AppendLine($"  Variation Change Timing: {patch.Rhythm.VariationChangeTiming}");
            
            // Track settings
            for (int i = 0; i < patch.Tracks.Length; i++)
            {
                sb.AppendLine();
                sb.AppendLine($"Track {i + 1} Settings:");
                
                var track = patch.Tracks[i];
                
                // Only show the track details if we have data for this track
                if (track != null)
                {
                    // Track settings in order matching the RC600Param.md documentation
                    sb.AppendLine($"  Reverse: {(track.Reverse ? "ON" : "OFF")}");
                    sb.AppendLine($"  OneShot: {(track.OneShot ? "ON" : "OFF")}");
                    sb.AppendLine($"  Pan: {track.Pan}");
                    sb.AppendLine($"  Volume: {track.Level}%");
                    sb.AppendLine($"  Start Mode: {track.StartMode}");
                    sb.AppendLine($"  Stop Mode: {track.StopMode}");
                    sb.AppendLine($"  Overdub Mode: {track.OverdubMode}");
                    sb.AppendLine($"  Track FX: {(track.FXEnabled ? "ON" : "OFF")}");
                    sb.AppendLine($"  Play Mode: {(track.PlayMode == PlayModeEnum.Multi ? "MULTI" : "SINGLE")}");
                    sb.AppendLine($"  Measure Count: {track.MeasureCount}");
                    if (track.UnknownK != 0)
                        sb.AppendLine($"  Unknown K: {track.UnknownK}");
                    sb.AppendLine($"  Loop Sync: {(track.LoopSyncSw ? "ON" : "OFF")}");
                    sb.AppendLine($"  Loop Sync Mode: {track.LoopSyncMode}");
                    sb.AppendLine($"  Tempo Sync: {(track.TempoSyncSw ? "ON" : "OFF")}");
                    sb.AppendLine($"  Tempo Sync Mode: {track.TempoSyncMode}");
                    sb.AppendLine($"  Tempo Sync Speed: {track.TempoSyncSpeed}");
                    if (track.UnknownP != 0)
                        sb.AppendLine($"  Unknown P: {track.UnknownP}");
                    sb.AppendLine($"  Bounce In: {(track.BounceIn ? "ON" : "OFF")}");
                    sb.AppendLine($"  Measure Count B: {track.MeasureCountB}");
                      // Unknown parameters
                    if (track.UnknownR != 0)
                        sb.AppendLine($"  Unknown R: {track.UnknownR}");
                    if (track.UnknownT != 0)
                        sb.AppendLine($"  Unknown T: {track.UnknownT}");
                    if (track.UnknownU != 0)
                        sb.AppendLine($"  Unknown U: {track.UnknownU}");
                    if (track.UnknownV != 0)
                        sb.AppendLine($"  Unknown V: {track.UnknownV}");
                    if (track.UnknownX != 0)
                        sb.AppendLine($"  Unknown X: {track.UnknownX}");                    // Input routing                    sb.AppendLine($"  Input Routing:");
                    
                    // Display all available input routing fields
                    sb.AppendLine($"    Mic 1: {(track.InputRouting?.Mic1Enabled == true ? "ON" : "OFF")}");
                    sb.AppendLine($"    Mic 2: {(track.InputRouting?.Mic2Enabled == true ? "ON" : "OFF")}");
                    sb.AppendLine($"    Rhythm: {(track.InputRouting?.RhythmEnabled == true ? "ON" : "OFF")}");
                    
                    // Show all stereo channel info for completeness
                    sb.AppendLine($"    Mic 1 Left: {(track.InputRouting?.Mic1LeftEnabled == true ? "ON" : "OFF")}");
                    sb.AppendLine($"    Mic 1 Right: {(track.InputRouting?.Mic1RightEnabled == true ? "ON" : "OFF")}");
                    sb.AppendLine($"    Mic 2 Left: {(track.InputRouting?.Mic2LeftEnabled == true ? "ON" : "OFF")}");
                    sb.AppendLine($"    Mic 2 Right: {(track.InputRouting?.Mic2RightEnabled == true ? "ON" : "OFF")}");
                    
                }
                else
                {
                    sb.AppendLine("  (No track data available)");
                }
            }
            
            // Input FX
            if (patch.InputFX != null)
            {
                sb.AppendLine();
                sb.AppendLine("Input Effects:");
                sb.AppendLine($"  Active Bank: {patch.InputFX.ActiveBank}");
                
                foreach (var effectBank in patch.InputFX.Banks)
                {
                    sb.AppendLine($"  Bank {effectBank.Key} Effects:");
                    
                    // Add bank settings at the beginning of each bank section
                    FormatBankSettings(sb, effectBank.Value.Slots);
                    
                    foreach (var effect in effectBank.Value.Slots)
                    {
                        // Check if we have valid effect data to display
                        if (effect.Value != null && !string.IsNullOrEmpty(effect.Value.EffectName))
                        {
                            sb.AppendLine($"    Slot {effect.Key}: {effect.Value.EffectName} ({(effect.Value.Enabled ? "enabled" : "disabled")})");
                            
                            if (!string.IsNullOrEmpty(effect.Value.Target))
                                sb.AppendLine($"      Target: {effect.Value.Target}");
                                
                            if (effect.Value.SwitchMode != SwitchModeEnum.Toggle)
                                sb.AppendLine($"      Switch Mode: {effect.Value.SwitchMode}");
                            
                            if (effect.Value.Parameters.Count > 0)
                            {
                                sb.AppendLine($"      Parameters:");
                                foreach (var param in effect.Value.Parameters)
                                {
                                    // Skip sequence-related parameters in the general parameters list
                                    // These will be displayed in the Sequence section for effects that support it
                                    if (param.Key.StartsWith("SEQ_") || param.Key.StartsWith("S_"))
                                        continue;
                                        
                                    sb.AppendLine($"        {param.Key}: {param.Value}");
                                }
                            }
                            
                            // Add sequence data formatting if present
                            FormatSequenceData(sb, effect.Value);
                        }
                    }
                }
            }
            
            // Track FX
            if (patch.TrackFX != null)
            {
                sb.AppendLine();
                sb.AppendLine("Track Effects:");
                sb.AppendLine($"  Active Bank: {patch.TrackFX.ActiveBank}");
                
                foreach (var effectBank in patch.TrackFX.Banks)
                {
                    sb.AppendLine($"  Bank {effectBank.Key} Effects:");
                    
                    // Add bank settings at the beginning of each bank section
                    FormatBankSettings(sb, effectBank.Value.Slots);
                    
                    foreach (var effect in effectBank.Value.Slots)
                    {
                        // Check if we have valid effect data to display
                        if (effect.Value != null && !string.IsNullOrEmpty(effect.Value.EffectName))
                        {
                            sb.AppendLine($"    Slot {effect.Key}: {effect.Value.EffectName} ({(effect.Value.Enabled ? "enabled" : "disabled")})");
                            
                            if (!string.IsNullOrEmpty(effect.Value.Target))
                                sb.AppendLine($"      Target: {effect.Value.Target}");
                                
                            if (effect.Value.SwitchMode != SwitchModeEnum.Toggle)  
                                sb.AppendLine($"      Switch Mode: {effect.Value.SwitchMode}");
                            
                            if (effect.Value.Parameters.Count > 0)
                            {
                                sb.AppendLine($"      Parameters:");
                                foreach (var param in effect.Value.Parameters)
                                {
                                    // Skip sequence-related parameters in the general parameters list
                                    // These will be displayed in the Sequence section for effects that support it
                                    if (param.Key.StartsWith("SEQ_") || param.Key.StartsWith("S_"))
                                        continue;
                                        
                                    sb.AppendLine($"        {param.Key}: {param.Value}");
                                }
                            }
                            
                            // Add sequence data formatting if present
                            FormatSequenceData(sb, effect.Value);
                        }
                    }
                }
            }
            
            // Assigns
            if (patch.Assigns != null && patch.Assigns.Count > 0)
            {
                sb.AppendLine();
                sb.AppendLine("Assignments:");
                
                foreach (var assign in patch.Assigns)
                {
                    sb.AppendLine($"  {assign.Source} -> {assign.Target} (Mode: {assign.ActionMode})");
                    if (assign.RangeMin.HasValue && assign.RangeMax.HasValue)
                    {
                        sb.AppendLine($"    Range: {assign.RangeMin} to {assign.RangeMax}");
                    }
                }
            }
            
            // Control assignments
            if (patch.Controls != null)
            {
                sb.AppendLine();
                sb.AppendLine("Control Assignments:");
                
                // Expression pedals
                if (!string.IsNullOrEmpty(patch.Controls.Expression1))
                {
                    sb.AppendLine($"  Expression Pedal 1: {patch.Controls.Expression1}");
                }
                
                if (!string.IsNullOrEmpty(patch.Controls.Expression2))
                {
                    sb.AppendLine($"  Expression Pedal 2: {patch.Controls.Expression2}");
                }
                
                // External switches
                if (!string.IsNullOrEmpty(patch.Controls.ExternalSwitch1))
                {
                    sb.AppendLine($"  External Switch 1: {patch.Controls.ExternalSwitch1}");
                }
                
                if (!string.IsNullOrEmpty(patch.Controls.ExternalSwitch2))
                {
                    sb.AppendLine($"  External Switch 2: {patch.Controls.ExternalSwitch2}");
                }
                
                // Pedals
                foreach (var pedal in patch.Controls.Pedals)
                {
                    sb.AppendLine($"  Pedal {pedal.Key}:");
                    if (!string.IsNullOrEmpty(pedal.Value.Mode1))
                        sb.AppendLine($"    Mode 1: {pedal.Value.Mode1}");
                    if (!string.IsNullOrEmpty(pedal.Value.Mode2))
                        sb.AppendLine($"    Mode 2: {pedal.Value.Mode2}");
                    if (!string.IsNullOrEmpty(pedal.Value.Mode3))
                        sb.AppendLine($"    Mode 3: {pedal.Value.Mode3}");
                }
            }
            
            return sb.ToString();
        }
        
        /// <summary>
        /// Formats the bank settings (SW, MODE, FX Target) 
        /// </summary>
        private void FormatBankSettings(StringBuilder sb, Dictionary<int, EffectSlot> slots)
        {
            // Get the bank settings from any slot in the bank
            // Bank settings should be the same for all slots in the bank
            if (slots.Count == 0 || slots.Values.FirstOrDefault() == null)
                return;
                
            var firstSlot = slots.Values.FirstOrDefault();
            if (firstSlot == null || firstSlot.Parameters == null)
                return;
            
            // Check for bank mode parameter
            string bankMode = "SINGLE";
            if (firstSlot.Parameters.TryGetValue("BankMode", out int bankModeValue))
            {
                bankMode = bankModeValue == 1 ? "MULTI" : "SINGLE";
            }
            
            // SW (Bank On/Off) is represented by the Enabled property
            bool bankEnabled = firstSlot.Enabled;
            
            // Get the target if it exists
            string target = !string.IsNullOrEmpty(firstSlot.Target) ? firstSlot.Target : "ALL";
            
            // Display bank settings
            sb.AppendLine($"    Bank Settings:");
            sb.AppendLine($"      SW: {(bankEnabled ? "ON" : "OFF")}");
            sb.AppendLine($"      MODE: {bankMode}");
            sb.AppendLine($"      FX Target: {target}");
            sb.AppendLine();
        }
    }
}