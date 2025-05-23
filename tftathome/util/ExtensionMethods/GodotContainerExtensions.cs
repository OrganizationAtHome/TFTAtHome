using Godot;

namespace TFTAtHome.util.ExtensionMethods;

public static class GodotContainerExtensions
{
    public static void RemoveAllChildren(this GridContainer gridContainer)
    {
        var children = gridContainer.GetChildren();

        foreach (var child in children)
        {
            gridContainer.RemoveChild(child);
        }
    }
}