using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFTAtHome.Backend.models.Effect;
using TFTAtHome.Backend.models.Matches;
using TFTAtHome.util.ExtensionMethods;
using static TFTAtHome.Backend.storage.PhaseSingleton;
using static TFTAtHome.Backend.storage.TraitSingleton;

namespace TFTAtHome.Backend.models.Rounds
{
    public class EffectRound : Round
    {
        public bool IsUsingEffect { get; set; } = false;
        public MatchEffect CurrentEffect { get; set; }
        public Player PlayerUsingEffect { get; set; }
        public EffectRound(Match match) : base(match)
        {
        }

    }
}
