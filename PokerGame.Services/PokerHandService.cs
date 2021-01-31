using System.Text.RegularExpressions;
using System.Collections.Generic;
using PokerGame.Data;
using System.Linq;
using System;
using System.Xml.XPath;

namespace PokerGame.Services
{
    public class PokerHandService : IPokerHandService
    {
        readonly IModificationService _modificationService;

        public PokerHandService(IModificationService modificationService)
        {
            _modificationService = modificationService;
        }

        public PokerHand CheckCards(List<string> cards)
        {
            var newCards = _modificationService.ModifyCards(cards);
            

            if (RoyalFlush(cards))
                return PokerHand.RoyalFlush;

            if (StraightFlush(newCards))
                return PokerHand.StraightFlush;

            if (FourOfaKind(newCards))
                return PokerHand.FourOfAKind;

            if (FullHouse(newCards))
                return PokerHand.FullHouse;

            if (Flush(newCards))
                return PokerHand.Flush;

            if (Straight(newCards))
                return PokerHand.Straight;

            if (ThreeofaKind(newCards))
                return PokerHand.ThreeOfAKind;

            if (TwoPair(newCards))
                return PokerHand.TwoPair;

            if (Pair(newCards))
                return PokerHand.Pair;

            return PokerHand.HighCard;
        }

        private bool RoyalFlush(List<string> cards) // done
        {
            string[] value = {"A", "K", "Q", "J", "10"}; 
            var allMatch = true;

            cards.ForEach(x => 
            {
                if (!value.Any(x.Contains)) // bug: if the cards has identical number
                    allMatch = false;
            });

            return Flush(cards) && allMatch;
        }

        private bool StraightFlush(List<string> cards)
        {
            return Flush(cards) && Straight(cards);
        }

        private  bool FourOfaKind(IEnumerable<string> cards) 
        {
            return cards.GroupBy(x => _modificationService.ConvertToNumber(x)).Any(y => y.Count() == 4);
            //return cards.GroupBy(ConvertToNumber).Any(y => y.Count() == 4);
        }

        private bool FullHouse(List<string> cards)
        {
            return CheckforTriple(cards) && CheckforDouble(cards);  
        }

        private bool Flush(IEnumerable<string> cards) 
        {
            return cards.GroupBy(x => x.Last()).Any(y => y.Count() == 5);
        }

        private bool CheckforTriple(IEnumerable<string> cards)
        {
            return cards.GroupBy(x => _modificationService.ConvertToNumber(x)).Any(y => y.Count() == 3);
        }

        private bool CheckforDouble(IEnumerable<string> cards)
        {
            return cards.GroupBy(x => _modificationService.ConvertToNumber(x)).Any(y => y.Count() == 2);
        }

        private bool Straight(List<string> cards)
        {
            var isStraight = false;
            var tempValue = new List<int>();

            cards.ForEach(x =>
            {
                tempValue.Add(Convert.ToInt32(_modificationService.ConvertToNumber(x)));
            });

            var removeDup = tempValue.Distinct().ToList();
            if (removeDup.Count == 5)
            {
                tempValue.Sort();

                var checker = tempValue.SequenceEqual(Enumerable.Range(tempValue[0], tempValue.Count));

                if (checker)
                    isStraight = true;
            }

            return isStraight;
        }

        private bool ThreeofaKind(IEnumerable<string> cards)
        {
            return cards.GroupBy(x => _modificationService.ConvertToNumber(x)).Any(y => y.Count() == 3);
        }

        private bool TwoPair(List<string> cards)
        {
            var tempData = new List<string>();
            var firstPair = "";
            var secondPair = "";
            
            if (cards.GroupBy(x => _modificationService.ConvertToNumber(x)).Any(x => x.Count() == 2))
            {
                firstPair = cards.GroupBy(x => _modificationService.ConvertToNumber(x)).Where(x => x.Count() == 2).Select(y => y.Key).First();
            }

            cards.ForEach(x =>
            {
                if (!x.Contains(firstPair))
                {
                    tempData.Add(x);
                }
            });

            if (tempData.GroupBy(x => _modificationService.ConvertToNumber(x)).Any(x => x.Count() == 2))
                secondPair = tempData.GroupBy(x => _modificationService.ConvertToNumber(x)).Where(x => x.Count() == 2).Select(y => y.Key).First();
            
            return (!string.IsNullOrEmpty(firstPair) && !string.IsNullOrEmpty(secondPair));
        }

        private bool Pair(IEnumerable<string> cards)
        {
            return cards.GroupBy(x => _modificationService.ConvertToNumber(x)).Any(y => y.Count() == 2);
        }


        private List<Player> RankDuplicateCard(List<Player> players)
        {
            switch (players[0].PokerHand)
            {
                case PokerHand.HighCard:
                    break;
                case PokerHand.Pair:
                    break;
                case PokerHand.TwoPair:
                    break;
                case PokerHand.ThreeOfAKind:
                    break;
                case PokerHand.Straight:
                    break;
                case PokerHand.FullHouse:
                    break;
                case PokerHand.FourOfAKind:
                    break;
            }

            return null;
        }
    }


    public  interface IPokerHandService
    {
        PokerHand CheckCards(List<string> cards);

    }
}


