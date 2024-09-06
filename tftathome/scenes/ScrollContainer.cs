using Godot;
using System;

public partial class ScrollContainer : Godot.ScrollContainer
{

    // private PackedScene cardScene = GD.Load<PackedScene>("res://scenes/models/card.tscn");
	// private ScrollContainer scrollContainer;
	// private GridContainer gridContainer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{

		/*
        ScrollContainer scrollContainer = GetNode("CardContainer") as ScrollContainer;
        GD.Print(scrollContainer);
        */
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
    }
}
