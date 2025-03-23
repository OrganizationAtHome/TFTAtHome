using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFTAtHome.Backend.models;
using TFTAtHome.Backend.storage;

namespace TFTAtHome.util
{
    public class PlayerUtil
    {
        private static PackedScene cardScene = GD.Load<PackedScene>("res://Frontend/Card/cardScene.tscn");
        private static SceneReferenceSingleton _srs = SceneReferenceSingleton.GetInstance();

        public static void AddCardToPlayerHand(Card card, Container playerHand)
        {
          CardUtil.CreateCustomCardAndAddToContainer(card.CardName, playerHand, 0.5f);
        }

        /**
         * ADDS THE CONTAINER OF ALL PLAYERS TO A GIVEN SCENE NODE
         */
        public static void AddPlayerListSceneToScene(Node parentNode)
        {
            GD.Print(parentNode);
            PackedScene playerListScene = _srs.GetPhaseSceneByName("PlayerListScene");

            GD.Print(playerListScene);

            Node playerListNode = playerListScene.Instantiate();

            GD.Print(playerListNode);

            parentNode.AddChild(playerListNode);
        }
    }
}
