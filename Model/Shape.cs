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
        SOUTH, WEST, NORTH, EAST, UNDEFINED
    }

    enum Direction
    {
        LEFT, RIGHT, DOWN
    }

    abstract class Shape
    {
        public Shape(Coord bound, int colorCode)
        {
            TempCoords = new Coord[4];

            coordBound = bound;
            ColorCode = colorCode;
        }

        public bool Init(Position pos, Coord coord)
        {
            TempCoords[0] = coord;
            Replace(pos, true);
            if (!OutOfBounds())
            {

                Position = pos;
                Coordinates = new List<Coord>(TempCoords);
                return true;
            }

            return false;
        }

        public event EventHandler<DrawnEventArgs> Drawn;
        public Position Position { get; set; }
        public List<Coord> Coordinates { get; private set; }
        public int ColorCode { get; set; }
        private Coord coordBound;
        public Coord[] TempCoords { get; private set; }

        public void Rotate()
        {
            Position newPos = this.Position == Position.EAST ? Position.SOUTH : this.Position + 1;
            Coordinates.CopyTo(TempCoords);
            Replace(newPos);
        }

        public void Move(Direction dir)
        {
            Coordinates.CopyTo(TempCoords);
            if (dir == Direction.DOWN)
            {
                TempCoords[0] = new Coord(Coordinates[0].X, Coordinates[0].Y + 1);
            }
            else
            {
                TempCoords[0] = new Coord(dir == Direction.LEFT ? Coordinates[0].X - 1 : Coordinates[0].X + 1,
                                            Coordinates[0].Y);
            }

            Replace(Position.UNDEFINED);
        }

        public void OnDrawn(DrawnEventArgs e, bool dropMode = false)
        {
            if (!OutOfBounds()) Drawn?.Invoke(this, e);
        }

        public bool OutOfBounds()
        {
            return TempCoords.Any<Coord>(p1 => p1.OutOfBounds(coordBound));
        }

        public static bool OverFlown(Coord[] newCoords)
        {
            return newCoords.Any<Coord>(p1 => p1.Y == 0);
        }

        public abstract Coord[] Replace(Position newPos, bool coldStart = false);
    }
}
