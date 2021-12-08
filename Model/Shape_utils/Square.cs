using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris_WinForms.Shape_utils
{
    class Square : Shape
    {
        public Square(Position pos, Coord coord, Coord bound, int colorCode) : base(pos, coord, bound, colorCode)
        {
        }
        public override void Replace(Coord[] newCoords, Position newPos)
        {
            switch (newPos)
            {
                case Position.SOUTH:
                    newCoords[1] = new Coord(newCoords[0].X + 1, newCoords[0].Y);
                    newCoords[2] = new Coord(newCoords[0].X, newCoords[0].Y + 1);
                    newCoords[3] = new Coord(newCoords[0].X + 1, newCoords[0].Y + 1);
                    break;
                case Position.WEST:
                    newCoords[1] = new Coord(newCoords[0].X, newCoords[0].Y + 1);
                    newCoords[2] = new Coord(newCoords[0].X - 1, newCoords[0].Y);
                    newCoords[3] = new Coord(newCoords[0].X - 1, newCoords[0].Y + 1);
                    break;
                case Position.NORTH:
                    newCoords[1] = new Coord(newCoords[0].X - 1, newCoords[0].Y);
                    newCoords[2] = new Coord(newCoords[0].X, newCoords[0].Y - 1);
                    newCoords[3] = new Coord(newCoords[0].X - 1, newCoords[0].Y - 1);
                    break;
                case Position.EAST:
                    newCoords[1] = new Coord(newCoords[0].X, newCoords[0].Y - 1);
                    newCoords[2] = new Coord(newCoords[0].X + 1, newCoords[0].Y);
                    newCoords[3] = new Coord(newCoords[0].X + 1, newCoords[0].Y - 1);
                    break;
                default:
                    break;
            }

            OnDrawn(new Shape_events.DrawnEventArgs(newCoords, Position != newPos));
        }
    }
}
