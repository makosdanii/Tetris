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
        public Position Position { get; private set; }
        public bool Falling { get; set; }
        public DrawnEventArgs() { }
        public DrawnEventArgs(bool falling, Position position = Position.UNDEFINED)
        {
            this.Position = position;
            this.Falling = falling;
        }
    }
}
