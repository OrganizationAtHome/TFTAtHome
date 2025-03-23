using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFTAtHome.Backend.models.Effect
{
    public interface IActiveTraitEffect
    {
        void ApplyEffect(Match match, Player player, Card card);
    }
}
