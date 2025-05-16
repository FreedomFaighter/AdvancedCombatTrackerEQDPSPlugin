using EverQuestDPS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System;
using EverQuestDPS.Enums;
using System.Collections;

namespace EQDPSPluginUnitTests
{
    [TestClass]
    public sealed class SelfTests
    {
        EQDPSParser plugin;
        [TestInitialize]
        public void Init()
        {
            plugin = new EQDPSParser();
        }

        [DataTestMethod]
        [DataRow("himself")]
        [DataRow("herself")]
        [DataRow("itself")]
        [DataRow("themselves")]
        [TestCategory("Self check")]
        public void selfIsTrue(string selfTest)
        {
            Assert.IsTrue(plugin.CheckIfSelf(selfTest));
        }

        [DataTestMethod]
        [DataRow("ourself")]
        [DataRow("myself")]
        [DataRow("weselves")]
        [DataRow("theirselves")]
        [TestCategory("Self check")]
        public void selfIsFalse(string selfTest)
        {
            Assert.IsFalse(plugin.CheckIfSelf(selfTest));
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
