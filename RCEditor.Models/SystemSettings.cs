using System;
using System.Collections.Generic;

namespace RCEditor.Models
{
    /// <summary>
    /// System setup settings based on SETUP section in RC600Param.md
    /// </summary>
    public class SystemSetupSettings
    {
        public int Contrast { get; set; } = 6;                    // 1-10, default 6
        public string DisplayMode { get; set; } = "";             // MEMORY NUMBER, TRACK STATUS, etc.
        public string LoopIndicators { get; set; } = "";          // LOOP, RHYTHM, BEAT, TEMPO
        public bool AutoOff { get; set; } = false;                // OFF/ON, default OFF
        public int MemoryExtentMin { get; set; } = 1;             // 01-99
        public int MemoryExtentMax { get; set; } = 99;            // 01-99
        public string[] KnobFunc { get; set; } = new string[4];   // Array of 4 knob functions
    }

    /// <summary>
    /// USB settings based on USB section in RC600Param.md
    /// </summary>
    public class UsbSettings
    {
        public bool Storage { get; set; } = false;                // OFF/CONNECT, default OFF
        public string AudioMode { get; set; } = "GENERIC";        // GENERIC/VENDOR, default GENERIC
        public string Routing { get; set; } = "LINE OUT";         // LINE OUT/SUB MIX/LOOP IN, default LINE OUT
        public int InputLevel { get; set; } = 100;                // 0-200, default 100
        public int OutputLevel { get; set; } = 100;               // 0-200, default 100
    }

    /// <summary>
    /// MIDI settings based on MIDI section in RC600Param.md
    /// </summary>
    public class MidiSettings
    {
        public int RxChCtl { get; set; } = 1;                     // 1-16
        public int RxChRhythm { get; set; } = 1;                  // 1-16
        public int RxChVoice { get; set; } = 1;                   // 1-16
        public string TxCh { get; set; } = "1";                   // 1-16 or "RX CTL"
        public string SyncClock { get; set; } = "AUTO";           // AUTO/INTERNAL/MIDI/USB, default AUTO
        public bool ClockOut { get; set; } = false;               // OFF/ON, default OFF
        public string StartSync { get; set; } = "ALL";            // OFF/ALL/RHYTHM, default ALL
        public bool PcOut { get; set; } = false;                  // OFF/ON, default OFF
        public string Thru { get; set; } = "OFF";                 // OFF/MIDI OUT/USB OUT/USB & MIDI, default OFF
    }

    /// <summary>
    /// System input setup settings
    /// </summary>
    public class SystemInputSetup
    {
        public bool PhantomMic1 { get; set; } = false;            // OFF/ON, default OFF
        public bool PhantomMic2 { get; set; } = false;            // OFF/ON, default OFF
        public string Inst1Gain { get; set; } = "INST";          // INST/LINE, default INST
        public string Inst2Gain { get; set; } = "INST";          // INST/LINE, default INST
        public bool StereoLinkMic { get; set; } = true;          // OFF/ON, default ON
        public bool StereoLinkInst1 { get; set; } = true;        // OFF/ON, default ON
        public bool StereoLinkInst2 { get; set; } = true;        // OFF/ON, default ON
        public string PreferenceMic { get; set; } = "SYSTEM";     // SYSTEM/MEMORY, default SYSTEM
        public string PreferenceInst1 { get; set; } = "SYSTEM";   // SYSTEM/MEMORY, default SYSTEM
        public string PreferenceInst2 { get; set; } = "SYSTEM";   // SYSTEM/MEMORY, default SYSTEM
    }

    /// <summary>
    /// System EQ settings
    /// </summary>
    public class SystemEqSettings
    {
        public bool SW { get; set; } = false;                     // OFF/ON, default OFF
        public int LoGain { get; set; } = 0;                      // -20 to +20 dB, default 0
        public int HiGain { get; set; } = 0;                      // -20 to +20 dB, default 0
        public int LoMidFreq { get; set; } = 250;                 // 20 Hz to 10 kHz, default 250 Hz
        public int LoMidQ { get; set; } = 4;                      // 0.5 to 16, default 4
        public int LoMidGain { get; set; } = 0;                   // -20 to +20 dB, default 0
        public int HiMidFreq { get; set; } = 800;                 // 20 Hz to 10 kHz, default 800 Hz
        public int HiMidQ { get; set; } = 4;                      // 0.5 to 16, default 4
        public int HiMidGain { get; set; } = 0;                   // -20 to +20 dB, default 0
        public int Level { get; set; } = 0;                       // -20 to +20 dB, default 0
        public string LoCut { get; set; } = "FLAT";               // FLAT or 20-800 Hz, default FLAT
        public string HiCut { get; set; } = "FLAT";               // 630 Hz-12.5 kHz or FLAT, default FLAT
    }

    /// <summary>
    /// System dynamics settings
    /// </summary>
    public class SystemDynamicsSettings
    {
        public string Mic1Comp { get; set; } = "OFF";             // OFF or 1-100, default OFF
        public int Mic1NS { get; set; } = 0;                      // 0-100, default 0
        public string Mic2Comp { get; set; } = "OFF";             // OFF or 1-100, default OFF
        public int Mic2NS { get; set; } = 0;                      // 0-100, default 0
        public int Inst1NS { get; set; } = 0;                     // 0-100, default 0
        public int Inst2NS { get; set; } = 0;                     // 0-100, default 0
    }

    /// <summary>
    /// System input settings container
    /// </summary>
    public class SystemInputSettings
    {
        public SystemInputSetup Setup { get; set; } = new SystemInputSetup();
        public SystemEqSettings Eq { get; set; } = new SystemEqSettings();
        public SystemDynamicsSettings Dynamics { get; set; } = new SystemDynamicsSettings();
    }

    /// <summary>
    /// System output setup settings
    /// </summary>
    public class SystemOutputSetup
    {
        public string OutputKnob { get; set; } = "ALL";           // ALL/MASTER/PHONES/OFF, default ALL
        public bool StereoLinkMain { get; set; } = true;          // OFF/ON, default ON
        public bool StereoLinkSub1 { get; set; } = true;          // OFF/ON, default ON
        public bool StereoLinkSub2 { get; set; } = true;          // OFF/ON, default ON
        public string PreferenceMain { get; set; } = "SYSTEM";    // SYSTEM/MEMORY, default SYSTEM
        public string PreferenceSub1 { get; set; } = "SYSTEM";    // SYSTEM/MEMORY, default SYSTEM
        public string PreferenceSub2 { get; set; } = "SYSTEM";    // SYSTEM/MEMORY, default SYSTEM
        public string PreferencePhones { get; set; } = "SYSTEM";  // SYSTEM/MEMORY, default SYSTEM
        public string PreferenceRhythm { get; set; } = "SYSTEM";  // SYSTEM/MEMORY, default SYSTEM
        public string PreferenceMfx { get; set; } = "SYSTEM";     // SYSTEM/MEMORY, default SYSTEM
    }

    /// <summary>
    /// System master FX settings
    /// </summary>
    public class SystemMasterFxSettings
    {
        public string Comp { get; set; } = "OFF";                 // OFF or 1-40, default OFF
        public int Reverb { get; set; } = 0;                      // 0-40, default 0
        public string Insert { get; set; } = "OFF";               // MAIN-L/R, SUB1-L/R, SUB2-L/R, OFF, default OFF
    }

    /// <summary>
    /// System routing settings (Track → Jack)
    /// </summary>
    public class SystemRoutingSettings
    {
        public bool[] MainTracks { get; set; } = new bool[6];     // Track 1-6 to MAIN output
        public bool[] Sub1Tracks { get; set; } = new bool[6];     // Track 1-6 to SUB1 output
        public bool[] Sub2Tracks { get; set; } = new bool[6];     // Track 1-6 to SUB2 output
        public bool[] PhonesTracks { get; set; } = new bool[6];   // Track 1-6 to PHONES output

        public SystemRoutingSettings()
        {
            // Initialize with default values (all tracks enabled for all outputs)
            for (int i = 0; i < 6; i++)
            {
                MainTracks[i] = true;
                Sub1Tracks[i] = true;
                Sub2Tracks[i] = true;
                PhonesTracks[i] = true;
            }
        }
    }

    /// <summary>
    /// System output settings container
    /// </summary>
    public class SystemOutputSettings
    {
        public SystemOutputSetup Setup { get; set; } = new SystemOutputSetup();
        public SystemMasterFxSettings MasterFx { get; set; } = new SystemMasterFxSettings();
        public SystemRoutingSettings Routing { get; set; } = new SystemRoutingSettings();
    }

    /// <summary>
    /// System mixer settings
    /// </summary>
    public class SystemMixerSettings
    {
        public int Mic1In { get; set; } = 100;                    // 0-200, default 100
        public int Mic2In { get; set; } = 100;                    // 0-200, default 100
        public int Inst1LIn { get; set; } = 100;                  // 0-200, default 100
        public int Inst1RIn { get; set; } = 100;                  // 0-200, default 100
        public int Inst2LIn { get; set; } = 100;                  // 0-200, default 100
        public int Inst2RIn { get; set; } = 100;                  // 0-200, default 100
        public int MainLOut { get; set; } = 100;                  // 0-200, default 100
        public int MainROut { get; set; } = 100;                  // 0-200, default 100
        public int Sub1LOut { get; set; } = 100;                  // 0-200, default 100
        public int Sub1ROut { get; set; } = 100;                  // 0-200, default 100
        public int Sub2LOut { get; set; } = 100;                  // 0-200, default 100
        public int Sub2ROut { get; set; } = 100;                  // 0-200, default 100
        public int LoopOut { get; set; } = 100;                   // 0-200, default 100
        public int RhythmOut { get; set; } = 100;                 // 0-200, default 100
        public int PhonesOut { get; set; } = 100;                 // 0-200, default 100
        public int MasterOut { get; set; } = 100;                 // 0-200, default 100
    }    /// <summary>
    /// External control settings (ECTL)
    /// </summary>
    public class ExternalControlSettings
    {
        public Dictionary<string, object> Settings { get; set; } = new Dictionary<string, object>();
    }

    /// <summary>
    /// Preference settings (PREF)
    /// </summary>
    public class PreferenceSettings
    {
        public Dictionary<string, object> Preferences { get; set; } = new Dictionary<string, object>();
    }    /// <summary>
    /// Main container for all system settings from SYSTEM1.RC0 and SYSTEM2.RC0
    /// </summary>
    public class SystemSettings
    {
        public string FileName { get; set; } = "";
        public SystemSetupSettings Setup { get; set; } = new SystemSetupSettings();
        public string ColorSettings { get; set; } = "";          // COLOR section (could be expanded later)
        public UsbSettings Usb { get; set; } = new UsbSettings();
        public MidiSettings Midi { get; set; } = new MidiSettings();
        public ExternalControlSettings Ictl1 { get; set; } = new ExternalControlSettings();
        public ExternalControlSettings Ictl2 { get; set; } = new ExternalControlSettings();
        public ExternalControlSettings Ictl3 { get; set; } = new ExternalControlSettings();
        public ExternalControlSettings Ectl { get; set; } = new ExternalControlSettings();
        public PreferenceSettings Pref { get; set; } = new PreferenceSettings();
        public SystemInputSettings Input { get; set; } = new SystemInputSettings();
        public SystemOutputSettings Output { get; set; } = new SystemOutputSettings();
        public SystemRoutingSettings Routing { get; set; } = new SystemRoutingSettings();
        public SystemMixerSettings Mixer { get; set; } = new SystemMixerSettings();
        public EqParameter Eq { get; set; } = new EqParameter();  // Reuse existing EqParameter
        public string Count { get; set; } = "001F";               // Count value from system file
    }
}
