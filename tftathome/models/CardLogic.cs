using Godot;
using System;
using System.ComponentModel;

public partial class CardLogic : Area2D
{
    public static bool isDragging = false;
    public static int printCount = 0;
    bool isDraggable = false;
	bool isInsideDroppable = false;
    ulong bodyRef;
    Vector2 initialPos;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        Node2D card = GetParent() as Node2D;
        if (isDraggable) {
            if (Input.IsActionJustPressed("click")) {
                initialPos = card.Position;
                isDragging = true;
            } if (Input.IsActionPressed("click")) {
                Vector2 mousePos = GetGlobalMousePosition();
                card.GlobalPosition = mousePos;
            } else if (Input.IsActionJustReleased("click")) {
                isDragging = false;
                Tween tween = GetTree().CreateTween();
                if (isInsideDroppable) {
                    var body = InstanceFromId(bodyRef) as Node2D;
                    card.GetParent().RemoveChild(card);
                    body.AddChild(card);
                    card.Position = new Vector2(0, 0);
                    tween.TweenProperty(card, "position", new Vector2(0, 0), 0.2).SetEase(Tween.EaseType.Out);

                } else {
                    Node2D parent = card.GetParent() as Node2D;
                    if (IsParentCardPlatform())
                    {
                        tween.TweenProperty(card, "position", new Vector2(0, 0), 0.2).SetEase(Tween.EaseType.Out);
                    } else
                    {
                        tween.TweenProperty(card, "position", initialPos, 0.2).SetEase(Tween.EaseType.Out);
                        GD.PushWarning("CardPlatform not found: " + card.GetParent().Name + " " + card.GetParent().GetType());
                    }
                }
            }
        }
    }

	public void OnArea2DMouseEntered()
    {
        if (!isDragging)
        {
            Node2D parent = GetParent() as Node2D;
            isDraggable = true;
            Vector2 parentScale = parent.Scale;
            Vector2 vector2 = new Vector2(1.05f, 1.05f);
            parent.Scale = vector2;
        }

    }

	public void OnArea2DMouseExited() {

        if (!isDragging)
        {
            Node2D parent = GetParent() as Node2D;
            isDraggable = false;
            Vector2 vector2 = new Vector2(1f, 1f);
            parent.Scale = vector2;
        }
    }
	public void OnArea2DBodyEntered(Node2D body) 
    {
        if (body.IsInGroup("droppable"))
        {
            GD.Print("Body entered");
            isInsideDroppable = true;
            bodyRef = body.GetInstanceId();
        }
    }

    public void OnArea2DBodyExited(Node2D body)
    {
        if (body.IsInGroup("droppable") && body.GetInstanceId() == bodyRef)
        {
            GD.Print("Body exited");
            isInsideDroppable = false;
        }

    }

    public void Print()
    {
        printCount++;
        GD.Print("printCount: ", printCount);
        GD.Print("isDragging: ", isDragging);
        GD.Print("isDraggable: ", isDraggable);
        GD.Print("isInsideDroppable: ", isInsideDroppable);
    }

    private bool IsParentCardPlatform()
    {
        return GetParent().GetParent().Name.ToString().Contains("CardPlatform");
    }
}
