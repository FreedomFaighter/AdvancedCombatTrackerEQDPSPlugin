using EverQuestDPS.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EverQuestDPSPluginUnitTests
{
    [TestClass]
    public class EnumTests
    {
        [DataTestMethod]
        [TestCategory("Healing Enum tests")]
        [DataRow(EQSwingType.HealingOverTime)]
        [DataRow(EQSwingType.InstantHealing)]
        public void ContainsHealing(EQSwingType healingType)
        {
            Assert.IsTrue((healingType & EQSwingType.Healing) == EQSwingType.Healing);
        }

        [DataTestMethod]
        [TestCategory("Healing does not contain damage")]
        [DataRow(EQSwingType.HealingOverTime)]
        [DataRow(EQSwingType.InstantHealing)]
        public void HealingDoesNotContainHealing(EQSwingType healingType)
        {
            Assert.IsFalse((healingType & EQSwingType.Melee) == EQSwingType.Melee);
        }
    }
}
