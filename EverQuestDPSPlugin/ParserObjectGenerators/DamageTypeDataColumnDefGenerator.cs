using Advanced_Combat_Tracker;
using EverQuestDPS.StatisticalProcessors;
using System;

namespace EverQuestDPS.ParserObjectGenerators
{
    internal static class DamageTypeDataColumnDefGenerator
    {
        internal static DamageTypeData.ColumnDef GetDamageTypeDataCritColumnDef(String ColumnName,
            Boolean Visible,
            String SQLDataType)
        {
            return new DamageTypeData.ColumnDef(ColumnName, Visible, SQLDataType, ColumnName,
                (Data) =>
                {
                    return Data.GetPercentageAsString(ColumnName);
                },
                (Data) =>
                {
                    return Data.GetPercentageAsString(ColumnName);
                });
        }
    }
}
