using Godot;
using System;
using TFTAtHome.models;
using TFTAtHome.storage;
using TFTAtHome.util;

public partial class HomeScreen : Node2D
{
    // Called when the node enters the scene tree for the first time.

    [Export]
    public TextEdit IPBox { get; set; }
    private GameManager _GameManager;
    public override void _Ready()
	{
        this._GameManager = new GameManager(this, 1);
        LocalStorage.SetGameManager(this._GameManager);
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    public static bool ConnectionTest()
    {
        GD.Print("ConnectionTest");
        return true;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}

    public void _on_join_pressed()
    {
        _GameManager.JoinServer(this, this.IPBox.Text);
        ConnectionTest();
    }

    public void _on_start_server_pressed()
    {
        _GameManager.HostServer(this);
        ConnectionTest();
    }

    public void SwitchScene()
    {
        SceneUtil.SwitchScene("PreGameScene", this);
    }
}
