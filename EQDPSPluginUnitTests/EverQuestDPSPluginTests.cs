using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EQDPSPluginUnitTests
{
    [TestClass]
    public class EverQuestDPSPluginTests
    {

        EverQuestDPSPlugin.EverQuestDPSPlugin eqDPSPlugin;
        [TestInitialize] public void Init() { 
            eqDPSPlugin = new EverQuestDPSPlugin.EverQuestDPSPlugin();
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
    }
}
