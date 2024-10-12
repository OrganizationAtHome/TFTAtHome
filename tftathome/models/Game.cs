using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFTAtHome.storage;

namespace TFTAtHome.models
{
    public class Game
    {
        public int Id { get; set; }
        private readonly List<Player> _players = new List<Player>();
        private List<Card> _activeCardPool;
        private List<Card> _inactiveCards = new List<Card>();
        public GameState GameState { get; set; }

        public Game(int id) 
        { 
            Id = id;
            GameState = GameState.AWAITING_PLAYERS;
            _activeCardPool = LocalStorage.getCards();
        }

        public void AddPlayer(Player player)
        {
            _players.Add(player);
        }

        public List<Card> getActiveCardPool()
        {
            return _activeCardPool;
        }
    }
}
