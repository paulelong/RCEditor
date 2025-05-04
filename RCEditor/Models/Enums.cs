namespace RCEditor.Models
{
    public enum PlayModeEnum
    {
        Multi,
        Single
    }

    public enum SingleModeSwitchEnum
    {
        Immediate,
        Loop,
        OneShot,
        FadeOut
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
}