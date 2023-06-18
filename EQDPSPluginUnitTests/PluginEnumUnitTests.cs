using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using EverQuestDPSPlugin;

namespace EQDPSPluginUnitTests
{
    [TestClass]
    public sealed class PluginEnumUnitTests
    {
        [TestMethod]
        public void TestMeleeAndPetEnumCharacterPossesiveTypeAttackMethod()
        {
            Assert.AreEqual(EverQuestSwingType.Pet.CharacterPossesiveTypeAttack(EverQuestSwingType.Melee), EverQuestSwingType.PetMelee);
        }

        [TestMethod]
        public void TestInstantHealingAndWardEnumCharacterPossessiveTypeAttackMethod()
        {
            Assert.AreEqual(EverQuestSwingType.Ward.CharacterPossesiveTypeAttack(EverQuestSwingType.InstantHealing), EverQuestSwingType.WardInstantHealing);
        }

        [TestMethod]
        public void TestHealingOverTimeAndWardEnumCharacterPossessiveTypeAttackMethod()
        {
            Assert.AreEqual(EverQuestSwingType.Ward.CharacterPossesiveTypeAttack(EverQuestSwingType.HealOverTime), EverQuestSwingType.WardHealOverTime);
        }
    }
}
