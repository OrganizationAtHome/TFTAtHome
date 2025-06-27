using Godot;

namespace TFTAtHome.Frontend.Singletons;

public class PackedScenesSingleton {
    public static PackedScene cardScene = GD.Load<PackedScene>("res://Frontend/Card/FancyCardScene.tscn");
    public static PackedScene platformScene = GD.Load<PackedScene>("res://Frontend/Card/CardPlatformScene.tscn");
}