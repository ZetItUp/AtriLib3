using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace AtriLib3.Screens
{
    public class Screen
    {
        internal bool Initialized { get; set; } = false;

        public Screen()
        {

        }

        public virtual void LoadContent()
        {
            Initialized = true;
        }

        public virtual void LoadContent(ContentManager content)
        {
            Initialized = true;
        }

        public virtual void LoadContent(string args)
        {

        }

        public virtual void Unload()
        {

        }

        public virtual bool Update(GameTime gameTime)
        {
            if (!Initialized)
            {
                return false;
            }

            return true;
        }

        public virtual bool Draw(SpriteBatch spriteBatch)
        {
            if (!Initialized)
            {
                return false;
            }

            return true;
        }
    }
}
