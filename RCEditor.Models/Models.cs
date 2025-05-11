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
        public double Tempo { get; set; }
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
            Genre = string.Empty;
            Pattern = string.Empty;
            Variation = 'A';
            Kit = string.Empty;
            Beat = string.Empty;
            Tempo = 120;
        }
    }

    public class PedalModeAssignment
    {
        public string Mode1 { get; set; }
        public string Mode2 { get; set; }
        public string Mode3 { get; set; }

        public PedalModeAssignment()
        {
            Mode1 = string.Empty;
            Mode2 = string.Empty;
            Mode3 = string.Empty;
        }
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
            ExternalSwitch1 = string.Empty;
            ExternalSwitch2 = string.Empty;
            Expression1 = string.Empty;
            Expression2 = string.Empty;
        }
    }

    public class AssignSlot
    {
        public string Source { get; set; }
        public string Target { get; set; }
        public ActionModeEnum ActionMode { get; set; }
        public double? RangeMin { get; set; }
        public double? RangeMax { get; set; }

        public AssignSlot()
        {
            Source = string.Empty;
            Target = string.Empty;
        }
    }

    public class RecSettings
    {
        public RecActionEnum RecAction { get; set; }
        public bool QuantizeEnabled { get; set; }
        public bool AutoRecEnabled { get; set; }
        public int AutoRecSensitivity { get; set; } // 1-100
        public bool BounceEnabled { get; set; }
        public int BounceTrack { get; set; } // 1-6

        public RecSettings()
        {
            RecAction = RecActionEnum.RecToDub;
            QuantizeEnabled = false;
            AutoRecEnabled = false;
            AutoRecSensitivity = 50;
            BounceEnabled = false;
            BounceTrack = 1;
        }
    }

    public class PlaySettings
    {
        public SingleTrackChangeEnum SingleTrackChange { get; set; }
        public int FadeTimeIn { get; set; }
        public int FadeTimeOut { get; set; }
        public bool[] AllStartTracks { get; set; } = new bool[6];
        public bool[] AllStopTracks { get; set; } = new bool[6];
        public int LoopLength { get; set; }
        public SpeedChangeEnum SpeedChange { get; set; }
        public SyncAdjustEnum SyncAdjust { get; set; }

        public PlaySettings()
        {
            SingleTrackChange = SingleTrackChangeEnum.Immediate;
            FadeTimeIn = 1;
            FadeTimeOut = 1;
            AllStartTracks = new bool[6];
            AllStopTracks = new bool[6];
            LoopLength = 0; // 0 = AUTO
            SpeedChange = SpeedChangeEnum.Immediate;
            SyncAdjust = SyncAdjustEnum.Measure;
        }
    }

    public class MemoryPatch
    {
        public string Name { get; set; }           // max 12 chars
        public double Tempo { get; set; }
        public PlayModeEnum PlayMode { get; set; }
        public SingleModeSwitchEnum SingleModeSwitch { get; set; }
        public bool LoopSync { get; set; }
        public Track[] Tracks { get; set; } = new Track[6];
        
        // Update to use new effect models
        public EffectBanks InputFX { get; set; }
        public EffectBanks TrackFX { get; set; }
        
        public RhythmSettings Rhythm { get; set; }
        public ControlAssignments Controls { get; set; }
        public List<AssignSlot> Assigns { get; set; }
        public RecSettings Rec { get; set; }
        public PlaySettings Play { get; set; }

        public MemoryPatch()
        {
            Name = "NEW PATCH";
            Tempo = 120;
            PlayMode = PlayModeEnum.Multi;
            SingleModeSwitch = SingleModeSwitchEnum.Loop;
            
            // Initialize tracks
            for (int i = 0; i < 6; i++)
            {
                Tracks[i] = new Track();
            }

            // Initialize effects banks
            InputFX = new EffectBanks();
            TrackFX = new EffectBanks();
            
            Rhythm = new RhythmSettings();
            Controls = new ControlAssignments();
            Assigns = new List<AssignSlot>();
            Rec = new RecSettings();
            Play = new PlaySettings();
        }
    }
}
