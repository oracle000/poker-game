
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using PokerGame.Services;

namespace PokerGame.Test
{
    [TestClass]
    public class ModificationServiceTest
    {
        private ModificationService _modificationServiceTest;

        [TestInitialize]
        public void Initialize()
        {
            _modificationServiceTest = new ModificationService();
        }

        [TestMethod]
        public void Can_Execute_ModifyCards()
        {
            var sampleObj = new List<string> { "JC", "2D", "KS", "6D", "9C" };
            var expectedOutput = new List<string> { "11C", "2D", "13S", "6D", "9C" };
            var result = _modificationServiceTest.ModifyCards(sampleObj);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<string>));
            CollectionAssert.AreEqual(result, expectedOutput);
            CollectionAssert.AreEquivalent(result, expectedOutput);
        }

        [TestMethod]
        public void Can_Execute_RevertCards()
        {
            var sampleObj = new List<string>  { "11C", "2D", "13S", "6D", "9C" };
            var expectedOutput = new List<string> { "JC", "2D", "KS", "6D", "9C" };
            var result = _modificationServiceTest.RevertCards(sampleObj);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<string>));
            CollectionAssert.AreEqual(result, expectedOutput);
            CollectionAssert.AreEquivalent(result, expectedOutput);
        }

        [TestMethod]
        public void Can_Execute_ConvertToNumber()
        {
            var result = _modificationServiceTest.ConvertToNumber("5S");

            Assert.IsNotNull(result);
            Assert.AreEqual(result, "5");
            Assert.IsInstanceOfType(result, typeof(string));
        }
    }
}
