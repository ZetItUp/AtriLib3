using System;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Microsoft.Xna.Framework.Input
{
    public class KeyboardLayout
    {
        const uint KLF_ACTIVATE = 1;    // Activate the layout
        const int KL_NAMELENGTH = 9;
        const string LANG_EN_US = "00000409";   // More language supports? Maybe...

        [DllImport("user32.dll")]
        private static extern long LoadKeyboardLayout(string pwszKLID, uint Flags);

        [DllImport("user32.dll")]
        private static extern long GetKeyboardLayoutName(StringBuilder pwszKLID);

        public static string getName()
        {
            StringBuilder name = new StringBuilder(KL_NAMELENGTH);
            GetKeyboardLayoutName(name);

            return name.ToString();
        }
    }

    public class CharacterEventArgs : EventArgs
    {
        private readonly char character;
        private readonly int lParam;

        public CharacterEventArgs(char character, int lParam)
        {
            this.character = character;
            this.lParam = lParam;
        }

        public char Character
        {
            get { return character; }
        }

        public int Param
        {
            get { return lParam; }
        }

        public int RepeatCount
        {
            get { return lParam & 0xffff; }
        }

        public bool ExtendedKey
        {
            get { return (lParam & (1 << 24)) > 0; }
        }

        public bool AltPressed
        {
            get { return (lParam & (1 << 29)) > 0; }
        }

        public bool PreviousState
        {
            get { return (lParam & (1 << 30)) > 0; }
        }

        public bool TransitionState
        {
            get { return (lParam & (1 << 31)) > 0; }
        }
    }

    public class KeyEventArgs : EventArgs
    {
        private Keys keyCode;

        public KeyEventArgs(Keys keyCode)
        {
            this.keyCode = keyCode;
        }

        public Keys KeyCode
        {
            get { return keyCode; }
        }
    }

    public delegate void CharEnteredHandler(object sender, CharacterEventArgs e);
    public delegate void KeyEventHandler(object sender, KeyEventArgs e);

    public static class EventInput
    {
        public static event CharEnteredHandler CharEntered;
        public static event KeyEventHandler KeyDown;
        public static event KeyEventHandler KeyUp;

        delegate IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        static bool Initialized;
        static IntPtr prevWndProc;
        static WndProc hookProcDelegate;
        static IntPtr hIMC;

        // Win32 Constants
        const int GWL_WNDPROC = -4;
        const int WM_KEYDOWN = 0x100;
        const int WM_KEYUP = 0x101;
        const int WM_CHAR = 0x102;
        const int WM_IME_SETCONTEXT = 0x0281;
        const int WM_INPUTLANGCHANGE = 0x51;
        const int WM_GETDLGCODE = 0x87;
        const int WM_IME_COMPOSITION = 0x10f;
        const int DLGC_WANTALLKEYS = 4;

        // Win32 Functions
        [DllImport("Imm32.dll")]
        static extern IntPtr ImmGetContext(IntPtr hWnd);

        [DllImport("Imm32.dll")]
        static extern IntPtr ImmAssociateContext(IntPtr hWnd, IntPtr hIMC);

        [DllImport("User32.dll")]
        static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("User32.dll")]
        static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        public static void Initialize(GameWindow window)
        {
            if (Initialized)
                throw new InvalidOperationException("Text Initialize can only be called once!");

            hookProcDelegate = new WndProc(HookProc);
            prevWndProc = SetWindowLong(window.Handle, GWL_WNDPROC, (int)Marshal.GetFunctionPointerForDelegate(hookProcDelegate));

            hIMC = ImmGetContext(window.Handle);
            Initialized = true;
        }

        static IntPtr HookProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            IntPtr returnCode = CallWindowProc(prevWndProc, hWnd, msg, wParam, lParam);

            switch (msg)
            {
                case WM_GETDLGCODE:
                    returnCode = (IntPtr)(returnCode.ToInt32() | DLGC_WANTALLKEYS);
                    break;

                case WM_KEYDOWN:
                        KeyDown?.Invoke(null, new KeyEventArgs((Keys)wParam));
                    break;

                case WM_KEYUP:
                        KeyUp?.Invoke(null, new KeyEventArgs((Keys)wParam));
                    break;

                case WM_CHAR:
                        CharEntered?.Invoke(null, new CharacterEventArgs((char)wParam, lParam.ToInt32()));
                    break;

                case WM_IME_SETCONTEXT:
                    if (wParam.ToInt32() == 1)
                        ImmAssociateContext(hWnd, hIMC);
                    break;

                case WM_INPUTLANGCHANGE:
                    ImmAssociateContext(hWnd, hIMC);
                    returnCode = (IntPtr)1;
                    break;
            }

            return returnCode;
        }
    }
}
