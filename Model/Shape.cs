using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris_WinForms.Shape_events;

namespace Tetris_WinForms
{
    enum Position
    {
        SOUTH, WEST, NORTH, EAST
    }

    enum Direction
    {
        LEFT, RIGHT
    }

    struct Coord
    {
        public Coord(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public int x { get; set; }
        public int y { get; set; }
    }

    abstract class Shape
    {
        public Shape(Position pos, Coord coord)
        {
            Coordinates = new Coord[4];
            Coordinates[0] = coord;
            Position = pos;
            Redraw();
        }
        public event EventHandler<DrawnEventArgs> Drawn;
        public Position Position { get; set; }
        public Coord[] Coordinates { get; set; }

        public void Rotate() { 
            this.Position = this.Position == Position.EAST ? Position.SOUTH : this.Position + 1;
            Redraw();
        }

        public void Move(Direction dir)
        {
            Coordinates[0].x = dir == Direction.LEFT ? Coordinates[0].x - 1 : Coordinates[0].x + 1;
            Redraw();
        }

        public void OnDrawn(DrawnEventArgs e)
        {
            Drawn?.Invoke(this, e);
        }

        

        public abstract void Redraw();
    }
}
