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
    public class Map
    {
        private Dictionary<int, Layer> _layers;

        public void AddLayer(int key, Layer layer)
        {
            _layers.Add(key, layer);
        }

        public void RemoveLayer(int key)
        {
            if (_layers.ContainsKey(key))
            {
                _layers.Remove(key);
            }
        }

        public Layer GetLayer(int key)
        {
            Layer layer = new Layer();
            if(_layers.TryGetValue(key, out layer))
            {
                return layer;
            }

            return null;
        }

        public Map()
        {
            _layers = new Dictionary<int, Layer>();
        }

        public virtual void LoadContent(ContentManager Content)
        {

        }

        public virtual void Update(GameTime gameTime)
        {
            foreach(var layer in _layers)
            {
                if (layer.Value != null)
                {
                    layer.Value.Update(gameTime);
                }
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (var layer in _layers)
            {
                if (layer.Value != null || layer.Value.DrawMe == true)
                {
                    layer.Value.Draw(spriteBatch);
                }
            }
        }
    }
}
