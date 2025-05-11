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
        public Dictionary<string, double> Parameters { get; set; }

        public EffectSlot()
        {
            Parameters = new Dictionary<string, double>();
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
    }

    public class EffectBank
    {
        public Dictionary<int, EffectSlot> Slots { get; set; } = new Dictionary<int, EffectSlot>();

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

        public EffectBanks()
        {
            Banks["A"] = new EffectBank();
            Banks["B"] = new EffectBank();
            Banks["C"] = new EffectBank();
            Banks["D"] = new EffectBank();
        }
    }

    
}