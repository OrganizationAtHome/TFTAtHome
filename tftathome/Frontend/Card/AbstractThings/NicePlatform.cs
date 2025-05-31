using Godot;
using TFTAtHome.Frontend.Singletons;

namespace TFTAtHome.Frontend.Card;

public partial class NicePlatform : StaticBody2D
{
    public StaticBody2D platform {
        get => this;
    }
    public CollisionShape2D platformCollision {
        get => platform.GetNode("PlatformCollision") as CollisionShape2D;
    }
    public ColorRect PlatformColor {
        get => platformCollision.GetNode("PlatformColor") as ColorRect;
    }
    public NiceCard cardRoot {
        get => this.GetNode(CardNodeNameSingleton.CardRoot) as NiceCard;
    }

    public CardHand CardHand
    {
        get => platform.GetParent().GetParent() as CardHand;
    }
    public CardLogic CardBody {
        get => cardRoot.GetNode(CardNodeNameSingleton.CardBody) as CardLogic;
    }
    public Vector2 PlatformPosition {
        get => platform.Position;
        set => platform.Position = value;
    }
    public Vector2 PlatformGlobalPosition {
        get => platform.GlobalPosition;
        set => platform.GlobalPosition = value;
    }
    public Vector2 PlatformGlobalScale {
        get => platform.GlobalScale;
        set => platform.GlobalScale = value;
    }
    public Vector2 PlatformSize {
        get => platformCollision.Shape.GetRect().Size;
    }
    public Vector2 PlatformSizeGlobal {
        get => new(platformCollision.Shape.GetRect().Size.X * platform.GlobalScale.X, platformCollision.Shape.GetRect().Size.Y * platform.GlobalScale.Y);
    }
    public float Height {
        get => platformCollision.Shape.GetRect().Size.Y * GlobalScale.Y;
    }
    public float Length {
        get => platformCollision.Shape.GetRect().Size.X * GlobalScale.X;
    }
    public bool AddCardToPlatform(NiceCard rootCard) {
        if (rootCard == null) {
            GD.PrintErr("Cannot add card to platform: rootCard is null.");
            return false;
        }
        this.AddChild(rootCard);
        rootCard.Position = new Vector2(0, 0);
        return true;
    }
    
    public void AddCardToPlatform(NiceCard rootCard, Node2D platformFrom) {
        platformFrom.RemoveChild(rootCard);
        this.AddChild(rootCard);
        rootCard.Position = new Vector2(0, 0);
    }
        
    public void SwitchCardPlatforms(NicePlatform platformFrom) {
        var cardFrom = platformFrom.cardRoot;
        var cardTo = this.cardRoot;
        platformFrom.RemoveChild(cardFrom);
        this.RemoveChild(cardTo);
        
        this.AddChild(cardFrom);
        platformFrom.AddChild(cardTo);
    }
    
    
}