using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using TFTAtHome.models;
using TFTAtHome.storage;
using TFTAtHome.util;

public partial class PreBattleScene : Node2D
{
    private VBoxContainer _p1Vbox;
    private VBoxContainer _p2Vbox;
    [Export]
    public PackedScene CardScene { get; set; }
    [Export]
    public PackedScene CardPlatformScene { get; set; }
    private int platformCount = 0;
    ulong center1Id;
    ulong center2Id;
    public static ulong PreBattleSceneId;
    public Node2D cardTargetted = null;


    public override void _Ready()
	{
        PreBattleSceneId = this.GetInstanceId();
        Node root = GetTree().Root.GetChild(0);
        GD.Print(root);
        center1Id = this.GetNode("CardHand1/CardSpace1").GetInstanceId();

        Player testPlayer = new Player(1, "Test");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (!CardLogic.isDragging)
        {
            if (cardTargetted != null)
                cardTargetted.Scale = new Vector2(1f, 1f);
            Node2D platformtarget = PlatformTargettingSystem();
            if (platformtarget != null)
            {
                cardTargetted = platformtarget.GetNode("Card") as Node2D;
                cardTargetted.Scale = new Vector2(1.2f, 1.2f);
            }
        }



    }

    public void zimmer()
    {
        CollisionShape2D center = InstanceFromId(center1Id) as CollisionShape2D;
        var cardPlatform = CardPlatformScene.Instantiate() as Node2D;
        var card = CardScene.Instantiate() as Node2D;
        cardPlatform.AddChild(card);
        cardPlatform.Name = "CardPlatform" + platformCount++;
        cardPlatform.AddToGroup("handPlatform");

        CardLogic yeet = card as CardLogic;
        
        center.AddChild(cardPlatform);

        ReshuffleHands();
    }

    public void printRecursive(Node node)
    {
        GD.Print(node.Name);
        foreach (Node child in node.GetChildren())
        {
            printRecursive(child);
        }
    }

    public void ReshuffleHands()
    {

        CollisionShape2D center = InstanceFromId(center1Id) as CollisionShape2D;
        var platforms = center.GetChildren();

        var amplitudeWeight = 5;


        if (platforms.Count == 0)
        {
            GD.Print("No platforms found");
            return;
        }
        else
        {
            var cardBody = center.GetChildren()[0].GetNode("Card/CardBody/CardCollision") as CollisionShape2D;
            // Place many cards
            for (int cardIndex = 0; cardIndex < platforms.Count; cardIndex++)
            {
                var platformCount = platforms.Count;
                var platform = platforms[cardIndex] as Node2D;
                var cardWidth = cardBody.Shape.GetRect().Size.X / 2 * platform.Scale.X;
                // Dynamically increases the hand width based on the number of cards to eleminate big gaps between the cards
                var handWidth = CalcRealisticHandWidthSize(center.Shape.GetRect().Size.X, platforms.Count);
                // Interpolates the relative placement of the card between 0 and 1
                var interpolatedWeight = (cardIndex + 1f) / platforms.Count;
                // Calculates the horizontal with cardWidth as spacing and handWidth as the total width of the area the cards are placed in. 
                var horizontalPlacement = cardWidth / 2 + handWidth / 2 - handWidth * interpolatedWeight;
                var verticalAmplitude = amplitudeWeight * cardBody.Scale.Y;


                // Here goes the vertical shit
                float alignResult = 0.5f;
                if (platformCount >= 2) alignResult = cardIndex / (platformCount - 1f);
                // makes alignResult (thereafter defined as x) revert from increasing in size to decreasing in size as to make the card fan look. 
                if (alignResult > 0.5) alignResult = 1 - alignResult;
                alignResult *= 2;
                var verticalPlacement = Mathf.Lerp(-verticalAmplitude * platformCount, verticalAmplitude * platformCount, alignResult);

                // Apply the calculated values
                platform.Position = new Vector2((float)horizontalPlacement, verticalPlacement * -1);

                // Make the cards mad
                var totalAngle = amplitudeWeight / 2 * platforms.Count;
                var angle = totalAngle / 2 - 3 * cardIndex;
                platform.RotationDegrees = angle;
            }
        }
    }

    public void FlatShuffleHand(CollisionShape2D center)
    {
        var platforms = center.GetChildren();

        if (platforms.Count == 0)
        {
            GD.Print("No platforms found");
            return;
        }
        else
        {
            var cardBody = center.GetChildren()[0].GetNode("Card/CardBody/CardCollision") as CollisionShape2D;
            // Place many cards
            for (int cardIndex = 0; cardIndex < platforms.Count; cardIndex++)
            {
                var cardCount = platforms.Count;
                var platform = platforms[cardIndex] as Node2D;
                var cardWidth = cardBody.Shape.GetRect().Size.X / 2 * platform.Scale.X;
                // Dynamically increases the hand width based on the number of cards to eleminate big gaps between the cards
                var handWidth = center.Shape.GetRect().Size.X * (1 - 1 / Math.Pow(1.15, platforms.Count));
                // Interpolates the relative placement of the card between 0 and 1
                var interpolatedWeight = (cardIndex + 1f) / platforms.Count;
                float alignResult = 0.5f;
                if (cardCount >= 2) alignResult = cardIndex / (cardCount - 1f);
                // Calculates the horizontal with cardWidth as spacing and handWidth as the total width of the area the cards are placed in. 
                var horizontalPlacement = cardWidth / 2 + handWidth / 2 - handWidth * interpolatedWeight;

                platform.Position = new Vector2((float)horizontalPlacement, 0);

            }
        }
    }

    public double CalcRealisticHandWidthSize(float x, int cardCount)
    {
        return x * (1 - 1 / Math.Pow(1.15, cardCount));
    }

    public Node2D PlatformTargettingSystem()
    {
        CollisionShape2D center = InstanceFromId(center1Id) as CollisionShape2D;
        var platforms = center.GetChildren().Cast<Node2D>().ToList();
        
        var moooooooose = center.GetGlobalMousePosition().X;
        (this.GetNode("Moose") as Label).Text = moooooooose.ToString();
        Node2D found = null;
        var lastindex = 0;
        for (int i = 0; i < platforms.Count; i++)
        {
            var platform = platforms[i];
            foreach (var group in platform.GetGroups())
            {
                GD.Print(group);
            }
            var platformX = platforms[i].GlobalPosition.X;
            if (i == platforms.Count) {
                found = platforms[i];
            } else if (platformX.CompareTo(moooooooose) < 0) {
                var lastPlat = platforms[lastindex].GlobalPosition.X;
                var currentPlat = platforms[i].GlobalPosition.X;
                var currentMooCompare = Math.Abs(currentPlat - moooooooose);
                var lastMooCompare = Math.Abs(lastPlat - moooooooose);
                if (currentMooCompare.CompareTo(lastMooCompare) < 0)
                {
                    found = platforms[i];
                    (this.GetNode("FoundCounter") as Label).Text = i.ToString();
                    break;
                }
                else
                {
                    found = platforms[lastindex];
                    (this.GetNode("FoundCounter") as Label).Text = lastindex.ToString();
                    break;
                }
            } else {
                lastindex = i;
            }
        }
        if (found == null && platformCount > 0)
        {
            found = platforms[platforms.Count - 1];
            (this.GetNode("FoundCounter") as Label).Text = (platforms.Count - 1).ToString();
        }
        return found;
    }

    private void EVENBETTERNAMETHATIKNOWTHINGS()
    {

    }
}
