using System;

namespace EverQuestDPS.Enums
{
    [Flags]
    public enum EverQuestSwingType : int
    {
        Melee = 1,
        NonMelee = 2,
        Healing = 4,
        Spell = 8,
        Bane = 16,
        Instant = 32,
        OverTime = 64,
        DamageShield = 128,
    }

    internal static class EverQuestSwingTypeExtensions
    {
        internal static int GetEverQuestSwingTypeExtensionIntValue(this EverQuestSwingType type)
        {
            return (int)type;
        }

        internal static EverQuestSwingType GetFromIntEverQuestSwingType(this int intValue)
        {
            if (intValue > byte.MaxValue)
                throw new ArgumentOutOfRangeException($@"Value of passed int {intValue}");
            return (EverQuestSwingType)intValue;
        }
    }
}
