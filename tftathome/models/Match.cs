using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFTAtHome.util.ExtensionMethods;
using static TFTAtHome.storage.TraitSingleton;
 
namespace TFTAtHome.models
{
    public class Match
    {
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }

        public List<Card> Player1Hand { get; set; }
        public List<Card> Player2Hand { get; set; }
        public List<Card> CurrentCardsOnBoardP1 { get; set; }
        public List<Card> CurrentCardsOnBoardP2 { get; set; }
        // public List<Effect> Player1Effects { get; set; }
        // public List<Effect> Player2Effects { get; set; }
        public int RoundNumber { get; set; }
        // public List<Round> Rounds  { get; set; }

        public Match(Player player1, Player player2)
        {
            Player1 = player1;
            Player2 = player2;
            Player1Hand = player1.GetCopyOfPlayerHand();
            Player2Hand = player2.GetCopyOfPlayerHand();
            CurrentCardsOnBoardP1 = new List<Card>();
            CurrentCardsOnBoardP2 = new List<Card>();
            RoundNumber = 1;
        }

        public void AddCardToBoard(Card card, Player player)
        {
            if (player == Player1)
            {
                Player1Hand.Remove(card);
                CurrentCardsOnBoardP1.Add(card);
            }
            else
            {
                Player2Hand.Remove(card);
                CurrentCardsOnBoardP2.Add(card);
            }
        }

        public void RemoveCardFromBoard(Card card, Player player)
        {
            if (player == Player1)
            {
                Player1Hand.Add(card);
                CurrentCardsOnBoardP1.Remove(card);
            }
            else
            {
                Player2Hand.Add(card);
                CurrentCardsOnBoardP2.Remove(card);
            }
        }

        public void SetCardStatsForMatchForPlayer(Player player)
        {
            if (player == Player1)
            {
               SetCardStats(CurrentCardsOnBoardP1, true);
            }
            else
            {
               SetCardStats(CurrentCardsOnBoardP2, false);
            }
        }
        private void SetCardStats(List<Card> currentPlayerCardsOnBoard, bool p1)
        {

            bool leaderBonus = CheckLeaderBonus(currentPlayerCardsOnBoard, p1);
            Dictionary<string, int> cardBonuses = new Dictionary<string, int>();
            List<Card> opponentPlayerActiveBoard = p1 ? CurrentCardsOnBoardP2 : CurrentCardsOnBoardP1;

            // TIME TO CHECK STATS BITCHES

            if (leaderBonus)
            {
                Card leaderCard = currentPlayerCardsOnBoard.GetAllCardsWithTraitOnList(Leader)[0];
                leaderCard.Early += 3;
                leaderCard.Mid += 3;
                leaderCard.Late += 3;
            }

            if (currentPlayerCardsOnBoard.CheckTraitIsOnList(Politician))
            {
                cardBonuses.Add("Politician", CheckPoliticianCount(p1));
                if (leaderBonus)
                {
                    cardBonuses["Politician"]++;
                }
            }
            if (currentPlayerCardsOnBoard.CheckTraitIsOnList(TVCelebrity))
            {
                int realCardCount = currentPlayerCardsOnBoard.GetRealCardCountOnListAndOpponent(opponentPlayerActiveBoard);
                int tvCelebrityCount = currentPlayerCardsOnBoard.GetTraitCountOnListAndOpponent(TVCelebrity,opponentPlayerActiveBoard);
                if (leaderBonus)
                {
                    tvCelebrityCount++;
                }
                List<Card> tvCelebrityCards = currentPlayerCardsOnBoard.GetAllCardsWithTraitOnList(TVCelebrity);

                foreach (Card card in tvCelebrityCards)
                {
                    card.SetTVCelebrityBonusOnCard(realCardCount, tvCelebrityCount);
                }
            }
            if (currentPlayerCardsOnBoard.CheckTraitIsOnList(Drawing))
            {
                int[] fictionalAndDrawingCount = currentPlayerCardsOnBoard.GetFictionalAndDrawingCountOnListAndOpponent(opponentPlayerActiveBoard);
                if (leaderBonus)
                {
                    fictionalAndDrawingCount[1]++;
                }
                List<Card> drawingCards = currentPlayerCardsOnBoard.GetAllCardsWithTraitOnList(Drawing);
                foreach (Card card in drawingCards) {
                    card.SetDrawingBonusOnCard(fictionalAndDrawingCount);
                }
            }


        }
        private int CheckPoliticianCount(bool p1)
        {
            return p1 ? Player1.GetPlayerHand().Count: Player2.GetPlayerHand().Count;
        }

        private bool CheckLeaderBonus(List<Card> currentBoardPlayer, bool p1)
        {
            if (p1)
            {
                return !CurrentCardsOnBoardP2.CheckTraitIsOnList("Leader");
            } else
            {
                return !CurrentCardsOnBoardP1.CheckTraitIsOnList("Leader");
            } 
        }

    }
}
