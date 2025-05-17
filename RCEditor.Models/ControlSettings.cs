using System;
using System.Collections.Generic;
using System.Text;

namespace RCEditor.Models
{
    /// <summary>
    /// Represents a generic control parameter with A, B, C values
    /// </summary>
    public class ControlParameter
    {
        /// <summary>
        /// Parameter A value
        /// </summary>
        public int A { get; set; }

        /// <summary>
        /// Parameter B value
        /// </summary>
        public int B { get; set; }

        /// <summary>
        /// Parameter C value
        /// </summary>
        public int C { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ControlParameter()
        {
            A = 0;
            B = 0;
            C = 1; // Default for most control parameters
        }

        /// <summary>
        /// Constructor with values
        /// </summary>
        public ControlParameter(int a, int b, int c)
        {
            A = a;
            B = b;
            C = c;
        }
    }

    /// <summary>
    /// Represents an external control parameter with A, B, C, D values
    /// </summary>
    public class ExtControlParameter : ControlParameter
    {
        /// <summary>
        /// Parameter D value
        /// </summary>
        public int D { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ExtControlParameter() : base()
        {
            D = 1; // Default value
        }

        /// <summary>
        /// Constructor with values
        /// </summary>
        public ExtControlParameter(int a, int b, int c, int d) : base(a, b, c)
        {
            D = d;
        }
    }

    /// <summary>
    /// Represents an assignment with parameters A through J
    /// </summary>
    public class AssignParameter
    {
        /// <summary>
        /// Parameter A value - Source control
        /// </summary>
        public int A { get; set; }

        /// <summary>
        /// Parameter B value - Target control
        /// </summary>
        public int B { get; set; }

        /// <summary>
        /// Parameter C value
        /// </summary>
        public int C { get; set; }

        /// <summary>
        /// Parameter D value
        /// </summary>
        public int D { get; set; }

        /// <summary>
        /// Parameter E value
        /// </summary>
        public int E { get; set; }

        /// <summary>
        /// Parameter F value - Max value (default 127)
        /// </summary>
        public int F { get; set; }

        /// <summary>
        /// Parameter G value
        /// </summary>
        public int G { get; set; }

        /// <summary>
        /// Parameter H value
        /// </summary>
        public int H { get; set; }

        /// <summary>
        /// Parameter I value
        /// </summary>
        public int I { get; set; }

        /// <summary>
        /// Parameter J value - Enabled flag (0=off, 1=on)
        /// </summary>
        public int J { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public AssignParameter()
        {
            A = 0;
            B = 0;
            C = 0;
            D = 0;
            E = 0;
            F = 127;
            G = 0;
            H = 0;
            I = 0;
            J = 1;
        }
    }

    /// <summary>
    /// Class to store input settings with parameters A through M
    /// </summary>
    public class InputParameter
    {
        public int A { get; set; }
        public int B { get; set; }
        public int C { get; set; }
        public int D { get; set; }
        public int E { get; set; }
        public int F { get; set; }
        public int G { get; set; }
        public int H { get; set; }
        public int I { get; set; }
        public int J { get; set; }
        public int K { get; set; }
        public int L { get; set; }
        public int M { get; set; }

        public InputParameter()
        {
            // Default values based on the provided example
            A = 0;
            B = 0;
            C = 0;
            D = 0;
            E = 0;
            F = 1;
            G = 1;
            H = 0;
            I = 0;
            J = 40;
            K = 40;
            L = 40;
            M = 40;
        }
    }

    /// <summary>
    /// Class to store output settings with parameters A through D
    /// </summary>
    public class OutputParameter
    {
        public int A { get; set; }
        public int B { get; set; }
        public int C { get; set; }
        public int D { get; set; }

        public OutputParameter()
        {
            // Default values based on the provided example
            A = 0;
            B = 1;
            C = 1;
            D = 1;
        }
    }

    /// <summary>
    /// Class to store routing settings with parameters A through S
    /// </summary>
    public class RoutingParameter
    {
        public int A { get; set; }
        public int B { get; set; }
        public int C { get; set; }
        public int D { get; set; }
        public int E { get; set; }
        public int F { get; set; }
        public int G { get; set; }
        public int H { get; set; }
        public int I { get; set; }
        public int J { get; set; }
        public int K { get; set; }
        public int L { get; set; }
        public int M { get; set; }
        public int N { get; set; }
        public int O { get; set; }
        public int P { get; set; }
        public int Q { get; set; }
        public int R { get; set; }
        public int S { get; set; }

        public RoutingParameter()
        {
            // Default values based on the provided example
            A = 63;
            B = 63;
            C = 63;
            D = 63;
            E = 63;
            F = 63;
            G = 63;
            H = 127;
            I = 127;
            J = 127;
            K = 127;
            L = 127;
            M = 127;
            N = 127;
            O = 0;
            P = 0;
            Q = 0;
            R = 0;
            S = 1;
        }
    }

    /// <summary>
    /// Class to store mixer settings with parameters A through V
    /// </summary>
    public class MixerParameter
    {
        public int A { get; set; }
        public int B { get; set; }
        public int C { get; set; }
        public int D { get; set; }
        public int E { get; set; }
        public int F { get; set; }
        public int G { get; set; }
        public int H { get; set; }
        public int I { get; set; }
        public int J { get; set; }
        public int K { get; set; }
        public int L { get; set; }
        public int M { get; set; }
        public int N { get; set; }
        public int O { get; set; }
        public int P { get; set; }
        public int Q { get; set; }
        public int R { get; set; }
        public int S { get; set; }
        public int T { get; set; }
        public int U { get; set; }
        public int V { get; set; }

        public MixerParameter()
        {
            // Default values based on the provided example
            A = 100;
            B = 0;
            C = 100;
            D = 0;
            E = 100;
            F = 0;
            G = 100;
            H = 0;
            I = 100;
            J = 0;
            K = 100;
            L = 0;
            M = 100;
            N = 100;
            O = 100;
            P = 100;
            Q = 100;
            R = 100;
            S = 100;
            T = 100;
            U = 100;
            V = 100;
        }
    }

    /// <summary>
    /// Class to store EQ settings with parameters A through L
    /// </summary>
    public class EqParameter
    {
        public int A { get; set; }
        public int B { get; set; }
        public int C { get; set; }
        public int D { get; set; }
        public int E { get; set; }
        public int F { get; set; }
        public int G { get; set; }
        public int H { get; set; }
        public int I { get; set; }
        public int J { get; set; }
        public int K { get; set; }
        public int L { get; set; }

        public EqParameter()
        {
            // Default values based on the provided example
            A = 0;
            B = 20;
            C = 20;
            D = 11;
            E = 0;
            F = 20;
            G = 16;
            H = 0;
            I = 20;
            J = 20;
            K = 0;
            L = 14;
        }
    }

    /// <summary>
    /// Class to store master FX settings with parameters A through C
    /// </summary>
    public class MasterFxParameter
    {
        public int A { get; set; }
        public int B { get; set; }
        public int C { get; set; }

        public MasterFxParameter()
        {
            // Default values based on the provided example
            A = 0;
            B = 0;
            C = 0;
        }
    }

    /// <summary>
    /// Class to store fixed value settings with parameters A and B
    /// </summary>
    public class FixedValueParameter
    {
        public int A { get; set; }
        public int B { get; set; }

        public FixedValueParameter()
        {
            // Default values based on the provided example
            A = 0;
            B = 1;
        }
    }

    /// <summary>
    /// Main class to store all control settings
    /// </summary>
    public class ControlSettings
    {
        /// <summary>
        /// Track controls group 1
        /// </summary>
        public Dictionary<string, ControlParameter> TrackControls1 { get; set; }

        /// <summary>
        /// Track controls group 2
        /// </summary>
        public Dictionary<string, ControlParameter> TrackControls2 { get; set; }

        /// <summary>
        /// Pedal controls group 1
        /// </summary>
        public Dictionary<string, ControlParameter> PedalControls1 { get; set; }

        /// <summary>
        /// Pedal controls group 2
        /// </summary>
        public Dictionary<string, ControlParameter> PedalControls2 { get; set; }

        /// <summary>
        /// Pedal controls group 3
        /// </summary>
        public Dictionary<string, ControlParameter> PedalControls3 { get; set; }

        /// <summary>
        /// External controls
        /// </summary>
        public Dictionary<string, ExtControlParameter> ExternalControls { get; set; }

        /// <summary>
        /// Assignments
        /// </summary>
        public Dictionary<int, AssignParameter> Assignments { get; set; }

        /// <summary>
        /// Input settings
        /// </summary>
        public InputParameter Input { get; set; }

        /// <summary>
        /// Output settings
        /// </summary>
        public OutputParameter Output { get; set; }

        /// <summary>
        /// Routing settings
        /// </summary>
        public RoutingParameter Routing { get; set; }

        /// <summary>
        /// Mixer settings
        /// </summary>
        public MixerParameter Mixer { get; set; }

        /// <summary>
        /// EQ settings for different channels
        /// </summary>
        public Dictionary<string, EqParameter> EqSettings { get; set; }

        /// <summary>
        /// Master FX settings
        /// </summary>
        public MasterFxParameter MasterFx { get; set; }

        /// <summary>
        /// Fixed value settings
        /// </summary>
        public FixedValueParameter FixedValue { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ControlSettings()
        {
            InitializeCollections();
        }

        /// <summary>
        /// Initialize all collections with default values
        /// </summary>
        private void InitializeCollections()
        {
            // Initialize tracks and FX controls
            TrackControls1 = new Dictionary<string, ControlParameter>();
            TrackControls2 = new Dictionary<string, ControlParameter>();
            
            for (int i = 1; i <= 5; i++)
            {
                TrackControls1.Add($"TRACK{i}_FX", new ControlParameter());
                TrackControls1.Add($"TRACK{i}_TRACK", new ControlParameter());
                TrackControls2.Add($"TRACK{i}_FX", new ControlParameter());
                TrackControls2.Add($"TRACK{i}_TRACK", new ControlParameter());
            }

            // Initialize pedal controls
            PedalControls1 = new Dictionary<string, ControlParameter>();
            PedalControls2 = new Dictionary<string, ControlParameter>();
            PedalControls3 = new Dictionary<string, ControlParameter>();
            
            for (int i = 1; i <= 9; i++)
            {
                PedalControls1.Add($"PEDAL{i}", new ControlParameter());
                PedalControls2.Add($"PEDAL{i}", new ControlParameter());
                PedalControls3.Add($"PEDAL{i}", new ControlParameter());
            }

            // Initialize external controls
            ExternalControls = new Dictionary<string, ExtControlParameter>();
            
            for (int i = 1; i <= 4; i++)
            {
                ExternalControls.Add($"CTL{i}", new ExtControlParameter());
            }
            
            ExternalControls.Add("EXP1", new ExtControlParameter());
            ExternalControls.Add("EXP2", new ExtControlParameter());

            // Initialize assignments
            Assignments = new Dictionary<int, AssignParameter>();
            
            for (int i = 1; i <= 16; i++)
            {
                Assignments.Add(i, new AssignParameter());
            }

            // Initialize input, output, routing, mixer
            Input = new InputParameter();
            Output = new OutputParameter();
            Routing = new RoutingParameter();
            Mixer = new MixerParameter();

            // Initialize EQ settings
            EqSettings = new Dictionary<string, EqParameter>
            {
                { "MIC1", new EqParameter() },
                { "MIC2", new EqParameter() },
                { "INST1L", new EqParameter() },
                { "INST1R", new EqParameter() },
                { "INST2L", new EqParameter() },
                { "INST2R", new EqParameter() },
                { "MAINOUTL", new EqParameter() },
                { "MAINOUTR", new EqParameter() },
                { "SUBOUT1L", new EqParameter() },
                { "SUBOUT1R", new EqParameter() },
                { "SUBOUT2L", new EqParameter() },
                { "SUBOUT2R", new EqParameter() }
            };

            // Initialize master FX and fixed value
            MasterFx = new MasterFxParameter();
            FixedValue = new FixedValueParameter();
        }
    }
}
