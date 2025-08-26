using System;

namespace EverQuestDPS.Enums
{
    [Flags]
    public enum EQSwingType : uint
    {
        None = 0,
        Melee = 1,
        NonMelee = 2,
        Healing = 4,
        Spell = 8,
        Bane = 16,
        DamageShield = 32,
        Instant = 64,
        OverTime = 128,
        HealingOverTime = Healing | OverTime,
        InstantHealing = Healing | Instant,
        DirectDamageSpell = Spell | Instant,
        SpellOverTime = Spell | OverTime
    }
}
