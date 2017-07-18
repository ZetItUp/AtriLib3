using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AtriLib3.Graphics
{
    public static class Pixels
    {
        static bool init = false;
        static GraphicsDevice gfxDev;
        static SpriteBatch spriteBatch;
        static Texture2D pixel;
        static Color[] color;

        static Pixels()
        {

        }

        public static void Initialize(GraphicsDevice gDevice, SpriteBatch sBatch)
        {
            gfxDev = gDevice;
            spriteBatch = sBatch;

            pixel = new Texture2D(gfxDev, 1, 1);
            color = new Color[1];
            color[0] = Color.White;
            pixel.SetData(color);

            init = true;
        }

        public static void ApplySpriteBatch(SpriteBatch sBatch)
        {
            spriteBatch = sBatch;
        }

        public static void PutPixel(int x, int y, Color c)
        {
            if(!init)
            {
                return;
            }

            spriteBatch.Draw(pixel, new Vector2(x, y), null, c, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1.0f);
        }
    }
}
