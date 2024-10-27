using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFTAtHome.storage
{
    public sealed class SceneReferenceSingleton
    {
        private static SceneReferenceSingleton instance = null;
        public PackedScene PlayerElementScene { get; set; }
        public PackedScene CardScene { get; set; }
        public HomeScreen HomeScreenScene { get; set; }
        public PreGameScene PreGameScene { get; set; }
        public MainScene MainScene { get; set; }


        private SceneReferenceSingleton()
        {

        }

        public static SceneReferenceSingleton GetInstance()
        {
            instance ??= new SceneReferenceSingleton();
            return instance;
        }

        public Node GetPhaseSceneByName(string name)
        {
            var property = typeof(SceneReferenceSingleton).GetProperty(name);
            GD.Print("GetPhaseSceneByName");
            Node test = property.GetValue(GetInstance()) as Node;
            GD.Print(test);
            return property.GetValue(GetInstance()) as Node;
        }

        public PackedScene GetModelSceneByName(string name)
        {
            var property = typeof(SceneReferenceSingleton).GetProperty(name);
            return property?.GetValue(GetInstance()) as PackedScene;
        }
    }
}
