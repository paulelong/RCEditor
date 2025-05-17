using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace RCEditor.Models.Services
{
    /// <summary>
    /// Service to parse and process control settings from RC0 files
    /// </summary>
    public class ControlSettingsParser
    {
        /// <summary>
        /// Parse control settings from RC0 XML content
        /// </summary>
        /// <param name="xmlContent">The XML content to parse</param>
        /// <returns>A ControlSettings object populated with the parsed data</returns>
        public static ControlSettings ParseControlSettings(string xmlContent)
        {
            var settings = new ControlSettings();
            
            // Split the input into individual XML sections
            var sectionRegex = new Regex(@"<([A-Za-z0-9_]+)>\s*((?:<[A-Z]>[0-9]+</[A-Z]>\s*)+)</\1>", RegexOptions.Singleline);
            var matches = sectionRegex.Matches(xmlContent);

            foreach (Match match in matches)
            {
                string sectionName = match.Groups[1].Value;
                string sectionContent = match.Groups[2].Value;
                
                // Process each section based on its name
                ProcessSection(settings, sectionName, sectionContent);
            }
            
            return settings;
        }

        /// <summary>
        /// Process a specific XML section and update the settings object
        /// </summary>
        /// <param name="settings">The settings object to update</param>
        /// <param name="sectionName">The name of the section</param>
        /// <param name="sectionContent">The XML content of the section</param>
        private static void ProcessSection(ControlSettings settings, string sectionName, string sectionContent)
        {
            // Extract parameters using regex
            var paramRegex = new Regex(@"<([A-Z])>([0-9]+)</\1>");
            var paramMatches = paramRegex.Matches(sectionContent);
            
            // Create a dictionary to hold all the parameters
            var parameters = new Dictionary<char, int>();
            foreach (Match paramMatch in paramMatches)
            {
                char paramName = paramMatch.Groups[1].Value[0]; // Get the parameter letter
                int paramValue = int.Parse(paramMatch.Groups[2].Value); // Get the parameter value
                parameters[paramName] = paramValue;
            }
            
            // Process based on section type
            if (sectionName.StartsWith("ICTL1_TRACK"))
            {
                ProcessTrackControl(settings.TrackControls1, sectionName.Substring(6), parameters);
            }
            else if (sectionName.StartsWith("ICTL2_TRACK"))
            {
                ProcessTrackControl(settings.TrackControls2, sectionName.Substring(6), parameters);
            }
            else if (sectionName.StartsWith("ICTL1_PEDAL"))
            {
                ProcessPedalControl(settings.PedalControls1, sectionName.Substring(6), parameters);
            }
            else if (sectionName.StartsWith("ICTL2_PEDAL"))
            {
                ProcessPedalControl(settings.PedalControls2, sectionName.Substring(6), parameters);
            }
            else if (sectionName.StartsWith("ICTL3_PEDAL"))
            {
                ProcessPedalControl(settings.PedalControls3, sectionName.Substring(6), parameters);
            }
            else if (sectionName.StartsWith("ECTL_"))
            {
                ProcessExternalControl(settings.ExternalControls, sectionName.Substring(5), parameters);
            }
            else if (sectionName.StartsWith("ASSIGN"))
            {
                ProcessAssignment(settings.Assignments, sectionName, parameters);
            }
            else if (sectionName == "INPUT")
            {
                ProcessInput(settings.Input, parameters);
            }
            else if (sectionName == "OUTPUT")
            {
                ProcessOutput(settings.Output, parameters);
            }
            else if (sectionName == "ROUTING")
            {
                ProcessRouting(settings.Routing, parameters);
            }
            else if (sectionName == "MIXER")
            {
                ProcessMixer(settings.Mixer, parameters);
            }
            else if (sectionName.StartsWith("EQ_"))
            {
                ProcessEq(settings.EqSettings, sectionName.Substring(3), parameters);
            }
            else if (sectionName == "MASTER_FX")
            {
                ProcessMasterFx(settings.MasterFx, parameters);
            }
            else if (sectionName == "FIXED_VALUE")
            {
                ProcessFixedValue(settings.FixedValue, parameters);
            }
        }

        /// <summary>
        /// Process track control parameters
        /// </summary>
        private static void ProcessTrackControl(Dictionary<string, ControlParameter> controls, string name, Dictionary<char, int> parameters)
        {
            if (controls.ContainsKey(name))
            {
                var control = controls[name];
                if (parameters.ContainsKey('A')) control.A = parameters['A'];
                if (parameters.ContainsKey('B')) control.B = parameters['B'];
                if (parameters.ContainsKey('C')) control.C = parameters['C'];
            }
        }

        /// <summary>
        /// Process pedal control parameters
        /// </summary>
        private static void ProcessPedalControl(Dictionary<string, ControlParameter> controls, string name, Dictionary<char, int> parameters)
        {
            if (controls.ContainsKey(name))
            {
                var control = controls[name];
                if (parameters.ContainsKey('A')) control.A = parameters['A'];
                if (parameters.ContainsKey('B')) control.B = parameters['B'];
                if (parameters.ContainsKey('C')) control.C = parameters['C'];
            }
        }

        /// <summary>
        /// Process external control parameters
        /// </summary>
        private static void ProcessExternalControl(Dictionary<string, ExtControlParameter> controls, string name, Dictionary<char, int> parameters)
        {
            if (controls.ContainsKey(name))
            {
                var control = controls[name];
                if (parameters.ContainsKey('A')) control.A = parameters['A'];
                if (parameters.ContainsKey('B')) control.B = parameters['B'];
                if (parameters.ContainsKey('C')) control.C = parameters['C'];
                if (parameters.ContainsKey('D')) control.D = parameters['D'];
            }
        }

        /// <summary>
        /// Process assignment parameters
        /// </summary>
        private static void ProcessAssignment(Dictionary<int, AssignParameter> assignments, string name, Dictionary<char, int> parameters)
        {
            // Extract the assignment number
            if (int.TryParse(name.Replace("ASSIGN", ""), out int assignNumber) && assignments.ContainsKey(assignNumber))
            {
                var assign = assignments[assignNumber];
                if (parameters.ContainsKey('A')) assign.A = parameters['A'];
                if (parameters.ContainsKey('B')) assign.B = parameters['B'];
                if (parameters.ContainsKey('C')) assign.C = parameters['C'];
                if (parameters.ContainsKey('D')) assign.D = parameters['D'];
                if (parameters.ContainsKey('E')) assign.E = parameters['E'];
                if (parameters.ContainsKey('F')) assign.F = parameters['F'];
                if (parameters.ContainsKey('G')) assign.G = parameters['G'];
                if (parameters.ContainsKey('H')) assign.H = parameters['H'];
                if (parameters.ContainsKey('I')) assign.I = parameters['I'];
                if (parameters.ContainsKey('J')) assign.J = parameters['J'];
            }
        }

        /// <summary>
        /// Process input parameters
        /// </summary>
        private static void ProcessInput(InputParameter input, Dictionary<char, int> parameters)
        {
            if (parameters.ContainsKey('A')) input.A = parameters['A'];
            if (parameters.ContainsKey('B')) input.B = parameters['B'];
            if (parameters.ContainsKey('C')) input.C = parameters['C'];
            if (parameters.ContainsKey('D')) input.D = parameters['D'];
            if (parameters.ContainsKey('E')) input.E = parameters['E'];
            if (parameters.ContainsKey('F')) input.F = parameters['F'];
            if (parameters.ContainsKey('G')) input.G = parameters['G'];
            if (parameters.ContainsKey('H')) input.H = parameters['H'];
            if (parameters.ContainsKey('I')) input.I = parameters['I'];
            if (parameters.ContainsKey('J')) input.J = parameters['J'];
            if (parameters.ContainsKey('K')) input.K = parameters['K'];
            if (parameters.ContainsKey('L')) input.L = parameters['L'];
            if (parameters.ContainsKey('M')) input.M = parameters['M'];
        }

        /// <summary>
        /// Process output parameters
        /// </summary>
        private static void ProcessOutput(OutputParameter output, Dictionary<char, int> parameters)
        {
            if (parameters.ContainsKey('A')) output.A = parameters['A'];
            if (parameters.ContainsKey('B')) output.B = parameters['B'];
            if (parameters.ContainsKey('C')) output.C = parameters['C'];
            if (parameters.ContainsKey('D')) output.D = parameters['D'];
        }

        /// <summary>
        /// Process routing parameters
        /// </summary>
        private static void ProcessRouting(RoutingParameter routing, Dictionary<char, int> parameters)
        {
            if (parameters.ContainsKey('A')) routing.A = parameters['A'];
            if (parameters.ContainsKey('B')) routing.B = parameters['B'];
            if (parameters.ContainsKey('C')) routing.C = parameters['C'];
            if (parameters.ContainsKey('D')) routing.D = parameters['D'];
            if (parameters.ContainsKey('E')) routing.E = parameters['E'];
            if (parameters.ContainsKey('F')) routing.F = parameters['F'];
            if (parameters.ContainsKey('G')) routing.G = parameters['G'];
            if (parameters.ContainsKey('H')) routing.H = parameters['H'];
            if (parameters.ContainsKey('I')) routing.I = parameters['I'];
            if (parameters.ContainsKey('J')) routing.J = parameters['J'];
            if (parameters.ContainsKey('K')) routing.K = parameters['K'];
            if (parameters.ContainsKey('L')) routing.L = parameters['L'];
            if (parameters.ContainsKey('M')) routing.M = parameters['M'];
            if (parameters.ContainsKey('N')) routing.N = parameters['N'];
            if (parameters.ContainsKey('O')) routing.O = parameters['O'];
            if (parameters.ContainsKey('P')) routing.P = parameters['P'];
            if (parameters.ContainsKey('Q')) routing.Q = parameters['Q'];
            if (parameters.ContainsKey('R')) routing.R = parameters['R'];
            if (parameters.ContainsKey('S')) routing.S = parameters['S'];
        }

        /// <summary>
        /// Process mixer parameters
        /// </summary>
        private static void ProcessMixer(MixerParameter mixer, Dictionary<char, int> parameters)
        {
            if (parameters.ContainsKey('A')) mixer.A = parameters['A'];
            if (parameters.ContainsKey('B')) mixer.B = parameters['B'];
            if (parameters.ContainsKey('C')) mixer.C = parameters['C'];
            if (parameters.ContainsKey('D')) mixer.D = parameters['D'];
            if (parameters.ContainsKey('E')) mixer.E = parameters['E'];
            if (parameters.ContainsKey('F')) mixer.F = parameters['F'];
            if (parameters.ContainsKey('G')) mixer.G = parameters['G'];
            if (parameters.ContainsKey('H')) mixer.H = parameters['H'];
            if (parameters.ContainsKey('I')) mixer.I = parameters['I'];
            if (parameters.ContainsKey('J')) mixer.J = parameters['J'];
            if (parameters.ContainsKey('K')) mixer.K = parameters['K'];
            if (parameters.ContainsKey('L')) mixer.L = parameters['L'];
            if (parameters.ContainsKey('M')) mixer.M = parameters['M'];
            if (parameters.ContainsKey('N')) mixer.N = parameters['N'];
            if (parameters.ContainsKey('O')) mixer.O = parameters['O'];
            if (parameters.ContainsKey('P')) mixer.P = parameters['P'];
            if (parameters.ContainsKey('Q')) mixer.Q = parameters['Q'];
            if (parameters.ContainsKey('R')) mixer.R = parameters['R'];
            if (parameters.ContainsKey('S')) mixer.S = parameters['S'];
            if (parameters.ContainsKey('T')) mixer.T = parameters['T'];
            if (parameters.ContainsKey('U')) mixer.U = parameters['U'];
            if (parameters.ContainsKey('V')) mixer.V = parameters['V'];
        }

        /// <summary>
        /// Process EQ parameters
        /// </summary>
        private static void ProcessEq(Dictionary<string, EqParameter> eqSettings, string name, Dictionary<char, int> parameters)
        {
            if (eqSettings.ContainsKey(name))
            {
                var eq = eqSettings[name];
                if (parameters.ContainsKey('A')) eq.A = parameters['A'];
                if (parameters.ContainsKey('B')) eq.B = parameters['B'];
                if (parameters.ContainsKey('C')) eq.C = parameters['C'];
                if (parameters.ContainsKey('D')) eq.D = parameters['D'];
                if (parameters.ContainsKey('E')) eq.E = parameters['E'];
                if (parameters.ContainsKey('F')) eq.F = parameters['F'];
                if (parameters.ContainsKey('G')) eq.G = parameters['G'];
                if (parameters.ContainsKey('H')) eq.H = parameters['H'];
                if (parameters.ContainsKey('I')) eq.I = parameters['I'];
                if (parameters.ContainsKey('J')) eq.J = parameters['J'];
                if (parameters.ContainsKey('K')) eq.K = parameters['K'];
                if (parameters.ContainsKey('L')) eq.L = parameters['L'];
            }
        }

        /// <summary>
        /// Process master FX parameters
        /// </summary>
        private static void ProcessMasterFx(MasterFxParameter masterFx, Dictionary<char, int> parameters)
        {
            if (parameters.ContainsKey('A')) masterFx.A = parameters['A'];
            if (parameters.ContainsKey('B')) masterFx.B = parameters['B'];
            if (parameters.ContainsKey('C')) masterFx.C = parameters['C'];
        }

        /// <summary>
        /// Process fixed value parameters
        /// </summary>
        private static void ProcessFixedValue(FixedValueParameter fixedValue, Dictionary<char, int> parameters)
        {
            if (parameters.ContainsKey('A')) fixedValue.A = parameters['A'];
            if (parameters.ContainsKey('B')) fixedValue.B = parameters['B'];
        }
        
        /// <summary>
        /// Converts control settings to RC0 XML format
        /// </summary>
        /// <param name="settings">The control settings to convert</param>
        /// <returns>A string containing the RC0 XML representation</returns>
        public static string ConvertToXml(ControlSettings settings)
        {
            StringBuilder sb = new StringBuilder();
            
            // Add track controls group 1
            foreach (var kvp in settings.TrackControls1)
            {
                AppendControlParameter(sb, $"ICTL1_{kvp.Key}", kvp.Value);
            }
            
            // Add track controls group 2
            foreach (var kvp in settings.TrackControls2)
            {
                AppendControlParameter(sb, $"ICTL2_{kvp.Key}", kvp.Value);
            }
            
            // Add pedal controls groups
            foreach (var kvp in settings.PedalControls1)
            {
                AppendControlParameter(sb, $"ICTL1_{kvp.Key}", kvp.Value);
            }
            
            foreach (var kvp in settings.PedalControls2)
            {
                AppendControlParameter(sb, $"ICTL2_{kvp.Key}", kvp.Value);
            }
            
            foreach (var kvp in settings.PedalControls3)
            {
                AppendControlParameter(sb, $"ICTL3_{kvp.Key}", kvp.Value);
            }
            
            // Add external controls
            foreach (var kvp in settings.ExternalControls)
            {
                AppendExtControlParameter(sb, $"ECTL_{kvp.Key}", kvp.Value);
            }
            
            // Add assignments
            foreach (var kvp in settings.Assignments)
            {
                AppendAssignParameter(sb, $"ASSIGN{kvp.Key}", kvp.Value);
            }
            
            // Add input, output, routing, mixer
            AppendInputParameter(sb, "INPUT", settings.Input);
            AppendOutputParameter(sb, "OUTPUT", settings.Output);
            AppendRoutingParameter(sb, "ROUTING", settings.Routing);
            AppendMixerParameter(sb, "MIXER", settings.Mixer);
            
            // Add EQ settings
            foreach (var kvp in settings.EqSettings)
            {
                AppendEqParameter(sb, $"EQ_{kvp.Key}", kvp.Value);
            }
            
            // Add master FX and fixed value
            AppendMasterFxParameter(sb, "MASTER_FX", settings.MasterFx);
            AppendFixedValueParameter(sb, "FIXED_VALUE", settings.FixedValue);
            
            return sb.ToString();
        }
        
        /// <summary>
        /// Append a control parameter to the string builder
        /// </summary>
        private static void AppendControlParameter(StringBuilder sb, string name, ControlParameter param)
        {
            sb.AppendLine($"<{name}>");
            sb.AppendLine($"\t<A>{param.A}</A>");
            sb.AppendLine($"\t<B>{param.B}</B>");
            sb.AppendLine($"\t<C>{param.C}</C>");
            sb.AppendLine($"</{name}>");
        }
        
        /// <summary>
        /// Append an external control parameter to the string builder
        /// </summary>
        private static void AppendExtControlParameter(StringBuilder sb, string name, ExtControlParameter param)
        {
            sb.AppendLine($"<{name}>");
            sb.AppendLine($"\t<A>{param.A}</A>");
            sb.AppendLine($"\t<B>{param.B}</B>");
            sb.AppendLine($"\t<C>{param.C}</C>");
            sb.AppendLine($"\t<D>{param.D}</D>");
            sb.AppendLine($"</{name}>");
        }
        
        /// <summary>
        /// Append an assignment parameter to the string builder
        /// </summary>
        private static void AppendAssignParameter(StringBuilder sb, string name, AssignParameter param)
        {
            sb.AppendLine($"<{name}>");
            sb.AppendLine($"\t<A>{param.A}</A>");
            sb.AppendLine($"\t<B>{param.B}</B>");
            sb.AppendLine($"\t<C>{param.C}</C>");
            sb.AppendLine($"\t<D>{param.D}</D>");
            sb.AppendLine($"\t<E>{param.E}</E>");
            sb.AppendLine($"\t<F>{param.F}</F>");
            sb.AppendLine($"\t<G>{param.G}</G>");
            sb.AppendLine($"\t<H>{param.H}</H>");
            sb.AppendLine($"\t<I>{param.I}</I>");
            sb.AppendLine($"\t<J>{param.J}</J>");
            sb.AppendLine($"</{name}>");
        }
        
        /// <summary>
        /// Append an input parameter to the string builder
        /// </summary>
        private static void AppendInputParameter(StringBuilder sb, string name, InputParameter param)
        {
            sb.AppendLine($"<{name}>");
            sb.AppendLine($"\t<A>{param.A}</A>");
            sb.AppendLine($"\t<B>{param.B}</B>");
            sb.AppendLine($"\t<C>{param.C}</C>");
            sb.AppendLine($"\t<D>{param.D}</D>");
            sb.AppendLine($"\t<E>{param.E}</E>");
            sb.AppendLine($"\t<F>{param.F}</F>");
            sb.AppendLine($"\t<G>{param.G}</G>");
            sb.AppendLine($"\t<H>{param.H}</H>");
            sb.AppendLine($"\t<I>{param.I}</I>");
            sb.AppendLine($"\t<J>{param.J}</J>");
            sb.AppendLine($"\t<K>{param.K}</K>");
            sb.AppendLine($"\t<L>{param.L}</L>");
            sb.AppendLine($"\t<M>{param.M}</M>");
            sb.AppendLine($"</{name}>");
        }
        
        /// <summary>
        /// Append an output parameter to the string builder
        /// </summary>
        private static void AppendOutputParameter(StringBuilder sb, string name, OutputParameter param)
        {
            sb.AppendLine($"<{name}>");
            sb.AppendLine($"\t<A>{param.A}</A>");
            sb.AppendLine($"\t<B>{param.B}</B>");
            sb.AppendLine($"\t<C>{param.C}</C>");
            sb.AppendLine($"\t<D>{param.D}</D>");
            sb.AppendLine($"</{name}>");
        }
        
        /// <summary>
        /// Append a routing parameter to the string builder
        /// </summary>
        private static void AppendRoutingParameter(StringBuilder sb, string name, RoutingParameter param)
        {
            sb.AppendLine($"<{name}>");
            sb.AppendLine($"\t<A>{param.A}</A>");
            sb.AppendLine($"\t<B>{param.B}</B>");
            sb.AppendLine($"\t<C>{param.C}</C>");
            sb.AppendLine($"\t<D>{param.D}</D>");
            sb.AppendLine($"\t<E>{param.E}</E>");
            sb.AppendLine($"\t<F>{param.F}</F>");
            sb.AppendLine($"\t<G>{param.G}</G>");
            sb.AppendLine($"\t<H>{param.H}</H>");
            sb.AppendLine($"\t<I>{param.I}</I>");
            sb.AppendLine($"\t<J>{param.J}</J>");
            sb.AppendLine($"\t<K>{param.K}</K>");
            sb.AppendLine($"\t<L>{param.L}</L>");
            sb.AppendLine($"\t<M>{param.M}</M>");
            sb.AppendLine($"\t<N>{param.N}</N>");
            sb.AppendLine($"\t<O>{param.O}</O>");
            sb.AppendLine($"\t<P>{param.P}</P>");
            sb.AppendLine($"\t<Q>{param.Q}</Q>");
            sb.AppendLine($"\t<R>{param.R}</R>");
            sb.AppendLine($"\t<S>{param.S}</S>");
            sb.AppendLine($"</{name}>");
        }
        
        /// <summary>
        /// Append a mixer parameter to the string builder
        /// </summary>
        private static void AppendMixerParameter(StringBuilder sb, string name, MixerParameter param)
        {
            sb.AppendLine($"<{name}>");
            sb.AppendLine($"\t<A>{param.A}</A>");
            sb.AppendLine($"\t<B>{param.B}</B>");
            sb.AppendLine($"\t<C>{param.C}</C>");
            sb.AppendLine($"\t<D>{param.D}</D>");
            sb.AppendLine($"\t<E>{param.E}</E>");
            sb.AppendLine($"\t<F>{param.F}</F>");
            sb.AppendLine($"\t<G>{param.G}</G>");
            sb.AppendLine($"\t<H>{param.H}</H>");
            sb.AppendLine($"\t<I>{param.I}</I>");
            sb.AppendLine($"\t<J>{param.J}</J>");
            sb.AppendLine($"\t<K>{param.K}</K>");
            sb.AppendLine($"\t<L>{param.L}</L>");
            sb.AppendLine($"\t<M>{param.M}</M>");
            sb.AppendLine($"\t<N>{param.N}</N>");
            sb.AppendLine($"\t<O>{param.O}</O>");
            sb.AppendLine($"\t<P>{param.P}</P>");
            sb.AppendLine($"\t<Q>{param.Q}</Q>");
            sb.AppendLine($"\t<R>{param.R}</R>");
            sb.AppendLine($"\t<S>{param.S}</S>");
            sb.AppendLine($"\t<T>{param.T}</T>");
            sb.AppendLine($"\t<U>{param.U}</U>");
            sb.AppendLine($"\t<V>{param.V}</V>");
            sb.AppendLine($"</{name}>");
        }
        
        /// <summary>
        /// Append an EQ parameter to the string builder
        /// </summary>
        private static void AppendEqParameter(StringBuilder sb, string name, EqParameter param)
        {
            sb.AppendLine($"<{name}>");
            sb.AppendLine($"\t<A>{param.A}</A>");
            sb.AppendLine($"\t<B>{param.B}</B>");
            sb.AppendLine($"\t<C>{param.C}</C>");
            sb.AppendLine($"\t<D>{param.D}</D>");
            sb.AppendLine($"\t<E>{param.E}</E>");
            sb.AppendLine($"\t<F>{param.F}</F>");
            sb.AppendLine($"\t<G>{param.G}</G>");
            sb.AppendLine($"\t<H>{param.H}</H>");
            sb.AppendLine($"\t<I>{param.I}</I>");
            sb.AppendLine($"\t<J>{param.J}</J>");
            sb.AppendLine($"\t<K>{param.K}</K>");
            sb.AppendLine($"\t<L>{param.L}</L>");
            sb.AppendLine($"</{name}>");
        }
        
        /// <summary>
        /// Append a master FX parameter to the string builder
        /// </summary>
        private static void AppendMasterFxParameter(StringBuilder sb, string name, MasterFxParameter param)
        {
            sb.AppendLine($"<{name}>");
            sb.AppendLine($"\t<A>{param.A}</A>");
            sb.AppendLine($"\t<B>{param.B}</B>");
            sb.AppendLine($"\t<C>{param.C}</C>");
            sb.AppendLine($"</{name}>");
        }
        
        /// <summary>
        /// Append a fixed value parameter to the string builder
        /// </summary>
        private static void AppendFixedValueParameter(StringBuilder sb, string name, FixedValueParameter param)
        {
            sb.AppendLine($"<{name}>");
            sb.AppendLine($"\t<A>{param.A}</A>");
            sb.AppendLine($"\t<B>{param.B}</B>");
            sb.AppendLine($"</{name}>");
        }
    }
}
