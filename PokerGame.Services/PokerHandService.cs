using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Xml.XPath;
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

        public List<Player> RankPlayers(List<Player> players)
        {
            var winnerOrder = new List<Player>();
            var tempPlayer = new List<Player>();

            // sort by enum
            var sorted = players.OrderBy(x => (int) (x.PokerHand)).ToList();
            var index = 0;
            sorted.ForEach(x =>
            {
                if (index == 0)
                {
                    if (x.PokerHand != sorted[index + 1].PokerHand)
                    {
                        winnerOrder.Add(x);
                    }
                    else
                    {

                    }
                }
                else
                {
                    if (x.PokerHand == sorted[index + 1].PokerHand && x.PokerHand != sorted[index - 1].PokerHand)
                    {
                        tempPlayer.Add(x);
                    } else if (x.PokerHand == sorted[index - 1].PokerHand && x.PokerHand != sorted[index + 1].PokerHand)
                    {
                        tempPlayer.Add(x);
                    } else if (x.PokerHand != sorted[index - 1].PokerHand)
                    {
                        // process temp


                        winnerOrder.Add(x);
                    }
                    
                    else if (x.PokerHand == sorted[index - 1].PokerHand)
                    {
                        tempPlayer.Add(x);
                    } else if (x.PokerHand != sorted[index + 1].PokerHand || sorted[index + 1] == null)
                    {

                    } else if (x.PokerHand != sorted[index + 1].PokerHand && x.PokerHand != sorted[index - 1].PokerHand)
                    {
                        winnerOrder.Add(x);
                        
                    }
                }

                
                index++;
            });


            


            var z = sorted.GroupBy(x => x.PokerHand).Where(highest => highest.Count() > 1).ToList();

            // check for duplicate
            var result = sorted.GroupBy(x => x.PokerHand).Where(y => y.Count() > 1).ToList();

            // sort duplicate
            result.ForEach(y =>
            {
                if (y.Key == PokerHand.HighCard)
                {
                    var listOfHighCard = y.OrderByDescending(x => TotalValue(x.Cards)).ToList();
                    Console.WriteLine(listOfHighCard);
                }

                if (y.Key == PokerHand.Pair)
                {
                    Console.WriteLine("Pair here");
                }

            });


            return sorted;
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

        private static bool FourOfaKind(IEnumerable<string> cards) 
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

        private static bool CheckforTriple(IEnumerable<string> cards)
        {
            return cards.GroupBy(ConvertToNumber).Any(y => y.Count() == 3);
        }

        private static bool CheckforDouble(IEnumerable<string> cards)
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

        private static bool ThreeofaKind(IEnumerable<string> cards)
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

        private static bool Pair(IEnumerable<string> cards)
        {
            return cards.GroupBy(ConvertToNumber).Any(y => y.Count() == 2);
        }


        private static List<Player> RankDuplicateCard(List<Player> players)
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

        private int TotalValue(List<string> cards)
        {
            var modify = ModifyCards(cards);
            var totalAmount = 0;
            modify.ForEach(x =>
            {
                totalAmount += Convert.ToInt32(x.Remove(x.Length - 1, 1));
            });

            return totalAmount;
        }
    }


    internal interface IPokerHandService
    {
        PokerHand CheckCards(List<string> cards);

        List<Player> RankPlayers(List<Player> players);
    }
}


