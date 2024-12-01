using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFTAtHome.models;
using static TFTAtHome.storage.TraitSingleton;

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

            Console.WriteLine(drawingCount);
            Console.WriteLine(fictionalCount);

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
                card.GetType().GetProperty(bestPhase).SetValue(card, phases[bestPhase] + ((realCount / 2) * 2));
                card.GetType().GetProperty(secondBestPhase).SetValue(card, phases[secondBestPhase] + ((tvCelebrityCount) * 2));
            }
        }

        public static string[] GetSecondBestPhaseOnCard(this Card card)
        {
            string bestPhase = "Early";
            string[] secondBestPhase = new string[2];
            Dictionary<string, int> phases = new Dictionary<string, int>();
            phases.Add("Early", card.Early);
            phases.Add("Mid", card.Mid);
            phases.Add("Late", card.Late);

            foreach (KeyValuePair<string, int> phase in phases)
            {
                if (phase.Value > phases[bestPhase])
                {
                    if (secondBestPhase[0].Length == 0)
                    {
                        secondBestPhase[0] = bestPhase;
                    }
                    else
                    {
                        secondBestPhase[1] = bestPhase;
                    }
                    bestPhase = phase.Key;
                } else if (phase.Value > phases[secondBestPhase[0]] || phase.Value > phases[secondBestPhase[1]])
                {
                    if (secondBestPhase[0].Length == 0)
                    {
                        secondBestPhase[0] = phase.Key;
                    } else
                    {
                        secondBestPhase[1] = phase.Key;
                    }
                }
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
    }
}
