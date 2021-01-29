using System.Collections.Generic;
using PokerGame.Services;
using PokerGame.Data;
using System.Linq;
using System;

namespace PokerGame
{
    public class PokerGame 
    {

        static void Main(string[] args)
        {

            var deckService = new DeckService();
            var pokerHandService = new PokerHandService();
            var listOfPlayer = new List<Player>();
            var playerRanking = new List<Player>();
            var tempRandomNumber = new List<string>();
            bool added;

            Console.WriteLine(@"********************** POKER GAME *************************");
            Console.Write(@"How many Player: ");
            var numberOfUser = Console.ReadLine();

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
            Console.WriteLine("----------------------------------");


            listOfPlayer.ForEach(x =>
            {
                x.Cards = new List<string>();
                for (var i = 0; i < 5; i++)
                {
                    added = false;
                    do
                    {
                        var newCard = deckService.DrawRandomCard();
                        if (!tempRandomNumber.All(z => z != newCard)) continue;
                        
                        x.Cards.Add(newCard);
                        
                        tempRandomNumber.Add(newCard);

                        added = true;
                    } while (!added);
                }
            });


            listOfPlayer.ForEach(x =>
            {
                x.PokerHand = pokerHandService.CheckCards(x.Cards);
            });

            DisplayCard(listOfPlayer);

            // identify who is the winner


            // identify based on rank

            playerRanking = pokerHandService.RankPlayers(listOfPlayer);

            Console.WriteLine("------------------------");
            // DisplayCard(playerRanking);

        }


        public static void DisplayCard(List<Player> player)
        {
            player.ForEach(x =>
            {
                
                Console.Write($@"{x.Name} Cards : ");
                x.Cards.ForEach(y =>
                {
                    Console.Write(y);
                    Console.Write(" ");
                });
                Console.WriteLine();
                Console.WriteLine($"Poker Hand : {x.PokerHand}");

                Console.WriteLine("----------------------------------");
            });
        }

       
    }
}
