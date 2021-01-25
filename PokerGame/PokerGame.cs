using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using PokerGame.Data;
using PokerGame.Services;

namespace PokerGame
{
    public class PokerGame
    {
        
        static void Main(string[] args)
        {

            var deckService = new DeckService();
            var numberOfUser = "";
            var listOfPlayer = new List<Player>();

            Console.WriteLine(@"********************** POKER GAME *************************");
            Console.Write(@"How many Player: ");
            numberOfUser = Console.ReadLine();

            Console.WriteLine();
            for (var i = 0; i < Convert.ToInt32(numberOfUser); i++)
            {
                var player = new Player();
                
                Console.Write(@"Enter Players Name : ");
                player.Name = Console.ReadLine();

                listOfPlayer.Add(player);
            }
            
            Console.WriteLine();
            Console.WriteLine(@"Generate Cards for player");


            listOfPlayer.ForEach(x =>
            {
                x.Cards = new List<PlayerCards>();
                for (var i = 0; i < 5; i++)
                {
                    x.Cards.Add(new PlayerCards
                    {
                        CardNumber = deckService.DrawRandomCard()
                    });

                    //var added = false;
                    //while (added != true)
                    //{
                    //    var newCard = deckService.DrawRandomCard();
                    //    if (!listOfPlayer.Any(x => x.Cards.Select(y => y.CardNumber == newCard).FirstOrDefault()))
                    //    {
                    //        x.Cards.Add(new PlayerCards
                    //        {
                    //            CardNumber = deckService.DrawRandomCard()
                    //        });
                    //        added = true;
                    //    }
                    //}
                }
            });

            DisplayCard(listOfPlayer);

            var listOfCards = deckService.DrawAllCard();

        }

        public static void DisplayCard(List<Player> player)
        {
            player.ForEach(x =>
            {
                Console.Write($@"{x.Name} Cards : ");
                x.Cards.ForEach(y =>
                {
                    Console.Write(y.CardNumber);
                    Console.Write(" ");
                });
                Console.WriteLine();
            });
        }

        public static bool CheckForDuplicate(Player player, string newCard)
        {
            return (player.Cards.Any(x => x.CardNumber == newCard));
        }

    }
}
