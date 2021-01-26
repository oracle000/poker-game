using System.Collections.Generic;
using System.Dynamic;

namespace PokerGame.Data
{
    public class Player
    {
        public string Name { get; set; }
        public List<string> Cards { get; set; }
        public PokerHand PokerHand { get; set; }
        public bool Winner { get; set; }
    }
}
