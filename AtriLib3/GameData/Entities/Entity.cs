using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AtriLib3.Interfaces;

namespace AtriLib3.Entities
{
    public class Entity : IWorldObject
    {
        public List<GameData.GameComponent> Components;
        public bool Initialized { get; private set; }
        public Vector2 Size { get; set; }
        protected Rectangle CollisionRect { get; set; }

        public int Width
        {
            get { return (int)Size.X; }
        }

        public int Height
        {
            get { return (int)Size.Y; }
        }

        public virtual Rectangle ObjectRectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
            }
        }

        private bool hasCol = false;
        public bool HasCollision
        {
            get
            {
                return hasCol;
            }
            set
            {
                hasCol = value;
            }
        }

        public Entity()
        {
            Components = new List<GameData.GameComponent>();
            Initialized = false;
            Size = new Vector2(32, 32);
        }

        public Entity(int width, int height)
        {
            Components = new List<GameData.GameComponent>();
            Initialized = false;
            Size = new Vector2(width, height);
        }

        public virtual void LoadContent()
        {
            Initialized = true;
        }

        public virtual void Unload()
        {

        }

        public virtual Vector2 Position
        {
            get; protected set;
        }

        public virtual void Update(GameTime gameTime)
        {
            for(int i = 0; i < Components.Count; i++)
            {
                Components[i]?.Update(gameTime);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < Components.Count; i++)
            {
                Components[i]?.Draw(spriteBatch);
            }
        }
    }
}
