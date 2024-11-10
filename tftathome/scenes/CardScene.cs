using Godot;
using System;

public partial class CardScene : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ColorRect colorRect = GetNode<ColorRect>("ColorRect");
		Vector2 size = colorRect.Size;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
