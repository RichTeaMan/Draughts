using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RichTea.Utilities.Tests
{
    [TestClass]
    public class NameUtilityTests
    {
        [TestMethod]
        public void FindNamesCountTest()
        {
            var names = NameUtility.FindNames();

            Assert.AreEqual(5163, names.Length);
        }

        [TestMethod]
        public void NameTest()
        {
            var name = NameUtility.GenerateName(1105563953);

            Assert.AreEqual("Tanya-954", name);
        }
    }
}
