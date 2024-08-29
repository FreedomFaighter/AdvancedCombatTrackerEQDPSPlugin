using Advanced_Combat_Tracker;
using System;
using System.Linq;
using EverQuestDPS.Extensions;

namespace EverQuestDPS.StatisticalProcessors
{
    internal static class CalculatePercentageOnCriticalDamageTypeData
    {
        internal static double GetPercentage(this DamageTypeData damageTypeData, String damageTypeDataCritType)
        {            
            return damageTypeData.Items.Any() ? damageTypeData.GetDamageOfTag(damageTypeDataCritType) / damageTypeData.GetCount() : double.NaN;
        }

        internal static String GetPercentageAsString(this DamageTypeData damageTypeData, String damageTypeDataCritType)
        {
            double percentage = damageTypeData.GetPercentage(damageTypeDataCritType);
            if (percentage == double.NaN)
                return ActGlobals.ActLocalization.LocalizationStrings[Properties.GenericObjects.specialAttackNoneLocalization].DisplayedText;
            else
                return percentage.ToString();
        }
    }
}
