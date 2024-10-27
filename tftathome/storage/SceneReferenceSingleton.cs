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
        public PackedScene HomeScreen { get; set; }


        private SceneReferenceSingleton()
        {

        }

        public static SceneReferenceSingleton GetInstance()
        {
            instance ??= new SceneReferenceSingleton();
            return instance;
        }
    }
}
