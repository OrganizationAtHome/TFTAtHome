using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFTAtHome.util
{
    public static class CardUtil
    {
        private static PackedScene cardScene = GD.Load<PackedScene>("res://scenes/models/card.tscn");
        
        
        public static Node2D createGodotCard(string cardName)
        {
            // getCard() -> Needs to be implemented from wherever we store our cards
            string[] cardHeader = { "Navn", "Title" };
            string imgSrc = "ImgSrc";
            string[] stats = {"Early", "Mid", "Late", "Trait", "Cost"};
           Node card = cardScene.Instantiate();
           Node2D card2D = card as Node2D;
           card2D.ApplyScale(new Vector2(0.7f, 0.7f));
           card2D.GetChildren();

            for (int i = 0; i < stats.Length; i++)
            {
                Label node = (Label)getNodeFromCard(card2D, stats[i]);
                node.Text = stats[i]; // Needs to get the value from the card itself
            }
            for (int i = 0; i < cardHeader.Length; i++)
            {
                RichTextLabel node = (RichTextLabel)getNodeFromCard(card2D, cardHeader[i]);
                node.Text = cardHeader[i]; // Needs to get the value from the card itself
            }

            Sprite2D sprite2D = (Sprite2D)getNodeFromCard(card2D, "Character_Name_Label");
            sprite2D.Texture.ResourcePath = imgSrc;

            return card2D;
        }

        /**
     * RETURNS A Node OR NULL
     */
        private static Node getNodeFromCard(Node2D card, string name)
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
    }
}
