using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFTAtHome.Backend.models;
using TFTAtHome.Backend.storage;
using static TFTAtHome.Backend.storage.TraitSingleton;

namespace TFTAtHome.util.ExtensionMethods
{
    public static class ExtensionMethods
    {
        public static bool CheckTraitIsOnList(this List<Card> cards, string trait)
        {
            foreach (Card card in cards)
            {
                if (card.Trait == trait)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets all cards with a specific trait on a list of cards
        /// </summary>
        /// <param name="cards">List you call the extension method on</param>
        /// <param name="trait"> Trait you want to check for </param>
        /// <returns></returns>
        public static List<Card> GetAllCardsWithTraitOnList(this List<Card> cards, string trait)
        {
            List<Card> cardsWithTrait = new List<Card>();
            foreach (Card card in cards)
            {
                if (card.Trait == trait)
                {
                    cardsWithTrait.Add(card);
                }
            }
            return cardsWithTrait;
        }

        public static int GetTraitCountOnList(this List<Card> cards, string trait)
        {
            int count = 0;
            foreach (Card card in cards)
            {
                if (card.Trait == trait)
                {
                    count++;
                }
            }
            return count;
        }

        public static bool CheckIsFictionalIsOnList(this List<Card> cards, bool isFictional)
        {
            foreach (Card card in cards)
            {
                if (card.IsFictional)
                {
                    return true;
                }
            }
            return false;
        }

        public static int GetRealCardCountOnListAndOpponent(this List<Card> cards, List<Card> opponentList)
        {
            Console.WriteLine(cards);
            Console.WriteLine(opponentList);
            int count = 0;
            foreach (Card card in cards)
            {
                if (!card.IsFictional)
                {
                    count++;
                }
            }
            foreach(Card card in opponentList)
            {
                if (!card.IsFictional)
                {
                    count++;
                }
            }
            return count;
        }

        public static int GetFictionalCardCountOnListAndOpponent(this List<Card> cards, List<Card> opponentList)
        {
            int count = 0;
            foreach (Card card in cards)
            {
                if (card.IsFictional)
                {
                    count++;
                }
            }
            foreach (Card card in opponentList)
            {
                if (card.IsFictional)
                {
                    count++;
                }
            }
            return count;
        }

        /**
         * Returns an int array with [0] = fictional cards and [1] = count of drawing cards
         */
        public static int[] GetFictionalAndDrawingCountOnListAndOpponent(this List<Card> cards, List<Card> opponentList)
        {
            int[] counts = new int[2];

            int drawingCount = cards.GetTraitCountOnListAndOpponent(Drawing, opponentList);
            int fictionalCount = GetFictionalCardCountOnListAndOpponent(cards, opponentList);

            drawingCount--;

            fictionalCount -= drawingCount;

            counts[0] = fictionalCount;
            counts[1] = drawingCount;

            return counts;
        }

        public static int GetTraitCountOnListAndOpponent(this List<Card> cards, string trait, List<Card> opponentList)
        {
            int count = 0;
            foreach (Card card in cards)
            {
                if (card.Trait == trait)
                {
                    count++;
                }
            }
            foreach (Card card in opponentList)
            {
                if (card.Trait == trait)
                {
                    count++;
                }
            }
            return count;
        }

        public static void SetTVCelebrityBonusOnCard(this Card card, int realCount, int tvCelebrityCount)
        {
            string bestPhase = "Early";
            string secondBestPhase = "Mid";
            Dictionary<string, int> phases = new Dictionary<string, int>();
            phases.Add("Early", card.Early);
            phases.Add("Mid", card.Mid);
            phases.Add("Late", card.Late);

            foreach (KeyValuePair<string, int> phase in phases)
            {
                if (phase.Value > phases[bestPhase])
                {
                    secondBestPhase = bestPhase;
                    bestPhase = phase.Key;
                } else if (phase.Value > phases[secondBestPhase])
                {
                    secondBestPhase = phase.Key;
                }
            }



            if (card.Trait == "TV-Celebrity")
            {
                // We divide by 2 to ensure we only count every second card (For example with 3 cards we only get +2 from the first pair)

                phases[bestPhase] = phases[bestPhase] + ((realCount / 2) * 2);
                phases[secondBestPhase] = phases[secondBestPhase] + ((tvCelebrityCount) * 2);

                card.SetCardStats(phases["Early"], phases["Mid"], phases["Late"]);
            }
        }

        public static string[] GetSecondBestPhaseOnCard(this Card card)
        {
            string[] secondBestPhase = new string[2];
            Dictionary<string, int> phases = new Dictionary<string, int>();
            phases.Add("Early", card.Early);
            phases.Add("Mid", card.Mid);
            phases.Add("Late", card.Late);

            var phasesAsList = phases.Select(p => new Phase
            {
                PhaseName = p.Key,
                Value = p.Value
            });
            
            var phasesSorted = phasesAsList.OrderByDescending(p => p.Value).ToList();

            if (phasesSorted[1].Value == phasesSorted[2].Value)
            {
                secondBestPhase[0] = phasesSorted[1].PhaseName;
                secondBestPhase[1] = phasesSorted[2].PhaseName;
            }
            else
            {
                secondBestPhase[0] = phasesSorted[1].PhaseName;
                secondBestPhase[1] = "";
            }

            return secondBestPhase;
        }

        public static string GetBestPhaseOnCard(this Card card)
        {
            if (card == null) return null;
            string bestPhase = "Early";
            Dictionary<string, int> phases = new Dictionary<string, int>();
            phases.Add("Early", card.Early);
            phases.Add("Mid", card.Mid);
            phases.Add("Late", card.Late);

            foreach (KeyValuePair<string, int> phase in phases)
            {
                if (phase.Value > phases[bestPhase])
                {
                    bestPhase = phase.Key;
                }
            }
            return bestPhase;
        }

            public static void SetDrawingBonusOnCard(this Card card, int[] counts)
        {
            int bonus = (counts[0] / 2) + (counts[1] * 2);
            card.Early += bonus;
            card.Mid += bonus;
            card.Late += bonus;
        }

        public static void SetPoliticianBonusOnCard(this Card card, int amount)
        {
            card.Early += amount;
            card.Mid += amount;
            card.Late += amount;
        }

        public static bool IsTraitWithPlayerInputRequirement(this string input)
        {
            return input switch
            {
                Genius => true,
                Queen => false,
                _ => false
            };
        }

        public static bool IsPassiveTrait(this string input)
        {
            return input switch
            {
                Genius => false,
                Queen => false,
                Musician => false,
                _ => true
            };
        }



        public class Phase
        {
            public string PhaseName { get; set; }
            public int Value { get; set; }
        }
    }
}
