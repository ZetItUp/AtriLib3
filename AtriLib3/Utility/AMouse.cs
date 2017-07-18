using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AtriLib3.Utility
{
    public class AMouse : GameComponent
    {
        public enum MouseButton
        {
            Left,
            Middle,
            Right
        };

        static MouseState _currMouseState;
        static MouseState _prevMouseState;
        static Rectangle _mouseRect;

        public AMouse(Game game)
            : base(game)
        {
            _currMouseState = Mouse.GetState();
        }

        public override void Update(GameTime gameTime)
        {
            _prevMouseState = _currMouseState;
            _currMouseState = Mouse.GetState();

            _mouseRect = new Rectangle(_currMouseState.X, _currMouseState.Y, 1, 1);

            base.Update(gameTime);
        }

        /// <summary>
        /// Get the Mouse current scroll wheel value.
        /// </summary>
        public static float ScrollWheelValue
        {
            get
            {
                return _currMouseState.ScrollWheelValue;
            }
        }

        /// <summary>
        /// Get the Mouse previous scroll wheel value.
        /// </summary>
        public static float PreviousScrollWheelValue
        {
            get
            {
                return _prevMouseState.ScrollWheelValue;
            }
        }

        /// <summary>
        /// Get a 1x1 rectangle at mouse current position.
        /// </summary>
        public static Rectangle MouseRectangle
        {
            get
            {
                return _mouseRect;
            }
        }

        /// <summary>
        /// Get Mouse current position
        /// </summary>
        public static Vector2 MousePosition
        {
            get
            {
                return new Vector2(MouseRectangle.X, MouseRectangle.Y);
            }
        }

        /// <summary>
        /// Was the mouse button released
        /// </summary>
        /// <param name="mButton"></param>
        /// <returns>bool</returns>
        public static bool MouseReleased(MouseButton mButton)
        {
            switch(mButton)
            {
                case MouseButton.Left:
                    {
                        return (_currMouseState.LeftButton == ButtonState.Released && _prevMouseState.LeftButton == ButtonState.Pressed);
                    }
                case MouseButton.Middle:
                    {
                        return (_currMouseState.MiddleButton == ButtonState.Released && _prevMouseState.MiddleButton == ButtonState.Pressed);
                    }
                case MouseButton.Right:
                    {
                        return (_currMouseState.RightButton == ButtonState.Released && _prevMouseState.RightButton == ButtonState.Pressed);
                    }
                default:
                    return false;
            }
        }

        /// <summary>
        /// Was the mouse button clicked.
        /// </summary>
        /// <param name="mButton"></param>
        /// <returns>bool</returns>
        public static bool MousePressed(MouseButton mButton)
        {
            switch (mButton)
            {
                case MouseButton.Left:
                    {
                        return (_currMouseState.LeftButton == ButtonState.Pressed && _prevMouseState.LeftButton == ButtonState.Released);
                    }
                case MouseButton.Middle:
                    {
                        return (_currMouseState.MiddleButton == ButtonState.Pressed && _prevMouseState.MiddleButton == ButtonState.Released);
                    }
                case MouseButton.Right:
                    {
                        return (_currMouseState.RightButton == ButtonState.Pressed && _prevMouseState.RightButton == ButtonState.Released);
                    }
                default:
                    return false;
            }
        }

        /// <summary>
        /// Is the mouse button pressed down
        /// </summary>
        /// <param name="mButton"></param>
        /// <returns>bool</returns>
        public static bool MouseDown(MouseButton mButton)
        {
            switch (mButton)
            {
                case MouseButton.Left:
                    {
                        return (_currMouseState.LeftButton == ButtonState.Pressed);
                    }
                case MouseButton.Middle:
                    {
                        return (_currMouseState.MiddleButton == ButtonState.Pressed);
                    }
                case MouseButton.Right:
                    {
                        return (_currMouseState.RightButton == ButtonState.Pressed);
                    }
                default:
                    return false;
            }
        }
    }
}
