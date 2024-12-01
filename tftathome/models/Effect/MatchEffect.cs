using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TFTAtHome.storage.TraitSingleton;

namespace TFTAtHome.models.Effect
{
    public class MatchEffect
    {
        public string TraitName { get; set; }
        public Card UsedOnCard { get; set; }
        public string UsedOnTrait { get; set; }
        public bool Persistent { get; set; }
        public int WeightedIndex { get; } = 0;

        public MatchEffect(string traitName, bool persistent)
        {
            TraitName = traitName;
            Persistent = persistent;
            WeightedIndex = GenerateWeightedIndex(traitName);
        }


        private int GenerateWeightedIndex(string traitName)
        {
            switch(traitName)
            {
                case Queen:
                    return 1;
                break;
                case Genius:
                    return 2;
                break;
                case Musician:
                    return 3;
                default:
                    return -1;
            }
        }
    }
}
