using System.Collections.Generic;
using PokerGame.Data;
using System.Linq;
using System;
using static System.Convert;

namespace PokerGame.Services
{
    public class RankCardService : IRankCardService
    {
        readonly IModificationService _modificationService;
        public RankCardService(IModificationService modificationService)
        {
            _modificationService = modificationService;
        }

        public List<Player> RankPlayer(List<Player> players)
        {

            var winningOrder = new List<Player>();
            var tempPlayer = new List<Player>();

            var sorted = players.OrderBy(x => (int)(x.PokerHand)).ToList();

            var index = 0;
            sorted.ForEach(item =>
            {
                switch (index)
                {
                    case 0 when item.PokerHand != sorted[index + 1].PokerHand:
                        winningOrder.Add(item);
                        break;
                    case 0 when item.PokerHand == sorted[index + 1].PokerHand:
                        tempPlayer.Add(item);
                        break;
                    default:
                    {
                        if (index != 0 && index == sorted.Count() -1) // end page
                        {
                            if (item.PokerHand == sorted[index - 1].PokerHand)
                            {
                                tempPlayer.Add(item);
                                var result = FindHighestValue(tempPlayer);
                                result.ForEach(x =>
                                {
                                    winningOrder.Add(x);
                                });
                                tempPlayer.Clear();
                            }
                            else
                            {
                                if (tempPlayer.Any())
                                {
                                    var result = FindHighestValue(tempPlayer);
                                    result.ForEach(x =>
                                    {
                                        winningOrder.Add(x);
                                    });
                                    tempPlayer.Clear();
                                }
                                winningOrder.Add(item);
                            }
                        } else if (index != 0 && index <= sorted.Count() - 1) 
                        {
                            if (item.PokerHand == sorted[index - 1].PokerHand &&  item.PokerHand != sorted[index + 1].PokerHand)  // [1][1][0]
                            {
                                tempPlayer.Add(item);
                            }
                            else if (item.PokerHand == sorted[index + 1].PokerHand && item.PokerHand != sorted[index - 1].PokerHand) // [0][1][1]
                            {
                                if (tempPlayer.Any())
                                {
                                    var result = FindHighestValue(tempPlayer);
                                    result.ForEach(x =>
                                    {
                                        winningOrder.Add(x);
                                    });
                                }
                                tempPlayer.Clear();
                                tempPlayer.Add(item);
                            } else if (item.PokerHand == sorted[index + 1].PokerHand && // [0][1][0]
                                       item.PokerHand == sorted[index - 1].PokerHand)
                            {
                                    tempPlayer.Add(item);
                            }
                            else
                            {
                                if (tempPlayer.Any())
                                {
                                    var result = FindHighestValue(tempPlayer);
                                    result.ForEach(x =>
                                    {
                                        winningOrder.Add(x);
                                    });
                                }
                                tempPlayer.Clear();
                                winningOrder.Add(item);
                            }
                        }
                        else if (index != 0 && index < sorted.Count()) 
                        {
                            if (item.PokerHand == sorted[index - 1].PokerHand)
                                tempPlayer.Add(item);
                        }
                        else
                        {
                            if (tempPlayer.Any())
                            {
                                var result = FindHighestValue(tempPlayer);
                                result.ForEach(x =>
                                {
                                    winningOrder.Add(x);
                                });
                            }
                            tempPlayer.Clear();
                        }
                        break;
                    }
                }
                index++;
            });


            winningOrder.ForEach(player =>
            {
                player.Cards = _modificationService.RevertCards(player.Cards);
            });


            return winningOrder;
        }

        private List<Player> FindHighestValue(List<Player> players)
        {
            var sortedCards = new List<Player>();
            players.ForEach(x =>
            {
                x.Cards = _modificationService.ModifyCards(x.Cards);
            });

            switch (players[0].PokerHand)
            {
                case PokerHand.RoyalFlush:
                    
                    break;
                case PokerHand.StraightFlush:
                    sortedCards = StraightOrder(players);
                    break;
                case PokerHand.FourOfAKind:
                    sortedCards = FourofaKindOrder(players);
                    break;
                case PokerHand.FullHouse:
                    sortedCards = FullHouseOrder(players);
                    break;
                case PokerHand.Flush:
                    break;
                case PokerHand.Straight:
                    sortedCards = StraightOrder(players);
                    break;
                case PokerHand.ThreeOfAKind:
                    sortedCards = ThreeOfAKindOrder(players);
                    break;
                case PokerHand.TwoPair:
                    sortedCards = TwoPairOrder(players);
                    break;
                case PokerHand.Pair:
                    sortedCards = PairOrder(players);
                    break;
                case PokerHand.HighCard:
                    sortedCards = HighCardOrder(players);
                    break;
                case PokerHand.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return sortedCards;
        }

        private List<Player> FourofaKindOrder(List<Player> players)
        {
            var list = new List<Player> {players[0]};

            foreach (var player in players.Skip(1))
            {
                var totalCost = player.Cards
                                            .GroupBy(val => _modificationService.ConvertToNumber(val))
                                            .Where(y => y.Count() == 4)
                                            .Select(z => z.Key).First();

                var currentTotalCost = list[0].Cards
                                                    .GroupBy(val => _modificationService.ConvertToNumber(val))
                                                    .Where(y => y.Count() == 4)
                                                    .Select(z => z.Key).First();

                if (ToInt32(totalCost) > ToInt32(currentTotalCost))
                    list.Insert(0, player);
                else
                    list.Add(player);
            }
            return list;
        }


        private List<Player> FullHouseOrder(List<Player> players)
        {
            var list = new List<Player>{players[0]};
            foreach (var player in players.Skip(1))
            {
                var pairValue = player.Cards
                                            .GroupBy(val => _modificationService.ConvertToNumber(val))
                                            .Where(y => y.Count() == 2)
                                            .Select(z => z.Key).Sum(ToInt32);

                var threePairValue = player.Cards
                                                .GroupBy(val => _modificationService.ConvertToNumber(val))
                                                .Where(y => y.Count() == 3)
                                                .Select(z => z.Key).Sum(ToInt32);


                var currentPair = list[0].Cards
                                                .GroupBy(val => _modificationService.ConvertToNumber(val))
                                                .Where(y => y.Count() == 2)
                                                .Select(z => z.Key).Sum(ToInt32);

                var currentThreePair = list[0].Cards
                                                    .GroupBy(val => _modificationService.ConvertToNumber(val))
                                                    .Where(y => y.Count() == 3)
                                                    .Select(z => z.Key).Sum(ToInt32);

                if ((pairValue + threePairValue) > (currentPair + currentThreePair))
                    list.Insert(0, player);
                else
                    list.Add(player);
            }
            
            return list;
        }

        private List<Player> StraightOrder(List<Player> players)
        {
            var list = new List<Player>{players[0]};

            foreach (var player in players.Skip(1))
            {
                if (player.Cards.Sum(ToInt32) > list[0].Cards.Sum(ToInt32))
                    list.Insert(0, player);
                else
                    list.Add(player);
            }
            
            return list;
        }



        private List<Player> ThreeOfAKindOrder(List<Player> players)
        {
            var list = new List<Player>{players[0]};
            foreach (var player in players)
            {
                var totalCost = player.Cards.GroupBy(val => _modificationService.ConvertToNumber(val)).Where(y => y.Count() == 3).Select(z => z.Key).First();
                var currentTotalCost = list[0].Cards.GroupBy(val => _modificationService.ConvertToNumber(val)).Where(y => y.Count() == 3).Select(z => z.Key).First();
                if (ToInt32(totalCost) > ToInt32(currentTotalCost))
                    list.Insert(0, player);
                else
                    list.Add(player);
            }
            return list;
        }


        private List<Player> TwoPairOrder(List<Player> players)
        {
            var list = new List<Player>{players[0]};
            foreach (var player in players)
            {
                var totalCost = player.Cards.GroupBy(val => _modificationService.ConvertToNumber(val)).Where(y => y.Count() == 2).Select(z => z.Key).ToList();
                var currentTotalCost = list[0].Cards.GroupBy(val => _modificationService.ConvertToNumber(val)).Where(y => y.Count() == 2).Select(z => z.Key).ToList();

                if (totalCost.Sum(ToInt32) > currentTotalCost.Sum(ToInt32))

                    list.Insert(0, player);
                else
                    list.Add(player);
            }

          
            return list;
        }

        private List<Player> PairOrder(List<Player> players)
        {
            var list = new List<Player>{players[0]};
            var index = 0;

            foreach (var player in players.Skip(1))
            {
                var totalCost = player.Cards.GroupBy(val => _modificationService.ConvertToNumber(val)).Where(y => y.Count() == 2).Select(z => z.Key).First();
                var currentTotalCost = list[0].Cards.GroupBy(val => _modificationService.ConvertToNumber(val)).Where(y => y.Count() == 2).Select(z => z.Key).First();
                if (ToInt32(totalCost) > ToInt32(currentTotalCost))
                    list.Insert(0, player);
                else
                    list.Add(player);
            }
            return list;
        }

        private List<Player> HighCardOrder(List<Player> players)
        {
            var list = new List<Player>{players[0]};

            foreach (var player in players)
            {
                if (TotalValue(player.Cards) > TotalValue(list[0].Cards))
                    list.Insert(0, player);
                else
                    list.Add(player);
            }

            return list;
        }


        private int TotalValue(List<string> cards)
        {
            var modify = _modificationService.ModifyCards(cards);
            var totalAmount = 0;
            modify.ForEach(x =>
            {
                totalAmount += ToInt32(x.Remove(x.Length - 1, 1));
            });

            return totalAmount;
        }
    }


    public interface IRankCardService
    {
        List<Player> RankPlayer(List<Player> players);

    }
}
