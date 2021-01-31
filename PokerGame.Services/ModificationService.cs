using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;


namespace PokerGame.Services
{
    public class ModificationService : IModificationService
    {

        public List<string> ModifyCards(List<string> cards)
        {
            var tempCard = new List<string>();

            cards.ForEach(x =>
            {
                var tempValue = "";
                switch (x.First())
                {
                    case 'A':
                        tempValue = $"14{x.Last()}";
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

        public List<string> RevertCards(List<string> cards)
        {
            var tempCard = new List<string>();

            cards.ForEach(x =>
            {
                var tempValue = "";
                switch (ConvertToNumber(x))
                {
                    case "14":
                        tempValue = $"A{x.Last()}";
                        break;
                    case "11":
                        tempValue = $"J{x.Last()}";
                        break;
                    case "12":
                        tempValue = $"Q{x.Last()}";
                        break;
                    case "13":
                        tempValue = $"K{x.Last()}";
                        break;
                    default:
                        tempValue = x;
                        break;
                }
                tempCard.Add(tempValue);
            });

            return tempCard;
        }

        public string ConvertToNumber(string value)
        {
            var number = Regex.Split(value, @"\D+");
            return number.FirstOrDefault();
        }
    }

    public interface IModificationService
    {
        List<string> ModifyCards(List<string> cards);
        List<string> RevertCards(List<string> cards);
        string ConvertToNumber(string value);

    }

}
