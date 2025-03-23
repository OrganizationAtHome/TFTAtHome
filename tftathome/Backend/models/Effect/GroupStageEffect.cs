using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFTAtHome.Backend.models.Effect
{
    public class GroupStageEffect
    {
        public string TraitName { get; set; }
        public Card UsedOnCard { get; set; }
        public string UsedOnTrait { get; set; }

        public GroupStageEffect(string traitName)
        {
            TraitName = traitName;
        }
    }
}
