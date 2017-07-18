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
    public class ScreenManager
    {
        public static Color ScreenColor { get; set; }
        public static ContentManager Content { get; set; }

        private static Screen _currentScreen { get; set; }

        private static SpriteBatch _spriteBatch;
        public static SpriteBatch SpriteBatch
        {
            get { return _spriteBatch; }
        }

        public static void SetSpriteBatch(SpriteBatch sb)
        {
            _spriteBatch = sb;
        }

        public static Screen CurrentScreen
        {
            get { return _currentScreen; }
        }

        static ScreenManager()
        {

        }

        public static void SetScreen(Screen newScreen)
        {
            newScreen.Initialized = false;  // Pre-Caution in case someone calls the LoadContent() function before setting the new screen!

            if(_currentScreen != null)
            {
                _currentScreen.Unload();
                _currentScreen = null;  // Make sure it's null...
            }

            _currentScreen = newScreen;
            _currentScreen.LoadContent();
        }

        public static void SetScreen(Screen newScreen, string args)
        {
            if (_currentScreen != null)
            {
                _currentScreen.Unload();
                _currentScreen = null;  // Make sure it's null...
            }

            _currentScreen = newScreen;
            newScreen.LoadContent(args);
        }

        public static T GetScreen<T>() where T : Screen
        {
            return (T)_currentScreen;
        }

        public static void LoadContent()
        {
            // Necessery?...
        }

        public static void Unload()
        {

        }

        public static void Update(GameTime gameTime)
        {
            _currentScreen?.Update(gameTime);
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            _currentScreen?.Draw(spriteBatch);
        }

        public static void Draw()
        {
            _currentScreen?.Draw(SpriteBatch);
        }
    }
}
