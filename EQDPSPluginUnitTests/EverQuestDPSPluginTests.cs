using EverQuestDPSPlugin;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text.RegularExpressions;

namespace EQDPSPluginUnitTests
{
    [TestClass]
    public class EverQuestDPSPluginTests
    {

        EverQuestDPSPlugin.EverQuestDPSPlugin eqDPSPlugin;
        [TestInitialize] public void Init() { 
            eqDPSPlugin = new EverQuestDPSPlugin.EverQuestDPSPlugin();
            eqDPSPlugin.PopulateRegexNonCombat();
        }

        [DataTestMethod]
        [DataRow("himself")]
        [DataRow("herself")]
        [DataRow("itself")]
        [DataRow("themselves")]
        public void selfIsTrue(string selfTest)
        {
            Assert.IsTrue(eqDPSPlugin.CheckIfSelf(selfTest));
        }

        [TestMethod]
        public void RegexStringTestExceptionOnNullString()
        {
            Assert.ThrowsException<ArgumentNullException>(new Action(() => eqDPSPlugin.RegexString(null)));
        }

        [TestMethod]
        public void ParseDateTimeIsDateTime()
        {
            Assert.AreEqual(eqDPSPlugin.ParseDateTime(DateTime.Now.ToString(EverQuestDPSPlugin.EverQuestDPSPluginResource.eqDateTimeStampFormat)).GetType(), typeof(DateTime));
        }

        [DataTestMethod]
        [DataRow("`s pet", EverQuestSwingType.Pet)]
        [DataRow("`s ward", EverQuestSwingType.Ward)]
        [DataRow("`s warder", EverQuestSwingType.Warder)]
        [DataRow("`s familiar", EverQuestSwingType.Familiar)]
        [DataRow("'s flames", EverQuestSwingType.NonMelee)]
        [DataRow("'s frost", EverQuestSwingType.NonMelee)]
        [DataRow("'s thorns", EverQuestSwingType.NonMelee)]
        public void GetTypeAndNameForPetPossesiveTest(string StringToTestForOwnership, EverQuestSwingType swingTypeToTestForMatch)
        {
            Assert.AreEqual<EverQuestSwingType>(eqDPSPlugin.GetTypeAndNameForPet(StringToTestForOwnership).Item1, swingTypeToTestForMatch);
        }
    }
}
