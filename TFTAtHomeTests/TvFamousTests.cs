using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TFTAtHome.Backend.models;
using TFTAtHome.Backend.models.Matches;
using static TFTAtHome.Backend.storage.TraitSingleton;

namespace TFTAtHomeTests
{
    [TestClass]
    public class TvFamousTests
    {

        [TestMethod]
        public void TestTVFamousBonus_1TVFamousP1_2RealLegends()
        {
            Card TVStarZimmer = new Card(52, "ZimmerSomRockStjerne", "GZim", "images/cardimages/light-yagami.jpeg", 55, 2, 1, TVCelebrity, 3, false);
            Card AegteKoch = new Card(50, "Kocher", "ToiletMesteren", "images/cardimages/light-yagami.jpeg", 3, 2, 1, Genius, 3, false);
            Card randomCard = new Card(54, "Kocher", "ToiletMesteren", "images/cardimages/light-yagami.jpeg", 3, 2, 1, Genius, 3, true);
            // Arrange
            Player player1 = new Player(1, "Player 1");
            Player player2 = new Player(2, "Player 2");

            // Both players get Eric Cartman (Drawing) and two random cards
            player1.SetPlayerHand(new List<Card> { TVStarZimmer, randomCard, AegteKoch });
            player2.SetPlayerHand(new List<Card> { AegteKoch, AegteKoch, randomCard });

            Match match = new Match(player1, player2);

            // Act: Adding cards to the board
            // Player 1 adds Eric Cartman
            match.AddCardToBoard(match.Player1Hand[0], player1); // TVStar

            match.AddCardToBoard(match.Player1Hand[0], player1);  // Fictional

            match.AddCardToBoard(match.Player1Hand[0], player1);  // Real

            match.AddCardToBoard(match.Player2Hand[0], player2);  // Real

            match.AddCardToBoard(match.Player2Hand[0], player2);  // Real

            match.AddCardToBoard(match.Player2Hand[0], player2);  // Fictional

            // Assert:
            match.SetCardStatsForMatchForPlayer(player1);
            match.SetCardStatsForMatchForPlayer(player2);

            var test = match.CurrentCardsOnBoardP1[0];

            Console.Write("Idk");
            Assert.AreEqual(59, match.CurrentCardsOnBoardP1[0].Early); // +2 from real in best phase
            Assert.AreEqual(2, match.CurrentCardsOnBoardP1[0].Mid);
        }
    }
}
