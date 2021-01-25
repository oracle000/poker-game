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
        public Cards ReadCard()
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
            return ReadCard();
        }

        public string DrawRandomCard()
        {
            var rnd = new Random();
            var result = ReadCard();
            return result.Name[rnd.Next(1, 52)].ToString();
        }
    }

    public interface IDeckService
    {
        Cards DrawAllCard();
        string DrawRandomCard();
    }
}
