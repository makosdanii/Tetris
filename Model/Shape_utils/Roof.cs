using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris_WinForms.Shape_utils
{
    class Roof : Shape
    {
        public Roof(Coord coord, Coord bound, int colorCode) : base(bound, colorCode)
        {
        }
        public override Coord[] Replace(Position newPos, bool coldStart = false)
        {
            switch (newPos == Position.UNDEFINED ? Position : newPos)
            {
                case Position.SOUTH:
                    TempCoords[1] = new Coord(TempCoords[0].X, TempCoords[0].Y + 1);
                    TempCoords[2] = new Coord(TempCoords[0].X + 1, TempCoords[0].Y + 1);
                    TempCoords[3] = new Coord(TempCoords[0].X - 1, TempCoords[0].Y + 1);
                    break;
                case Position.WEST:
                    TempCoords[1] = new Coord(TempCoords[0].X - 1, TempCoords[0].Y);
                    TempCoords[2] = new Coord(TempCoords[0].X - 1, TempCoords[0].Y + 1);
                    TempCoords[3] = new Coord(TempCoords[0].X - 1, TempCoords[0].Y - 1);
                    break;
                case Position.NORTH:
                    TempCoords[1] = new Coord(TempCoords[0].X, TempCoords[0].Y - 1);
                    TempCoords[2] = new Coord(TempCoords[0].X - 1, TempCoords[0].Y - 1);
                    TempCoords[3] = new Coord(TempCoords[0].X + 1, TempCoords[0].Y - 1);
                    break;
                case Position.EAST:
                    TempCoords[1] = new Coord(TempCoords[0].X + 1, TempCoords[0].Y);
                    TempCoords[2] = new Coord(TempCoords[0].X + 1, TempCoords[0].Y - 1);
                    TempCoords[3] = new Coord(TempCoords[0].X + 1, TempCoords[0].Y + 1);
                    break;
                default:
                    break;

            }

            if (coldStart) return TempCoords;


            OnDrawn(new Shape_events.DrawnEventArgs(Coordinates[0].Y != TempCoords[0].Y, newPos));
            return TempCoords;
        }
    }
}
