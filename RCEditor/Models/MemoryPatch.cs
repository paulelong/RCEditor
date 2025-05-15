using System.Collections.Generic;

namespace RCEditor.Models
{
    public class RhythmSettings
    {
        public bool Enabled { get; set; }
        public string Genre { get; set; }
        public string Pattern { get; set; }
        public char Variation { get; set; }      // 'A','B','C','D'
        public string Kit { get; set; }
        public string Beat { get; set; }
        public RhythmStartTrigEnum StartMode { get; set; }
        public RhythmStopTrigEnum StopMode { get; set; }
        public bool IntroOnRec { get; set; }
        public bool IntroOnPlay { get; set; }
        public bool Ending { get; set; }
        public bool FillIn { get; set; }
        public RhythmVariationChangeEnum VariationChangeTiming { get; set; }

        public RhythmSettings()
        {
            Enabled = false;
            Variation = 'A';
            StartMode = RhythmStartTrigEnum.LoopStart;
            StopMode = RhythmStopTrigEnum.Off;
            VariationChangeTiming = RhythmVariationChangeEnum.Measure;
        }
    }

    public class PedalModeAssignment
    {
        public string Mode1 { get; set; }
        public string Mode2 { get; set; }
        public string Mode3 { get; set; }
    }

    public class ControlAssignments
    {
        public Dictionary<int, PedalModeAssignment> Pedals { get; set; } // key=1..9
        public string ExternalSwitch1 { get; set; }
        public string ExternalSwitch2 { get; set; }
        public string Expression1 { get; set; }
        public string Expression2 { get; set; }

        public ControlAssignments()
        {
            Pedals = new Dictionary<int, PedalModeAssignment>();
            for (int i = 1; i <= 9; i++)
            {
                Pedals[i] = new PedalModeAssignment();
            }
        }
    }

    public class AssignSlot
    {
        public string Source { get; set; }
        public string Target { get; set; }
        public ActionModeEnum ActionMode { get; set; }
        public double? RangeMin { get; set; }
        public double? RangeMax { get; set; }
    }

    public class MemoryPatch
    {
        public string Name { get; set; }           // max 12 chars
        public Track[] Tracks { get; set; } = new Track[6];
        public EffectBanks Effects { get; set; }
        public RhythmSettings Rhythm { get; set; }
        public ControlAssignments Controls { get; set; }
        public List<AssignSlot> Assigns { get; set; }

        public MemoryPatch()
        {
            Name = "NEW PATCH";
            
            // Initialize tracks
            for (int i = 0; i < 6; i++)
            {
                Tracks[i] = new Track();
            }

            Effects = new EffectBanks();
            Rhythm = new RhythmSettings();
            Controls = new ControlAssignments();
            Assigns = new List<AssignSlot>();
        }
    }
}