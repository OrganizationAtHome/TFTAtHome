using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFTAtHome.Backend.models;

namespace TFTAtHome.Backend.models.Stages
{
    public class SplitStage : Stage
    {
        public Dictionary<Player, bool[]> PlayerStatsThisSplit { get; set; }
        public Player SplitWinner { get; set; }
    }
}
