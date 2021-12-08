using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris_WPF.Shape_utils
{
    class Polymorph : Shape
    {
        public Polymorph(List<Coord> coords, Coord bound, int colorCode) : base(coords, bound, colorCode)
        {
        }

        public override Coord[] Replace(Position newPos, bool coldStart = false)
        {
            if (newPos != Position.UNDEFINED) throw new NotImplementedException();

            for (int i = 0; i<Coordinates.Count; i++)
            {
                TempCoords[i] = new Coord(Coordinates[i].X, Coordinates[i].Y + 1);
            }

            OnDrawn(new Shape_events.DrawnEventArgs(Coordinates[0].Y != TempCoords[0].Y, newPos));
            return TempCoords;
        }
    }
}
