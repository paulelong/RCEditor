using System.Collections.Generic;

namespace RCEditor.Models
{
    public class EffectSlot
    {
        public enum Category { InputFX, TrackFX }
        public Category SlotCategory { get; set; }
        public string Type { get; set; }
        public bool EnabledByDefault { get; set; }
        public SwitchModeEnum SwitchMode { get; set; }
        public string Target { get; set; }
        public Dictionary<string, double> Parameters { get; set; }

        public EffectSlot()
        {
            Parameters = new Dictionary<string, double>();
            EnabledByDefault = true;
            SwitchMode = SwitchModeEnum.Toggle;
        }
    }

    public class EffectBank
    {
        public List<EffectSlot> Slots { get; set; } = new List<EffectSlot>();

        public EffectBank()
        {
            // Each bank has 4 slots
            for (int i = 0; i < 4; i++)
            {
                Slots.Add(new EffectSlot());
            }
        }
    }

    public class EffectBanks
    {
        public EffectBank BankA { get; set; } = new EffectBank();
        public EffectBank BankB { get; set; } = new EffectBank();
        public EffectBank BankC { get; set; } = new EffectBank();
        public EffectBank BankD { get; set; } = new EffectBank();
    }
}