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
    // Redundant?
    public class ColliderComponent : GameComponent
    {
        public ColliderComponent()
            : base()
        {

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
            // Collider Components are not drawn!
            return;
        }
    }
}
