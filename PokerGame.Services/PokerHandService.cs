using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text.RegularExpressions;
using PokerGame.Data;

namespace PokerGame.Services
{
    public class PokerHandService : IPokerHandService
    {
        public PokerHand CheckCards(List<string> cards)
        {
            var newCards = ModifyCards(cards);

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

            if (ThreeofaKind(newCards))
                return PokerHand.ThreeOfAKind;

            if (TwoPair(newCards))
                return PokerHand.TwoPair;

            if (Pair(newCards))
                return PokerHand.Pair;

            return PokerHand.HighCard;
        }

        private static bool RoyalFlush(List<string> cards) // done
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

        private static bool FourOfaKind(List<string> cards) 
        {
            return cards.GroupBy(ConvertToNumber).Any(y => y.Count() == 4);
        }

        private static bool FullHouse(List<string> cards)
        {
            return CheckforTriple(cards) && CheckforDouble(cards);  
        }

        private static bool Flush(IEnumerable<string> cards) 
        {
            return cards.GroupBy(x => x.Last()).Any(y => y.Count() == 5);
        }

        private static bool CheckforTriple(List<string> cards)
        {
            return cards.GroupBy(ConvertToNumber).Any(y => y.Count() == 3);
        }

        private static bool CheckforDouble(List<string> cards)
        {
            return cards.GroupBy(ConvertToNumber).Any(y => y.Count() == 2);
        }

        private static bool Straight(List<string> cards)
        {
            var isStraight = false;
            var tempValue = new List<int>();

            cards.ForEach(x =>
            {
                tempValue.Add(Convert.ToInt32(ConvertToNumber(x)));
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

        private static bool ThreeofaKind(List<string> cards)
        {
            return cards.GroupBy(ConvertToNumber).Any(y => y.Count() == 3);
        }

        private static bool TwoPair(List<string> cards)
        {
            var tempData = new List<string>();
            var firstPair = "";
            var secondPair = "";
            
            if (cards.GroupBy(ConvertToNumber).Any(x => x.Count() == 2))
            {
                firstPair = cards.GroupBy(ConvertToNumber).Where(x => x.Count() == 2).Select(y => y.Key).First();
            }

            cards.ForEach(x =>
            {
                if (!x.Contains(firstPair))
                {
                    tempData.Add(x);
                }
            });

            if (tempData.GroupBy(ConvertToNumber).Any(x => x.Count() == 2))
                secondPair = tempData.GroupBy(ConvertToNumber).Where(x => x.Count() == 2).Select(y => y.Key).First();
            
            return (!string.IsNullOrEmpty(firstPair) && !string.IsNullOrEmpty(secondPair));
        }

        private static bool Pair(List<string> cards)
        {
            return cards.GroupBy(ConvertToNumber).Any(y => y.Count() == 2);
        }

        private static bool OnePair(List<string> cards)
        {
            return cards.GroupBy(ConvertToNumber).Any(y => y.Count() == 2);
        }

        private static List<string> ModifyCards(List<string> cards)
        {
            var tempCard = new List<string>();

            cards.ForEach(x =>
            {
                var tempValue = "";
                switch (x.First())
                {
                    case 'A':
                        tempValue = $"1{x.Last()}";
                        break;
                    case 'J':
                        tempValue = $"11{x.Last()}";
                        break;
                    case 'Q':
                        tempValue = $"12{x.Last()}";
                        break;
                    case 'K':
                        tempValue = $"13{x.Last()}";
                        break;
                    default:
                        tempValue = x;
                        break;
                }
                tempCard.Add(tempValue);
            });

            return tempCard;
        }

        private static string ConvertToNumber(string value)
        {
            var number = Regex.Split(value, @"\D+");
            return number.FirstOrDefault();
        }
    }


    internal interface IPokerHandService
    {
        PokerHand CheckCards(List<string> cards);
    }
}


