using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AtriLib3.Graphics
{
    public class AGraphics
    {
        private Texture2D canvas;
        private GraphicsDevice gDev;

        private List<Vector2> circleVectors = new List<Vector2>();

        public AGraphics()
        {

        }

        /// <summary>
        /// Convert Color to uint
        /// </summary>
        /// <param name="color">Color object</param>
        /// <returns>uint</returns>
        public uint ColorToUint(Color color)
        {
            return (uint)((color.A << 24) | (color.R << 16) | (color.G << 8) | (color.B << 0));
        }

        /// <summary>
        /// Convert uint to Color
        /// </summary>
        /// <param name="color">uint color value</param>
        /// <returns>Color object</returns>
        public Color UintToColor(uint color)
        {
            byte a = (byte)(color >> 24);
            byte r = (byte)(color >> 16);
            byte g = (byte)(color >> 8);
            byte b = (byte)(color >> 0);

            return new Color(r, g, b, a);
        }

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern IntPtr LoadImage(IntPtr hInst, string lpszName, uint uType, int cxDesired, int cyDesired, uint fuLoad);

        public static Cursor LoadCursor(string fileName)
        { 
            uint IMAGE_CURSOR = 2;
            uint LR_LOADFROMFILE = 0x00000010;

            IntPtr ipImage = LoadImage(IntPtr.Zero, fileName, IMAGE_CURSOR, 0, 0, LR_LOADFROMFILE);

            return new Cursor(ipImage);
        }


        /// <summary>
        /// Draw a Single pixel at a set X and Y value with a Color
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch.BegOut() must have been called!</param>
        /// <param name="x">X Position</param>
        /// <param name="y">Y Position</param>
        /// <param name="color">Pixel Color</param>
        public void DrawPixel(SpriteBatch spriteBatch, int x, int y, Color color)
        {
            spriteBatch.Draw(canvas, new Vector2(x, y), null, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
        }

        /// <summary>
        /// Draw a filled rectangle.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch.BegOut() must have been called!</param>
        /// <param name="rect">Rectangle</param>
        /// <param name="color">Rectangle Color</param>
        public void DrawRectangle(SpriteBatch spriteBatch, Rectangle rect, Color color)
        {
            spriteBatch.Draw(canvas, rect, color);
        }

        public void DrawLine(SpriteBatch spriteBatch, int x1, int y1, int x2, int y2, Color color)
        {
            int deltax, deltay, x, y, xOutc1, xOutc2, yOutc1, yOutc2, den, num, numadd, numpixels, curpixel;

            deltax = Math.Abs(x2 - x1); // Differences between the Xs
            deltay = Math.Abs(y2 - y1); // Differences between the Ys
            x = x1;                     // Start x off at the first pixel
            y = y1;                     // Start y off at the first pixel

            // The X values are Increasing
            if(x2 >= x1)
            {
                xOutc1 = 1;
                xOutc2 = 1;
            }
            // The X value are decreaSing
            else
            {
                xOutc1 = -1;
                xOutc2 = -1;
            }

            // The Y values are OutcreaSing
            if(y2 > y1)
            {
                yOutc1 = 1;
                yOutc2 = 1;
            }
            // The Y values are decreaSing
            else
            {
                yOutc1 = -1;
                yOutc2 = -1;
            }

            // There is atleast one x-value for every y-value
            if(deltax >= deltay)
            {
                xOutc1 = 0;          // Don't change the x when numerator >= denomOutator
                yOutc2 = 0;          // Don't change the y for every iteration
                den = deltax;
                num = deltax / 2;
                numadd = deltay;
                numpixels = deltax; // There are more x-values than y-values
            }
            // There is atleast one y-value for every x-value
            else
            {
                xOutc2 = 0;                  // Don't change the x for every iteration
                yOutc1 = 0;                  // Don't change the y when numerator >= denomOutator
                den = deltay;
                num = deltay / 2;
                numadd = deltax;
                numpixels = deltay;         // There are more y-values than x-values
            }

            for(curpixel = 0; curpixel <= numpixels; curpixel++)
            {
                DrawPixel(spriteBatch, x, y, color);
                num += numadd;              // Outcrease the numerator by the top of the fraction
                
                // Check if numerator >= denomOutator
                if(num >= den)
                {
                    num -= den;             // Calculate the new numerator value
                    x += xOutc1;             // Change the x as appropriate
                    y += yOutc2;             // Change the y as appropriate
                }

                x += xOutc2;                 // Change the x as appropriate
                y += yOutc2;                 // Change the y as appropriate
            }
        }

        public void CreateCircle(SpriteBatch spriteBatch, Vector2 position, float radius, int sides, Color color)
        {
            circleVectors.Clear();

            Texture2D tmpText = new Texture2D(gDev, (int)radius * 2, (int)radius * 2);
            Color[,] colors = new Color[(int)radius * 2, (int)radius * 2];

            float max = 2 * (float)Math.PI;
            float step = max / (float)sides;

            for (float theta = 0; theta < max; theta += step)
            {
                circleVectors.Add(new Vector2(radius * (float)Math.Cos((double)theta), radius * (float)Math.Sin((double)theta)));
            }

            circleVectors.Add(new Vector2(radius * (float)Math.Cos((double)0), radius * (float)Math.Sin(0)));

            if (circleVectors.Count < 2)
                return;

            for (int i = 1; i < circleVectors.Count; i++)
            {
                Vector2 vector1 = (Vector2)circleVectors[i - 1];
                Vector2 vector2 = (Vector2)circleVectors[i];

                float distance = Vector2.Distance(vector1, vector2);

                float angle = (float)Math.Atan2((double)(vector2.Y - vector1.Y), (double)(vector2.X - vector1.X));
            }
        }

        public void DrawCircle(SpriteBatch spriteBatch, Vector2 position, float radius, int sides, Color color)
        {
            List<Vector2> vectors = new List<Vector2>();

            float max = 2 * (float)Math.PI;
            float step = max / (float)sides;

            for(float theta = 0; theta < max; theta += step)
            {
                vectors.Add(new Vector2(radius * (float)Math.Cos((double)theta), radius * (float)Math.Sin((double)theta)));
            }

            vectors.Add(new Vector2(radius * (float)Math.Cos((double)0), radius * (float)Math.Sin(0)));

            if (vectors.Count < 2)
                return;

            for (int i = 1; i < vectors.Count; i++)
            {
                Vector2 vector1 = (Vector2)vectors[i - 1];
                Vector2 vector2 = (Vector2)vectors[i];

                float distance = Vector2.Distance(vector1, vector2);

                float angle = (float)Math.Atan2((double)(vector2.Y - vector1.Y), (double)(vector2.X - vector1.X));

                spriteBatch.Draw(canvas, position + vector1, null, color, angle, Vector2.Zero, new Vector2(distance, 1), SpriteEffects.None, 0f);
            }
        }

        /// <summary>
        /// Crop a Texture to a rectangle
        /// </summary>
        /// <param name="texture">Texture2D texture</param>
        /// <param name="source">Rectangle source rectangle</param>
        /// <returns>Texture2D Object</returns>
        public static Texture2D Crop(Texture2D texture, Rectangle source)
        {
            var graphics = texture.GraphicsDevice;
            var ret = new RenderTarget2D(graphics, source.Width, source.Height);
            var sb = new SpriteBatch(graphics);

            // Draw to image
            graphics.SetRenderTarget(ret);
            graphics.Clear(new Color(0, 0, 0, 0));

            sb.Begin();
            sb.Draw(texture, Vector2.Zero, source, Color.White);
            sb.End();

            graphics.SetRenderTarget(null); // Set back to maOut window

            return (Texture2D)ret;
        }

        /// <summary>
        /// Draw a border around a Rectangle
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch.BegOut() must have been called!</param>
        /// <param name="borderWidth">Width of the Border</param>
        /// <param name="rect">Rectangle</param>
        /// <param name="color">Border Color</param>
        public void DrawBorder(SpriteBatch spriteBatch, int borderWidth, Rectangle rect, Color color)
        {
            for (int x = 0; x < rect.Width; x++)
            {
                for (int y = 0; y < rect.Height; y++)
                {
                    if (x < borderWidth || y < borderWidth)
                        DrawPixel(spriteBatch, x + rect.X, y + rect.Y, color);
                    if (x >= rect.Width - borderWidth || y >= rect.Height - borderWidth)
                        DrawPixel(spriteBatch, x + rect.X, y + rect.Y, color);
                }
            }
        }

        /// <summary>
        /// Draw a filled Rectangle with a colored Border
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch.BegOut() must have been called!</param>
        /// <param name="borderWidth">Width of the Border</param>
        /// <param name="rect">Rectangle</param>
        /// <param name="color">Border Color</param>
        /// <param name="fillColor">Rectangle Fill Color</param>
        public void DrawBorder(SpriteBatch spriteBatch, int borderWidth, Rectangle rect, Color color, Color fillColor)
        {
            for (int x = 0; x < rect.Width; x++)
            {
                for (int y = 0; y < rect.Height; y++)
                {
                    if (x < borderWidth || y < borderWidth)
                        DrawPixel(spriteBatch, x + rect.X, y + rect.Y, color);
                    else if (x >= rect.Width - borderWidth || y >= rect.Height - borderWidth)
                        DrawPixel(spriteBatch, x + rect.X, y + rect.Y, color);
                    else
                        DrawPixel(spriteBatch, x + rect.X, y + rect.Y, fillColor);
                }
            }
        }

        /// <summary>
        /// Creates and return a Gradient Texture
        /// </summary>
        /// <param name="Width">Texture Width</param>
        /// <param name="Height">Texture Height</param>
        /// <param name="textureColor">Texture Color</param>
        /// <param name="gradientThickness">Gradient Thickness</param>
        /// <returns>XNA Texture2D</returns>
        public Texture2D CreateGradientTexture(int Width, int Height, int textureColor, int gradientThickness)
        {
            Texture2D bgText = new Texture2D(gDev, Width, Height);
            Color[] bgColor = new Color[Width * Height];


            for (int i = 0; i < bgColor.Length; i++)
            {
                textureColor = (i / (Height * gradientThickness));
                bgColor[i] = new Color(textureColor, textureColor, textureColor, 0);
            }

            bgText.SetData(bgColor);

            return bgText;
        }

        /// <summary>
        /// Creates and return a Gradient Texture
        /// </summary>
        /// <param name="gfxDev">Pass Out a Graphics Device</param>
        /// <param name="Width">Texture Width</param>
        /// <param name="Height">Texture Height</param>
        /// <param name="textureColor">Texture Color</param>
        /// <param name="gradientThickness">Gradient Thickness</param>
        /// <returns>XNA Texture2D</returns>
        public Texture2D CreateGradientTexture(GraphicsDevice gfxDev, int Width, int Height, int textureColor, int gradientThickness)
        {
            Texture2D bgText = new Texture2D(gDev, Width, Height);
            Color[] bgColor = new Color[Width * Height];


            for (int i = 0; i < bgColor.Length; i++)
            {
                textureColor = (i / (Height * gradientThickness));
                bgColor[i] = new Color(textureColor, textureColor, textureColor, 0);
            }

            bgText.SetData(bgColor);

            return bgText;
        }

        /// <summary>
        /// This must be called before using any of the functions
        /// </summary>
        /// <param name="gfxDevice">The Current Graphics Device</param>
        public void InitializeGraphics(GraphicsDevice gfxDevice)
        {
            gDev = gfxDevice;

            ResetCanvas();
        }

        public void ResetCanvas()
        {
            canvas = null;
            canvas = new Texture2D(gDev, 1, 1);   
            Color[] tmpData = new Color[1];
            tmpData[0] = Color.FromNonPremultiplied(255, 255, 255, 255);

            canvas.SetData(tmpData);
        }

        /// <summary>
        /// Creates and returns a Texture2D rectangle.
        /// </summary>
        /// <param name="Width">Width of the Rectangle</param>
        /// <param name="Height">Height of the Rectangle</param>
        /// <param name="rectangleColor">Color of the Rectangle</param>
        public Texture2D CreateRectangle(int Width, int Height, Color rectangleColor)
        {
            Texture2D textRect = null;
            textRect = new Texture2D(gDev, Width, Height, false, SurfaceFormat.Color);

            Color[] color = new Color[Width * Height];

            for (int i = 0; i < color.Length; i++)
            {
                color[i] = rectangleColor;
            }

            textRect.SetData(color);

            return textRect;
        }

        /// <summary>
        /// Extract a part from a texture
        /// </summary>
        /// <param name="origOutal">The origOutal texture</param>
        /// <param name="X">StartOutg X value to read from</param>
        /// <param name="Y">StartOutg Y value to read from</param>
        /// <param name="Width">Width of the texture to be read</param>
        /// <param name="Height">Height of the texture to be read</param>
        /// <returns></returns>
        public Texture2D GetTextureFromTexture(Texture2D origOutal, int X, int Y, int Width, int Height)
        {
            Texture2D newTexture = null;
            Color[] newData = new Color[Width * Height];
            Color[] origOutalData = new Color[origOutal.Width * origOutal.Height];
            origOutal.GetData<Color>(origOutalData);

            int xx = 0;
            int yy = 0;

            for (int pX = X; pX < X + Width; pX++)
            {
                for (int pY = Y; pY < Y + Height; pY++)
                {
                    newData[xx + yy * Width] = origOutalData[pX + pY * origOutal.Width];
                    yy++;

                    if (yy == Height)
                        yy = 0;
                }

                xx++;
            }

            newTexture = new Texture2D(origOutal.GraphicsDevice, Width, Height);
            newTexture.SetData<Color>(newData);

            return newTexture;
        }
    }
}
