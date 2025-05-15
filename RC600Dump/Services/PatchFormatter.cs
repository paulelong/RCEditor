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
        /// Gets a friendly name for the rhythm genre based on its numeric value
        /// </summary>
        private string GetGenreName(int genreValue)
        {
            // Genre names from RC-600 documentation
            return genreValue switch
            {
                0 => "ACOUSTIC",
                1 => "BALLAD",
                2 => "BLUES",
                3 => "JAZZ",
                4 => "FUSION",
                5 => "R&B",
                6 => "SOUL",
                7 => "FUNK",
                8 => "POP",
                9 => "SOFT ROCK",
                10 => "ROCK",
                11 => "ALT ROCK",
                12 => "PUNK",
                13 => "HEAVY ROCK",
                14 => "METAL",
                15 => "TRAD",
                16 => "WORLD",
                17 => "BALLROOM",
                18 => "ELECTRO",
                19 => "GUIDE",
                20 => "USER",
                _ => $"Unknown ({genreValue})"
            };
        }

        /// <summary>
        /// Gets a pattern name for a specific genre and pattern number
        /// </summary>
        private string GetPatternNameByGenre(string genre, int patternNumber)
        {
            // Return the pattern name based on genre and pattern number (0-based)
            switch (genre)
            {
                case "ACOUSTIC":
                    return patternNumber switch
                    {
                        0 => "SIDE STICK",
                        1 => "BOSSA",
                        2 => "BRUSH1",
                        3 => "BRUSH2",
                        4 => "CONGA 8BEAT",
                        5 => "CONGA 16BEAT",
                        6 => "CONGA 4BEAT",
                        7 => "CONGA SWING",
                        8 => "CONGA BOSSA",
                        9 => "CAJON1",
                        10 => "CAJON2",
                        _ => $"Pattern {patternNumber+1}"
                    };
                case "BALLAD":
                    return patternNumber switch
                    {
                        0 => "SHUFFLE2",
                        1 => "SIDE STICK1",
                        2 => "SIDE STICK2", 
                        3 => "SIDE STICK3",
                        4 => "SIDE STICK4",
                        5 => "SHUFFLE1",
                        6 => "8BEAT",
                        7 => "16BEAT1",
                        8 => "16BEAT2",
                        9 => "SWING",
                        10 => "6/8 BEAT",
                        _ => $"Pattern {patternNumber+1}"
                    };
                case "BLUES":
                    return patternNumber switch
                    {
                        0 => "3BEAT",
                        1 => "12BARS",
                        2 => "SHUFFLE1",
                        3 => "SHUFFLE2",
                        4 => "SWING",
                        5 => "6/8 BEAT",
                        _ => $"Pattern {patternNumber+1}"
                    };
                case "JAZZ":
                    return patternNumber switch
                    {
                        0 => "JAZZ BLUES",
                        1 => "FAST 4BEAT",
                        2 => "HARD BOP",
                        3 => "BRUSH BOP",
                        4 => "BRUSH SWING",
                        5 => "FAST SWNG",
                        6 => "MED SWING",
                        7 => "SLOW LEGATO",
                        8 => "JAZZ SAMBA",
                        9 => "6/8 BEAT",
                        _ => $"Pattern {patternNumber+1}"
                    };
                case "FUSION":
                    return patternNumber switch
                    {
                        0 => "16BEAT1",
                        1 => "16BEAT2",
                        2 => "16BEAT3",
                        3 => "16BEAT4",
                        4 => "16BEAT5",
                        5 => "16BEAT6",
                        6 => "16BEAT7",
                        7 => "SWING",
                        8 => "7/8 BEAT",
                        _ => $"Pattern {patternNumber+1}"
                    };
                case "R&B":
                    return patternNumber switch
                    {
                        0 => "SWING1",
                        1 => "SWING2",
                        2 => "SWING3",
                        3 => "SIDE STICK1",
                        4 => "SIDE STICK2",
                        5 => "SIDE STICK3",
                        6 => "SHUFFLE1",
                        7 => "SHUFFLE2",
                        8 => "8BEAT1",
                        9 => "16BEAT",
                        10 => "7/8 BEAT",
                        _ => $"Pattern {patternNumber+1}"
                    };
                case "SOUL":
                    return patternNumber switch
                    {
                        0 => "SWING1",
                        1 => "SWING2",
                        2 => "SWING3",
                        3 => "SWING4",
                        4 => "16BEAT1",
                        5 => "16BEAT2",
                        6 => "16BEAT3",
                        7 => "SIDESTK1",
                        8 => "SIDESTK2",
                        9 => "MOTOWN",
                        10 => "PERCUS",
                        _ => $"Pattern {patternNumber+1}"
                    };
                case "FUNK":
                    return patternNumber switch
                    {
                        0 => "8BEAT1",
                        1 => "8BEAT2",
                        2 => "8BEAT3",
                        3 => "8BEAT4",
                        4 => "16BEAT1",
                        5 => "16BEAT2",
                        6 => "16BEAT3",
                        7 => "16BEAT4",
                        8 => "SWING1",
                        9 => "SWING2",
                        10 => "SWING3",
                        _ => $"Pattern {patternNumber+1}"
                    };
                case "POP":
                    return patternNumber switch
                    {
                        0 => "8BEAT1",
                        1 => "8BEAT2",
                        2 => "16BEAT1",
                        3 => "16BEAT2",
                        4 => "PERCUS1",
                        5 => "SHUFFLE1",
                        6 => "SHUFFLE2",
                        7 => "SIDE STICK1",
                        8 => "SIDE STICK2",
                        9 => "SWING1",
                        10 => "SWING2",
                        11 => "PERCUS2",
                        _ => $"Pattern {patternNumber+1}"
                    };
                case "SOFT ROCK":
                    return patternNumber switch
                    {
                        0 => "16BEAT1",
                        1 => "16BEAT2",
                        2 => "16BEAT3",
                        3 => "16BEAT4",
                        4 => "8BEAT",
                        5 => "SWING1",
                        6 => "SWING2",
                        7 => "SWING3",
                        8 => "SWING4",
                        9 => "SIDE STICK1",
                        10 => "SIDE STICK2",
                        11 => "PERCUS1",
                        12 => "PERCUS2",
                        _ => $"Pattern {patternNumber+1}"
                    };
                case "ROCK":
                    return patternNumber switch
                    {
                        0 => "8BEAT1",
                        1 => "8BEAT2",
                        2 => "8BEAT3",
                        3 => "8BEAT4",
                        4 => "8BEAT5",
                        5 => "8BEAT6",
                        6 => "16BEAT1",
                        7 => "16BEAT2",
                        8 => "16BEAT3",
                        9 => "16BEAT4",
                        10 => "SHUFFLE1",
                        11 => "SHUFFLE2",
                        12 => "SWING1",
                        13 => "SWING2",
                        14 => "SWING3",
                        15 => "SWING4",
                        _ => $"Pattern {patternNumber+1}"
                    };
                case "ALT ROCK":
                    return patternNumber switch
                    {
                        0 => "RIDEBEAT",
                        1 => "8BEAT1",
                        2 => "8BEAT2",
                        3 => "8BEAT3",
                        4 => "8BEAT4",
                        5 => "16BEAT1",
                        6 => "16BEAT2",
                        7 => "16BEAT3",
                        8 => "16BEAT4",
                        9 => "SWING",
                        10 => "5/4 BEAT",
                        _ => $"Pattern {patternNumber+1}"
                    };
                case "PUNK":
                    return patternNumber switch
                    {
                        0 => "8BEAT1",
                        1 => "8BEAT2",
                        2 => "8BEAT3",
                        3 => "8BEAT4",
                        4 => "8BEAT5",
                        5 => "8BEAT6",
                        6 => "16BEAT1",
                        7 => "16BEAT2",
                        8 => "16BEAT3",
                        9 => "SIDE STICK",
                        _ => $"Pattern {patternNumber+1}"
                    };
                case "HEAVY ROCK":
                    return patternNumber switch
                    {
                        0 => "8BEAT1",
                        1 => "8BEAT2",
                        2 => "8BEAT3",
                        3 => "16BEAT1",
                        4 => "16BEAT2",
                        5 => "16BEAT3",
                        6 => "SHUFFLE1",
                        7 => "SHUFFLE2",
                        8 => "SWING1",
                        9 => "SWING2",
                        10 => "SWING3",
                        _ => $"Pattern {patternNumber+1}"
                    };
                case "METAL":
                    return patternNumber switch
                    {
                        0 => "8BEAT1",
                        1 => "8BEAT2",
                        2 => "8BEAT3",
                        3 => "8BEAT4",
                        4 => "8BEAT5",
                        5 => "8BEAT6",
                        6 => "2XBD1",
                        7 => "2XBD2",
                        8 => "2XBD3",
                        9 => "2XBD4",
                        10 => "2XBD5",
                        _ => $"Pattern {patternNumber+1}"
                    };
                case "TRAD":
                    return patternNumber switch
                    {
                        0 => "TRAIN2",
                        1 => "ROCKN ROLL",
                        2 => "TRAIN1",
                        3 => "COUNTRY1",
                        4 => "COUNTRY2",
                        5 => "COUNTRY3",
                        6 => "FOXTROT",
                        7 => "TRAD1",
                        8 => "TRAD2",
                        _ => $"Pattern {patternNumber+1}"
                    };
                case "WORLD":
                    return patternNumber switch
                    {
                        0 => "BOSSA1",
                        1 => "BOSSA2",
                        2 => "SAMBA1",
                        3 => "SAMBA2",
                        4 => "BOOGALOO",
                        5 => "MERENGUE",
                        6 => "REGGAE",
                        7 => "LATIN ROCK1",
                        8 => "LATIN ROCK2",
                        9 => "LATIN PERC",
                        10 => "SURDO",
                        11 => "LATIN1",
                        12 => "LATIN2",
                        _ => $"Pattern {patternNumber+1}"
                    };
                case "BALLROOM":
                    return patternNumber switch
                    {
                        0 => "CUMBIA",
                        1 => "WALTZ1",
                        2 => "WALTZ2",
                        3 => "CHACHA",
                        4 => "BEGUINE",
                        5 => "RHUMBA",
                        6 => "TANGO1",
                        7 => "TANGO2",
                        8 => "JIVE",
                        9 => "CHARLSTON",
                        _ => $"Pattern {patternNumber+1}"
                    };
                case "ELECTRO":
                    return patternNumber switch
                    {
                        0 => "ELCTRO01",
                        1 => "ELCTRO02",
                        2 => "ELCTRO03",
                        3 => "ELCTRO04",
                        4 => "ELCTRO05",
                        5 => "ELCTRO06",
                        6 => "ELCTRO07",
                        7 => "ELCTRO08",
                        8 => "5/4 BEAT",
                        _ => $"Pattern {patternNumber+1}"
                    };
                case "GUIDE":
                    return patternNumber switch
                    {
                        0 => "2/4 TRIPLE",
                        1 => "3/4",
                        2 => "3/4 TRIPLE",
                        3 => "4/4",
                        4 => "4/4 TRIPLE",
                        5 => "BD 8BEAT",
                        6 => "BD 16BEAT",
                        7 => "BD SHUFFLE",
                        8 => "HH 8BEAT",
                        9 => "HH 16BEAT",
                        10 => "HH SWING2",
                        11 => "8BEAT1",
                        12 => "8BEAT2",
                        13 => "8BEAT3",
                        14 => "8BEAT4",
                        15 => "5/4",
                        16 => "5/4 TRIPLE",
                        17 => "6/4",
                        18 => "6/4 TRIPLE",
                        19 => "7/4",
                        20 => "7/4 TRIPLE",
                        _ => $"Pattern {patternNumber+1}"
                    };
                case "USER":
                    return patternNumber switch
                    {
                        0 => "SIMPLE BEAT",
                        _ => $"USER {patternNumber+1}"
                    };
                default:
                    return $"Pattern {patternNumber+1}";
            }
        }
        
        /// <summary>
        /// Gets a description for the specified variation based on genre
        /// </summary>
        private string GetVariationDescription(char variation)
        {
            // Variations are generally similar across genres (A is basic, B-D are more complex)
            return variation switch
            {
                'A' => "Basic",
                'B' => "Variation with fills",
                'C' => "More complex variation",
                'D' => "Most complex variation",
                _ => $"Variation {variation}"
            };
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
            
            // Patch header
            sb.AppendLine($"Patch {patchNumber:D2}{variation} - \"{patch.Name}\" (Full Dump):");
            sb.AppendLine();
            
            // Basic patch info
            sb.AppendLine($"Name: {patch.Name}");
            
            // Play mode settings
            // string playMode = patch.PlayMode == PlayModeEnum.Multi ? "MULTI" : "SINGLE";
            
            // sb.AppendLine($"PlayMode: {playMode}");
            
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
            string genreName = GetGenreName(Convert.ToInt32(patch.Rhythm.Genre));
            sb.AppendLine($"  Genre: {genreName}");
            
            // Get the pattern name using the stored PatternId (0-based)
            string patternName = GetPatternNameByGenre(genreName, patch.Rhythm.PatternId);
            sb.AppendLine($"  Pattern: {patch.Rhythm.Pattern} ({patternName})");
            sb.AppendLine($"  Variation: {patch.Rhythm.Variation} ({GetVariationDescription(patch.Rhythm.Variation)})");
            sb.AppendLine($"  Kit: {patch.Rhythm.Kit}");
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
                    sb.AppendLine($"  Loop Sync: {(track.LoopSyncSw ? "ON" : "OFF")}");
                    sb.AppendLine($"  Loop Sync Mode: {track.LoopSyncMode}");
                    sb.AppendLine($"  Tempo Sync: {(track.TempoSyncSw ? "ON" : "OFF")}");
                    sb.AppendLine($"  Tempo Sync Mode: {track.TempoSyncMode}");
                    sb.AppendLine($"  Tempo Sync Speed: {track.TempoSyncSpeed}");
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