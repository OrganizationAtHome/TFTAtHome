using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFTAtHome.Backend.storage;
using TFTAtHome.Backend.models;

namespace TFTAtHome.util
{
    public static class CardUtil
    {
        private static PackedScene cardScene = GD.Load<PackedScene>("res://Frontend/Card/cardScene.tscn");
            
        public static Node CreateGodotCard(string cardName, float scale)
        {
                Card cardObj = LocalStorage.GetCardFromName(cardName); // ?? throw new Exception("Could not find card!");
                string[] cardHeader = { "CardTitle", "CardName" };
                string[] cardHeaderValues = { cardObj.CardTitle, cardObj.CardName };
                string[] statsName = { "Early", "Mid", "Late", "Trait", "Cost" };
                string[] statValues = cardObj.GetStatsValuesAsString();
                // Create the card node
                Node card = cardScene.Instantiate();

                Node2D card2D = card as Node2D;
                card2D.ApplyScale(new Vector2(scale, scale));

                // Set card stats and headers
                for (int i = 0; i < statsName.Length; i++)
                {
                    Label node = (Label)GetNodeFromCard(card2D, statsName[i]);
                    node.Text = statValues[i];
                }

                for (int i = 0; i < cardHeader.Length; i++)
                {
                    RichTextLabel node = (RichTextLabel)GetNodeFromCard(card2D, cardHeader[i]);
                    node.Text = "[center]" + cardHeaderValues[i] + "[/center]";
                }

            Texture2D newTexture = (Texture2D)GD.Load($"res://{cardObj.CardImgSrc}");
            Texture2D scaledTexture = RenderUtil.ResizeTexture(newTexture, 300f, 300f);

            Sprite2D sprite2D = (Sprite2D)GetNodeFromCard(card2D, "CardImg");
            
            sprite2D.Texture = scaledTexture;
            return card;
        }

        public static Node CreateGodotCard(Card cardInput, float scale)
        {
            string[] cardHeader = { "CardTitle", "CardName" };
            string[] cardHeaderValues = { cardInput.CardTitle, cardInput.CardName };
            string[] statsName = { "Early", "Mid", "Late", "Trait", "Cost" };
            string[] statValues = cardInput.GetStatsValuesAsString();

            // Create the card node
            Node card = cardScene.Instantiate();

            Node2D card2D = card as Node2D;
            card2D.ApplyScale(new Vector2(scale, scale));

            // Set card stats and headers
            for (int i = 0; i < statsName.Length; i++)
            {
                Label node = (Label)GetNodeFromCard(card2D, statsName[i]);
                node.Text = statValues[i];
            }

            for (int i = 0; i < cardHeader.Length; i++)
            {
                RichTextLabel node = (RichTextLabel)GetNodeFromCard(card2D, cardHeader[i]);
                node.Text = "[center]" + cardHeaderValues[i] + "[/center]";
            }

            Texture2D newTexture = (Texture2D)GD.Load($"res://{cardInput.CardImgSrc}");
            Texture2D scaledTexture = RenderUtil.ResizeTexture(newTexture, 300f, 300f);

            Sprite2D sprite2D = (Sprite2D)GetNodeFromCard(card2D, "CardImg");

            sprite2D.Texture = scaledTexture;
            return card;
        }

        public static void UpdateCardStats(Node2D card, int early, int mid, int late)
        {
            string[] statValues = { early.ToString(), mid.ToString(), late.ToString() };
            string[] statsName = { "Early", "Mid", "Late" };

            for (int i = 0; i < statValues.Length; i++)
            {
                Label node = (Label)GetNodeFromCard(card, statsName[i]);
                var goDotNodesErLort = node.Text;
                node.Text = statValues[i];
            }
        }

        public static Card GetCardModelFromCardNode(Node2D node2D)
        {
            string cardName = ((RichTextLabel)GetNodeFromCard(node2D, "CardName")).Text;

            Card card = LocalStorage.GetCardFromName(cardName);

            return card;
        }

        public static void HighLightCard(Node2D card, bool active)
        {
            ColorRect highlight = (ColorRect)GetNodeFromCard(card, "CardHighlight");
            highlight.Visible = active;
        }

        /**
     * RETURNS A Node OR NULL
     */
        private static Node GetNodeFromCard(Node2D card, string name)
        {
            Godot.Collections.Array<Node> slaves = card.GetNode("CardVisuals").GetChildren();

            foreach (var item in slaves)
            {

                if (item is Node node)
                {
                    if (node.Name.Equals(name))
                    {
                        return node;
                    }
                }
            }

            return null;
        }

        /**
         * INPUTS:
         *  CardName --> Name of the card you want to add
         *  ParentContainer --> The container you want to add the card to
         */
        public static void CreateCustomCardAndAddToContainer(string cardName, Container parentContainer, float scale)
        {
            /*
            Godot.Container container = new();
            container.CustomMinimumSize = new Vector2(220, 250);

            Node customCard = CreateGodotCard(cardName, scale);
            container.AddChild(customCard);

            parentContainer.AddChild(container); */
        }

        public static void CreateCardForGameBoardAndAddToContainer(Card card, Container parentContainer, float scale)
        {
            // Create a new container for the card
            Control container = new Control();

            // Create the card
            Node customCard = CreateGodotCard(card, scale);
            Node2D card2D = customCard as Node2D;

            // Get the size of the card
            Vector2 cardSize = GetCardSize(card2D);

            // Set the container's minimum size to the card's size
            container.CustomMinimumSize = cardSize;

            // Adjust the card's position to align with the container's top-left corner
            card2D.Position = new Vector2(cardSize.X / 2, cardSize.Y / 2);

            // Add the card to the container
            container.AddChild(customCard);

            // Add the container to the parent container
            parentContainer.AddChild(container);
        }

        public static Node2D CreateCardForBattleFieldPlatform(Card card, StaticBody2D platform)
        {
            Node customCard = CreateGodotCard(card, 1.0f);
            Node2D card2D = customCard as Node2D;

            platform.AddChild(card2D);

            return card2D;
        }

        public static float GetCardWidth()
        {
            Node card = cardScene.Instantiate();
            Node2D card2D = card as Node2D;
            ColorRect colorRect = card2D.GetNode("CardVisuals").GetNode("CardBackground") as ColorRect;
            Vector2 vector = colorRect.GetRect().Size;
            float width = vector.X;
            return width;
        }

        private static Vector2 GetCardSize(Node2D card2D)
        {
            // Get the ColorRect or background element to determine the card's size
            ColorRect colorRect = card2D.GetNode("CardVisuals").GetNode("CardBackground") as ColorRect;
            return colorRect.GetRect().Size * card2D.Scale;
        }
    }
}
