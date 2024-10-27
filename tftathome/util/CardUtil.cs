using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFTAtHome.storage;
using TFTAtHome.models;

namespace TFTAtHome.util
{
    public static class CardUtil
    {
        private static PackedScene cardScene = GD.Load<PackedScene>("res://scenes/models/cardScene.tscn");
        
        public static Node CreateGodotCard(string cardName, float scale)
        {
                Card cardObj = LocalStorage.getCardFromName(cardName); // ?? throw new Exception("Could not find card!");
                string[] cardHeader = { "CardTitle", "CardName" };
                string[] cardHeaderValues = { cardObj.CardTitle, cardObj.CardName };
                string[] statsName = { "Early", "Mid", "Late", "Trait", "Cost" };
                string[] statValues = cardObj.getStatsValuesAsString();

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

            Sprite2D sprite2D = (Sprite2D)GetNodeFromCard(card2D, "CardImgSrc");
            
            sprite2D.Texture = scaledTexture;
            return card;
        }

        /**
     * RETURNS A Node OR NULL
     */
        private static Node GetNodeFromCard(Node2D card, string name)
        {
            Godot.Collections.Array<Node> slaves = card.GetChild(0).GetChildren();

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

            // New container to insert card into
            Godot.Container container = new();
            container.CustomMinimumSize = new Vector2(220, 250);

            Node customCard = CreateGodotCard(cardName, scale);
            container.AddChild(customCard);

            parentContainer.AddChild(container);
        }

        public static float GetCardWidth()
        {
            Node card = cardScene.Instantiate();
            Node2D card2D = card as Node2D;
            ColorRect colorRect = card2D.GetChild(0).GetChild(0) as ColorRect;
            Vector2 vector = colorRect.GetRect().Size;
            float width = vector.X;
            return width;
        }
    }
}
