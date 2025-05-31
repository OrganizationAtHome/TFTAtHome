using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFTAtHome.Backend.models;
using static TFTAtHome.Backend.storage.TraitSingleton;

namespace TFTAtHome.Backend.models.Effect
{
    public class MatchEffect: Effect
    {
        public string TraitName { get; set; }
        public bool Persistent { get; set; }
        public int WeightedIndex { get; } = 0;
        public string SelectedPhase1 { get; set; }
        public string SelectedPhase2 { get; set; }

        public MatchEffect(string traitName, bool persistent): base(traitName)
        {
            TraitName = traitName;
            Persistent = persistent;
            WeightedIndex = GenerateWeightedIndex(traitName);
        }

        private int GenerateWeightedIndex(string traitName)
        {
            switch (traitName)
            {
                case Queen:
                    return 1;
                case Genius:
                    return 2;
                case Musician:
                    return 3;
                case EarlyPeaker:
                    return 4;
                default:
                    return -1;
            }
        }

        public override string ToString()
        {
            return "Trait: " + TraitName;
        }
    }
}
