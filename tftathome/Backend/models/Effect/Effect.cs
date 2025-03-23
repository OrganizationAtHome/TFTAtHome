using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFTAtHome.Backend.models.Effect
{
    public abstract class Effect
    {
        public string TraitName { get; }
        public Card UsedOnCard { get; }
        public string UsedOnTrait { get; }

        public Effect(string traitName)
        {
            TraitName = traitName;
        }
    }
}
