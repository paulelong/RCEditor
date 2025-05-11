namespace RC600Dump.Services
{
    public class CommandLineOptions
    {
        public bool ShowHelp { get; set; }
        public string? PatchDirectory { get; set; }
        public int? PatchNumber { get; set; }
        public char? PatchVariation { get; set; }
    }

    public static class CommandLineParser
    {
        public static CommandLineOptions Parse(string[] args)
        {
            var options = new CommandLineOptions();

            for (int i = 0; i < args.Length; i++)
            {
                var arg = args[i];

                if (arg.StartsWith("-") || arg.StartsWith("--"))
                {
                    // Handle options
                    switch (arg.ToLower())
                    {
                        case "-h":
                        case "--help":
                            options.ShowHelp = true;
                            break;

                        case "-p":
                        case "--patch":
                            if (i + 1 < args.Length)
                            {
                                ParsePatchNumberAndVariation(args[++i], options);
                            }
                            break;

                        default:
                            // Unrecognized option, ignore
                            break;
                    }
                }
                else
                {
                    // Handle positional arguments
                    if (options.PatchDirectory == null)
                    {
                        options.PatchDirectory = arg;
                    }
                    else if (options.PatchNumber == null)
                    {
                        // Second positional arg is the patch number
                        ParsePatchNumberAndVariation(arg, options);
                    }
                }
            }

            return options;
        }

        private static void ParsePatchNumberAndVariation(string arg, CommandLineOptions options)
        {
            // Handle patch number with optional variation (e.g., "42" or "42B")
            string patchNumberStr = arg;
            char? variation = null;

            // Check if the last character is A or B
            if (patchNumberStr.Length > 0)
            {
                char lastChar = patchNumberStr[patchNumberStr.Length - 1];
                if (lastChar == 'A' || lastChar == 'a' || lastChar == 'B' || lastChar == 'b')
                {
                    variation = char.ToUpper(lastChar);
                    patchNumberStr = patchNumberStr.Substring(0, patchNumberStr.Length - 1);
                }
            }

            if (int.TryParse(patchNumberStr, out int patchNumber))
            {
                options.PatchNumber = patchNumber;
                options.PatchVariation = variation;
            }
        }
    }
}