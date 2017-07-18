using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AtriLib3.Graphics
{
    public static class Drawing
    {
        public static void DrawRectangle(SpriteBatch spriteBatch, int x, int y, int width, int height, Color c)
        {
            DrawRectangle(spriteBatch, new Rectangle(x, y, width, height), c);
        }

        public static void DrawRectangle(SpriteBatch spriteBatch, Rectangle rect, Color c)
        {
            Pixels.ApplySpriteBatch(spriteBatch);
            for(int x = rect.X; x < rect.X + rect.Width; x++)
            {
                Pixels.PutPixel(x, rect.Y, c);
                Pixels.PutPixel(x, rect.Y + rect.Height, c);
            }

            for(int y = rect.Y; y < rect.Y + rect.Height; y++)
            {
                Pixels.PutPixel(rect.X, y, c);
                Pixels.PutPixel(rect.X + rect.Width, y, c);
            }
        }
    }
}
