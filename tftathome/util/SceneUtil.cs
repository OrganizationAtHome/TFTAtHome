using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFTAtHome.models;
using TFTAtHome.storage;

namespace TFTAtHome.util
{
    public static class SceneUtil
    {
        private static PackedScene _playerElementScene = GD.Load<PackedScene>("res://scenes/models/PlayerElementSceneV2.tscn");
        private static PackedScene _playerHandScene = GD.Load<PackedScene>("res://scenes/models/PlayerHandScene.tscn");

        public static void CreatePlayerElementContainer(Player player, Container parentContainer, bool playerhand, string name)
        {

            Node2D playerElement2D = null;
            // New container to insert card into
            Godot.Container container = new();
            container.Name = name;
            if (!playerhand)
            {
                container.CustomMinimumSize = new Vector2(220, 250);
                Node playerElementInstance = _playerElementScene.Instantiate();
                playerElement2D = playerElementInstance as Node2D;

            } else
            {
                container.CustomMinimumSize = new Vector2(220, 50);
                Node playerHandInstance = _playerHandScene.Instantiate();
                playerElement2D = playerHandInstance as Node2D;
            }
            playerElement2D.ApplyScale(new Vector2(0.35f, 0.35f));

            Container playerHandGrid = GetPlayerHandGridFromPlayerElementScene(playerElement2D);

            float playerHandVSeperation = playerHandGrid.GetThemeConstant("h_separation");
            int numberOfCards = player.GetPlayerHand().Count();
            float test = ((CardUtil.GetCardWidth()) + ((playerHandVSeperation - CardUtil.GetCardWidth()) * 2));
            float test2 = test * numberOfCards;
            playerHandGrid.CustomMinimumSize = new Vector2(test * (numberOfCards), 600);

            foreach (Card card in player.GetPlayerHand())
            {
                CardUtil.CreateCustomCardAndAddToContainer(card.CardName, playerHandGrid, 1.3f);
            }

            container.AddChild(playerElement2D);

            parentContainer.AddChild(container);
        }

        private static Container GetPlayerHandGridFromPlayerElementScene(Node node)
        {

            Node vBoxContainer = node.GetChildren()[1]; //Gets the vBox Container

            Node playerHandsContainer = vBoxContainer.GetChildren()[1];

            if (playerHandsContainer == null)
            {
                GD.Print("Could not find playerHandsContainer");
                throw new DirectoryNotFoundException("Could not find playerHandsContainer");
            }

            Container cardGridContainer = (Container)playerHandsContainer.GetChild(0);

            if (cardGridContainer == null)
            {
                GD.Print("Could not find cardGridContainer");
                throw new DirectoryNotFoundException("Could not find cardGridContainer");
            }

            return cardGridContainer;
        }

        private static Container GetPlayerHandScrollContainerFromPlayerElementScene(Node node)
        {
            Node vBoxContainer = node.GetChildren()[1]; //Gets the vBox Container

            Container playerHandsContainer = (Container)vBoxContainer.GetChildren()[1];

            return playerHandsContainer;
        }

        public static void AddButtonInCorner(Button button, Corner corner)
        {
            // Set the button's size
            button.CustomMinimumSize = new Vector2(100, 50); 

            // Set the button's anchor to the right side (1 for x-axis)
            button.AnchorRight = 1;

            switch (corner)
            {
                case Corner.TopRight:
                    // Anchor to the top-right corner
                    button.AnchorTop = 0;
                    button.AnchorBottom = 0;
                    break;

                case Corner.BottomRight:
                    // Anchor to the bottom-right corner
                    button.AnchorTop = 1;
                    button.AnchorBottom = 1;
                    break;
            }
        }

        public static void SwitchScene(string nameOfNewScene, Node mainSceneRoot)
        {
            SceneReferenceSingleton srs = SceneReferenceSingleton.GetInstance();

            PackedScene newActiveScene = srs.GetPhaseSceneByName(nameOfNewScene);

            GD.Print(newActiveScene);

            Node newActiveSceneNode = newActiveScene.Instantiate();

            GD.Print("NewActiveScene: " + newActiveSceneNode.Name);


            // GD.Print("MainSceneRoot: " + mainSceneRoot.Name);

            Node currentActiveScene = mainSceneRoot.GetChild(0);
            // GD.Print("CurrentActiveScene: " + currentActiveScene.Name);

            if (mainSceneRoot.GetChild(0) == currentActiveScene)
            {
                // Remove the current active scene
                mainSceneRoot.RemoveChild(currentActiveScene);

                // Add the new active scene
                mainSceneRoot.AddChild(newActiveSceneNode);
            }
            else
            {
                GD.PrintErr("Error: The provided currentActiveScene is not the first child of the root node.");
            }
        }

    }
}
