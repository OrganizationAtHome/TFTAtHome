using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TFTAtHome.models;

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

        /** 
         THIS METHOD NEEDS TO BE EDITED TO WORK WITH THE PROPER SCENE, RIGHT NOW IT IS USING THE TEST SCENE
         */
        public static List<Node2D> GetCardNodesFromContainer(Container cardGrid)
        {
            List<Node2D> cardNodes = new List<Node2D>();
            foreach (var node in cardGrid.GetChildren())
            {
                cardNodes.Add(node as Node2D);
            }
            return cardNodes;
        }
    }
}
