using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AtriLib3.Utility
{
    public class Input : GameComponent
    {
        static KeyboardState currKeyState;
        static KeyboardState lastKeyState;

        static GamePadState[] gamePadStates;
        static GamePadState[] lastGamePadStates;

        public static KeyboardState KeyboardState
        {
            get { return currKeyState; }
        }

        public static KeyboardState PreviousKeyboardState
        {
            get { return lastKeyState; }
        }

        /// <summary>
        /// Get an array of currently pressed keys
        /// </summary>
        /// <returns>Keys[] Array</returns>
        public static Keys[] PressedKeys()
        {
            return KeyboardState.GetPressedKeys();
        }

        public static bool GamePadConnected
        {
            get
            {
                GamePadState state = GamePad.GetState(PlayerIndex.One);

                return state.IsConnected;
            }
        }

        public static bool GamePadIndexConnected(PlayerIndex pIndex)
        {
            GamePadState state = GamePad.GetState(pIndex);

            return state.IsConnected;
        }

        public Input(Game game)
            : base(game)
        {
            currKeyState = Keyboard.GetState();
            gamePadStates = new GamePadState[Enum.GetValues(typeof(PlayerIndex)).Length];

            foreach (PlayerIndex idx in Enum.GetValues(typeof(PlayerIndex)))
            {
                gamePadStates[(int)idx] = GamePad.GetState(idx);
            }
        }

        public override void Update(GameTime gameTime)
        {
            Flush();
            currKeyState = Keyboard.GetState();

            lastGamePadStates = (GamePadState[])gamePadStates.Clone();

            foreach (PlayerIndex idx in Enum.GetValues(typeof(PlayerIndex)))
            {
                gamePadStates[(int)idx] = GamePad.GetState(idx);
            }

            base.Update(gameTime);
        }

        public static void Flush()
        {
            lastKeyState = currKeyState;
        }

        public static GamePadState[] GamePadStates
        {
            get { return gamePadStates; }
        }

        public static GamePadState[] PreviousGamePadStates
        {
            get { return lastGamePadStates; }
        }

        public static bool AnyKeyPressed
        {
            get
            {
                if(currKeyState.GetPressedKeys().Length > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static Keys[] GetPressedKeys()
        {
            return currKeyState.GetPressedKeys();
        }

        public static bool KeyReleased(Keys key)
        {
            return currKeyState.IsKeyUp(key) && lastKeyState.IsKeyDown(key);
        }

        public static bool KeyPressed(Keys key)
        {
            return currKeyState.IsKeyDown(key) && lastKeyState.IsKeyUp(key);
        }

        public static bool KeyDown(Keys key)
        {
            return currKeyState.IsKeyDown(key);
        }

        public static bool ButtonReleased(Buttons button, PlayerIndex pIndex)
        {
            return gamePadStates[(int)pIndex].IsButtonUp(button) &&
                lastGamePadStates[(int)pIndex].IsButtonDown(button);
        }

        public static bool ButtonPressed(Buttons button, PlayerIndex pIndex)
        {
            return gamePadStates[(int)pIndex].IsButtonDown(button) &&
                lastGamePadStates[(int)pIndex].IsButtonUp(button);
        }

        public static bool ButtonDown(Buttons button, PlayerIndex pIndex)
        {
            return gamePadStates[(int)pIndex].IsButtonDown(button);
        }
    }
}
