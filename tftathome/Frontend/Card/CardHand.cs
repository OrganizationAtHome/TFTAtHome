using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Godot.Collections;
using TFTAtHome.Frontend.Card;
using TFTAtHome.util;
using static TFTAtHome.Frontend.Singletons.CardNodeNameSingleton;

public partial class CardHand : NiceCardHand {
    public NiceCard LastCardTargetted;
    public NiceCard LastLastCardTargetted;
    private NiceCard lastCard = null;
    private double totalCardWidth;
    const float highlightSpeed = 0.15f;
    const float highlightSize = 1.5f;
    private float[] PlatformRotations;
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        
    }
    
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
        if (!CardLogic.isDragging) {
            
            CardPlatform platformtarget = PlatformTargettingSystem();
            if (platformtarget != null && !platformtarget.CardRoot.Equals(lastCard)) {
                LastLastCardTargetted = LastCardTargetted;
                LastCardTargetted = platformtarget.CardRoot;
                lastCard = platformtarget.CardRoot;
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
            if (LastCardTargetted != null && LastCardTargetted.isInHandPlatform)
                CardDown(LastCardTargetted);
            return null;
        }
        
        CardPlatform platformFound = null;
        var lastindex = 0;
        // I forgot how this works, but I was really cocking when I did, so please don't touch it. 
        for (int i = 0; i < platforms.Count; i++) {
            var platformX = platforms[i].GlobalPosition.X;
            if (i == platforms.Count) {
                platformFound = platforms[i];
            } else if (platformX.CompareTo(moooooooose) < 0) {
                var lastPlat = platforms[lastindex].GlobalPosition.X;
                var currentPlat = platforms[i].GlobalPosition.X;
                var currentMooCompare = Math.Abs(currentPlat - moooooooose);
                var lastMooCompare = Math.Abs(lastPlat - moooooooose);
                if (currentMooCompare.CompareTo(lastMooCompare) < 0) {
                    platformFound = platforms[i];
                    break;
                } else {
                    platformFound = platforms[lastindex];
                    break;
                }
            } else {
                lastindex = i;
            }
        }
        if (platformFound == null) {
            platformFound = platforms[^1]; // Last card is the first card visually
        
        }
        HighlightCard(platformFound.CardRoot);
        return platformFound;
    }

    private void HighlightCard(NiceCard card, bool hackermode = false) {
        CardUp(card);
        
        if (LastLastCardTargetted != null) {
            CardDown(LastLastCardTargetted);
        }
    }

    private void CardUp(NiceCard cardRoot, float heightincease = 1) {
        var upTween = GetTree().CreateTween();
        var height = cardRoot.Height;
        cardRoot.SetZIndex(1);
        cardRoot.Platform.Rotation = 0;
        
        cardRoot.Scale = new Vector2(highlightSize, highlightSize);
        cardRoot.Position = new Vector2(0, -1*(height/2));
        //tween.TweenProperty(cardRoot, "scale", new Vector2(highlightSize, highlightSize), highlightSpeed).SetEase(Tween.EaseType.Out);
        upTween.TweenProperty(cardRoot, "position", new Vector2(0, -1*(height/2*heightincease)-cardRoot.Platform.Position.Y), highlightSpeed).SetEase(Tween.EaseType.Out);
    }

    private void CardDown(NiceCard cardRoot) {
        var downTween = GetTree().CreateTween();
        downTween.TweenProperty(cardRoot, "position", new Vector2(0, 0), highlightSpeed).SetEase(Tween.EaseType.Out);
        //tween.TweenProperty(LastCardTargetted, "scale", new Vector2(1, 1), highlightSpeed).SetEase(Tween.EaseType.Out);
        cardRoot.Scale = new Vector2(1, 1);
        cardRoot.SetZIndex(0);
        var index = FindPlatformIndexByCard(cardRoot);
        if (index != -1) {
            cardRoot.Platform.Rotation = PlatformRotations[index];
        }
        
    }
    
    private int FindPlatformIndexByCard(NiceCard card) {
        var platforms = Platforms;
        for (int i = 0; i < platforms.Count; i++) {
            if (platforms[i].CardRoot != null && platforms[i].CardRoot.Equals(card)) {
                return i;
            }
        }
        return -1;
    }
    
    public void Shuffle(bool hasFan = true) {
        var platforms = Platforms;

        var amplitudeWeight = 5;


        if (platforms.Count == 0) {
            GD.PushWarning("No platforms found");
            return;
        } 
        var cardBody = platforms[0].CardRoot.CardCollision;
        var firstX = Double.PositiveInfinity;
        var lastX = 0.0; // I forgot why 0.0 is important, but it is.
        // Dynamically increases the hand width based on the number of cards to eleminate big gaps between the cards
        var handWidth = CalcRealisticHandWidthSize(CardSpace.Shape.GetRect().Size.X/2, platforms.Count);
        PlatformRotations = new float[platforms.Count];
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
                PlatformRotations[cardIndex] = platform.Rotation;
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

    private double CalcRealisticHandWidthSize(float length, int cardCount) {
        return length * (1 - 1 / Math.Pow(1.15, cardCount));
    }
}
