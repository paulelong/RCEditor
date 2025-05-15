using System.Collections.Generic;

namespace RCEditor.Models
{
    public class InputRouting
    {
        public InputRouteEnum MicIn { get; set; }
        public InputRouteEnum Inst1 { get; set; }
        public InputRouteEnum Inst2 { get; set; }
    }

    public class Track
    {
        public bool OneShot { get; set; }
        public bool Reverse { get; set; }
        public int Level { get; set; }          // 0–200
        public int Pan { get; set; }            // -50..50
        public StartModeEnum StartMode { get; set; }
        public int FadeInMeasures { get; set; }
        public StopModeEnum StopMode { get; set; }
        public int FadeOutMeasures { get; set; }
        public OverdubModeEnum OverdubMode { get; set; }
        public bool TempoSyncSw { get; set; }  // Renamed from TempoSyncMode for clarity
        public TempoSyncModeEnum TempoSyncMode { get; set; } // PITCH/XFADE
        public TempoSyncSpeedEnum TempoSyncSpeed { get; set; } // HALF/NORMAL/DOUBLE
        public int MeasureCount { get; set; }
        public bool FXEnabled { get; set; }     // Enable/disable track FX
        public PlayModeEnum PlayMode { get; set; }
        public bool LoopSyncSw { get; set; }    // Turns loop sync on/off
        public LoopSyncModeEnum LoopSyncMode { get; set; } // How syncing is performed
        public bool BounceIn { get; set; }      // Whether playback from other tracks is recorded during overdub
        public InputRouting InputRouting { get; set; }
        public OutputAssignEnum? OutputAssign { get; set; }

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