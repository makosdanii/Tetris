﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris_WPF.Shape_events
{
    public class BlowEventArgs : EventArgs
    {
        public int Index { get; set; }

        public BlowEventArgs(int index)
        {
            this.Index = index;
        }
    }
}
