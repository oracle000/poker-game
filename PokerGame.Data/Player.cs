using System.Collections.Generic;
using System.Dynamic;

namespace PokerGame.Data
{
    public class Player
    {
        public string Name { get; set; }
        public List<PlayerCards> Cards { get; set; }
        public bool Winner { get; set; }
    }
}
