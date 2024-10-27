using Godot;
using System;
using TFTAtHome.storage;
using TFTAtHome.util;

public partial class MainScene : Node
{
	[Export]
	public PackedScene HomeScreenScene;
    [Export]
    public PackedScene PreGameScene;
	private SceneReferenceSingleton _srs;

    public override void _Ready()
	{
        _srs = SceneReferenceSingleton.GetInstance();
        _srs.HomeScreenScene = HomeScreenScene;
        _srs.PreGameScene = PreGameScene;
        LoadHomeScreen();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void LoadHomeScreen()
	{
        Node rootNode = GetTree().Root.GetChild(0);

        Node homeScreenInstance = HomeScreenScene.Instantiate();
		
		Button switchSceneBtn = new Button();
		switchSceneBtn.Text = "Switch Scene";

        switchSceneBtn.Connect("pressed", Callable.From(() => SceneUtil.SwitchScene("PreGameScene", homeScreenInstance, rootNode)));


        homeScreenInstance.AddChild(switchSceneBtn);

		SceneUtil.AddButtonInCorner(switchSceneBtn, Corner.BottomRight);

        rootNode.AddChild(homeScreenInstance);
    }
}
