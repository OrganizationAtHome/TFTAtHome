using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFTAtHome.models
{
    public class Match
    {
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public List<Card> CurrentCardsOnBoardP1 { get; set; }
        public List<Card> CurrentCardsOnBoardP2 { get; set; }
        // public List<Effect> Player1Effects { get; set; }
        // public List<Effect> Player2Effects { get; set; }
        public int RoundNumber { get; set; }
        // public List<Round> Rounds  { get; set; }

        public void AddCardToBoard(Card card, Player player)
        {
            if (player == Player1)
            {
                Player1.GetPlayerHand().Remove(card);
                CurrentCardsOnBoardP1.Add(card);
            }
            else
            {
                Player2.GetPlayerHand().Remove(card);
                CurrentCardsOnBoardP2.Add(card);
            }
        }

        public void RemoveCardFromBoard(Card card, Player player)
        {
            if (player == Player1)
            {
                Player1.GetPlayerHand().Add(card);
                CurrentCardsOnBoardP1.Remove(card);
            }
            else
            {
                Player2.GetPlayerHand().Add(card);
                CurrentCardsOnBoardP2.Remove(card);
            }
        }

        public void SetCardStatsForMatchForPlayer(Player player)
        {
            bool leaderBonus = 
            if (player == Player1)
            {
                foreach (Card card in CurrentCardsOnBoardP1)
                {
                    var result = card.Trait switch
                    {
                        "Politician" => DoStuff(),
                        "Leader" => DoStuff(),
                        "TV-Famous" => DoStuff(),
                        "MovieHero" => DoStuff(),
                        "CartonCharacter" => DoStuff(),
                        "Le" => DoStuff(),
                        "Queen" => DoStuff(),
                        "Early peaker" => DoStuff(),
                        "Musician" => DoStuff(),
                        "Genius" => DoStuff(),
                        "" => DoStuff()
                    };
                    // card.SetCardStats(player);
                }
            }
            else
            {
                foreach (Card card in CurrentCardsOnBoardP2)
                {
                    // card.SetCardStats(player);
                }
            }
        }

        private int[] CheckPoliticianBonus()
        {

            return new int[2];
            // Do stuff
        }

        private bool CheckLeaderBonus(bool player1)
        {
            bool test = Player2.GetPlayerHand().Any(c => c.Trait == "Leader");
        }

    }
}
