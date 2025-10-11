using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFTAtHome.Backend.models.Matches;

namespace TFTAtHome.Backend.models.Rounds
{
    public abstract class Round
    {
        protected Match match;
        public Round(Match match)
        {
            this.match = match;
        }
    }
}
