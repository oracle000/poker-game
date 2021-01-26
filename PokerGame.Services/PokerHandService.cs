using System;
using System.Collections.Generic;
using System.Linq;
using PokerGame.Data;

namespace PokerGame.Services
{
    public class PokerHandService : IPokerHandService
    {
        public PokerHand CheckCards(List<string> cards)
        {

            // royal flush
            var result = CheckRoyalFlush(cards);

            // full house
            CheckFullHouse(cards);

            return PokerHand.Flush;
        }

        private static PokerHand? CheckRoyalFlush(List<string> cards)
        {
            string[] value = {"A", "K", "Q", "J", "10"};
            var getSign = cards[0].Last();
            var sameSign = cards.All(x => x.Last() == getSign);
            var allMatch = true;
            

            cards.ForEach(x =>
            {
                if (!value.Any(x.Contains))
                    allMatch = false;
            });

            if (sameSign && allMatch)
                return PokerHand.RoyalFlush;
            return null;
        }

        public PokerHand CheckStraightFlush(List<string> cards)
        {
            var getSign = cards[0].Last();
            var sameSign = cards.All(x => x.Last() == getSign);

            return PokerHand.StraightFlush;
        }

        public PokerHand CheckFourOfaKind(List<string> cards)
        {


            return PokerHand.FourOfAKind;
        }

        private static PokerHand? CheckFullHouse(List<string> cards)
        {
            var getSign = cards[0].Last();
            var sameSign = cards.All(x => x.Last() == getSign);

            if(sameSign)
                return PokerHand.FullHouse;

            return null;
        }
        private static PokerHand? CheckFlush(List<string> cards)
        {
            var getSign = cards[0].Last();
            var sameSign = cards.All(x => x.Last() == getSign);

            if (sameSign)
                return PokerHand.FullHouse;
            return null;
        }
        public PokerHand CheckStraight(List<string> cards)
        {
            return PokerHand.Straight;
        }
        public PokerHand CheckThreeOfaKind(List<string> cards)
        {
            return PokerHand.ThreeOfAKind;
        }
        public PokerHand CheckTwoPair(List<string> cards)
        {
            return PokerHand.TwoPair;
        }
        public PokerHand CheckOnePair(List<string> cards)
        {
            return PokerHand.OnePair;
        }
        public PokerHand CheckHighCard(List<string> cards)
        {
            return PokerHand.HighCard;
        }
    }


    internal interface IPokerHandService
    {
        PokerHand CheckCards(List<string> cards);
    }
}


