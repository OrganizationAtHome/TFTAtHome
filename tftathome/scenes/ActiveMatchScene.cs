using Godot;
using System;
using System.Collections.Generic;
using TFTAtHome.models;
using TFTAtHome.storage;
using TFTAtHome.util;


public partial class ActiveMatchScene : Node2D
{
    [Export]
    private PackedScene cardScene;
    [Export]
    private PackedScene playerElementScene = GD.Load<PackedScene>("res://scenes/models/PlayerElementSceneV2.tscn");
    private ScrollContainer cardScrollContainer;
    private ScrollContainer playerListScrollContainer;
    private GridContainer gridContainer;
    private GridContainer playerListGrid;

    [Rpc]
    public static bool ConnectionTest()
    {
        GD.Print("ConnectionTest");
        return true;
    }


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        Game testGame = LocalStorage.GetGame();
        cardScrollContainer = GetNode("CardContainer") as ScrollContainer;
        gridContainer = cardScrollContainer.GetNode("CardGrid") as GridContainer;
        playerListScrollContainer = GetNode("PlayerListContainer") as ScrollContainer;

        gridContainer = cardScrollContainer.GetNode("CardGrid") as GridContainer;
        playerListGrid = GetNode("PlayerListContainer/PlayerListGrid") as GridContainer;
        GD.Print(playerListGrid);

        foreach (Card cardObj in testGame.getActiveCardPool())
        {
            CardUtil.CreateCustomCardAndAddToContainer(cardObj.CardName, gridContainer, 0.6f);
        }

        List<Card> playerHand1 = new List<Card>();
        playerHand1.Add(testGame.getActiveCardPool()[0]);
        playerHand1.Add(testGame.getActiveCardPool()[1]);
        playerHand1.Add(testGame.getActiveCardPool()[2]);
        playerHand1.Add(testGame.getActiveCardPool()[3]);
        playerHand1.Add(testGame.getActiveCardPool()[4]);
        playerHand1.Add(testGame.getActiveCardPool()[5]);
        playerHand1.Add(testGame.getActiveCardPool()[6]);
        playerHand1.Add(testGame.getActiveCardPool()[7]);

        Player testPlayer = new Player(1, "Test", playerHand1);

        SceneUtil.CreatePlayerElementContainer(testPlayer, playerListGrid);
        SceneUtil.CreatePlayerElementContainer(testPlayer, playerListGrid);
        SceneUtil.CreatePlayerElementContainer(testPlayer, playerListGrid);
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

}
