using Godot;
using System;
using System.Collections.Generic;
using TFTAtHome.models;
using TFTAtHome.storage;
using TFTAtHome.util;

public partial class PreBattleScene : Node2D
{
    private VBoxContainer _p1Vbox;
    private VBoxContainer _p2Vbox;
    [Export]
    public PackedScene CardScene { get; set; }
    [Export]
    public PackedScene CardPlatformScene { get; set; }


    public override void _Ready()
	{
        Node root = GetTree().Root.GetChild(0);
        GD.Print(root);
        GameManager testGame = LocalStorage.GetGame();
        _p1Vbox = GetNode("PlayerBattleContainer/P1VBox") as VBoxContainer;
        _p2Vbox = GetNode("PlayerBattleContainer/P2VBox") as VBoxContainer;
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

        SceneUtil.CreatePlayerElementContainer(testPlayer, _p1Vbox, true, "PlayerContainer1");
        SceneUtil.CreatePlayerElementContainer(testPlayer, _p2Vbox, true, "PlayerContainer2");

        PlayerUtil.AddPlayerListSceneToScene(root);
    }

    public void zimmer()
    {
        GD.Print("Zimmer");
        Node center = this.GetNode("CenterPoint1");

        var CardPlatform = CardPlatformScene.Instantiate();
        var Card = CardScene.Instantiate();
        CardPlatform.AddChild(Card);
        center.AddChild(CardPlatform);
        GD.Print(CardPlatform.Name);
        GD.Print(CardPlatform.GetChildren());

        var platforms = center.GetChildren();

        for (int i = 0; i < platforms.Count; i++)
        {
            var platform = platforms[i] as Node2D;
            platform.Scale = new Vector2(0.5f, 0.5f);
            platform.Position = new Vector2(100*i, 0);
            platform.RotationDegrees = 0;

        }
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
