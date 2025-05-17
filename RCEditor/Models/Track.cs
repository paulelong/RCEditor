using System.Collections.Generic;

namespace RCEditor.Models
{
    public class InputRouting
    {
        public bool MicInEnabled { get; set; }
        public bool Inst1Enabled { get; set; }
        public bool Inst2Enabled { get; set; }
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
        public TempoSyncEnum TempoSyncMode { get; set; }
        public int MeasureCount { get; set; }
        public InputRouting InputRouting { get; set; }
        public OutputAssignEnum? OutputAssign { get; set; }

        public Track()
        {
            Level = 100;
            Pan = 0;
            InputRouting = new InputRouting();
        }
    }
}