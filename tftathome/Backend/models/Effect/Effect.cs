using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFTAtHome.Backend.models;

namespace TFTAtHome.Backend.models.Effect
{
    public abstract class Effect
    {
        public string TraitName { get; }
        public Card UsedOnCard { get; set; }
        public string UsedOnTrait { get; set; }

        public Effect(string traitName)
        {
            TraitName = traitName;
        }
    }
}
