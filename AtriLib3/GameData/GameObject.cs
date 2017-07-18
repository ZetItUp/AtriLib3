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
    public class GameObject : GameComponent
    {
        public override void AddComponent(GameComponent component)
        {
            if(component is GameObject)
            {
                // TODO: Throw error?
                // Return for now, do not allow the default gameobject to have default gameobjects inside it's children.
                return;
            }

            base.AddComponent(component);
        }

        public GameObject()
            : base()
        {
            
        }

        public override void LoadContent(ContentManager Content)
        {

        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
