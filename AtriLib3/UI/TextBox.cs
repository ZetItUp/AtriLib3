using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AtriLib3.Utility;
using Microsoft.Xna.Framework.Input;

namespace AtriLib3.UI
{
    public class TextBox : Control, IKeyboardSubscriber
    {
        private string _text = "";
        private bool _hover = false;
        private float zorderFont = 0f;

        public SpriteFont Font { get; set; }
        public Color BackgroundColor { get; set; } = Color.White;
        public Color FontColor { get; set; } = Color.Black;

        private int _currentPosition { get; set; } = 0;

        private int CurrentPosition
        {
            get
            {
                return _currentPosition;
            }
            set
            {
                _currentPosition = value;
            }
        }

        private void CurrentPositionErrorCheck()
        {
            if (_currentPosition < 0)
            {
                _currentPosition = 0;
            }

            if (_currentPosition > Text.Length)
            {
                _currentPosition = Text.Length;
            }
        }

        private void RemoveCurrentChar(bool delete = false)
        {
            if (delete)
            {
                if(_currentPosition < Text.Length)
                {
                    Text = Text.Remove(_currentPosition, 1);
                }
            }
            else
            {
                if (_currentPosition > 0)
                {
                    Text = Text.Remove(_currentPosition - 1, 1);
                    CurrentPosition--;
                }
            }
        }

        private bool _caretVisible = true;
        public bool ShowTextCursor { get; set; } = true;

        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                CurrentPositionErrorCheck();
            }
        }

        public bool Selected { get; set; }

        public TextBox(string name, Rectangle textBoxRectangle)
            : base(name, textBoxRectangle)
        {
            SubscribeToEvents();
            Parent_OnParentRectangleChange(this, new ControlEventArgs(this));
        }

        public TextBox(string name, Control parent, Rectangle textBoxRectangle)
            : base(name, parent, textBoxRectangle)
        {
            SubscribeToEvents();
            Parent_OnParentRectangleChange(this, new ControlEventArgs(this));
        }

        public TextBox(string name, Control parent, SpriteFont font, Rectangle textBoxRectangle)
            : base(name, parent, textBoxRectangle)
        {
            SubscribeToEvents();
            Font = font;
            Parent_OnParentRectangleChange(this, new ControlEventArgs(this));
        }
        
        public void RecieveCommandInput(char command)
        {
            switch(command)
            {
                case '\b':
                    RemoveCurrentChar();
                    break;
                default:
                    break;
            }
        }

        public void RecieveInput(KeyEventArgs e)
        {
            
        }

        public void RecieveSpecialInput(Keys key)
        {
            
        }

        public void RecieveSpecialInput(Keys key, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Home)
            {
                CurrentPosition = 0;
            }
            else if(e.KeyCode == Keys.End)
            {
                CurrentPosition = Text.Length;
            }
            else if(e.KeyCode == Keys.Delete)
            {
                RemoveCurrentChar(true);
            }
            else if (e.KeyCode == Keys.Left)
            {
                if (CurrentPosition > 0)
                {
                    CurrentPosition--;
                }
            }
            else if (e.KeyCode == Keys.Right)
            {
                if (CurrentPosition < Text.Length)
                {
                    CurrentPosition++;
                }
            }
        }

        public void RecieveTextInput(char inputChar)
        {
            // Error checks, need to validate in case we get a negative number between inputs.
            if(CurrentPosition < 0)
            {
                CurrentPosition = 0;
            }

            Text = Text.Insert(CurrentPosition, inputChar.ToString());
            CurrentPosition++;
        }

        public void RecieveTextInput(string text)
        {
            // Error checks, need to validate in case we get a negative number between inputs.
            if (CurrentPosition < 0)
            {
                CurrentPosition = 0;
            }

            Text = Text.Insert(CurrentPosition, text);
            CurrentPosition += text.Length;
        }

        public void RecieveTextInput(char OutputChar, CharacterEventArgs e)
        {
            
        }

        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (ShowTextCursor)
            {
                if ((gameTime.TotalGameTime.TotalMilliseconds % 1000) < 500)
                {
                    _caretVisible = false;
                }
                else
                {
                    _caretVisible = true;
                }
            }
        }

        private void DrawTextBox(SpriteBatch spriteBatch)
        {
            if(_hover)
            {
                // Top Left
                spriteBatch.Draw(UIManager.ActiveInstance.Texture,
                    new Rectangle(Rectangle.X, Rectangle.Y, UIManager.ActiveInstance.ControlGraphicsData.TextBoxHoverTopLeft.Width, UIManager.ActiveInstance.ControlGraphicsData.TextBoxHoverTopLeft.Height),
                    UIManager.ActiveInstance.ControlGraphicsData.TextBoxHoverTopLeft,
                    BackgroundColor, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
                // Top Right
                spriteBatch.Draw(UIManager.ActiveInstance.Texture,
                    new Rectangle(Rectangle.X + Rectangle.Width - UIManager.ActiveInstance.ControlGraphicsData.TextBoxHoverTopRight.Width, Rectangle.Y, UIManager.ActiveInstance.ControlGraphicsData.TextBoxHoverTopRight.Width, UIManager.ActiveInstance.ControlGraphicsData.TextBoxHoverTopRight.Height),
                    UIManager.ActiveInstance.ControlGraphicsData.TextBoxHoverTopRight,
                    BackgroundColor, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);

                // Bottom Left
                spriteBatch.Draw(UIManager.ActiveInstance.Texture,
                    new Rectangle(Rectangle.X, Rectangle.Y + Rectangle.Height - UIManager.ActiveInstance.ControlGraphicsData.TextBoxHoverBottomLeft.Height, UIManager.ActiveInstance.ControlGraphicsData.TextBoxHoverBottomLeft.Width, UIManager.ActiveInstance.ControlGraphicsData.TextBoxHoverBottomLeft.Height),
                    UIManager.ActiveInstance.ControlGraphicsData.TextBoxHoverBottomLeft,
                    BackgroundColor, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
                // Bottom Right
                spriteBatch.Draw(UIManager.ActiveInstance.Texture,
                    new Rectangle(Rectangle.X + Rectangle.Width - UIManager.ActiveInstance.ControlGraphicsData.TextBoxHoverBottomRight.Width, Rectangle.Y + Rectangle.Height - UIManager.ActiveInstance.ControlGraphicsData.TextBoxHoverBottomLeft.Height, UIManager.ActiveInstance.ControlGraphicsData.TextBoxHoverBottomRight.Width, UIManager.ActiveInstance.ControlGraphicsData.TextBoxHoverBottomRight.Height),
                    UIManager.ActiveInstance.ControlGraphicsData.TextBoxHoverBottomRight,
                    BackgroundColor, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);

                // Only do the Fills if size is greater than the set dimensions

                if (Rectangle.Width > UIManager.ActiveInstance.ControlGraphicsData.WindowDimension)
                {
                    // Bottom Fill
                    for (int x = 0; x < Rectangle.Width - UIManager.ActiveInstance.ControlGraphicsData.TextBoxHoverBottomLeft.Width * 2; x++)
                    {
                        spriteBatch.Draw(UIManager.ActiveInstance.Texture,
                            new Rectangle(Rectangle.X + UIManager.ActiveInstance.ControlGraphicsData.TextBoxHoverBottomLeft.Width + x, Rectangle.Y + Rectangle.Height - UIManager.ActiveInstance.ControlGraphicsData.TextBoxHoverBottomLeft.Height, UIManager.ActiveInstance.ControlGraphicsData.TextBoxHoverBottomFill.Width, UIManager.ActiveInstance.ControlGraphicsData.TextBoxHoverBottomFill.Height),
                            UIManager.ActiveInstance.ControlGraphicsData.TextBoxHoverBottomFill,
                            BackgroundColor, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
                    }

                    // Top Fill
                    for (int x = 0; x < Rectangle.Width - UIManager.ActiveInstance.ControlGraphicsData.TextBoxTopLeft.Width * 2; x++)
                    {
                        spriteBatch.Draw(UIManager.ActiveInstance.Texture,
                            new Rectangle(Rectangle.X + UIManager.ActiveInstance.ControlGraphicsData.TextBoxHoverTopLeft.Width + x, Rectangle.Y, UIManager.ActiveInstance.ControlGraphicsData.TextBoxHoverTopFill.Width, UIManager.ActiveInstance.ControlGraphicsData.TextBoxHoverTopFill.Height),
                            UIManager.ActiveInstance.ControlGraphicsData.TextBoxHoverTopFill,
                            BackgroundColor, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
                    }
                }

                if (Rectangle.Height > UIManager.ActiveInstance.ControlGraphicsData.WindowDimension)
                {
                    // Left Fill
                    for (int y = 0; y < Rectangle.Height - UIManager.ActiveInstance.ControlGraphicsData.TextBoxHoverBottomLeft.Height * 2; y++)
                    {
                        spriteBatch.Draw(UIManager.ActiveInstance.Texture,
                            new Rectangle(Rectangle.X, Rectangle.Y + UIManager.ActiveInstance.ControlGraphicsData.TextBoxHoverBottomLeft.Height + y, UIManager.ActiveInstance.ControlGraphicsData.TextBoxHoverLeftFill.Width, UIManager.ActiveInstance.ControlGraphicsData.TextBoxHoverLeftFill.Height),
                            UIManager.ActiveInstance.ControlGraphicsData.TextBoxHoverLeftFill,
                            BackgroundColor, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
                    }

                    // Right Fill
                    for (int y = 0; y < Rectangle.Height - UIManager.ActiveInstance.ControlGraphicsData.TextBoxHoverBottomLeft.Height * 2; y++)
                    {
                        spriteBatch.Draw(UIManager.ActiveInstance.Texture,
                            new Rectangle(Rectangle.X + Rectangle.Width - UIManager.ActiveInstance.ControlGraphicsData.TextBoxTopLeft.Width, Rectangle.Y + UIManager.ActiveInstance.ControlGraphicsData.TextBoxBottomLeft.Height + y, UIManager.ActiveInstance.ControlGraphicsData.TextBoxHoverLeftFill.Width, UIManager.ActiveInstance.ControlGraphicsData.TextBoxHoverLeftFill.Height),
                            UIManager.ActiveInstance.ControlGraphicsData.TextBoxHoverRightFill,
                            BackgroundColor, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
                    }
                }

                // Fill Rectangle
                spriteBatch.Draw(UIManager.ActiveInstance.Texture,
                    new Rectangle(Rectangle.X + UIManager.ActiveInstance.ControlGraphicsData.TextBoxHoverTopLeft.Width, Rectangle.Y + UIManager.ActiveInstance.ControlGraphicsData.TextBoxHoverTopLeft.Height, Rectangle.Width - UIManager.ActiveInstance.ControlGraphicsData.TextBoxHoverTopLeft.Width * 2, Rectangle.Height - UIManager.ActiveInstance.ControlGraphicsData.TextBoxHoverTopLeft.Height * 2),
                    UIManager.ActiveInstance.ControlGraphicsData.TextBoxHoverBase,
                    BackgroundColor, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
            }
            else
            {
                // Top Left
                spriteBatch.Draw(UIManager.ActiveInstance.Texture,
                    new Rectangle(Rectangle.X, Rectangle.Y, UIManager.ActiveInstance.ControlGraphicsData.TextBoxTopLeft.Width, UIManager.ActiveInstance.ControlGraphicsData.TextBoxTopLeft.Height),
                    UIManager.ActiveInstance.ControlGraphicsData.TextBoxTopLeft,
                    BackgroundColor, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
                // Top Right
                spriteBatch.Draw(UIManager.ActiveInstance.Texture,
                    new Rectangle(Rectangle.X + Rectangle.Width - UIManager.ActiveInstance.ControlGraphicsData.TextBoxTopRight.Width, Rectangle.Y, UIManager.ActiveInstance.ControlGraphicsData.TextBoxTopRight.Width, UIManager.ActiveInstance.ControlGraphicsData.TextBoxTopRight.Height),
                    UIManager.ActiveInstance.ControlGraphicsData.TextBoxTopRight,
                    BackgroundColor, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);

                // Bottom Left
                spriteBatch.Draw(UIManager.ActiveInstance.Texture,
                    new Rectangle(Rectangle.X, Rectangle.Y + Rectangle.Height - UIManager.ActiveInstance.ControlGraphicsData.TextBoxBottomLeft.Height, UIManager.ActiveInstance.ControlGraphicsData.TextBoxBottomLeft.Width, UIManager.ActiveInstance.ControlGraphicsData.TextBoxBottomLeft.Height),
                    UIManager.ActiveInstance.ControlGraphicsData.TextBoxBottomLeft,
                    BackgroundColor, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
                // Bottom Right
                spriteBatch.Draw(UIManager.ActiveInstance.Texture,
                    new Rectangle(Rectangle.X + Rectangle.Width - UIManager.ActiveInstance.ControlGraphicsData.TextBoxBottomRight.Width, Rectangle.Y + Rectangle.Height - UIManager.ActiveInstance.ControlGraphicsData.TextBoxBottomLeft.Height, UIManager.ActiveInstance.ControlGraphicsData.TextBoxBottomRight.Width, UIManager.ActiveInstance.ControlGraphicsData.TextBoxBottomRight.Height),
                    UIManager.ActiveInstance.ControlGraphicsData.TextBoxBottomRight,
                    BackgroundColor, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);

                // Only do the Fills if size is greater than the set dimensions

                if (Rectangle.Width > UIManager.ActiveInstance.ControlGraphicsData.WindowDimension)
                {
                    // Bottom Fill
                    for (int x = 0; x < Rectangle.Width - UIManager.ActiveInstance.ControlGraphicsData.TextBoxBottomLeft.Width * 2; x++)
                    {
                        spriteBatch.Draw(UIManager.ActiveInstance.Texture,
                            new Rectangle(Rectangle.X + UIManager.ActiveInstance.ControlGraphicsData.TextBoxBottomLeft.Width + x, Rectangle.Y + Rectangle.Height - UIManager.ActiveInstance.ControlGraphicsData.TextBoxBottomLeft.Height, UIManager.ActiveInstance.ControlGraphicsData.TextBoxBottomFill.Width, UIManager.ActiveInstance.ControlGraphicsData.TextBoxBottomFill.Height),
                            UIManager.ActiveInstance.ControlGraphicsData.TextBoxBottomFill,
                            BackgroundColor, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
                    }

                    // Top Fill
                    for (int x = 0; x < Rectangle.Width - UIManager.ActiveInstance.ControlGraphicsData.TextBoxTopLeft.Width * 2; x++)
                    {
                        spriteBatch.Draw(UIManager.ActiveInstance.Texture,
                            new Rectangle(Rectangle.X + UIManager.ActiveInstance.ControlGraphicsData.TextBoxTopLeft.Width + x, Rectangle.Y, UIManager.ActiveInstance.ControlGraphicsData.TextBoxTopFill.Width, UIManager.ActiveInstance.ControlGraphicsData.TextBoxTopFill.Height),
                            UIManager.ActiveInstance.ControlGraphicsData.TextBoxTopFill,
                            BackgroundColor, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
                    }
                }

                if (Rectangle.Height > UIManager.ActiveInstance.ControlGraphicsData.WindowDimension)
                {
                    // Left Fill
                    for (int y = 0; y < Rectangle.Height - UIManager.ActiveInstance.ControlGraphicsData.TextBoxBottomLeft.Height * 2; y++)
                    {
                        spriteBatch.Draw(UIManager.ActiveInstance.Texture,
                            new Rectangle(Rectangle.X, Rectangle.Y + UIManager.ActiveInstance.ControlGraphicsData.TextBoxBottomLeft.Height + y, UIManager.ActiveInstance.ControlGraphicsData.TextBoxLeftFill.Width, UIManager.ActiveInstance.ControlGraphicsData.TextBoxLeftFill.Height),
                            UIManager.ActiveInstance.ControlGraphicsData.TextBoxLeftFill,
                            BackgroundColor, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
                    }

                    // Right Fill
                    for (int y = 0; y < Rectangle.Height - UIManager.ActiveInstance.ControlGraphicsData.TextBoxBottomLeft.Height * 2; y++)
                    {
                        spriteBatch.Draw(UIManager.ActiveInstance.Texture,
                            new Rectangle(Rectangle.X + Rectangle.Width - UIManager.ActiveInstance.ControlGraphicsData.TextBoxTopLeft.Width, Rectangle.Y + UIManager.ActiveInstance.ControlGraphicsData.TextBoxBottomLeft.Height + y, UIManager.ActiveInstance.ControlGraphicsData.TextBoxLeftFill.Width, UIManager.ActiveInstance.ControlGraphicsData.TextBoxLeftFill.Height),
                            UIManager.ActiveInstance.ControlGraphicsData.TextBoxRightFill,
                            BackgroundColor, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
                    }
                }

                // Fill Rectangle
                spriteBatch.Draw(UIManager.ActiveInstance.Texture,
                    new Rectangle(Rectangle.X + UIManager.ActiveInstance.ControlGraphicsData.TextBoxTopLeft.Width, Rectangle.Y + UIManager.ActiveInstance.ControlGraphicsData.TextBoxTopLeft.Height, Rectangle.Width - UIManager.ActiveInstance.ControlGraphicsData.TextBoxTopLeft.Width * 2, Rectangle.Height - UIManager.ActiveInstance.ControlGraphicsData.TextBoxTopLeft.Height * 2),
                    UIManager.ActiveInstance.ControlGraphicsData.TextBoxBase,
                    BackgroundColor, 0f, Vector2.Zero, SpriteEffects.None, ZOrder);
            }
        }

        private void DrawCursor(SpriteBatch spriteBatch)
        {
            if(Focus && ShowTextCursor && _caretVisible)
            {
                float width = 0;
                float height = 0;
                

                for(int i = 0; i < CurrentPosition; i++)
                {
                    if(Font.GetGlyphs().TryGetValue(Text[i], out SpriteFont.Glyph glyph))
                    {
                        width += glyph.WidthIncludingBearings;
                    }
                }

                Rectangle caretRect = UIManager.ActiveInstance.ControlGraphicsData.GetTextBoxCursorRectangle(20);

                spriteBatch.Draw(UIManager.ActiveInstance.Texture, 
                    new Rectangle(Rectangle.X + (int)width, Rectangle.Y + 4, caretRect.Width, caretRect.Height), 
                    caretRect,
                    BackgroundColor, 0f, Vector2.Zero, SpriteEffects.None, ZOrder + 0.01f);
            }
        }

        private void DrawFont(SpriteBatch spriteBatch)
        {
            Vector2 size = Font.MeasureString(Text);
            zorderFont = ZOrder + 0.00001f;

            spriteBatch.DrawString(Font, Text, 
                new Vector2(Rectangle.X + 1, Rectangle.Y + 1), 
                FontColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, zorderFont);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(!Visible)
            {
                return;
            }

            DrawTextBox(spriteBatch);
            DrawCursor(spriteBatch);
            DrawFont(spriteBatch);

            base.Draw(spriteBatch);
        }

        protected override void SubscribeToEvents()
        {
            if (Parent != null)
            {
                Parent.OnParentRectangleChange += Parent_OnParentRectangleChange;
                Parent.OnZOrderChanged += Parent_OnZOrderChanged;
            }

            OnMouseEnter += TextBox_OnMouseEnter;
            OnMouseLeave += TextBox_OnMouseLeave;
            OnMouseClicked += TextBox_OnMouseClicked;
        }

        private void TextBox_OnMouseClicked(object sender, ControlEventArgs e)
        {
            Focus = true;
            UIManager.KeyboardDispatcher.Subscriber = e.Control as TextBox;
        }

        private void TextBox_OnMouseLeave(object sender, ControlEventArgs e)
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

        private void TextBox_OnMouseEnter(object sender, ControlEventArgs e)
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

            OnMouseEnter -= TextBox_OnMouseEnter;
            OnMouseLeave -= TextBox_OnMouseLeave;
            OnMouseClicked -= TextBox_OnMouseClicked;
        }
    }
}
