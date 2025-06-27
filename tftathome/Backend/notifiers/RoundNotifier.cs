using System;

namespace TFTAtHome.Backend.notifiers;

public static class RoundNotifier
{
    public static event Action<int[]> PlayerTotalsFrontend;
    public static event Action<string[]> PlayerPhaseTotalsFrontend;
    
    public static void UpdatePlayerTotalsFrontend(int[] totals)
    {
        PlayerTotalsFrontend?.Invoke(totals);
    }
    public static void UpdatePlayerPhaseTotalsFrontend(string[] values)
    {
        PlayerPhaseTotalsFrontend?.Invoke(values);
    }
}