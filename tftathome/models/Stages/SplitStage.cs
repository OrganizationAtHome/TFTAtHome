using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFTAtHome.models.Stages
{
    public class SplitStage: Stage
    {
        public Dictionary<Player, bool[]> PlayerStatsThisSplit { get; set; }
        public Player SplitWinner { get; set; }
    }
}
