namespace EverQuestDPSPlugin
{
    public enum EverQuestSwingType : int
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
        Ward = 2048,
        PetMelee = Pet | Melee,
        PetNonMelee = Pet | NonMelee,
        WardInstantHealing = InstantHealing | Ward,
        WardHealOverTime = HealOverTime | Ward,
        WarderMelee = Warder | Melee,
        WarderNonMelee = Warder | NonMelee,
        WarderDirectDamageSpell = Warder | DirectDamageSpell,
        WarderDamageOverTimeSpell = Warder | DamageOverTimeSpell,
    }

    public static class EverQuestSwingTypeExtensions
    {
        internal static int GetEverQuestSwingTypeExtensionIntValue(this EverQuestSwingType type)
        {
            return (int)type;
        }

        public static EverQuestSwingType CharacterPossesiveTypeAttack(this EverQuestSwingType possessiveOf, EverQuestSwingType everQuestAttackType)
        {
            switch (possessiveOf)
            {
                case EverQuestSwingType.Pet:
                    return EverQuestSwingType.Pet | everQuestAttackType;
                case EverQuestSwingType.Warder:
                    return EverQuestSwingType.Warder | everQuestAttackType;
                case EverQuestSwingType.Ward:
                    return EverQuestSwingType.Ward | everQuestAttackType;
                default:
                    return everQuestAttackType;
            }
        }
    }
}
