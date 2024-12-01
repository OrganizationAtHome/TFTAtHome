using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFTAtHome.util.ExtensionMethods;
using static TFTAtHome.storage.PhaseSingleton;
using static TFTAtHome.storage.TraitSingleton;

namespace TFTAtHome.models
{
    public class SpecialRound: Round
    {
        public string Phase { get; set; }

        public SpecialRound(Match match, string phase) : base(match)
        {
            Phase = phase;
        }

        public bool UseEffect(string effect, Player player)
        {
            if (player == match.Player1)
            {
                if (match.Player1Effects.ContainsKey(effect))
                {
                    match.Player1Effects[effect]--;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (match.Player2Effects.ContainsKey(effect))
                {
                    match.Player2Effects[effect]--;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public List<string> GetUseableEffectsForPlayerThisRound(Player player)
        {
            List<string> effects = new List<string>();

            if (player == match.Player1)
            {
                if (Phase == EFFECTSPREGAMEP1)
                {
                    foreach (var effect in match.Player1Effects)
                    {
                        if (effect.Value > 0 && effect.Key != EarlyPeaker)
                        {
                            effects.Add(effect.Key);
                        }
                    }
                } else if (Phase == EFFECTSTRANSITIONP1)
                {
                    for (int i = 0; i < match.Player1Effects[EarlyPeaker]; i++)
                    {
                        effects.Add(EarlyPeaker);
                    }
                }
            }
            else
            {
                if (Phase == EFFECTSPREGAMEP2)
                {
                    foreach (var effect in match.Player2Effects)
                    {
                        if (effect.Value > 0 && effect.Key != EarlyPeaker)
                        {
                            effects.Add(effect.Key);
                        }
                    }
                }
                else if (Phase == EFFECTSTRANSITIONP2)
                {
                    for (int i = 0; i < match.Player2Effects[EarlyPeaker]; i++)
                    {
                        effects.Add(EarlyPeaker);
                    }
                }
            }
            return effects;
        }


    }
}
