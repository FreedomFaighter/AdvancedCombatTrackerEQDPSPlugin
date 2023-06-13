//Spell damage saves currently known
//Chromatic is a combination of these enum values and may be Fire | Cold = 3

namespace EverQuestDPSPlugin.Enums
{
    internal enum SpellDamageSave
    {
        Fire = 1,
        Cold = 2,
        Poison = 4,
        Disease = 8,
        Magic = 16,
        Corruption = 32,
        Unresistable = 64
    }
}
