using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris_WPF
{
    public class SelectedEventAgrs : EventArgs
    {
        public int Index { get; private set; }
        public SelectedEventAgrs(int index)
        {
            this.Index = index;
        }
    }
}
