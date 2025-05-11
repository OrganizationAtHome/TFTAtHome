using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFTAtHome.util {
    internal class MathUtil {

        public static bool IsMouseOverCollisionShape2D(Vector2 position, Rect2 rect, Vector2 globalMousePos) {
            return new Rect2(position, rect.Size).HasPoint(globalMousePos);
        }
    }
}
