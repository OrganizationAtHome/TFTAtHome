using Godot;
using System;
using System.ComponentModel;

public partial class CardLogic : Area2D {
    public static bool isDragging = false;
    public static bool isAnimating = false;
    private static int printCount = 0;
    bool isDraggable = false;
    bool isInsideDroppable = false;
    ulong bodyRef;
    Vector2 initialPos;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) {
        if (isAnimating) { return;}
        Node2D card = GetParent() as Node2D;
        Node2D platform = card.GetParent() as Node2D;
        if (platform != null && platform.IsInGroup("handPlatform")) {
            PreBattleScene preBattleScene = InstanceFromId(PreBattleScene.PreBattleSceneId) as PreBattleScene;
            Node2D handCard = preBattleScene.cardTargetted;
            if (handCard != null && !this.Equals(handCard.GetNode("CardBody") as CardLogic)) {
                return;
            }
        }

        if (isDraggable) {
            if (Input.IsActionJustPressed("click")) {
                initialPos = card.Position;
                isDragging = true;
            }
            if (Input.IsActionPressed("click")) {       
                Vector2 mousePos = GetGlobalMousePosition();
                card.GlobalPosition = mousePos;
            } else if (Input.IsActionJustReleased("click")) {
                if (IsParentCardPlatform()) {
                    CollisionShape2D collision = platform.GetNode("PlatformCollision") as CollisionShape2D;
                    GD.Print(collision.ToGlobal(collision.Shape.GetRect().Position));
                    GD.Print(GetLocalMousePosition());
                    if (!new Rect2(collision.ToGlobal(collision.Shape.GetRect().Position), collision.Shape.GetRect().Size).HasPoint(GetGlobalMousePosition())) {
                        isDraggable = false; //Important! Animations can bypass MouseExited code and keep the card draggable
                    }
                }
                isAnimating = true;


                Tween tween = GetTree().CreateTween();
                tween.Connect("finished", Callable.From(() => { isDragging = false; isAnimating = false; }));
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
                        (InstanceFromId(PreBattleScene.PreBattleSceneId) as PreBattleScene).ReshuffleHands();// reshuffle hands
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
}
