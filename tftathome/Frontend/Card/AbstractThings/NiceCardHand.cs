using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;

namespace TFTAtHome.Frontend.Card;

public partial class NiceCardHand : StaticBody2D {
    public CollisionShape2D CardSpace {
        get => GetNodeOrNull("CardSpace") as CollisionShape2D;
    }

    public List<CardPlatform> Platforms {
        get {
            Array<CardPlatform> platforms = new();
            foreach (CardPlatform child in CardSpace.GetChildren()) {
                if (child.CardRoot != null)
                    platforms.Add(child);
            }
            return platforms.ToList();
        }
    }
    
    public float SpaceHeight { 
        get => CardSpace.Shape.GetRect().Size.Y*CardSpace.GlobalScale.Y;
    }
}