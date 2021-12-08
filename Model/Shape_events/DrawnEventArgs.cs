using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris_WinForms.Shape_events
{
    class DrawnEventArgs : EventArgs
    {
        public int Index { get; set; }

        public DrawnEventArgs()
        {
            Index = -1;
        }

        public DrawnEventArgs(int index)
        {
            this.Index = index;
        }
    }
}
