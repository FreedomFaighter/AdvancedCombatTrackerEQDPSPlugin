using System.ComponentModel;

namespace EverQuestDPS.Enums
{
    [Description("EverQuest Swing Types for ACT Plugin")]
    public enum EQSwingType
    {
        [Description("None")]
        None = 0,
        [Description("Melee")]
        Melee = 1,
        [Description("Non Melee")]
        NonMelee = 2,
        [Description("Heal")]
        Heal = 4,
        [Description("Spell")]
        Spell = 8,
        [Description("Bane")]
        Bane = 16,
    }
}
