using EverQuestDPS;
using EverQuestDPS.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Advanced_Combat_Tracker;
using System.Collections.Generic;

namespace EQDPSPluginUnitTests
{
    [TestClass]
    public sealed class EverQuestDPSPluginTests
    {
        EverQuestDPSPlugin eqDPSPlugin;
        [TestInitialize] 
        public void Init() { 
            eqDPSPlugin = new EverQuestDPSPlugin();
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

        /*
        [DataTestMethod]
        [TestCategory("MasterSwing Class Creation")]
        [DataRow(EverQuestSwingType.NonMelee, "Critical", 0, "cheech", "attacker", "Hitpoints", "victim", "non-melee")]
        [DataRow(EverQuestSwingType.Melee, "Flurry", 0, "chongya", "attacker", "Hitpoints", "victim", "melee")]
        public void GetMasterSwingTest(
            EverQuestSwingType eqst
            , String criticalAttack
            , Int64 damage
            , String damageType
            , String attacker
            , String typeOfResource
            , String victim
            , String dnumAttackType)
        {
	    Dictionary<string, Object> testDicitonary = new Dictionary<string, Object>
        {
            { "testKey", "testValue" }
        };
        Dnum dnum = new Dnum(damage, dnumAttackType);

        DateTime testDateTime = DateTime.Now;
            MasterSwing testMasterSwing = 
                EverQuestDPSPlugin.GetMasterSwing(
                  eqst
                , criticalAttack
                , dnum
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
            Assert.IsTrue(testMasterSwing.DamageType != String.Empty);
	        Assert.IsTrue(testMasterSwing.Tags.Equals(testDicitonary));
        }*/
    }
}
