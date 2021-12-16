using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris_Android
{
    public class SingleIntEventArg : EventArgs
    {
        public int MyInt { get; set; }
        public SingleIntEventArg(int myInt)
        {
            MyInt = myInt;
        }
    }
}
