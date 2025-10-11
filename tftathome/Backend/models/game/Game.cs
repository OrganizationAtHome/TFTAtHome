using System.Collections.Generic;
using TFTAtHome.Backend.models.Splits;

namespace TFTAtHome.Backend.models.game;

// Purpose of this class is to keep track of a full game between X amount of players
// It will possibly also have logic for keeping track of player actions
public class Game
{
    private List<Split> splits;
    public List<Player> players {get; set;}
}