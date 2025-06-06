using System;
using System.Collections.Generic;

namespace RCEditor.Models
{
    /// <summary>
    /// Centralized mapping class for RC-600 effect names and IDs
    /// </summary>
    public static class EffectMappings
    {
        /// <summary>
        /// Maps effect type ID to effect name using RC600Param.md specifications
        /// </summary>
        /// <param name="effectTypeId">The effect type ID to get the name for</param>
        /// <returns>The standardized effect name</returns>
        public static string GetEffectNameFromId(int effectTypeId)
        {
            switch (effectTypeId)
            {
                case 0: return "THRU";

                // Filter effects
                case 1: return "LPF";
                case 2: return "BPF";
                case 3: return "HPF";

                // Modulation effects
                case 4: return "PHASER";
                case 5: return "FLANGER";
                case 48: return "CHORUS";
                case 32: return "TREMOLO";
                case 29: return "AUTO_PAN";
                case 30: return "MANUAL_PAN";
                case 33: return "VIBRATO";

                // Character effects
                case 6: return "SYNTH";
                case 7: return "LOFI";
                case 8: return "RADIO";
                case 9: return "RING_MODULATOR";
                case 31: return "STEREO_ENHANCE";
                case 24: return "DIST";
                case 23: return "PREAMP";

                // Pitch/Utility effects
                case 10: return "G2B";
                case 11: return "SUSTAINER";
                case 12: return "AUTO_RIFF";
                case 13: return "SLOW_GEAR";
                case 14: return "TRANSPOSE";
                case 15: return "PITCH_BEND";
                case 16: return "ROBOT";
                case 17: return "ELECTRIC";
                case 18: return "HARMONIST_MANUAL";
                case 19: return "HARMONIST_AUTO";
                case 20: return "VOCODER";
                case 28: return "OCTAVE";

                // Vocal/Oscillator effects
                case 21: return "OSC_VOCODER";
                case 22: return "OSC_BOT";
                
                // Dynamics/EQ effects
                case 25: return "DYNAMICS";
                case 26: return "EQ";
                case 27: return "ISOLATOR";

                // Sequence/DJ effects
                case 34: return "PATTERN_SLICER";
                case 35: return "STEP_SLICER";
                case 43: return "WARP";
                case 44: return "TWIST";
                case 45: return "ROLL";
                case 46: return "ROLL_V505V2";
                case 47: return "FREEZE";

                // Delay effects
                case 36: return "DELAY";
                case 37: return "PANNING_DELAY";
                case 38: return "REVERSE_DELAY";
                case 39: return "MOD_DELAY";
                case 40: return "TAPE_ECHO";
                case 41: return "TAPE_ECHO_V505V2";
                case 42: return "GRANULAR_DELAY";

                // Reverb effects
                case 49: return "REVERB";
                case 50: return "GATE_REVERB";
                case 51: return "REVERSE_REVERB";

                // Track-only effects
                case 52: return "BEAT_SCATTER";
                case 53: return "BEAT_REPEAT";
                case 54: return "BEAT_SHIFT";
                case 55: return "VINYL_FLICK";

                default: return $"EFFECT_{effectTypeId}";
            }
        }        /// <summary>
        /// Maps effect name to effect type ID using RC600Param.md specifications
        /// </summary>
        /// <param name="effectName">The effect name to get the ID for</param>
        /// <returns>The effect type ID, or -1 if not found</returns>
        public static int GetEffectIdFromName(string effectName)
        {
            switch (effectName?.ToUpper())
            {
                case "THRU": return 0;

                // Filter effects (1-3)
                case "LPF": return 1;
                case "BPF": return 2;
                case "HPF": return 3;

                // Modulation effects (4-5)
                case "PHASER": return 4;
                case "FLANGER": return 5;

                // Character effects (6-9)
                case "SYNTH": return 6;
                case "LOFI":
                case "LO-FI": return 7;
                case "RADIO": return 8;
                case "RING_MODULATOR":
                case "RING_MOD": return 9;

                // Pitch/Utility effects (10-20)
                case "G2B": return 10;
                case "SUSTAINER": return 11;
                case "AUTO_RIFF": return 12;
                case "SLOW_GEAR": return 13;
                case "TRANSPOSE": return 14;
                case "PITCH_BEND": return 15;
                case "ROBOT": return 16;
                case "ELECTRIC": return 17;
                case "HARMONIST_MANUAL": return 18;
                case "HARMONIST_AUTO": return 19;
                case "VOCODER": return 20;

                // Vocal/Oscillator effects (21-22)
                case "OSC_VOCODER":
                case "OSC_VOC_MIDI": return 21;
                case "OSC_BOT": return 22;

                // Character effects (23-24)
                case "PREAMP": return 23;
                case "DIST":
                case "DISTORTION":
                case "OVERDRIVE": // Legacy name to support old RC0 files
                case "FUZZ": return 24;      // Legacy name to support old RC0 files

                // Dynamics/EQ effects (25-27)
                case "DYNAMICS": return 25;
                case "EQ":
                case "4BAND_EQ": return 26;
                case "ISOLATOR": return 27;

                // Pitch/Utility continued (28)
                case "OCTAVE": return 28;

                // Modulation effects (29-33)
                case "AUTO_PAN": return 29;
                case "MANUAL_PAN": return 30;
                case "STEREO_ENHANCE": return 31;
                case "TREMOLO": return 32;
                case "VIBRATO": return 33;

                // Sequence/DJ effects (34-35)
                case "PATTERN_SLICER": return 34;
                case "STEP_SLICER": return 35;

                // Delay effects (36-42)
                case "DELAY": return 36;
                case "PANNING_DELAY": return 37;
                case "REVERSE_DELAY": return 38;
                case "MOD_DELAY": return 39;
                case "TAPE_ECHO":
                case "TAPE_ECHO_1": return 40;
                case "TAPE_ECHO_V505V2":
                case "TAPE_ECHO_2": return 41;
                case "GRANULAR_DELAY": return 42;

                // Sequence/DJ effects (43-47)
                case "WARP": return 43;
                case "TWIST": return 44;
                case "ROLL":
                case "ROLL_1": return 45;
                case "ROLL_V505V2": return 46;
                case "FREEZE": return 47;

                // Modulation effects (48)
                case "CHORUS": return 48;

                // Reverb effects (49-51)
                case "REVERB": return 49;
                case "GATE_REVERB": return 50;
                case "REVERSE_REVERB": return 51;
                
                // Track-only effects (52-55)
                case "BEAT_SCATTER": return 52; // Legacy mapping from older code
                case "BEAT_REPEAT":
                case "DEFRETTER": return 53;   // Legacy mapping from older code
                case "BEAT_SHIFT":
                case "PEDAL_BEND": return 54;  // Legacy mapping from older code
                case "VINYL_FLICK":
                case "RESONATOR": return 55;   // Legacy mapping from older code

                default:
                    // Try to parse EFFECT_X format
                    if (effectName != null && effectName.StartsWith("EFFECT_") && 
                        int.TryParse(effectName.Substring(7), out int id))
                    {
                        return id;
                    }
                    return -1; // Unknown effect
            }
        }

        /// <summary>
        /// Maps an effect type ID to its original RC0 format name for file writing
        /// </summary>
        /// <param name="effectTypeId">The effect type ID</param>
        /// <param name="fallbackName">Fallback name to use if the ID is not recognized</param>
        /// <returns>The name in RC0 format</returns>
        public static string MapEffectNameForRC0Format(int effectTypeId, string fallbackName)
        {
            // First get the standard name
            string effectName = GetEffectNameFromId(effectTypeId);
            
            // If it's a default EFFECT_X name, try to use the fallback
            if (effectName.StartsWith("EFFECT_") && !string.IsNullOrEmpty(fallbackName))
            {
                // Normalize the fallback name to match RC0 format
                return fallbackName.Replace(" ", "_").Replace("-", "_").ToUpper();
            }
            
            return effectName;
        }

        /// <summary>
        /// Determines if an effect type supports sequence based on RC600Param.md documentation
        /// </summary>
        /// <param name="effectType">The effect type ID to check</param>
        /// <returns>True if the effect supports sequence, false otherwise</returns>
        public static bool EffectSupportsSequence(int effectType)
        {
            // Based on the "Seq" column in RC600Param.md effect enumeration table
            switch (effectType)
            {
                case 1:  // LPF (Low-pass filter)
                case 2:  // BPF (Band-pass filter)
                case 3:  // HPF (High-pass filter)
                case 4:  // Phaser
                case 5:  // Flanger
                case 6:  // Synth
                case 9:  // Ring Mod
                case 14: // Transpose
                case 15: // Pitch Bend
                case 22: // OSC Bot
                case 27: // Isolator
                case 28: // Octave
                case 30: // Manual Pan
                case 32: // Tremolo
                case 33: // Vibrato
                    return true;
                default:
                    return false;
            }
        }
        
        /// <summary>
        /// Returns array of effect IDs in their standard enumeration order as defined in RC600Param.md
        /// </summary>
        /// <returns>Array of effect IDs in enumeration order</returns>
        public static int[] GetEffectsInEnumerationOrder()
        {
            // Based on the enumeration in RC600Param.md
            return new int[] {
                1,  // LPF (Low-pass filter)
                2,  // BPF (Band-pass filter)
                3,  // HPF (High-pass filter)
                4,  // Phaser
                5,  // Flanger
                6,  // Synth
                7,  // Lo-Fi
                8,  // Radio
                9,  // Ring Mod
                10, // G2B (Guitar-to-Bass)
                11, // Sustainer
                12, // Auto Riff
                13, // Slow Gear
                14, // Transpose
                15, // Pitch Bend
                16, // Robot
                17, // Electric
                18, // HRM Manual
                19, // HRM Auto
                20, // Vocoder
                21, // OSC Voc (MIDI)
                22, // OSC Bot
                23, // Preamp
                24, // Dist (Distortion)
                25, // Dynamics (Comp/Lim presets)
                26, // EQ (4-band)
                27, // Isolator
                28, // Octave
                29, // Auto Pan
                30, // Manual Pan
                31, // Stereo Enhance
                32, // Tremolo
                33, // Vibrato
                34, // Pattern Slicer
                35, // Step Slicer
                36, // Delay
                37, // Panning Delay
                38, // Reverse Delay
                39, // Mod Delay
                40, // Tape Echo 1
                41, // Tape Echo 2
                42, // Granular Delay
                43, // Warp
                44, // Twist
                45, // Roll 1
                46, // Roll 2
                47, // Freeze
                48, // Chorus
                49, // Reverb
                50, // Gate Reverb
                51, // Reverse Reverb
                52, // Beat Scatter (Track-only)
                53, // Beat Repeat (Track-only)
                54, // Beat Shift (Track-only)
                55, // Vinyl Flick (Track-only)
            };
        }
    }
}
