using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
using TFTAtHome.Backend.models.Effect;
using TFTAtHome.Backend.storage;
using TFTAtHome.util.ExtensionMethods;
using static TFTAtHome.Backend.storage.TraitSingleton;

namespace TFTAtHome.Backend.models.Matches
{
    public partial class Match
    {
        private void UseMatchEffectOnCard(Card card, MatchEffect effect)
        {
            if (card == null)
            {
                throw new Exception("Card is null you absolute piece of filth in UseMatchEffectOnCard");
            }

            if (effect == null)
            {
                throw new Exception("Your match effect is null you retarded cucumber in UseMatchEffectOnCard");
            }
            switch (effect.TraitName)
            {
                case Queen:
                {
                    if (ShouldUseQueenEffectWithIndex(card))
                    {
                        // Frontend needs to have the player specify which phase they want swapped
                    }
                    else
                    {
                        UseQueenEffectWithoutIndex(card, effect);
                    }
                } break;
                case Musician:
                {
                    UseMusicianEffect(card, effect);
                } break;
                case Genius:
                {
                    string phase1 = effect.SelectedPhase1;
                    string phase2 = effect.SelectedPhase2;
                    if (phase1 == string.Empty || phase2 == string.Empty)
                    {
                        throw new Exception("Phase 1 or Phase 2 is empty");
                    }
                    UseGeniusEffect(card, effect, phase1, phase2);
                } break;
                default:
                {
                    throw new Exception("WHY THE FUCK ARE YOU GIVING ME A MATCH EFFECT THAT DOESN'T EXIST????");
                }
            }
        }

        /// <summary>
        /// This method returns which player can use the next effect
        /// </summary>
        /// <returns>Player</returns>
        public Player GetPlayerThatCanUseNextEffect()
        {
            var p1Index = Player1Effects.GetCurrentMatchEffectForPlayer()?.Key.WeightedIndex;
            var p2Index = Player2Effects.GetCurrentMatchEffectForPlayer()?.Key.WeightedIndex;

            if (p1Index == null && p2Index == null)
            {
                return null;
            }

            if (p1Index == null)
            {
                return Player2;
            }
            if (p2Index == null)
            {
                return Player1;
            }

            if (p1Index > p2Index)
            {
                return Player1;
            } 
            if (p2Index > p1Index)
            {
                return Player2;
            }
            return Player1;
        }

        public bool ShouldUseQueenEffectWithIndex(Card cardToUseEffectOn)
        {
            return cardToUseEffectOn.GetSecondBestPhaseOnCard()[1].Length != 0;
        }

        public void UseQueenEffectWithoutIndex(Card cardToUseEffectOn, MatchEffect effectUsed)
        {
            int originalEarly = cardToUseEffectOn.Early;
            int originalMid = cardToUseEffectOn.Mid;
            int originalLate = cardToUseEffectOn.Late;
            
            var bestPhaseProperty = cardToUseEffectOn.GetType().GetProperty(cardToUseEffectOn.GetBestPhaseOnCard());

            if (bestPhaseProperty == null || bestPhaseProperty.PropertyType != typeof(int)) return;
            int bestPhaseValueOnCard = (int)bestPhaseProperty.GetValue(cardToUseEffectOn);
            string secondBestPhase = cardToUseEffectOn.GetSecondBestPhaseOnCard()[0];

            cardToUseEffectOn.GetType().GetProperty(secondBestPhase).SetValue(cardToUseEffectOn, bestPhaseValueOnCard);
            
            SaveUsedMatchEffectStatChange(cardToUseEffectOn, effectUsed, originalEarly, originalMid, originalLate);
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

        public void UseGeniusEffect(Card cardToUseEffectOn, MatchEffect effectUsed, string phase1, string phase2)
        {
            int originalEarly = cardToUseEffectOn.Early;
            int originalMid = cardToUseEffectOn.Mid;
            int originalLate = cardToUseEffectOn.Late;
            
            var phase1Property = cardToUseEffectOn.GetType().GetProperty(phase1);
            var phase2Property = cardToUseEffectOn.GetType().GetProperty(phase2);

            if (phase1Property.PropertyType != typeof(int) || phase2Property.PropertyType != typeof(int))
            {
                throw new Exception("PHASE 1 and PHASE 2 for the genius effect are not ints you absolute inbreed imbecile");
            }
            
            int phase1ValueOnCard = (int)phase1Property.GetValue(cardToUseEffectOn);
            int phase2ValueOnCard = (int)phase2Property.GetValue(cardToUseEffectOn);
            
            cardToUseEffectOn.GetType().GetProperty(phase1).SetValue(cardToUseEffectOn, phase2ValueOnCard);
            cardToUseEffectOn.GetType().GetProperty(phase2).SetValue(cardToUseEffectOn, phase1ValueOnCard);
            
            SaveUsedMatchEffectStatChange(cardToUseEffectOn, effectUsed, originalEarly, originalMid, originalLate);
        }
        
        public void UseMusicianEffect(Card cardToUseEffectOn, MatchEffect effectUsed)
        {
            var trait = cardToUseEffectOn.Trait;
            DisabledTraits.Add(trait);
            if (trait.IsPassiveTrait())
            {
                ResetCardStats();
            } 
            else
            {
                RevertMatchEffect(trait);
            }
        }

        public void RevertMatchEffect(string trait)
        {
            var doesRevertEffectExist = UsedMatchEffects.Exists(mf => mf.TraitName == trait);
            if (doesRevertEffectExist)
            {
                var effectToRevert = UsedMatchEffects.Find(mf => mf.TraitName == trait);
                var card = effectToRevert.UsedOnCard;
                card.SetCardStats(card.Early + effectToRevert.StatsChange[0], 
                    card.Mid + effectToRevert.StatsChange[1], 
                    card.Late + effectToRevert.StatsChange[2]);
                UsedMatchEffects.Remove(effectToRevert);
            }
            else
            {
                throw new Exception(
                    "The trait you are trying to revert doesn't exist in UsedMatchEffects. I think you might actually be retarded");
            }
        }

        private void SaveUsedMatchEffectStatChange(Card cardToUseEffectOn, MatchEffect effectUsed, int originalEarly, int originalMid, int originalLate)
        {
            effectUsed.UsedOnCard = cardToUseEffectOn;
            
            int earlyDiff = originalEarly - cardToUseEffectOn.Early;
            int midDiff = originalMid - cardToUseEffectOn.Mid;
            int lateDiff = originalLate - cardToUseEffectOn.Late;
            
            effectUsed.StatsChange = new int[] { earlyDiff, midDiff, lateDiff };
            
            UsedMatchEffects.Add(effectUsed);
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


        public KeyValuePair<MatchEffect, int>? GetCurrentMatchEffectForPlayer()
        {
            try
            {
                var currentMatchEffect = MatchEffects.Where(me => me.Value > 0).OrderBy(me => me.Key.WeightedIndex)
                    .FirstOrDefault();
                if (currentMatchEffect.Key == null)
                {
                    return null;
                }
                return currentMatchEffect;
            }
            catch (Exception e)
            {
                return null;
            }
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
                if (MatchEffects[matchEffect] >= 1)
                {
                    MatchEffects[matchEffect]--;
                    GD.Print("Removed MatchEffect new Dictionary:");
                    GD.Print(DictionaryToString(MatchEffects));
                    return MatchEffects[matchEffect] != 0;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
        
        static string DictionaryToString<TKey, TValue>(Dictionary<TKey, TValue> dict)
        {
            var items = new List<string>();
            foreach (var kvp in dict)
            {
                items.Add($"{kvp.Key}: {kvp.Value}");
            }
            return string.Join(", ", items);
        }
    }
}
