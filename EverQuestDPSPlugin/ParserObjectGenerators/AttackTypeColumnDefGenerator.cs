using Advanced_Combat_Tracker;
using EverQuestDPS.StatisticalProcessors;
using System;

namespace EverQuestDPS.ParserObjectGenerators
{
    internal class AttackTypeColumnDefGenerator
    {
        internal static AttackType.ColumnDef GetAttackTypeCritColumnDef(String ColumnName,
            Boolean Visible,
            String SQLDataType)
        {
            return new AttackType.ColumnDef(ColumnName, Visible, SQLDataType, ColumnName,
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
