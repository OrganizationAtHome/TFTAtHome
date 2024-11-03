using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFTAtHome.models;

namespace TFTAtHome.storage
{

    public static class LocalStorage
    {
        public static List<Card> Cards = new List<Card>();
        private static GameManager _GameManager;

        static LocalStorage()
        {
            Card card1 = new Card(1, "LIGHT YAGAMI", "THE FIRST KIRA", "images/cardimages/light-yagami.jpeg", 6, 12, 1, "Geni", 3, true);

            Card card2 = new Card(2, "ADOLF HITLER", "THE FÜHRER", "images/cardimages/adolf-hitler.jpg", 9, 9, 1, "Politiker", 4, true);

            Card card3 = new Card(3, "CHUCK NORRIS", "THE TEXAS RANGER", "images/cardimages/chuck-norris.jpg", 7, 8, 3, "TV-kendis", 3, true);

            Card card4 = new Card(4, "DEADPOOL", "THE SUPERHERO REJECT", "images/cardimages/deadpool.jpg", 5, 1, 11, "Filmhelt", 1, false);

            Card card5 = new Card(5, "DONALD TRUMP", "THE 45TH PRESIDENT", "images/cardimages/donald-trump.jpg", 6, 8, 4, "Politiker", 3, true);

            Card card6 = new Card(6, "EGON OLSEN", "THE MASTER PLANNER", "images/cardimages/egon-olsen.jpg", 1, 17, 1, "Geni", 5, false);

            Card card7 = new Card(7, "ELIZABETH II", "THE QUEEN OF ENGLAND", "images/cardimages/elizabeth-2.jpg", 3, 7, 7, "Queen", 3, true);

            Card card8 = new Card(8, "ELON MUSK", "THE CEO", "images/cardimages/elon-musk.jpg", 5, 7, 6, "Leder", 3, true);

            Card card9 = new Card(9, "ERIC CARTMAN", "THE FAT KID", "images/cardimages/eric-cartman.jpg", 5, 8, 5, "Tegning", 1, false);

            Card card10 = new Card(10, "FIE LAURSEN", "THE BULLIED BLOGGER", "images/cardimages/fie-laursen.jpg", 10, 6, 3, "TV-kendis", 1, true);

            Card card11 = new Card(11, "GORDON RAMSAY", "THE INTERNATIONAL CHEF", "images/cardimages/gordon-ramsay.jpg", 4, 8, 6, "TV-kendis", 2, true);

            Card card12 = new Card(12, "HARRY POTTER", "THE BOY WHO LIVED", "images/cardimages/harry-potter.jpg", 5, 5, 7, "Filmhelt", 4, false);

            Card card13 = new Card(13, "JESUS CHRIST", "THE SON OF GOD", "images/cardimages/jesus-christ.jpg", 12, 1, 6, "Leder", 4, true);

            Card card14 = new Card(14, "JOE EXOTIC", "THE TIGER KING", "images/cardimages/joe-exotic.jpg", 14, 1, 4, "TV-kendis", 2, true);

            Card card15 = new Card(15, "L", "THE DETECTIVE", "images/cardimages/l.png", 12, 6, 1, "Geni", 2, false);

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
