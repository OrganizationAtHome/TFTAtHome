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
    [Export]
    public PackedScene CardPickingScene;
    [Export]
    public PackedScene PreBattleScene;
    [Export]
    public PackedScene PlayerListScene;

    private SceneReferenceSingleton _srs = SceneReferenceSingleton.GetInstance();

    public override void _Ready()
	{
        _srs.PreGameScene = PreGameScene;
        _srs.PlayerListScene = PlayerListScene;
        _srs.PreBattleScene = PreBattleScene;
        // _srs.HomeScreenScene = HomeScreenScene.Instantiate() as HomeScreen;

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

        switchSceneBtn.Connect("pressed", Callable.From(() => SceneUtil.SwitchScene("PreBattleScene", this)));


        homeScreenInstance.AddChild(switchSceneBtn);

		SceneUtil.AddButtonInCorner(switchSceneBtn, Corner.BottomRight);

        rootNode.AddChild(homeScreenInstance);
    }
}
