using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFTAtHome.models.Effect;
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

        public void RunPreEarlyRound()
        {
            bool playersDoneUsingEffects = false;
            while (!playersDoneUsingEffects)
            {

            }
        }

        public bool UseEffect(string effect, Player player)
        {
            /*
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
            } */
            return true;
        }
        /*
        public List<string> GetUseableEffectsForPlayerThisRound(Player player)
        {
            if (match.RoundNumber == 0)
            {
              List<String> useableEffects = new List<string> { "Queen", "Genius", "Musician" };

            } else
            {
                return null;
            }
        } */

        /*
        public void ProcessMatchEffects()
        {
            var allEffects = new List<(PlayerCardEffects PlayerEffects, MatchEffect Effect, int Count)>();

            foreach (var effect in Player1Effects.MatchEffects)
            {
                allEffects.Add((Player1Effects, effect.Key, effect.Value));
            }

            foreach (var effect in Player2Effects.MatchEffects)
            {
                allEffects.Add((Player2Effects, effect.Key, effect.Value));
            }

            var sortedEffects = allEffects.OrderBy(e => e.Effect.WeightedIndex).ToList();

            foreach (var entry in sortedEffects)
            {
                if (entry.PlayerEffects == Player1Effects)
                {
                    Player1RequestUseEffect(entry.Effect, entry.Count);
                }
                else if (entry.PlayerEffects == Player2Effects)
                {
                    Player2RequestUseEffect(entry.Effect, entry.Count);
                }
            }
        } */


    }
}
