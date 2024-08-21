using Advanced_Combat_Tracker;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EverQuestDPS.StatisticalProcessors
{
    internal class Variance
    {
        #region Variance
        internal static Func<AttackType, double> varianceCalc = default;
        internal static Func<AttackType, double> populationVariance = new Func<AttackType, double>((Data) =>
        {
            List<MasterSwing> masterSwingList = Data.Items.ToList().Where((item) => item.Damage.Number >= 0).ToList();
            if (Data.Swings < 1)
                return double.NaN;
            return (masterSwingList.Sum((item) =>
            {
                return Math.Pow(Data.Average - item.Damage.Number, 2.0);
            }) / Data.Swings);
        });
        internal static Func<AttackType, double> sampleVariance = new Func<AttackType, double>((Data) =>
        {
            List<MasterSwing> masterSwingList = Data.Items.ToList().Where((item) => item.Damage.Number >= 0).ToList();
            if (Data.Swings < 2)
                return double.NaN;
            return (masterSwingList.Sum((item) =>
            {
                return Math.Pow(Data.Average - item.Damage.Number, 2.0);
            }) / (Data.Swings - 1));
        });
        #endregion
    }
}
