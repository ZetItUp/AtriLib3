using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AtriLib3.Utility;

namespace AtriLib3.UI
{
    public class Window : Control
    {
        public bool DrawTitleBar { get; set; } = true;
        public bool DrawBorders { get; set; } = true;
        public Color WindowTint { get; set; }
        public bool Active { get; set; }
        public bool LockToScreen { get; set; } = false;

        public Window(string name, Rectangle windowRect)
            : base(name, windowRect)
        {
            WindowTint = Color.White;
            Active = false;

            SubscribeToEvents();
        }

        Vector2 mouseDifference;

        private void Window_OnMouseDown(object sender, ControlEventArgs e)
        {
            if(!CanMove || !Active)
            {
                return;
            }

            Rectangle = new Rectangle((int)(AMouse.MousePosition.X - mouseDifference.X), (int)(AMouse.MousePosition.Y - mouseDifference.Y), Rectangle.Width, Rectangle.Height);

            if (LockToScreen)
            {
                Rectangle screenRect = UIManager.ActiveInstance.Monitor.ScreenRectangle;

                if (Rectangle.X < screenRect.X)
                {
                    Rectangle = new Rectangle(screenRect.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
                }

                if (Rectangle.Y < screenRect.Y)
                {
                    Rectangle = new Rectangle(Rectangle.X, screenRect.Y, Rectangle.Width, Rectangle.Height);
                }

                if (Rectangle.X > screenRect.X + screenRect.Width - Rectangle.Width)
                {
                    Rectangle = new Rectangle(screenRect.X + screenRect.Width - Rectangle.Width, Rectangle.Y, Rectangle.Width, Rectangle.Height);
                }

                if (Rectangle.Y > screenRect.Y + screenRect.Height - Rectangle.Height)
                {
                    Rectangle = new Rectangle(Rectangle.X, screenRect.Y + screenRect.Height - Rectangle.Height, Rectangle.Width, Rectangle.Height);
                }
            }

            SetPosition();
        }

        private void Window_OnZOrderChanged(object sender, ControlEventArgs e)
        {
            var controls = GetChildren<Control>();

            for (int i = 0; i < controls.Count(); i++)
            {
                controls[i].InternalZOrder = ZOrder + 0.01f;
            }
        }

        private void Window_OnClicked(object sender, ControlEventArgs e)
        {
            List<Window> windows = UIManager.ActiveInstance.GetControls<Window>();

            Rectangle mouseRect = AMouse.MouseRectangle;

            bool isTop = true;

            // Check if we are the top one of the intersecting windows
            for (int i = 0; i < windows.Count(); i++)
            {
                if (this != windows[i] && mouseRect.Intersects(windows[i].Rectangle))
                {
                    if (windows[i].ZOrder >= ZOrder)
                    {
                        isTop = false;
                        break;
                    }
                }
            }

            if (isTop && ZOrder != 0.9f)
            {
                ZOrder = 0.9f;
                Active = true;

                for (int i = 0; i < windows.Count(); i++)
                {
                    if (this != windows[i])
                    {
                        windows[i].Active = false;
                        windows[i].ZOrder = ZOrder - 0.2f;
                    }
                }
            }
        }

        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);
        }

        public override void Update(GameTime gameTime)
        {
            if (Active)
            {
                if (AMouse.MouseRectangle.Intersects(Rectangle))
                {
                    if (AMouse.MousePressed(AMouse.MouseButton.Left))
                    {
                        mouseDifference = new Vector2(AMouse.MousePosition.X - Rectangle.X, AMouse.MousePosition.Y - Rectangle.Y);
                    }
                }
            }

            base.Update(gameTime);
        }

        private void drawTitleBar(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(UIManager.ActiveInstance.Texture, new Rectangle(Rectangle.X, Rectangle.Y - UIManager.ActiveInstance.ControlGraphicsData.TitleBarFill.Height, UIManager.ActiveInstance.ControlGraphicsData.WindowDimension, UIManager.ActiveInstance.ControlGraphicsData.WindowDimension), UIManager.ActiveInstance.ControlGraphicsData.TitleBarTopLeft, WindowTint, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
            spriteBatch.Draw(UIManager.ActiveInstance.Texture, new Rectangle(Rectangle.X + Rectangle.Width - UIManager.ActiveInstance.ControlGraphicsData.WindowDimension, Rectangle.Y - UIManager.ActiveInstance.ControlGraphicsData.TitleBarFill.Height, UIManager.ActiveInstance.ControlGraphicsData.WindowDimension, UIManager.ActiveInstance.ControlGraphicsData.WindowDimension), UIManager.ActiveInstance.ControlGraphicsData.TitleBarTopRight, WindowTint, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
            spriteBatch.Draw(UIManager.ActiveInstance.Texture, new Rectangle(Rectangle.X, Rectangle.Y - UIManager.ActiveInstance.ControlGraphicsData.TitleBarFill.Height + UIManager.ActiveInstance.ControlGraphicsData.WindowDimension, UIManager.ActiveInstance.ControlGraphicsData.WindowDimension, UIManager.ActiveInstance.ControlGraphicsData.WindowDimension), UIManager.ActiveInstance.ControlGraphicsData.TitleBarBottomLeft, WindowTint, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
            spriteBatch.Draw(UIManager.ActiveInstance.Texture, new Rectangle(Rectangle.X + Rectangle.Width - UIManager.ActiveInstance.ControlGraphicsData.WindowDimension, Rectangle.Y - UIManager.ActiveInstance.ControlGraphicsData.TitleBarFill.Height + UIManager.ActiveInstance.ControlGraphicsData.WindowDimension, UIManager.ActiveInstance.ControlGraphicsData.WindowDimension, UIManager.ActiveInstance.ControlGraphicsData.WindowDimension), UIManager.ActiveInstance.ControlGraphicsData.TitleBarBottomRight, WindowTint, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);

            int width = (Rectangle.Width - UIManager.ActiveInstance.ControlGraphicsData.TitleBarFill.Width * 4);
            for (int x = 0; x < width / UIManager.ActiveInstance.ControlGraphicsData.TitleBarFill.Width; x++)
            {
                spriteBatch.Draw(UIManager.ActiveInstance.Texture, new Rectangle(Rectangle.X + UIManager.ActiveInstance.ControlGraphicsData.WindowDimension + UIManager.ActiveInstance.ControlGraphicsData.TitleBarFill.Width * x, Rectangle.Y - UIManager.ActiveInstance.ControlGraphicsData.TitleBarFill.Height, UIManager.ActiveInstance.ControlGraphicsData.TitleBarFill.Width, UIManager.ActiveInstance.ControlGraphicsData.TitleBarFill.Height), UIManager.ActiveInstance.ControlGraphicsData.TitleBarFill, WindowTint, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
            }

            if (width % UIManager.ActiveInstance.ControlGraphicsData.TitleBarFill.Width != 0)
            {
                int rema = width % UIManager.ActiveInstance.ControlGraphicsData.TitleBarFill.Width;
                spriteBatch.Draw(UIManager.ActiveInstance.Texture, new Rectangle(Rectangle.X + Rectangle.Width - UIManager.ActiveInstance.ControlGraphicsData.WindowDimension - rema, Rectangle.Y - UIManager.ActiveInstance.ControlGraphicsData.TitleBarFill.Height, rema, UIManager.ActiveInstance.ControlGraphicsData.TitleBarFill.Height), UIManager.ActiveInstance.ControlGraphicsData.TitleBarFill, WindowTint, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
            }
        }

        private void drawBorders(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(UIManager.ActiveInstance.Texture, new Rectangle(Rectangle.X, Rectangle.Y, UIManager.ActiveInstance.ControlGraphicsData.WindowDimension, UIManager.ActiveInstance.ControlGraphicsData.WindowDimension), UIManager.ActiveInstance.ControlGraphicsData.WindowBorderTopLeft, WindowTint, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
            spriteBatch.Draw(UIManager.ActiveInstance.Texture, new Rectangle(Rectangle.X + Rectangle.Width - UIManager.ActiveInstance.ControlGraphicsData.WindowDimension, Rectangle.Y, UIManager.ActiveInstance.ControlGraphicsData.WindowDimension, UIManager.ActiveInstance.ControlGraphicsData.WindowDimension), UIManager.ActiveInstance.ControlGraphicsData.WindowBorderTopRight, WindowTint, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
            spriteBatch.Draw(UIManager.ActiveInstance.Texture, new Rectangle(Rectangle.X, Rectangle.Y + Rectangle.Height - UIManager.ActiveInstance.ControlGraphicsData.WindowBorderBottomLeft.Height, UIManager.ActiveInstance.ControlGraphicsData.WindowDimension, UIManager.ActiveInstance.ControlGraphicsData.WindowDimension), UIManager.ActiveInstance.ControlGraphicsData.WindowBorderBottomLeft, WindowTint, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
            spriteBatch.Draw(UIManager.ActiveInstance.Texture, new Rectangle(Rectangle.X + Rectangle.Width - UIManager.ActiveInstance.ControlGraphicsData.WindowDimension, Rectangle.Y + Rectangle.Height - UIManager.ActiveInstance.ControlGraphicsData.WindowBorderBottomRight.Height, UIManager.ActiveInstance.ControlGraphicsData.WindowDimension, UIManager.ActiveInstance.ControlGraphicsData.WindowDimension), UIManager.ActiveInstance.ControlGraphicsData.WindowBorderBottomRight, WindowTint, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);

            // Draw Top And Bottom Borders
            int width = (Rectangle.Width - UIManager.ActiveInstance.ControlGraphicsData.WindowDimension * 2);

            for (int x = 0; x < width / UIManager.ActiveInstance.ControlGraphicsData.WindowBorderTopFill.Width; x++)
            {
                spriteBatch.Draw(UIManager.ActiveInstance.Texture, new Rectangle(Rectangle.X + UIManager.ActiveInstance.ControlGraphicsData.WindowDimension + UIManager.ActiveInstance.ControlGraphicsData.WindowBorderTopFill.Width * x, Rectangle.Y, UIManager.ActiveInstance.ControlGraphicsData.WindowBorderTopFill.Width, UIManager.ActiveInstance.ControlGraphicsData.WindowBorderTopFill.Height), UIManager.ActiveInstance.ControlGraphicsData.WindowBorderTopFill, WindowTint, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
                spriteBatch.Draw(UIManager.ActiveInstance.Texture, new Rectangle(Rectangle.X + UIManager.ActiveInstance.ControlGraphicsData.WindowDimension + UIManager.ActiveInstance.ControlGraphicsData.WindowBorderBottomFill.Width * x, Rectangle.Y + Rectangle.Height - UIManager.ActiveInstance.ControlGraphicsData.WindowBorderBottomFill.Height, UIManager.ActiveInstance.ControlGraphicsData.WindowBorderBottomFill.Width, UIManager.ActiveInstance.ControlGraphicsData.WindowBorderBottomFill.Height), UIManager.ActiveInstance.ControlGraphicsData.WindowBorderBottomFill, WindowTint, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
            }

            if (width % UIManager.ActiveInstance.ControlGraphicsData.WindowBorderBottomFill.Width != 0)
            {
                int rema = width % UIManager.ActiveInstance.ControlGraphicsData.WindowBorderTopFill.Width;
                spriteBatch.Draw(UIManager.ActiveInstance.Texture, new Rectangle(Rectangle.X + Rectangle.Width - UIManager.ActiveInstance.ControlGraphicsData.WindowDimension - rema, Rectangle.Y, rema, UIManager.ActiveInstance.ControlGraphicsData.WindowBorderTopFill.Height), UIManager.ActiveInstance.ControlGraphicsData.WindowBorderTopFill, WindowTint, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
                spriteBatch.Draw(UIManager.ActiveInstance.Texture, new Rectangle(Rectangle.X + Rectangle.Width - UIManager.ActiveInstance.ControlGraphicsData.WindowDimension - rema, Rectangle.Y + Rectangle.Height - UIManager.ActiveInstance.ControlGraphicsData.WindowBorderBottomFill.Height, rema, UIManager.ActiveInstance.ControlGraphicsData.WindowBorderBottomFill.Height), UIManager.ActiveInstance.ControlGraphicsData.WindowBorderBottomFill, WindowTint, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
            }

            // Draw Left and Right Borders
            int height = (Rectangle.Height - UIManager.ActiveInstance.ControlGraphicsData.WindowDimension * 2);

            for (int y = 0; y < height / UIManager.ActiveInstance.ControlGraphicsData.WindowBorderLeftFill.Height; y++)
            {
                spriteBatch.Draw(UIManager.ActiveInstance.Texture, new Rectangle(Rectangle.X, Rectangle.Y + UIManager.ActiveInstance.ControlGraphicsData.WindowDimension + UIManager.ActiveInstance.ControlGraphicsData.WindowBorderLeftFill.Height * y, UIManager.ActiveInstance.ControlGraphicsData.WindowBorderLeftFill.Width, UIManager.ActiveInstance.ControlGraphicsData.WindowBorderLeftFill.Height), UIManager.ActiveInstance.ControlGraphicsData.WindowBorderLeftFill, WindowTint, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
                spriteBatch.Draw(UIManager.ActiveInstance.Texture, new Rectangle(Rectangle.X + Rectangle.Width - UIManager.ActiveInstance.ControlGraphicsData.WindowDimension, Rectangle.Y + UIManager.ActiveInstance.ControlGraphicsData.WindowDimension + UIManager.ActiveInstance.ControlGraphicsData.WindowBorderRightFill.Height * y, UIManager.ActiveInstance.ControlGraphicsData.WindowBorderRightFill.Width, UIManager.ActiveInstance.ControlGraphicsData.WindowBorderRightFill.Height), UIManager.ActiveInstance.ControlGraphicsData.WindowBorderRightFill, WindowTint, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
            }

            if (height % UIManager.ActiveInstance.ControlGraphicsData.WindowBorderLeftFill.Height != 0)
            {
                int rema = height % UIManager.ActiveInstance.ControlGraphicsData.WindowBorderLeftFill.Height;
                spriteBatch.Draw(UIManager.ActiveInstance.Texture, new Rectangle(Rectangle.X, Rectangle.Y + Rectangle.Height - UIManager.ActiveInstance.ControlGraphicsData.WindowDimension - rema, UIManager.ActiveInstance.ControlGraphicsData.WindowBorderLeftFill.Width, rema), UIManager.ActiveInstance.ControlGraphicsData.WindowBorderLeftFill, WindowTint, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
                spriteBatch.Draw(UIManager.ActiveInstance.Texture, new Rectangle(Rectangle.X + Rectangle.Width - UIManager.ActiveInstance.ControlGraphicsData.WindowDimension, Rectangle.Y + Rectangle.Height - UIManager.ActiveInstance.ControlGraphicsData.WindowDimension - rema, UIManager.ActiveInstance.ControlGraphicsData.WindowBorderRightFill.Width, rema), UIManager.ActiveInstance.ControlGraphicsData.WindowBorderRightFill, WindowTint, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
            }
        }

        private void drawWindow(SpriteBatch spriteBatch)
        {
            if (DrawBorders)
            {
                spriteBatch.Draw(UIManager.ActiveInstance.Texture, new Rectangle(Rectangle.X + UIManager.ActiveInstance.ControlGraphicsData.WindowBorderLeftFill.Width, Rectangle.Y + UIManager.ActiveInstance.ControlGraphicsData.WindowBorderTopFill.Height, Rectangle.Width - UIManager.ActiveInstance.ControlGraphicsData.WindowDimension * 2, Rectangle.Height - UIManager.ActiveInstance.ControlGraphicsData.WindowDimension * 2), UIManager.ActiveInstance.ControlGraphicsData.WindowBase, WindowTint, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
            }
            else
            {
                spriteBatch.Draw(UIManager.ActiveInstance.Texture, new Rectangle(Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height), UIManager.ActiveInstance.ControlGraphicsData.WindowBase, WindowTint, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (UIManager.ActiveInstance.ControlGraphicsData == null)
            {
                return;
            }

            if (!Visible)
            {
                return;
            }

            //spriteBatch.End();
            //spriteBatch.Begin(SpriteSortMode.FrontToBack, null, null, null, new RasterizerState { ScissorTestEnable = true });

            //if (!DrawTitleBar)
            //{
            //    spriteBatch.GraphicsDevice.ScissorRectangle = Rectangle;
            //}
            //else
            //{
            //    spriteBatch.GraphicsDevice.ScissorRectangle = new Rectangle(Rectangle.X, Rectangle.Y - UIManager.ActiveInstance.ControlGraphicsData.WindowDimension * 2, Rectangle.Width, Rectangle.Height + UIManager.ActiveInstance.ControlGraphicsData.WindowDimension * 2);
            //}

            drawWindow(spriteBatch);

            if (DrawBorders)
            {
                drawBorders(spriteBatch);
            }

            if (DrawTitleBar)
            {
                drawTitleBar(spriteBatch);
            }

            //spriteBatch.End();
            //spriteBatch.Begin(SpriteSortMode.FrontToBack);

            base.Draw(spriteBatch);
        }

        protected override void SubscribeToEvents()
        {
            OnMouseClicked += Window_OnClicked;
            OnZOrderChanged += Window_OnZOrderChanged;
            OnMouseUp += Window_OnMouseUp;
            OnMouseDown += Window_OnMouseDown;
        }

        private void Window_OnMouseUp(object sender, ControlEventArgs e)
        {
            CanMove = true;
        }

        protected override void UnsubscribeToEvents()
        {
            OnMouseClicked -= Window_OnClicked;
            OnZOrderChanged -= Window_OnZOrderChanged;
            OnMouseDown -= Window_OnMouseDown;
            OnMouseUp -= Window_OnMouseUp;
        }
    }
}
