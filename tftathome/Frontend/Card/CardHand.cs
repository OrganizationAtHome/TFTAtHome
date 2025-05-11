using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Godot.Collections;
using TFTAtHome.util;

public partial class CardHand : StaticBody2D
{
    public Node2D cardTargetted;
    private double totalCardWidth;
    
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		if (GetNode("CardSpace") == null) {
            GD.PrintErr("CardSpace: "+ Name + " node not found in CardHand.");
            return;
        }
    }
    
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        if (!CardLogic.isDragging) {
            
            Node2D platformtarget = PlatformTargettingSystem();
            if (platformtarget != null) {
                cardTargetted = platformtarget.GetNode("Card") as Node2D;
            }
        }
    }

    public StaticBody2D PlatformTargettingSystem() {
        CollisionShape2D center = GetNode("CardSpace") as CollisionShape2D;
        var platforms = center.GetChildren().Cast<StaticBody2D>().ToList();
        var moooooooose = center.GetGlobalMousePosition().X;
        
        
        if (platforms.Count == 0 ) return null;
        var width = (float) totalCardWidth;
        var centerPos = center.GlobalPosition;
        var newCenterPos = new Vector2(centerPos.X-(width/2), centerPos.Y-center.Shape.GetRect().Size.Y/2*center.GlobalScale.Y);
        var rect = new Rect2(newCenterPos, new Vector2(width, center.Shape.GetRect().Size.Y*center.GlobalScale.Y));
        if (!MathUtil.IsMouseOverCollisionShape2D(newCenterPos, rect, center.GetGlobalMousePosition())) {
            return null;
        }
        
        StaticBody2D found = null;
        var lastindex = 0;
        int i;
        for (i = 0; i < platforms.Count; i++) {
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
            found = platforms[^1]; // Last card is the first card visually
        
        }
        highlightCard(found.GetNode("Card").GetNode("CardBody") as CardLogic, center.GetChildren());
        return found;
    }

    private void highlightCard(CardLogic card, Array<Node> centerNodes, bool hackermode = false) {
        const float highlightSpeed = 0.15f;
        const float highlightSize = 1.15f;
        
        int heightincease = hackermode ? 10 : 2;
        if (cardTargetted != card) {
            var tween = GetTree().CreateTween();
            var rect = (card.GetNode("CardCollision") as CollisionShape2D).Shape.GetRect();
            tween.TweenProperty(card.GetParent(), "position", new Vector2(0, -1*(rect.Size.Y*heightincease*card.GlobalScale.Y)), highlightSpeed).SetEase(Tween.EaseType.Out);
            tween.TweenProperty(card.GetParent(), "scale", new Vector2(highlightSize, highlightSize), highlightSpeed).SetEase(Tween.EaseType.Out);
        } if (cardTargetted != null) {
            var tween = GetTree().CreateTween();
            tween.TweenProperty(cardTargetted, "position", new Vector2(0, 0), highlightSpeed).SetEase(Tween.EaseType.Out);
            tween.TweenProperty(cardTargetted, "scale", new Vector2(1, 1), highlightSpeed).SetEase(Tween.EaseType.Out);
        }

        for (int i = 0; i < centerNodes.Count; i++) {
            var platform = centerNodes[i] as Node2D;
            if ((platform.GetNode("Card").GetNode("CardBody") as CardLogic).Equals(card)) {
                var temp = centerNodes[i];
                centerNodes.RemoveAt(i);
                centerNodes.Insert(centerNodes.Count, temp);
            }
        }
        
    }

    public void Shuffle(bool hasFan = true) {
        CollisionShape2D center = GetNode("CardSpace") as CollisionShape2D;
        var platforms = center.GetChildren();

        var amplitudeWeight = 5;


        if (platforms.Count == 0) {
            GD.PushWarning("No platforms found");
            return;
        } 
        var cardBody = center.GetChildren()[0].GetNode("Card/CardBody/CardCollision") as CollisionShape2D;
        var firstX = Double.PositiveInfinity;
        var lastX = 0.0;
        // Dynamically increases the hand width based on the number of cards to eleminate big gaps between the cards
        var handWidth = CalcRealisticHandWidthSize(center.Shape.GetRect().Size.X/2, platforms.Count);
        // Place many cards
        for (int cardIndex = 0; cardIndex < platforms.Count; cardIndex++) {
            var platformCount = platforms.Count;
            var platform = platforms[cardIndex] as Node2D;
            var cardWidth = CalcCardHalfWidthSize(cardBody);


            // Interpolates the relative placement of the card between 0 and 1
            var interpolatedWeight = (cardIndex + 1f) / platforms.Count;
            // Calculates the horizontal with cardWidth as spacing and handWidth as the total width of the area the cards are placed in. 
            var horizontalPlacement = cardWidth + handWidth / 2 - handWidth * interpolatedWeight;

            var verticalPlacement = 0f;
            var verticalAmplitude = amplitudeWeight * cardBody.GlobalScale.Y;
            if (hasFan) {
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
        
        var FirstPlatform = (Node2D)platforms[0];
        var firstCardPos = FirstPlatform.GlobalPosition.X;
        
        var lastPlatform = (Node2D) platforms[^1];
        var lastCardPos = lastPlatform.GlobalPosition.X;
        // Calculate the total width of the cards
        totalCardWidth = (firstCardPos + CalcCardHalfWidthSize(cardBody)) - (lastCardPos - CalcCardHalfWidthSize(cardBody)); 
        
        GD.Print("FirstCardPos: " + firstCardPos);
        GD.Print("LastCardPos: " + lastCardPos);
        GD.Print("TotalCardWidth: " + totalCardWidth);
    }
    
    private float CalcCardHalfWidthSize(CollisionShape2D cardBody) {
        return cardBody.Shape.GetRect().Size.X / 2 * cardBody.GlobalScale.X;
    }

    public double CalcRealisticHandWidthSize(float length, int cardCount) {
        return length * (1 - 1 / Math.Pow(1.15, cardCount));
    }
}
