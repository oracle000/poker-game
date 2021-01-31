using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerGame.Services;

namespace PokerGame.Test
{
    [TestClass]
    public class DeckServiceTest
    {
        private DeckService _deckService;

        [TestInitialize]
        public void Initialize()
        {
            // arrange
            _deckService = new DeckService();
        }

        [TestMethod]
        public void Can_Execute_DrawRandomCard()
        {
            // act
            var result = _deckService.DrawRandomCard();
            
            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(string));
        }
    }
}
