using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AtriLib3.Utility;
using AtriLib3.Interfaces;

namespace AtriLib3.UI
{
    public abstract class Control
    {
        private bool _previousMouseOver = false;

        public string Name
        {
            get;
            set;
        }

        protected abstract void SubscribeToEvents();
        protected abstract void UnsubscribeToEvents();
        internal event EventHandler<ControlEventArgs> OnZOrderChanged;
        internal event EventHandler<ControlEventArgs> OnParentRectangleChange;
        public event EventHandler<ControlEventArgs> OnMouseClicked;
        public event EventHandler<ControlEventArgs> OnMouseEnter;
        public event EventHandler<ControlEventArgs> OnMouseLeave;
        public event EventHandler<ControlEventArgs> OnMouseDown;
        public event EventHandler<ControlEventArgs> OnMouseUp;

        
        private Dictionary<string, Control> children;
        public Control Parent { get; set; }
        private float _zOrder = 0.9f;
        public bool Visible { get; set; } = true;
        protected Rectangle _rect;
        private Vector2 Position = Vector2.Zero;
        public bool CanMove { get; protected set; } = true;

        private bool _hasFocus;

        public bool Focus
        {
            get
            {
                return _hasFocus;
            }
            set
            {
                if (value == true)
                {
                    UIManager.ActiveInstance.UnfocusControls();
                }

                _hasFocus = value;
            }
        }

        internal void Unfocus()
        {
            _hasFocus = false;

            int count = children.Count();
            for(int i = 0; i < count; i++)
            {
                children.ElementAt(i).Value.Unfocus();
            }
        }

        public virtual Rectangle Rectangle
        {
            get
            {
                return _rect;
            }
            set
            {
                // Rectangle Changed, Trigger Event!
                bool doChange = false;
                if (_rect != value)
                {
                    doChange = true;
                }

                _rect = value;
                Position = new Vector2(_rect.X, _rect.Y);
                if(doChange)
                {
                    OnParentRectangleChange?.Invoke(this, new ControlEventArgs(this));
                }
            }
        }

        protected virtual void SetPosition()
        {
            if (Parent != null)
            {
                _rect = new Rectangle((int)(Parent.Rectangle.X + Position.X), (int)(Parent.Rectangle.Y + Position.Y), _rect.Width, _rect.Height);
            }
        }

        public virtual Control GetControl(string controlID)
        {
            if(children.TryGetValue(controlID, out Control retCtrl))
            {
                return retCtrl;
            }
            else
            {
                return null;
            }
        }

        public virtual T GetControl<T>(string controlID) where T : Control
        {
            if (children.TryGetValue(controlID, out Control retCtrl))
            {
                return (T)retCtrl;
            }
            else
            {
                return null;
            }
        }

        public virtual List<T> GetChildren<T>() where T : Control
        {
            return children.OfType<T>().Where(ctrl => ctrl.Name == Name).ToList();
        }

        internal float InternalZOrder
        {
            get
            {
                return _zOrder;
            }
            set
            {
                if (value < 0.0f)
                {
                    _zOrder = 0.0f;
                }
                else if (value > 1.0f)
                {
                    _zOrder = 1.0f;
                }
                else
                {
                    _zOrder = value;
                }
            }
        }

        public float ZOrder
        {
            get
            {
                return _zOrder;
            }
            set
            {
                if (value < 0.0f)
                {
                    _zOrder = 0.0f;
                }
                else if (value > 1.0f)
                {
                    _zOrder = 1.0f;
                }
                else
                {
                    _zOrder = value;
                }

                OnZOrderChanged?.Invoke(this, new ControlEventArgs(this));
            }
        }

        public void AddControl(Control c)
        {
            children.Add(c.Name, c);
        }

        public void RemoveControl(Control c)
        {
            if (children.ContainsValue(c))
            {
                children.Remove(c.Name);
            }
        }

        public Control(string name, Rectangle controlRectangle)
        {
            children = new Dictionary<string, Control>();
            Rectangle = controlRectangle;
            Name = name;
        }

        public Control(string name, Control parentControl, Rectangle controlRectangle)
        {
            children = new Dictionary<string, Control>();
            Rectangle = controlRectangle;
            Name = name;
            Parent = parentControl;
            Parent.AddControl(this);
        }

        public virtual void LoadContent(ContentManager Content)
        {

        }

        public virtual void Update(GameTime gameTime)
        {
            // Do Mouse Intersection Stuff
            if (AMouse.MouseRectangle.Intersects(Rectangle))
            {
                if (Parent != null)
                {
                    Parent.CanMove = false;
                }

                if (_previousMouseOver == false)
                {
                    OnMouseEnter?.Invoke(this, new ControlEventArgs(this));
                    _previousMouseOver = true;
                }

                if (AMouse.MouseDown(AMouse.MouseButton.Left))
                {
                    OnMouseDown?.Invoke(this, new ControlEventArgs(this));
                }
                else
                {
                    if (Parent != null)
                    {
                        Parent.CanMove = true;
                    }
                }

                if (AMouse.MousePressed(AMouse.MouseButton.Left))
                {
                    OnMouseClicked?.Invoke(this, new ControlEventArgs(this));
                }
            }
            else
            {
                if(_previousMouseOver == true)
                {
                    OnMouseLeave?.Invoke(this, new ControlEventArgs(this));
                    _previousMouseOver = false;
                }
            }

            if (AMouse.MouseReleased(AMouse.MouseButton.Left))
            {
                if (Parent != null)
                {
                    Parent.CanMove = true;
                }
                else
                {
                    CanMove = true;
                }

                OnMouseUp?.Invoke(this, new ControlEventArgs(this));
            }

            for (int i = 0; i < children.Count(); i++)
            {
                children.ElementAt(i).Value.Update(gameTime);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible)
            {
                return;
            }
            
            for (int i = 0; i < children.Count(); i++)
            {
                children.ElementAt(i).Value.Draw(spriteBatch);
            }
        }
    }
}
