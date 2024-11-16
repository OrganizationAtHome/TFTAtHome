using Godot;
using System;
using System.ComponentModel;

public partial class CardBody : Area2D
{
    public static bool isDragging = false;
    public static int printCount = 0;
    bool isDraggable = false;
	bool isInsideDroppable = false;
	Node2D bodyRef;
    Vector2 initialPos;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        Node2D parent = GetParent() as Node2D;
        if (isDraggable) {
            if (Input.IsActionJustPressed("click")) {
                initialPos = parent.Position;
                isDragging = true;
            } if (Input.IsActionPressed("click")) {
                Vector2 mousePos = GetGlobalMousePosition();
                parent.GlobalPosition = mousePos;
            } else if (Input.IsActionJustReleased("click")) {
                isDragging = false;
                Tween tween = GetTree().CreateTween();
                if (isInsideDroppable) {
                    tween.TweenProperty(parent, "position", bodyRef.Position, 0.2).SetEase(Tween.EaseType.Out);
                } else {
                    tween.TweenProperty(parent, "position", initialPos, 0.2).SetEase(Tween.EaseType.Out);
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
            Vector2 vector2 = new Vector2(parentScale.X + 0.05f, parentScale.Y + 0.05f);
            parent.Scale = vector2;
        }

    }

	public void OnArea2DMouseExited() {

        if (!isDragging)
        {
            Node2D parent = GetParent() as Node2D;
            isDraggable = false;
            Vector2 parentScale = parent.Scale;
            Vector2 vector2 = new Vector2(parentScale.X - 0.05f, parentScale.Y - 0.05f);
            parent.Scale = vector2;
        }
    }
	public void OnArea2DBodyEntered(Node2D body) 
    {
        if (body.IsInGroup("droppable"))
        {
            isInsideDroppable = true;
            bodyRef = body;
        }
    }

    public void OnArea2DBodyExited(Node2D body)
    {
        if (body.IsInGroup("droppable"))
        {
            isInsideDroppable = false;
            bodyRef = null;
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
}
