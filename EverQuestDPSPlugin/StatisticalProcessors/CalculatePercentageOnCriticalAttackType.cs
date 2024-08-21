using Advanced_Combat_Tracker;
using System;
using System.Linq;

namespace EverQuestDPS.StatisticalProcessors
{
    internal static class CalculatePercentageOnCriticalAttackType
    {
        internal static int GetCount(this AttackType attackTypeData)
        {
            return attackTypeData.Items.Count(item => item.Damage != Dnum.Death);
        }

        internal static int GetCountCritType(this AttackType attackTypeData, String attackTypeCritType)
        {
            return attackTypeData.Items.Where(item => item.Damage >= 0).Sum((item) => (Boolean)item.Tags[attackTypeCritType] ? 1 : 0);
        }

        internal static double GetPercentage(this AttackType attackTypeData, String attackTypeCritType)
        {
            int attackTypeCount = attackTypeData.GetCount();
            return attackTypeCount > 0 ? attackTypeData.GetCountCritType(attackTypeCritType) / attackTypeData.GetCount() : double.NaN;
        }

        internal static String GetPercentageAsString(this AttackType attackTypeData, String attackTypeCritType)
        {
            double percentage = attackTypeData.GetPercentage(attackTypeCritType);
            if (percentage == double.NaN)
                return ActGlobals.ActLocalization.LocalizationStrings[Properties.EQDPSPlugin.specialAttackNoneLocalization];
            else
                return percentage.ToString();
        }
    }
}
