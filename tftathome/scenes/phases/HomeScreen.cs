using Godot;
using System;
using TFTAtHome.models;

public partial class HomeScreen : Node2D
{
    // Called when the node enters the scene tree for the first time.

    [Export]
    public TextEdit IPBox { get; set; }
    private GameManager GameManager;
    public override void _Ready()
	{
        this.GameManager = new GameManager(this);
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void _on_join_pressed()
    {
        GameManager.JoinServer();
    }

    public void _on_start_server_pressed()
    {
        GameManager.HostServer();
    }
}
