using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFTAtHome.Backend.storage
{
    public sealed class TraitSingleton
    {
        private static TraitSingleton instance = null;
        public const string Politician = "Politician";
        public const string Leader = "Leader";
        public const string Genius = "Genius";
        public const string Drawing = "Drawing";
        public const string Queen = "Queen";
        public const string MovieHero = "Movie-Hero";
        public const string Musician = "Musician";
        public const string TVCelebrity = "TV-Celebrity";
        public const string EarlyPeaker = "Early-Peaker";
        public const string Le = "Le";

        public TraitSingleton()
        {

        }

        public static TraitSingleton GetInstance()
        {
            instance ??= new TraitSingleton();
            return instance;
        }
    }
}
