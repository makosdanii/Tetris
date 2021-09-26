using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris_WinForms.Shape_utils
{
    class Square : Shape
    {
        public Square(Position pos, Coord coord) : base(pos, coord)
        {
        }
        public override void Redraw()
        {
            switch (this.Position)
            {
                case Position.SOUTH:
                    Coordinates[1] = new Coord(Coordinates[0].x + 1, Coordinates[0].y);
                    Coordinates[2] = new Coord(Coordinates[0].x, Coordinates[0].y + 1);
                    Coordinates[3] = new Coord(Coordinates[0].x + 1, Coordinates[0].y + 1);
                    break;
                case Position.WEST:
                    Coordinates[1] = new Coord(Coordinates[0].x, Coordinates[0].y + 1);
                    Coordinates[2] = new Coord(Coordinates[0].x - 1, Coordinates[0].y);
                    Coordinates[3] = new Coord(Coordinates[0].x - 1, Coordinates[0].y + 1);
                    break;
                case Position.NORTH:
                    Coordinates[1] = new Coord(Coordinates[0].x - 1, Coordinates[0].y);
                    Coordinates[2] = new Coord(Coordinates[0].x, Coordinates[0].y - 1);
                    Coordinates[3] = new Coord(Coordinates[0].x - 1, Coordinates[0].y - 1);
                    break;
                case Position.EAST:
                    Coordinates[1] = new Coord(Coordinates[0].x, Coordinates[0].y - 1);
                    Coordinates[2] = new Coord(Coordinates[0].x + 1, Coordinates[0].y);
                    Coordinates[3] = new Coord(Coordinates[0].x + 1, Coordinates[0].y - 1);
                    break;
                default:
                    break;
            }

            OnDrawn(new Shape_events.DrawnEventArgs());
        }
    }
}
