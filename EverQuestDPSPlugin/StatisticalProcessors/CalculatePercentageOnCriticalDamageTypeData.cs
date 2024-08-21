using Advanced_Combat_Tracker;
using System;
using System.Linq;

namespace EverQuestDPS.StatisticalProcessors
{
    internal static class CalculatePercentageOnCriticalDamageTypeData
    {
        internal static int GetCount(this DamageTypeData damageTypeData)
        {
            return damageTypeData.Items.Sum((kvp) =>
            {
                return damageTypeData.Items[kvp.Key].GetCount();
            });
        }

        internal static int GetCountCritType(this DamageTypeData damageTypeData, String damageTypeDataCritType)
        {
            return damageTypeData.Items.Sum((kvp) => damageTypeData.Items[kvp.Key].GetCountCritType(damageTypeDataCritType));
        }

        internal static double GetPercentage(this DamageTypeData damageTypeData, String damageTypeDataCritType)
        {            
            int count = damageTypeData.GetCount();
            return count > 0 ? damageTypeData.GetCountCritType(damageTypeDataCritType) / count : double.NaN;
        }

        internal static String GetPercentageAsString(this DamageTypeData damageTypeData, String damageTypeDataCritType)
        {
            double percentage = damageTypeData.GetPercentage(damageTypeDataCritType);
            if (percentage == double.NaN)
                return ActGlobals.ActLocalization.LocalizationStrings[Properties.EQDPSPlugin.specialAttackNoneLocalization].DisplayedText;
            else
                return percentage.ToString();
        }
    }
}
