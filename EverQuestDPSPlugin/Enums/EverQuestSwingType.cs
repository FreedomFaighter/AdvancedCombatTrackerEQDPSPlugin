//Currently attack types in view of being parsed

namespace EverQuestDPSPlugin.Enums
{
    internal enum EverQuestSwingType : int
    {
        Melee = 1,
        NonMelee = 2,
        InstantHealing = 4,
        HealOverTime = 8,
        Bane = 16,
        Pet = 32,
        Warder = 64,
        Incoming = 128,
        Outgoing = 256,
        DirectDamageSpell = 512,
        DamageOverTimeSpell = 1024,
        PetMelee = Pet | Melee,
        PetNonMelee = Pet | NonMelee,
        Ward = 2048,
        WardInstantHealing = InstantHealing | Ward,
        WardHealOverTime = HealOverTime | Ward
    }

    internal static class EverQuestSwingTypeExtensions
    {
        internal static int GetEverQuestSwingTypeExtensionIntValue(this EverQuestSwingType type)
        {
            return (int)type;
        }
    }
}
