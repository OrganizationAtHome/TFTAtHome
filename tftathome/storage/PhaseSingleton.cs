using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFTAtHome.storage
{
    public sealed class PhaseSingleton
    {
        private static PhaseSingleton instance = null;
        public const string EARLY = "Early";
        public const string MID = "Mid";
        public const string LATE = "Late";
        public const string EFFECTSPREGAMEP1 = "EffectsPreGameP1";
        public const string EFFECTSPREGAMEP2 = "EffectsPreGameP2";
        public const string EFFECTSTRANSITIONP1 = "EffectsTransitionP1";
        public const string EFFECTSTRANSITIONP2 = "EffectsTransitionP2";

        public PhaseSingleton()
        {

        }

        public static PhaseSingleton GetInstance()
        {
            instance ??= new PhaseSingleton();
            return instance;
        }
    }
}
