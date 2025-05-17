using Godot;
using System;
using System.ComponentModel;
using TFTAtHome.Backend.models;
using TFTAtHome.Backend.notifiers;
using TFTAtHome.util;

public partial class CardLogic : Area2D {
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
    public override void _Process(double delta) {
        
        Node2D card = GetParent() as Node2D;
        Node2D platform = card.GetParent() as Node2D;
        CardHand handCard = platform.GetParent().GetParent() as CardHand;

        if (handCard != null && platform.IsInGroup("handPlatform")) {
            Node2D targettedCard = handCard.cardTargetted;
            if (targettedCard != null && !Equals(targettedCard.GetNode("CardBody") as CardLogic)) {
                return;
            }
        }
        CollisionShape2D collision = platform.GetNode("PlatformCollision") as CollisionShape2D;

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
                initialPos = card.Position;
                isDragging = true;
            }
            if (Input.IsActionPressed("click") && (isDragging || (QueuedForClick && isMouseOverPlatform(collision)))) {  
                if (QueuedForClick && !isAnimating) {
                    // Do whatever is in IsActionJustPressed + falsify QueuedForClick
                    initialPos = card.Position;
                    isDragging = true;
                    QueuedForClick = false;
                }

                if (isDragging) {
                    Vector2 mousePos = GetGlobalMousePosition();
                    card.GlobalPosition = mousePos;
                }
            } else if (Input.IsActionJustReleased("click") && isDragging) {
                if (isAnimating) { return; }
                isAnimating = true;


                Tween tween = GetTree().CreateTween();
                tween.Connect("finished", Callable.From(() => { softReset(); isDragging = false; isAnimating = false;  }));
                if (isInsideDroppable) {
                    
                    var body = InstanceFromId(bodyRef) as Node2D;
                    Node2D cardParent = card.GetParent() as Node2D;

                    cardParent.RemoveChild(card);
                    body.AddChild(card);
                    card.Position = new Vector2(0, 0);
                    tween.TweenProperty(card, "position", new Vector2(0, 0), 0.15).SetEase(Tween.EaseType.Out);

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
                        tween.TweenProperty(card, "position", new Vector2(0, 0), 0.15).SetEase(Tween.EaseType.Out);
                    } else {
                        tween.TweenProperty(card, "position", initialPos, 0.2).SetEase(Tween.EaseType.Out);
                        GD.PushWarning("CardPlatform not found: " + card.GetParent().Name + " " + card.GetParent().GetType());
                    }
                }
            }
        }
    }



    public void OnArea2DMouseEntered() {
        Node2D cardRoot = GetParent() as Node2D;
        Node2D parent = cardRoot.GetParent() as Node2D;
        if (parent == null) return;
        if (!parent.IsInGroup("handPlatform") && !isDragging) {

            Vector2 vector2 = new Vector2(1.05f, 1.05f);
            cardRoot.Scale = vector2;
        }
        if (!isDragging) {
            isDraggable = true;
        }
    }

    public void OnArea2DMouseExited() {
        Node2D cardRoot = GetParent() as Node2D;
        Node2D parent = cardRoot.GetParent() as Node2D;
        if (parent == null) return;
        if (!parent.IsInGroup("handPlatform") && !isDragging) {
            Vector2 vector2 = new Vector2(1f, 1f);
            cardRoot.Scale = vector2;
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
        Node2D card = GetParent() as Node2D;
        Node2D platform = card.GetParent() as Node2D;
        CollisionShape2D collision = platform.GetNode("PlatformCollision") as CollisionShape2D;
        if (!isMouseOverPlatform(collision)) {
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

}
