using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AtriLib3.Interfaces;

namespace AtriLib3.World2D
{
    public class Tile : IWorldObject
    {
        public const int DEFAULT_WIDTH = 32;
        public const int DEFAULT_HEIGHT = 32;
        public const float DEFAULT_DEPHT = 0.5f;

        public Vector2 Position { get; set; }
        public string ID { get; set; }
        public float Depth { get; set; } = DEFAULT_DEPHT;

        public int Width { get; set; } = DEFAULT_WIDTH;
        public int Height { get; set; } = DEFAULT_HEIGHT;

        /// <summary>
        /// Get the ID as an Integer
        /// </summary>
        public int IDInt
        {
            get
            {
                int val = 0;

                if(int.TryParse(ID, out val))
                {
                    return val;
                }

                return -1;
            }
        }

        public Rectangle ObjectRectangle
        {
            get
            {
                return new Rectangle((int)Position.X * Width, (int)Position.Y * Height, Width, Height);
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

        public Tile()
        {
            ID = string.Empty;
            Position = Vector2.Zero;
        }

        public Tile(string id)
        {
            Position = Vector2.Zero;
            ID = id;
        }

        public Tile(Vector2 position)
        {
            ID = string.Empty;
            Position = position;
        }

        public Tile(string id, Vector2 position)
        {
            ID = id;
            Position = position;
        }

        public Tile DeepCopy()
        {
            return MemberwiseClone() as Tile;
        }
    }
}
