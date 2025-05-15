using System.Collections.Generic;

namespace RCEditor.Models
{
    public class EffectSlot
    {
        public enum Category { InputFX, TrackFX }
        public enum BankType { None, A, B, C, D }

        public Category SlotCategory { get; set; }
        public BankType Bank { get; set; }
        public string Type { get; set; }
        public bool Enabled { get; set; }  // Adding this based on PatchFormatter usage
        public bool EnabledByDefault { get; set; }
        public SwitchModeEnum SwitchMode { get; set; }
        public string Target { get; set; }
        public Dictionary<string, int> Parameters { get; set; }
        
        // New property to store all effect settings
        public Dictionary<int, EffectSettings> AllEffectSettings { get; set; }
        
        public EffectSlot()
        {
            Parameters = new Dictionary<string, int>();
            AllEffectSettings = new Dictionary<int, EffectSettings>();
            EnabledByDefault = true;
            Enabled = true;
            SwitchMode = SwitchModeEnum.Toggle;
            Type = string.Empty;
            Target = string.Empty;
            EffectName = string.Empty;
            Bank = BankType.None;
        }
        
        public int EffectType { get; set; }
        public string EffectName { get; set; }
        
        // Get the current effect's settings
        public EffectSettings GetCurrentEffectSettings()
        {
            if (AllEffectSettings.ContainsKey(EffectType))
            {
                return AllEffectSettings[EffectType];
            }
            
            // If no settings exist for the current effect type, create new settings
            var settings = new EffectSettings
            {
                EffectType = this.EffectType,
                EffectName = this.EffectName,
                Parameters = new Dictionary<string, int>(this.Parameters)
            };
            
            AllEffectSettings[EffectType] = settings;
            return settings;
        }
        
        // Apply settings when switching to a different effect
        public void SwitchToEffect(int effectType, string effectName)
        {
            // Store current settings if we have an effect type set
            if (EffectType > 0 && !AllEffectSettings.ContainsKey(EffectType))
            {
                AllEffectSettings[EffectType] = new EffectSettings
                {
                    EffectType = this.EffectType,
                    EffectName = this.EffectName,
                    Parameters = new Dictionary<string, int>(this.Parameters)
                };
            }
            
            // Update to new effect
            EffectType = effectType;
            EffectName = effectName;
            
            // Load parameters for the new effect if they exist
            if (AllEffectSettings.ContainsKey(effectType))
            {
                Parameters.Clear();
                foreach (var param in AllEffectSettings[effectType].Parameters)
                {
                    Parameters[param.Key] = param.Value;
                }
            }
            else
            {
                // Initialize with empty parameters
                Parameters.Clear();
            }
        }
    }

    public class EffectBank
    {
        public Dictionary<int, EffectSlot> Slots { get; set; } = new Dictionary<int, EffectSlot>();
        public bool Enabled { get; set; } = true;
        public Dictionary<string, int> Parameters { get; set; } = new Dictionary<string, int>();

        public EffectBank()
        {
            // Each bank has 4 slots
            for (int i = 1; i <= 4; i++)
            {
                Slots[i] = new EffectSlot();
            }
        }
    }

    public class EffectBanks
    {
        public Dictionary<string, EffectBank> Banks { get; set; } = new Dictionary<string, EffectBank>();
        public EffectSlot.BankType ActiveBank { get; set; } = EffectSlot.BankType.A;  // Default to Bank A if not specified

        public EffectBanks()
        {
            Banks["A"] = new EffectBank();
            Banks["B"] = new EffectBank();
            Banks["C"] = new EffectBank();
            Banks["D"] = new EffectBank();
        }
    }

    // New class to store settings for a specific effect
    public class EffectSettings
    {
        public int EffectType { get; set; }
        public string EffectName { get; set; } = string.Empty;
        public Dictionary<string, int> Parameters { get; set; } = new Dictionary<string, int>();
    }
}