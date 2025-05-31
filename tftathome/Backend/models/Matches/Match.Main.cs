using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
using TFTAtHome.Backend.models.Effect;
using TFTAtHome.Backend.models.Rounds;
using TFTAtHome.Backend.notifiers;
using TFTAtHome.util.ExtensionMethods;
using static TFTAtHome.Backend.storage.TraitSingleton;

namespace TFTAtHome.Backend.models.Matches
{
    public partial class Match
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
        public Round CurrentRound { get; set; }
        public List<MatchEffect> UsedMatchEffects { get; set; }
        

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
            EffectNotifier.OnEffectUsed += OnEffectUsed;
            EffectNotifier.OnGeniusEffectUsed += OnGeniusEffectUsed;
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
            Round initialRound = new EffectRound(this);
            Rounds.Add(initialRound);
            this.CurrentRound = initialRound;
            SetCardStatsForMatchForPlayer(Player1);
            SetCardStatsForMatchForPlayer(Player2);
            Player1Effects = new PlayerCardEffects(Player1);
            Player2Effects = new PlayerCardEffects(Player2);
            Player1Effects.SetupMatchEffects(CurrentCardsOnBoardP1);
            Player1Effects.SetupMatchEffects(CurrentCardsOnBoardP2);
            /*
            if (Player1Effects.MatchEffects.Count > 0)
            {
                EffectNotifier.NotifyNeedsToUseEffect(Player1);
            } */
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

        private void OnEffectUsed(int cardId)
        {
            int length = CurrentCardsOnBoardP1.Count;
            foreach (var card in CurrentCardsOnBoardP1)
            {
                if (card.Id == cardId)
                {
                    var currentEffectRound = CurrentRound as EffectRound;
                    if (currentEffectRound == null) throw new Exception("CurrentRound is null you absolute piece of filth in OnEffectUsed in Match");
                    UseMatchEffectOnCard(card, currentEffectRound.CurrentEffect);
                    currentEffectRound.IsUsingEffect = false;
                    currentEffectRound.CurrentEffect = null;
                    var nextPlayer = GetPlayerThatCanUseNextEffect();
                    EffectNotifier.NotifyCardEffectUpdate(nextPlayer);
                }
            }
            int length2 = CurrentCardsOnBoardP2.Count;
            foreach (var card in CurrentCardsOnBoardP2)
            {
                if (card.Id == cardId)
                {
                    var currentEffectRound = CurrentRound as EffectRound;
                    if (currentEffectRound == null) throw new Exception("CurrentRound is null you absolute piece of filth in OnEffectUsed in Match");
                    UseMatchEffectOnCard(card, currentEffectRound.CurrentEffect);
                    currentEffectRound.IsUsingEffect = false;
                    currentEffectRound.CurrentEffect = null;
                    var nextPlayer = GetPlayerThatCanUseNextEffect();
                    EffectNotifier.NotifyCardEffectUpdate(nextPlayer);
                }
            }
        }

        private void OnGeniusEffectUsed(GeniusEffect geniusEffect)
        {
            Card card = null;
            var p1Card = CurrentCardsOnBoardP1.Find(c => c.Id == geniusEffect.CardId);
            var p2Card = CurrentCardsOnBoardP2.Find(c => c.Id == geniusEffect.CardId);
            
            // Selects the found card
            card = p1Card != null ? p1Card : p2Card;

            if (card == null)
            {
                throw new Exception("You royally screwed up your OnGeniusEffectUsed logic, you retarded lama");
            }
            var currentEffectRound = CurrentRound as EffectRound;
            if (currentEffectRound == null) throw new Exception("CurrentRound is null you absolute piece of filth in OnGeniusEffectUsed in Match");
            UseMatchEffectOnCard(card, currentEffectRound.CurrentEffect);
            
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
}
