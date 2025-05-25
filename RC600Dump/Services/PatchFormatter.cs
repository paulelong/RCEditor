using System.Text;
using RCEditor.Models;
using RCEditor.Models.Services;
using System.IO;
using System.Text.RegularExpressions;

namespace RC600Dump.Services
{
    public class PatchFormatter
    {
        /// <summary>
        /// Stores the count value from the RC0 file
        /// </summary>
        public string Count { get; set; } = "001F"; // Default value
          /// <summary>        /// <summary>
        /// Reads and displays high-level system information from SYSTEM1.RC0 and SYSTEM2.RC0 files
        /// Uses count-based logic to determine which system file has the highest count
        /// </summary>
        /// <param name="dataPath">Path to the DATA directory containing the system files</param>
        /// <returns>Formatted string containing system information</returns>
        public async Task<string> FormatSystemInformationAsync(string dataPath)
        {
            var sb = new StringBuilder();
            sb.AppendLine("=== SYSTEM INFORMATION ===");
            sb.AppendLine();
            
            var systemParser = new SystemSettingsParser();
            
            try
            {
                SystemSettings? system1Settings = null;
                SystemSettings? system2Settings = null;
                string system1Path = Path.Combine(dataPath, "SYSTEM1.RC0");
                string system2Path = Path.Combine(dataPath, "SYSTEM2.RC0");
                
                // Read both system files if they exist
                if (File.Exists(system1Path))
                {
                    system1Settings = await systemParser.ReadSystemFileAsync(system1Path);
                }
                
                if (File.Exists(system2Path))
                {
                    system2Settings = await systemParser.ReadSystemFileAsync(system2Path);
                }
                
                // Determine which system file has the highest count
                SystemSettings? primarySystemSettings = null;
                string? primarySystemFile = null;
                  if (system1Settings != null && system2Settings != null)
                {
                    // Compare count values using the helper method
                    if (IsCountHigherOrEqual(system1Settings.Count, system2Settings.Count))
                    {
                        primarySystemSettings = system1Settings;
                        primarySystemFile = "SYSTEM1.RC0";
                        this.Count = system1Settings.Count ?? "001F"; // Update the formatter's count property
                    }
                    else
                    {
                        primarySystemSettings = system2Settings;
                        primarySystemFile = "SYSTEM2.RC0";
                        this.Count = system2Settings.Count ?? "001F"; // Update the formatter's count property
                    }
                    
                    sb.AppendLine($"Primary System File: {primarySystemFile} (Count: {primarySystemSettings.Count ?? "001F"})");
                    sb.AppendLine($"Secondary System File: {(primarySystemFile == "SYSTEM1.RC0" ? "SYSTEM2.RC0" : "SYSTEM1.RC0")} (Count: {(primarySystemFile == "SYSTEM1.RC0" ? system2Settings.Count ?? "001F" : system1Settings.Count ?? "001F")})");
                    sb.AppendLine();
                }
                else if (system1Settings != null)
                {
                    primarySystemSettings = system1Settings;
                    primarySystemFile = "SYSTEM1.RC0";
                    this.Count = system1Settings.Count ?? "001F";
                    sb.AppendLine($"Using: {primarySystemFile} (Count: {primarySystemSettings.Count ?? "001F"})");
                    sb.AppendLine("SYSTEM2.RC0: File not found");
                    sb.AppendLine();
                }
                else if (system2Settings != null)
                {
                    primarySystemSettings = system2Settings;
                    primarySystemFile = "SYSTEM2.RC0";
                    this.Count = system2Settings.Count ?? "001F";
                    sb.AppendLine("SYSTEM1.RC0: File not found");
                    sb.AppendLine($"Using: {primarySystemFile} (Count: {primarySystemSettings.Count ?? "001F"})");
                    sb.AppendLine();
                }
                else
                {
                    sb.AppendLine("SYSTEM1.RC0: File not found");
                    sb.AppendLine("SYSTEM2.RC0: File not found");
                    sb.AppendLine();
                }
                
                // Display detailed information for the primary system file
                if (primarySystemSettings != null)
                {
                    sb.AppendLine($"=== {primarySystemFile} SETTINGS (Primary) ===");
                    FormatSystemSettingsInfo(sb, primarySystemSettings);
                    sb.AppendLine();
                }
                
                // Show count comparison summary
                if (system1Settings != null && system2Settings != null)
                {
                    sb.AppendLine("=== COUNT COMPARISON ===");
                    sb.AppendLine($"SYSTEM1.RC0 Count: {system1Settings.Count ?? "001F"}");
                    sb.AppendLine($"SYSTEM2.RC0 Count: {system2Settings.Count ?? "001F"}");
                    sb.AppendLine($"Selected: {primarySystemFile} (highest count)");
                    sb.AppendLine();
                }
            }
            catch (Exception ex)
            {
                sb.AppendLine($"Error reading system files: {ex.Message}");
                sb.AppendLine();
            }
            
            sb.AppendLine("=== END SYSTEM INFORMATION ===");
            sb.AppendLine();
            return sb.ToString();
        }
          /// <summary>
        /// Formats system settings information into a readable string
        /// </summary>
        /// <param name="sb">StringBuilder to append the information to</param>
        /// <param name="settings">The system settings to format</param>
        private void FormatSystemSettingsInfo(StringBuilder sb, SystemSettings settings)
        {
            // SETUP section
            if (settings.Setup != null)
            {
                sb.AppendLine("  SETUP:");
                sb.AppendLine($"    Contrast: {settings.Setup.Contrast}");
                sb.AppendLine($"    Display Mode: {settings.Setup.DisplayMode}");
                sb.AppendLine($"    Loop Indicators: {settings.Setup.LoopIndicators}");
                sb.AppendLine($"    Auto Off: {(settings.Setup.AutoOff ? "Enabled" : "Disabled")}");
                sb.AppendLine($"    Memory Range: {settings.Setup.MemoryExtentMin:D2}-{settings.Setup.MemoryExtentMax:D2}");
                if (settings.Setup.KnobFunc?.Length > 0)
                {
                    sb.AppendLine($"    Knob Functions: {string.Join(", ", settings.Setup.KnobFunc.Where(k => !string.IsNullOrEmpty(k)))}");
                }
                sb.AppendLine();
            }
            
            // USB section
            if (settings.Usb != null)
            {
                sb.AppendLine("  USB:");
                sb.AppendLine($"    Storage: {(settings.Usb.Storage ? "Connected" : "Off")}");
                sb.AppendLine($"    Audio Mode: {settings.Usb.AudioMode}");
                sb.AppendLine($"    Routing: {settings.Usb.Routing}");
                sb.AppendLine($"    Input Level: {settings.Usb.InputLevel}");
                sb.AppendLine($"    Output Level: {settings.Usb.OutputLevel}");
                sb.AppendLine();
            }
            
            // MIDI section
            if (settings.Midi != null)
            {
                sb.AppendLine("  MIDI:");
                sb.AppendLine($"    RX Ch Control: {settings.Midi.RxChCtl}");
                sb.AppendLine($"    RX Ch Rhythm: {settings.Midi.RxChRhythm}");
                sb.AppendLine($"    RX Ch Voice: {settings.Midi.RxChVoice}");
                sb.AppendLine($"    TX Channel: {settings.Midi.TxCh}");
                sb.AppendLine($"    Sync Clock: {settings.Midi.SyncClock}");
                sb.AppendLine($"    Clock Out: {(settings.Midi.ClockOut ? "Enabled" : "Disabled")}");
                sb.AppendLine($"    Start Sync: {settings.Midi.StartSync}");
                sb.AppendLine($"    PC Out: {(settings.Midi.PcOut ? "Enabled" : "Disabled")}");
                sb.AppendLine($"    Thru: {settings.Midi.Thru}");
                sb.AppendLine();
            }
            
            // Input section
            if (settings.Input?.Setup != null)
            {
                sb.AppendLine("  INPUT:");
                sb.AppendLine($"    Phantom Mic1: {(settings.Input.Setup.PhantomMic1 ? "Enabled" : "Disabled")}");
                sb.AppendLine($"    Phantom Mic2: {(settings.Input.Setup.PhantomMic2 ? "Enabled" : "Disabled")}");
                sb.AppendLine($"    Inst1 Gain: {settings.Input.Setup.Inst1Gain}");
                sb.AppendLine($"    Inst2 Gain: {settings.Input.Setup.Inst2Gain}");
                sb.AppendLine($"    Stereo Link Mic: {(settings.Input.Setup.StereoLinkMic ? "Enabled" : "Disabled")}");
                sb.AppendLine();
            }
            
            // Output section
            if (settings.Output?.Setup != null)
            {
                sb.AppendLine("  OUTPUT:");
                sb.AppendLine($"    Output Knob: {settings.Output.Setup.OutputKnob}");
                sb.AppendLine($"    Stereo Link Main: {(settings.Output.Setup.StereoLinkMain ? "Enabled" : "Disabled")}");
                sb.AppendLine($"    Stereo Link Sub1: {(settings.Output.Setup.StereoLinkSub1 ? "Enabled" : "Disabled")}");
                sb.AppendLine($"    Stereo Link Sub2: {(settings.Output.Setup.StereoLinkSub2 ? "Enabled" : "Disabled")}");
                sb.AppendLine();
            }
            
            // Mixer section
            if (settings.Mixer != null)
            {
                sb.AppendLine("  MIXER:");
                sb.AppendLine($"    Mic1 In: {settings.Mixer.Mic1In}");
                sb.AppendLine($"    Mic2 In: {settings.Mixer.Mic2In}");
                sb.AppendLine($"    Main L Out: {settings.Mixer.MainLOut}");
                sb.AppendLine($"    Main R Out: {settings.Mixer.MainROut}");
                sb.AppendLine($"    Rhythm Out: {settings.Mixer.RhythmOut}");
                sb.AppendLine($"    Master Out: {settings.Mixer.MasterOut}");
                sb.AppendLine();
            }
            
            // Show parameter count if available
            if (!string.IsNullOrEmpty(settings.Count))
            {
                sb.AppendLine($"  Parameter Count: {settings.Count}");
                sb.AppendLine();
            }        }
        
        /// <summary>
        /// Formats a detailed view of a patch including system information
        /// </summary>
        public async Task<string> FormatPatchDetailsAsync(int patchNumber, char variation, MemoryPatch patch, string dataPath)
        {
            var sb = new StringBuilder();
            
            // Include system information first
            if (!string.IsNullOrEmpty(dataPath))
            {
                string systemInfo = await FormatSystemInformationAsync(dataPath);
                sb.Append(systemInfo);
            }
            
            // Then include the regular patch details
            string patchDetails = FormatPatchDetails(patchNumber, variation, patch);
            sb.Append(patchDetails);
            
            return sb.ToString();
        }
        
        /// <summary>
        /// Reads a system RC0 file and extracts high-level information
        /// </summary>
        /// <param name="filePath">Path to the system RC0 file</param>
        /// <returns>Dictionary containing parsed system information</returns>
        private Dictionary<string, object> ReadSystemFile(string filePath)
        {
            var systemInfo = new Dictionary<string, object>();
            
            try
            {
                string content = File.ReadAllText(filePath);
                
                // Extract count value and update the Count property
                var countMatch = Regex.Match(content, @"<count>([^<]*)</count>");
                if (countMatch.Success)
                {
                    Count = countMatch.Groups[1].Value;
                    systemInfo["Count"] = Count;
                }
                
                // Extract main sections
                systemInfo["Setup"] = ExtractSystemSection(content, "SETUP");
                systemInfo["Color"] = ExtractSystemSection(content, "COLOR");
                systemInfo["USB"] = ExtractSystemSection(content, "USB");
                systemInfo["MIDI"] = ExtractSystemSection(content, "MIDI");
                systemInfo["Input_Controls"] = new Dictionary<string, object>
                {
                    ["ICTL1"] = ExtractSystemSection(content, "ICTL1"),
                    ["ICTL2"] = ExtractSystemSection(content, "ICTL2"),
                    ["ICTL3"] = ExtractSystemSection(content, "ICTL3")
                };
                systemInfo["External_Controls"] = ExtractSystemSection(content, "ECTL");
                systemInfo["Preferences"] = ExtractSystemSection(content, "PREF");
                systemInfo["Input_Settings"] = ExtractSystemSection(content, "INPUT");
                systemInfo["Output_Settings"] = ExtractSystemSection(content, "OUTPUT");
                systemInfo["Routing"] = ExtractSystemSection(content, "ROUTING");
                systemInfo["Mixer"] = ExtractSystemSection(content, "MIXER");
                systemInfo["EQ"] = ExtractSystemSection(content, "EQ");
            }
            catch (Exception ex)
            {
                systemInfo["Error"] = ex.Message;
            }
            
            return systemInfo;
        }
        
        /// <summary>
        /// Extracts parameters from a specific XML section
        /// </summary>
        /// <param name="content">The full XML content</param>
        /// <param name="sectionName">Name of the section to extract</param>
        /// <returns>Dictionary containing the section parameters</returns>
        private Dictionary<string, int> ExtractSystemSection(string content, string sectionName)
        {
            var parameters = new Dictionary<string, int>();
            
            var sectionPattern = $@"<{sectionName}>(.*?)</{sectionName}>";
            var sectionMatch = Regex.Match(content, sectionPattern, RegexOptions.Singleline);
            
            if (sectionMatch.Success)
            {
                string sectionContent = sectionMatch.Groups[1].Value;
                var paramPattern = @"<([A-Z])>([0-9]+)</[A-Z]>";
                var paramMatches = Regex.Matches(sectionContent, paramPattern);
                
                foreach (Match match in paramMatches)
                {
                    string paramName = match.Groups[1].Value;
                    if (int.TryParse(match.Groups[2].Value, out int paramValue))
                    {
                        parameters[paramName] = paramValue;
                    }
                }
            }
            
            return parameters;
        }
        
        /// <summary>
        /// Formats system file information for display
        /// </summary>
        /// <param name="sb">StringBuilder to append formatted information to</param>
        /// <param name="systemInfo">Dictionary containing system information</param>
        private void FormatSystemFileInfo(StringBuilder sb, Dictionary<string, object> systemInfo)
        {
            if (systemInfo.ContainsKey("Error"))
            {
                sb.AppendLine($"  Error: {systemInfo["Error"]}");
                return;
            }
            
            // Display count
            if (systemInfo.ContainsKey("Count"))
            {
                sb.AppendLine($"  Count: {systemInfo["Count"]}");
            }
            
            // Display high-level setup information
            if (systemInfo["Setup"] is Dictionary<string, int> setup && setup.Count > 0)
            {
                sb.AppendLine("  Setup Configuration:");
                sb.AppendLine($"    Global Settings: {setup.Count} parameters configured");
                if (setup.ContainsKey("A")) sb.AppendLine($"    Parameter A: {setup["A"]}");
                if (setup.ContainsKey("B")) sb.AppendLine($"    Parameter B: {setup["B"]}");
                if (setup.ContainsKey("C")) sb.AppendLine($"    Parameter C: {setup["C"]}");
            }
            
            // Display MIDI settings
            if (systemInfo["MIDI"] is Dictionary<string, int> midi && midi.Count > 0)
            {
                sb.AppendLine("  MIDI Configuration:");
                sb.AppendLine($"    MIDI Settings: {midi.Count} parameters configured");
                if (midi.ContainsKey("E")) sb.AppendLine($"    MIDI Channel: {midi["E"]}");
            }
            
            // Display USB settings
            if (systemInfo["USB"] is Dictionary<string, int> usb && usb.Count > 0)
            {
                sb.AppendLine("  USB Configuration:");
                sb.AppendLine($"    USB Settings: {usb.Count} parameters configured");
            }
            
            // Display input controls summary
            if (systemInfo["Input_Controls"] is Dictionary<string, object> inputControls)
            {
                int totalInputs = 0;
                foreach (var control in inputControls.Values)
                {
                    if (control is Dictionary<string, int> controlDict)
                        totalInputs += controlDict.Count;
                }
                if (totalInputs > 0)
                {
                    sb.AppendLine($"  Input Controls: {totalInputs} total parameters across ICTL1, ICTL2, ICTL3");
                }
            }
            
            // Display mixer and routing info
            if (systemInfo["Mixer"] is Dictionary<string, int> mixer && mixer.Count > 0)
            {
                sb.AppendLine($"  Mixer: {mixer.Count} parameters configured");
            }
            
            if (systemInfo["Routing"] is Dictionary<string, int> routing && routing.Count > 0)
            {
                sb.AppendLine($"  Routing: {routing.Count} parameters configured");
            }
            
            if (systemInfo["EQ"] is Dictionary<string, int> eq && eq.Count > 0)
            {
                sb.AppendLine($"  EQ: {eq.Count} parameters configured");
            }
        }
        
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

        /// <summary>
        /// Compares two count values treating them as hexadecimal numbers
        /// </summary>
        /// <param name="count1">First count value (hex string)</param>
        /// <param name="count2">Second count value (hex string)</param>
        /// <returns>True if count1 is greater than or equal to count2</returns>
        private bool IsCountHigherOrEqual(string? count1, string? count2)
        {
            try
            {
                int value1 = Convert.ToInt32(count1 ?? "001F", 16);
                int value2 = Convert.ToInt32(count2 ?? "001F", 16);
                return value1 >= value2;
            }
            catch (Exception)
            {
                // If conversion fails, fall back to string comparison
                return string.Compare(count1 ?? "001F", count2 ?? "001F", StringComparison.OrdinalIgnoreCase) >= 0;
            }
        }
    }
}