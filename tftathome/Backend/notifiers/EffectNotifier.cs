using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFTAtHome.Backend.models;

namespace TFTAtHome.Backend.notifiers
{
    public static class EffectNotifier
    {
        public static event Action<Player> CardEffectUpdate;
        public static event Action<int> OnEffectUsed;
        public static event Action<GeniusEffect> OnGeniusEffectUsed;
        public static event Action<Player> NeedsToUseEffect;
        public static event Action<Player> NeedsToUseGeniusEffect;
        
        public static void NotifyCardEffectUpdate(Player nextPlayer)
        {
            GD.Print("NotifyCardEffectUpdate called");
            CardEffectUpdate?.Invoke(nextPlayer);
        }
        public static void NotifyEffectUsed(int cardId)
        {
            // GD.Print("OnEffectUsed");
            OnEffectUsed?.Invoke(cardId);
        }

        public static void NotifyNeedsToUseGeniusEffect(Player player)
        {
            NeedsToUseGeniusEffect?.Invoke(player);
        }

        public static void NotifyNeedsToUseEffect(Player player)
        {
            // GD.Print("NeedsToUseEffect");
            NeedsToUseEffect?.Invoke(player);
        }
    }
}