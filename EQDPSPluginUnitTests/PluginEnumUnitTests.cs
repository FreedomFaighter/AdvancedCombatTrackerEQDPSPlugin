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
            Assert.AreEqual(EverQuestSwingType.Pet.CharacterPossesiveTypeAttack(EverQuestSwingType.Melee), (EverQuestSwingType.Pet | EverQuestSwingType.Melee));
        }

        private EverQuestDPSPlugin.EverQuestDPSPlugin _everQuestDPSPlugin;

        public PluginEnumUnitTests()
        {
            _everQuestDPSPlugin = new EverQuestDPSPlugin.EverQuestDPSPlugin();
        }
    }
}
