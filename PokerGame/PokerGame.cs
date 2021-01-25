using System;
using System.Collections.Generic;
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

            var newCard = deckService.DrawRandomCard();

            listOfPlayer.ForEach(x =>
            {
                x.Cards.ForEach(y =>
                {
                    y.CardNumber = deckService.DrawRandomCard();
                });
            });

            var listOfCards = deckService.DrawAllCard();

        }

        public static bool CheckForDuplicate(Player player, string newCard)
        {
            return (player.Cards.Any(x => x.CardNumber == newCard));
        }

    }
}
