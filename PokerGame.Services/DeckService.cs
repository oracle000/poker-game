using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using PokerGame.Data;
using System.IO;
using System;

namespace PokerGame.Services
{
    public class DeckService : IDeckService
    {
        private static Cards ReadCardFromJSON()
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

        public string DrawRandomCard()
        {
            var rnd = new Random();
            var result = ReadCardFromJSON();
            return result.Name[rnd.Next(1, 52)];
        }
    }


    public interface IDeckService
    {
        string DrawRandomCard();
    }
}
