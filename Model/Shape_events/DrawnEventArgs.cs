using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris_WinForms.Shape_events
{
    class DrawnEventArgs : EventArgs
    {
        public Coord[] Coords { get; private set; }
        public bool Rotated { get; set; }
        public DrawnEventArgs(Coord[] coords = null, bool rotated = false)
        {
            this.Coords = coords;
            this.Rotated = rotated;
        }
    }
}
