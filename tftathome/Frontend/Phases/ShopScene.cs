using Godot;
using System;
using TFTAtHome.Backend.storage;
using TFTAtHome.Frontend.Card;
using TFTAtHome.util;

public partial class ShopScene : Node2D {
	
	[Export]
	private CardHand cost1CardHand;
	[Export]
	private CardHand cost2CardHand;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		foreach (var card in LocalStorage.GetCards()) {
			switch (card.Cost)
			{
				case 1:
					
					cost1CardHand.AddCardToHand(card.CreateGodotCard(1f));
					break;
				case 2:
					var card2 = CardUtil.CreateGodotCard(card, 1f) as NiceCard;
					cost2CardHand.AddCardToHand(card2);
					break;
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
		
	}
}
