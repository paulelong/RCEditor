using RCEditor.Models;
using RC600Dump.Services;

namespace RC600Dump
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            try
            {
                var commandLineOptions = CommandLineParser.Parse(args);

                if (commandLineOptions.ShowHelp || string.IsNullOrWhiteSpace(commandLineOptions.PatchDirectory))
                {
                    ShowUsage();
                    return commandLineOptions.ShowHelp ? 0 : 1;
                }

                // Validate directory exists
                if (!Directory.Exists(commandLineOptions.PatchDirectory))
                {
                    Console.Error.WriteLine($"Error: Directory \"{commandLineOptions.PatchDirectory}\" not found or inaccessible.");
                    return 1;
                }

                var patchReader = new PatchReader();

                if (commandLineOptions.PatchNumber.HasValue)
                {
                    // Full patch dump mode
                    await DumpSinglePatch(patchReader, commandLineOptions.PatchDirectory, 
                        commandLineOptions.PatchNumber.Value, commandLineOptions.PatchVariation);
                }
                else
                {
                    // Summary dump mode
                    await DumpAllPatches(patchReader, commandLineOptions.PatchDirectory);
                }

                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                return 1;
            }
        }

        private static async Task DumpSinglePatch(PatchReader patchReader, string patchDirectory, 
            int patchNumber, char? patchVariation)
        {
            var patchVariationToUse = patchVariation ?? 'A';
            var patchFileName = $"MEMORY{patchNumber:D3}{patchVariationToUse}.RC0";
            var patchFilePath = Path.Combine(patchDirectory, "DATA", patchFileName);

            if (!File.Exists(patchFilePath))
            {
                Console.Error.WriteLine($"Error: Patch data for patch {patchNumber}{patchVariationToUse} was not found in the directory.");
                return;
            }

            try
            {
                var patch = await patchReader.ReadPatchAsync(patchFilePath);
                if (patch != null)
                {
                    var formatter = new PatchFormatter();
                    Console.WriteLine(formatter.FormatPatchDetails(patchNumber, patchVariationToUse, patch));
                }
                else
                {
                    Console.Error.WriteLine($"Error: Could not parse patch data from file {patchFileName}.");
                }
            }
            catch (IOException ex)
            {
                Console.Error.WriteLine($"Error: Could not read patch file \"{patchFileName}\" (access denied or file corrupt).");
                Console.Error.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }

        private static async Task DumpAllPatches(PatchReader patchReader, string patchDirectory)
        {
            var patchesPath = Path.Combine(patchDirectory, "DATA");
            
            if (!Directory.Exists(patchesPath))
            {
                Console.Error.WriteLine($"Error: Expected DATA subdirectory not found in \"{patchDirectory}\".");
                return;
            }

            // Find all patch files with pattern MEMORY*A.RC0
            var patchFiles = Directory.GetFiles(patchesPath, "MEMORY???A.RC0")
                .OrderBy(file => file)
                .ToList();

            if (patchFiles.Count == 0)
            {
                Console.Error.WriteLine($"Error: No RC-600 patch data files were found in \"{patchDirectory}\".");
                return;
            }

            var formatter = new PatchFormatter();
            
            foreach (var patchFile in patchFiles)
            {
                try
                {
                    var patch = await patchReader.ReadPatchAsync(patchFile);
                    if (patch != null)
                    {
                        // Extract patch number from filename (e.g., "MEMORY042A.RC0" -> 42)
                        var fileName = Path.GetFileName(patchFile);
                        var patchNumberStr = fileName.Substring(6, 3);
                        if (int.TryParse(patchNumberStr, out int patchNumber))
                        {
                            Console.WriteLine(formatter.FormatPatchSummary(patchNumber, patch));
                        }
                    }
                }
                catch (Exception ex)
                {
                    var fileName = Path.GetFileName(patchFile);
                    Console.Error.WriteLine($"Warning: Could not read patch file \"{fileName}\": {ex.Message}");
                }
            }
        }

        private static void ShowUsage()
        {
            Console.WriteLine("RC-600 Patch Data Dump Tool");
            Console.WriteLine("===========================");
            Console.WriteLine();
            Console.WriteLine("Usage:");
            Console.WriteLine("  RC600Dump.exe [options] <PatchDirectory> [<MemoryNumber>[A|B]]");
            Console.WriteLine();
            Console.WriteLine("Arguments:");
            Console.WriteLine("  <PatchDirectory>        Path to the directory containing the RC-600 patch data files");
            Console.WriteLine("                          (e.g. the Roland/Data folder from the RC-600's USB drive)");
            Console.WriteLine("  <MemoryNumber>[A|B]     Optional. The specific patch number (memory slot) to dump in full.");
            Console.WriteLine("                          If omitted, a summary of all patches will be displayed.");
            Console.WriteLine("                          Optionally append A or B to specify the variation (defaults to A).");
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine("  -p, --patch <Number>    Specifies the patch number to dump in full");
            Console.WriteLine("  -h, --help              Displays this help message");
            Console.WriteLine();
            Console.WriteLine("Examples:");
            Console.WriteLine("  RC600Dump.exe \"D:\\RC600\\Backup\\Roland\\Data\"");
            Console.WriteLine("  RC600Dump.exe --patch 42 \"D:\\RC600\\Backup\\Roland\\Data\"");
            Console.WriteLine("  RC600Dump.exe \"D:\\RC600\\Backup\\Roland\\Data\" 42B");
        }
    }
}
