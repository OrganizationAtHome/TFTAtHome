using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFTAtHome.Backend.models.Matches;
using TFTAtHome.Backend.notifiers;
using static TFTAtHome.Backend.storage.PhaseSingleton;

namespace TFTAtHome.Backend.models.Rounds
{
    public class NormalRound : Round
    {
        private int[] diceResultsP1;
        private int[] diceResultsP2;
        public string Phase { get; set; }
        public Player Winner { get; set; }

        public NormalRound(Match match, string phase) : base(match)
        {
            Phase = phase;
            diceResultsP1 = new int[2];
            diceResultsP2 = new int[2];
        }

        public bool CanThrowDice()
        {
            return diceResultsP1[0] == 0 || diceResultsP2[0] == 0;
        }

        public void ThrowDiceP1()
        {
            Random rnd = new Random();
            int dice1 = rnd.Next(1, 6);
            int dice2 = rnd.Next(1, 6);
            diceResultsP1[0] = dice1;
            diceResultsP1[1] = dice2;
            
            
            if (diceResultsP2[0] != 0)
            {
                int p1Total = diceResultsP1[0] + diceResultsP1[1] + GetTotalStatsP1(Phase);
                int p2Total = diceResultsP2[0] + diceResultsP2[1] + GetTotalStatsP2(Phase);
                int[] totals = { p1Total, p2Total };
                Winner = GetWinnnerForRound();
                RoundNotifier.UpdatePlayerTotalsFrontend(totals);
            }
        }
        
        public void ThrowDiceP2()
        {
            Random rnd = new Random();
            int dice1 = rnd.Next(1, 6);
            int dice2 = rnd.Next(1, 6);
            diceResultsP2[0] = dice1;
            diceResultsP2[1] = dice2;

            if (diceResultsP1[0] != 0)
            {
                int p1Total = diceResultsP1[0] + diceResultsP1[1] + GetTotalStatsP1(Phase);
                int p2Total = diceResultsP2[0] + diceResultsP2[1] + GetTotalStatsP2(Phase);
                int[] totals = { p1Total, p2Total };
                RoundNotifier.UpdatePlayerTotalsFrontend(totals);
            }
        }

        public int[] GetDiceResultsP1()
        {
            return diceResultsP1;
        }

        public int[] GetDiceResultsP2()
        {
            return diceResultsP2;
        }

        public Player GetWinnnerForRound()
        {
            if (CanThrowDice())
            {
                throw new Exception(
                    "Are you really this dumb? THERE IS STILL A PLAYER WHO HAS NOT THROWN THEIR DICE RETARD");
            }
            int p1Total = diceResultsP1[0] + diceResultsP1[1] + GetTotalStatsP1(Phase);
            int p2Total = diceResultsP2[0] + diceResultsP2[1] + GetTotalStatsP2(Phase);

            if (p1Total > p2Total)
            {
                return match.Player1;
            }

            if (p2Total > p1Total)
            {
                return match.Player2;
            }

            return null;
        }

        /// <summary>
        /// Gets total stats for both players, using the phase set for the round
        /// </summary>
        /// <returns></returns>
        public int[] TotalStatsBothPlayersCurrentPhase()
        {
            int[] totals = new int[2];
            totals[0] = GetTotalStatsP1(Phase);
            totals[1] = GetTotalStatsP2(Phase);
            return totals;
        }


        /// <summary>
        /// Gets total stats for both players, using the phase set for the round
        /// </summary>
        /// <returns></returns>
        public int[] TotalStatsBothPlayersCurrentPhase()
        {
            int[] totals = new int[2];
            totals[0] = GetTotalStatsP1(Phase);
            totals[1] = GetTotalStatsP2(Phase);
            return totals;
        }

        public int GetTotalStatsP1(string phase)
        {
            int totals = 0;
            
            foreach (Card card in match.CurrentCardsOnBoardP1)
            {
                var property = card.GetType().GetProperty(phase);
                if (property != null)
                {
                    totals += (int)property.GetValue(card);
                }
            }

            return totals;
        }
        
        public int GetTotalStatsP2(string phase)
        {
            int totals = 0;
            
            foreach (Card card in match.CurrentCardsOnBoardP2)
            {
                var property = card.GetType().GetProperty(phase);
                if (property != null)
                {
                    totals += (int)property.GetValue(card);
                }
            }

            return totals;
        }
        
    }
}
