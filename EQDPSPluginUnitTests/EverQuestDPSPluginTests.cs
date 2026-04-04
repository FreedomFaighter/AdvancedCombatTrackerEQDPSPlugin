using EverQuestDPS;
//using EverQuestDPS.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace EverQuestDPSPluginUnitTests
{
    [TestClass]
    public sealed class EverQuestDPSPluginTests
    {
        private EQDPSParser parser;

        [TestInitialize]
        public void Setup()
        {
           parser = new EQDPSParser();
        }

        [DataTestMethod]
        [DataRow("himself")]
        [DataRow("herself")]
        [DataRow("itself")]
        [DataRow("themselves")]
        [TestCategory("Self check")]
        public void selfIsTrue(string selfTest)
        {
            Assert.IsTrue(parser.CheckIfSelf(selfTest));
        }

        [DataTestMethod]
        [DataRow("ourself")]
        [DataRow("myself")]
        [DataRow("weselves")]
        [DataRow("theirselves")]
        [TestCategory("Self check")]
        public void selfIsFalse(string selfTest)
        {
            Assert.IsFalse(parser.CheckIfSelf(selfTest));
        }

        [DataTestMethod]
        [TestCategory("array contains object")]
        [DataRow("Flurry")]
        [DataRow("Lucky")]
        [DataRow("Riposte")]
        [DataRow("Locked")]
        [DataRow("Crippling Blow")]
        [DataRow("Wild Rampage")]
        [DataRow("Double Bow Shot")]
        [DataRow("Twincast")]
        [DataRow("Strikethrough")]
        [DataRow("Finishing Blow")]
        public void CriticalListContains(string str)
        {
            Assert.IsTrue(EverQuestDPS.EQDPSParser.SpecialAttack.Contains(str));
        }
    }
}
