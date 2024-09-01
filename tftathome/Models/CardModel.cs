using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFTAtHome.Models
{
    public class CardModel
    {
        public int Id { get; set; }
        public string CardName { get; set; }
        public string CardTitle { get; set; }
        public string ImgSrch { get; set; }
        public int Early { get; set; }
        public int Mid { get; set; }
        public int Late { get; set; }

        /* MISSING CardClass Rn
         public string CardClass
         */
        public int Cost { get; set; }
        public string Type { get; set; }


    }
}
