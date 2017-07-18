using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AtriLib3.Interfaces;

namespace AtriLib3.UI
{
    public class Button : Control
    {
        private string _text = "";
        private bool _hover = false;
        private bool _mouseDown = false;
        private float zorderFont = 0f;

        public SpriteFont Font { get; set; }

        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
            }
        }

        public bool CastShadow { get; set; } = false;
        public Vector2 ShadowDistance { get; set; }
        public Color Color { get; set; } = Color.White;
        public Color TextColor { get; set; } = Color.Black;
        public Color TextHoverColor { get; set; } = Color.Black;
        public Color ShadowColor { get; set; } = Color.White;

        public Button(string name, Rectangle buttonRect)
            : base(name, buttonRect)
        {
            SubscribeToEvents();
            ShadowDistance = new Vector2(1, 1);
            Parent_OnParentRectangleChange(this, new ControlEventArgs(this));
        }

        public Button(string name, Control parent, Rectangle buttonRect)
            : base(name, parent, buttonRect)
        {
            SubscribeToEvents();
            ShadowDistance = new Vector2(1, 1);
            Parent_OnParentRectangleChange(this, new ControlEventArgs(this));
        }

        public Button(string name, Control parent, SpriteFont font, Rectangle buttonRect)
            : base(name, parent, buttonRect)
        {
            SubscribeToEvents();
            Font = font;
            ShadowDistance = new Vector2(1, 1);
            Parent_OnParentRectangleChange(this, new ControlEventArgs(this));
        }

        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        private void DrawButton(SpriteBatch spriteBatch)
        {
            if (_hover)
            {
                if (_mouseDown)
                {
                    // Top Left
                    spriteBatch.Draw(UIManager.ActiveInstance.Texture,
                        new Rectangle(Rectangle.X, Rectangle.Y, UIManager.ActiveInstance.ControlGraphicsData.ButtonTopLeft.Width, UIManager.ActiveInstance.ControlGraphicsData.ButtonTopLeft.Height),
                        UIManager.ActiveInstance.ControlGraphicsData.ButtonMouseDownTopLeft,
                        Color, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
                    // Top Right
                    spriteBatch.Draw(UIManager.ActiveInstance.Texture,
                        new Rectangle(Rectangle.X + Rectangle.Width - UIManager.ActiveInstance.ControlGraphicsData.ButtonTopRight.Width, Rectangle.Y, UIManager.ActiveInstance.ControlGraphicsData.ButtonTopRight.Width, UIManager.ActiveInstance.ControlGraphicsData.ButtonTopRight.Height),
                        UIManager.ActiveInstance.ControlGraphicsData.ButtonMouseDownTopRight,
                        Color, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);

                    // Bottom Left
                    spriteBatch.Draw(UIManager.ActiveInstance.Texture,
                        new Rectangle(Rectangle.X, Rectangle.Y + Rectangle.Height - UIManager.ActiveInstance.ControlGraphicsData.ButtonBottomLeft.Height, UIManager.ActiveInstance.ControlGraphicsData.ButtonBottomLeft.Width, UIManager.ActiveInstance.ControlGraphicsData.ButtonBottomLeft.Height),
                        UIManager.ActiveInstance.ControlGraphicsData.ButtonMouseDownBottomLeft,
                        Color, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
                    // Bottom Right
                    spriteBatch.Draw(UIManager.ActiveInstance.Texture,
                        new Rectangle(Rectangle.X + Rectangle.Width - UIManager.ActiveInstance.ControlGraphicsData.ButtonBottomRight.Width, Rectangle.Y + Rectangle.Height - UIManager.ActiveInstance.ControlGraphicsData.ButtonBottomLeft.Height, UIManager.ActiveInstance.ControlGraphicsData.ButtonBottomRight.Width, UIManager.ActiveInstance.ControlGraphicsData.ButtonBottomRight.Height),
                        UIManager.ActiveInstance.ControlGraphicsData.ButtonMouseDownBottomRight,
                        Color, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);

                    // Only do the Fills if size is greater than the set dimensions

                    if (Rectangle.Width > UIManager.ActiveInstance.ControlGraphicsData.WindowDimension)
                    {
                        // Bottom Fill
                        for (int x = 0; x < Rectangle.Width - UIManager.ActiveInstance.ControlGraphicsData.ButtonBottomLeft.Width * 2; x++)
                        {
                            spriteBatch.Draw(UIManager.ActiveInstance.Texture,
                                new Rectangle(Rectangle.X + UIManager.ActiveInstance.ControlGraphicsData.ButtonBottomLeft.Width + x, Rectangle.Y + Rectangle.Height - UIManager.ActiveInstance.ControlGraphicsData.ButtonBottomLeft.Height, UIManager.ActiveInstance.ControlGraphicsData.ButtonBottomFill.Width, UIManager.ActiveInstance.ControlGraphicsData.ButtonBottomFill.Height),
                                UIManager.ActiveInstance.ControlGraphicsData.ButtonMouseDownBottomFill,
                                Color, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
                        }

                        // Top Fill
                        for (int x = 0; x < Rectangle.Width - UIManager.ActiveInstance.ControlGraphicsData.ButtonTopLeft.Width * 2; x++)
                        {
                            spriteBatch.Draw(UIManager.ActiveInstance.Texture,
                                new Rectangle(Rectangle.X + UIManager.ActiveInstance.ControlGraphicsData.ButtonTopLeft.Width + x, Rectangle.Y, UIManager.ActiveInstance.ControlGraphicsData.ButtonTopFill.Width, UIManager.ActiveInstance.ControlGraphicsData.ButtonTopFill.Height),
                                UIManager.ActiveInstance.ControlGraphicsData.ButtonMouseDownTopFill,
                                Color, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
                        }
                    }

                    if (Rectangle.Height > UIManager.ActiveInstance.ControlGraphicsData.WindowDimension)
                    {
                        // Left Fill
                        for (int y = 0; y < Rectangle.Height - UIManager.ActiveInstance.ControlGraphicsData.ButtonBottomLeft.Height * 2; y++)
                        {
                            spriteBatch.Draw(UIManager.ActiveInstance.Texture,
                                new Rectangle(Rectangle.X, Rectangle.Y + UIManager.ActiveInstance.ControlGraphicsData.ButtonBottomLeft.Height + y, UIManager.ActiveInstance.ControlGraphicsData.ButtonLeftFill.Width, UIManager.ActiveInstance.ControlGraphicsData.ButtonLeftFill.Height),
                                UIManager.ActiveInstance.ControlGraphicsData.ButtonMouseDownLeftFill,
                                Color, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
                        }

                        // Right Fill
                        for (int y = 0; y < Rectangle.Height - UIManager.ActiveInstance.ControlGraphicsData.ButtonBottomLeft.Height * 2; y++)
                        {
                            spriteBatch.Draw(UIManager.ActiveInstance.Texture,
                                new Rectangle(Rectangle.X + Rectangle.Width - UIManager.ActiveInstance.ControlGraphicsData.ButtonTopLeft.Width, Rectangle.Y + UIManager.ActiveInstance.ControlGraphicsData.ButtonBottomLeft.Height + y, UIManager.ActiveInstance.ControlGraphicsData.ButtonLeftFill.Width, UIManager.ActiveInstance.ControlGraphicsData.ButtonLeftFill.Height),
                                UIManager.ActiveInstance.ControlGraphicsData.ButtonMouseDownRightFill,
                                Color, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
                        }
                    }

                    // Fill Rectangle
                    spriteBatch.Draw(UIManager.ActiveInstance.Texture,
                        new Rectangle(Rectangle.X + UIManager.ActiveInstance.ControlGraphicsData.ButtonTopLeft.Width, Rectangle.Y + UIManager.ActiveInstance.ControlGraphicsData.ButtonTopLeft.Height, Rectangle.Width - UIManager.ActiveInstance.ControlGraphicsData.ButtonTopLeft.Width * 2, Rectangle.Height - UIManager.ActiveInstance.ControlGraphicsData.ButtonTopLeft.Height * 2),
                        UIManager.ActiveInstance.ControlGraphicsData.ButtonMouseDownBase,
                        Color, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
                }
                else
                {
                    // Top Left
                    spriteBatch.Draw(UIManager.ActiveInstance.Texture,
                        new Rectangle(Rectangle.X, Rectangle.Y, UIManager.ActiveInstance.ControlGraphicsData.ButtonTopLeft.Width, UIManager.ActiveInstance.ControlGraphicsData.ButtonTopLeft.Height),
                        UIManager.ActiveInstance.ControlGraphicsData.ButtonHoverTopLeft,
                        Color, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
                    // Top Right
                    spriteBatch.Draw(UIManager.ActiveInstance.Texture,
                        new Rectangle(Rectangle.X + Rectangle.Width - UIManager.ActiveInstance.ControlGraphicsData.ButtonTopRight.Width, Rectangle.Y, UIManager.ActiveInstance.ControlGraphicsData.ButtonTopRight.Width, UIManager.ActiveInstance.ControlGraphicsData.ButtonTopRight.Height),
                        UIManager.ActiveInstance.ControlGraphicsData.ButtonHoverTopRight,
                        Color, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);

                    // Bottom Left
                    spriteBatch.Draw(UIManager.ActiveInstance.Texture,
                        new Rectangle(Rectangle.X, Rectangle.Y + Rectangle.Height - UIManager.ActiveInstance.ControlGraphicsData.ButtonBottomLeft.Height, UIManager.ActiveInstance.ControlGraphicsData.ButtonBottomLeft.Width, UIManager.ActiveInstance.ControlGraphicsData.ButtonBottomLeft.Height),
                        UIManager.ActiveInstance.ControlGraphicsData.ButtonHoverBottomLeft,
                        Color, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
                    // Bottom Right
                    spriteBatch.Draw(UIManager.ActiveInstance.Texture,
                        new Rectangle(Rectangle.X + Rectangle.Width - UIManager.ActiveInstance.ControlGraphicsData.ButtonBottomRight.Width, Rectangle.Y + Rectangle.Height - UIManager.ActiveInstance.ControlGraphicsData.ButtonBottomLeft.Height, UIManager.ActiveInstance.ControlGraphicsData.ButtonBottomRight.Width, UIManager.ActiveInstance.ControlGraphicsData.ButtonBottomRight.Height),
                        UIManager.ActiveInstance.ControlGraphicsData.ButtonHoverBottomRight,
                        Color, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);

                    // Only do the Fills if size is greater than the set dimensions

                    if (Rectangle.Width > UIManager.ActiveInstance.ControlGraphicsData.WindowDimension)
                    {
                        // Bottom Fill
                        for (int x = 0; x < Rectangle.Width - UIManager.ActiveInstance.ControlGraphicsData.ButtonBottomLeft.Width * 2; x++)
                        {
                            spriteBatch.Draw(UIManager.ActiveInstance.Texture,
                                new Rectangle(Rectangle.X + UIManager.ActiveInstance.ControlGraphicsData.ButtonBottomLeft.Width + x, Rectangle.Y + Rectangle.Height - UIManager.ActiveInstance.ControlGraphicsData.ButtonBottomLeft.Height, UIManager.ActiveInstance.ControlGraphicsData.ButtonBottomFill.Width, UIManager.ActiveInstance.ControlGraphicsData.ButtonBottomFill.Height),
                                UIManager.ActiveInstance.ControlGraphicsData.ButtonHoverBottomFill,
                                Color, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
                        }

                        // Top Fill
                        for (int x = 0; x < Rectangle.Width - UIManager.ActiveInstance.ControlGraphicsData.ButtonTopLeft.Width * 2; x++)
                        {
                            spriteBatch.Draw(UIManager.ActiveInstance.Texture,
                                new Rectangle(Rectangle.X + UIManager.ActiveInstance.ControlGraphicsData.ButtonTopLeft.Width + x, Rectangle.Y, UIManager.ActiveInstance.ControlGraphicsData.ButtonTopFill.Width, UIManager.ActiveInstance.ControlGraphicsData.ButtonTopFill.Height),
                                UIManager.ActiveInstance.ControlGraphicsData.ButtonHoverTopFill,
                                Color, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
                        }
                    }

                    if (Rectangle.Height > UIManager.ActiveInstance.ControlGraphicsData.WindowDimension)
                    {
                        // Left Fill
                        for (int y = 0; y < Rectangle.Height - UIManager.ActiveInstance.ControlGraphicsData.ButtonBottomLeft.Height * 2; y++)
                        {
                            spriteBatch.Draw(UIManager.ActiveInstance.Texture,
                                new Rectangle(Rectangle.X, Rectangle.Y + UIManager.ActiveInstance.ControlGraphicsData.ButtonBottomLeft.Height + y, UIManager.ActiveInstance.ControlGraphicsData.ButtonLeftFill.Width, UIManager.ActiveInstance.ControlGraphicsData.ButtonLeftFill.Height),
                                UIManager.ActiveInstance.ControlGraphicsData.ButtonHoverLeftFill,
                                Color, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
                        }

                        // Right Fill
                        for (int y = 0; y < Rectangle.Height - UIManager.ActiveInstance.ControlGraphicsData.ButtonBottomLeft.Height * 2; y++)
                        {
                            spriteBatch.Draw(UIManager.ActiveInstance.Texture,
                                new Rectangle(Rectangle.X + Rectangle.Width - UIManager.ActiveInstance.ControlGraphicsData.ButtonTopLeft.Width, Rectangle.Y + UIManager.ActiveInstance.ControlGraphicsData.ButtonBottomLeft.Height + y, UIManager.ActiveInstance.ControlGraphicsData.ButtonLeftFill.Width, UIManager.ActiveInstance.ControlGraphicsData.ButtonLeftFill.Height),
                                UIManager.ActiveInstance.ControlGraphicsData.ButtonHoverRightFill,
                                Color, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
                        }
                    }

                    // Fill Rectangle
                    spriteBatch.Draw(UIManager.ActiveInstance.Texture,
                        new Rectangle(Rectangle.X + UIManager.ActiveInstance.ControlGraphicsData.ButtonTopLeft.Width, Rectangle.Y + UIManager.ActiveInstance.ControlGraphicsData.ButtonTopLeft.Height, Rectangle.Width - UIManager.ActiveInstance.ControlGraphicsData.ButtonTopLeft.Width * 2, Rectangle.Height - UIManager.ActiveInstance.ControlGraphicsData.ButtonTopLeft.Height * 2),
                        UIManager.ActiveInstance.ControlGraphicsData.ButtonHoverBase,
                        Color, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
                }
            }
            else
            {
                // Top Left
                spriteBatch.Draw(UIManager.ActiveInstance.Texture,
                    new Rectangle(Rectangle.X, Rectangle.Y, UIManager.ActiveInstance.ControlGraphicsData.ButtonTopLeft.Width, UIManager.ActiveInstance.ControlGraphicsData.ButtonTopLeft.Height),
                    UIManager.ActiveInstance.ControlGraphicsData.ButtonTopLeft,
                    Color, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
                // Top Right
                spriteBatch.Draw(UIManager.ActiveInstance.Texture,
                    new Rectangle(Rectangle.X + Rectangle.Width - UIManager.ActiveInstance.ControlGraphicsData.ButtonTopRight.Width, Rectangle.Y, UIManager.ActiveInstance.ControlGraphicsData.ButtonTopRight.Width, UIManager.ActiveInstance.ControlGraphicsData.ButtonTopRight.Height),
                    UIManager.ActiveInstance.ControlGraphicsData.ButtonTopRight,
                    Color, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);

                // Bottom Left
                spriteBatch.Draw(UIManager.ActiveInstance.Texture,
                    new Rectangle(Rectangle.X, Rectangle.Y + Rectangle.Height - UIManager.ActiveInstance.ControlGraphicsData.ButtonBottomLeft.Height, UIManager.ActiveInstance.ControlGraphicsData.ButtonBottomLeft.Width, UIManager.ActiveInstance.ControlGraphicsData.ButtonBottomLeft.Height),
                    UIManager.ActiveInstance.ControlGraphicsData.ButtonBottomLeft,
                    Color, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
                // Bottom Right
                spriteBatch.Draw(UIManager.ActiveInstance.Texture,
                    new Rectangle(Rectangle.X + Rectangle.Width - UIManager.ActiveInstance.ControlGraphicsData.ButtonBottomRight.Width, Rectangle.Y + Rectangle.Height - UIManager.ActiveInstance.ControlGraphicsData.ButtonBottomLeft.Height, UIManager.ActiveInstance.ControlGraphicsData.ButtonBottomRight.Width, UIManager.ActiveInstance.ControlGraphicsData.ButtonBottomRight.Height),
                    UIManager.ActiveInstance.ControlGraphicsData.ButtonBottomRight,
                    Color, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);

                // Only do the Fills if size is greater than the set dimensions

                if (Rectangle.Width > UIManager.ActiveInstance.ControlGraphicsData.WindowDimension)
                {
                    // Bottom Fill
                    for (int x = 0; x < Rectangle.Width - UIManager.ActiveInstance.ControlGraphicsData.ButtonBottomLeft.Width * 2; x++)
                    {
                        spriteBatch.Draw(UIManager.ActiveInstance.Texture,
                            new Rectangle(Rectangle.X + UIManager.ActiveInstance.ControlGraphicsData.ButtonBottomLeft.Width + x, Rectangle.Y + Rectangle.Height - UIManager.ActiveInstance.ControlGraphicsData.ButtonBottomLeft.Height, UIManager.ActiveInstance.ControlGraphicsData.ButtonBottomFill.Width, UIManager.ActiveInstance.ControlGraphicsData.ButtonBottomFill.Height),
                            UIManager.ActiveInstance.ControlGraphicsData.ButtonBottomFill,
                            Color, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
                    }

                    // Top Fill
                    for (int x = 0; x < Rectangle.Width - UIManager.ActiveInstance.ControlGraphicsData.ButtonTopLeft.Width * 2; x++)
                    {
                        spriteBatch.Draw(UIManager.ActiveInstance.Texture,
                            new Rectangle(Rectangle.X + UIManager.ActiveInstance.ControlGraphicsData.ButtonTopLeft.Width + x, Rectangle.Y, UIManager.ActiveInstance.ControlGraphicsData.ButtonTopFill.Width, UIManager.ActiveInstance.ControlGraphicsData.ButtonTopFill.Height),
                            UIManager.ActiveInstance.ControlGraphicsData.ButtonTopFill,
                            Color, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
                    }
                }

                if (Rectangle.Height > UIManager.ActiveInstance.ControlGraphicsData.WindowDimension)
                {
                    // Left Fill
                    for (int y = 0; y < Rectangle.Height - UIManager.ActiveInstance.ControlGraphicsData.ButtonBottomLeft.Height * 2; y++)
                    {
                        spriteBatch.Draw(UIManager.ActiveInstance.Texture,
                            new Rectangle(Rectangle.X, Rectangle.Y + UIManager.ActiveInstance.ControlGraphicsData.ButtonBottomLeft.Height + y, UIManager.ActiveInstance.ControlGraphicsData.ButtonLeftFill.Width, UIManager.ActiveInstance.ControlGraphicsData.ButtonLeftFill.Height),
                            UIManager.ActiveInstance.ControlGraphicsData.ButtonLeftFill,
                            Color, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
                    }

                    // Right Fill
                    for (int y = 0; y < Rectangle.Height - UIManager.ActiveInstance.ControlGraphicsData.ButtonBottomLeft.Height * 2; y++)
                    {
                        spriteBatch.Draw(UIManager.ActiveInstance.Texture,
                            new Rectangle(Rectangle.X + Rectangle.Width - UIManager.ActiveInstance.ControlGraphicsData.ButtonTopLeft.Width, Rectangle.Y + UIManager.ActiveInstance.ControlGraphicsData.ButtonBottomLeft.Height + y, UIManager.ActiveInstance.ControlGraphicsData.ButtonLeftFill.Width, UIManager.ActiveInstance.ControlGraphicsData.ButtonLeftFill.Height),
                            UIManager.ActiveInstance.ControlGraphicsData.ButtonRightFill,
                            Color, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
                    }
                }

                // Fill Rectangle
                spriteBatch.Draw(UIManager.ActiveInstance.Texture,
                    new Rectangle(Rectangle.X + UIManager.ActiveInstance.ControlGraphicsData.ButtonTopLeft.Width, Rectangle.Y + UIManager.ActiveInstance.ControlGraphicsData.ButtonTopLeft.Height, Rectangle.Width - UIManager.ActiveInstance.ControlGraphicsData.ButtonTopLeft.Width * 2, Rectangle.Height - UIManager.ActiveInstance.ControlGraphicsData.ButtonTopLeft.Height * 2),
                    UIManager.ActiveInstance.ControlGraphicsData.ButtonBase,
                    Color, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
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

            if (Font == null)
            {
                return;
            }

            //spriteBatch.GraphicsDevice.ScissorRectangle = Rectangle;
            DrawButton(spriteBatch);
            DrawFont(spriteBatch);

            base.Draw(spriteBatch);
        }

        private void DrawFont(SpriteBatch spriteBatch)
        {
            Vector2 size = Font.MeasureString(Text);
            zorderFont = ZOrder + 0.01f;

            if (_hover)
            {
                if (_mouseDown)
                {
                    if (CastShadow)
                    {
                        spriteBatch.DrawString(Font, Text, new Vector2(Rectangle.X - 1 - size.X / 2 + Rectangle.Width / 2 + ShadowDistance.X, Rectangle.Y + Rectangle.Height / 2 - size.Y / 2 + ShadowDistance.Y), ShadowColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, zorderFont - 0.001f);
                    }

                    spriteBatch.DrawString(Font, Text, new Vector2(Rectangle.X-1 - size.X / 2 + Rectangle.Width / 2, Rectangle.Y + Rectangle.Height / 2 - size.Y / 2), TextHoverColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, zorderFont);
                }
                else
                {
                    if (CastShadow)
                    {
                        spriteBatch.DrawString(Font, Text, new Vector2(Rectangle.X - size.X / 2 + Rectangle.Width / 2 + ShadowDistance.X, Rectangle.Y + Rectangle.Height / 2 - size.Y / 2 + ShadowDistance.Y), ShadowColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, zorderFont - 0.001f);
                    }

                    spriteBatch.DrawString(Font, Text, new Vector2(Rectangle.X - size.X / 2 + Rectangle.Width / 2, Rectangle.Y + Rectangle.Height / 2 - size.Y / 2), TextHoverColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, zorderFont);
                }
            }
            else
            {
                if (CastShadow)
                {
                    spriteBatch.DrawString(Font, Text, new Vector2(Rectangle.X - size.X / 2 + Rectangle.Width / 2 + ShadowDistance.X, Rectangle.Y + Rectangle.Height / 2 - size.Y / 2 + ShadowDistance.Y), ShadowColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, zorderFont - 0.001f);
                }

                spriteBatch.DrawString(Font, Text, new Vector2(Rectangle.X - size.X / 2 + Rectangle.Width / 2, Rectangle.Y + Rectangle.Height / 2 - size.Y / 2), TextColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, zorderFont);
            }
        }

        protected override void SubscribeToEvents()
        {
            if (Parent != null)
            {
                Parent.OnParentRectangleChange += Parent_OnParentRectangleChange;
                Parent.OnZOrderChanged += Parent_OnZOrderChanged;
            }

            OnMouseEnter += Button_OnMouseEnter;
            OnMouseLeave += Button_OnMouseLeave;
            OnMouseDown += Button_OnMouseDown;
            OnMouseUp += Button_OnMouseUp;
        }

        private void Button_OnMouseUp(object sender, ControlEventArgs e)
        {
            _mouseDown = false;
        }

        private void Button_OnMouseDown(object sender, ControlEventArgs e)
        {
            _mouseDown = true;
        }

        private void Button_OnMouseLeave(object sender, ControlEventArgs e)
        {
            if (Parent == null)
            {
                _hover = false;
            }
            else
            {
                if (Parent is Window)
                {
                    var parent = Parent as Window;
                    if (parent.Active)
                    {
                        _hover = false;
                    }
                }
            }
        }

        private void Button_OnMouseEnter(object sender, ControlEventArgs e)
        {
            if (Parent == null)
            {
                if (_mouseDown == false)
                {
                    _hover = true;
                }
            }
            else
            {
                if (Parent is Window)
                {
                    var parent = Parent as Window;
                    if (parent.Active)
                    {
                        if (_mouseDown == false)
                        {
                            _hover = true;
                        }
                    }
                }
            }
        }

        private void Parent_OnZOrderChanged(object sender, ControlEventArgs e)
        {
            InternalZOrder = e.Control.ZOrder + 0.01f;
        }

        private void Parent_OnParentRectangleChange(object sender, ControlEventArgs e)
        {
            SetPosition();
        }

        protected override void UnsubscribeToEvents()
        {
            if (Parent != null)
            {
                Parent.OnParentRectangleChange -= Parent_OnParentRectangleChange;
                Parent.OnZOrderChanged -= Parent_OnZOrderChanged;
            }

            OnMouseEnter -= Button_OnMouseEnter;
            OnMouseLeave -= Button_OnMouseLeave;
            OnMouseDown -= Button_OnMouseDown;
            OnMouseUp -= Button_OnMouseUp;
        }
    }
}
