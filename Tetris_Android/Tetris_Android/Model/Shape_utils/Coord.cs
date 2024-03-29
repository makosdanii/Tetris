﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris_Android
{
    public class Coord
    {
        public Coord(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
        public int X { get; set; }
        public int Y { get; set; }

        public override bool Equals(object coord)
        {
            return this.X == (coord as Coord)?.X && this.Y == (coord as Coord)?.Y;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public bool OutOfBounds(Coord bound)
        {

            return X < 0 || Y < 0 || X > bound.X - 1;

        }
    }
}
