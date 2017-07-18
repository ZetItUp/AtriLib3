using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AtriLib3.Interfaces;

namespace AtriLib3.GameData
{
    public class AnimationComponent : GameComponent, IResizable
    {
        private int row { get; set; }
        private bool repeat { get; set; }
        private bool didPlayOnce { get; set; } = false;
        private bool playOnce { get; set; }
        private bool countBack { get; set; }
        private bool loopback { get; set; }
        private float currentTick { get; set; } = 0f;
        private float loopTick { get; set; } = 0f;

        public SpriteEffects SpriteEffect { get; set; }
        public Color DrawColor { get; set; } = Color.White;
        public Vector2 Position { get; set; }
        public Vector2 Origin { get; set; }
        public Texture2D Texture { get; set; }
        public int FrameWidth { get; }
        public int FrameHeight { get; }
        public int CurrentFrame { get; private set; }
        public int MaxFrames { get; set; }
        public bool Pause { get; set; }
        public bool Visible { get; set; }
        public bool FacingRight { get; set; }
        public float Depth { get; set; }
        public float TimeBetweenLoops { get; set; } = 0f;
        public float AnimationSpeed { get; set; }
        public float Rotation { get; set; }

        public event EventHandler PlayedOnce;

        public AnimationComponent(Texture2D animationTexture, Vector2 position, int frameWidth, int frameHeight, int maxFrames, float animationSpeed)
        {
            SpriteEffect = SpriteEffects.None;
            Origin = Vector2.Zero;
            Rotation = Constants.ROTATION_0;
            Depth = Constants.ZORDER_MAX;
            ScaleX = Constants.SCALE_PROPORTIONAL;
            ScaleY = Constants.SCALE_PROPORTIONAL;
            ScaleXY = Constants.SCALE_PROPORTIONAL;
            Repeat = true;
            PlayOnce = false;
            Visible = true;
            Position = position;

            FacingRight = true;
            Texture = animationTexture;
            FrameWidth = frameWidth;
            FrameHeight = frameHeight;
            ScaleTo(frameWidth, frameHeight);
            CurrentFrame = 0;
            Row = 0;
            MaxFrames = maxFrames;
            AnimationSpeed = animationSpeed;
        }

        public void SetFrame(int newFrame)
        {
            CurrentFrame = newFrame;
        }

        public bool PlayOnce
        {
            get
            {
                return playOnce;
            }
            set
            {
                if(value == true)
                {
                    didPlayOnce = false;
                    Pause = false;
                }

                playOnce = value;
            }
        }

        public bool Repeat
        {
            get
            {
                return repeat;
            }
            set
            {
                if(repeat != value)
                {
                    loopTick = 0f;
                }

                repeat = value;
            }
        }

        public bool Loopback
        {
            get
            {
                return loopback;
            }
            set
            {
                loopback = value;
            }
        }

        public int Row
        {
            get
            {
                return row;
            }
            set
            {
                if(row != value)
                {
                    row = value;
                    CurrentFrame = 0;   // Reset the current frame to 0 so we don't get weird jaggin
                }
            }
        }

        public bool InProportion
        {
            get; set;
        }

        public bool Resizable
        {
            get; set;
        }

        public float ScaleX
        {
            get; set;
        }

        public float ScaleXY
        {
            get; set;
        }

        public float ScaleY
        {
            get; set;
        }

        public void ScaleTo(float scaleXY)
        {
            SetScale(scaleXY, scaleXY);
        }

        public void ScaleTo(float width, float height)
        {
            // Check if in Proportion to eachother
            if (width % FrameWidth == 0 && height % FrameHeight == 0)
            {
                // In Proportion
                ScaleXY = width / FrameWidth;
                InProportion = true;
            }
            else
            {
                // Not in Proportion
                ScaleX = width / FrameWidth;
                ScaleY = height / FrameHeight;
                InProportion = false;
            }
        }

        public void SetScale(float scaleX, float scaleY)
        {
            float newWidth = FrameWidth * scaleX;
            float newHeight = FrameHeight * scaleY;

            // Check if in Proportion to eachother
            if (newWidth % FrameWidth == 0 && newHeight % FrameHeight == 0)
            {
                // In Proportion
                ScaleXY = newWidth / FrameWidth;
                InProportion = true;
            }
            else
            {
                ScaleX = newWidth / FrameWidth;
                ScaleY = newHeight / FrameHeight;
                InProportion = false;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if(Texture == null)
            {
                return;
            }

            if(Pause)
            {
                return;
            }

            currentTick += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if(!Repeat)
            {
                loopTick += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }

            if(currentTick >= AnimationSpeed)
            {
                if(Repeat)
                {
                    if(Loopback)
                    {
                        if(countBack)
                        {
                            CurrentFrame--;

                            if(CurrentFrame <= 0)
                            {
                                CurrentFrame = 0;
                                countBack = false;
                            }
                        }
                        else
                        {
                            CurrentFrame++;

                            if(CurrentFrame >= MaxFrames)
                            {
                                CurrentFrame = MaxFrames;

                                countBack = true;
                            }
                        }
                    }
                    else
                    {
                        CurrentFrame++;

                        if(CurrentFrame > MaxFrames)
                        {
                            CurrentFrame = 0;
                        }
                    }
                }
                else if(PlayOnce && !didPlayOnce)
                {
                    CurrentFrame++;

                    if(CurrentFrame > MaxFrames)
                    {
                        CurrentFrame = 0;
                        didPlayOnce = true;
                        Pause = true;
                        PlayOnce = false;

                        PlayedOnce?.Invoke(this, null);
                    }
                }
                else
                {
                    if(loopTick >= TimeBetweenLoops)
                    {
                        if(countBack)
                        {
                            CurrentFrame--;

                            if(CurrentFrame < 0)
                            {
                                CurrentFrame = 0;
                                countBack = false;
                                loopTick = 0;
                            }
                        }
                        else
                        {
                            CurrentFrame++;

                            if(CurrentFrame > MaxFrames)
                            {
                                CurrentFrame = MaxFrames;
                                countBack = true;
                            }
                        }
                    }
                }

                currentTick -= AnimationSpeed;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(Texture == null)
            {
                return;
            }

            if(!Visible)
            {
                return;
            }

            if(FacingRight)
            {
                if(InProportion)
                {
                    var sourceRect = new Rectangle(CurrentFrame * FrameWidth, Row * FrameHeight, FrameWidth, FrameHeight);
                    Rectangle destRect = new Rectangle();

                    if(ScaleXY != 1.0f)
                    {
                        destRect = new Rectangle((int)Position.X - ((int)(FrameWidth * ScaleXY) / 2), (int)Position.Y - (int)(FrameHeight * ScaleXY) + FrameHeight, (int)(sourceRect.Width * ScaleXY), (int)(sourceRect.Height * ScaleXY));
                    }
                    else
                    {
                        destRect = new Rectangle((int)Position.X, (int)Position.Y, (int)(sourceRect.Width * ScaleXY), (int)(sourceRect.Height * ScaleXY));
                    }

                    spriteBatch.Draw(Texture, destRect, sourceRect, DrawColor, Rotation, Origin, SpriteEffect, Depth);
                }
                else
                {
                    var sourceRect = new Rectangle(CurrentFrame * FrameWidth, Row * FrameHeight, FrameWidth, FrameHeight);
                    Rectangle destRect = new Rectangle();

                    if (ScaleX != 1.0f || ScaleY != 1.0f)
                    {
                        destRect = new Rectangle((int)Position.X - ((int)(FrameWidth * ScaleX) / 2), (int)Position.Y - (int)(FrameHeight * ScaleY) + FrameHeight, (int)(sourceRect.Width * ScaleX), (int)(sourceRect.Height * ScaleY));
                    }
                    else
                    {
                        destRect = new Rectangle((int)Position.X, (int)Position.Y, (int)(sourceRect.Width * ScaleX), (int)(sourceRect.Height * ScaleY));
                    }

                    spriteBatch.Draw(Texture, destRect, sourceRect, DrawColor, Rotation, Origin, SpriteEffect, Depth);
                }
            }
            else
            {
                if (InProportion)
                {
                    var sourceRect = new Rectangle(CurrentFrame * FrameWidth, Row * FrameHeight, FrameWidth, FrameHeight);
                    Rectangle destRect = new Rectangle();

                    if (ScaleXY != 1.0f)
                    {
                        destRect = new Rectangle((int)Position.X - ((int)(FrameWidth * ScaleXY) / 2), (int)Position.Y - (int)(FrameHeight * ScaleXY) + FrameHeight, (int)(sourceRect.Width * ScaleXY), (int)(sourceRect.Height * ScaleXY));
                    }
                    else
                    {
                        destRect = new Rectangle((int)Position.X, (int)Position.Y, (int)(sourceRect.Width * ScaleXY), (int)(sourceRect.Height * ScaleXY));
                    }

                    if (SpriteEffect == SpriteEffects.None)
                    {
                        // Override sprite effect to force a flip...
                        spriteBatch.Draw(Texture, destRect, sourceRect, DrawColor, Rotation, Origin, SpriteEffects.FlipHorizontally, Depth);
                    }
                    else
                    { 
                        // ... else ignore is
                        spriteBatch.Draw(Texture, destRect, sourceRect, DrawColor, Rotation, Origin, SpriteEffect, Depth);
                    }
                }
                else
                {
                    var sourceRect = new Rectangle(CurrentFrame * FrameWidth, Row * FrameHeight, FrameWidth, FrameHeight);
                    Rectangle destRect = new Rectangle();

                    if (ScaleX != 1.0f || ScaleY != 1.0f)
                    {
                        destRect = new Rectangle((int)Position.X - ((int)(FrameWidth * ScaleX) / 2), (int)Position.Y - (int)(FrameHeight * ScaleY) + FrameHeight, (int)(sourceRect.Width * ScaleX), (int)(sourceRect.Height * ScaleY));
                    }
                    else
                    {
                        destRect = new Rectangle((int)Position.X, (int)Position.Y, (int)(sourceRect.Width * ScaleX), (int)(sourceRect.Height * ScaleY));
                    }

                    if (SpriteEffect == SpriteEffects.None)
                    {
                        // Override sprite effect to force a flip...
                        spriteBatch.Draw(Texture, destRect, sourceRect, DrawColor, Rotation, Origin, SpriteEffects.FlipHorizontally, Depth);
                    }
                    else
                    {
                        // ... else ignore is
                        spriteBatch.Draw(Texture, destRect, sourceRect, DrawColor, Rotation, Origin, SpriteEffect, Depth);
                    }
                }
            }
        }
    }
}
