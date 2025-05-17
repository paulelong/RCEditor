using System;
using System.IO;
using System.Threading.Tasks;

namespace RCEditor.Models.Services
{
    public class PatchExportService
    {
        private readonly PatchWriter _patchWriter;
        
        public PatchExportService()
        {
            _patchWriter = new PatchWriter();
        }
        
        /// <summary>
        /// Exports a memory patch to the specified file path
        /// </summary>
        /// <param name="patch">The memory patch to export</param>
        /// <param name="filePath">The file path to save to</param>
        /// <returns>True if successful, false otherwise</returns>
        public async Task<bool> ExportPatchAsync(MemoryPatch patch, string filePath)
        {
            try
            {
                await _patchWriter.WritePatchAsync(patch, filePath);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error exporting patch: {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// Validates that a generated RC0 file matches the structure of an original
        /// By comparing the key elements of both files
        /// </summary>
        public async Task<bool> ValidateExportedPatchAsync(string originalFilePath, string exportedFilePath)
        {
            try
            {
                if (!File.Exists(originalFilePath) || !File.Exists(exportedFilePath))
                {
                    return false;
                }
                
                // Read both files
                string originalContent = await File.ReadAllTextAsync(originalFilePath);
                string exportedContent = await File.ReadAllTextAsync(exportedFilePath);
                
                // Perform validation checks
                bool nameMatches = DoesNameSectionMatch(originalContent, exportedContent);
                bool structureMatches = DoesStructureMatch(originalContent, exportedContent);
                
                return nameMatches && structureMatches;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error validating exported patch: {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// Checks if the name sections of two RC0 files match
        /// </summary>
        private bool DoesNameSectionMatch(string originalContent, string exportedContent)
        {
            // Extract the name section from both files
            var originalNameSection = ExtractSection(originalContent, "NAME");
            var exportedNameSection = ExtractSection(exportedContent, "NAME");
            
            return !string.IsNullOrEmpty(originalNameSection) && 
                   !string.IsNullOrEmpty(exportedNameSection) && 
                   originalNameSection == exportedNameSection;
        }
        
        /// <summary>
        /// Checks if the overall structure of two RC0 files match
        /// </summary>
        private bool DoesStructureMatch(string originalContent, string exportedContent)
        {
            // Check for the presence of key sections in both files
            bool memSectionPresent = originalContent.Contains("<mem") && exportedContent.Contains("<mem");
            bool ifxSectionPresent = originalContent.Contains("<ifx") && exportedContent.Contains("<ifx");
            bool tfxSectionPresent = originalContent.Contains("<tfx") && exportedContent.Contains("<tfx");
            
            return memSectionPresent && ifxSectionPresent && tfxSectionPresent;
        }
          /// <summary>
        /// Extracts a section from an RC0 file content
        /// </summary>
        private string? ExtractSection(string content, string sectionName)
        {
            int startIndex = content.IndexOf($"<{sectionName}>");
            if (startIndex == -1) return null;
            
            int endIndex = content.IndexOf($"</{sectionName}>", startIndex);
            if (endIndex == -1) return null;
            
            return content.Substring(startIndex, endIndex - startIndex + sectionName.Length + 3);
        }
    }
}
