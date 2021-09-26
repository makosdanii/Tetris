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
        public Model(int size)
        {
            Shapes = new List<Shape>();
            _rand = new Random();
            this.Size = size;
        }

        private Random _rand;
        private static int SHAPECOUNT = Enum.GetValues(typeof(ShapeTag)).Length;
        private static int POSCOUNT = Enum.GetValues(typeof(Position)).Length;


        public int Size { get; private set; }
        public event EventHandler<DrawnEventArgs> Drawn;
        public List<Shape> Shapes { get; set; }

        public void AddShape()
        {
            ShapeTag shapeTag = (ShapeTag)(_rand.Next() % SHAPECOUNT);
            try
            {
                switch (shapeTag)
                {
                    case ShapeTag.SQUARE:
                        Shapes.Add(new Square((Position)(_rand.Next() % POSCOUNT), new Coord(_rand.Next() % Size, 1)));
                        break;
                    case ShapeTag.ROOF:
                        Shapes.Add(new Roof((Position)(_rand.Next() % 4), new Coord(_rand.Next() % Size, 1)));
                        break;
                    case ShapeTag.ROMBUS:
                        Shapes.Add(new Rombus((Position)(_rand.Next() % 4), new Coord(_rand.Next() % Size, 1)));
                        break;
                    case ShapeTag.LSHAPE:
                        Shapes.Add(new LShape((Position)(_rand.Next() % 4), new Coord(_rand.Next() % Size, 1)));
                        break;
                    case ShapeTag.LINE:
                        Shapes.Add(new Line((Position)(_rand.Next() % 4), new Coord(_rand.Next() % Size, 1)));
                        break;
                    default:
                        break;
                }

                Shapes[^1].Drawn += Model_Drawn;
                Drawn?.Invoke(this, new DrawnEventArgs(Shapes.Count - 1));

            }
            catch(Exception e){
                
                System.Diagnostics.Debug.WriteLine("Problem at creating shape");
            }
        }

        private void Model_Drawn(object sender, DrawnEventArgs e)
        {
            Drawn?.Invoke(this, new DrawnEventArgs(Shapes.Count-1));
        }
    }
}
