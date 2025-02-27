namespace EverQuestDPS.Classes
{
    public static class EQSwingType
    {
        public const int None = 0;
        public const int Melee = 1;
        public const int NonMelee = 2;
        public const int Healing = 4;
        public const int Spell = 8;
        public const int Bane = 16;
        public const int DamageShield = 32;
        public const int Instant = 64;
        public const int OverTime = 128;
        public const int HealingOverTime = Healing + OverTime;
        public const int InstantHealing = Healing + Instant;
        public const int DirectDamageSpell = Spell + Instant;
        public const int SpellOverTime = Spell + OverTime;
    }
}
