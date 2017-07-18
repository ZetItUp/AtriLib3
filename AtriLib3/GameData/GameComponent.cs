using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace AtriLib3.GameData
{
    public class GameComponent
    {
        private List<GameComponent> _childComponents;
        public int ID { get; protected set; }

        public virtual void AddComponent(GameComponent component)
        {
            _childComponents.Add(component);
        }

        public virtual GameComponent GetComponent(GameComponent component)
        {
            foreach(var c in _childComponents)
            {
                if(c == component)
                {
                    return c;
                }
            }

            return null;
        }

        public virtual GameComponent GetComponent(int index)
        {
            // TODO: Fix errors?

            if(index > _childComponents.Count || index < 0)
            {
                return null;
            }

            return _childComponents[index];
        }

        public GameComponent()
        {
            ID = -1;
            _childComponents = new List<GameComponent>();
        }

        public virtual void LoadContent(ContentManager Content)
        {

        }

        public virtual void Update(GameTime gameTime)
        {
            var tmpList = _childComponents.ToArray();

            foreach(var c in tmpList)
            {
                c.Update(gameTime);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            var tmpList = _childComponents.ToArray();

            foreach (var c in tmpList)
            {
                c.Draw(spriteBatch);
            }
        }
    }
}
