using EverQuestDPS;
using EverQuestDPS.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Advanced_Combat_Tracker;
using System.Collections.Generic;
using System.Resources;
using System.Linq;

namespace GenericStringsUnitTests
{
    [TestClass]
    public sealed class EverQuestDPSPluginTests
    {
        EverQuestDPSPlugin plugin;
        [TestInitialize] 
        public void Init() { 
            plugin = new EverQuestDPSPlugin();
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

        [TestMethod]
        [TestCategory("null arguement")]
        public void RegexStringTestExceptionOnNullString()
        {
            Assert.ThrowsException<ArgumentNullException>(new Action(() => plugin.RegexString(null)));
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
            Assert.IsTrue(EverQuestDPS.EverQuestDPSPlugin.SpecialAttack.Contains(str));
        }
    }
}
