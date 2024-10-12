using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFTAtHome.models;
using TFTAtHome.storage;

namespace TFTAtHome.util
{
    public class PlayerUtil
    {
        private static PackedScene cardScene = GD.Load<PackedScene>("res://scenes/models/cardScene.tscn");

        public static void AddCardToPlayerHand(Card card, Container playerHand)
        {
          CardUtil.CreateCustomCardAndAddToContainer(card.CardName, playerHand);
        }
    }
}
