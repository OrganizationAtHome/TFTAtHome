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
using TFTAtHome.Backend.models.Rounds;
using TFTAtHome.Backend.notifiers;
using TFTAtHome.util.ExtensionMethods;
using static TFTAtHome.Backend.storage.TraitSingleton;

namespace TFTAtHome.util
{
    public static class MatchUtil
    {
        public static void UpdateCardStatsListGodot(List<Node2D> cardNodes, List<Card> cardObjects)
        {
            for (int i = 0; i < cardNodes.Count; i++)
            {
                CardUtil.UpdateCardStats(cardNodes[i], cardObjects[i].Early, cardObjects[i].Mid, cardObjects[i].Late);
            }
        }

        /// <summary>
        /// This method highlights the cards in the frontend 
        /// </summary>
        /// <param name="cardNodes"></param>
        /// <param name="active"></param>
        public static void HighLightEffectableCards(List<Node2D> cardNodes, bool active)
        {
            foreach (var cardNode in cardNodes)
            {
                CardUtil.HighLightCard(cardNode, active);
            }
        }

        public static void SetupActiveEffectsButtons(GridContainer buttonContainer, Player player, Match match)
        {
            Func<MatchEffect, bool> useEffectMethod; // Here we define the function type that we will use to call the effect
            
            KeyValuePair<MatchEffect, int> currentEffect;

            
            // When button is pressed we need to notify frontend that they must click on card
            // 
            
            if (player == match.Player1)
            {
                useEffectMethod = (e) => match.Player1Effects.UseMatchEffect(e);
                var p1CurrentEff = match.Player1Effects.GetCurrentMatchEffectForPlayer();
                if (p1CurrentEff == null) return;
                currentEffect = (KeyValuePair<MatchEffect, int>)p1CurrentEff;
                if (currentEffect.Value == 0) return;
            } else
            {
                useEffectMethod = (e) => match.Player2Effects.UseMatchEffect(e);
                var p2CurrentEff = match.Player2Effects.GetCurrentMatchEffectForPlayer();
                if (p2CurrentEff == null) return;
                currentEffect = (KeyValuePair<MatchEffect, int>)p2CurrentEff;
                if (currentEffect.Value == 0) return;
            }

            string traitName = currentEffect.Key.TraitName;

            Button button = new Button();
            button.Text = traitName + " " + currentEffect.Value;
            button.Size = new Vector2(100, 50);
            buttonContainer.AddChild(button);
            button.Pressed += () =>
            {
                if (!useEffectMethod(currentEffect.Key))
                {
                    buttonContainer.RemoveChild(button);
                    if (traitName == Genius)
                    {
                        EffectNotifier.NotifyNeedsToUseGeniusEffect(player);
                    }
                    else
                    {
                        EffectNotifier.NotifyNeedsToUseEffect(player);
                    }
                    var effectRound = new EffectRound(match);
                    effectRound.CurrentEffect = currentEffect.Key;
                    match.CurrentRound = effectRound;
                    effectRound.IsUsingEffect = true;
                }
                else
                {
                    EffectNotifier.NotifyNeedsToUseEffect(player);
                    var effectRound = new EffectRound(match);
                    effectRound.CurrentEffect = currentEffect.Key;
                    match.CurrentRound = effectRound;
                    effectRound.IsUsingEffect = true;
                    button.Text = currentEffect.Key.TraitName + " " + currentEffect.Value;
                }
            };
        }
        
        public static void SetupSelectPhaseButtons(GridContainer buttonContainer, Player player, Match match)
        {
            Dictionary<String, String[]> stringsMap = new Dictionary<String, String[]>();
            // String[] stringValues = new[] { "Early->Mid", "Early->Late", "Mid->Late" };
            stringsMap.Add("Early->Mid", new []{"Early", "Mid"});
            stringsMap.Add("Early->Late", new []{"Early", "Late"});
            stringsMap.Add("Mid->Late", new []{"Mid", "Late"});

            foreach (KeyValuePair<string, string[]> pair in stringsMap)
            {
                Button button = new Button();
                button.Text = pair.Key;
                button.Size = new Vector2(100, 50);
                buttonContainer.AddChild(button);
                button.Pressed += () =>
                {
                    var currentRound = match.CurrentRound as EffectRound;
                    var geniusEffect = currentRound.CurrentEffect;
                    
                    geniusEffect.SelectedPhase1 = pair.Value[0];
                    geniusEffect.SelectedPhase2 = pair.Value[1];
                    EffectNotifier.NotifyNeedsToUseEffect(player);
                    buttonContainer.RemoveAllChildren();
                };
            }
        }

        public static void SetupDiceRollButton(GridContainer buttonContainer, Player player, Match match)
        {
            buttonContainer.RemoveAllChildren();
            Button button = new Button();
            button.Text = "Roll dice";
            button.Size = new Vector2(100, 50);
            buttonContainer.AddChild(button);
            button.Pressed += () =>
            {
                match.ThrowDice(player);
                int[] diceResults = match.GetDiceResultsForMatchForPlayer(player);
                Label dice1 = new Label();
                dice1.Text = diceResults[0].ToString();
                Label dice2 = new Label();
                dice2.Text = diceResults[1].ToString();
                buttonContainer.AddChild(dice1);
                buttonContainer.AddChild(dice2);
                buttonContainer.RemoveChild(button);
            };
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
