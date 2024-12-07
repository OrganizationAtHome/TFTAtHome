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
        var  card = CardScene.Instantiate() as Node2D;
        cardPlatform.AddChild(card);
        center.AddChild(cardPlatform);
        GD.Print(cardPlatform.Name);
        GD.Print(cardPlatform.GetChildren());
        var cardBody = card.GetNode("CardBody").GetNode("CardCollision") as CollisionShape2D;

        var platforms = center.GetChildren();
        var amplitudeWeight = 2;


        if (platforms.Count == 0)
        {
            GD.Print("No platforms found");
            return;
        } else
        {
            // Place many cards
            for (int i = 0; i < platforms.Count; i++)
            {
                var platform = platforms[i] as Node2D;
                var cardWidth = cardBody.Shape.GetRect().Size.X / 2 * platform.Scale.X;
                var halfCount = platforms.Count / 2;
                var handWidth = center.Shape.GetRect().Size.X * (1 - 1 / Math.Pow(1.15, platforms.Count));
                var newSpacing = handWidth / platforms.Count;
                var interpolatedWeight = (i + 1f) / platforms.Count;
                var horizontalPlacement = cardWidth / 2 + handWidth / 2 - handWidth * interpolatedWeight;
                var verticalAmplitude = amplitudeWeight*2 * cardBody.Scale.Y;
                var verticalPlacement = 0f;
                if (i < platforms.Count /2)
                {
                    verticalPlacement = (halfCount - i) * -1 * verticalAmplitude + verticalAmplitude;
                } else if (i > platforms.Count / 2)
                {
                    verticalPlacement = -1 * verticalAmplitude * i + verticalAmplitude;
                } 

                platform.Position = new Vector2((float) horizontalPlacement, verticalPlacement);
                var totalAngle = amplitudeWeight/2*platforms.Count;
                var angle = totalAngle / 2 - 3 * i;
                platform.RotationDegrees = angle;
            }
        }
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
