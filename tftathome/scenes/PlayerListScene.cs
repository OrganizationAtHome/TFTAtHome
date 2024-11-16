using Godot;
using System;
using System.Collections.Generic;
using TFTAtHome.models;
using TFTAtHome.storage;
using TFTAtHome.util;

public partial class PlayerListScene : Node
{
    [Export]
    private PackedScene cardScene;
    [Export]
    private PackedScene playerElementScene = GD.Load<PackedScene>("res://scenes/models/PlayerElementSceneV2.tscn");
    private ScrollContainer cardScrollContainer;
    private ScrollContainer playerListScrollContainer;
    private GridContainer gridContainer;
    private GridContainer playerListGrid;

    public override void _Ready()
	{
        GameManager testGame = LocalStorage.GetGame();
        playerListGrid = GetNode("PlayerListContainer/PlayerListGrid") as GridContainer;
        playerListScrollContainer = GetNode("PlayerListContainer") as ScrollContainer;
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

        SceneUtil.CreatePlayerElementContainer(testPlayer, playerListGrid, false);
        SceneUtil.CreatePlayerElementContainer(testPlayer, playerListGrid, false);
        SceneUtil.CreatePlayerElementContainer(testPlayer, playerListGrid, false);


        /* TODO
		ADD CODE FOR INSERTION OF ALL CURRENT PLAYERS FROM GAMEMANAGER USING PlayerElement model
		*/
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
