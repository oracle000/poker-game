﻿using System.Collections.Generic;
using PokerGame.Data;
using System.Linq;
using System;

namespace PokerGame.Services
{
    /// <summary>
    /// CardRankService - In-charge of identifying what is the rank of the players Cards (Royal Flush, Flush ...
    /// </summary>
    public class CardRankService : ICardRankService
    {
        private readonly IModificationService _modificationService;

        public CardRankService(IModificationService modificationService)
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

        private bool RoyalFlush(List<string> cards) 
        {
            string[] value = {"A", "K", "Q", "J", "10"}; 
            var allMatch = true;

            cards.ForEach(x => 
            {
                if (!value.Any(x.Contains))
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
                tempValue.Add(x.Contains("14")
                    ? Convert.ToInt32(_modificationService.ConvertToNumber($"1{x.Last()}"))
                    : Convert.ToInt32(_modificationService.ConvertToNumber(x)));
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

        private bool Pair(List<string> cards)
        {
            return cards.GroupBy(x => _modificationService.ConvertToNumber(x)).Any(y => y.Count() == 2);
        }
    }


    public interface ICardRankService
    {
        PokerHand CheckCards(List<string> cards);

    }
}


