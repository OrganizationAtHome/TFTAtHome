using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Godot.Collections;
using TFTAtHome.Frontend.Card;
using TFTAtHome.util;
using static TFTAtHome.Frontend.Singletons.CardNodeNameSingleton;

public partial class CardHand : NiceCardHand
{
    public NiceCard CardTargetted;
    public NiceCard LastCardTargetted;
    private NiceCard lastCard = null;
    private double totalCardWidth;
    const float highlightSpeed = 0.15f;
    const float highlightSize = 1.5f;
    private int cardTargettedIndex;
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        
    }
    
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        if (!CardLogic.isDragging) {
            
            CardPlatform platformtarget = PlatformTargettingSystem();
            if (platformtarget != null && !platformtarget.GetNode(CardRoot).Equals(lastCard)) {
                LastCardTargetted = CardTargetted;
                CardTargetted = platformtarget.cardRoot;
                lastCard = platformtarget.cardRoot;
            } 
        }
    }

    public CardPlatform PlatformTargettingSystem() {
        List<CardPlatform> platforms = this.Platforms;
        var moooooooose = CardSpace.GetGlobalMousePosition().X;
        
        
        if (platforms.Count == 0 ) return null;
        var width = (float) totalCardWidth;
        var centerPos = CardSpace.GlobalPosition;
        var newCenterPos = new Vector2(centerPos.X-width/2, centerPos.Y-SpaceHeight);
        var rect = new Rect2(newCenterPos, new Vector2(width, SpaceHeight*1.5f));
        
        if (!MathUtil.IsMouseOverCollisionShape2D(newCenterPos, rect, CardSpace.GetGlobalMousePosition())) {
            if (CardTargetted != null)
                CardDown(CardTargetted);
            return null;
        }
        
        CardPlatform PlatformFound = null;
        var lastindex = 0;
        
        for (int i = 0; i < platforms.Count; i++) {
            var platformX = platforms[i].GlobalPosition.X;
            if (i == platforms.Count) {
                PlatformFound = platforms[i];
            } else if (platformX.CompareTo(moooooooose) < 0) {
                var lastPlat = platforms[lastindex].GlobalPosition.X;
                var currentPlat = platforms[i].GlobalPosition.X;
                var currentMooCompare = Math.Abs(currentPlat - moooooooose);
                var lastMooCompare = Math.Abs(lastPlat - moooooooose);
                if (currentMooCompare.CompareTo(lastMooCompare) < 0) {
                    PlatformFound = platforms[i];
                    break;
                } else {
                    PlatformFound = platforms[lastindex];
                    break;
                }
            } else {
                lastindex = i;
            }
        }
        if (PlatformFound == null) {
            PlatformFound = platforms[^1]; // Last card is the first card visually
        
        }
        highlightCard(PlatformFound.cardRoot);
        return PlatformFound;
    }

    private void highlightCard(NiceCard card, bool hackermode = false) {
        CardUp(card);
        
        if (LastCardTargetted != null) {
            CardDown(LastCardTargetted);
        }
    }

    private void CardUp(NiceCard cardRoot, float heightincease = 1) {
        var upTween = GetTree().CreateTween();
        var height = cardRoot.Height;
        
        cardRoot.SetZIndex(1);
        
        
        cardRoot.Scale = new Vector2(highlightSize, highlightSize);
        cardRoot.Position = new Vector2(0, -1*(height/2));
        //tween.TweenProperty(cardRoot, "scale", new Vector2(highlightSize, highlightSize), highlightSpeed).SetEase(Tween.EaseType.Out);
        upTween.TweenProperty(cardRoot, "position", new Vector2(0, -1*(height/2*heightincease)), highlightSpeed).SetEase(Tween.EaseType.Out);
    }

    private void CardDown(NiceCard cardRoot) {
        var downTween = GetTree().CreateTween();
        downTween.TweenProperty(cardRoot, "position", new Vector2(0, 0), highlightSpeed).SetEase(Tween.EaseType.Out);
        //tween.TweenProperty(LastCardTargetted, "scale", new Vector2(1, 1), highlightSpeed).SetEase(Tween.EaseType.Out);
        cardRoot.Scale = new Vector2(1, 1);
        cardRoot.SetZIndex(0);
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
    }
    
    private float CalcCardHalfWidthSize(CollisionShape2D cardBody) {
        return cardBody.Shape.GetRect().Size.X / 2 * cardBody.GlobalScale.X;
    }

    public double CalcRealisticHandWidthSize(float length, int cardCount) {
        return length * (1 - 1 / Math.Pow(1.15, cardCount));
    }
}
