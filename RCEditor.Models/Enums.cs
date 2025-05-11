namespace RCEditor.Models
{
    public enum PlayModeEnum
    {
        Multi = 0,
        Single = 1
    }
    
    public enum SingleModeSwitchEnum
    {
        Loop = 0,
        Immediate = 1
    }
    
    public enum SingleTrackChangeEnum
    {
        Immediate = 0,
        Measure = 1,
        LoopEnd = 2
    }
    
    public enum RecActionEnum
    {
        RecToDub = 0,
        RecToPlay = 1
    }
    
    public enum SpeedChangeEnum
    {
        Immediate = 0,
        LoopEnd = 1
    }
    
    public enum SyncAdjustEnum
    {
        Measure = 0,
        Beat = 1
    }

    public enum StartModeEnum
    {
        Immediate,
        Fade,
        InputLevel
    }

    public enum StopModeEnum
    {
        Immediate,
        Fade,
        RecStop,
        RecFade
    }

    public enum OverdubModeEnum
    {
        Normal,
        Replace,
        Substract
    }

    public enum TempoSyncEnum
    {
        Auto,
        Manual
    }

    public enum TempoSyncModeEnum
    {
        Pitch,
        Xfade
    }

    public enum TempoSyncSpeedEnum
    {
        Half,
        Normal,
        Double
    }

    public enum LoopSyncModeEnum
    {
        Immediate,
        Measure,
        LoopLength
    }

    public enum OutputAssignEnum
    {
        Main,
        Sub,
        MainSub
    }

    public enum SwitchModeEnum
    {
        Momentary,
        Toggle,
        Auto
    }

    public enum ActionModeEnum
    {
        Momentary,
        Toggle,
        Continuous
    }

    public enum VariationTimingEnum
    {
        Immediate,
        End,
        Measure
    }

    public enum InputRouteEnum
    {
        None,
        Input1,
        Input2,
        Input3,
        Input4,
        All
    }

    // Rhythm settings enums
    public enum RhythmStartTrigEnum
    {
        LoopStart,
        RecEnd,
        BeforeLoop
    }

    public enum RhythmStopTrigEnum
    {
        Off,
        LoopStop,
        RecEnd
    }

    public enum RhythmVariationChangeEnum
    {
        Measure,
        LoopEnd
    }
}