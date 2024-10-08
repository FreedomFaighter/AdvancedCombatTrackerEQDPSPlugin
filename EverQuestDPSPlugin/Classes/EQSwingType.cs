using System;

namespace EverQuestDPS.Classes
{
    public class EQSwingType
    {
        static readonly int melee = 1;
        static readonly int nonMelee = 2;
        static readonly int healing = 4;
        static readonly int spell = 8;
        static readonly int bane = 16;
        static readonly int damageShield = 32;
        static readonly int instant = 64;
        static readonly int overTime = 128;
        static readonly int healingOverTime = healing + overTime;
        static readonly int instantHealing = healing + instant;
        static readonly int directDamageSpell = spell + instant;
        static readonly int spellOverTime = spell + overTime;

        public int Melee {  get { return melee; } }
        public int NonMelee { get {return nonMelee; } }
        public int Healing { get { return healing; } }
        public int Spell { get { return spell; } }
        public int DamageShield { get { return damageShield; } }
        public int InstantHealing { get {return instantHealing; } }
        public int HealingOverTime { get { return healingOverTime; } }
        public int DirectDamageSpell { get { return directDamageSpell; } }
        public int SpellOverTime { get {return spellOverTime; } }
    }
}
