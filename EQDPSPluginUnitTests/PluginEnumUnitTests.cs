using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using EverQuestDPSPlugin;

namespace EQDPSPluginUnitTests
{
    [TestClass]
    public sealed class PluginEnumUnitTests
    {
        [DataTestMethod]
        [DataRow(EverQuestSwingType.PetMelee, EverQuestSwingType.Pet | EverQuestSwingType.Melee)]
        [DataRow(EverQuestSwingType.WardInstantHealing, EverQuestSwingType.Ward | EverQuestSwingType.InstantHealing)]
        [DataRow(EverQuestSwingType.WardHealOverTime, EverQuestSwingType.Ward | EverQuestSwingType.HealOverTime)]
        [DataRow(EverQuestSwingType.PetNonMelee, EverQuestSwingType.Pet | EverQuestSwingType.NonMelee)]
        [DataRow(EverQuestSwingType.PetMelee | EverQuestSwingType.Incoming, EverQuestSwingType.Pet | EverQuestSwingType.Melee | EverQuestSwingType.Incoming)]
        [DataRow(EverQuestSwingType.WarderMelee, EverQuestSwingType.Warder | EverQuestSwingType.Melee)]
        [DataRow(EverQuestSwingType.WarderNonMelee, EverQuestSwingType.Warder | EverQuestSwingType.NonMelee)]
        [DataRow(EverQuestSwingType.WarderDirectDamageSpell, EverQuestSwingType.Warder | EverQuestSwingType.DirectDamageSpell)]
        [DataRow(EverQuestSwingType.WarderDamageOverTimeSpell, EverQuestSwingType.Warder | EverQuestSwingType.DamageOverTimeSpell)]
        public void EnumEqualityTests(EverQuestSwingType composite, EverQuestSwingType rawComposite)
        {
            Assert.AreEqual(composite, rawComposite);
        }

        [TestMethod("MeleeIncomingAndPetEnumCharacterPossessive")]
        public void TestMeleeIncomingAndPetEnumCharacterPossessiveTypeAttackMethod()
        {
            Assert.AreEqual(EverQuestSwingType.Pet.CharacterPossesiveTypeAttack(EverQuestSwingType.Melee | EverQuestSwingType.Incoming), EverQuestSwingType.Pet | EverQuestSwingType.Incoming | EverQuestSwingType.Melee);
        }

        [TestMethod("MeleeIncomingEnumCharacterPossessive")]
        public void TestMeleeIncomingEnumCharacterPossessiveTypeAttackMethod()
        {
            Assert.AreEqual((((EverQuestSwingType)0).CharacterPossesiveTypeAttack(EverQuestSwingType.Incoming | EverQuestSwingType.Melee)), EverQuestSwingType.Incoming | EverQuestSwingType.Melee);
        }
    }
}
