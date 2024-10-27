using Godot;
using System;
using TFTAtHome.storage;

public partial class MainScene : Node
{
	[Export]
	public PackedScene HomeScreen;
	
	public override void _Ready()
	{
		SceneReferenceSingleton.GetInstance().HomeScreen = HomeScreen;
		LoadHomeScreen();

		Button switchSceneBtn = new Button();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void LoadHomeScreen()
	{
        Node rootNode = GetTree().Root.GetChild(0);

        Node homeScreenInstance = HomeScreen.Instantiate();
		// Node2D homeScreenRoot = homeScreenInstance.GetChild(0) as Node2D;


        rootNode.AddChild(homeScreenInstance);
    }
}
