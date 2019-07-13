using Advanced_Combat_Tracker;
using EverQuestDPS.StatisticalProcessors;
using System;
using EverQuestDPS.Enums;

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
                    return Data.GetPercentageAsString(SpecialParsers.GetSpecialByString(ColumnName));
                },
                (Data) =>
                {
                    return Data.GetPercentageAsString(SpecialParsers.GetSpecialByString(ColumnName));
                },
                (Left, Right) =>
                {
                    return Left.GetPercentage(SpecialParsers.GetSpecialByString(ColumnName)).CompareTo(Right.GetPercentage(SpecialParsers.GetSpecialByString(ColumnName)));
                }
                );
        }
    }
}
