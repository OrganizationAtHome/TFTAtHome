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
        public PackedScene HomeScreenScene { get; set; }
        public PackedScene PreGameScene { get; set; }
        public PackedScene PreBattleScene { get; set; }
        public PackedScene MainScene { get; set; }
        public PackedScene PlayerListScene { get; set; }


        private SceneReferenceSingleton()
        {

        }

        public static SceneReferenceSingleton GetInstance()
        {
            instance ??= new SceneReferenceSingleton();
            return instance;
        }

        public PackedScene GetPhaseSceneByName(string name)
        {
            var property = typeof(SceneReferenceSingleton).GetProperty(name);
            Console.WriteLine(property);
            return property.GetValue(GetInstance()) as PackedScene;
        }

        public PackedScene GetModelSceneByName(string name)
        {
            var property = typeof(SceneReferenceSingleton).GetProperty(name);
            return property?.GetValue(GetInstance()) as PackedScene;
        }
    }
}
