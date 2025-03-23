using Godot;
using System;
using static Godot.OpenXRInterface;
using System.Collections.Generic;
using TFTAtHome.util;
using System.Collections;
using TFTAtHome.Backend.models;
using TFTAtHome.Backend.storage;

public partial class PlayerMatchTest : Node
{
    private GridContainer P1Hand;
    private GridContainer P2Hand;
    private Label TotalP1Label;
    private Label TotalP2Label;
    private GridContainer P1Effects;

    private Match match;

    private Card card1 = new Card(1, "LIGHT YAGAMI", "THE FIRST KIRA", "images/cardimages/light-yagami.jpeg", 6, 12, 1, "Genius", 3, true);
    private Card card2 = new Card(2, "ADOLF HITLER", "THE FUHRER", "images/cardimages/adolf-hitler.jpg", 9, 9, 1, "Politician", 4, false);
    private Card card8 = new Card(8, "ELON MUSK", "THE CEO", "images/cardimages/elon-musk.jpg", 5, 7, 6, "Leader", 3, false);
    private Card card13 = new Card(13, "JESUS CHRIST", "THE SON OF GOD", "images/cardimages/jesus-christ.jpg", 12, 1, 6, "Leader", 4, false);
    private Card card3 = new Card(3, "CHUCK NORRIS", "THE TEXAS RANGER", "images/cardimages/chuck-norris.jpg", 7, 8, 3, "TVCelebrity", 3, false);
    private Card card4 = new Card(4, "DEADPOOL", "THE SUPERHERO REJECT", "images/cardimages/deadpool.jpg", 5, 1, 11, "MovieHero", 1, true);
    private Card card7 = new Card(7, "ELIZABETH II", "THE QUEEN OF ENGLAND", "images/cardimages/elizabeth-2.jpg", 3, 7, 7, "Queen", 3, false);
    private Card card9 = new Card(9, "ERIC CARTMAN", "THE FAT KID", "images/cardimages/eric-cartman.jpg", 5, 8, 5, "Drawing", 1, true);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        ColorRect colorRect = GetNode<ColorRect>("ColorRect");
        ColorRect colorRect2 = GetNode<ColorRect>("ColorRect2");
        P1Hand = colorRect.GetNode<GridContainer>("P1Hand");
        P2Hand = colorRect2.GetNode<GridContainer>("P2Hand");
        TotalP1Label = GetNode<Label>("TotalP1Label");
        TotalP2Label = GetNode<Label>("TotalP2Label");
        P1Effects = GetNode<GridContainer>("P1Effects");

        // SetupCartoonTest1(P1Hand, P2Hand);
        SetupActiveTraitTest1(P1Hand, P2Hand);
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
    }


    private void SetupCartoonTest1(GridContainer P1Hand, GridContainer P2Hand)
    {
        Player player1 = new Player(1, "Player 1");
        Player player2 = new Player(2, "Player 2");

        player1.SetPlayerHand(new List<Card> { card9, card4, card1 });
        player2.SetPlayerHand(new List<Card> { card9, card8, card3 });

        match = new Match(player1, player2);

        GD.Print(match);

        // Act: Adding cards to the board
        // Player 1 adds Eric Cartman

        var p1Hand = match.Player1Hand;
        var p2Hand = match.Player2Hand;

        match.AddCardToBoard(match.Player1Hand[0], player1); // Eric Cartman (Drawing): 5, 8, 5, true

        match.AddCardToBoard(match.Player2Hand[0], player2); // Eric Cartman (Drawing): 5, 8, 5, true

        match.AddCardToBoard(match.Player1Hand[0], player1); // Deadpool (MovieHero): 5, 1, 11, true

        match.AddCardToBoard(match.Player2Hand[0], player2); // Elon Musk (Leader): 5, 7, 6, false

        match.AddCardToBoard(match.Player1Hand[0], player1); // Light Yagami (Genius): 6, 12, 1, true

        match.AddCardToBoard(match.Player2Hand[0], player2); // Chuck Norris (TVCelebrity): 7, 8, 3, false

        GD.Print(match.CurrentCardsOnBoardP1);

        // match.SetCardStatsForMatchForPlayer(player1);
        // match.SetCardStatsForMatchForPlayer(player2);

        foreach (Card card in match.CurrentCardsOnBoardP1)
        {
            CardUtil.CreateCardForGameBoardAndAddToContainer(card, P1Hand, 0.6f);
        }

        foreach (Card card in match.CurrentCardsOnBoardP2)
        {
            CardUtil.CreateCardForGameBoardAndAddToContainer(card, P2Hand, 0.6f);
        }

        MatchUtil.SetupActiveEffectsButtons(P1Effects, player1, match);
    }

    private void SetupCartoonTest2(GridContainer P1Hand, GridContainer P2Hand)
    {
        Player player1 = new Player(1, "Player 1");
        Player player2 = new Player(2, "Player 2");

        player1.SetPlayerHand(new List<Card> { card9, card4, card1 });
        player2.SetPlayerHand(new List<Card> { card1, card8, card3 });

        match = new Match(player1, player2);

        // Act: Adding cards to the board
        // Player 1 adds Eric Cartman
        match.AddCardToBoard(match.Player1Hand[0], player1); // Eric Cartman (Drawing): 5, 8, 5, true

        match.AddCardToBoard(match.Player2Hand[0], player2); // Light Yagami (Genius): 5, 8, 5, true

        match.AddCardToBoard(match.Player1Hand[0], player1); // Deadpool (MovieHero): 5, 1, 11, true

        match.AddCardToBoard(match.Player2Hand[0], player2); // Elon Musk (Leader): 5, 7, 6, false

        match.AddCardToBoard(match.Player1Hand[0], player1); // Light Yagami (Genius): 6, 12, 1, true

        match.AddCardToBoard(match.Player2Hand[0], player2); // Chuck Norris (TVCelebrity): 7, 8, 3, false

        GD.Print(match.CurrentCardsOnBoardP1);

        // match.SetCardStatsForMatchForPlayer(player1);
        // match.SetCardStatsForMatchForPlayer(player2);

        foreach (Card card in match.CurrentCardsOnBoardP1)
        {
            CardUtil.CreateCardForGameBoardAndAddToContainer(card, P1Hand, 0.6f);
        }

        foreach (Card card in match.CurrentCardsOnBoardP2)
        {
            CardUtil.CreateCardForGameBoardAndAddToContainer(card, P2Hand, 0.6f);
        }
    }

    private void SetupActiveTraitTest1(GridContainer P1Hand, GridContainer P2Hand)
    {
        Player player1 = new Player(1, "Player 1");
        Player player2 = new Player(2, "Player 2");

        Card elizabeth = LocalStorage.getCardFromName("ELIZABETH II");
        Card redfoo = LocalStorage.getCardFromName("REDFOO");

        player1.SetPlayerHand(new List<Card> { elizabeth, redfoo, card1 });
        player2.SetPlayerHand(new List<Card> { card9, card8, card3 });

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

        GD.Print(match.CurrentCardsOnBoardP1);

        // match.SetCardStatsForMatchForPlayer(player1);
        // match.SetCardStatsForMatchForPlayer(player2);

        foreach (Card card in match.CurrentCardsOnBoardP1)
        {
            CardUtil.CreateCardForGameBoardAndAddToContainer(card, P1Hand, 0.6f);
        }

        foreach (Card card in match.CurrentCardsOnBoardP2)
        {
            CardUtil.CreateCardForGameBoardAndAddToContainer(card, P2Hand, 0.6f);
        }

        MatchUtil.SetupActiveEffectsButtons(P1Effects, player1, match);
    }

    public void SetStatsP1Pressed()
    {
        List<Node2D> list = MatchUtil.GetCardNodesFromContainer(P1Hand);
        GD.Print(list);
        match.SetCardStatsForMatchForPlayer(match.Player1);
        MatchUtil.UpdateCardStatsListGodot(list, match.CurrentCardsOnBoardP1);

        match.Player1Effects.SetupMatchEffects(match.CurrentCardsOnBoardP1);
    }

    public void SetStatsP2Pressed()
    {
        List<Node2D> list = MatchUtil.GetCardNodesFromContainer(P2Hand);
        match.SetCardStatsForMatchForPlayer(match.Player2);
        MatchUtil.UpdateCardStatsListGodot(list, match.CurrentCardsOnBoardP2);

        match.Player2Effects.SetupMatchEffects(match.CurrentCardsOnBoardP2);
    } 
}
