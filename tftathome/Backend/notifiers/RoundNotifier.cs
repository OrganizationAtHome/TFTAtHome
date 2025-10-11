using System;

namespace TFTAtHome.Backend.notifiers;

public static class RoundNotifier
{
    public static event Action<int[]> PlayerTotalsFrontend;
    public static event Action<string[]> PlayerPhaseTotalsFrontend;
    
    /// <summary>
    /// Sends total of all card values for the given phase (If dice has been rolled, it sends the values of the dice results)
    /// </summary>
    /// <param name="totals"></param>
    public static void UpdatePlayerTotalsFrontend(int[] totals)
    {
        PlayerTotalsFrontend?.Invoke(totals);
    }
    /// <summary>
    /// Sends the result of the given round
    /// </summary>
    /// <param name="values"></param>
    public static void UpdatePlayerPhaseTotalsFrontend(string[] values)
    {
        PlayerPhaseTotalsFrontend?.Invoke(values);
    }
}