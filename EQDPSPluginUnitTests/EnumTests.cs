using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace EQDPSPluginUnitTests
{
    [TestClass]
    public class EnumTests
    {
        [DataTestMethod]
        [DataRow("None")]
        [DataRow("Melee")]
        [DataRow("NonMelee")]
        [DataRow("Spell")]
        [DataRow("Heal")]
        public void SwingTypeListPositive(string swingType)
        {
            Assert.IsTrue(Enum.GetNames(typeof(EverQuestDPS.Enums.EQSwingType)).Contains(swingType));
        }

        [DataTestMethod]
        [DataRow("None1")]
        [DataRow("Melee1")]
        [DataRow("NonMelee1")]
        [DataRow("Spell1")]
        [DataRow("Heal1")]
        public void SwingTypeListNegative(string swingType)
        {
            Assert.IsFalse(Enum.GetNames(typeof(EverQuestDPS.Enums.EQSwingType)).Contains(swingType));
        }
    }
}
