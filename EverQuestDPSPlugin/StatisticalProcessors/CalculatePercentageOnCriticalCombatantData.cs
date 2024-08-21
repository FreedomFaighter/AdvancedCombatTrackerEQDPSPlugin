
using Advanced_Combat_Tracker;
using System.Collections.Generic;
using System;
using System.Linq;

namespace EverQuestDPS.StatisticalProcessors
{
    internal static class CalculatePercentageOnCriticalCombatantData
    {

        internal static int GetCount(this CombatantData combatantDataTypeData)
        {
            return combatantDataTypeData.Items.Sum((kvp) =>
            {
                return combatantDataTypeData.Items[kvp.Key].GetCount();
            });
        }

        internal static int GetCountCritType(this CombatantData combatantDataTypeData, String attackTypeCritType)
        {
            return combatantDataTypeData.Items.Sum(
                (kvp) => combatantDataTypeData.Items[kvp.Key].Items.Sum((kvpDamageType)
                => combatantDataTypeData.Items[kvp.Key].Items[kvpDamageType.Key].Items.Sum((masterSwing) =>
                    (Boolean)masterSwing.Tags[attackTypeCritType] ? 1 : 0)));
        }

        internal static double GetPercentage(this CombatantData combatantDataTypeData, String attackTypeCritType)
        {
            int count = combatantDataTypeData.GetCount();
            return (count > 0) ? combatantDataTypeData.GetCountCritType(attackTypeCritType) / count : double.NaN;
        }

        internal static String GetPercentageAsString(this CombatantData attackTypeData, String attackTypeCritType)
        {
            double percentage = attackTypeData.GetPercentage(attackTypeCritType);
            if (percentage == double.NaN)
                return ActGlobals.ActLocalization.LocalizationStrings[Properties.EQDPSPlugin.specialAttackNoneLocalization].DisplayedText;
            else
                return percentage.ToString();
        }
    }
}
