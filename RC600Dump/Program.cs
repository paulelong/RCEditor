using RCEditor.Models;
using RCEditor.Models.Services;
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

                // Check if the provided path is a file instead of a directory
                bool isFilePath = File.Exists(commandLineOptions.PatchDirectory) && 
                                 Path.GetExtension(commandLineOptions.PatchDirectory).ToLower() == ".rc0";
                
                if (isFilePath)
                {
                    // If it's a file, extract the directory and set the patch number
                    string filePath = commandLineOptions.PatchDirectory;
                    string fileName = Path.GetFileName(filePath);
                    
                    // Extract patch number and variation from filename (e.g., "MEMORY042A.RC0")
                    if (fileName.StartsWith("MEMORY", StringComparison.OrdinalIgnoreCase) && 
                        fileName.Length >= 11)
                    {
                        string patchNumberStr = fileName.Substring(6, 3);
                        if (int.TryParse(patchNumberStr, out int patchNumber))
                        {
                            commandLineOptions.PatchNumber = patchNumber;
                            
                            if (fileName.Length >= 10)
                            {
                                commandLineOptions.PatchVariation = fileName[9];
                            }
                        }
                    }
                    
                    // Use the directory containing the file
                    commandLineOptions.PatchDirectory = Path.GetDirectoryName(filePath);
                }

                // Validate directory exists
                if (!Directory.Exists(commandLineOptions.PatchDirectory))
                {
                    Console.Error.WriteLine($"Error: Directory \"{commandLineOptions.PatchDirectory}\" not found or inaccessible.");
                    return 1;
                }
                  // If we're testing the writer, check for output directory
                if (commandLineOptions.TestWriter && string.IsNullOrWhiteSpace(commandLineOptions.OutputDirectory))
                {
                    // Default to the patch directory if not specified
                    commandLineOptions.OutputDirectory = commandLineOptions.PatchDirectory;
                }
                
                if (commandLineOptions.TestWriter && !string.IsNullOrWhiteSpace(commandLineOptions.OutputDirectory) && 
                    !Directory.Exists(commandLineOptions.OutputDirectory))
                {
                    // Create the output directory if it doesn't exist
                    Directory.CreateDirectory(commandLineOptions.OutputDirectory);
                }

                var patchReader = new PatchReader();

                if (commandLineOptions.TestWriter)
                {
                    // Test the patch writer
                    await TestPatchWriter(commandLineOptions);
                }
                else if (commandLineOptions.PatchNumber.HasValue)
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

            // Check if the directory already contains the full patch file or if it's in a DATA subdirectory
            var patchFilePath = Path.Combine(patchDirectory, patchFileName);
            if (!File.Exists(patchFilePath))
            {
                patchFilePath = Path.Combine(patchDirectory, "DATA", patchFileName);
            }

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
        }        private static void ShowUsage()
        {
            Console.WriteLine("RC-600 Patch Data Dump Tool");
            Console.WriteLine("===========================");
            Console.WriteLine();
            Console.WriteLine("Usage:");
            Console.WriteLine("  RC600Dump.exe [options] <PatchDirectory|PatchFile> [<MemoryNumber>[A|B]]");
            Console.WriteLine();
            Console.WriteLine("Arguments:");
            Console.WriteLine("  <PatchDirectory|PatchFile> Path to the directory containing the RC-600 patch data files");
            Console.WriteLine("                          (e.g. the Roland/Data folder from the RC-600's USB drive)");
            Console.WriteLine("                          OR path to a specific .RC0 patch file to analyze");
            Console.WriteLine("  <MemoryNumber>[A|B]     Optional. The specific patch number (memory slot) to dump in full.");
            Console.WriteLine("                          If omitted, a summary of all patches will be displayed.");
            Console.WriteLine("                          Optionally append A or B to specify the variation (defaults to A).");
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine("  -p, --patch <Number>    Specifies the patch number to dump in full");
            Console.WriteLine("  -w, --write             Test the patch writer by reading and writing patches");
            Console.WriteLine("  -o, --output <dir>      Specify output directory for written patches");
            Console.WriteLine("  -h, --help              Displays this help message");
            Console.WriteLine();
            Console.WriteLine("Examples:");            Console.WriteLine("  RC600Dump.exe \"D:\\RC600\\Backup\\Roland\\Data\"");
            Console.WriteLine("  RC600Dump.exe --patch 42 \"D:\\RC600\\Backup\\Roland\\Data\"");
            Console.WriteLine("  RC600Dump.exe \"D:\\RC600\\Backup\\Roland\\Data\" 42B");
            Console.WriteLine("  RC600Dump.exe \"D:\\RC600\\Backup\\Roland\\Data\\MEMORY042A.RC0\"   Specify a file directly");
            Console.WriteLine("  RC600Dump.exe -w \"D:\\RC600\\Backup\\Roland\\Data\"      Test writing all patches");
            Console.WriteLine("  RC600Dump.exe -w -p 42 -o \"D:\\Output\" \"D:\\RC600\"   Test writing patch 42");
        }

        private static async Task TestPatchWriter(CommandLineOptions options)
        {
            var writerTester = new PatchWriterTest();
            string inputDirectory = options.PatchDirectory!;
            string outputDirectory = options.OutputDirectory!;
            
            Console.WriteLine($"Test mode: Reading patches from {inputDirectory}");
            Console.WriteLine($"Writing test output to {outputDirectory}");
              // Get list of RC0 files in directory
            var rc0Files = new List<string>();
            
            // Check for RC0 files in the main directory
            rc0Files.AddRange(Directory.GetFiles(inputDirectory, "*.rc0", SearchOption.TopDirectoryOnly));
            
            // Also check the DATA subdirectory if it exists
            var dataDir = Path.Combine(inputDirectory, "DATA");
            if (Directory.Exists(dataDir))
            {
                rc0Files.AddRange(Directory.GetFiles(dataDir, "*.rc0", SearchOption.TopDirectoryOnly));
            }
            
            if (rc0Files.Count == 0)
            {
                Console.WriteLine("No .rc0 files found in the specified directory or its DATA subdirectory.");
                return;
            }            // Filter by patch number if specified
            if (options.PatchNumber.HasValue)
            {
                var patchVariationToUse = options.PatchVariation ?? 'A';
                string prefix = $"MEMORY{options.PatchNumber.Value:D3}{patchVariationToUse}";
                
                rc0Files = rc0Files.Where(f => Path.GetFileNameWithoutExtension(f).Equals(prefix, StringComparison.OrdinalIgnoreCase)).ToList();
                
                if (rc0Files.Count == 0)
                {
                    Console.WriteLine($"No matching patch files found for {prefix}");
                    return;
                }
            }
            
            Console.WriteLine($"Found {rc0Files.Count} patch files to process");
            
            // Process each file
            int successCount = 0;
            int failCount = 0;
            
            foreach (var rc0File in rc0Files)
            {
                try
                {
                    string fileName = Path.GetFileName(rc0File);
                    Console.WriteLine($"\nProcessing file: {fileName}");
                    
                    // Test writing the patch
                    string outputPath = await writerTester.TestWritePatch(rc0File, outputDirectory);
                    
                    // Compare the original and written files
                    string report = await writerTester.ComparePatchFiles(rc0File, outputPath);
                    
                    // Write the report to a file
                    string reportFile = Path.Combine(outputDirectory, Path.GetFileNameWithoutExtension(rc0File) + "_report.txt");
                    await File.WriteAllTextAsync(reportFile, report);
                    
                    Console.WriteLine($"Comparison report written to: {reportFile}");
                    successCount++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing {Path.GetFileName(rc0File)}: {ex.Message}");
                    failCount++;
                }
            }
            
            Console.WriteLine($"\nSummary: Processed {successCount} files successfully, {failCount} failures");
        }
    }
}
