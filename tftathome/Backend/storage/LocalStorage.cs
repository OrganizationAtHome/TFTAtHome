using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFTAtHome.Backend.models;
using static TFTAtHome.Backend.storage.TraitSingleton;

namespace TFTAtHome.Backend.storage
{

    public static class LocalStorage
    {
        public static List<Card> Cards = new List<Card>();
        private static GameManager _GameManager;

        static LocalStorage()
        {
            Card card1 = new Card(1, "LIGHT YAGAMI", "THE FIRST KIRA", "images/cardimages/light-yagami.jpeg", 6, 12, 1, Genius, 3, true);

            Card card2 = new Card(2, "ADOLF HITLER", "THE FÜHRER", "images/cardimages/adolf-hitler.jpg", 9, 9, 1, Politician, 4, false);

            Card card3 = new Card(3, "CHUCK NORRIS", "THE TEXAS RANGER", "images/cardimages/chuck-norris.jpg", 7, 8, 3, TVCelebrity, 3, false);

            Card card4 = new Card(4, "DEADPOOL", "THE SUPERHERO REJECT", "images/cardimages/deadpool.jpg", 5, 1, 11, MovieHero, 1, true);

            Card card5 = new Card(5, "DONALD TRUMP", "THE 45TH PRESIDENT", "images/cardimages/donald-trump.jpg", 6, 8, 4, Politician, 3, false);

            Card card6 = new Card(6, "EGON OLSEN", "THE MASTER PLANNER", "images/cardimages/egon-olsen.jpg", 1, 17, 1, Genius, 5, true);

            Card card7 = new Card(7, "ELIZABETH II", "THE QUEEN OF ENGLAND", "images/cardimages/elizabeth-2.jpg", 3, 7, 7, Queen, 3, false);

            Card card8 = new Card(8, "ELON MUSK", "THE CEO", "images/cardimages/elon-musk.jpg", 5, 7, 6, Leader, 3, false);

            Card card9 = new Card(9, "ERIC CARTMAN", "THE FAT KID", "images/cardimages/eric-cartman.jpg", 5, 8, 5, Drawing, 1, true);

            Card card10 = new Card(10, "FIE LAURSEN", "THE BULLIED BLOGGER", "images/cardimages/fie-laursen.jpg", 10, 6, 3, TVCelebrity, 1, false);

            Card card11 = new Card(11, "GORDON RAMSAY", "THE INTERNATIONAL CHEF", "images/cardimages/gordon-ramsay.jpg", 4, 8, 6, TVCelebrity, 2, false);

            Card card12 = new Card(12, "HARRY POTTER", "THE BOY WHO LIVED", "images/cardimages/harry-potter.jpg", 5, 5, 7, MovieHero, 4, true);

            Card card13 = new Card(13, "JESUS CHRIST", "THE SON OF GOD", "images/cardimages/jesus-christ.jpg", 12, 1, 6, Leader, 4, false);

            Card card14 = new Card(14, "JOE EXOTIC", "THE TIGER KING", "images/cardimages/joe-exotic.jpg", 14, 1, 4, TVCelebrity, 2, false);

            Card card15 = new Card(15, "L", "THE DETECTIVE", "images/cardimages/l.png", 12, 6, 1, Genius, 2, true);

            Card card16 = new Card(16, "LE AT HOME", "THE REAL LE", "images/cardimages/leathome.png", 2, 6, 8, Le, 4, false);

            Card card17 = new Card(17, "LUKE SKYWALKER", "THE JEDI MASTER", "images/cardimages/luke-skywalker.jpeg", 2, 5, 9, MovieHero, 3, false);

            Card card18 = new Card(18, "MARGRETHE II", "THE QUEEN OF DENMARK", "images/cardimages/margrethe-2.jpg", 1, 8, 8, Queen, 3, false);

            Card card19 = new Card(19, "METTE FREDERIKSEN", "THE STATSMINISTER", "images/cardimages/mette-frederiksen.jpg", 7, 2, 8, Politician, 1, false);

            Card card20 = new Card(20, "NAPOLEON", "THE GREAT EMPEROR", "images/cardimages/napoleon.jpg", 8, 6, 4, Politician, 2, false);

            Card card21 = new Card(21, "NARUTO", "THE BOY NINJA", "images/cardimages/naruto.png", 1, 5, 11, Drawing, 2, true);

            Card card22 = new Card(22, "PERRY", "THE PLATYPUS", "images/cardimages/perry.jpeg", 2, 12, 4, Drawing, 3, true);

            Card card23 = new Card(23, "PO", "THE KUNG FU PANDA", "images/cardimages/po.jpg", 1, 1, 14, Drawing, 1, true);

            Card card24 = new Card(24, "RASPUTIN", "THE RUSSIAN LOVER", "images/cardimages/rasputin.jpg", 3, 10, 5, Politician, 2, false);

            Card card25 = new Card(25, "REDFOO", "THE SEXY AND HE KNOWS IT", "images/cardimages/redfoo.jpeg", 15, 3, 1, Musician, 3, false);

            Card card26 = new Card(26, "RICH PIANA", "THE DEAD BODYBUILDER", "images/cardimages/rich-piana.jpg", 18, 1, 1, EarlyPeaker, 5, false);

            Card card27 = new Card(27, "RICK ASTLEY", "THE RICK ROLLER", "images/cardimages/rick-astley.jpg", 4, 10, 4, Musician, 4, false);

            Card card28 = new Card(28, "RONALD MCDONALD", "THE FAST-FOOD CLOWN", "images/cardimages/ronald-mcdonald.jpg", 9, 8, 2, EarlyPeaker, 2, false);

            Card card29 = new Card(29, "SHREK", "THE GREEN OGRE", "images/cardimages/shrek.jpg", 10, 4, 4, EarlyPeaker, 1, true);

            Card card30 = new Card(30, "THANOS", "THE MAD TITAN", "images/cardimages/thanos.jpg", 6, 6, 6, Leader, 2, false);

            Card card31 = new Card(31, "VLADIMIR", "THE CRIMSON REAPER", "images/cardimages/vladimir.png", 2, 5, 10, Drawing, 1, true);

            Card card32 = new Card(32, "XYP9X", "THE CLUTCH MINISTER", "images/cardimages/xyp9x.png", 3, 3, 11, TVCelebrity, 1, false);

            Card card33 = new Card(33, "ZAGARA", "THE OVERQUEEN", "images/cardimages/zagara.png", 14, 4, 1, Queen, 5, false);

            Cards.Add(card1);
            Cards.Add(card2);
            Cards.Add(card3);
            Cards.Add(card4);
            Cards.Add(card5);
            Cards.Add(card6);
            Cards.Add(card7);
            Cards.Add(card8);
            Cards.Add(card9);
            Cards.Add(card10);
            Cards.Add(card11);
            Cards.Add(card12);
            Cards.Add(card13);
            Cards.Add(card14);
            Cards.Add(card15);
            Cards.Add(card16);
            Cards.Add(card17);
            Cards.Add(card18);
            Cards.Add(card19);
            Cards.Add(card20);
            Cards.Add(card21);
            Cards.Add(card22);
            Cards.Add(card23);
            Cards.Add(card24);
            Cards.Add(card25);
            Cards.Add(card26);
            Cards.Add(card27);
            Cards.Add(card28);
            Cards.Add(card29);
            Cards.Add(card30);
            Cards.Add(card31);
            Cards.Add(card32);
            Cards.Add(card33);

        }

        public static List<Card> getCards()
        {
            return Cards;
        }

        public static Card getCardFromName(string name)
        {
            foreach (Card card in Cards)
            {
                if (card.CardName == name) return card;
            }
            return null;
        }

        public static void SetGameManager(GameManager game)
        {
            if (game == null) return;

            if (_GameManager == null)
            {
                _GameManager = game;
            }
        }

        public static GameManager GetGame()
        {
            return _GameManager;
        }
    }
}
