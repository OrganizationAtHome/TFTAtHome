using Godot;
using System;

public partial class CardArea2D : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public override void _InputEvent(Viewport viewport, InputEvent @event, int shapeIdx)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
        {
            GD.Print("You clicked me correctly" + @event.GetInstanceId);
        }
        if (@event is InputEventMouseButton mouseEvent2 && mouseEvent2.Pressed && mouseEvent2.ButtonIndex == MouseButton.Right)
        {
            GD.Print("YEEET");
        }
    }
}
