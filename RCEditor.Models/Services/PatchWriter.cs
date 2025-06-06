using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCEditor.Models.Services
{
    public class PatchWriter
    {
        /// <summary>
        /// Helper method to convert all CRLF line endings to LF only
        /// </summary>
        private string NormalizeLineEndings(string content)
        {
            return content.Replace("\r\n", "\n");
        }        /// <summary>
        /// Writes a MemoryPatch to a file in the RC0 format
        /// </summary>
        /// <param name="patch">The MemoryPatch to write</param>
        /// <param name="filePath">The path to save the file to</param>
        /// <returns>A task representing the async operation</returns>
        public async Task WritePatchAsync(MemoryPatch patch, string filePath)
        {
            // Use \n for line endings to match the original RC0 format
            const string newline = "\n";
            
            StringBuilder fileContent = new StringBuilder();
            // Add file header
            fileContent.Append(@"<?xml version=""1.0"" encoding=""utf-8""?>" + newline);
            fileContent.Append(@"<database name=""RC-600"" revision=""0"">" + newline);            // Write memory section
            fileContent.Append($@"<mem id=""{patch.Id}"">" + newline);
            // Convert CRLF to LF in memory section content
            string memorySection = NormalizeLineEndings(CreateMemorySection(patch));
            fileContent.Append(memorySection);


            
            fileContent.Append("</mem>" + newline);
            
            // Write input FX section
            fileContent.Append($@"<ifx id=""{patch.Id}"">" + newline);
            // Convert CRLF to LF in input FX section content
            string inputFXSection = NormalizeLineEndings(CreateInputFXSection(patch));
            fileContent.Append(inputFXSection);
            fileContent.Append("</ifx>" + newline);
              // Write track FX section
            fileContent.Append($@"<tfx id=""{patch.Id}"">" + newline);
            // Convert CRLF to LF in track FX section content
            string trackFXSection = NormalizeLineEndings(CreateTrackFXSection(patch));
            fileContent.Append(trackFXSection);
            fileContent.Append("</tfx>" + newline);            // Add file footer
            fileContent.Append("</database>" + newline);
            
            // Add the count field at the end using the patch's Count property
            fileContent.Append($"<count>{patch.Count}</count>");

            // Write the file
            await File.WriteAllTextAsync(filePath, fileContent.ToString(), Encoding.UTF8);
        }
        /// <summary>
        /// Creates the memory section of the RC0 file
        /// </summary>
        private string CreateMemorySection(MemoryPatch patch)
        {
            StringBuilder sb = new StringBuilder();

            // Create the NAME section
            sb.AppendLine("<NAME>");
            sb.Append(CreateNameSection(patch.Name));
            sb.AppendLine("</NAME>");

            // Create TRACK sections first (matches MEMORY001A.RC0 order)
            for (int i = 0; i < patch.Tracks.Length; i++)
            {
                sb.AppendLine($"<TRACK{i + 1}>");
                sb.Append(CreateTrackSection(patch.Tracks[i]));
                sb.AppendLine($"</TRACK{i + 1}>");
            }

            // Create the MASTER section (if present in your model)
            // This position is based on MEMORY001A.RC0
            if (patch.Master != null)
            {
                sb.AppendLine("<MASTER>");
                sb.Append(CreateMasterSection(patch.Master));
                sb.AppendLine("</MASTER>");
            }

            // Create the REC section
            sb.AppendLine("<REC>");
            sb.Append(CreateRecSection(patch.Rec));
            sb.AppendLine("</REC>");

            // Create the PLAY section
            sb.AppendLine("<PLAY>");
            sb.Append(CreatePlaySection(patch.Play));
            sb.AppendLine("</PLAY>");

            // Create the RHYTHM section
            sb.AppendLine("<RHYTHM>");
            sb.Append(CreateRhythmSection(patch.Rhythm));
            sb.AppendLine("</RHYTHM>");

            // Create ASSIGN sections
            foreach (var assign in patch.Assigns)
            {
                sb.AppendLine("<ASSIGN>");
                sb.Append(CreateAssignSection(assign));
                sb.AppendLine("</ASSIGN>");
            }

            // Write control settings section if available
            if (patch.ControlSettings != null)
            {
                // Convert CRLF to LF in control settings content
                sb.Append(NormalizeLineEndings(ControlSettingsParser.ConvertToXml(patch.ControlSettings)));
            }

            return sb.ToString();
        }
        /// <summary>
        /// Creates the NAME section with ASCII character parameters
        /// </summary>
        private string CreateNameSection(string name)
        {
            StringBuilder sb = new StringBuilder();

            // Ensure name is not longer than 12 characters
            string patchName = name ?? "NEW PATCH";
            if (patchName.Length > 12)
            {
                patchName = patchName.Substring(0, 12);
            }

            // Always create all parameters from A to L (12 characters total)
            for (int i = 0; i < 12; i++)
            {
                char letter = (char)('A' + i);

                // Use the actual character if available, otherwise use space (ASCII 32)
                int asciiValue = (i < patchName.Length) ? (int)patchName[i] : 32;
                sb.AppendLine($"\t<{letter}>{asciiValue}</{letter}>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Creates the MASTER section
        /// </summary>
        private string CreateMasterSection(MasterSettings master)
        {
            StringBuilder sb = new StringBuilder();

            // A -> Loop Position
            sb.AppendLine($"\t<A>{master.LoopPosition}</A>");

            // B -> Loop Length
            sb.AppendLine($"\t<B>{master.LoopLength}</B>");

            // C -> Mode Flag
            sb.AppendLine($"\t<C>{master.ModeFlag}</C>");

            // D -> Mode Value
            sb.AppendLine($"\t<D>{master.ModeValue}</D>");

            return sb.ToString();
        }

        /// <summary>
        /// Creates the PLAY section
        /// </summary>
        private string CreatePlaySection(PlaySettings play)
        {
            StringBuilder sb = new StringBuilder();

            // A -> Single Track Change
            sb.AppendLine($"\t<A>{(int)play.SingleTrackChange}</A>");

            // B -> Fade Time In
            sb.AppendLine($"\t<B>{play.FadeTimeIn}</B>");

            // C -> Fade Time Out
            sb.AppendLine($"\t<C>{play.FadeTimeOut}</C>");

            // D -> All Start Tracks (bit mask)
            int startTracksMask = 0;
            for (int i = 0; i < play.AllStartTracks.Length; i++)
            {
                if (play.AllStartTracks[i])
                {
                    startTracksMask |= (1 << i);
                }
            }
            sb.AppendLine($"\t<D>{startTracksMask}</D>");

            // E -> All Stop Tracks (bit mask)
            int stopTracksMask = 0;
            for (int i = 0; i < play.AllStopTracks.Length; i++)
            {
                if (play.AllStopTracks[i])
                {
                    stopTracksMask |= (1 << i);
                }
            }
            sb.AppendLine($"\t<E>{stopTracksMask}</E>");

            // F -> Loop Length
            sb.AppendLine($"\t<F>{play.LoopLength}</F>");            // G -> Speed Change
            sb.AppendLine($"\t<G>{(int)play.SpeedChange}</G>");

            // H -> Sync Adjust
            sb.AppendLine($"\t<H>{(int)play.SyncAdjust}</H>");

            // Note: 'I' (Tempo) is not saved in PlaySettings

            return sb.ToString();
        }

        /// <summary>
        /// Creates the REC section
        /// </summary>
        private string CreateRecSection(RecSettings rec)
        {
            StringBuilder sb = new StringBuilder();

            // A -> Rec Action
            sb.AppendLine($"\t<A>{(int)rec.RecAction}</A>");

            // B -> Quantize Enabled
            sb.AppendLine($"\t<B>{(rec.QuantizeEnabled ? 1 : 0)}</B>");

            // C -> Auto Rec Enabled
            sb.AppendLine($"\t<C>{(rec.AutoRecEnabled ? 1 : 0)}</C>");

            // D -> Auto Rec Sensitivity
            sb.AppendLine($"\t<D>{rec.AutoRecSensitivity}</D>");

            // E -> Bounce Enabled
            sb.AppendLine($"\t<E>{(rec.BounceEnabled ? 1 : 0)}</E>");

            // F -> Bounce Track
            sb.AppendLine($"\t<F>{rec.BounceTrack}</F>");

            return sb.ToString();
        }        /// <summary>
        /// Creates a TRACK section
        /// </summary>
        private string CreateTrackSection(Track track)
        {
            StringBuilder sb = new StringBuilder();

            // A -> Reverse
            sb.AppendLine($"\t<A>{(track.Reverse ? 1 : 0)}</A>");

            // B -> One Shot
            sb.AppendLine($"\t<B>{(track.OneShot ? 1 : 0)}</B>");

            // C -> Pan (convert from -50 to +50 range to 0-100)
            sb.AppendLine($"\t<C>{track.Pan + 50}</C>");

            // D -> Play Level
            sb.AppendLine($"\t<D>{track.Level}</D>");

            // E -> Start Mode
            sb.AppendLine($"\t<E>{(int)track.StartMode}</E>");

            // F -> Stop Mode
            sb.AppendLine($"\t<F>{(int)track.StopMode}</F>");

            // G -> Overdub Mode
            sb.AppendLine($"\t<G>{(int)track.OverdubMode}</G>");

            // H -> FX Enabled
            sb.AppendLine($"\t<H>{(track.FXEnabled ? 1 : 0)}</H>");

            // I -> Play Mode
            sb.AppendLine($"\t<I>{(int)track.PlayMode}</I>");

            // J -> Measure Count
            sb.AppendLine($"\t<J>{track.MeasureCount}</J>");

            // K -> Unknown parameter K
            sb.AppendLine($"\t<K>{track.UnknownK}</K>");

            // L -> Loop Sync Switch
            sb.AppendLine($"\t<L>{(track.LoopSyncSw ? 1 : 0)}</L>");

            // M -> Tempo Sync Switch
            sb.AppendLine($"\t<M>{(track.TempoSyncSw ? 1 : 0)}</M>");

            // N -> Tempo Sync Mode
            sb.AppendLine($"\t<N>{(int)track.TempoSyncMode}</N>");

            // O -> Tempo Sync Speed
            sb.AppendLine($"\t<O>{(int)track.TempoSyncSpeed}</O>");

            // P -> Unknown parameter P
            sb.AppendLine($"\t<P>{track.UnknownP}</P>");            // Q -> Input routing bitmask with bit assignments:
            // Bit 0 (0x01): MIC1
            // Bit 1 (0x02): MIC2
            // Bit 2 (0x04): Inst1Left (now Mic1Left)
            // Bit 3 (0x08): Inst1Right (now Mic1Right)
            // Bit 4 (0x10): Inst2Left (now Mic2Left)
            // Bit 5 (0x20): Inst2Right (now Mic2Right)
            // Bit 6 (0x40): RHYTHM
            int inputMask = 0x00; // No default bits set
            if (track.InputRouting.Mic1Enabled) inputMask |= 0x01;
            if (track.InputRouting.Mic2Enabled) inputMask |= 0x02;
            if (track.InputRouting.Mic1LeftEnabled) inputMask |= 0x04;
            if (track.InputRouting.Mic1RightEnabled) inputMask |= 0x08;
            if (track.InputRouting.Mic2LeftEnabled) inputMask |= 0x10;
            if (track.InputRouting.Mic2RightEnabled) inputMask |= 0x20;
            if (track.InputRouting.RhythmEnabled) inputMask |= 0x40;
            sb.AppendLine($"\t<Q>{inputMask}</Q>");
            
            // All input routing is controlled by the Q parameter - there is no Z tag

            // R -> Unknown parameter R
            sb.AppendLine($"\t<R>{track.UnknownR}</R>");

            // S -> MEASUREB
            sb.AppendLine($"\t<S>{track.MeasureCountB}</S>");
            
            // T -> Unknown parameter T
            sb.AppendLine($"\t<T>{track.UnknownT}</T>");
            
            // U -> Unknown parameter U
            sb.AppendLine($"\t<U>{track.UnknownU}</U>");
            
            // V -> Unknown parameter V
            sb.AppendLine($"\t<V>{track.UnknownV}</V>");
            
            // W -> BOUNCE IN
            sb.AppendLine($"\t<W>{(track.BounceIn ? 1 : 0)}</W>");
            
            // X -> Unknown parameter X
            sb.AppendLine($"\t<X>{track.UnknownX}</X>");
            
            // Y -> LOOP SYNC MODE
            sb.AppendLine($"\t<Y>{(int)track.LoopSyncMode}</Y>");

            return sb.ToString();
        }

        /// <summary>
        /// Creates the RHYTHM section
        /// </summary>        
        private string CreateRhythmSection(RhythmSettings rhythm)
        {
            StringBuilder sb = new StringBuilder();

            // A -> Genre
            int genreValue;
            if (!int.TryParse(rhythm.Genre, out genreValue))
            {
                genreValue = 0;
            }
            sb.AppendLine($"\t<A>{genreValue}</A>");

            // B -> Pattern
            int patternValue = rhythm.PatternId + 1; // Convert 0-based to 1-based
            sb.AppendLine($"\t<B>{patternValue}</B>");

            // C -> Variation (A=0, B=1, C=2, D=3)
            int variationValue = rhythm.Variation - 'A';
            sb.AppendLine($"\t<C>{variationValue}</C>");

            // D -> Variation Change Timing (according to updated RC600Param.md)
            sb.AppendLine($"\t<D>{(int)rhythm.VariationChangeTiming}</D>");

            // E -> Kit
            int kitValue;
            if (!int.TryParse(rhythm.Kit, out kitValue))
            {
                kitValue = 0;
            }
            sb.AppendLine($"\t<E>{kitValue}</E>");

            // F -> Beat
            int beatValue;
            if (!int.TryParse(rhythm.Beat, out beatValue))
            {
                beatValue = 0;
            }
            sb.AppendLine($"\t<F>{beatValue}</F>");

            // G -> Fill
            sb.AppendLine($"\t<G>{(rhythm.FillIn ? 1 : 0)}</G>");

            // H -> Intro on Rec
            sb.AppendLine($"\t<H>{(rhythm.IntroOnRec ? 1 : 0)}</H>");

            // I -> Intro on Play
            sb.AppendLine($"\t<I>{(rhythm.IntroOnPlay ? 1 : 0)}</I>");

            // J -> Ending
            sb.AppendLine($"\t<J>{(rhythm.Ending ? 1 : 0)}</J>");

            // K -> Stop Trigger Mode
            sb.AppendLine($"\t<K>{(int)rhythm.StopMode}</K>");

            // L -> Start Trigger Mode
            sb.AppendLine($"\t<L>{(int)rhythm.StartMode}</L>");
            
            // M -> Unknown parameter (preserving for future compatibility)
            sb.AppendLine($"\t<M>{rhythm.UnknownM}</M>");

            return sb.ToString();
        }

        /// <summary>
        /// Creates an ASSIGN section for a single assignment
        /// </summary>
        private string CreateAssignSection(AssignSlot assign)
        {
            StringBuilder sb = new StringBuilder();

            // Example parameters based on your model
            if (!string.IsNullOrEmpty(assign.Source))
            {
                int sourceId = GetControlSourceId(assign.Source);
                sb.AppendLine($"<SOURCE>{sourceId}</SOURCE>");
            }

            if (!string.IsNullOrEmpty(assign.Target))
            {
                int targetId = GetControlTargetId(assign.Target);
                sb.AppendLine($"<TARGET>{targetId}</TARGET>");
            }

            if (assign.RangeMin.HasValue)
            {
                sb.AppendLine($"<TARGET_MIN>{assign.RangeMin.Value}</TARGET_MIN>");
            }

            if (assign.RangeMax.HasValue)
            {
                sb.AppendLine($"<TARGET_MAX>{assign.RangeMax.Value}</TARGET_MAX>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Creates the Input FX section
        /// </summary>
        private string CreateInputFXSection(MemoryPatch patch)
        {
            return CreateFXSection(patch.InputFX);
        }

        /// <summary>
        /// Creates the Track FX section
        /// </summary>
        private string CreateTrackFXSection(MemoryPatch patch)
        {
            return CreateFXSection(patch.TrackFX);
        }        /// <summary>
        /// Creates an FX section (common code for both Input FX and Track FX)
        /// </summary>
        private string CreateFXSection(EffectBanks effectBanks)
        {
            StringBuilder sb = new StringBuilder();

            // Add SETUP tag with active bank setting
            sb.AppendLine("<SETUP>");
            int activeBankValue = GetBankValue(effectBanks.ActiveBank);
            sb.AppendLine($"\t<A>{activeBankValue}</A>");
            sb.AppendLine("</SETUP>");
            
            // // Process each bank (A, B, C, D)
            // ProcessBank(sb, "A", effectBanks.Banks["A"]);
            // ProcessBank(sb, "B", effectBanks.Banks["B"]);            ProcessBank(sb, "C", effectBanks.Banks["C"]);
            // ProcessBank(sb, "D", effectBanks.Banks["D"]);
            
            // Process each effect slot in each bank.  Each bank header, ie <A>, <B>, <C>, <D> is already at the beginning of the bank.
            foreach (var bankEntry in effectBanks.Banks)
            {
                string bankKey = bankEntry.Key;
                EffectBank bank = bankEntry.Value;
                char bankLetter = bankKey[0]; // A, B, C, or D

                ProcessBank(sb, bankLetter.ToString(), effectBanks.Banks[bankLetter.ToString()]);

                // Process each slot in the bank (1-4)
                for (int slotIndex = 1; slotIndex <= 4; slotIndex++)
                {
                    var slot = bank.Slots[slotIndex];
                    if (slot == null) continue;

                    char slotLetter = (char)('A' + slotIndex - 1); // A, B, C, D for slots 1-4
                    string slotKey = $"{bankLetter}{slotLetter}";

                    // Add slot tag
                    sb.AppendLine($"<{slotKey}>");
                    // append each slot parameter from slot.Parameters
                    foreach (var param in slot.Parameters)
                    {
                        sb.AppendLine($"\t<{param.Key}>{param.Value}</{param.Key}>");
                    }

                    sb.AppendLine($"</{slotKey}>");

                    // Get array of effect IDs in correct enumeration order
                    int[] effectOrder = GetEffectsInEnumerationOrder();

                    // Process all effects in the enumeration order
                    foreach (int effectType in effectOrder)
                    {
                        // Check if this effect exists in AllEffectSettings
                        if (slot.AllEffectSettings.TryGetValue(effectType, out var effectSettings))
                        {
                            // // If this is the currently active effect, merge any new parameters from the current slot parameters
                            // // that might not yet exist in the stored effect settings
                            // if (effectType == slot.EffectType)
                            // {
                            //     // Create a copy of parameters to avoid modifying the original
                            //     Dictionary<string, int> mergedParams = new Dictionary<string, int>(effectSettings.Parameters);

                            //     // Add any additional parameters from the current slot
                            //     foreach (var param in slot.Parameters)
                            //     {
                            //         if (!mergedParams.ContainsKey(param.Key))
                            //         {
                            //             mergedParams[param.Key] = param.Value;
                            //         }
                            //     }

                            //     // Write effect with the merged parameters
                            //     WriteEffectData(sb, slotKey, effectType, effectSettings.EffectName, mergedParams);
                            // }
                            // else
                            // {
                                // Write this saved effect
                                WriteEffectData(sb, slotKey, effectType, effectSettings.EffectName, effectSettings.Parameters);
                            // }
                        }

                        // Handle sequence data for the effect
                        WriteEffectSequenceData(sb, slotKey, effectType, slot);
                    }
                }
            }

            return sb.ToString();
        }        /// <summary>
        /// Write effect parameters to the string builder
        /// </summary>
        private void WriteEffectData(StringBuilder sb, string slotKey, int effectType, string effectName, Dictionary<string, int> parameters)
        {
            string effectTypeMap = MapEffectTypeToOriginalName(effectType, effectName);
            string effectKey = $"{slotKey}_{effectTypeMap}";

            sb.AppendLine($"<{effectKey}>");            // Add effect parameters with alphabetical letter parameters
            // RC0 format uses A, B, C, etc. as parameter names
            var sortedParams = new SortedDictionary<string, int>();
            
            // // First ensure all standard letter parameters are initialized with default values
            // for (char c = 'A'; c <= 'Z'; c++)
            // {
            //     sortedParams[c.ToString()] = 0; // Default value 0
            // }
            
            // Now override with any provided parameters
            foreach (var param in parameters)
            {
                // Skip sequence parameters, they will be handled separately
                if (param.Key.StartsWith("SEQ_")) continue;
                if (param.Key.Length == 1 && char.IsLetter(param.Key[0]))
                {
                    sortedParams[param.Key] = param.Value;
                }
            }

            // Add generic alphabetical parameters (A-Z)
            foreach (var param in sortedParams)
            {
                sb.AppendLine($"\t<{param.Key}>{param.Value}</{param.Key}>");
            }

            // Add any additional non-alphabetical parameters if they exist
            // This allows for adding extra parameters beyond the A-Z naming convention
            foreach (var param in parameters)
            {
                if (param.Key.StartsWith("SEQ_")) continue;
                if (!(param.Key.Length == 1 && char.IsLetter(param.Key[0])))
                {
                    sb.AppendLine($"\t<{param.Key}>{param.Value}</{param.Key}>");
                }
            }

            sb.AppendLine($"</{effectKey}>");
        }        /// <summary>
        /// Write sequence data for effects that support sequences
        /// </summary>
        private void WriteEffectSequenceData(StringBuilder sb, string slotKey, int effectType, EffectSlot slot)
        {
            // Check if this effect supports sequence data
            if (!EffectSupportsSequence(effectType))
            {
                return; // Skip sequence data for effects that don't support it
            }
            
            // Create a dictionary for parameters
            Dictionary<string, int> seqParams = new Dictionary<string, int>();
            
            // If this is the active effect, copy its sequence parameters if any
            if (effectType == slot.EffectType)
            {
                foreach (var param in slot.Parameters)
                {
                    if (param.Key.StartsWith("SEQ_"))
                    {
                        seqParams[param.Key] = param.Value;
                    }
                }
            }
            // If this effect is in AllEffectSettings, copy any sequence parameters
            else if (slot.AllEffectSettings.TryGetValue(effectType, out var effectSettings))
            {
                foreach (var param in effectSettings.Parameters)
                {
                    if (param.Key.StartsWith("SEQ_"))
                    {
                        seqParams[param.Key] = param.Value;
                    }
                }
            }
            
            string effectName = MapEffectTypeToOriginalName(effectType, 
                effectType == slot.EffectType ? slot.EffectName : $"EFFECT_{effectType}");
            string seqKey = $"{slotKey}_{effectName}_SEQ";
            sb.AppendLine($"<{seqKey}>");

            // Sequential parameter mapping with standard letter parameters (A-V)
            if (seqParams.ContainsKey("SEQ_SW"))
                sb.AppendLine($"\t<A>{seqParams["SEQ_SW"]}</A>");
            else
                sb.AppendLine("\t<A>0</A>");

            if (seqParams.ContainsKey("SEQ_SYNC"))
                sb.AppendLine($"\t<B>{seqParams["SEQ_SYNC"]}</B>");
            else
                sb.AppendLine("\t<B>0</B>");

            if (seqParams.ContainsKey("SEQ_RETRIG"))
                sb.AppendLine($"\t<C>{seqParams["SEQ_RETRIG"]}</C>");
            else
                sb.AppendLine("\t<C>0</C>");

            if (seqParams.ContainsKey("SEQ_TARGET"))
                sb.AppendLine($"\t<D>{seqParams["SEQ_TARGET"]}</D>");
            else
                sb.AppendLine("\t<D>0</D>");

            if (seqParams.ContainsKey("SEQ_RATE"))
                sb.AppendLine($"\t<E>{seqParams["SEQ_RATE"]}</E>");
            else
                sb.AppendLine("\t<E>6</E>");  // Default rate value

            if (seqParams.ContainsKey("SEQ_MAX"))
                sb.AppendLine($"\t<F>{seqParams["SEQ_MAX"]}</F>");
            else
                sb.AppendLine("\t<F>15</F>"); // Default max value            // Add step values SEQ_VAL1 through SEQ_VAL16 (mapped to G through V)
            char valLetter = 'G';
            for (int i = 1; i <= 16; i++)
            {
                string paramKey = $"SEQ_VAL{i}";
                if (seqParams.ContainsKey(paramKey))
                {
                    sb.AppendLine($"\t<{valLetter}>{seqParams[paramKey]}</{valLetter}>");
                }
                else
                {
                    sb.AppendLine($"\t<{valLetter}>0</{valLetter}>");
                }
                valLetter++;
            }
            
            // Add any additional custom sequence parameters
            // This allows for adding extra sequence parameters beyond the standard ones
            foreach (var param in seqParams)
            {
                // Skip parameters we've already handled
                if (param.Key == "SEQ_SW" || param.Key == "SEQ_SYNC" || param.Key == "SEQ_RETRIG" ||
                    param.Key == "SEQ_TARGET" || param.Key == "SEQ_RATE" || param.Key == "SEQ_MAX" ||
                    (param.Key.StartsWith("SEQ_VAL") && int.TryParse(param.Key.Substring(7), out int index) && index >= 1 && index <= 16))
                {
                    continue;
                }
                
                // Add any other sequence parameters
                sb.AppendLine($"\t<{param.Key}>{param.Value}</{param.Key}>");
            }

            sb.AppendLine($"</{seqKey}>");
        }

        /// <summary>
        /// Process a single effect bank (A, B, C, D)
        /// </summary>
        private void ProcessBank(StringBuilder sb, string bankTag, EffectBank bank)
        {
            sb.AppendLine($"<{bankTag}>");
            sb.AppendLine($"\t<A>{(bank.Enabled ? 1 : 0)}</A>"); // Bank enable state

            // Always include B parameter (sw mode or 0 in original RC0 files)
            if (bank.Parameters.ContainsKey("B"))
            {
                sb.AppendLine($"\t<B>{bank.Parameters["B"]}</B>");
            }
            else
            {
                sb.AppendLine("\t<B>0</B>");
            }

            // Always include C parameter (target track or 3 in original RC0 files)
            if (bank.Parameters.ContainsKey("C"))
            {
                sb.AppendLine($"\t<C>{bank.Parameters["C"]}</C>");
            }
            else
            {
                sb.AppendLine("\t<C>3</C>"); // Default target value often seen in original files
            }

            sb.AppendLine($"</{bankTag}>");
        }

        /// <summary>
        /// Convert BankType enum to numeric value
        /// </summary>
        private int GetBankValue(EffectSlot.BankType bankType)
        {
            switch (bankType)
            {
                case EffectSlot.BankType.A: return 0;
                case EffectSlot.BankType.B: return 1;
                case EffectSlot.BankType.C: return 2;
                case EffectSlot.BankType.D: return 3;
                default: return 0; // Default to A
            }
        }

        /// <summary>
        /// Convert control source name to ID (inverse of GetControlSourceName in PatchReader)
        /// </summary>
        private int GetControlSourceId(string sourceName)
        {
            switch (sourceName)
            {
                case "CTL1": return 0;
                case "CTL2": return 1;
                case "CTL3": return 2;
                case "CTL4": return 3;
                case "EXP1": return 4;
                case "EXP2": return 5;
                default:
                    // Try to parse SOURCE_X format
                    if (sourceName.StartsWith("SOURCE_") && int.TryParse(sourceName.Substring(7), out int id))
                    {
                        return id;
                    }
                    return 0;
            }
        }

        /// <summary>
        /// Convert control target name to ID (inverse of GetControlTargetName in PatchReader)
        /// </summary>
        private int GetControlTargetId(string targetName)
        {
            switch (targetName)
            {
                case "TRACK1_LEVEL": return 0;
                case "TRACK2_LEVEL": return 1;
                case "TRACK3_LEVEL": return 2;
                case "TRACK4_LEVEL": return 3;
                case "TRACK5_LEVEL": return 4;
                case "TRACK6_LEVEL": return 5;
                case "RHYTHM_LEVEL": return 10;
                case "MASTER_LEVEL": return 20;
                default:
                    // Try to parse TARGET_X format
                    if (targetName.StartsWith("TARGET_") && int.TryParse(targetName.Substring(7), out int id))
                    {
                        return id;
                    }
                    return 0;
            }
        }        /// <summary>
        /// Maps an effect type ID and name to the original RC0 effect name format
        /// </summary>
        private string MapEffectTypeToOriginalName(int effectTypeId, string effectName)
        {
            // Use the centralized mapping class
            return EffectMappings.MapEffectNameForRC0Format(effectTypeId, effectName);
        }        /// <summary>
        /// Returns array of effect IDs in their standard enumeration order as defined in RC600Param.md
        /// </summary>
        /// <returns>Array of effect IDs in enumeration order</returns>
        private int[] GetEffectsInEnumerationOrder()
        {
            // Use the centralized mapping class
            return EffectMappings.GetEffectsInEnumerationOrder();
        }

        /// <summary>
        /// Determines if an effect type supports sequence based on RC600Param.md documentation
        /// </summary>
        /// <param name="effectType">The effect type ID to check</param>
        /// <returns>True if the effect supports sequence, false otherwise</returns>
        private bool EffectSupportsSequence(int effectType)
        {
            // Use the centralized mapping class
            return EffectMappings.EffectSupportsSequence(effectType);
        }
    }
}
