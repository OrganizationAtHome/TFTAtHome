using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TFTAtHome.Backend.models;
using TFTAtHome.Backend.models.Effect;

namespace TFTAtHome.util
{
    public static class MatchUtil
    {
        delegate void ButtonPressedDelegate(MatchEffect effect);
        public static void UpdateCardStatsListGodot(List<Node2D> cardNodes, List<Card> cardObjects)
        {
            for (int i = 0; i < cardNodes.Count; i++)
            {
                CardUtil.UpdateCardStats(cardNodes[i], cardObjects[i].Early, cardObjects[i].Mid, cardObjects[i].Late);
            }
        }
        public static void SetupActiveEffectsButtons(GridContainer buttonContainer, Player player, Match match)
        {
            Dictionary<MatchEffect, int> effectList = new Dictionary<MatchEffect, int>();

            Func<MatchEffect, bool> useEffectMethod; // Here we define the function type that we will use to call the effect

            if (player == match.Player1)
            {
                effectList = match.Player1Effects.MatchEffects;
                match.Player1Effects.SetupMatchEffects(match.CurrentCardsOnBoardP1);
                // Here we define the function that will be called when the button is pressed
                useEffectMethod = (e) => match.Player1Effects.UseMatchEffect(e);
            } else
            {
                effectList = match.Player2Effects.MatchEffects;
                match.Player2Effects.SetupMatchEffects(match.CurrentCardsOnBoardP2);
                useEffectMethod = (e) => match.Player2Effects.UseMatchEffect(e);
            }

            foreach (KeyValuePair<MatchEffect, int> effect in effectList)
            {
                Button button = new Button();
                button.Text = effect.Key.TraitName + " " + effect.Value;
                button.Size = new Vector2(100, 50);
                buttonContainer.AddChild(button);
                button.Pressed += () =>
                {
                    if (!useEffectMethod(effect.Key)) { // Will return whether or not there are any effect uses left
                        buttonContainer.RemoveChild(button);
                    } else
                    {
                        button.Text = effect.Key.TraitName + " " + --effectList[effect.Key];
                    }
                };
            }
        }

        public static void UseEffectButton(Button button, MatchEffect effect, Player player)
        {
            
        }

        /** 
         THIS METHOD NEEDS TO BE EDITED TO WORK WITH THE PROPER SCENE, RIGHT NOW IT IS USING THE TEST SCENE
         */
        public static List<Node2D> GetCardNodesFromContainer(Container cardGrid)
        {
            List<Node2D> cardNodes = new List<Node2D>();

            foreach (var node in cardGrid.GetChildren())
            {
                var node2D = node.GetChild(0) as Node2D;
                cardNodes.Add(node2D);
            }
            return cardNodes;
        }
    }
}
