using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFTAtHome.Backend.models.Effect;
using TFTAtHome.Backend.models.Rounds;
using TFTAtHome.util.ExtensionMethods;
using static TFTAtHome.Backend.storage.TraitSingleton;

namespace TFTAtHome.Backend.models
{
    public class Match
    {

        // TODO --> Implement logic for MovieHero -> Card needs to get +3 on all stages after a match
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }

        public List<Card> Player1Hand { get; set; }
        public List<Card> Player2Hand { get; set; }
        public List<Card> CurrentCardsOnBoardP1 { get; set; }
        public List<Card> CurrentCardsOnBoardP2 { get; set; }
        public PlayerCardEffects Player1Effects { get; set; }
        public PlayerCardEffects Player2Effects { get; set; }
        public int RoundNumber { get; set; }
        public List<Round> Rounds { get; set; }

        public Match(Player player1, Player player2)
        {
            Player1 = player1;
            Player2 = player2;
            Player1Hand = player1.GetCopyOfPlayerHand();
            Player2Hand = player2.GetCopyOfPlayerHand();
            CurrentCardsOnBoardP1 = new List<Card>();
            CurrentCardsOnBoardP2 = new List<Card>();
            Player1Effects = new PlayerCardEffects(player1);
            Player2Effects = new PlayerCardEffects(player2);
            Rounds = new List<Round>();
            RoundNumber = 1;
        }

        public void AddCardToBoard(Card card, Player player)
        {
            if (player.Id == Player1.Id)
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

        public void RunInitialRound()
        {
            Round initialRound = new SpecialRound(this, "Initial");
            Rounds.Add(initialRound);
            SetCardStatsForMatchForPlayer(Player1);
            SetCardStatsForMatchForPlayer(Player2);
            Player1Effects = new PlayerCardEffects(Player1);
            Player2Effects = new PlayerCardEffects(Player2);
            Player1Effects.SetupMatchEffects(CurrentCardsOnBoardP1);
            Player1Effects.SetupMatchEffects(CurrentCardsOnBoardP2);

            if (Player1Effects.MatchEffects.Count > 0)
            {
                // Kald metode der fortæller spilleren at de skal udføre en action
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

        public List<MatchEffect> GetMatchEffectsForPlayer(Player player)
        {
            return null;
        }

        //  FIX ME GENERALKOCH
        public void SetupPlayerEffects(Player player)
        {
            if (player == Player1)
            {


            }
            else
            {
                // Player2Effects = GetEffectCountForCardOnBoardList(CurrentCardsOnBoardP2);
            }
        }

        private void SetCardStats(List<Card> currentPlayerCardsOnBoard, bool p1)
        {
            bool leaderBonus = currentPlayerCardsOnBoard.GetAllCardsWithTraitOnList(Leader).Count != 0 ?
            CheckLeaderBonus(currentPlayerCardsOnBoard, p1) : false;

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
                List<Card> politicianCards = currentPlayerCardsOnBoard.GetAllCardsWithTraitOnList(Politician);
                foreach (Card card in politicianCards)
                {
                    card.SetPoliticianBonusOnCard(cardBonuses["Politician"]);
                }
            }
            if (currentPlayerCardsOnBoard.CheckTraitIsOnList(TVCelebrity))
            {
                int realCardCount = currentPlayerCardsOnBoard.GetRealCardCountOnListAndOpponent(opponentPlayerActiveBoard);
                int tvCelebrityCount = currentPlayerCardsOnBoard.GetTraitCountOnListAndOpponent(TVCelebrity, opponentPlayerActiveBoard);
                if (tvCelebrityCount > 0) tvCelebrityCount--;
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
                foreach (Card card in drawingCards)
                {
                    card.SetDrawingBonusOnCard(fictionalAndDrawingCount);
                }
            }
        }



        public bool ShouldUseQueenEffectWithIndex(Card cardToUseEffectOn)
        {
            return cardToUseEffectOn.GetSecondBestPhaseOnCard()[1].Length != 0;
        }

        public void UseQueenEffectWithoutIndex(Player player, Card cardToUseEffectOn)
        {
            var bestPhaseProperty = cardToUseEffectOn.GetType().GetProperty(cardToUseEffectOn.GetBestPhaseOnCard());

            if (bestPhaseProperty == null || bestPhaseProperty.PropertyType != typeof(int)) return;
            int bestPhaseValueOnCard = (int)bestPhaseProperty.GetValue(cardToUseEffectOn);
            string secondBestPhase = cardToUseEffectOn.GetSecondBestPhaseOnCard()[0];

            cardToUseEffectOn.GetType().GetProperty(secondBestPhase).SetValue(cardToUseEffectOn, bestPhaseValueOnCard);
        }

        public void UseQueenEffectWithIndex(Player player, Card cardToUseEffectOn, int indexOfSecondBestPhase)
        {
            var bestPhaseProperty = cardToUseEffectOn.GetType().GetProperty(cardToUseEffectOn.GetBestPhaseOnCard());
            if (bestPhaseProperty == null || bestPhaseProperty.PropertyType != typeof(int)) return;
            int bestPhaseValueOnCard = (int)bestPhaseProperty.GetValue(cardToUseEffectOn);

            string secondBestPhase = "";
            switch (indexOfSecondBestPhase)
            {
                case 0:
                    secondBestPhase = "Early";
                    break;
                case 1:
                    secondBestPhase = "Mid";
                    break;
                case 2:
                    secondBestPhase = "Late";
                    break;
                default: return;
            }

            cardToUseEffectOn.GetType().GetProperty(secondBestPhase).SetValue(cardToUseEffectOn, bestPhaseValueOnCard);
        }


        // HELPER METHODS
        private int CheckPoliticianCount(bool p1)
        {
            return p1 ? Player1Hand.Count : Player2Hand.Count;
        }

        private bool CheckLeaderBonus(List<Card> currentBoardPlayer, bool p1)
        {
            if (p1)
            {
                return !CurrentCardsOnBoardP2.CheckTraitIsOnList("Leader");
            }
            else
            {
                return !CurrentCardsOnBoardP1.CheckTraitIsOnList("Leader");
            }
        }
    }


    public class PlayerCardEffects
    {
        public Dictionary<MatchEffect, int> MatchEffects { get; } = new Dictionary<MatchEffect, int>();
        public Player Player { get; set; }
        public bool CanUseNewEffect { get; set; }

        public PlayerCardEffects(Player player)
        {
            Player = player;
            CanUseNewEffect = true;
        }

        public void SetupMatchEffects(List<Card> currentCardsOnBoard)
        {
            if (currentCardsOnBoard.CheckTraitIsOnList(Queen))
            {
                int count = currentCardsOnBoard.GetAllCardsWithTraitOnList(Queen).Count;
                MatchEffects.Add(new MatchEffect(Queen, true), count);
            }
            if (currentCardsOnBoard.CheckTraitIsOnList(EarlyPeaker))
            {
                int count = currentCardsOnBoard.GetAllCardsWithTraitOnList(EarlyPeaker).Count;
                MatchEffects.Add(new MatchEffect(EarlyPeaker, false), count);
            }
            if (currentCardsOnBoard.CheckTraitIsOnList(Genius))
            {
                int count = currentCardsOnBoard.GetAllCardsWithTraitOnList(Genius).Count;
                MatchEffects.Add(new MatchEffect(Genius, true), count);
            }
            if (currentCardsOnBoard.CheckTraitIsOnList(Musician))
            {
                int count = currentCardsOnBoard.GetAllCardsWithTraitOnList(Musician).Count;
                MatchEffects.Add(new MatchEffect(Musician, true), count);
            }
            if (currentCardsOnBoard.CheckTraitIsOnList(Le))
            {
                MatchEffects.Add(new MatchEffect(Le, false), 1);
            }
        }

        public bool UseMatchEffect(MatchEffect matchEffect)
        {
            if (MatchEffects.ContainsKey(matchEffect))
            {
                if (MatchEffects[matchEffect] > 1)
                {
                    MatchEffects[matchEffect]--;
                    return MatchEffects[matchEffect] != 0;
                } else
                {
                    return false;
                }
            }
            return false;
        }
    }
}
