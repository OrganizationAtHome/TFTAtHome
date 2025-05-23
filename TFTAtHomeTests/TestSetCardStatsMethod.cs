using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TFTAtHome.Backend.models;
using TFTAtHome.Backend.models.Matches;
using TFTAtHome.Backend.storage;
using static TFTAtHome.Backend.storage.TraitSingleton;

namespace TFTAtHomeTests
{
    [TestClass]
    public class TestSetCardStatsMethod
    {

        private Card card1 = new Card(1, "LIGHT YAGAMI", "THE FIRST KIRA", "images/cardimages/light-yagami.jpeg", 6, 12, 1, Genius, 3, true);
        private Card card2 = new Card(2, "ADOLF HITLER", "THE FÜHRER", "images/cardimages/adolf-hitler.jpg", 9, 9, 1, Politician, 4, false);
        private Card card8 = new Card(8, "ELON MUSK", "THE CEO", "images/cardimages/elon-musk.jpg", 5, 7, 6, Leader, 3, false);
        private Card card13 = new Card(13, "JESUS CHRIST", "THE SON OF GOD", "images/cardimages/jesus-christ.jpg", 12, 1, 6, Leader, 4, false);
        private Card card3 = new Card(3, "CHUCK NORRIS", "THE TEXAS RANGER", "images/cardimages/chuck-norris.jpg", 7, 8, 3, TVCelebrity, 3, false);
        private Card card4 = new Card(4, "DEADPOOL", "THE SUPERHERO REJECT", "images/cardimages/deadpool.jpg", 5, 1, 11, MovieHero, 1, true);
        private Card card7 = new Card(7, "ELIZABETH II", "THE QUEEN OF ENGLAND", "images/cardimages/elizabeth-2.jpg", 3, 7, 7, Queen, 3, false);
        private Card card9 = new Card(9, "ERIC CARTMAN", "THE FAT KID", "images/cardimages/eric-cartman.jpg", 5, 8, 5, Drawing, 1, true);

        [TestMethod]
        public void TestMatchWithSingleLeaderCard()
        {
            // Arrange
            Player player1 = new Player(1, "Player 1");
            Player player2 = new Player(2, "Player 2");

            player1.GetPlayerHand().Add(card8);
            player1.GetPlayerHand().Add(card2);
            player2.GetPlayerHand().Add(card4);
            player2.GetPlayerHand().Add(card7);
            player2.GetPlayerHand().Add(card9);

            int[] oldStatsLeaderCard = {card8.Early, card8.Mid, card8.Late };

            Match match = new Match(player1, player2);

            match.AddCardToBoard(card8, player1);
            match.AddCardToBoard(card2, player1);
            match.AddCardToBoard(card4, player2);
            match.AddCardToBoard(card7, player2);
            match.AddCardToBoard(card9, player2);

            // Act
            match.SetCardStatsForMatchForPlayer(player1);

            // Assert

            // Testing Leader early
            Assert.AreEqual(oldStatsLeaderCard[0] + 3, match.CurrentCardsOnBoardP1[0].Early);

            // Testing Leader mid
            Assert.AreEqual(oldStatsLeaderCard[1] + 3, match.CurrentCardsOnBoardP1[0].Mid);

            // Testing Leader Late
            Assert.AreEqual(oldStatsLeaderCard[2] + 3, match.CurrentCardsOnBoardP1[0].Late);

        }

        [TestMethod]
        public void TestMatchWithLeaderCardsForBothPlayers()
        {
            // Arrange
            Player player1 = new Player(1, "Player 1");
            Player player2 = new Player(2, "Player 2");

            player1.GetPlayerHand().Add(card8);
            player2.GetPlayerHand().Add(card13);
            player1.GetPlayerHand().Add(card2);
            player2.GetPlayerHand().Add(card4);
            player2.GetPlayerHand().Add(card9);

            Match match = new Match(player1, player2);

            match.AddCardToBoard(card8, player1);
            match.AddCardToBoard(card2, player1);
            match.AddCardToBoard(card4, player2);
            match.AddCardToBoard(card13, player2);
            match.AddCardToBoard(card9, player2);

            // Act
            match.SetCardStatsForMatchForPlayer(player1);
            match.SetCardStatsForMatchForPlayer(player2);

            // Assert
            Assert.AreEqual(card8.Late, match.CurrentCardsOnBoardP1[0].Late);
            Assert.AreEqual(card13.Mid, match.CurrentCardsOnBoardP2[0].Mid);
        }
        [TestMethod]
        public void TestMatchWithPoliticianCardAndAddedCardsToPlayerHand()
        {

            // Politician Card is Card 2
            // Arrange
            Player player4 = new Player(4, "Player 1");
            Player player6 = new Player(6, "Player 2");

            player4.SetPlayerHand(new List<Card> { card2, card7, card9, card4, card8, card1 });
            player6.SetPlayerHand(new List<Card> { card4, card8, card3 });



            Match match = new Match(player4, player6);

            Console.WriteLine("Player1Hand");
            Console.WriteLine(String.Join(", ", match.Player1Hand));
            Console.WriteLine("Player2Hand");
            Console.WriteLine(String.Join(", ", match.Player2Hand));

            // Player 4 adds cards to the board
            match.AddCardToBoard(match.Player1Hand[0], player4); // Adolf Hitler (Politician): 9, 9, 1, false
            match.AddCardToBoard(match.Player1Hand[0], player4); // Elizabeth II (Queen): 3, 7, 7, false
            match.AddCardToBoard(match.Player1Hand[0], player4); // Eric Cartman (Drawing): 5, 8, 5, true

            Console.WriteLine("Player hand count: " + match.Player1Hand.Count);

            
            var cardOnBoard = match.CurrentCardsOnBoardP1[0];


            // Player 6 adds cards to the board
            match.AddCardToBoard(match.Player2Hand[0], player6); // Deadpool (MovieHero): 5, 1, 11, false
            match.AddCardToBoard(match.Player2Hand[0], player6); // Elon Musk (Leader): 5, 7, 6, true
            match.AddCardToBoard(match.Player2Hand[0], player6); // Chuck Norris (TVCelebrity): 7, 8, 3, true

            
            // Act
            match.SetCardStatsForMatchForPlayer(player4);
            var cardOnBoard2 = match.CurrentCardsOnBoardP1[0];
            Console.WriteLine("Test card: " + match.CurrentCardsOnBoardP1[0].CardName);
            // Assert
            Assert.AreEqual(card2.Early + 3, match.CurrentCardsOnBoardP1[0].Early);
        }
        [TestMethod]
        public void TestOnePlayerGetsDrawingCard()
        {
            // Arrange
            Player player1 = new Player(1, "Player 1");
            Player player2 = new Player(2, "Player 2");

            // Player 1 gets Eric Cartman (Drawing), Deadpool (MovieHero), and Light Yagami (Genius)
            player1.SetPlayerHand(new List<Card> { card9, card4, card1 });
            // Player 2 gets Deadpool (MovieHero), Light Yagami (Genius), and Elon Musk (Leader)
            player2.SetPlayerHand(new List<Card> { card4, card1, card8 });

            Match match = new Match(player1, player2);

            // Act: Adding cards to the board
            // Player 1 adds Eric Cartman
            match.AddCardToBoard(match.Player1Hand[0], player1); // Eric Cartman (Drawing): 5, 8, 5, true
            match.AddCardToBoard(match.Player1Hand[0], player1); // Deadpool (MovieHero): 5, 1, 11, true
            match.AddCardToBoard(match.Player1Hand[0], player1); // Light Yagami (Genius): 6, 12, 1, true

            match.AddCardToBoard(match.Player2Hand[0], player2); // Deadpool (MovieHero): 5, 1, 11, true                                             
            match.AddCardToBoard(match.Player2Hand[0], player2); // Light Yagami (Genius): 6, 12, 1, true
            match.AddCardToBoard(match.Player2Hand[0], player2); // Elon Musk (Leader): 5, 7, 6, false

            Console.WriteLine();


            // Assert: Check Eric Cartman's updated stats
            match.SetCardStatsForMatchForPlayer(player1);
            Assert.AreEqual(9, match.CurrentCardsOnBoardP1[0].Early); 
        }
        
        [TestMethod]
        public void TestBothPlayersGetEricCartman()
        {
            // Arrange
            Player player1 = new Player(1, "Player 1");
            Player player2 = new Player(2, "Player 2");

            // Both players get Eric Cartman (Drawing) and two random cards
            player1.SetPlayerHand(new List<Card> { card9, card4, card1 });
            player2.SetPlayerHand(new List<Card> { card9, card8, card3 });

            Match match = new Match(player1, player2);

            // Act: Adding cards to the board
            // Player 1 adds Eric Cartman
            match.AddCardToBoard(match.Player1Hand[0], player1); // Eric Cartman (Drawing): 5, 8, 5, true
             
            match.AddCardToBoard(match.Player2Hand[0], player2); // Eric Cartman (Drawing): 5, 8, 5, true
       
            match.AddCardToBoard(match.Player1Hand[0], player1); // Deadpool (MovieHero): 5, 1, 11, true

            match.AddCardToBoard(match.Player2Hand[0], player2); // Elon Musk (Leader): 5, 7, 6, false
   
            match.AddCardToBoard(match.Player1Hand[0], player1); // Light Yagami (Genius): 6, 12, 1, true
  
            match.AddCardToBoard(match.Player2Hand[0], player2); // Chuck Norris (TVCelebrity): 7, 8, 3, false

            // Assert: Check Eric Cartman's updated stats for both players
            match.SetCardStatsForMatchForPlayer(player1);
            match.SetCardStatsForMatchForPlayer(player2);

            Assert.AreEqual(11, match.CurrentCardsOnBoardP1[0].Early); // +4 for Drawings, +2 from fictional
            Assert.AreEqual(13, match.CurrentCardsOnBoardP2[0].Early); // Same but 1+ Leader bonus
        }



        [TestMethod]
        public void TestOnePlayerGets2DrawingCards()
        {
            // Arrange
            Player player1 = new Player(1, "Player 1");
            Player player2 = new Player(2, "Player 2");

            // Both players get Eric Cartman (Drawing) and two random cards
            player1.SetPlayerHand(new List<Card> { card9, card9, card1 });
            player2.SetPlayerHand(new List<Card> { card4, card1, card8 });

            Match match = new Match(player1, player2);

            // Act: Adding cards to the board
            // Player 1 adds Eric Cartman
            match.AddCardToBoard(match.Player1Hand[0], player1); // Eric Cartman (Drawing): 5, 8, 5, true

            match.AddCardToBoard(match.Player2Hand[0], player2); // Deadpool (MovieHero): 5, 1, 11, true

            match.AddCardToBoard(match.Player1Hand[0], player1); // Eric Cartman (Drawing): 5, 8, 5, 

            match.AddCardToBoard(match.Player2Hand[0], player2); // Light Yagami (Genius): 6, 12, 1, true

            match.AddCardToBoard(match.Player1Hand[0], player1); // Light Yagami (Genius): 6, 12, 1, true


            // Assert: Check Eric Cartman's updated stats for both players
            match.SetCardStatsForMatchForPlayer(player1);
            match.SetCardStatsForMatchForPlayer(player2);

            Assert.AreEqual(11, match.CurrentCardsOnBoardP1[0].Early); // +2 for Drawing, +1 for each other true card
        }

        [TestMethod]
        public void TestOnePlayerWithDrawingCard_3FictionalCardsOnBoard()
        {
            // Arrange
            Player player1 = new Player(1, "Player 1");
            Player player2 = new Player(2, "Player 2");


            player1.SetPlayerHand(new List<Card> { card9, LocalStorage.GetRandomFictionalCardNotDrawing(), card2 });
            player2.SetPlayerHand(new List<Card> { LocalStorage.GetRandomFictionalCardNotDrawing(), LocalStorage.GetRandomFictionalCardNotDrawing(), card2 });

            Match match = new Match(player1, player2);

            // Act: Adding cards to the board
            // Player 1 adds Eric Cartman
            match.AddCardToBoard(match.Player1Hand[0], player1); // Eric Cartman (Drawing): 5, 8, 5, true

            match.AddCardToBoard(match.Player2Hand[0], player2);

            match.AddCardToBoard(match.Player1Hand[0], player1);

            match.AddCardToBoard(match.Player2Hand[0], player2); 

            match.AddCardToBoard(match.Player1Hand[0], player1); 
            match.AddCardToBoard(match.Player2Hand[0], player2);


            // Assert: Check Eric Cartman's updated stats for both players
            match.SetCardStatsForMatchForPlayer(player1);
            match.SetCardStatsForMatchForPlayer(player2);

            Assert.AreEqual(7, match.CurrentCardsOnBoardP1[0].Early); // +0 for Drawing, +1 for each other true card
        }
    }
}
