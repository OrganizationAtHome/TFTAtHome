using Godot;
using System;
using System.Linq;

public partial class CardHand : StaticBody2D
{
    public Node2D cardTargetted = null;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		if (GetNode("CardSpace") == null) {
            GD.PrintErr("CardSpace: "+ this.Name + " node not found in CardHand.");
            return;
        }
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        if (!CardLogic.isDragging) {
            if (cardTargetted != null)
                cardTargetted.Scale = new Vector2(1f, 1f);
            Node2D platformtarget = PlatformTargettingSystem();
            if (platformtarget != null) {
                cardTargetted = platformtarget.GetNode("Card") as Node2D;
                cardTargetted.Scale = new Vector2(1.15f, 1.15f);
            }
        }
    }

    public StaticBody2D PlatformTargettingSystem() {
        CollisionShape2D center = this.GetNode("CardSpace") as CollisionShape2D;
        var platforms = center.GetChildren().Cast<StaticBody2D>().ToList();

        var moooooooose = center.GetGlobalMousePosition().X;
        StaticBody2D found = null;
        var lastindex = 0;
        for (int i = 0; i < platforms.Count; i++) {
            var platform = platforms[i];
            var platformX = platforms[i].GlobalPosition.X;
            if (i == platforms.Count) {
                found = platforms[i];
            } else if (platformX.CompareTo(moooooooose) < 0) {
                var lastPlat = platforms[lastindex].GlobalPosition.X;
                var currentPlat = platforms[i].GlobalPosition.X;
                var currentMooCompare = Math.Abs(currentPlat - moooooooose);
                var lastMooCompare = Math.Abs(lastPlat - moooooooose);
                if (currentMooCompare.CompareTo(lastMooCompare) < 0) {
                    found = platforms[i];
                    break;
                } else {
                    found = platforms[lastindex];
                    break;
                }
            } else {
                lastindex = i;
            }
        }
        if (found == null && platforms.Count > 0) {
            found = platforms[platforms.Count - 1];
        
        }
        return found;
    }

    public void Shuffle(bool HasFan = true) {
        CollisionShape2D center = this.GetNode("CardSpace") as CollisionShape2D;
        var platforms = center.GetChildren();

        var amplitudeWeight = 5;


        if (platforms.Count == 0) {
            GD.PushWarning("No platforms found: FanShuffle edition");
            return;
        } else {
            var cardBody = center.GetChildren()[0].GetNode("Card/CardBody/CardCollision") as CollisionShape2D;
            // Place many cards
            for (int cardIndex = 0; cardIndex < platforms.Count; cardIndex++) {
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

                var verticalPlacement = 0f;

                if (HasFan) {                 
                    // Here goes the vertical shit
                    float alignResult = 0.5f;
                    if (platformCount >= 2) alignResult = cardIndex / (platformCount - 1f);
                    // makes alignResult (thereafter defined as x) revert from increasing in size to decreasing in size as to make the card fan look. 
                    if (alignResult > 0.5) alignResult = 1 - alignResult;
                    alignResult *= 2;
                    verticalPlacement = Mathf.Lerp(-verticalAmplitude * platformCount, verticalAmplitude * platformCount, alignResult);



                    // Make the cards mad
                    var totalAngle = amplitudeWeight / 2 * platforms.Count;
                    var angle = totalAngle / 2 - 3 * cardIndex;
                    platform.RotationDegrees = angle;
                }

                // Apply the calculated values
                platform.Position = new Vector2((float)horizontalPlacement, verticalPlacement * -1);

            }
        }
    }

    public double CalcRealisticHandWidthSize(float x, int cardCount) {
        return x * (1 - 1 / Math.Pow(1.15, cardCount));
    }
}
