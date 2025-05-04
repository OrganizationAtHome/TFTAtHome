using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TFTAtHome.Backend.models;
using TFTAtHome.Backend.models.Effect;
using TFTAtHome.Backend.models.Matches;
using TFTAtHome.Backend.notifiers;

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

        public static void HighLightEffectableCards(List<Node2D> cardNodes)
        {
            foreach (var cardNode in cardNodes)
            {
                CardUtil.HighLightCard(cardNode, true);
            }
        }

        public static void SetupActiveEffectsButtons(GridContainer buttonContainer, Player player, Match match)
        {
            Func<MatchEffect, bool> useEffectMethod; // Here we define the function type that we will use to call the effect
            
            KeyValuePair<MatchEffect, int> currentEffect;


            if (player == match.Player1)
            {
                useEffectMethod = (e) => match.Player1Effects.UseMatchEffect(e);
                currentEffect = match.Player1Effects.GetCurrentMatchEffectForPlayer();
            } else
            {
                useEffectMethod = (e) => match.Player2Effects.UseMatchEffect(e);
                currentEffect = match.Player2Effects.GetCurrentMatchEffectForPlayer();
            }

            Button button = new Button();
            button.Text = currentEffect.Key.TraitName + " " + currentEffect.Value;
            button.Size = new Vector2(100, 50);
            buttonContainer.AddChild(button);
            button.Pressed += () =>
            {
                if (!useEffectMethod(currentEffect.Key))
                {
                    buttonContainer.RemoveChild(button);
                }
                else
                {
                    button.Text = currentEffect.Key.TraitName + " " + currentEffect.Value;
                }
                EffectNotifier.NotifyEffectUsed();
            };
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
