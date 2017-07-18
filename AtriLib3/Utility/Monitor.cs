using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Forms;

namespace AtriLib3.Utility
{
    public enum Resolutions
    {
        R320x200,
        R640x480,
        R800x600,
        R1024x768,
        R1280x720,
        R1280x768,
        R1280x800,
        R1280x960,
        R1280x1024,
        R1360x768,
        R1366x768,
        R1440x900,
        R1536x864,
        R1600x900,
        R1600x1200,
        R1680x1050,
        R1920x1080,
        R1920x1200,
        R2560x1080,
        R2560x1440
    };

    public static class GameWindowExtensions
    {
        public static void SetPosition(this GameWindow gameWindow, Point position)
        {
            OpenTK.GameWindow window = GetForm(gameWindow);

            if(window != null)
            {
                window.X = position.X;
                window.Y = position.Y;
            }
        }

        public static OpenTK.GameWindow GetForm(this GameWindow gameWindow)
        {
            Type type = typeof(OpenTK.GameWindow);
            System.Reflection.FieldInfo field = type.GetField("window", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if(field != null)
            {
                return field.GetValue(gameWindow) as OpenTK.GameWindow;
            }

            return null;
        }
    }

    public class Monitor
    {
        private Resolutions _resolution;
        private Rectangle _screenRect;
        private int _virtualWidth;
        private int _virtualHeight;
        private bool _virtualMatrix;
        private Matrix _scaleMatrix;

        public float VirtualScale { get; set; } = 1.0f;

        public GraphicsDeviceManager GfxDeviceManager { get; set; }

        private bool useGL;

        public bool UseOpenGL
        {
            get { return useGL; }
            set { useGL = value; }
        }

        public Resolutions Resolution
        {
            get { return _resolution; }
            private set { _resolution = value; }
        }

        public Viewport Viewport
        {
            get
            {
                return GfxDeviceManager.GraphicsDevice.Viewport;
            }
            set
            {
                // TODO: Change this?
                GfxDeviceManager.GraphicsDevice.Viewport = value;
            }
        }

        public Rectangle ScreenRectangle
        {
            get
            {
                return _screenRect;
            }
        }

        public bool Fullscreen
        {
            get
            {
                return GfxDeviceManager.GraphicsDevice.PresentationParameters.IsFullScreen;
            }
            set
            {
                GfxDeviceManager.GraphicsDevice.PresentationParameters.IsFullScreen = value;
            }
        }

        public void CenterToScreen(Game game)
        {
            System.Drawing.Rectangle r = Screen.PrimaryScreen.Bounds;

            int x = (r.Width / 2) - (Width / 2);
            int y = (r.Height / 2) - (Height / 2);

            game.Window.Position = new Point(x, y);
        }

        public int Width
        {
            get
            {
                return _screenRect.Width;
            }
            private set
            {
                _screenRect.Width = value;
            }
        }

        public int VirtualWidth
        {
            get { return _virtualWidth; }
            set { _virtualWidth = value; }
        }

        public int VirtualHeight
        {
            get { return _virtualHeight; }
            set { _virtualHeight = value; }
        }

        public Matrix ScaleMatrix
        {
            get { return _scaleMatrix; }
            private set { _scaleMatrix = value; }
        }

        public bool VirtualMatrix
        {
            get { return _virtualMatrix; }
            set { _virtualMatrix = value; }
        }

        public void SetVirtualResolution(int Width, int Height)
        {
            _virtualWidth = Width;
            _virtualHeight = Height;
            _virtualMatrix = true;
        }

        public int Height
        {
            get
            {
                return _screenRect.Height;
            }
            private set
            {
                _screenRect.Height = value;
            }
        }

        /// <summary>
        /// Get the Window Center Point
        /// </summary>
        public Vector2 Origin
        {
            get
            {
                return new Vector2(_screenRect.Width / 2, _screenRect.Height / 2);
            }
        }

        /// <summary>
        /// Get the Window Center Point on the X axis.
        /// </summary>
        public float OriginX
        {
            get
            {
                return _screenRect.Width / 2;
            }
        }

        /// <summary>
        /// Get the Window Center Point on the Y axis.
        /// </summary>
        public float OriginY
        {
            get
            {
                return _screenRect.Height / 2;
            }
        }

        public Monitor(GraphicsDeviceManager gfxDevMgr)
        {
            GfxDeviceManager = gfxDevMgr;

            // Set up out virtual matrix to 800 x 600,
            // so no matter what resolution you will run, we will scale everything up to
            // 800 by 600.
            VirtualWidth = 800;
            VirtualHeight = 600;
            VirtualMatrix = true;
        }

        private void CreateScaleMatrix()
        {
            VirtualMatrix = false;

            ScaleMatrix = Matrix.CreateScale((float)Viewport.Width / VirtualWidth,
                (float)Viewport.Height / VirtualHeight, VirtualScale);
        }

        public Matrix GetTransformationMatrix()
        {
            if(VirtualMatrix)
            {
                CreateScaleMatrix();
            }

            return ScaleMatrix;
        }

        public void ResetViewport()
        {
            float targetAspectRatio = GetVirtualAspectRatio();
            int width = Width;
            int height = (int)(width / targetAspectRatio * 0.5f);
            bool changed = false;

            if(height > GfxDeviceManager.PreferredBackBufferHeight)
            {
                height = GfxDeviceManager.PreferredBackBufferHeight;
                width = (int)(height * targetAspectRatio * 0.5f);
                changed = true;
            }

            Viewport vp = new Viewport();
            vp.X = (GfxDeviceManager.PreferredBackBufferWidth / 2) - (width / 2);
            vp.Y = (GfxDeviceManager.PreferredBackBufferHeight / 2) - (height / 2);
            vp.Width = width;
            vp.Height = height;
            vp.MinDepth = 0;
            vp.MaxDepth = 1;

            if(changed)
            {
                VirtualMatrix = true;
            }

            GfxDeviceManager.GraphicsDevice.Viewport = vp;
        }

        public void FullViewport()
        {
            Viewport vp = new Viewport();
            vp.X = 0;
            vp.Y = 0;
            vp.Width = Width;
            vp.Height = Height;
            GfxDeviceManager.GraphicsDevice.Viewport = vp;
        }

        public float GetVirtualAspectRatio()
        {
            return VirtualWidth / VirtualHeight;
        }

        public void SetResolution(Resolutions resolution)
        {
            Resolution = resolution;
            ApplySettings();
        }

        public void SetResolution(Resolutions resolution, bool fullScreen)
        {
            Resolution = resolution;
            GfxDeviceManager.GraphicsDevice.PresentationParameters.IsFullScreen = fullScreen;

            ApplySettings();
        }

        public void SetResolutionAndVirtual(Resolutions resolution, bool fullScreen)
        {
            Resolution = resolution;
            GfxDeviceManager.GraphicsDevice.PresentationParameters.IsFullScreen = fullScreen;

            ApplySettings(true);
        }

        public void SetResolutionAndVirtual(Resolutions resolution)
        {
            Resolution = resolution;
            ApplySettings(true);
        }

        public void SetCustomResolution(int width, int height)
        {
            GfxDeviceManager.PreferredBackBufferWidth = width;
            GfxDeviceManager.PreferredBackBufferHeight = height;
            GfxDeviceManager.ApplyChanges();
            _screenRect = new Rectangle(0, 0, width, height);
            Width = width;
            Height = Height;
        }

        public void SetCustomResolution(int width, int height, bool fullScreen)
        {
            GfxDeviceManager.PreferredBackBufferWidth = width;
            GfxDeviceManager.PreferredBackBufferHeight = height;
            GfxDeviceManager.GraphicsDevice.PresentationParameters.IsFullScreen = fullScreen;
            GfxDeviceManager.ApplyChanges();
            _screenRect = new Rectangle(0, 0, width, height);
            Width = width;
            Height = Height;
        }

        private void ApplySettings(bool incVR = false)
        {
            // TODO: Poll for supported resolutions.

            switch(Resolution)
            {
                case Resolutions.R320x200:
                    Width = 320;
                    Height = 200;
                    break;
                case Resolutions.R640x480:
                    Width = 640;
                    Height = 480;
                    break;
                case Resolutions.R800x600:
                    Width = 800;
                    Height = 600;
                    break;
                case Resolutions.R1024x768:
                    Width = 1024;
                    Height = 768;
                    break;
                case Resolutions.R1280x1024:
                    Width = 1280;
                    Height = 1024;
                    break;
                case Resolutions.R1280x720:
                    Width = 1280;
                    Height = 720;
                    break;
                case Resolutions.R1280x768:
                    Width = 1280;
                    Height = 768;
                    break;
                case Resolutions.R1280x800:
                    Width = 1280;
                    Height = 800;
                    break;
                case Resolutions.R1280x960:
                    Width = 1280;
                    Height = 960;
                    break;
                case Resolutions.R1360x768:
                    Width = 1360;
                    Height = 768;
                    break;
                case Resolutions.R1366x768:
                    Width = 1366;
                    Height = 768;
                    break;
                case Resolutions.R1440x900:
                    Width = 1440;
                    Height = 900;
                    break;
                case Resolutions.R1536x864:
                    Width = 1536;
                    Height = 864;
                    break;
                case Resolutions.R1600x1200:
                    Width = 1600;
                    Height = 1200;
                    break;
                case Resolutions.R1600x900:
                    Width = 1600;
                    Height = 900;
                    break;
                case Resolutions.R1680x1050:
                    Width = 1680;
                    Height = 1050;
                    break;
                case Resolutions.R1920x1080:
                    Width = 1920;
                    Height = 1080;
                    break;
                case Resolutions.R1920x1200:
                    Width = 1920;
                    Height = 1200;
                    break;
                case Resolutions.R2560x1080:
                    Width = 2560;
                    Height = 1080;
                    break;
                case Resolutions.R2560x1440:
                    Width = 2560;
                    Height = 1440;
                    break;
                default:
                    Width = 800;
                    Height = 600;
                    break;
            }

            if(Fullscreen)
            {
                // This should prevent from applying fullscreen to an unsupported display mode...
                foreach (var dm in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
                {
                    if (dm.Width == Width && dm.Height == Height)
                    {
                        GfxDeviceManager.PreferredBackBufferWidth = Width;
                        GfxDeviceManager.PreferredBackBufferHeight = Height;
                        GfxDeviceManager.ApplyChanges();
                    }
                }
            }
            else
            {
                // TODO: Set resolution rules...
                // Do we want to be able to set a resolution greater than the desktop resolution?
                GfxDeviceManager.PreferredBackBufferWidth = Width;
                GfxDeviceManager.PreferredBackBufferHeight = Height;
                GfxDeviceManager.ApplyChanges();
            }

            if(incVR)
            {
                SetVirtualResolution(Width, Height);
            }
        }
    }
}
