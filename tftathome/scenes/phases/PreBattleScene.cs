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


    public override void _Ready()
	{
        Node root = GetTree().Root.GetChild(0);
        GD.Print(root);

        Player testPlayer = new Player(1, "Test");


        PlayerUtil.AddPlayerListSceneToScene(root);
    }

    public void zimmer()
    {
        GD.Print("Zimmer");
        CollisionShape2D center = this.GetNode("CardHand1/CardSpace1") as CollisionShape2D;

        var cardPlatform = CardPlatformScene.Instantiate() as Node2D;
        var card = CardScene.Instantiate() as Node2D;
        cardPlatform.AddChild(card);
        center.AddChild(cardPlatform);
        GD.Print(cardPlatform.Name);
        GD.Print(cardPlatform.GetChildren());
        var cardBody = card.GetNode("CardBody").GetNode("CardCollision") as CollisionShape2D;

        var platforms = center.GetChildren();
        var amplitudeWeight = 4;


        if (platforms.Count == 0)
        {
            GD.Print("No platforms found");
            return;
        } else
        {
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
                GD.Print("Align1: " + alignResult);
                // Calculates the horizontal with cardWidth as spacing and handWidth as the total width of the area the cards are placed in. 
                var horizontalPlacement = cardWidth / 2 + handWidth / 2 - handWidth * interpolatedWeight;
                var verticalAmplitude = amplitudeWeight * cardBody.Scale.Y;

                if (alignResult > 0.5) alignResult = 1 - alignResult;
                GD.Print("Align2; " + alignResult);
                alignResult *= 2;
                GD.Print("Align3: " + alignResult);
                var verticalPlacement = Mathf.Lerp(-verticalAmplitude*cardCount, verticalAmplitude*cardCount, alignResult);

                platform.Position = new Vector2((float) horizontalPlacement, verticalPlacement*-1);

                var totalAngle = amplitudeWeight/2*platforms.Count;
                var angle = totalAngle / 2 - 3 * cardIndex;
                platform.RotationDegrees = angle;  
            }
        }
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
