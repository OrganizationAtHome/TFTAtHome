using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFTAtHome.models;

namespace TFTAtHome.util
{
    public static class SceneUtil
    {
        private static PackedScene _playerElementScene = GD.Load<PackedScene>("res://scenes/playerElementScene.tscn");

        public static void addPlayerElementToPlayerList(Player player, VBoxContainer playerListVBox)
        {
            Node playerElementInstance = _playerElementScene.Instantiate();
            Node2D playerElement2D = playerElementInstance as Node2D;
            playerElement2D.ApplyScale(new Vector2(0.7f, 0.7f));


            Node test = GetPlayerHandGridFromPlayerElementScene(playerElement2D);
            GD.Print(test.Name);
        }

        private static Node GetPlayerHandGridFromPlayerElementScene(Node2D node)
        {

            Node vBoxContainer = node.GetChildren()[1]; //Gets the vBox Container

            Node playerHandsContainer = vBoxContainer.GetChildren()[1];

            if (playerHandsContainer == null)
            {
                throw new DirectoryNotFoundException("Could not find playerHandsContainer");
            }

            Node cardGridContainer = playerHandsContainer.GetChild(0);

            if (cardGridContainer == null)
            {
                throw new DirectoryNotFoundException("Could not find cardGridContainer");
            }

            return cardGridContainer;
        }
    }
}
