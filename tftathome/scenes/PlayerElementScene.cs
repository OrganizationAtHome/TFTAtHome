using Godot;
using System;
using System.Collections.Generic;
using TFTAtHome.models;
using TFTAtHome.storage;
using TFTAtHome.util;

public partial class PlayerElementScene : Node
{
    private ScrollContainer _playerHandContainer;
    private GridContainer _cardGridContainer;
    private Game _currentGame;
    private List<Card> _cards;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
       SetupSceneElements();
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void InitializeData(List<Card> cards)
    {
       _cards = cards;
        AddCardsToPlayerHand();
    }

    public void SetupSceneElements()
    {
        _currentGame = LocalStorage.GetGame();
        AssignContainers();
    }
	public void AssignContainers()
	{
        _playerHandContainer = GetNode("PlayerHandContainer") as ScrollContainer;
        _cardGridContainer = GetNode("CardGridContainer") as GridContainer;
    }

    public void AddCardsToPlayerHand()
    {
        foreach (Card cardObj in _cards)
        {
            CardUtil.CreateCustomCardAndAddToContainer(cardObj.CardName, _cardGridContainer);
        }
    }
}
