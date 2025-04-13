using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFTAtHome.Backend.models.Matches;
using static TFTAtHome.Backend.storage.PhaseSingleton;

namespace TFTAtHome.Backend.models.Rounds
{
    public class NormalRound : Round
    {
        private int[] diceResultsP1;
        private int[] diceResultsP2;
        public string Phase { get; set; }

        public NormalRound(Match match, string phase) : base(match)
        {
            Phase = phase;
            diceResultsP1 = new int[2];
            diceResultsP2 = new int[2];
        }

        public int[] GetDiceResultsP1()
        {
            return diceResultsP1;
        }

        public int[] GetDiceResultsP2()
        {
            return diceResultsP2;
        }

        public int[] GetTotalStatsP1()
        {
            int[] totals = new int[3];

            foreach (Card card in match.CurrentCardsOnBoardP1)
            {
                totals[0] += card.Early;
                totals[1] += card.Mid;
                totals[2] += card.Late;
            }

            return totals;
        }

        public int[] GetTotalStatsP2()
        {
            int[] totals = new int[3];

            foreach (Card card in match.CurrentCardsOnBoardP2)
            {
                totals[0] += card.Early;
                totals[1] += card.Mid;
                totals[2] += card.Late;
            }

            return totals;
        }
    }
}
