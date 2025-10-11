using System;
using System.Collections.Generic;
using TFTAtHome.Backend.models.Matches;
using TFTAtHome.Backend.models.game;

namespace TFTAtHome.Backend.models.Splits;

public class Split
{
    private Guid Id;
    private List<Match> matches;

    public Split(Game game)
    {
        Id = Guid.NewGuid();
        matches = new List<Match>();
        CalculateSplit(game);
    }

    
    private void CalculateSplit(Game game)
    {
        List<Player> players = game.players;
        int currentPlayerIndex = 0;

        for (int i = 0; i < players.Count; i++)
        {
            for (int j = 0; j < players.Count; j++)
            {
                if (i != currentPlayerIndex)
                {
                    matches.Add(new Match(players[i], players[currentPlayerIndex]));
                }
            }
            continue;
        }
    }
    
}