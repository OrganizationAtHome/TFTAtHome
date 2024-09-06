using Godot;
using System;
using TFTAtHome.util;

public partial class ActiveMatchScene : Node2D
{
    private PackedScene cardScene = GD.Load<PackedScene>("res://scenes/models/cardController.tscn");
    private ScrollContainer scrollContainer;
    private GridContainer gridContainer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        scrollContainer = GetNode("CardContainer") as ScrollContainer;
        gridContainer = scrollContainer.GetNode("CardGrid") as GridContainer;
        ColorRect colorRect = GetNode<ColorRect>("ColorRect");
        Vector2 size = colorRect.Size;

        for (int i = 0; i < 5; i++)
        {
            Container container = new Container();
            container.CustomMinimumSize = new Vector2(300, 350);
            addCard(container);
        }
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        // addCard();
	}

    public void addCard(Container container)
    {
        Node card = cardScene.Instantiate();
        Node2D card2D = card as Node2D;
        card2D.ApplyScale(new Vector2(0.7f, 0.7f));

        container.AddChild(card2D);
        gridContainer.AddChild(container);
        GD.Print(gridContainer.ToString());
        foreach (Node child in gridContainer.GetChildren())
        {
            GD.Print(child);
        }
    }
}
