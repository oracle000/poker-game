using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using PokerGame.Services;
using PokerGame.Data;

namespace PokerGame.Test
{
    [TestClass]
    public class WinningCardServiceTest
    {
        private Player PlayerRoyalFlush;
        private Player PlayerStraightFlush;
        private Player PlayerFourofaKind;
        private Player PlayerFullHouse;
        private Player PlayerFlush;
        private Player PlayerStraight;
        private Player PlayerThreeofaKind;
        private Player PlayerTwoPair;
        private Player PlayerPair;
        private Player PlayerHighCard;


        private WinningCardService _WinningCardService;
        private IModificationService _modificationService;
        

        [TestInitialize]
        public void Initialize()
        {
            _modificationService = new ModificationService();
            _WinningCardService = new WinningCardService(_modificationService);

            #region player declaration
            PlayerRoyalFlush = new Player
            {
                Cards = new List<string> {"AC", "10C", "JC", "QC", "KC"},
                Name = "John",
                PokerHand = PokerHand.RoyalFlush
            };

            PlayerStraightFlush = new Player
            {
                Cards = new List<string> { "5C", "AC", "4C", "2C", "3C" },
                Name = "Marc",
                PokerHand = PokerHand.StraightFlush
            };

            PlayerFourofaKind = new Player
            {
                Cards = new List<string> { "8D", "8C", "8S", "8H", "QS" },
                Name = "Kim",
                PokerHand = PokerHand.FourOfAKind
            };

            PlayerFullHouse = new Player
            {
                Cards = new List<string> { "8D", "8C", "8S", "2H", "2S" },
                Name = "Efren",
                PokerHand = PokerHand.FullHouse
            };

            PlayerFlush = new Player()
            {
                Cards = new List<string> {"8D", "2D", "5D", "QD", "AD"},
                Name = "Jefferson",
                PokerHand = PokerHand.Flush
            };

            PlayerStraight = new Player
            {
                Cards = new List<string> { "8D", "7C", "4S", "5D", "6S" },
                Name = "Lara",
                PokerHand = PokerHand.Straight
            };

            PlayerThreeofaKind = new Player
            {
                Cards = new List<string> { "QD", "QC", "AS", "QH", "2S" },
                Name = "Jason",
                PokerHand = PokerHand.ThreeOfAKind
            };

            PlayerTwoPair = new Player
            {
                Cards = new List<string> { "5D", "5C", "AS", "AH", "QS" },
                Name = "Mike",
                PokerHand = PokerHand.TwoPair
            };

            PlayerPair = new Player
            {
                Cards = new List<string> { "8D", "4C", "AS", "JH", "JS" },
                Name = "Eric",
                PokerHand = PokerHand.Pair
            };

            PlayerHighCard = new Player
            {
                Cards = new List<string> { "AD", "8C", "5H", "QD", "2S" },
                Name = "Chris",
                PokerHand = PokerHand.HighCard
            };

            #endregion

        }

        [TestMethod]
        public void Can_Execute_RankPlayer()
        {
            var sampleObj = new List<Player>
            {
                PlayerRoyalFlush,
                PlayerPair,
                PlayerHighCard,
                PlayerTwoPair
            };

            var result = _WinningCardService.RankPlayer(sampleObj);
            
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<Player>));
        }

        [TestMethod]
        public void Can_Execute_RankPlayer_win_ROYALFLUSH()
        {
            var sampleObj = new List<Player>
            {
                PlayerFourofaKind,
                PlayerPair,
                PlayerFlush,
                PlayerRoyalFlush
            };

            var result = _WinningCardService.RankPlayer(sampleObj);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<Player>));
            Assert.AreEqual(result[0].PokerHand, PokerHand.RoyalFlush);
        }

        [TestMethod]
        public void Can_Execute_RankPlayer_win_STRAIGHTFLUSH()
        {
            var sampleObj = new List<Player>
            {
                PlayerThreeofaKind,
                PlayerStraightFlush,
                PlayerPair,
                PlayerFullHouse
            };

            var result = _WinningCardService.RankPlayer(sampleObj);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<Player>));
            Assert.AreEqual(result[0].PokerHand, PokerHand.StraightFlush);
        }

        [TestMethod]
        public void Can_Execute_RankPlayer_win_FOUROFAKIND()
        {
            var sampleObj = new List<Player>
            {
                PlayerHighCard,
                PlayerStraight,
                PlayerPair,
                PlayerFourofaKind
            };

            var result = _WinningCardService.RankPlayer(sampleObj);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<Player>));
            Assert.AreEqual(result[0].PokerHand, PokerHand.FourOfAKind);
        }


        [TestMethod]
        public void Can_Execute_RankPlayer_win_FULLHOUSE()
        {
            var sampleObj = new List<Player>
            {
                PlayerHighCard,
                PlayerStraight,
                PlayerPair,
                PlayerFullHouse
            };

            var result = _WinningCardService.RankPlayer(sampleObj);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<Player>));
            Assert.AreEqual(result[0].PokerHand, PokerHand.FullHouse);
        }

        [TestMethod]
        public void Can_Execute_RankPlayer_win_FLUSH()
        {
            var sampleObj = new List<Player>
            {
                PlayerFlush,
                PlayerStraight,
                PlayerPair,
                PlayerHighCard
            };

            var result = _WinningCardService.RankPlayer(sampleObj);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<Player>));
            Assert.AreEqual(result[0].PokerHand, PokerHand.Flush);
        }


        [TestMethod]
        public void Can_Execute_RankPlayer_win_STRAIGHT()
        {
            var sampleObj = new List<Player>
            {
                PlayerHighCard,
                PlayerTwoPair,
                PlayerStraight,
                PlayerHighCard
            };

            var result = _WinningCardService.RankPlayer(sampleObj);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<Player>));
            Assert.AreEqual(result[0].PokerHand, PokerHand.Straight);
        }

        [TestMethod]
        public void Can_Execute_RankPlayer_win_THREEOFAKIND()
        {
            var sampleObj = new List<Player>
            {
                PlayerHighCard,
                PlayerTwoPair,
                PlayerThreeofaKind,
                PlayerHighCard
            };

            var result = _WinningCardService.RankPlayer(sampleObj);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<Player>));
            Assert.AreEqual(result[0].PokerHand, PokerHand.ThreeOfAKind);
        }

        [TestMethod]
        public void Can_Execute_RankPlayer_win_TWOPAIR()
        {
            var sampleObj = new List<Player>
            {
                PlayerHighCard,
                PlayerTwoPair,
                PlayerPair,
                PlayerHighCard
            };

            var result = _WinningCardService.RankPlayer(sampleObj);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<Player>));
            Assert.AreEqual(result[0].PokerHand, PokerHand.TwoPair);
        }


        [TestMethod]
        public void Can_Execute_RankPlayer_win_PAIR()
        {
            var sampleObj = new List<Player>
            {
                PlayerHighCard,
                PlayerHighCard,
                PlayerPair,
                PlayerHighCard
            };

            var result = _WinningCardService.RankPlayer(sampleObj);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<Player>));
            Assert.AreEqual(result[0].PokerHand, PokerHand.Pair);
        }


        [TestMethod]
        public void Can_Execute_RankPlayer_win_HIGHCARD()
        {
            var sampleObj = new List<Player>
            {
                PlayerHighCard,
                PlayerHighCard,
                PlayerHighCard,
                PlayerHighCard
            };

            var result = _WinningCardService.RankPlayer(sampleObj);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<Player>));
            Assert.AreEqual(result[0].PokerHand, PokerHand.HighCard);
        }
    }
}
