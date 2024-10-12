using Godot;
using System;
using System.Collections.Generic;
using TFTAtHome.models;
using TFTAtHome.storage;
using TFTAtHome.util;


public partial class ActiveMatchScene : Node2D
{
    private PackedScene cardScene = GD.Load<PackedScene>("res://scenes/models/cardScene.tscn");
    private PackedScene playerElementScene = GD.Load<PackedScene>("res://scenes/playerElementScene.tscn");
    private ScrollContainer cardScrollContainer;
    private ScrollContainer playerListScrollContainer;
    private GridContainer gridContainer;
    private VBoxContainer playerListVBox;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{

        Game testGame = LocalStorage.GetGame();
        cardScrollContainer = GetNode("CardContainer") as ScrollContainer;
        gridContainer = cardScrollContainer.GetNode("CardGrid") as GridContainer;
        playerListScrollContainer = GetNode("PlayerListContainer") as ScrollContainer;

        gridContainer = cardScrollContainer.GetNode("CardGrid") as GridContainer;
        playerListVBox = GetNode("PlayerListContainer/PlayerList") as VBoxContainer;


        foreach (Card cardObj in testGame.getActiveCardPool())
        {
            CardUtil.CreateCustomCardAndAddToContainer(cardObj.CardName, gridContainer);
        }

        List<Card> playerHand1 = new List<Card>();
        playerHand1.Add(testGame.getActiveCardPool()[0]);
        playerHand1.Add(testGame.getActiveCardPool()[1]);
        playerHand1.Add(testGame.getActiveCardPool()[2]);

        Player testPlayer = new Player(1, "Test", playerHand1);

        SceneUtil.addPlayerElementToPlayerList(testPlayer, playerListVBox);
        /*
        Godot.Container container2 = new();
        container2.CustomMinimumSize = new Vector2(220, 250);
        Godot.Container container3 = new();
        container3.CustomMinimumSize = new Vector2(220, 250);
        addCard(container2, playerListVBox);
        addCard(container3, playerListVBox); */
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        // addCard();
	}

    public void addCard(Container container, Container parentContainer)
    {
        Node card = cardScene.Instantiate();
        Node2D card2D = card as Node2D;
        card2D.ApplyScale(new Vector2(0.6f, 0.6f));

        container.AddChild(card);
        parentContainer.AddChild(container);
        GD.Print(gridContainer.ToString());
    }
    

    public void createCustomCard(string characterName, Container container)
    {
        Node card = CardUtil.CreateGodotCard(characterName);
        container.AddChild(card);
    }

    private static Node getNodeFromCard(Node2D card, string name)
    {
        Godot.Collections.Array<Node> slaves = card.GetChild(0).GetChildren();

        foreach (var item in slaves)
        {

            if (item is Node node)
            {
                if (node.Name.Equals(name))
                {
                    return node;
                }
            }
        }

        return null;
    }
    
}
