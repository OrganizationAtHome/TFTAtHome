using Godot;
using System;

public partial class CardArea2D : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		InputPickable = true;
		// this.InputEvent += (viewport, @event, shapeIdx) => CardClicked(viewport, @event, shapeIdx);
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void CardClicked(Node viewport, InputEvent @event, long shapeIdx)
	{
        GD.Print("You clicked me retard");
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
		{
			GD.Print("You clicked me correctly");
		}
	}

    public override void _InputEvent(Viewport viewport, InputEvent @event, int shapeIdx)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
        {
            GD.Print("You clicked me correctly");
        }
    }
}
