using Godot;
using System;
using System.ComponentModel;
using TFTAtHome.Backend.models;
using TFTAtHome.Backend.notifiers;
using TFTAtHome.Frontend.Card;
using TFTAtHome.util;
using static TFTAtHome.Frontend.Singletons.CardNodeNameSingleton;

public partial class CardLogic : Node2D {
    // Zimmer's stuff
    public static bool isDragging = false;
    public static bool isAnimating = false;
    bool isDraggable = false;
    bool queueIsDraggable = false;
    bool isInsideDroppable = false;
    ulong bodyRef;
    Vector2 initialPos;
    bool QueuedForClick = false;
    
    // Stuff Danie needs 
    public bool IsEffectAble = false;
    public int CardId = 0;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) {
        var rootCard = this.CardRoot();
        NicePlatform platform = rootCard.Platform;
        CardHand handCard = platform.CardHand;
        var isMousedOver = IsMouseOverPlatform(platform.platformCollision);
        if (!isDragging && queueIsDraggable && isMousedOver) {
            isDraggable = true;
            queueIsDraggable = false;
        }
        
        
        if (handCard != null && platform.IsInGroup("handPlatform")) {
            NiceCard targettedCard = handCard.LastCardTargetted;
            if (targettedCard != null && !this.Equals(targettedCard.CardBody)) {
                return;
            }
        }
        
        if (isDraggable) {
            if (Input.IsActionJustPressed("click")) {
                if (isDragging) {
                    QueuedForClick = true;
                    return;
                }
                GD.Print("Yeet");
                foreach (var group in CardRoot().Platform.GetGroups()) {
                    GD.Print(group);
                }
                // Implement check for click when player is using effect
                if (IsEffectAble) {
                    GD.Print("Effect clicked");
                    EffectNotifier.NotifyEffectUsed(CardId);
                    return;
                }
                initialPos = rootCard.Position;
                isDragging = true;
            }
            if (Input.IsActionPressed("click") && (isDragging || (QueuedForClick && isMousedOver))) {  
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
                tween.Connect("finished", Callable.From(() => { SoftReset(); isDragging = false; isAnimating = false;  }));
                if (isInsideDroppable) {
                    
                    var platformTo = InstanceFromId(bodyRef) as NicePlatform;
                    var platformFrom = rootCard.Platform; 
                    tween.TweenProperty(rootCard, "position", new Vector2(0, 0), 0.15).SetEase(Tween.EaseType.Out);
                    if (!platformFrom.IsInGroup("droppable")) {
                        return;
                    }
                    
                    // Case for moving card to battlefield (from CardHand)
                    if (platformFrom.GetGroups().Contains("handPlatform")) {
                        platformTo.AddCardToPlatform(rootCard, platformFrom, true);
                        handCard.Shuffle();// CardHand have changed card amount, so we need to shuffle
                        platformFrom.CardRoot.Position = new Vector2(0, 0);
                        HardReset();
                        // Case for switching card positions 
                    } else if (platformFrom.GetGroups().Contains("battlefieldPlatform") && platformTo?.CardRoot != null) {
                        platformFrom.CardRoot.Position = new Vector2(0, 0);
                        platformTo.SwitchCardPlatforms(platformFrom);
                    } else {
                        platformFrom.CardRoot.Position = new Vector2(0, 0);
                        platformTo.AddCardToPlatform(rootCard, true);
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

    private void OnArea2DMouseEntered() {
        var platformFrom = CardRoot().Platform;
        if (platformFrom == null) return;
        if (!platformFrom.IsInGroup("handPlatform") && !isDragging) {

            Vector2 vector2 = new Vector2(1.05f, 1.05f);
            CardRoot().Scale = vector2;
        }
        if (!isDragging) {
            isDraggable = true;
        } else {
            queueIsDraggable = true;
        }
    }

    private void OnArea2DMouseExited() {
        var platformFrom = CardRoot().Platform;
        if (platformFrom == null) return;
        if (!platformFrom.IsInGroup("handPlatform") && !isDragging) {
            Vector2 vector2 = new Vector2(1f, 1f);
            CardRoot().Scale = vector2;
        }
        if (!isDragging) {
            isDraggable = false;
        } else {
            queueIsDraggable = false;
        }
    }
    private void OnArea2DBodyEntered(Node2D platformTo) {
        if (platformTo is not CardPlatform platformTo2) {
            return;
        }
        var platformFrom = CardRoot().Platform;
        if (platformTo.IsInGroup("droppable") && CardRoot().Platform != platformTo && 
            (platformTo2.CardRoot == null || (!platformFrom.GetGroups().Contains("handPlatform") && platformTo2.CardRoot != null))) {
            GD.Print("Body entered");
            isInsideDroppable = true;
            bodyRef = platformTo.GetInstanceId();
        }
    }

    private void OnArea2DBodyExited(Node2D body) {
        if (body.IsInGroup("droppable") && body.GetInstanceId() == bodyRef && CardRoot().Platform != body) {
            GD.Print("Body exited");
            isInsideDroppable = false;
        }
    }

    private bool IsParentCardPlatform() {
        if (GetParent().GetParent() == null) {
            return false;
        }
        return GetParent().GetParent().Name.ToString().Contains("CardPlatform");
    }

    private Boolean SoftReset() {
        if (!IsMouseOverPlatform(CardRoot().CardCollision)) {
            isDraggable = false;
            isInsideDroppable = false;
            QueuedForClick = false;
            return true;
        }
        
        return false;
    }

    private void HardReset() {
        if (SoftReset()) {
            
        }
    }

    private bool IsMouseOverPlatform(CollisionShape2D collision)
    {
        var oldRect = collision.Shape.GetRect();
        var rect = new Rect2(oldRect.Position, new Vector2(oldRect.Size.X*collision.GlobalScale.X, oldRect.Size.Y*collision.GlobalScale.Y));
        return MathUtil.IsMouseOverCollisionShape2D(collision.ToGlobal(collision.Shape.GetRect().Position), rect, collision.GetGlobalMousePosition());
    }

    public NiceCard CardRoot() {
        return GetParent() as NiceCard;
    }
}
