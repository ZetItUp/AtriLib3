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
    public class Label : Control
    {
        public SpriteFont Font { get; set; }
        private string _text = "";
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;

                if(AutoSize)
                {
                    Vector2 size = Font.MeasureString(_text);
                    _rect = new Rectangle(Rectangle.X, Rectangle.Y, (int)size.X, (int)size.Y);
                }
            }
        }
        public bool CastShadow { get; set; } = false;
        public Vector2 ShadowDistance { get; set; }
        public Color Color { get; set; } = Color.White;
        public Color ShadowColor { get; set; } = Color.Black;
        public bool AutoSize { get; set; } = true;

        public Label(string name, Rectangle labelRectangle)
            : base(name, labelRectangle)
        {
            SubscribeToEvents();
            ShadowDistance = new Vector2(1, 1);
            Parent_OnParentRectangleChange(this, new ControlEventArgs(this));
        }

        public Label(string name, Control parent, Rectangle labelRectangle)
            : base(name, parent, labelRectangle)
        {
            SubscribeToEvents();
            ShadowDistance = new Vector2(1, 1);
            Parent_OnParentRectangleChange(this, new ControlEventArgs(this));
        }

        public Label(string name, Control parent, SpriteFont font, Rectangle labelRectangle)
            : base(name, parent, labelRectangle)
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

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible)
            {
                return;
            }

            if(Font == null)
            {
                return;
            }

            //spriteBatch.GraphicsDevice.ScissorRectangle = Rectangle;

            if (CastShadow)
            {
                spriteBatch.DrawString(Font, Text, new Vector2(Rectangle.X + ShadowDistance.X, Rectangle.Y + ShadowDistance.Y), ShadowColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, ZOrder - 0.01f);
            }

            spriteBatch.DrawString(Font, Text, new Vector2(Rectangle.X, Rectangle.Y), Color, 0f, Vector2.Zero, 1f, SpriteEffects.None, ZOrder);

            base.Draw(spriteBatch);
        }

        protected override void SubscribeToEvents()
        {
            if (Parent != null)
            {
                Parent.OnZOrderChanged += Parent_OnZOrderChanged;
                Parent.OnParentRectangleChange += Parent_OnParentRectangleChange;
            }
        }

        private void Parent_OnParentRectangleChange(object sender, ControlEventArgs e)
        {
            SetPosition();
        }

        private void Parent_OnZOrderChanged(object sender, ControlEventArgs e)
        {
            InternalZOrder = e.Control.ZOrder + 0.01f;
        }

        protected override void UnsubscribeToEvents()
        {
            if(Parent != null)
            {
                Parent.OnZOrderChanged -= Parent_OnZOrderChanged;
                Parent.OnParentRectangleChange -= Parent_OnParentRectangleChange;
            }
        }
    }
}
