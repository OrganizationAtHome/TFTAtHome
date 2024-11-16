using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFTAtHome.models
{
    public class Card
    {
        public int Id { get; set; }
        public string CardName { get; set; }
        public string CardTitle { get; set; }
        public string CardImgSrc { get; set; }
        public int Early {  get; set; }
        public int Mid { get; set; }
        public int Late { get; set; }
        public string Trait { get; set; }
        public int Cost { get; set; }
        public bool IsFictional { get; set; }

        public Card(int id, string cardName, string cardTitle, string cardImgSrc, int early, int mid, int late, string trait, int cost, bool isFictional)
        {
            Id = id;
            CardName = cardName;
            CardTitle = cardTitle;
            CardImgSrc = cardImgSrc;
            Early = early;
            Mid = mid;
            Late = late;
            Trait = trait;
            Cost = cost;
            IsFictional = isFictional;
        }

        public void SetCardStats(int early, int mid, int late)
        {
            Early = early;
            Mid = mid;
            Late = late;
        }

        public Card Clone()
        {
            return (Card)this.MemberwiseClone();
        }

        public string[] GetStatsValuesAsString()
        {
            string[] values = new string[5];
            values[0] = "" + Early;
            values[1] = "" + Mid;
            values[2] = "" + Late;
            values[3] = Trait;
            values[4] = "" + Cost;

            return values;
        }

        public override string ToString()
        {
            return "{ " + Id + ", " + CardName + " }";
        }
    }
}
