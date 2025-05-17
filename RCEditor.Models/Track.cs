using System.Collections.Generic;

namespace RCEditor.Models
{    public class InputRouting
    {
        public bool Mic1Enabled { get; set; }
        public bool Mic2Enabled { get; set; }
        public bool Inst1Enabled { get; set; }
        public bool Inst2Enabled { get; set; }
        public bool RhythmEnabled { get; set; }
    }

    public class Track
    {
        public bool OneShot { get; set; }           // Tag B - 1SHOT
        public bool Reverse { get; set; }           // Tag A - REVERSE
        public int Level { get; set; }              // Tag D - PLAY LEVEL (0–200)
        public int Pan { get; set; }                // Tag C - PAN (-50..50)
        public StartModeEnum StartMode { get; set; } // Tag E - START MODE
        public int FadeInMeasures { get; set; }     // Fade in time for START MODE = FADE
        public StopModeEnum StopMode { get; set; }   // Tag F - STOP MODE
        public int FadeOutMeasures { get; set; }    // Fade out time for STOP MODE = FADE
        public OverdubModeEnum OverdubMode { get; set; } // Tag G - DUB MODE
        public bool TempoSyncSw { get; set; }       // Tag M - TEMPO SYNC SW
        public TempoSyncModeEnum TempoSyncMode { get; set; } // Tag N - TEMPO SYNC MODE (PITCH/XFADE)
        public TempoSyncSpeedEnum TempoSyncSpeed { get; set; } // Tag O - TEMPO SYNC SPEED (HALF/NORMAL/DOUBLE)
        public int MeasureCount { get; set; }       // Tag J - MEASURE
        public int MeasureCountB { get; set; }      // Tag S - MEASUREB
        public bool FXEnabled { get; set; }         // Tag H - FX (Enable/disable track FX)
        public PlayModeEnum PlayMode { get; set; }  // Tag I - PLAY MODE
        public bool LoopSyncSw { get; set; }        // Tag L - LOOP SYNC SW
        public LoopSyncModeEnum LoopSyncMode { get; set; } // Tag Y - LOOP SYNC MODE
        public bool BounceIn { get; set; }          // Tag W - BOUNCE IN
        public InputRouting InputRouting { get; set; } // Tags Q,R,S,T - Input routing
        public OutputAssignEnum? OutputAssign { get; set; }
        
        // Unknown parameters that must be preserved
        public int UnknownK { get; set; }           // Tag K - Unknown parameter
        public int UnknownP { get; set; }           // Tag P - Unknown parameter
        public int UnknownR { get; set; }           // Tag R - Unknown parameter
        public int UnknownT { get; set; }           // Tag T - Unknown parameter (if not used for InputRouting.Rhythm)
        public int UnknownU { get; set; }           // Tag U - Unknown parameter
        public int UnknownV { get; set; }           // Tag V - Unknown parameter
        public int UnknownX { get; set; }           // Tag X - Unknown parameter

        public Track()
        {
            Level = 100;
            Pan = 0;
            InputRouting = new InputRouting();
            TempoSyncMode = TempoSyncModeEnum.Xfade;
            TempoSyncSpeed = TempoSyncSpeedEnum.Normal;
            LoopSyncMode = LoopSyncModeEnum.Measure;
        }
    }
}