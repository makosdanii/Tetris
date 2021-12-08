using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris_WPF.Shape_events;

namespace Tetris_WPF
{
    public enum Position
    {
        SOUTH, WEST, NORTH, EAST, UNDEFINED
    }

    public enum Direction
    {
        LEFT, RIGHT, DOWN
    }

    public abstract class Shape
    {
        public Shape(List<Coord> coords, Coord bound, int colorCode)
        {
            Coordinates = coords;
            coordBound = bound;
            ColorCode = colorCode;
        }

        public Shape(Coord bound, int colorCode)
        {
            coordBound = bound;
            ColorCode = colorCode;
        }

        public bool Init(Position pos, Coord coord)
        {
            TempCoords = new Coord[4];
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
            TempCoords = new Coord[Coordinates.Count];
            Coordinates.CopyTo(TempCoords);

            Position newPos = this.Position == Position.EAST ? Position.SOUTH : this.Position + 1;
            Replace(newPos);
        }

        public void Move(Direction dir)
        {
            TempCoords = new Coord[Coordinates.Count];
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

        public Direction Direction
        {
            get => default;
            set
            {
            }
        }

        public DrawnEventArgs DrawnEventArgs
        {
            get => default;
            set
            {
            }
        }
    }
}
