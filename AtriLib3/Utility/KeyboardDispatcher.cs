using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Windows;

namespace Microsoft.Xna.Framework.Input
{
    public interface IKeyboardSubscriber
    {
        void RecieveTextInput(char OutputChar);
        void RecieveTextInput(string text);
        void RecieveTextInput(char OutputChar, CharacterEventArgs e);
        void RecieveCommandInput(char command);
        void RecieveSpecialInput(Keys key);
        void RecieveSpecialInput(Keys key, KeyEventArgs e);
        void RecieveInput(KeyEventArgs e);
        bool Selected { get; set; }
    }

    public class KeyboardDispatcher
    {
        public KeyboardDispatcher(GameWindow window)
        {
            EventInput.Initialize(window);
            EventInput.CharEntered += new CharEnteredHandler(EventInput_CharEntered);
            EventInput.KeyDown += new KeyEventHandler(EventInput_KeyDown);
        }

        void EventInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (_sub == null)
                return;

            _sub.RecieveSpecialInput(e.KeyCode, e);
        }

        void EventInput_CharEntered(object sender, CharacterEventArgs e)
        {
            if (_sub == null)
                return;

            if (char.IsControl(e.Character))
            {
                // Ctrl + V
                if (e.Character == 0x16)
                {
                    Thread thread = new Thread(PasteThread);
                    thread.SetApartmentState(ApartmentState.STA);
                    thread.Start();
                    thread.Join();
                    _sub.RecieveTextInput(_pasteResult);
                }
                else
                {
                    _sub.RecieveCommandInput(e.Character);
                }
            }
            else
            {
                _sub.RecieveTextInput(e.Character);
            }
        }

        IKeyboardSubscriber _sub;
        public IKeyboardSubscriber Subscriber
        {
            get { return _sub; }
            set
            {
                if (_sub != null)
                    _sub.Selected = false;
                _sub = value;
                if (value != null)
                    value.Selected = true;
            }
        }

        string _pasteResult = "";
        [STAThread]
        void PasteThread()
        {
            if (Clipboard.ContainsText())
            {
                _pasteResult = Clipboard.GetText();
            }
            else
            {
                _pasteResult = "";
            }
        }
    }
}
