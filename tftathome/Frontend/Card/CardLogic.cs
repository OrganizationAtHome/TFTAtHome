using Godot;
using System;
using System.ComponentModel;
using TFTAtHome.Backend.models;
using TFTAtHome.Backend.notifiers;
using TFTAtHome.Frontend.Card;
using TFTAtHome.util;
using static TFTAtHome.Frontend.Singletons.CardNodeNameSingleton;

public partial class CardLogic : Node2D {
    public static bool isDragging = false;
    public static bool isAnimating = false;
    public bool IsEffectAble = false;
    public int CardId = 0;
    private static int printCount = 0;
    bool isDraggable = false;
    bool isInsideDroppable = false;
    ulong bodyRef;
    Vector2 initialPos;
    bool QueuedForClick = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        var rootCard = this.CardRoot();
        NicePlatform platform = rootCard.Platform;
        CardHand handCard = platform.CardHand;

        if (handCard != null && platform.IsInGroup("handPlatform")) {
            NiceCard targettedCard = handCard.CardTargetted;
            if (targettedCard != null && !this.Equals(targettedCard.CardBody)) {
                return;
            }
        }
        CollisionShape2D collision = platform.platformCollision;

        if (isDraggable) {
            if (Input.IsActionJustPressed("click")) {
                if (isDragging) {
                    QueuedForClick = true;
                    return;
                }
                // Implement check for click when player is using effect
                if (IsEffectAble)
                {
                    GD.Print("Effect clicked");
                    EffectNotifier.NotifyEffectUsed(CardId);
                    return;
                }
                /*
                if (card.Apply)
                {
                    // Do something "important"
                    return;
                } */
                initialPos = rootCard.Position;
                isDragging = true;
            }
            if (Input.IsActionPressed("click") && (isDragging || (QueuedForClick && isMouseOverPlatform(collision)))) {  
                if (QueuedForClick && !isAnimating) {
                    // Do whatever is in IsActionJustPressed + falsify QueuedForClick
                    initialPos = rootCard.Position;
                    isDragging = true;
                    QueuedForClick = false;
                }

                if (isDragging) {
                    Vector2 mousePos = GetGlobalMousePosition();
                    rootCard.GlobalPosition = mousePos;
                }
            } else if (Input.IsActionJustReleased("click") && isDragging) {
                if (isAnimating) { return; }
                isAnimating = true;


                Tween tween = GetTree().CreateTween();
                tween.Connect("finished", Callable.From(() => { softReset(); isDragging = false; isAnimating = false;  }));
                if (isInsideDroppable) {
                    
                    var body = InstanceFromId(bodyRef) as Node2D;
                    Node2D cardParent = rootCard.GetParent() as Node2D;

                    cardParent.RemoveChild(rootCard);
                    body.AddChild(rootCard);
                    rootCard.Position = new Vector2(0, 0);
                    tween.TweenProperty(rootCard, "position", new Vector2(0, 0), 0.15).SetEase(Tween.EaseType.Out);

                    body.RemoveFromGroup("droppable");
                    if (cardParent.GetGroups().Contains("handPlatform")) {
                        cardParent.GetParent().RemoveChild(cardParent);
                        handCard.Shuffle();// reshuffle hands
                        softReset();
                    } else {
                        cardParent.AddToGroup("droppable");
                    }
                } else {
                    if (IsParentCardPlatform()) {
                        tween.TweenProperty(rootCard, "position", new Vector2(0, 0), 0.15).SetEase(Tween.EaseType.Out);
                    } else {
                        tween.TweenProperty(rootCard, "position", initialPos, 0.2).SetEase(Tween.EaseType.Out);
                        GD.PushWarning("CardPlatform not found: " + rootCard.GetParent().Name + " " + rootCard.GetParent().GetType());
                    }
                }
            }
        }
    }



    public void OnArea2DMouseEntered()
    {

        Node2D parent = CardRoot().Platform;
        if (parent == null) return;
        if (!parent.IsInGroup("handPlatform") && !isDragging) {

            Vector2 vector2 = new Vector2(1.05f, 1.05f);
            CardRoot().Scale = vector2;
        }
        if (!isDragging) {
            isDraggable = true;
        }
    }

    public void OnArea2DMouseExited()
    {
        var Platform = CardRoot().Platform;
        if (Platform == null) return;
        if (!Platform.IsInGroup("handPlatform") && !isDragging) {
            Vector2 vector2 = new Vector2(1f, 1f);
            CardRoot().Scale = vector2;
        }
        if (!isDragging) {
            isDraggable = false;
        }
    }
    public void OnArea2DBodyEntered(Node2D body) {
        if (body.IsInGroup("droppable")) {
            GD.Print("Body entered");
            isInsideDroppable = true;
            bodyRef = body.GetInstanceId();
        }
    }

    public void OnArea2DBodyExited(Node2D body) {
        if (body.IsInGroup("droppable") && body.GetInstanceId() == bodyRef) {
            GD.Print("Body exited");
            isInsideDroppable = false;
        }

    }

    public void Print() {
        printCount++;
        GD.Print("printCount: ", printCount);
        GD.Print("isDragging: ", isDragging);
        GD.Print("isDraggable: ", isDraggable);
        GD.Print("isInsideDroppable: ", isInsideDroppable);
    }

    private bool IsParentCardPlatform() {
        if (GetParent().GetParent() == null) {
            return false;
        }
        return GetParent().GetParent().Name.ToString().Contains("CardPlatform");
    }

    private void softReset() {
        if (!isMouseOverPlatform(CardRoot().CardCollision)) {
            isDraggable = false;
            isInsideDroppable = false;
            QueuedForClick = false;
        }
    }

    private bool isMouseOverPlatform(CollisionShape2D collision)
    {
        var oldRect = collision.Shape.GetRect();
        var rect = new Rect2(oldRect.Position, new Vector2(oldRect.Size.X*collision.GlobalScale.X, oldRect.Size.Y*collision.GlobalScale.Y));
        return MathUtil.IsMouseOverCollisionShape2D(collision.ToGlobal(collision.Shape.GetRect().Position), rect, collision.GetGlobalMousePosition());
    }

    public NiceCard CardRoot() {
        return GetParent() as NiceCard;
    }
}
