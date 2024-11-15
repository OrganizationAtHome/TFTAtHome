using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFTAtHome.models
{
    public class Player
    {
        public long Id { get; set; }
        public string Name { get; set; }
        private List<Card> _playerHand = new List<Card>();
        public int Coins { get; set; }
        public int WinsThisSplit { get; set; }
        public int LossesThisSplit { get; set; }
        public int SplitWins { get; set; }
        public int SeasonWins { get; set; }

        public Player(long id, string name, List<Card> playerHand)
        {
            Id = id;
            Name = name;
            _playerHand = playerHand;
            Coins = 5;
        }

        public void AddCard(Card card)
        {
            _playerHand.Add(card);
        }

        public List<Card> GetPlayerHand() { return _playerHand; }
    }
}
