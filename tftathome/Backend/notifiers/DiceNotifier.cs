using System;
using TFTAtHome.Backend.models;

namespace TFTAtHome.Backend.notifiers;

public static class DiceNotifier
{
    public static event Action<Player> MustThrowDice;
    
    public static void NotifyMustThrowDice(Player nextPlayer)
    {
        MustThrowDice?.Invoke(nextPlayer);
    }
    
    
}