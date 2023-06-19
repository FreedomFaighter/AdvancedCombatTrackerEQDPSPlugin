using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using EverQuestDPSPlugin;

namespace EQDPSPluginUnitTests
{
    [TestClass]
    public sealed class PluginEnumUnitTests
    {
        [TestMethod("MeleeAndPetEnumCharacterPossesiveType")]
        public void TestMeleeAndPetEnumCharacterPossesiveTypeAttackMethod()
        {
            Assert.AreEqual(EverQuestSwingType.Pet.CharacterPossesiveTypeAttack(EverQuestSwingType.Melee), EverQuestSwingType.PetMelee);
        }

        [TestMethod("InstantHealingAndWardEnumCharacterPossessiveType")]
        public void TestInstantHealingAndWardEnumCharacterPossessiveTypeAttackMethod()
        {
            Assert.AreEqual(EverQuestSwingType.Ward.CharacterPossesiveTypeAttack(EverQuestSwingType.InstantHealing), EverQuestSwingType.WardInstantHealing);
        }

        [TestMethod("HealingOverTimeAndWardEnumCharacterPossessive")]
        public void TestHealingOverTimeAndWardEnumCharacterPossessiveTypeAttackMethod()
        {
            Assert.AreEqual(EverQuestSwingType.Ward.CharacterPossesiveTypeAttack(EverQuestSwingType.HealOverTime), EverQuestSwingType.WardHealOverTime);
        }

        [TestMethod("MeleeAndCharacterEnumCharacterPossesive")]
        public void TestMeleeAndCharacterEnumCharacterPossesiveTypeAttackMethod()
        {
            Assert.AreEqual(((EverQuestSwingType)0).CharacterPossesiveTypeAttack(EverQuestSwingType.Melee), EverQuestSwingType.Melee);
        }

        [TestMethod("NonMeleeAndCharacterEnumCharacterPossesive")]
        public void TestNonMeleeAndCharacterEnumCharacterPossesiveTypeAttackMethod()
        {
            Assert.AreEqual(((EverQuestSwingType)0).CharacterPossesiveTypeAttack(EverQuestSwingType.NonMelee), EverQuestSwingType.NonMelee);
        }

        [TestMethod("MeleeIncomingAndPetEnumCharacterPossessive")]
        public void TestMeleeIncomingAndPetEnumCharacterPossessiveTypeAttackMethod()
        {
            Assert.AreEqual(EverQuestSwingType.Pet.CharacterPossesiveTypeAttack(EverQuestSwingType.Melee | EverQuestSwingType.Incoming), EverQuestSwingType.Incoming | EverQuestSwingType.Melee);
        }
    }
}
