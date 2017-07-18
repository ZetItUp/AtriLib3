using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtriLib3.UI
{
    public class ControlEventArgs : EventArgs
    {
        public Control Control { get; private set; }

        public ControlEventArgs(Control c)
        {
            Control = c;
        }
    }
}
