using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace AtriLib3.World2D
{
    public class Layer
    {
        private float _zorder;

        public int Width { get; set; }
        public int Height { get; set; }
        public bool DrawMe { get; set; }

        public float ZOrder
        {
            get
            {
                return _zorder;
            }
            set
            {
                if(value < Constants.ZORDER_MIN)
                {
                    value = Constants.ZORDER_MIN;
                }

                if(value > Constants.ZORDER_MAX)
                {
                    value = Constants.ZORDER_MAX;
                }

                _zorder = value;
            }
        }

        private List<Tile> _tiles;

        public void AddTile(Tile t)
        {
            _tiles.Add(t);
        }

        public void RemoveTile(Tile t)
        {
            _tiles.Remove(t);
        }

        public List<Tile> Tiles
        {
            get { return _tiles; }
        }

        public Tile GetTile(int x, int y)
        {
            for(int i = 0; i < _tiles.Count; i++)
            {
                if(_tiles[i].Position.X == x && _tiles[i].Position.Y == y)
                {
                    return _tiles[i];
                }
            }

            return null;
        }

        public Layer()
        {
            _tiles = new List<Tile>();
            ZOrder = Constants.ZORDER_DEFAULT;
            DrawMe = true;
        }

        public virtual void LoadContent(ContentManager Content)
        {

        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            
        }
    }
}
