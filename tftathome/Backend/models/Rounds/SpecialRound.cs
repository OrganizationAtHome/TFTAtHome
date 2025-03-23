using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFTAtHome.Backend.models;
using TFTAtHome.Backend.models.Effect;
using TFTAtHome.util.ExtensionMethods;
using static TFTAtHome.Backend.storage.PhaseSingleton;
using static TFTAtHome.Backend.storage.TraitSingleton;

namespace TFTAtHome.Backend.models.Rounds
{
    public class SpecialRound : Round
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

        public bool UseEffect(MatchEffect matchEffect, Player player)
        {
            if (player == match.Player1)
            {
                match.GetMatchEffectsForPlayer(player);
            }
            else
            {

            }
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
