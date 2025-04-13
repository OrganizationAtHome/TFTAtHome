using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using TFTAtHome.Backend.models;
using TFTAtHome.Backend.storage;
using TFTAtHome.util;

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
    public static ulong PreBattleSceneId;



    public override void _Ready()
	{
        PreBattleSceneId = this.GetInstanceId();
        Node root = GetTree().Root.GetChild(0);
        GD.Print(root);
        center1Id = this.GetNode("CardHandMe/CardSpace").GetInstanceId();
        
        Player testPlayer = new Player(1, "Test");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        
    }

    public void zimmer()
    {
        CollisionShape2D center = InstanceFromId(center1Id) as CollisionShape2D;
        var cardPlatform = CardPlatformScene.Instantiate() as Node2D;
        var card = CardScene.Instantiate() as Node2D;
        cardPlatform.AddChild(card);
        cardPlatform.Name = "CardPlatform" + platformCount++;
        cardPlatform.AddToGroup("handPlatform");

        CardLogic yeet = card as CardLogic;
        
        center.AddChild(cardPlatform);

        (this.GetNode("CardHandMe") as CardHand).Shuffle();
    }

    public void printRecursive(Node node)
    {
        GD.Print(node.Name);
        foreach (Node child in node.GetChildren())
        {
            printRecursive(child);
        }
    }
}
