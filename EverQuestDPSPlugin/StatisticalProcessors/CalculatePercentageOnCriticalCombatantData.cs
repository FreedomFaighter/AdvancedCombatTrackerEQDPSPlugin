
using Advanced_Combat_Tracker;
using System;
using System.Linq;
using EverQuestDPS.Extensions;

namespace EverQuestDPS.StatisticalProcessors
{
    internal static class CalculatePercentageOnCriticalCombatantData
    {

        internal static double GetPercentage(this CombatantData combatantDataTypeData, String attackTypeCritType)
        {
            return combatantDataTypeData.Items.Any() ? combatantDataTypeData.GetDamageByTag(attackTypeCritType) / combatantDataTypeData.GetCount() : double.NaN;
        }

        internal static String GetPercentageAsString(this CombatantData attackTypeData, String attackTypeCritType)
        {
            double percentage = attackTypeData.GetPercentage(attackTypeCritType);
            if (percentage == double.NaN)
                return ActGlobals.ActLocalization.LocalizationStrings[Properties.GenericObjects.specialAttackNoneLocalization].DisplayedText;
            else
                return percentage.ToString();
        }
    }
}
