using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFTAtHome.Backend.models.Effect;
using TFTAtHome.util.ExtensionMethods;
using static TFTAtHome.Backend.storage.TraitSingleton;

namespace TFTAtHome.Backend.models.Matches
{
    public partial class Match
    {
        public bool ShouldUseQueenEffectWithIndex(Card cardToUseEffectOn)
        {
            return cardToUseEffectOn.GetSecondBestPhaseOnCard()[1].Length != 0;
        }

        public void UseQueenEffectWithoutIndex(Player player, Card cardToUseEffectOn)
        {
            var bestPhaseProperty = cardToUseEffectOn.GetType().GetProperty(cardToUseEffectOn.GetBestPhaseOnCard());

            if (bestPhaseProperty == null || bestPhaseProperty.PropertyType != typeof(int)) return;
            int bestPhaseValueOnCard = (int)bestPhaseProperty.GetValue(cardToUseEffectOn);
            string secondBestPhase = cardToUseEffectOn.GetSecondBestPhaseOnCard()[0];

            cardToUseEffectOn.GetType().GetProperty(secondBestPhase).SetValue(cardToUseEffectOn, bestPhaseValueOnCard);
        }

        public void UseQueenEffectWithIndex(Player player, Card cardToUseEffectOn, int indexOfSecondBestPhase)
        {
            var bestPhaseProperty = cardToUseEffectOn.GetType().GetProperty(cardToUseEffectOn.GetBestPhaseOnCard());
            if (bestPhaseProperty == null || bestPhaseProperty.PropertyType != typeof(int)) return;
            int bestPhaseValueOnCard = (int)bestPhaseProperty.GetValue(cardToUseEffectOn);

            string secondBestPhase = "";
            switch (indexOfSecondBestPhase)
            {
                case 0:
                    secondBestPhase = "Early";
                    break;
                case 1:
                    secondBestPhase = "Mid";
                    break;
                case 2:
                    secondBestPhase = "Late";
                    break;
                default: return;
            }

            cardToUseEffectOn.GetType().GetProperty(secondBestPhase).SetValue(cardToUseEffectOn, bestPhaseValueOnCard);
        }
    }

    public class PlayerCardEffects
    {
        public Dictionary<MatchEffect, int> MatchEffects { get; } = new Dictionary<MatchEffect, int>();
        public Player Player { get; set; }
        public bool CanUseNewEffect { get; set; }

        public PlayerCardEffects(Player player)
        {
            Player = player;
            CanUseNewEffect = true;
        }


        public KeyValuePair<MatchEffect, int> GetCurrentMatchEffectForPlayer()
        {
            var currentMatchEffect = MatchEffects.Where(me => me.Value > 0).OrderBy(me => me.Key.WeightedIndex).FirstOrDefault();
            return currentMatchEffect;
        }

        public void SetupMatchEffects(List<Card> currentCardsOnBoard)
        {
            if (currentCardsOnBoard.CheckTraitIsOnList(Queen))
            {
                int count = currentCardsOnBoard.GetAllCardsWithTraitOnList(Queen).Count;
                MatchEffects.Add(new MatchEffect(Queen, true), count);
            }
            if (currentCardsOnBoard.CheckTraitIsOnList(EarlyPeaker))
            {
                int count = currentCardsOnBoard.GetAllCardsWithTraitOnList(EarlyPeaker).Count;
                MatchEffects.Add(new MatchEffect(EarlyPeaker, false), count);
            }
            if (currentCardsOnBoard.CheckTraitIsOnList(Genius))
            {
                int count = currentCardsOnBoard.GetAllCardsWithTraitOnList(Genius).Count;
                MatchEffects.Add(new MatchEffect(Genius, true), count);
            }
            if (currentCardsOnBoard.CheckTraitIsOnList(Musician))
            {
                int count = currentCardsOnBoard.GetAllCardsWithTraitOnList(Musician).Count;
                MatchEffects.Add(new MatchEffect(Musician, true), count);
            }
            if (currentCardsOnBoard.CheckTraitIsOnList(Le))
            {
                MatchEffects.Add(new MatchEffect(Le, false), 1);
            }
        }

        public bool UseMatchEffect(MatchEffect matchEffect)
        {
            if (MatchEffects.ContainsKey(matchEffect))
            {
                if (MatchEffects[matchEffect] > 1)
                {
                    MatchEffects[matchEffect]--;
                    return MatchEffects[matchEffect] != 0;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
    }
}
