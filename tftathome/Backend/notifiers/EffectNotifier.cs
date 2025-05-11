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
        public static event Action<Card> OnEffectUsed;
        public static event Action<Player> NeedsToUseEffect;
        public static void NotifyEffectUsed(Card card)
        {
            GD.Print("OnEffectUsed");
            OnEffectUsed?.Invoke(card);
        }

        public static void NotifyNeedsToUseEffect(Player player)
        {
            GD.Print("NeedsToUseEffect");
            NeedsToUseEffect?.Invoke(player);
        }
    }
}