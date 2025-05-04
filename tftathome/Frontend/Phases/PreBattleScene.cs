using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using TFTAtHome.Backend.models;
using TFTAtHome.Backend.models.Matches;
using TFTAtHome.Backend.storage;
using TFTAtHome.util;

public partial class PreBattleScene : Node2D
{
    private VBoxContainer _p1Vbox;
    private VBoxContainer _p2Vbox;
    [Export]
    public PackedScene CardScene { get; set; }
    [Export]
    public PackedScene CardPlatformScene { get; set; }
    private int platformCount = 0;
    ulong center1Id;
    ulong center2Id;
    public static ulong PreBattleSceneId;
    [Export]
    public StaticBody2D CardPlatform { get; set; }
    [Export]
    public StaticBody2D CardPlatform2 { get; set; }
    [Export]
    public StaticBody2D CardPlatform3 { get; set; }
    [Export]
    public StaticBody2D CardPlatform4 { get; set; }
    [Export]
    public StaticBody2D CardPlatform5 { get; set; }
    [Export]
    public StaticBody2D CardPlatform6 { get; set; }
    [Export]
    GridContainer P1EffectButtons { get; set; }
    [Export]
    GridContainer P2EffectButtons { get; set; }

    private Match match;
    private List<Node2D> p1CardNodes;
    private List<Node2D> p2CardNodes;


    public override void _Ready()
	{
        PreBattleSceneId = this.GetInstanceId();
        Node root = GetTree().Root.GetChild(0);
        GD.Print(root);
        center1Id = this.GetNode("CardHandMe/CardSpace").GetInstanceId();
        
        Player testPlayer = new Player(1, "Test");

        P1EffectButtons = GetNode<GridContainer>("P1EffectButtons");
        
        SetupActiveTraitTest();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        
    }

    public void zimmer()
    {
        CollisionShape2D center = InstanceFromId(center1Id) as CollisionShape2D;
        var cardPlatform = CardPlatformScene.Instantiate() as Node2D;
        var card = CardScene.Instantiate() as Node2D;
        cardPlatform.AddChild(card);
        cardPlatform.Name = "CardPlatform" + platformCount++;
        cardPlatform.AddToGroup("handPlatform");

        CardLogic yeet = card as CardLogic;
        
        center.AddChild(cardPlatform);

        (this.GetNode("CardHandMe") as CardHand).Shuffle();
    }

    public void printRecursive(Node node)
    {
        GD.Print(node.Name);
        foreach (Node child in node.GetChildren())
        {
            printRecursive(child);
        }
    }

    public void SetupActiveTraitTest()
    {
        Player player1 = new Player(1, "Player 1");
        Player player2 = new Player(2, "Player 2");

        List<StaticBody2D> p1Platforms = new List<StaticBody2D>() { CardPlatform4, CardPlatform5, CardPlatform6 };
        List<StaticBody2D> p2Platforms = new List<StaticBody2D>() { CardPlatform, CardPlatform2, CardPlatform3 };

        Card elizabeth = LocalStorage.GetCardFromName("ELIZABETH II");
        Card redfoo = LocalStorage.GetCardFromName("REDFOO");
        Card lightYagami = LocalStorage.GetCardFromName("LIGHT YAGAMI"); // 6, 12, 1, "Genius, true

        Card ericCartman = LocalStorage.GetCardFromName("ERIC CARTMAN"); // 5, 8, 5, "Drawing", true
        Card elonMusk = LocalStorage.GetCardFromName("ELON MUSK"); // 5, 7, 6, "Leader", false
        Card deadpool = LocalStorage.GetCardFromName("DEADPOOL"); // 5, 1, 11, "MovieHero", true


        player1.SetPlayerHand(new List<Card> { elizabeth, redfoo, lightYagami });
        player2.SetPlayerHand(new List<Card> { ericCartman, elonMusk, deadpool });

        match = new Match(player1, player2);

        GD.Print(match);

        // Act: Adding cards to the board
        // Player 1 adds Eric Cartman

        var p1Hand = match.Player1Hand;
        var p2Hand = match.Player2Hand;

        match.AddCardToBoard(match.Player1Hand[0], player1); // Elizabeth

        match.AddCardToBoard(match.Player2Hand[0], player2); // Eric Cartman (Drawing): 5, 8, 5, true

        match.AddCardToBoard(match.Player1Hand[0], player1); // Deadpool (MovieHero): 5, 1, 11, true

        match.AddCardToBoard(match.Player2Hand[0], player2); // Elon Musk (Leader): 5, 7, 6, false

        match.AddCardToBoard(match.Player1Hand[0], player1); // Light Yagami (Genius): 6, 12, 1, true

        match.AddCardToBoard(match.Player2Hand[0], player2); // Chuck Norris (TVCelebrity): 7, 8, 3, false

        match.RunInitialRound();

        p1CardNodes = new List<Node2D>();
        p2CardNodes = new List<Node2D>();

        for (int i = 0; i < match.CurrentCardsOnBoardP1.Count; i++)
        {
            Node2D card = CardUtil.CreateCardForBattleFieldPlatform(match.CurrentCardsOnBoardP1[i], p1Platforms[i]);
            p1CardNodes.Add(card);
        }

        for (int i = 0; i < match.CurrentCardsOnBoardP2.Count; i++)
        {
            Node2D card = CardUtil.CreateCardForBattleFieldPlatform(match.CurrentCardsOnBoardP2[i], p2Platforms[i]);
            p2CardNodes.Add(card);
        }

        match.SetCardStatsForMatchForPlayer(match.Player1);
        match.SetCardStatsForMatchForPlayer(match.Player2);
        MatchUtil.UpdateCardStatsListGodot(p1CardNodes, match.CurrentCardsOnBoardP1);
        MatchUtil.UpdateCardStatsListGodot(p2CardNodes, match.CurrentCardsOnBoardP2);

        match.Player1Effects.SetupMatchEffects(match.CurrentCardsOnBoardP1);
        match.Player2Effects.SetupMatchEffects(match.CurrentCardsOnBoardP2);

        MatchUtil.SetupActiveEffectsButtons(P1EffectButtons, player1, match);
    }
}
