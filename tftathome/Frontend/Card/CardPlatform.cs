using Godot;
using System;
using TFTAtHome.Frontend.Card;

public partial class CardPlatform : NicePlatform
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		if (GetParent() is CardHand) {
			AddToGroup("handPlatform");
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
		
    }
}
