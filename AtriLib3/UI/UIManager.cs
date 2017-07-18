using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using AtriLib3.Utility;

namespace AtriLib3.UI
{
    public class UIManager
    {
        public static KeyboardDispatcher KeyboardDispatcher;
        public static UIManager ActiveInstance { get; set; } = null;
        public ControlGraphicsData ControlGraphicsData { get; private set; } = null;
        private Dictionary<string, Control> _controls;

        public Texture2D Texture { get; private set; }
        public Monitor Monitor { get; private set; }

        public void SetWindowData(ControlGraphicsData winData)
        {
            ControlGraphicsData = winData;
        }

        internal void UnfocusControls()
        {
            int count = _controls.Count();
            for (int i = 0; i < count; i++)
            {
                _controls.ElementAt(i).Value.Unfocus();
            }
        }

        public void AddControl(Control c)
        {
            c.OnZOrderChanged += C_OnZOrderChanged;
            _controls.Add(c.Name, c);
        }

        private void C_OnZOrderChanged(object sender, ControlEventArgs e)
        {
            for(int i = 0; i < _controls.Count(); i++)
            {
                Control ctrl = _controls.ElementAt(i).Value;
                if (ctrl != e.Control)
                {
                    ctrl.InternalZOrder -= 0.1f;
                }
            }
        }

        public void RemoveControl(Control c)
        {
            if (_controls.ContainsValue(c))
            {
                c.OnZOrderChanged -= C_OnZOrderChanged;
                _controls.Remove(c.Name);
            }
        }

        public virtual T GetControl<T>(string controlID) where T : Control
        {
            T retCtrl = null;

            for (int i = 0; i < _controls.Count(); i++)
            {
                var child = _controls.ElementAt(i).Value;

                if (child.Name == controlID)
                {
                    retCtrl = (T)child;
                    break;
                }
            }

            return retCtrl;
        }

        public virtual Control GetControl(string controlID)
        {
            Control retCtrl = null;

            for (int i = 0; i < _controls.Count(); i++)
            {
                if (_controls.ElementAt(i).Value.Name == controlID)
                {
                    retCtrl = _controls.ElementAt(i).Value;
                    break;
                }
            }

            return retCtrl;
        }

        public List<T> GetControls<T>() where T : Control
        {
            List<T> retList = new List<T>();

            for (int i = 0; i < _controls.Count(); i++)
            {
                if(_controls.ElementAt(i).Value is T)
                {
                    retList.Add((T)_controls.ElementAt(i).Value);
                }
            }

            return retList;
        }

        public UIManager(Monitor monitor)
        {
            Monitor = monitor;
            _controls = new Dictionary<string, Control>();
            ActiveInstance = this;
        }

        public void LoadContent(ContentManager Content)
        {
            Texture = Content.Load<Texture2D>("DefaultWindowTemplate");
        }

        public void Update(GameTime gameTime)
        {
            for(int i = 0; i < _controls.Count(); i++)
            {
                _controls.ElementAt(i).Value.Update(gameTime);
            }
        }

        /// <summary>
        /// Draws the UI, Make this a seperate call from your in-game objects.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw Windows as a priority.
            // The reasoning is to make sure everything is drawn properly.
            var windows = ActiveInstance.GetControls<Window>();

            spriteBatch.Begin(SpriteSortMode.FrontToBack);
            for (int i = 0; i < _controls.Count(); i++)
            {
                _controls.ElementAt(i).Value.Draw(spriteBatch);
            }
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.FrontToBack);
            for (int i = 0; i < _controls.Count(); i++)
            {
                _controls.ElementAt(i).Value.Draw(spriteBatch);
            }
            spriteBatch.End();
        }
    }
}
