using EverQuestDPSPlugin;
using EverQuestDPSPlugin.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Advanced_Combat_Tracker;
using System.Collections.Generic;

namespace EQDPSPluginUnitTests
{
    [TestClass]
    public sealed class EverQuestDPSPluginTests
    {
        EverQuestDPSPlugin.EverQuestDPSPlugin eqDPSPlugin;
        [TestInitialize] 
        public void Init() { 
            eqDPSPlugin = new EverQuestDPSPlugin.EverQuestDPSPlugin();
        }

        [DataTestMethod]
        [DataRow("himself")]
        [DataRow("herself")]
        [DataRow("itself")]
        [DataRow("themselves")]
        [TestCategory("Plugin Tests")]
        public void selfIsTrue(string selfTest)
        {
            Assert.IsTrue(eqDPSPlugin.CheckIfSelf(selfTest));
        }

        [DataTestMethod]
        [DataRow("ourself")]
        [DataRow("myself")]
        [DataRow("weselves")]
        [DataRow("theirselves")]
        [TestCategory("Plugin Tests")]
        public void selfIsFalse(string selfTest)
        {
            Assert.IsFalse(eqDPSPlugin.CheckIfSelf(selfTest));
        }

        [TestMethod]
        [TestCategory("Plugin Tests")]
        public void RegexStringTestExceptionOnNullString()
        {
            Assert.ThrowsException<ArgumentNullException>(new Action(() => eqDPSPlugin.RegexString(null)));
        }

        [DataTestMethod]
        [TestCategory("MasterSwing Class Creation")]
        [DataRow(EverQuestSwingType.NonMelee, "", 0, "smootches", "attacker", "Hitpoints", "victim")]
        [DataRow(EverQuestSwingType.Melee, "", 0, "hugs", "attacker", "Hitpoints", "victim")]
        public void GetMasterSwing(
            EverQuestSwingType eqst
            , String criticalAttack
            , Int64 damage
            , String damageType
            , String attacker
            , String typeOfResource
            , String victim)
        {
	    Dictionary<string, Object> testDicitonary = new Dictionary<string, Object>();
	    testDicitonary.Add("testKey", "testValue");
	    DateTime testDateTime = DateTime.Now;
            MasterSwing testMasterSwing = 
                EverQuestDPSPlugin.GetMasterSwing(
                  eqst
                , criticalAttack
                , new Dnum(damage, "non-melee")
                , testDateTime
                , damageType
                , attacker
                , typeOfResource
                , victim
		, testDicitonary);
            Assert.IsTrue(testMasterSwing.Victim == victim);
            Assert.IsTrue(testMasterSwing.Attacker == attacker);
            Assert.IsTrue(testMasterSwing.Time == testDateTime);
            Assert.IsFalse(testMasterSwing.Attacker == String.Empty);
            Assert.IsFalse(testMasterSwing.Victim == String.Empty);
            Assert.IsTrue(testMasterSwing.Critical == criticalAttack.Contains("Critical"));
            Assert.IsTrue(testMasterSwing.DamageType == damageType);
            //Assert.IsTrue()
            Assert.IsTrue(testMasterSwing.DamageType != String.Empty);
	    Assert.IsTrue(testMasterSwing.Tags.Equals(testDicitonary));
        }
    }
}
