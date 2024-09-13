using Godot;
using System;
using TFTAtHome.util;

public partial class ActiveMatchScene : Node2D
{
    private PackedScene cardScene = GD.Load<PackedScene>("res://scenes/models/card.tscn");
    private ScrollContainer scrollContainer;
    private GridContainer gridContainer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        scrollContainer = GetNode("CardContainer") as ScrollContainer;
        gridContainer = scrollContainer.GetNode("CardGrid") as GridContainer;
        // ColorRect colorRect = GetNode<ColorRect>("ColorRect");
        // Vector2 size = colorRect.Size;

        for (int i = 0; i < 5; i++)
        {
            Container container = new();
            container.CustomMinimumSize = new Vector2(220, 250);
            addCard(container);
        }
        Container container2 = new();
        container2.CustomMinimumSize = new Vector2(220, 250);
        createCustomCard("[center] ZimmerMAN [/center]", container2);
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

        container.AddChild(card);
        gridContainer.AddChild(container);
        GD.Print(gridContainer.ToString());
    }

    /*, string title, int early, int mid, int late, string trait, int cardCost */
    public void createCustomCard(string characterName, Container container)
    {
        Node card = cardScene.Instantiate();
        Node2D card2D = card as Node2D;
        card2D.ApplyScale(new Vector2(0.7f, 0.7f));
        card2D.GetChildren();
        RichTextLabel lblName = (RichTextLabel)getNodeFromCard(card2D, "Character_Name_Label");
        // GD.Print("createCustomCard: " + lblName.ToString());
        lblName.Text = characterName;
        container.AddChild(card);
        gridContainer.AddChild(container);
    }
}
