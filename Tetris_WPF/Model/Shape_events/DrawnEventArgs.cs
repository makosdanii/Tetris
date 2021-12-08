using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris_WPF.Shape_events
{
    public class DrawnEventArgs : EventArgs
    {
        public Position Position { get; private set; }
        public bool Falling { get; private set; }

        public int LostNo { get; private set; }
        public int ShapeNo { get; private set; }
        public DrawnEventArgs(int shapeNo, int lostNo = -1) { ShapeNo = shapeNo; LostNo = lostNo; }
        public DrawnEventArgs(bool falling, Position position = Position.UNDEFINED)
        {
            this.Position = position;
            this.Falling = falling;
        }
    }
}
