using Advanced_Combat_Tracker;
using System;
using EverQuestDPS.Enums;

namespace EverQuestDPS.Extensions
{
    internal static class MasterSwingExtensions
    {
        internal static object GetTag(this MasterSwing masterSwing, string tagName)
        {
            return masterSwing.Tags[tagName];
        }

        internal static bool GetSpecialStringStruct(this MasterSwing masterSwing, Specials specials)
        {
            return ((Specials)masterSwing.GetTag(Properties.GenericObjects.SpecialStringTag)).HasFlag(specials);
        }

        internal static bool GetTagAndSwingType(this MasterSwing masterSwing, String tagName, EQSwingType eqSwingType)
        {
            return masterSwing.Tags[EverQuestDPS.Properties.GenericObjects.OutgoingTag].Equals(tagName) && ((EQSwingType)masterSwing.SwingType).HasFlag(eqSwingType);
        }

        internal static bool GetPetAttack(this MasterSwing masterSwing)
        {
            return (bool)(masterSwing.GetTag(Properties.GenericObjects.pet));
        }

        internal static bool GetWardHealing(this MasterSwing masterSwing)
        {
            return masterSwing.Tags[EverQuestDPS.Properties.GenericObjects.OutgoingTag].Equals(Properties.GenericObjects.ward) && ((EQSwingType)masterSwing.SwingType).HasFlag(EQSwingType.Healing);
        }

        internal static bool GetWarderDamage(this MasterSwing masterSwing)
        {
            return masterSwing.Tags[EverQuestDPS.Properties.GenericObjects.OutgoingTag].Equals(Properties.GenericObjects.warder);
        }
    }
}
