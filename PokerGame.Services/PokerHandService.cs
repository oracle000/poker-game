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
        public PokerHand? CheckCards(List<string> cards)
        {
            var newCards = ModifyCards(cards);

            if (CheckRoyalFlush(cards))
                return PokerHand.RoyalFlush;
            
            if (CheckStraightFlush(newCards))
                return PokerHand.StraightFlush;
            
            if (CheckFourOfaKind(newCards))
                return PokerHand.FourOfAKind; 

            if (CheckFullHouse(newCards))
                return PokerHand.FullHouse;
            
            if (CheckFlush(newCards))
                return PokerHand.Flush;
            

            return null;
         
        }

        private static bool CheckRoyalFlush(List<string> cards) // done
        {
            string[] value = {"A", "K", "Q", "J", "10"}; 
            var allMatch = true;

            cards.ForEach(x => 
            {
                if (!value.Any(x.Contains)) // bug: if the cards has identical number
                    allMatch = false;
            });

            return CheckFlush(cards) && allMatch;
        }

        public bool CheckStraightFlush(List<string> cards)
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
            
            return CheckFlush(cards) && isStraight;
        }

        public static bool CheckFourOfaKind(List<string> cards) 
        {
            return cards.GroupBy(ConvertToNumber).Any(y => y.Count() == 4);
        }

        private static bool CheckFullHouse(List<string> cards)
        {
            return CheckforTriple(cards) && CheckforDouble(cards);  
        }

        private static bool CheckFlush(IEnumerable<string> cards) 
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
        PokerHand? CheckCards(List<string> cards);
    }
}


