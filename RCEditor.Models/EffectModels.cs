using System;
using System.Collections.Generic;

namespace RCEditor.Models
{
    // Base class for all effects
    public abstract class EffectBase
    {
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public bool Enabled { get; set; } = false;
        public SwitchModeEnum SwitchMode { get; set; } = SwitchModeEnum.Toggle;
    }

    #region Effect Categories

    // ----- Filter Effects -----
    
    public abstract class FilterEffect : EffectBase
    {
        public int Rate { get; set; } = 50;
        public int Depth { get; set; } = 50;
        public int Resonance { get; set; } = 50;
        public int Cutoff { get; set; } = 50;
        public string StepRate { get; set; } = "OFF";
    }

    public class LowPassFilter : FilterEffect
    {
        public LowPassFilter() 
        {
            Name = "LPF";
            Category = "Filter";
        }
    }

    public class BandPassFilter : FilterEffect
    {
        public BandPassFilter() 
        {
            Name = "BPF";
            Category = "Filter";
        }
    }

    public class HighPassFilter : FilterEffect
    {
        public HighPassFilter() 
        {
            Name = "HPF";
            Category = "Filter";
        }
    }

    // ----- Modulation Effects -----
    
    public class Phaser : EffectBase
    {
        public int Rate { get; set; } = 50;
        public int Depth { get; set; } = 50;
        public int Resonance { get; set; } = 50;
        public int Manual { get; set; } = 50;
        public string Stage { get; set; } = "4";
        public string StepRate { get; set; } = "OFF";
        public int DryLevel { get; set; } = 100;
        public int EffectLevel { get; set; } = 100;

        public Phaser() 
        {
            Name = "Phaser";
            Category = "Modulation";
        }
    }

    public class Flanger : EffectBase
    {
        public int Rate { get; set; } = 50;
        public int Depth { get; set; } = 50;
        public int Resonance { get; set; } = 50;
        public int Manual { get; set; } = 50;
        public string StepRate { get; set; } = "OFF";
        public int DryLevel { get; set; } = 100;
        public int EffectLevel { get; set; } = 100;
        public int Separation { get; set; } = 50;

        public Flanger() 
        {
            Name = "Flanger";
            Category = "Modulation";
        }
    }

    public class Chorus : EffectBase
    {
        public int Rate { get; set; } = 50;
        public int Depth { get; set; } = 50;
        public int DryLevel { get; set; } = 100;
        public string LowCut { get; set; } = "FLAT";
        public string HighCut { get; set; } = "FLAT";
        public int EffectLevel { get; set; } = 100;

        public Chorus() 
        {
            Name = "Chorus";
            Category = "Modulation";
        }
    }

    public class Tremolo : EffectBase
    {
        public int Rate { get; set; } = 50;
        public int Depth { get; set; } = 50;
        public string Waveform { get; set; } = "TRI";

        public Tremolo() 
        {
            Name = "Tremolo";
            Category = "Modulation";
        }
    }

    public class AutoPan : EffectBase
    {
        public int Rate { get; set; } = 50;
        public int Depth { get; set; } = 50;
        public int Waveform { get; set; } = 50;
        public int InitPhase { get; set; } = 0;
        public string StepRate { get; set; } = "OFF";

        public AutoPan() 
        {
            Name = "Auto Pan";
            Category = "Modulation";
        }
    }

    public class ManualPan : EffectBase
    {
        public string Position { get; set; } = "CENTER";

        public ManualPan() 
        {
            Name = "Manual Pan";
            Category = "Modulation";
        }
    }

    // ----- Dynamics/Tone Effects -----
    
    public class Sustainer : EffectBase
    {
        public int Attack { get; set; } = 50;
        public int Release { get; set; } = 50;
        public int Level { get; set; } = 50;
        public int LowGain { get; set; } = 0;
        public int HighGain { get; set; } = 0;
        public int Sustain { get; set; } = 50;

        public Sustainer() 
        {
            Name = "Sustainer";
            Category = "Dynamics";
        }
    }

    public class FourBandEQ : EffectBase
    {
        public bool Switch { get; set; } = false;
        public int LowGain { get; set; } = 0;
        public int HighGain { get; set; } = 0;
        public int LowMidFreq { get; set; } = 250;
        public int LowMidQ { get; set; } = 4;
        public int LowMidGain { get; set; } = 0;
        public int HighMidFreq { get; set; } = 800;
        public int HighMidQ { get; set; } = 4;
        public int HighMidGain { get; set; } = 0;
        public int Level { get; set; } = 0;
        public string LowCut { get; set; } = "FLAT";
        public string HighCut { get; set; } = "FLAT";

        public FourBandEQ() 
        {
            Name = "4-Band EQ";
            Category = "Tone";
        }
    }

    // ----- Pitch/Harmonic Effects -----
    
    public class Transpose : EffectBase
    {
        public int Trans { get; set; } = 0;
        public string Mode { get; set; } = "";

        public Transpose() 
        {
            Name = "Transpose";
            Category = "Pitch";
        }
    }

    public class PitchBend : EffectBase
    {
        public int Pitch { get; set; } = 0;
        public int Bend { get; set; } = 0;

        public PitchBend() 
        {
            Name = "Pitch Bend";
            Category = "Pitch";
        }
    }

    public class Octave : EffectBase
    {
        public string OctaveValue { get; set; } = "-1";
        public int Level { get; set; } = 50;

        public Octave() 
        {
            Name = "Octave";
            Category = "Pitch";
        }
    }

    public class G2B : EffectBase
    {
        public int Balance { get; set; } = 50;

        public G2B() 
        {
            Name = "G2B";
            Category = "Pitch";
        }
    }

    public class Robot : EffectBase
    {
        public string Note { get; set; } = "C";
        public int Formant { get; set; } = 50;

        public Robot() 
        {
            Name = "Robot";
            Category = "Pitch";
        }
    }

    public class Harmony : EffectBase
    {
        // Multiple parameters as per guide
        public Harmony() 
        {
            Name = "Harmony";
            Category = "Pitch";
        }
    }

    // ----- Character & Lo-Fi Effects -----
    
    public class LoFi : EffectBase
    {
        public int BitDepth { get; set; } = 16;
        public int SampleRate { get; set; } = 44100;
        public int Balance { get; set; } = 50;

        public LoFi() 
        {
            Name = "Lo-Fi";
            Category = "Character";
        }
    }

    public class Radio : EffectBase
    {
        public int LoFiLevel { get; set; } = 50;

        public Radio() 
        {
            Name = "Radio";
            Category = "Character";
        }
    }

    public class RingMod : EffectBase
    {
        public int Frequency { get; set; } = 50;
        public int Balance { get; set; } = 50;

        public RingMod() 
        {
            Name = "Ring Mod";
            Category = "Character";
        }
    }

    public class Synth : EffectBase
    {
        public int Frequency { get; set; } = 50;
        public int Resonance { get; set; } = 50;
        public int Decay { get; set; } = 50;
        public int Balance { get; set; } = 50;

        public Synth() 
        {
            Name = "Synth";
            Category = "Character";
        }
    }

    public class StereoEnhance : EffectBase
    {
        public string LowCut { get; set; } = "FLAT";
        public string HighCut { get; set; } = "FLAT";
        public int Enhance { get; set; } = 50;

        public StereoEnhance() 
        {
            Name = "Stereo Enhance";
            Category = "Character";
        }
    }

    // ----- Sequence / DJ-Style Effects (Track-FX-only) -----
    
    public class BeatScatter : EffectBase
    {
        public string Type { get; set; } = "";
        public string Length { get; set; } = "";

        public BeatScatter() 
        {
            Name = "Beat Scatter";
            Category = "Sequence";
        }
    }

    public class BeatRepeat : EffectBase
    {
        public string Type { get; set; } = "";
        public string Length { get; set; } = "";

        public BeatRepeat() 
        {
            Name = "Beat Repeat";
            Category = "Sequence";
        }
    }

    public class BeatShift : EffectBase
    {
        public string Type { get; set; } = "";
        public string Shift { get; set; } = "";

        public BeatShift() 
        {
            Name = "Beat Shift";
            Category = "Sequence";
        }
    }

    public class VinylFlick : EffectBase
    {
        public int Flick { get; set; } = 50;

        public VinylFlick() 
        {
            Name = "Vinyl Flick";
            Category = "Sequence";
        }
    }

    public class Roll : EffectBase
    {
        public string Time { get; set; } = "";
        public int Feedback { get; set; } = 50;
        public string Divisor { get; set; } = "";

        public Roll() 
        {
            Name = "Roll";
            Category = "Sequence";
        }
    }

    public class Warp : EffectBase
    {
        public int Level { get; set; } = 50;

        public Warp() 
        {
            Name = "Warp";
            Category = "Sequence";
        }
    }

    public class Twist : EffectBase
    {
        public int Release { get; set; } = 50;
        public int Rise { get; set; } = 50;
        public int Level { get; set; } = 50;

        public Twist() 
        {
            Name = "Twist";
            Category = "Sequence";
        }
    }

    public class Freeze : EffectBase
    {
        public int Attack { get; set; } = 50;
        public int Release { get; set; } = 50;
        public int Decay { get; set; } = 50;
        public int Sustain { get; set; } = 50;

        public Freeze() 
        {
            Name = "Freeze";
            Category = "Sequence";
        }
    }

    // ----- Delays & Echo Effects -----
    
    public class Delay : EffectBase
    {
        public string Time { get; set; } = "";
        public int Feedback { get; set; } = 50;
        public int Level { get; set; } = 50;

        public Delay() 
        {
            Name = "Delay";
            Category = "Delay";
        }
    }

    public class PingPongDelay : Delay
    {
        public PingPongDelay() 
        {
            Name = "Ping-Pong Delay";
        }
    }

    public class ModDelay : Delay
    {
        public int ModDepth { get; set; } = 50;

        public ModDelay() 
        {
            Name = "Mod Delay";
        }
    }

    public class TapeEcho1 : EffectBase
    {
        public int Rate { get; set; } = 50;
        public int Intensity { get; set; } = 50;
        public int EQ { get; set; } = 50;
        public int Level { get; set; } = 50;

        public TapeEcho1() 
        {
            Name = "Tape Echo 1";
            Category = "Delay";
        }
    }

    public class TapeEcho2 : EffectBase
    {
        public int Rate { get; set; } = 50;
        public int Intensity { get; set; } = 50;
        public string LowCut { get; set; } = "FLAT";
        public string HighCut { get; set; } = "FLAT";
        public int Level { get; set; } = 50;

        public TapeEcho2() 
        {
            Name = "Tape Echo 2";
            Category = "Delay";
        }
    }

    public class GranularDelay : EffectBase
    {
        public string Time { get; set; } = "";
        public int Feedback { get; set; } = 50;
        public int Level { get; set; } = 50;

        public GranularDelay() 
        {
            Name = "Granular Delay";
            Category = "Delay";
        }
    }

    // ----- Reverb Effects -----
    
    public abstract class ReverbBase : EffectBase
    {
        public int Time { get; set; } = 50;
        public int PreDelay { get; set; } = 50;
        public int Level { get; set; } = 50;
        
        public ReverbBase() 
        {
            Category = "Reverb";
        }
    }

    public class HallReverb : ReverbBase
    {
        public int Density { get; set; } = 50;
        public string LowCut { get; set; } = "FLAT";
        public string HighCut { get; set; } = "FLAT";

        public HallReverb() 
        {
            Name = "Hall Reverb";
        }
    }

    public class RoomReverb : ReverbBase
    {
        public int Density { get; set; } = 50;
        public string LowCut { get; set; } = "FLAT";
        public string HighCut { get; set; } = "FLAT";

        public RoomReverb() 
        {
            Name = "Room Reverb";
        }
    }

    public class PlateReverb : ReverbBase
    {
        public int Density { get; set; } = 50;
        public string LowCut { get; set; } = "FLAT";
        public string HighCut { get; set; } = "FLAT";

        public PlateReverb() 
        {
            Name = "Plate Reverb";
        }
    }

    public class GateReverb : ReverbBase
    {
        public int Threshold { get; set; } = 50;

        public GateReverb() 
        {
            Name = "Gate Reverb";
        }
    }

    public class ReverseReverb : ReverbBase
    {
        public int GateTime { get; set; } = 50;

        public ReverseReverb() 
        {
            Name = "Reverse Reverb";
        }
    }

    // ----- Utility / Misc Effects -----
    
    public class AutoRiff : EffectBase
    {
        public string Phrase { get; set; } = "";
        public int Tempo { get; set; } = 120;
        public bool Loop { get; set; } = true;
        public string Key { get; set; } = "C";
        public int Balance { get; set; } = 50;

        public AutoRiff() 
        {
            Name = "Auto Riff";
            Category = "Utility";
        }
    }

    public class SlowGear : EffectBase
    {
        public int Sens { get; set; } = 50;
        public int RiseTime { get; set; } = 50;
        public int Level { get; set; } = 50;

        public SlowGear() 
        {
            Name = "Slow Gear";
            Category = "Utility";
        }
    }

    public class Compressor : EffectBase
    {
        public int Threshold { get; set; } = -30;
        public int Ratio { get; set; } = 2;
        public int Attack { get; set; } = 50;
        public int Release { get; set; } = 50;
        public int Level { get; set; } = 0;

        public Compressor() 
        {
            Name = "Compressor";
            Category = "Dynamics";
        }
    }

    public class Limiter : EffectBase
    {
        public int Threshold { get; set; } = -10;
        public int Release { get; set; } = 50;
        public int Level { get; set; } = 0;

        public Limiter() 
        {
            Name = "Limiter";
            Category = "Dynamics";
        }
    }

    #endregion

    #region Additional Effects

    public class PatternSlicer : EffectBase
    {
        public int Rate { get; set; } = 50;
        public int Duty { get; set; } = 60;
        public int Attack { get; set; } = 35;
        public string Pattern { get; set; } = "P01";
        public int Depth { get; set; } = 50;
        public int CompThresh { get; set; } = -15;
        public int CompGain { get; set; } = 2;

        public PatternSlicer() 
        {
            Name = "Pattern Slicer";
            Category = "Sequence";
        }
    }

    public class StepSlicer : EffectBase
    {
        public int Rate { get; set; } = 50;
        public int StepMax { get; set; } = 8;
        public int StepLen { get; set; } = 50;
        public int StepLevel { get; set; } = 50;
        public int Depth { get; set; } = 50;
        public int CompThresh { get; set; } = -15;
        public int CompGain { get; set; } = 6;

        public StepSlicer() 
        {
            Name = "Step Slicer";
            Category = "Sequence";
        }
    }

    public class Isolator : EffectBase
    {
        public string Band { get; set; } = "MID";
        public int Rate { get; set; } = 50;
        public int BandLevel { get; set; } = 50;
        public int Depth { get; set; } = 50;
        public string StepRate { get; set; } = "OFF";
        public string Waveform { get; set; } = "TRI";

        public Isolator() 
        {
            Name = "Isolator";
            Category = "Filter";
        }
    }

    public class Preamp : EffectBase
    {
        public string AmpType { get; set; } = "JC-120";
        public string SpeakerType { get; set; } = "ORIGINAL";
        public int Gain { get; set; } = 50;
        public int TubeComp { get; set; } = 0;
        public int Bass { get; set; } = 50;
        public int Mid { get; set; } = 50;
        public int Treble { get; set; } = 50;
        public int Level { get; set; } = 50;

        public Preamp() 
        {
            Name = "Preamp";
            Category = "Character";
        }
    }

    public class Distortion : EffectBase
    {
        public string Type { get; set; } = "OD";
        public int Tone { get; set; } = 0;
        public int Dist { get; set; } = 50;
        public int DryLevel { get; set; } = 50;
        public int EffectLevel { get; set; } = 50;

        public Distortion() 
        {
            Name = "Distortion";
            Category = "Character";
        }
    }

    public class DynamicsProcessor : EffectBase
    {
        public string Type { get; set; } = "NAT COMP";
        public int Dynamics { get; set; } = 0;
        public int LowGain { get; set; } = 0;
        public int LowMidGain { get; set; } = 0;
        public int HighMidGain { get; set; } = 0;
        public int HighGain { get; set; } = 0;

        public DynamicsProcessor() 
        {
            Name = "Dynamics Processor";
            Category = "Dynamics";
        }
    }

    public class PanningDelay : EffectBase
    {
        public int Time { get; set; } = 500;
        public int Feedback { get; set; } = 8;
        public int DryLevel { get; set; } = 100;
        public string LowCut { get; set; } = "FLAT";
        public string HighCut { get; set; } = "FLAT";
        public int EffectLevel { get; set; } = 100;

        public PanningDelay() 
        {
            Name = "Panning Delay";
            Category = "Delay";
        }
    }

    public class ReverseDelay : EffectBase
    {
        public int Time { get; set; } = 500;
        public int Feedback { get; set; } = 8;
        public int DryLevel { get; set; } = 100;
        public string LowCut { get; set; } = "FLAT";
        public string HighCut { get; set; } = "FLAT";
        public int EffectLevel { get; set; } = 100;

        public ReverseDelay() 
        {
            Name = "Reverse Delay";
            Category = "Delay";
        }
    }

    public class OscBot : EffectBase
    {
        public string Oscillator { get; set; } = "SAW";
        public string Note { get; set; } = "C4";
        public int Tone { get; set; } = 0;
        public int Attack { get; set; } = 50;
        public int ModSens { get; set; } = 0;
        public int Balance { get; set; } = 50;

        public OscBot() 
        {
            Name = "OSC BOT";
            Category = "Pitch";
        }
    }

    public class OscVoc : EffectBase
    {
        public string Carrier { get; set; } = "SAW";
        public string Octave { get; set; } = "0OCT";
        public int Tone { get; set; } = 0;
        public int Attack { get; set; } = 50;
        public int ModSens { get; set; } = 0;
        public int Release { get; set; } = 50;
        public int Balance { get; set; } = 50;

        public OscVoc() 
        {
            Name = "OSC VOC";
            Category = "Pitch";
        }
    }

    public class BitCrusher : EffectBase
    {
        public string BitDepth { get; set; } = "OFF";
        public string SampleRate { get; set; } = "OFF";
        public string Filter { get; set; } = "THRU";
        public int Balance { get; set; } = 50;

        public BitCrusher() 
        {
            Name = "Bit Crusher";
            Category = "Lo-Fi";
        }
    }

    public class Defretter : EffectBase
    {
        public int Depth { get; set; } = 50;
        public int Tone { get; set; } = 0;
        public int Attack { get; set; } = 50;
        public int Balance { get; set; } = 50;

        public Defretter() 
        {
            Name = "Defretter";
            Category = "Character";
        }
    }

    public class PedalBend : EffectBase
    {
        public string BendUp { get; set; } = "+1";
        public string BendDown { get; set; } = "-1";
        public int PedalRange { get; set; } = 50;
        public string Mode { get; set; } = "1";

        public PedalBend() 
        {
            Name = "Pedal Bend";
            Category = "Pitch";
        }
    }

    public class Resonator : EffectBase
    {
        public string Body { get; set; } = "MEDIUM";
        public string Color { get; set; } = "BRIGHT";
        public int Reso { get; set; } = 50;
        public int Balance { get; set; } = 50;

        public Resonator() 
        {
            Name = "Resonator";
            Category = "Character";
        }
    }

    public class Seeker : EffectBase
    {
        public int Rate { get; set; } = 50;
        public int Depth { get; set; } = 50;
        public string StepRate { get; set; } = "OFF";
        public string Wave { get; set; } = "TRI";

        public Seeker() 
        {
            Name = "Seeker";
            Category = "Filter";
        }
    }

    public class StepFilter : EffectBase
    {
        public int StepMax { get; set; } = 8;
        public int Cutoff { get; set; } = 50;
        public int Depth { get; set; } = 50;
        public int Rate { get; set; } = 50;

        public StepFilter() 
        {
            Name = "Step Filter";
            Category = "Filter";
        }
    }

    public class Stutter : EffectBase
    {
        public string Rate { get; set; } = "1/8";
        public string Repeats { get; set; } = "4";
        public int Depth { get; set; } = 50;
        public int Gate { get; set; } = 50;

        public Stutter() 
        {
            Name = "Stutter";
            Category = "Sequence";
        }
    }

    public class TeraEcho : EffectBase
    {
        public int Spread { get; set; } = 50;
        public int Feedback { get; set; } = 50;
        public int ModRate { get; set; } = 50;
        public int ModDepth { get; set; } = 50;
        public int DryLevel { get; set; } = 100;
        public int EffectLevel { get; set; } = 100;

        public TeraEcho() 
        {
            Name = "Tera Echo";
            Category = "Delay";
        }
    }

    public class Vibrato : EffectBase
    {
        public int Rate { get; set; } = 50;
        public int Depth { get; set; } = 50;
        public int Rise { get; set; } = 0;
        public int DryLevel { get; set; } = 100;

        public Vibrato() 
        {
            Name = "Vibrato";
            Category = "Modulation";
        }
    }

    public class WahAuto : EffectBase
    {
        public int Rate { get; set; } = 50;
        public int Depth { get; set; } = 50;
        public int Resonance { get; set; } = 50;
        public int Manual { get; set; } = 50;

        public WahAuto() 
        {
            Name = "Wah Auto";
            Category = "Filter";
        }
    }

    public class WahManual : EffectBase
    {
        public int PedalPos { get; set; } = 50;
        public int Resonance { get; set; } = 50;
        public int LowGain { get; set; } = 0;
        public int HighGain { get; set; } = 0;

        public WahManual() 
        {
            Name = "Wah Manual";
            Category = "Filter";
        }
    }

    #endregion

    // Factory method to create an effect instance by name
    public static class EffectFactory
    {
        public static EffectBase CreateEffect(string effectType)
        {
            switch (effectType.ToUpper())
            {
                // Filter effects
                case "LPF": return new LowPassFilter();
                case "BPF": return new BandPassFilter();
                case "HPF": return new HighPassFilter();
                
                // Modulation effects
                case "PHASER": return new Phaser();
                case "FLANGER": return new Flanger();
                case "CHORUS": return new Chorus();
                case "TREMOLO": return new Tremolo();
                case "AUTO PAN": return new AutoPan();
                case "MANUAL PAN": return new ManualPan();
                
                // Dynamics/Tone effects
                case "SUSTAINER": return new Sustainer();
                case "4-BAND EQ": return new FourBandEQ();
                
                // Pitch/Harmonic effects
                case "TRANSPOSE": return new Transpose();
                case "PITCH BEND": return new PitchBend();
                case "OCTAVE": return new Octave();
                case "G2B": return new G2B();
                case "ROBOT": return new Robot();
                case "HARMONY": return new Harmony();
                
                // Character & Lo-Fi effects
                case "LO-FI": return new LoFi();
                case "RADIO": return new Radio();
                case "RING MOD": return new RingMod();
                case "SYNTH": return new Synth();
                case "STEREO ENHANCE": return new StereoEnhance();
                
                // Sequence / DJ-Style effects
                case "BEAT SCATTER": return new BeatScatter();
                case "BEAT REPEAT": return new BeatRepeat();
                case "BEAT SHIFT": return new BeatShift();
                case "VINYL FLICK": return new VinylFlick();
                case "ROLL": return new Roll();
                case "WARP": return new Warp();
                case "TWIST": return new Twist();
                case "FREEZE": return new Freeze();
                
                // Delays & Echo effects
                case "DELAY": return new Delay();
                case "PING-PONG DELAY": return new PingPongDelay();
                case "MOD DELAY": return new ModDelay();
                case "TAPE ECHO 1": return new TapeEcho1();
                case "TAPE ECHO 2": return new TapeEcho2();
                case "GRANULAR DELAY": return new GranularDelay();
                
                // Reverb effects
                case "HALL REVERB": return new HallReverb();
                case "ROOM REVERB": return new RoomReverb();
                case "PLATE REVERB": return new PlateReverb();
                case "GATE REVERB": return new GateReverb();
                case "REVERSE REVERB": return new ReverseReverb();
                
                // Utility / Misc effects
                case "AUTO RIFF": return new AutoRiff();
                case "SLOW GEAR": return new SlowGear();
                case "COMPRESSOR": return new Compressor();
                case "LIMITER": return new Limiter();
                
                // Additional effects
                case "PATTERN SLICER": return new PatternSlicer();
                case "STEP SLICER": return new StepSlicer();
                case "ISOLATOR": return new Isolator();
                case "PREAMP": return new Preamp();
                case "DISTORTION": return new Distortion();
                case "DYNAMICS PROCESSOR": return new DynamicsProcessor();
                case "PANNING DELAY": return new PanningDelay();
                case "REVERSE DELAY": return new ReverseDelay();
                case "OSC BOT": return new OscBot();
                case "OSC VOC": return new OscVoc();
                case "BIT CRUSHER": return new BitCrusher();
                case "DEFRETTER": return new Defretter();
                case "PEDAL BEND": return new PedalBend();
                case "RESONATOR": return new Resonator();
                case "SEEKER": return new Seeker();
                case "STEP FILTER": return new StepFilter();
                case "STUTTER": return new Stutter();
                case "TERA ECHO": return new TeraEcho();
                case "VIBRATO": return new Vibrato();
                case "WAH AUTO": return new WahAuto();
                case "WAH MANUAL": return new WahManual();
                
                default: throw new ArgumentException($"Unknown effect type: {effectType}");
            }
        }
    }
}