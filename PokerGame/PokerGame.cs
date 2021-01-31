using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using PokerGame.Services;
using PokerGame.Data;
using System.Linq;
using System;

namespace PokerGame
{
    public class PokerGame 
    {

        public static void Main(string[] args)
        {
            var service = ConfigureService();
            var rankService = service.GetService<IRankCardService>();
            var deckService = service.GetService<IDeckService>();
            var pokerHandService = service.GetService<IPokerHandService>();


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
            Console.WriteLine(@"----------------------------------");


            listOfPlayer.ForEach(x =>
            {
                x.Cards = new List<string>();
                for (var i = 0; i < 5; i++)
                {
                    added = false;
                    do
                    {
                        var newCard = deckService?.DrawRandomCard();
                        if (tempRandomNumber.Any(z => z == newCard)) continue;
                        
                        x.Cards.Add(newCard);
                        
                        tempRandomNumber.Add(newCard);

                        added = true;
                    } while (!added);
                }
            });

            var seedPlayer = new Player
            {
                Cards = new List<string> {"5C", "AC", "4C", "2C", "3C"},
                Name = "chris",
            };
            
            listOfPlayer.Add(seedPlayer);

            listOfPlayer.ForEach(x =>
            {
                if (pokerHandService != null) x.PokerHand = pokerHandService.CheckCards(x.Cards);
            });

            DisplayCards(listOfPlayer);

            if (rankService != null) playerRanking = rankService.RankPlayer(listOfPlayer);

            Console.WriteLine();
            Console.WriteLine($@"Winner is : {playerRanking[0].Name} with {playerRanking[0].PokerHand} Cards");
            DisplayPerCard(playerRanking[0].Cards);
            Console.WriteLine();
            Console.ReadLine();
        }


        private static ServiceProvider ConfigureService()
        {
            var service = new ServiceCollection()
                .AddSingleton<IDeckService, DeckService>()
                .AddSingleton<IPokerHandService, PokerHandService>(t => new PokerHandService(new ModificationService()))
                .AddSingleton<IRankCardService, RankCardService>(t => new RankCardService(new ModificationService()))
                .BuildServiceProvider();
            return service;
        }

        public static void DisplayCards(List<Player> player)
        {
            player.ForEach(x =>
            {
                
                Console.Write($@"{x.Name} Cards : ");
                DisplayPerCard(x.Cards);
                Console.WriteLine();
                Console.WriteLine($@"Poker Hand : {x.PokerHand}");
                Console.WriteLine(@"----------------------------------");
            });
        }

        public static void DisplayPerCard(List<string> cards)
        {
            cards.ForEach(card =>
            {
                Console.Write(card + " ");
            });
        }
       
    }
}
