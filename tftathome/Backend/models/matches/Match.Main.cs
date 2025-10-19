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
        public Guid Id { get; set; }
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
        public List<MatchEffect> UsedMatchEffects { get; }
        public List<string> DisabledTraits { get; set; }

        public Match(Player player1, Player player2)
        {
            Id = Guid.NewGuid();
            Player1 = player1;
            Player2 = player2;
            Player1Hand = player1.GetCopyOfPlayerHand();
            Player2Hand = player2.GetCopyOfPlayerHand();
            CurrentCardsOnBoardP1 = new List<Card>();
            CurrentCardsOnBoardP2 = new List<Card>();
            Player1Effects = new PlayerCardEffects(player1);
            Player2Effects = new PlayerCardEffects(player2);
            Rounds = new List<Round>();
            RoundNumber = 0;
            UsedMatchEffects = new List<MatchEffect>();
            DisabledTraits = new List<string>();
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
            CurrentRound = initialRound;
            SetupRounds();
            SetCardStatsForMatchForPlayer(Player1);
            SetCardStatsForMatchForPlayer(Player2);
            Player1Effects = new PlayerCardEffects(Player1);
            Player2Effects = new PlayerCardEffects(Player2);
            Player1Effects.SetupMatchEffects(CurrentCardsOnBoardP1);
            Player2Effects.SetupMatchEffects(CurrentCardsOnBoardP2);
        }

        private void SetupRounds()
        {
            Dictionary<string, int[]> phases = new Dictionary<string, int[]>();
            phases.Add("Early", new int[] { 1, 2 });
            phases.Add("Mid", new int[] { 1, 2 });
            phases.Add("Late", new int[] { 1, 2, 3 });
            foreach (KeyValuePair<string, int[]> phase in phases)
            {
                for (int i = 0; i < phase.Value.Length; i++)
                {
                    var round = new NormalRound(this, phase.Key);
                    Rounds.Add(round);
                }
            }
        }

        public void ResetCardStats()
        {
            foreach (var card in CurrentCardsOnBoardP1)
            {
                card.ResetCardStats();
            }

            foreach (var card in CurrentCardsOnBoardP2)
            {
                card.ResetCardStats();
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

        public void ThrowDice(Player player)
        {
            if (player == Player1)
            {
                var round = CurrentRound as NormalRound;
                round.ThrowDiceP1();
                if (round.CanThrowDice())
                {
                    DiceNotifier.NotifyMustThrowDice(Player2);
                }
                else
                {
                    RoundNotifier.UpdatePlayerPhaseTotalsFrontend(GetRoundResultStrings());
                }
            }
            else
            {
                var round = CurrentRound as NormalRound;
                round.ThrowDiceP2();
                if (round.CanThrowDice())
                {
                    DiceNotifier.NotifyMustThrowDice(Player1);
                }
                else
                {
                    RoundNotifier.UpdatePlayerPhaseTotalsFrontend(GetRoundResultStrings());
                }
            }
        }

        public int[] GetDiceResultsForMatchForPlayer(Player player)
        {
            var round = CurrentRound as NormalRound;
            if (player == Player1)
            {
                return round.GetDiceResultsP1();
            }
            return round.GetDiceResultsP2();
        }

        // Remove me please
        // public void GoToNextRound()
        // {
        //     int roundIndex = ++RoundNumber;
        //     CurrentRound = Rounds[roundIndex];
        //     DiceNotifier.NotifyMustThrowDice(Player1);
        // }
        
        private void SetCardStats(List<Card> currentPlayerCardsOnBoard, bool p1)
        {
            bool leaderBonus = currentPlayerCardsOnBoard.GetAllCardsWithTraitOnList(Leader).Count != 0 ?
            CheckLeaderBonus(currentPlayerCardsOnBoard, p1) : false;

            if (UsedMatchEffects.Exists(mf => mf.TraitName == Leader))
            {
                leaderBonus = false;
            }

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

            if (currentPlayerCardsOnBoard.CheckTraitIsOnList(Politician) && !DisabledTraits.Contains(Politician))
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
            if (currentPlayerCardsOnBoard.CheckTraitIsOnList(TVCelebrity) && !DisabledTraits.Contains(TVCelebrity))
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
            if (currentPlayerCardsOnBoard.CheckTraitIsOnList(Drawing) && !DisabledTraits.Contains(Drawing))
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
            var nextPlayer = GetPlayerThatCanUseNextEffect();
            foreach (var card in CurrentCardsOnBoardP1)
            {
                if (card.Id == cardId)
                {
                    var currentEffectRound = CurrentRound as EffectRound;
                    if (currentEffectRound == null) throw new Exception("CurrentRound is null you absolute piece of shit in OnEffectUsed in Match");
                    UseMatchEffectOnCard(card, currentEffectRound.CurrentEffect);
                    currentEffectRound.IsUsingEffect = false;
                    currentEffectRound.CurrentEffect = null;
                    EffectNotifier.NotifyCardEffectUpdate(nextPlayer);
                    if (nextPlayer == null)
                    {
                        GD.Print("All effects have been used");
                        RoundNumber++;
                        CurrentRound = Rounds[RoundNumber];
                        var normalCurrentRound = CurrentRound as NormalRound;
                        RoundNotifier.UpdatePlayerTotalsFrontend(normalCurrentRound.TotalStatsBothPlayersCurrentPhase());
                        DiceNotifier.NotifyMustThrowDice(nextPlayer);
                    }
                }
            }
            foreach (var card in CurrentCardsOnBoardP2)
            {
                if (card.Id == cardId)
                {
                    var currentEffectRound = CurrentRound as EffectRound;
                    if (currentEffectRound == null) throw new Exception("CurrentRound is null you absolute piece of filth in OnEffectUsed in Match");
                    UseMatchEffectOnCard(card, currentEffectRound.CurrentEffect);
                    currentEffectRound.IsUsingEffect = false;
                    currentEffectRound.CurrentEffect = null;
                    EffectNotifier.NotifyCardEffectUpdate(nextPlayer);
                    if (nextPlayer == null)
                    {
                        GD.Print("All effects have been used");
                        RoundNumber++;
                        CurrentRound = Rounds[RoundNumber];
                        var normalCurrentRound = CurrentRound as NormalRound;
                        RoundNotifier.UpdatePlayerTotalsFrontend(normalCurrentRound.TotalStatsBothPlayersCurrentPhase());
                        DiceNotifier.NotifyMustThrowDice(nextPlayer);
                    }
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
        
        public void NextRound()
        {
            CurrentRound = Rounds[++RoundNumber];
            var normalCurrentRound = CurrentRound as NormalRound;
            if (normalCurrentRound == null)
            {
                throw new NullReferenceException("NextRound method gives nullpointer exception. The current round for match seems to be null");
            }
            RoundNotifier.UpdatePlayerTotalsFrontend(normalCurrentRound.TotalStatsBothPlayersCurrentPhase());
            DiceNotifier.NotifyMustThrowDice(Player1);
        } 
        
        public Player GetMatchWinner()
        {
            if (GetWinsInRowForPlayer(Player1) == 4)
            {
                return Player1;
            } 
            if (GetWinsInRowForPlayer(Player2) == 4)
            {
                return Player2;
            }
            
            int p1Wins = 0;
            int p2Wins = 0;
            
            foreach (var round in Rounds)
            {
                var normalRound = round as NormalRound;
                if (normalRound != null)
                {
                    try
                    {
                        var roundWinner = normalRound.GetWinnnerForRound();
                        if (roundWinner == Player1)
                        {
                            p1Wins++;
                        } else if (roundWinner == Player2)
                        {
                            p2Wins++;
                        }
                    }
                    catch (Exception) { }
                }
            }
            Player winner = null;
            if (p1Wins == 4)
            {
                return Player1;
            }
            if (p2Wins == 4)
            {
                return Player2;
            }
            return null;
        }

        private int GetWinsInRowForPlayer(Player player)
        {
            int winsInRowCounter = 0;
            int MaxWinsInRow = 0;
            foreach (var round in Rounds)
            {
                var normalRound = round as NormalRound;
                if (normalRound != null && normalRound.Winner != null && normalRound.Winner == player)
                {
                    winsInRowCounter++;
                } else if (winsInRowCounter > 0)
                {
                    MaxWinsInRow = MaxWinsInRow < winsInRowCounter ? winsInRowCounter : MaxWinsInRow;
                    winsInRowCounter = 0;
                }
            }
            return MaxWinsInRow;
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

        private string[] GetRoundResultStrings()
        {
            var round = CurrentRound as NormalRound;
            string phase = round.Phase;
            string rounds = RoundNumber.ToString();
            string winner = round.Winner.Name;
            
            return new string[] { phase, rounds, winner };
        }
    }
}
