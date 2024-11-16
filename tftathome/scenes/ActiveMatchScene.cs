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


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        GameManager testGame = LocalStorage.GetGame();
        cardScrollContainer = GetNode("CardContainer") as ScrollContainer;
        gridContainer = cardScrollContainer.GetNode("CardGrid") as GridContainer;
        playerListScrollContainer = GetNode("PlayerListContainer") as ScrollContainer;


        // gridContainer = cardScrollContainer.GetNode("CardGrid") as GridContainer;
        playerListGrid = GetNode("PlayerListContainer/PlayerListGrid") as GridContainer;

        foreach (Card cardObj in testGame.GetActiveCardPool())
        {
            CardUtil.CreateCustomCardAndAddToContainer(cardObj.CardName, gridContainer, 0.6f);
        }

        List<Card> playerHand1 = new List<Card>();
        playerHand1.Add(testGame.GetActiveCardPool()[0]);
        playerHand1.Add(testGame.GetActiveCardPool()[1]);
        playerHand1.Add(testGame.GetActiveCardPool()[2]);
        playerHand1.Add(testGame.GetActiveCardPool()[3]);
        playerHand1.Add(testGame.GetActiveCardPool()[4]);
        playerHand1.Add(testGame.GetActiveCardPool()[5]);
        playerHand1.Add(testGame.GetActiveCardPool()[6]);
        playerHand1.Add(testGame.GetActiveCardPool()[7]);

        Player testPlayer = new Player(1, "Test");
        testPlayer.SetPlayerHand(playerHand1);

        SceneUtil.CreatePlayerElementContainer(testPlayer, playerListGrid, false, "fok");
        SceneUtil.CreatePlayerElementContainer(testPlayer, playerListGrid, false, "NavngivningV2");
        SceneUtil.CreatePlayerElementContainer(testPlayer, playerListGrid, false, "WAAAAAAA");
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

}
