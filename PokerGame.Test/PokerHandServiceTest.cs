using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using PokerGame.Services;
using PokerGame.Data;

namespace PokerGame.Test
{
    [TestClass]
    public class CardRankServiceTest
    {
        private CardRankService _CardRankService;
        private IModificationService _mockInterface;

        [TestInitialize]
        public void Initialize()
        {
            _mockInterface = new ModificationService();
            _CardRankService = new CardRankService(_mockInterface);
        }

        [TestMethod]
        public void Can_Execute_CheckCards()
        {
            var sampleObj = new List<string> { "JC", "2D", "KS", "6D", "9C" };
            var result = _CardRankService.CheckCards(sampleObj);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(PokerHand));
        }

        [TestMethod]
        public void Can_Execute_CheckCards_is_ROYALFLUSH()
        {
            var sampleObj = new List<string> { "AC", "10C", "JC", "QC", "KC" };
            var result = _CardRankService.CheckCards(sampleObj);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(PokerHand));
            Assert.AreEqual(result, PokerHand.RoyalFlush);
        }

        [TestMethod]
        public void Can_Execute_CheckCards_is_STRAIGHTFLUSH()
        {
            var sampleObj = new List<string> { "5C", "AC", "4C", "2C", "3C" };
            var result = _CardRankService.CheckCards(sampleObj);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(PokerHand));
            Assert.AreEqual(result, PokerHand.StraightFlush);
        }

        [TestMethod]
        public void Can_Execute_CheckCards_is_FOUROFAKIND()
        {
            var sampleObj = new List<string> { "8D", "8C", "8S", "8H", "QS" };
            var result = _CardRankService.CheckCards(sampleObj);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(PokerHand));
            Assert.AreEqual(result, PokerHand.FourOfAKind);
        }

        [TestMethod]
        public void Can_Execute_CheckCards_is_FULLHOUSE()
        {
            var sampleObj = new List<string> { "8D", "8C", "8S", "2H", "2S" };
            var result = _CardRankService.CheckCards(sampleObj);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(PokerHand));
            Assert.AreEqual(result, PokerHand.FullHouse);
        }

        [TestMethod]
        public void Can_Execute_CheckCards_is_FLUSH()
        {
            var sampleObj = new List<string> { "8D", "2D", "5D", "QD", "AD" };
            var result = _CardRankService.CheckCards(sampleObj);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(PokerHand));
            Assert.AreEqual(result, PokerHand.Flush);
        }

        [TestMethod]
        public void Can_Execute_CheckCards_is_STRAIGHT()
        {
            var sampleObj = new List<string> { "8D", "7C", "4S", "5D", "6S" };
            var result = _CardRankService.CheckCards(sampleObj);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(PokerHand));
            Assert.AreEqual(result, PokerHand.Straight);
        }

        [TestMethod]
        public void Can_Execute_CheckCards_is_THREEOFAKIND()
        {
            var sampleObj = new List<string> { "QD", "QC", "AS", "QH", "2S" };
            var result = _CardRankService.CheckCards(sampleObj);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(PokerHand));
            Assert.AreEqual(result, PokerHand.ThreeOfAKind);
        }

        [TestMethod]
        public void Can_Execute_CheckCards_is_TWOPAIR()
        {
            var sampleObj = new List<string> { "5D", "5C", "AS", "AH", "QS" };
            var result = _CardRankService.CheckCards(sampleObj);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(PokerHand));
            Assert.AreEqual(result, PokerHand.TwoPair);
        }

        [TestMethod]
        public void Can_Execute_CheckCards_is_PAIR()
        {
            var sampleObj = new List<string> { "8D", "4C", "AS", "JH", "JS" };
            var result = _CardRankService.CheckCards(sampleObj);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(PokerHand));
            Assert.AreEqual(result, PokerHand.Pair);
        }

        [TestMethod]
        public void Can_Execute_CheckCards_is_HIGHCARD()
        {
            var sampleObj = new List<string> { "AD", "8C", "5H", "QD", "2S" };
            var result = _CardRankService.CheckCards(sampleObj);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(PokerHand));
            Assert.AreEqual(result, PokerHand.HighCard);
        }
    }
}
