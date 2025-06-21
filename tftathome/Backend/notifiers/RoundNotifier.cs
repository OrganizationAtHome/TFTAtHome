using System;

namespace TFTAtHome.Backend.notifiers;

public static class RoundNotifier
{
    public static event Action<int[]> PlayerTotalsFrontend;
    public static event Action<int[]> PlayerPhaseTotalsFrontend;
    
    public static void UpdatePlayerTotalsFrontend(int[] totals)
    {
        PlayerTotalsFrontend?.Invoke(totals);
    }
    public static void UpdatePlayerPhaseTotalsFrontend(int[] totals)
    {
        PlayerPhaseTotalsFrontend?.Invoke(totals);
    }
}