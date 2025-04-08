using Advanced_Combat_Tracker;
using System;
using EverQuestDPS.Enums;
using EverQuestDPS.Extensions;

namespace EverQuestDPS.StatisticalProcessors
{
    internal static class CalculatePercentageOnCriticalAttackType
    {
        internal static double GetPercentage(this AttackType attackTypeData, Specials attackTypeCritType)
        {
            return attackTypeData.GetCount() > 0 ? attackTypeData.GetCountCritType(attackTypeCritType) / attackTypeData.GetCount() : double.NaN;
        }

        internal static String GetPercentageAsString(this AttackType attackTypeData, Specials attackTypeCritType)
        {
            double percentage = attackTypeData.GetPercentage(attackTypeCritType);
            if (percentage == double.NaN)
                return ActGlobals.ActLocalization.LocalizationStrings[Properties.PluginRegex.specialAttackNoneLocalization];
            else
                return percentage.ToString();
        }
    }
}
