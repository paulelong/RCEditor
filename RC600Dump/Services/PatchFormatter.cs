using System.Text;
using RCEditor.Models;

namespace RC600Dump.Services
{
    public class PatchFormatter
    {
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
        /// Formats a single-line summary of a patch
        /// </summary>
        public string FormatPatchSummary(int patchNumber, MemoryPatch patch)
        {
            string playMode = patch.PlayMode == PlayModeEnum.Multi ? "MULTI" : "SINGLE";
            string loopSync = patch.LoopSync ? "ON" : "OFF";
            string singleModeSwitch = patch.SingleModeSwitch == SingleModeSwitchEnum.Loop ? "LOOP_END" : "IMMEDIATE";
            
            return $"Patch {patchNumber:D2}: Name=\"{patch.Name}\", Tempo={patch.Tempo:F1} BPM, PlayMode={playMode}, LoopSync={loopSync}, SingleModeSwitch={singleModeSwitch}";
        }
        
        /// <summary>
        /// Formats a detailed view of a patch
        /// </summary>
        public string FormatPatchDetails(int patchNumber, char variation, MemoryPatch patch)
        {
            var sb = new StringBuilder();
            
            // Patch header
            sb.AppendLine($"Patch {patchNumber:D2}{variation} - \"{patch.Name}\" (Full Dump):");
            sb.AppendLine();
            
            // Basic patch info
            sb.AppendLine($"Name: {patch.Name}");
            sb.AppendLine($"Tempo: {patch.Tempo:F1} BPM");
            
            // Play mode settings
            string playMode = patch.PlayMode == PlayModeEnum.Multi ? "MULTI" : "SINGLE";
            string loopSync = patch.LoopSync ? "ON" : "OFF";
            string singleModeSwitch = patch.SingleModeSwitch == SingleModeSwitchEnum.Loop ? "LOOP_END" : "IMMEDIATE";
            
            sb.AppendLine($"PlayMode: {playMode}");
            sb.AppendLine($"LoopSync: {loopSync}");
            sb.AppendLine($"SingleModeSwitch (Track Change Mode): {singleModeSwitch}");
            
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
            sb.AppendLine($"  Enabled: {(patch.Rhythm.Enabled ? "ON" : "OFF")}");
            if (patch.Rhythm.Enabled)
            {
                sb.AppendLine($"  Genre: {patch.Rhythm.Genre}");
                sb.AppendLine($"  Pattern: {patch.Rhythm.Pattern}");
                sb.AppendLine($"  Variation: {patch.Rhythm.Variation}");
                sb.AppendLine($"  Kit: {patch.Rhythm.Kit}");
                sb.AppendLine($"  Beat: {patch.Rhythm.Beat}");
                sb.AppendLine($"  Start Mode: {patch.Rhythm.StartMode}");
                sb.AppendLine($"  Stop Mode: {patch.Rhythm.StopMode}");
                sb.AppendLine($"  Intro On Rec: {(patch.Rhythm.IntroOnRec ? "ON" : "OFF")}");
                sb.AppendLine($"  Intro On Play: {(patch.Rhythm.IntroOnPlay ? "ON" : "OFF")}");
                sb.AppendLine($"  Ending: {(patch.Rhythm.Ending ? "ON" : "OFF")}");
                sb.AppendLine($"  Fill In: {(patch.Rhythm.FillIn ? "ON" : "OFF")}");
                sb.AppendLine($"  Variation Change Timing: {patch.Rhythm.VariationChangeTiming}");
            }
            
            // Track settings
            for (int i = 0; i < patch.Tracks.Length; i++)
            {
                sb.AppendLine();
                sb.AppendLine($"Track {i + 1} Settings:");
                
                var track = patch.Tracks[i];
                
                // Only show the track details if we have data for this track
                if (track != null)
                {
                    sb.AppendLine($"  Pan: {track.Pan}");
                    sb.AppendLine($"  Volume: {track.Level}%");
                    sb.AppendLine($"  Reverse: {(track.Reverse ? "ON" : "OFF")}");
                    sb.AppendLine($"  OneShot: {(track.OneShot ? "ON" : "OFF")}");
                    sb.AppendLine($"  Track FX: {(track.FXEnabled ? "ON" : "OFF")}");
                    sb.AppendLine($"  Start Mode: {track.StartMode}");
                    sb.AppendLine($"  Fade In Measures: {track.FadeInMeasures}");
                    sb.AppendLine($"  Stop Mode: {track.StopMode}");
                    sb.AppendLine($"  Fade Out Measures: {track.FadeOutMeasures}");
                    sb.AppendLine($"  Overdub Mode: {track.OverdubMode}");
                    sb.AppendLine($"  Tempo Sync: {(track.TempoSyncSw ? "ON" : "OFF")}");
                    sb.AppendLine($"  Tempo Sync Mode: {track.TempoSyncMode}");
                    sb.AppendLine($"  Tempo Sync Speed: {track.TempoSyncSpeed}");
                    sb.AppendLine($"  Measure Count: {track.MeasureCount}");
                    sb.AppendLine($"  Loop Sync: {(track.LoopSyncSw ? "ON" : "OFF")}");
                    sb.AppendLine($"  Loop Sync Mode: {track.LoopSyncMode}");
                    sb.AppendLine($"  Bounce In: {(track.BounceIn ? "ON" : "OFF")}");
                    
                    // Input routing
                    sb.AppendLine($"  Input Routing:");
                    sb.AppendLine($"    Mic In: {track.InputRouting?.MicIn}");
                    sb.AppendLine($"    Inst 1: {track.InputRouting?.Inst1}");
                    sb.AppendLine($"    Inst 2: {track.InputRouting?.Inst2}");
                    
                    // Output assignment
                    sb.AppendLine($"  Output Assign: {track.OutputAssign}");
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
                
                foreach (var effectBank in patch.InputFX.Banks)
                {
                    sb.AppendLine($"  Bank {effectBank.Key} Effects:");
                    
                    foreach (var effect in effectBank.Value.Slots)
                    {
                        if (effect.Value != null && !string.IsNullOrEmpty(effect.Value.Type) && effect.Value.Type != "None")
                        {
                            sb.AppendLine($"    Slot {effect.Key}: {effect.Value.Type} ({(effect.Value.Enabled ? "enabled" : "disabled")})");
                            sb.AppendLine($"      Target: {effect.Value.Target}");
                            sb.AppendLine($"      Switch Mode: {effect.Value.SwitchMode}");
                            
                            if (effect.Value.Parameters.Count > 0)
                            {
                                sb.AppendLine($"      Parameters:");
                                foreach (var param in effect.Value.Parameters)
                                {
                                    sb.AppendLine($"        {param.Key}: {param.Value}");
                                }
                            }
                        }
                    }
                }
            }
            
            // Track FX
            if (patch.TrackFX != null)
            {
                sb.AppendLine();
                sb.AppendLine("Track Effects:");
                
                foreach (var effectBank in patch.TrackFX.Banks)
                {
                    sb.AppendLine($"  Bank {effectBank.Key} Effects:");
                    
                    foreach (var effect in effectBank.Value.Slots)
                    {
                        if (effect.Value != null && !string.IsNullOrEmpty(effect.Value.Type) && effect.Value.Type != "None")
                        {
                            sb.AppendLine($"    Slot {effect.Key}: {effect.Value.Type} ({(effect.Value.Enabled ? "enabled" : "disabled")})");
                            sb.AppendLine($"      Target: {effect.Value.Target}");
                            sb.AppendLine($"      Switch Mode: {effect.Value.SwitchMode}");
                            
                            if (effect.Value.Parameters.Count > 0)
                            {
                                sb.AppendLine($"      Parameters:");
                                foreach (var param in effect.Value.Parameters)
                                {
                                    sb.AppendLine($"        {param.Key}: {param.Value}");
                                }
                            }
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
    }
}