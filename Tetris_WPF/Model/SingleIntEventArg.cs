using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris_WPF
{
    class SingleIntEventArg : EventArgs
    {
        public int MyInt { get; set; }
        public SingleIntEventArg(int myInt)
        {
            MyInt = myInt;
        }
    }
}
