using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris_WinForms.Shape_utils;
using Tetris_WinForms.Shape_events;

namespace Tetris_WinForms
{
    enum ShapeTag
    {
        SQUARE, ROOF, ROMBUS, LSHAPE, LINE
    }

    class Model
    {
        public Model(int width)
        {
            Shapes = new List<Shape>();
            _rand = new Random();
            this.Size = new Coord(width, LENGTH);
        }

        private Random _rand;
        private static int SHAPECOUNT = Enum.GetValues(typeof(ShapeTag)).Length;
        private static int POSCOUNT = Enum.GetValues(typeof(Position)).Length;
        private static int LENGTH = 16;
        private static int COLORS = 5;


        public Coord Size { get; private set; }
        public event EventHandler<DrawnEventArgs> Drawn;
        public List<Shape> Shapes { get; set; }

        public void AddShape()
        {
            ShapeTag shapeTag = (ShapeTag)(_rand.Next() % SHAPECOUNT);
            int colorCode = _rand.Next() % COLORS;
            Shape shape = null;

            do
            {
                switch (shapeTag)
                {
                    case ShapeTag.SQUARE:
                        shape = new Square((Position)(_rand.Next() % POSCOUNT), new Coord(_rand.Next() % Size.X, 0), Size, colorCode);
                        break;
                    case ShapeTag.ROOF:
                        shape = new Roof((Position)(_rand.Next() % POSCOUNT), new Coord(_rand.Next() % Size.X, 0), Size, colorCode);
                        break;
                    case ShapeTag.ROMBUS:
                        shape = new Rombus((Position)(_rand.Next() % POSCOUNT), new Coord(_rand.Next() % Size.X, 0), Size, colorCode);
                        break;
                    case ShapeTag.LSHAPE:
                        shape = new LShape((Position)(_rand.Next() % POSCOUNT), new Coord(_rand.Next() % Size.X, 0), Size, colorCode);
                        break;
                    case ShapeTag.LINE:
                        shape = new Line((Position)(_rand.Next() % POSCOUNT), new Coord(_rand.Next() % Size.X, 0), Size, colorCode);
                        break;
                    default:
                        break;
                }

            } while (shape.OutOfBounds(shape.Coordinates));

            Shapes.Add(shape);
            Shapes[^1].Drawn += Model_Drawn;
            Drawn?.Invoke(this, new DrawnEventArgs());
        }

        public bool validator(Coord[] newCoords)
        {
            for (int i = Shapes.Count -2; i >= 0; i--) //init this way, so as not to compare new shape with itself
            {
                foreach (Coord coord in Shapes[i].Coordinates) //compare each coordinates with any of the new shape's
                    if (newCoords.Any<Coord>(p1 => p1.Equals(coord))) return false;
            }
            
            return true;
        }

        private void Model_Drawn(object sender, DrawnEventArgs e)
        {

            if (validator(e.Coords))
            {

                if (e.Rotated)
                {
                    Shapes[^1].Position = Shapes[^1].Position == Position.EAST ? Position.SOUTH : Shapes[^1].Position + 1;
                }

                Shapes[^1].Coordinates = e.Coords;

                Drawn?.Invoke(this, new DrawnEventArgs());
            }
        }
    }
}
