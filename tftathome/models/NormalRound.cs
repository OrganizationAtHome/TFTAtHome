using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFTAtHome.storage;
using static TFTAtHome.storage.PhaseSingleton;

namespace TFTAtHome.models
{
    public class NormalRound: Round
    {
        private int[] diceResultsP1;
        private int[] diceResultsP2;
        public string Phase { get; set; }

        public NormalRound(Match match, string phase): base(match)
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
    }
}
