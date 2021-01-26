using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using PokerGame.Data;
using System.IO;

namespace PokerGame.Services
{
    public class DeckService : IDeckService
    {
        public Cards ReadCardFromJSON()
        {
            string jsonFromFile;
            var result = new Cards {Name = new List<string>()};
            
            using (var reader = new StreamReader($"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}/cards.json"))
                jsonFromFile = reader.ReadToEnd();

            JsonConvert.DeserializeObject<List<Card>>(jsonFromFile).ForEach(x =>
            {
                result.Name.Add(x.Name);
            });

            return result;
        }

        public Cards DrawAllCard()
        {
            return ReadCardFromJSON();
        }

        public string DrawRandomCard()
        {
            var rnd = new Random();
            var result = ReadCardFromJSON();
            return result.Name[rnd.Next(1, 52)];
        }
    }

    public interface IDeckService
    {
        Cards DrawAllCard();
        string DrawRandomCard();
    }
}
