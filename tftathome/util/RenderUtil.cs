using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFTAtHome.util
{
    /**
     * This class is responsible for all sizing of nodes, resizing etc.
     */
    public static class RenderUtil
    {
        public static Texture2D ResizeTexture(Texture2D texture, float width, float height)
        {
            Image image = texture.GetImage();
            Vector2 newSize = new Vector2(width, height);

            image.Resize((int)newSize.X, (int)newSize.Y, Image.Interpolation.Bilinear);

            ImageTexture resizedTexture = ImageTexture.CreateFromImage(image);

            return resizedTexture;
        }
    }
}
