using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using TFTAtHome.Backend.models;
using TFTAtHome.Backend.models.Matches;
using TFTAtHome.Backend.models.Rounds;
using TFTAtHome.Backend.notifiers;
using TFTAtHome.Backend.storage;
using TFTAtHome.Frontend.Card;
using TFTAtHome.util;
using TFTAtHome.util.ExtensionMethods;
using static TFTAtHome.Backend.storage.TraitSingleton;
using static TFTAtHome.Frontend.Singletons.CardNodeNameSingleton;

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
    [Export] public NicePlatform CardPlatform { get; set; }
    [Export]
    public NicePlatform CardPlatform2 { get; set; }
    [Export]
    public NicePlatform CardPlatform3 { get; set; }
    [Export]
    public NicePlatform CardPlatform4 { get; set; }
    [Export]
    public NicePlatform CardPlatform5 { get; set; }
    [Export]
    public NicePlatform CardPlatform6 { get; set; }
    [Export]
    GridContainer P1EffectButtons { get; set; }
    [Export]
    GridContainer P2EffectButtons { get; set; }
    [Export]
    GridContainer ScoreBoard { get; set; }
    [Export]
    Control NextRoundButtonControl { get; set; }

    Label RoundStatusLabel { get; set; }
    [Export]
    public BoxContainer PlayerTotals { get; set; }
    private Match match;
    private List<Node2D> p1CardNodes;
    private List<Node2D> p2CardNodes;
    private string currentEffectTrait;


    public override void _Ready()
	{
        Node root = GetTree().Root.GetChild(0);
        GD.Print(root);
        center1Id = this.GetNode("CardHandMe/CardSpace").GetInstanceId();
        
        Player testPlayer = new Player(1, "Test");

        EffectNotifier.NeedsToUseEffect += OnNeedsToUseEffects;
        EffectNotifier.CardEffectUpdate += HandleCardEffectUpdate;
        EffectNotifier.NeedsToUseGeniusEffect += HandleGeniusEffectUsed;
        DiceNotifier.MustThrowDice += HandleMustThrowDice;
        RoundNotifier.PlayerTotalsFrontend += HandlePlayerTotals;
        RoundNotifier.PlayerPhaseTotalsFrontend += HandlePlayerPhaseTotalsFrontend;
        SetupMusicianTraitTest();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        
    }

    public void zimmer()
    {
        CollisionShape2D center = InstanceFromId(center1Id) as CollisionShape2D;
        var NCplayer = this.GetNode("NC-player1").GetNode("CardSpace") as CollisionShape2D;
        var cardPlatform1 = CardPlatformScene.Instantiate() as Node2D;
        var cardPlatform2 = CardPlatformScene.Instantiate() as Node2D;
        DOSOMETHIKNG(cardPlatform1);
        DOSOMETHIKNG(cardPlatform2);
        platformCount++;

        center.AddChild(cardPlatform1);
        NCplayer.AddChild(cardPlatform2);

        (this.GetNode("CardHandMe") as CardHand).Shuffle();
        (this.GetNode("NC-player1") as CardHand).Shuffle(false);
    }

    private void DOSOMETHIKNG(Node2D cardPlatform)
    {
        var card = CardScene.Instantiate() as Node2D;
        cardPlatform.AddChild(card);
        cardPlatform.Name = "CardPlatform" + platformCount;
        cardPlatform.AddToGroup("handPlatform");
        (cardPlatform.GetNode(CardRoot).GetNode(CardBody) as CardLogic).CardId = platformCount;
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

        List<NicePlatform> p1Platforms = new List<NicePlatform>() { CardPlatform4, CardPlatform5, CardPlatform6 };
        List<NicePlatform> p2Platforms = new List<NicePlatform>() { CardPlatform, CardPlatform2, CardPlatform3 };

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
        MatchUtil.UpdateCardStatsListGodot(p1CardNodes, match.CurrentCardsOnBoardP1);
        MatchUtil.UpdateCardStatsListGodot(p2CardNodes, match.CurrentCardsOnBoardP2);

        MatchUtil.SetupActiveEffectsButtons(P1EffectButtons, player1, match);
    }
    
    public void SetupMusicianTraitTest()
    {
        Player player1 = new Player(1, "Player 1");
        Player player2 = new Player(2, "Player 2");

        List<NicePlatform> p1Platforms = new List<NicePlatform>() { CardPlatform4, CardPlatform5, CardPlatform6 };
        List<NicePlatform> p2Platforms = new List<NicePlatform>() { CardPlatform, CardPlatform2, CardPlatform3 };

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
        
        MatchUtil.UpdateCardStatsListGodot(p1CardNodes, match.CurrentCardsOnBoardP1);
        MatchUtil.UpdateCardStatsListGodot(p2CardNodes, match.CurrentCardsOnBoardP2);

        MatchUtil.SetupActiveEffectsButtons(P1EffectButtons, player1, match);
    }
    
     public void SetupRandomTest()
    {
        Player player1 = new Player(1, "Player 1");
        Player player2 = new Player(2, "Player 2");

        List<NicePlatform> p1Platforms = new List<NicePlatform>() { CardPlatform4, CardPlatform5, CardPlatform6 };
        List<NicePlatform> p2Platforms = new List<NicePlatform>() { CardPlatform, CardPlatform2, CardPlatform3 };

        Card card1 = LocalStorage.GetRandomCardByTrait(Queen);
        Card card2 = LocalStorage.GetRandomCardByTrait(TVCelebrity);
        Card card3 = LocalStorage.GetRandomCardByTrait(TVCelebrity);

        Card card4 = LocalStorage.GetRandomCardByTrait(Genius); // 5, 8, 5, "Drawing", true
        Card card5 = LocalStorage.GetRandomCardByTrait(TVCelebrity); // 5, 7, 6, "Leader", false
        Card card6 = LocalStorage.GetCardFromName("DEADPOOL"); // 5, 1, 11, "MovieHero", true


        player1.SetPlayerHand(new List<Card> { card1, card2, card3 });
        player2.SetPlayerHand(new List<Card> { card4, card5, card6 });

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
        
        MatchUtil.UpdateCardStatsListGodot(p1CardNodes, match.CurrentCardsOnBoardP1);
        MatchUtil.UpdateCardStatsListGodot(p2CardNodes, match.CurrentCardsOnBoardP2);

        MatchUtil.SetupActiveEffectsButtons(P1EffectButtons, player1, match);
    }

    private void OnNeedsToUseEffects(Player player)
    {
        HighlightCards(match.CurrentCardsOnBoardP2, true);
    }
    
    private void HandleGeniusEffectUsed(Player player)
    {
        var currentMatchEffect = match.CurrentRound as EffectRound;
        if (currentMatchEffect == null)
        {
            throw new Exception("I am not an EffectRound dumbass HandleGeniusEffectUsed");
        }

        MatchUtil.SetupSelectPhaseButtons(P1EffectButtons, player, match);
    }

    private void HandleCardEffectUpdate(Player player)
    {
        if (currentEffectTrait.IsTraitWithPlayerInputRequirement())
        {
            if (currentEffectTrait == Genius)
            {
                HighlightCards(match.CurrentCardsOnBoardP1, false);
                MatchUtil.UpdateCardStatsListGodot(p1CardNodes, match.CurrentCardsOnBoardP1);
                MatchUtil.UpdateCardStatsListGodot(p2CardNodes, match.CurrentCardsOnBoardP2);
                if (player == match.Player1)
                {
                    MatchUtil.SetupActiveEffectsButtons(P1EffectButtons, player, match);
                }
                else
                {
                    MatchUtil.SetupActiveEffectsButtons(P2EffectButtons, player, match);
                }
            }
        }
        else
        {
            HighlightCards(match.CurrentCardsOnBoardP1, false);
            MatchUtil.UpdateCardStatsListGodot(p1CardNodes, match.CurrentCardsOnBoardP1);
            MatchUtil.UpdateCardStatsListGodot(p2CardNodes, match.CurrentCardsOnBoardP2);
            if (player == match.Player1)
            {
                MatchUtil.SetupActiveEffectsButtons(P1EffectButtons, player, match);
            }
            else
            {
                MatchUtil.SetupActiveEffectsButtons(P2EffectButtons, player, match);
            }
        }
    }

    private void HandleMustThrowDice(Player player)
    {
        if (player == match.Player1)
        {
            MatchUtil.SetupDiceRollButton(P1EffectButtons, player, match);
        }
        else
        {
            MatchUtil.SetupDiceRollButton(P2EffectButtons, player, match);
        }
    }
    private void HandlePlayerTotals(int[] totals)
    {
        var children = PlayerTotals.GetChildren();
        foreach (var child in children)
        {
            PlayerTotals.RemoveChild(child);
        }
        Label p1Total = new Label();
        Label p2Total = new Label();
        
        p1Total.Text = "Player1: " + totals[0].ToString();
        p2Total.Text = "Player2: " + totals[1].ToString();
        
        PlayerTotals.AddChild(p1Total);
        PlayerTotals.AddChild(p2Total);
    }
    
    /* Add a list of cards as input for future active traits */
    public void HighlightCards(List<Card> cards, bool active)
    {
        var p1enumerator = p2CardNodes.GetEnumerator();
        while (p1enumerator.MoveNext())
        {
            var cardNode = p1enumerator.Current as NiceCard;

            var cardBody = cardNode.GetChildren()[0];
            if (cardBody == null) throw new Exception("Your coding skills are terrible, cardBody in HighlightCards is null");
            CardLogic cardLogic = cardBody as CardLogic;
            if (cardLogic == null) throw new Exception("Your coding skills are terrible, Cardlogic in HighlightCards is null");
            cardLogic.IsEffectAble = active;

            Card card = CardUtil.GetCardModelFromCardNode(cardNode);
            if (!cards.Contains(card))
            {
                p1CardNodes.Remove(cardNode);
            }
        }
        
        var p2enumerator = p1CardNodes.GetEnumerator();
        while (p2enumerator.MoveNext())
        {
            var cardNode = p2enumerator.Current as NiceCard;
            
            var cardBody = cardNode.GetChildren()[0];
            CardLogic cardLogic = cardBody as CardLogic;
            if (cardLogic == null) throw new Exception("Your coding skills are terrible, Cardlogic is null");
            cardLogic.IsEffectAble = active;

            Card card = CardUtil.GetCardModelFromCardNode(cardNode);
            if (!cards.Contains(card))
            {
                p2CardNodes.Remove(cardNode);
            }
        }

        MatchUtil.HighLightEffectableCards(p1CardNodes, active);
        MatchUtil.HighLightEffectableCards(p2CardNodes, active);
    }

    public void HandlePlayerPhaseTotalsFrontend(string[] values)
    {
        Label phase = new Label();
        Label round = new Label();
        Label winner = new Label();
        
        phase.Text = values[0];
        round.Text = values[1];
        winner.Text = values[2];
        
        ScoreBoard.AddChild(phase);
        ScoreBoard.AddChild(round);
        ScoreBoard.AddChild(winner);
        
        MatchUtil.SetupNextRoundButton(P1EffectButtons, P2EffectButtons, NextRoundButtonControl, match);
    }
}
