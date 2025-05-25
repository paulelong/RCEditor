using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RCEditor.Models.Services
{
    /// <summary>
    /// Service to parse system settings from SYSTEM1.RC0 and SYSTEM2.RC0 files
    /// </summary>
    public class SystemSettingsParser
    {
        /// <summary>
        /// Reads and parses a system RC0 file into a SystemSettings object
        /// </summary>
        /// <param name="filePath">Path to the system RC0 file</param>
        /// <returns>Parsed SystemSettings object</returns>
        public async Task<SystemSettings> ReadSystemFileAsync(string filePath)
        {
            var systemSettings = new SystemSettings
            {
                FileName = Path.GetFileName(filePath)
            };

            try
            {
                string content = await File.ReadAllTextAsync(filePath);
                
                // Extract and parse each section
                systemSettings.Setup = ParseSetupSection(content);
                systemSettings.ColorSettings = ExtractSectionContent(content, "COLOR");
                systemSettings.Usb = ParseUsbSection(content);
                systemSettings.Midi = ParseMidiSection(content);                systemSettings.Ictl1 = ParseExternalControlSection(content, "ICTL1");
                systemSettings.Ictl2 = ParseExternalControlSection(content, "ICTL2");
                systemSettings.Ictl3 = ParseExternalControlSection(content, "ICTL3");
                systemSettings.Ectl = ParseExternalControlSection(content, "ECTL");
                systemSettings.Pref = ParsePreferenceSection(content);
                systemSettings.Input = ParseInputSection(content);
                systemSettings.Output = ParseOutputSection(content);
                systemSettings.Routing = ParseRoutingSection(content);
                systemSettings.Mixer = ParseMixerSection(content);
                systemSettings.Eq = ParseEqSection(content);
                systemSettings.Count = ExtractCountValue(content);
            }
            catch (Exception ex)
            {
                // Log error but continue with default values
                Console.WriteLine($"Error parsing system file {filePath}: {ex.Message}");
            }

            return systemSettings;
        }

        /// <summary>
        /// Extracts parameters from a specific XML section
        /// </summary>
        private Dictionary<string, int> ExtractSectionParameters(string content, string sectionName)
        {
            var parameters = new Dictionary<string, int>();
            
            var sectionPattern = $@"<{sectionName}>(.*?)</{sectionName}>";
            var sectionMatch = Regex.Match(content, sectionPattern, RegexOptions.Singleline);
            
            if (sectionMatch.Success)
            {
                string sectionContent = sectionMatch.Groups[1].Value;
                var paramPattern = @"<([A-Z])>([0-9]+)</[A-Z]>";
                var paramMatches = Regex.Matches(sectionContent, paramPattern);
                
                foreach (Match match in paramMatches)
                {
                    string paramName = match.Groups[1].Value;
                    if (int.TryParse(match.Groups[2].Value, out int paramValue))
                    {
                        parameters[paramName] = paramValue;
                    }
                }
            }
            
            return parameters;
        }

        /// <summary>
        /// Extracts raw content from a specific XML section
        /// </summary>
        private string ExtractSectionContent(string content, string sectionName)
        {
            var sectionPattern = $@"<{sectionName}>(.*?)</{sectionName}>";
            var sectionMatch = Regex.Match(content, sectionPattern, RegexOptions.Singleline);
            return sectionMatch.Success ? sectionMatch.Groups[1].Value : "";
        }

        /// <summary>
        /// Extracts the Count value from the system file
        /// </summary>
        private string ExtractCountValue(string content)
        {
            var countMatch = Regex.Match(content, @"<Count>([A-F0-9]+)</Count>");
            return countMatch.Success ? countMatch.Groups[1].Value : "001F";
        }        /// <summary>
        /// Parses the SETUP section
        /// </summary>
        private SystemSetupSettings ParseSetupSection(string content)
        {
            var parameters = ExtractSectionParameters(content, "SETUP");
            var setup = new SystemSetupSettings();
            
            // Map parameters based on RC600Param.md documentation
            if (parameters.ContainsKey("A")) setup.Contrast = Math.Max(1, Math.Min(10, parameters["A"]));
            // Add more parameter mappings as needed based on the documentation
            
            return setup;
        }

        /// <summary>
        /// Parses the USB section
        /// </summary>
        private UsbSettings ParseUsbSection(string content)
        {
            var parameters = ExtractSectionParameters(content, "USB");
            var usb = new UsbSettings();
            
            // Map parameters based on RC600Param.md documentation
            if (parameters.ContainsKey("A")) usb.Storage = parameters["A"] != 0;
            if (parameters.ContainsKey("B")) usb.AudioMode = parameters["B"] == 0 ? "GENERIC" : "VENDOR";
            if (parameters.ContainsKey("C")) 
            {
                usb.Routing = parameters["C"] switch
                {
                    0 => "LINE OUT",
                    1 => "SUB MIX",
                    2 => "LOOP IN",
                    _ => "LINE OUT"
                };
            }
            if (parameters.ContainsKey("D")) usb.InputLevel = Math.Max(0, Math.Min(200, parameters["D"]));
            if (parameters.ContainsKey("E")) usb.OutputLevel = Math.Max(0, Math.Min(200, parameters["E"]));
            
            return usb;
        }

        /// <summary>
        /// Parses the MIDI section
        /// </summary>
        private MidiSettings ParseMidiSection(string content)
        {
            var parameters = ExtractSectionParameters(content, "MIDI");
            var midi = new MidiSettings();
            
            // Map parameters based on RC600Param.md documentation
            if (parameters.ContainsKey("A")) midi.RxChCtl = Math.Max(1, Math.Min(16, parameters["A"]));
            if (parameters.ContainsKey("B")) midi.RxChRhythm = Math.Max(1, Math.Min(16, parameters["B"]));
            if (parameters.ContainsKey("C")) midi.RxChVoice = Math.Max(1, Math.Min(16, parameters["C"]));
            if (parameters.ContainsKey("D")) midi.TxCh = parameters["D"].ToString();
            if (parameters.ContainsKey("E")) 
            {
                midi.SyncClock = parameters["E"] switch
                {
                    0 => "AUTO",
                    1 => "INTERNAL",
                    2 => "MIDI",
                    3 => "USB",
                    _ => "AUTO"
                };
            }
            if (parameters.ContainsKey("F")) midi.ClockOut = parameters["F"] != 0;
            if (parameters.ContainsKey("G")) 
            {
                midi.StartSync = parameters["G"] switch
                {
                    0 => "OFF",
                    1 => "ALL",
                    2 => "RHYTHM",
                    _ => "ALL"
                };
            }
            if (parameters.ContainsKey("H")) midi.PcOut = parameters["H"] != 0;
            if (parameters.ContainsKey("I")) 
            {
                midi.Thru = parameters["I"] switch
                {
                    0 => "OFF",
                    1 => "MIDI OUT",
                    2 => "USB OUT",
                    3 => "USB & MIDI",
                    _ => "OFF"
                };
            }
            
            return midi;
        }        /// <summary>
        /// Parses external control sections (ECTL, ICTL1, ICTL2, ICTL3)
        /// </summary>
        private ExternalControlSettings ParseExternalControlSection(string content, string sectionName)
        {
            var parameters = ExtractSectionParameters(content, sectionName);
            var settings = new ExternalControlSettings();
            
            foreach (var param in parameters)
            {
                settings.Settings[param.Key] = param.Value;
            }
            
            return settings;
        }

        /// <summary>
        /// Parses the PREF (Preferences) section
        /// </summary>
        private PreferenceSettings ParsePreferenceSection(string content)
        {
            var parameters = ExtractSectionParameters(content, "PREF");
            var pref = new PreferenceSettings();
            
            foreach (var param in parameters)
            {
                pref.Preferences[param.Key] = param.Value;
            }
            
            return pref;
        }        /// <summary>
        /// Parses the INPUT section
        /// </summary>
        private SystemInputSettings ParseInputSection(string content)
        {
            var parameters = ExtractSectionParameters(content, "INPUT");
            var input = new SystemInputSettings();
            
            // Parse input setup parameters
            if (parameters.ContainsKey("A")) input.Setup.PhantomMic1 = parameters["A"] != 0;
            if (parameters.ContainsKey("B")) input.Setup.PhantomMic2 = parameters["B"] != 0;
            // Add more parameter mappings as needed
            
            return input;
        }

        /// <summary>
        /// Parses the OUTPUT section
        /// </summary>
        private SystemOutputSettings ParseOutputSection(string content)
        {
            var parameters = ExtractSectionParameters(content, "OUTPUT");
            var output = new SystemOutputSettings();
            
            // Parse output setup parameters
            if (parameters.ContainsKey("A")) 
            {
                output.Setup.OutputKnob = parameters["A"] switch
                {
                    0 => "ALL",
                    1 => "MASTER",
                    2 => "PHONES",
                    3 => "OFF",
                    _ => "ALL"
                };
            }
            // Add more parameter mappings as needed
            
            return output;
        }

        /// <summary>
        /// Parses the ROUTING section
        /// </summary>
        private SystemRoutingSettings ParseRoutingSection(string content)
        {
            var parameters = ExtractSectionParameters(content, "ROUTING");
            var routing = new SystemRoutingSettings();
            
            // Parse routing parameters - this would need to map the track routing matrix
            // The implementation would depend on how the routing is encoded in the RC0 file
            
            return routing;
        }        /// <summary>
        /// Parses the MIXER section
        /// </summary>
        private SystemMixerSettings ParseMixerSection(string content)
        {
            var parameters = ExtractSectionParameters(content, "MIXER");
            var mixer = new SystemMixerSettings();
            
            // Map mixer parameters
            if (parameters.ContainsKey("A")) mixer.Mic1In = Math.Max(0, Math.Min(200, parameters["A"]));
            if (parameters.ContainsKey("B")) mixer.Mic2In = Math.Max(0, Math.Min(200, parameters["B"]));
            if (parameters.ContainsKey("C")) mixer.Inst1LIn = Math.Max(0, Math.Min(200, parameters["C"]));
            if (parameters.ContainsKey("D")) mixer.Inst1RIn = Math.Max(0, Math.Min(200, parameters["D"]));
            if (parameters.ContainsKey("E")) mixer.Inst2LIn = Math.Max(0, Math.Min(200, parameters["E"]));
            if (parameters.ContainsKey("F")) mixer.Inst2RIn = Math.Max(0, Math.Min(200, parameters["F"]));
            // Add more parameter mappings as needed
            
            return mixer;
        }

        /// <summary>
        /// Parses the EQ section using existing EqParameter model
        /// </summary>
        private EqParameter ParseEqSection(string content)
        {
            var parameters = ExtractSectionParameters(content, "EQ");
            var eq = new EqParameter();
            
            // Map EQ parameters using existing EqParameter structure
            if (parameters.ContainsKey("A")) eq.A = parameters["A"];
            if (parameters.ContainsKey("B")) eq.B = parameters["B"];
            if (parameters.ContainsKey("C")) eq.C = parameters["C"];
            if (parameters.ContainsKey("D")) eq.D = parameters["D"];
            if (parameters.ContainsKey("E")) eq.E = parameters["E"];
            if (parameters.ContainsKey("F")) eq.F = parameters["F"];
            if (parameters.ContainsKey("G")) eq.G = parameters["G"];
            if (parameters.ContainsKey("H")) eq.H = parameters["H"];
            if (parameters.ContainsKey("I")) eq.I = parameters["I"];
            if (parameters.ContainsKey("J")) eq.J = parameters["J"];
            if (parameters.ContainsKey("K")) eq.K = parameters["K"];
            if (parameters.ContainsKey("L")) eq.L = parameters["L"];
            
            return eq;
        }
    }
}
