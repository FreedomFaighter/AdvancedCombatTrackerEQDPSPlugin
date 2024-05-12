using System;

namespace EverQuestDPS.Enums
{
    [Flags]
    internal enum SummonedEntity
    {
        Pet = 1,
        Warder = 2,
        Ward = 4,
        Familiar = 8
    }

    internal static class SummonedEntityExtensions{
        internal static int GetIntFromDamageFromSummonedEntity(this SummonedEntity summonedEntity)
        {
            return (int)summonedEntity;
        }

        internal static SummonedEntity GenerateSummonedEntityEnum(bool pet, bool ward, bool warder, bool familiar)
        {
            SummonedEntity summonedEntity = new SummonedEntity();
            if (pet)
            {
                summonedEntity |= SummonedEntity.Pet;
            }
            if(ward)
            {
                summonedEntity |= SummonedEntity.Ward;
            }
            if (warder)
            {
                summonedEntity |= SummonedEntity.Warder;
            }
            if (familiar)
            {
                summonedEntity |= SummonedEntity.Familiar;
            }

            return summonedEntity;
        }
    }

   
}
