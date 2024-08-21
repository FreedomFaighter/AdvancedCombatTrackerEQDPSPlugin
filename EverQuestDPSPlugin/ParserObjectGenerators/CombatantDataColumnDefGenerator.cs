using Advanced_Combat_Tracker;
using EverQuestDPS.StatisticalProcessors;
using System;

namespace EverQuestDPS.ParserObjectGenerators
{
    internal class CombatantDataColumnDefGenerator
    {
        internal static CombatantData.ColumnDef GetCombatantDataCritColumnDef(String ColumnName,
            Boolean Visible,
            String SQLDataType)
        {
            return new CombatantData.ColumnDef(ColumnName, Visible, SQLDataType, ColumnName,
                (Data) =>
                {
                    return Data.GetPercentageAsString(ColumnName);
                },
                (Data) =>
                {
                    return Data.GetPercentageAsString(ColumnName);
                },
                (Left, Right) =>
                {
                    return Left.GetPercentage(ColumnName).CompareTo(ColumnName);
                }
                );
        }
    }
}
