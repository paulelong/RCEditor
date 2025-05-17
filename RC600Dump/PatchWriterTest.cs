using System;
using System.IO;
using System.Threading.Tasks;
using RCEditor.Models;
using RCEditor.Models.Services;

namespace RC600Dump
{
    public class PatchWriterTest
    {
        private readonly PatchReader _patchReader;
        private readonly PatchWriter _patchWriter;
        
        public PatchWriterTest()
        {
            _patchReader = new PatchReader();
            _patchWriter = new PatchWriter();
        }
        
        /// <summary>
        /// Tests the patch writer by reading a patch and then writing it back to a new file
        /// </summary>
        /// <param name="sourcePatchPath">Path to the original patch file</param>
        /// <param name="outputDirectoryPath">Directory to write the output file to</param>
        /// <returns>The path to the written file</returns>
        public async Task<string> TestWritePatch(string sourcePatchPath, string outputDirectoryPath)
        {
            Console.WriteLine($"Reading patch from: {sourcePatchPath}");
            
            // Read the source patch
            var patch = await _patchReader.ReadPatchAsync(sourcePatchPath);
            if (patch == null)
            {
                throw new Exception($"Failed to read patch from {sourcePatchPath}");
            }
            
            Console.WriteLine($"Successfully read patch: {patch.Name}");
            
            // Create output filename based on the original
            string fileName = Path.GetFileNameWithoutExtension(sourcePatchPath);
            string outputFilePath = Path.Combine(outputDirectoryPath, $"{fileName}_written.rc0");
            
            // Write the patch to the output file
            Console.WriteLine($"Writing patch to: {outputFilePath}");
            await _patchWriter.WritePatchAsync(patch, outputFilePath);
            
            Console.WriteLine("Patch written successfully.");
            return outputFilePath;
        }
        
        /// <summary>
        /// Compares the contents of two patch files to validate they are equivalent
        /// </summary>
        /// <param name="originalFilePath">Path to the original patch file</param>
        /// <param name="writtenFilePath">Path to the written patch file</param>
        /// <returns>A report of the differences</returns>
        public async Task<string> ComparePatchFiles(string originalFilePath, string writtenFilePath)
        {
            Console.WriteLine("Comparing original and written patch files...");
            
            string originalContent = await File.ReadAllTextAsync(originalFilePath);
            string writtenContent = await File.ReadAllTextAsync(writtenFilePath);
            
            // Read patches with the patch reader to compare their object models
            var originalPatch = await _patchReader.ReadPatchAsync(originalFilePath);
            var writtenPatch = await _patchReader.ReadPatchAsync(writtenFilePath);
            
            if (originalPatch == null || writtenPatch == null)
            {
                return "Error: Could not read one or both patches";
            }
            
            var report = new System.Text.StringBuilder();
            report.AppendLine("Patch Comparison Report");
            report.AppendLine("======================");
            report.AppendLine($"Original file: {originalFilePath}");
            report.AppendLine($"Written file: {writtenFilePath}");
            report.AppendLine();
            
            // Compare basic properties
            report.AppendLine("Basic Properties:");
            report.AppendLine($"Name: {(originalPatch.Name == writtenPatch.Name ? "MATCH" : "DIFFERENT")}");
            report.AppendLine($"Original: \"{originalPatch.Name}\", Written: \"{writtenPatch.Name}\"");
            
            // Compare tracks
            report.AppendLine("\nTracks:");
            for (int i = 0; i < 6; i++)
            {
                var origTrack = originalPatch.Tracks[i];
                var writtenTrack = writtenPatch.Tracks[i];
                
                report.AppendLine($"Track {i+1}:");
                report.AppendLine($"  Level: {(origTrack.Level == writtenTrack.Level ? "MATCH" : "DIFFERENT")} " +
                                  $"(Orig: {origTrack.Level}, Written: {writtenTrack.Level})");
                report.AppendLine($"  Pan: {(origTrack.Pan == writtenTrack.Pan ? "MATCH" : "DIFFERENT")} " +
                                  $"(Orig: {origTrack.Pan}, Written: {writtenTrack.Pan})");
                report.AppendLine($"  Reverse: {(origTrack.Reverse == writtenTrack.Reverse ? "MATCH" : "DIFFERENT")} " +
                                  $"(Orig: {origTrack.Reverse}, Written: {writtenTrack.Reverse})");
            }            
            // Compare Play section
            report.AppendLine("\nPlay Settings:");
            report.AppendLine($"  SingleTrackChange: {(originalPatch.Play.SingleTrackChange == writtenPatch.Play.SingleTrackChange ? "MATCH" : "DIFFERENT")} " +
                              $"(Orig: {originalPatch.Play.SingleTrackChange}, Written: {writtenPatch.Play.SingleTrackChange})");
            // Compare Rec section
            report.AppendLine("\nRec Settings:");
            report.AppendLine($"  RecAction: {(originalPatch.Rec.RecAction == writtenPatch.Rec.RecAction ? "MATCH" : "DIFFERENT")} " +
                              $"(Orig: {originalPatch.Rec.RecAction}, Written: {writtenPatch.Rec.RecAction})");
            report.AppendLine($"  QuantizeEnabled: {(originalPatch.Rec.QuantizeEnabled == writtenPatch.Rec.QuantizeEnabled ? "MATCH" : "DIFFERENT")} " +
                              $"(Orig: {originalPatch.Rec.QuantizeEnabled}, Written: {writtenPatch.Rec.QuantizeEnabled})");
            
            // Compare Rhythm section
            report.AppendLine("\nRhythm Settings:");
            report.AppendLine($"  Pattern: {(originalPatch.Rhythm.Pattern == writtenPatch.Rhythm.Pattern ? "MATCH" : "DIFFERENT")} " +
                              $"(Orig: {originalPatch.Rhythm.Pattern}, Written: {writtenPatch.Rhythm.Pattern})");
            report.AppendLine($"  Variation: {(originalPatch.Rhythm.Variation == writtenPatch.Rhythm.Variation ? "MATCH" : "DIFFERENT")} " +
                              $"(Orig: {originalPatch.Rhythm.Variation}, Written: {writtenPatch.Rhythm.Variation})");
            
            // Compare effects
            report.AppendLine("\nEffects:");
            report.AppendLine($"  InputFX ActiveBank: {(originalPatch.InputFX.ActiveBank == writtenPatch.InputFX.ActiveBank ? "MATCH" : "DIFFERENT")} " +
                              $"(Orig: {originalPatch.InputFX.ActiveBank}, Written: {writtenPatch.InputFX.ActiveBank})");
            report.AppendLine($"  TrackFX ActiveBank: {(originalPatch.TrackFX.ActiveBank == writtenPatch.TrackFX.ActiveBank ? "MATCH" : "DIFFERENT")} " +
                              $"(Orig: {originalPatch.TrackFX.ActiveBank}, Written: {writtenPatch.TrackFX.ActiveBank})");
            
            // Count sections and tags in both files
            var originalSections = CountXmlSections(originalContent);
            var writtenSections = CountXmlSections(writtenContent);
            
            report.AppendLine("\nFile Structure:");
            report.AppendLine($"  Original file size: {originalContent.Length} characters");
            report.AppendLine($"  Written file size: {writtenContent.Length} characters");
            report.AppendLine($"  Original sections: {originalSections}");
            report.AppendLine($"  Written sections: {writtenSections}");
            
            return report.ToString();
        }
        
        /// <summary>
        /// Counts the number of top-level XML-like sections in a string
        /// </summary>
        private int CountXmlSections(string content)
        {
            int count = 0;
            int position = 0;
            
            while (position < content.Length)
            {
                int tagStart = content.IndexOf('<', position);
                if (tagStart == -1) break;
                
                int tagNameEnd = content.IndexOf('>', tagStart);
                if (tagNameEnd == -1) break;
                
                string tagName = content.Substring(tagStart + 1, tagNameEnd - tagStart - 1);
                
                // Skip if this is a closing tag or XML declaration
                if (tagName.StartsWith("/") || tagName.StartsWith("?"))
                {
                    position = tagNameEnd + 1;
                    continue;
                }
                
                // If it contains attributes, extract just the tag name
                if (tagName.Contains(" "))
                {
                    tagName = tagName.Substring(0, tagName.IndexOf(' '));
                }
                
                count++;
                position = tagNameEnd + 1;
            }
            
            return count;
        }
    }
}
