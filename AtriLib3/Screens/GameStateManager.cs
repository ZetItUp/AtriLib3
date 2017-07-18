using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using AtriLib3.Screens;

namespace AtriLib3
{
    public class GameStateManager
    {
        private ContentManager _content;
        private Stack<Screen> _screens;

        public bool DrawPreviousScreen { get; set; } = false;

        public GameStateManager(ContentManager content)
        {
            _content = content;
            _screens = new Stack<Screen>();
        }

        public void PushScreen(Screen screen)
        {
            screen.LoadContent(_content);
            _screens.Push(screen);
        }

        public void PopScreen()
        {
            var screen = _screens.Pop();
            screen?.Unload();
        }

        public void Update(GameTime gameTime)
        {
            var currScreen = _screens.Peek();

            currScreen?.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(DrawPreviousScreen && _screens.Count > 1)
            {
                var prevScreen = _screens.ElementAt(1);

                prevScreen?.Draw(spriteBatch);
            }

            var currScreen = _screens.Peek();

            currScreen?.Draw(spriteBatch);
        }
    }
}
