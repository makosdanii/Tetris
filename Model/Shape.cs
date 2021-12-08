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
        LEFT, RIGHT, DOWN
    }

    abstract class Shape
    {
        public Shape(Position pos, Coord coord, Coord bound, int colorCode)
        {
            Coordinates = new Coord[4];
            Coordinates[0] = coord;
            Position = pos;
            coordBound = bound;
            ColorCode = colorCode;
            Replace(Coordinates, Position);
        }
        public event EventHandler<DrawnEventArgs> Drawn;
        public Position Position { get; set; }
        public Coord[] Coordinates { get; set; }
        public int ColorCode { get; set; }
        private Coord coordBound;

        public void Rotate() {
            Coord[] newCoords = new Coord[4];
            Coordinates.CopyTo(newCoords, 0);

            Position newPos = this.Position == Position.EAST ? Position.SOUTH : this.Position + 1;

            Replace(newCoords, newPos);
        }

        public void Move(Direction dir)
        {
            Coord[] newCoords = new Coord[4];

            if (dir == Direction.DOWN) {
                newCoords[0] = new Coord(Coordinates[0].X, Coordinates[0].Y + 1);
            }else{
                newCoords[0] = new Coord(dir == Direction.LEFT ? Coordinates[0].X - 1 : Coordinates[0].X + 1,
                                            Coordinates[0].Y);
            }

            Replace(newCoords, this.Position);
        }

        public void OnDrawn(DrawnEventArgs e)
        {
            if (!OutOfBounds(e.Coords)) Drawn?.Invoke(this, e);
        }

        public bool OutOfBounds(Coord[] newCoords)
        {
            var  x = newCoords.Any<Coord>(p1 => p1.OutOfBounds(coordBound));
            return x;
        }

        public abstract void Replace(Coord[] newCoords, Position newPos);
    }
}
