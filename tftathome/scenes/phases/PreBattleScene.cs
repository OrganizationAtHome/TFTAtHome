using Godot;
using System;
using System.Collections.Generic;
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


    public override void _Ready()
	{
        PreBattleSceneId = this.GetInstanceId();
        Node root = GetTree().Root.GetChild(0);
        GD.Print(root);
        center1Id = this.GetNode("CardHand1/CardSpace1").GetInstanceId();

        Player testPlayer = new Player(1, "Test");
    }

    public void zimmer()
    {

        CollisionShape2D center = InstanceFromId(center1Id) as CollisionShape2D;
        var cardPlatform = CardPlatformScene.Instantiate() as Node2D;
        var card = CardScene.Instantiate() as Node2D;
        cardPlatform.AddChild(card);
        cardPlatform.Name = "CardPlatform" + platformCount;
        cardPlatform.AddToGroup("handPlatform");

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
                var cardCount = platforms.Count;
                var platform = platforms[cardIndex] as Node2D;
                var cardWidth = cardBody.Shape.GetRect().Size.X / 2 * platform.Scale.X;
                // Dynamically increases the hand width based on the number of cards to eleminate big gaps between the cards
                var handWidth = fuckingYeet(center.Shape.GetRect().Size.X, platforms.Count);
                // Interpolates the relative placement of the card between 0 and 1
                var interpolatedWeight = (cardIndex + 1f) / platforms.Count;
                // Calculates the horizontal with cardWidth as spacing and handWidth as the total width of the area the cards are placed in. 
                var horizontalPlacement = cardWidth / 2 + handWidth / 2 - handWidth * interpolatedWeight;
                var verticalAmplitude = amplitudeWeight * cardBody.Scale.Y;


                // Here goes the vertical shit
                float alignResult = 0.5f;
                if (cardCount >= 2) alignResult = cardIndex / (cardCount - 1f);
                // makes alignResult (thereafter defined as x) revert from increasing in size to decreasing in size as to make the card fan look. 
                if (alignResult > 0.5) alignResult = 1 - alignResult;
                alignResult *= 2;
                var verticalPlacement = Mathf.Lerp(-verticalAmplitude * cardCount, verticalAmplitude * cardCount, alignResult);

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

        var amplitudeWeight = 4;


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

    public double fuckingYeet(float x, int cardCount)
    {
        return x * (1 - 1 / Math.Pow(1.15, cardCount));
    }

    public void IDKHOWTONAMETHINGS()
    {

    }

    private void EVENBETTERNAMETHATIKNOWTHINGS()
    {

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
