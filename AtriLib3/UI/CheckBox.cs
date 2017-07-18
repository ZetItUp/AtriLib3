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
    public class CheckBox : Control
    {
        private string _text = "";
        private bool _hover = false;

        public SpriteFont Font { get; set; }
        public bool Checked { get; set; } = false;

        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;

                if (AutoSize)
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

        public CheckBox(string name, Rectangle checkBoxRect)
            : base(name, checkBoxRect)
        {
            SubscribeToEvents();
            ShadowDistance = new Vector2(1, 1);
            Parent_OnParentRectangleChange(this, new ControlEventArgs(this));
        }

        public CheckBox(string name, Control parent, Rectangle checkBoxRect)
            : base(name, parent, checkBoxRect)
        {
            SubscribeToEvents();
            ShadowDistance = new Vector2(1, 1);
            Parent_OnParentRectangleChange(this, new ControlEventArgs(this));
        }

        public CheckBox(string name, Control parent, SpriteFont font, Rectangle checkBoxRect)
            : base(name, parent, checkBoxRect)
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

            if (Checked)
            {
                if (_hover)
                {
                    spriteBatch.Draw(UIManager.ActiveInstance.Texture,
                        new Rectangle(Rectangle.X, Rectangle.Y, UIManager.ActiveInstance.ControlGraphicsData.CheckBoxCheckedRectangle.Width, UIManager.ActiveInstance.ControlGraphicsData.CheckBoxCheckedRectangle.Height),
                        UIManager.ActiveInstance.ControlGraphicsData.CheckBoxCheckedHoverRectangle, Color, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
                }
                else
                {
                    spriteBatch.Draw(UIManager.ActiveInstance.Texture,
                        new Rectangle(Rectangle.X, Rectangle.Y, UIManager.ActiveInstance.ControlGraphicsData.CheckBoxCheckedRectangle.Width, UIManager.ActiveInstance.ControlGraphicsData.CheckBoxCheckedRectangle.Height),
                        UIManager.ActiveInstance.ControlGraphicsData.CheckBoxCheckedRectangle, Color, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
                }
            }
            else
            {
                if (_hover)
                {
                    spriteBatch.Draw(UIManager.ActiveInstance.Texture,
                        new Rectangle(Rectangle.X, Rectangle.Y, UIManager.ActiveInstance.ControlGraphicsData.CheckBoxUncheckedRectangle.Width, UIManager.ActiveInstance.ControlGraphicsData.CheckBoxUncheckedRectangle.Height),
                        UIManager.ActiveInstance.ControlGraphicsData.CheckBoxUncheckedHoverRectangle, Color, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
                }
                else
                {
                    spriteBatch.Draw(UIManager.ActiveInstance.Texture,
                        new Rectangle(Rectangle.X, Rectangle.Y, UIManager.ActiveInstance.ControlGraphicsData.CheckBoxUncheckedRectangle.Width, UIManager.ActiveInstance.ControlGraphicsData.CheckBoxUncheckedRectangle.Height),
                        UIManager.ActiveInstance.ControlGraphicsData.CheckBoxUncheckedRectangle, Color, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
                }
            }

            if (CastShadow)
            {
                spriteBatch.DrawString(Font, Text, new Vector2(Rectangle.X + UIManager.ActiveInstance.ControlGraphicsData.CheckBoxCheckedRectangle.Width + 2 + ShadowDistance.X, Rectangle.Y + ShadowDistance.Y), ShadowColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, ZOrder - 0.01f);
            }

            spriteBatch.DrawString(Font, Text, new Vector2(Rectangle.X + UIManager.ActiveInstance.ControlGraphicsData.CheckBoxCheckedRectangle.Width + 2, Rectangle.Y), Color, 0f, Vector2.Zero, 1f, SpriteEffects.None, ZOrder);

            base.Draw(spriteBatch);
        }

        protected override void SubscribeToEvents()
        {
            if (Parent != null)
            {
                Parent.OnParentRectangleChange += Parent_OnParentRectangleChange;
                Parent.OnZOrderChanged += Parent_OnZOrderChanged;
            }

            OnMouseClicked += CheckBox_OnClicked;
            OnMouseEnter += CheckBox_OnMouseEnter;
            OnMouseLeave += CheckBox_OnMouseLeave;
        }

        protected override void UnsubscribeToEvents()
        {
            if (Parent != null)
            {
                Parent.OnZOrderChanged -= Parent_OnZOrderChanged;
                Parent.OnParentRectangleChange -= Parent_OnParentRectangleChange;
            }

            OnMouseClicked -= CheckBox_OnClicked;
            OnMouseEnter -= CheckBox_OnMouseEnter;
            OnMouseLeave -= CheckBox_OnMouseLeave;
        }

        private void CheckBox_OnMouseLeave(object sender, ControlEventArgs e)
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

        private void CheckBox_OnMouseEnter(object sender, ControlEventArgs e)
        {
            if (Parent == null)
            {
                _hover = true;
            }
            else
            {
                if (Parent is Window)
                {
                    var parent = Parent as Window;
                    if (parent.Active)
                    {
                        _hover = true;
                    }
                }
            }
        }

        private void CheckBox_OnClicked(object sender, ControlEventArgs e)
        {
            if (Parent == null)
            {
                Checked = !Checked;
            }
            else
            {
                if (Parent is Window)
                {
                    var parent = Parent as Window;
                    if (parent.Active)
                    {
                        Checked = !Checked;
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
    }
}
