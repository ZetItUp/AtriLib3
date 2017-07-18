using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AtriLib3.Utility
{
    public class Camera2D
    {
        private Matrix _customMatrix;
        private Matrix _transform;
        private Matrix _inverseTransform;
        private Vector2 _position;
        private Viewport _viewport;
        private Monitor _monitor;

        public float MaxZoom { get; set; } = 10.0f;
        public float LevelWidth { get; set; }
        public float LevelHeight { get; set; }
        public float Rotation { get; set; }

        private float _zoom;
        public float Zoom
        {
            get { return _zoom; }
            set
            {
                _zoom = value;

                if(_zoom < 0.0f)
                {
                    _zoom = 0.0f;
                }

                if(_zoom > MaxZoom)
                {
                    _zoom = MaxZoom;
                }
            }
        }

        private bool _useCustomMatrix = false;

        /// <summary>
        /// Get the Cameras Screen Rectangle
        /// </summary>
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, _viewport.Width, _viewport.Height);
            }
        }

        /// <summary>
        /// Get the Camera2D Matrix Transform
        /// </summary>
        public Matrix Transform
        {
            get { return _transform; }
        }

        /// <summary>
        /// Get the Camera2D Inverse Matrix Transform
        /// </summary>
        public Matrix InverseTransform
        {
            get { return _inverseTransform; }
        }

        /// <summary>
        /// Get or Set the Camera2D Position
        /// </summary>
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public Camera2D(Monitor monitor)
        {
            _viewport = monitor.Viewport;
            _monitor = monitor;
            LevelWidth = 800;
            LevelHeight = 600;
            Rotation = 0f;
            Zoom = 1f;
        }

        public Camera2D(Monitor monitor, int levelWidth, int levelHeight)
        {
            _viewport = monitor.Viewport;
            _monitor = monitor;
            Rotation = 0f;
            Zoom = 1f;
            LevelWidth = levelWidth;
            LevelHeight = levelHeight;
        }

        public Vector2 GetMousePosition(Vector2 mousePos)
        {
            return Vector2.Transform(mousePos, InverseTransform);
        }

        /// <summary>
        /// Sets a Custom Matrix to the Camera2D Matrix
        /// </summary>
        /// <param name="customMatrix">Custom Matrix</param>
        public void SetCustomMatrix(Matrix customMatrix)
        {
            _customMatrix = customMatrix;
            _useCustomMatrix = true;
        }

        /// <summary>
        /// Unsets the Custom Matrix which has been set
        /// </summary>
        public void UnsetCustomMatrix()
        {
            _useCustomMatrix = false;
        }

        public void Update(GameTime gameTime)
        {
            if(Position.X < 0)
            {
                _position.X = 0;
            }
            if(Position.Y < 0)
            {
                _position.Y = 0;
            }
            if(Position.X >= LevelWidth - _monitor.VirtualWidth / Zoom)
            {
                _position.X = LevelWidth - _monitor.VirtualWidth / Zoom;
            }
            if(Position.Y >= LevelHeight - _monitor.VirtualHeight / Zoom)
            {
                _position.Y = LevelHeight - _monitor.VirtualHeight / Zoom;
            }

            if (_useCustomMatrix)
            {
                _transform = Matrix.CreateTranslation(-Position.X, -Position.Y, 0) *
                    Matrix.CreateRotationZ(Rotation) *
                    _customMatrix *
                    Matrix.CreateScale(new Vector3(Zoom, Zoom, 1.0f));
            }
            else
            {
                _transform = Matrix.CreateTranslation(-Position.X, -Position.Y, 0) *
                    Matrix.CreateRotationZ(Rotation) *
                    Matrix.CreateScale(new Vector3(Zoom, Zoom, 1.0f));
            }

            _inverseTransform = Matrix.Invert(Transform);
        }

        /// <summary>
        /// Centers the Camera at an objects position, ALWAYS call this before Update()
        /// </summary>
        /// <param name="position"></param>
        public void CenterToPosition(Vector2 position)
        {
            Position = new Vector2(position.X - (_monitor.VirtualWidth / 2) / Zoom, position.Y - (_monitor.VirtualHeight / 2) / Zoom);
        }
    }
}
