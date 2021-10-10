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
            shapesToBlow = new List<int>();
            rowComplete = new int[LENGTH];
            for (int i = 0; i < rowComplete.Length; i++) rowComplete[i] = 0;
            _rand = new Random();
            this.Size = new Coord(width, LENGTH);
        }

        private Random _rand;
        private int[] rowComplete;

        private static int SHAPECOUNT = 5;
        private static int POSCOUNT = 4;
        private static int LENGTH = 16;
        private static int COLORS = 5;


        public List<Shape> Shapes { get; private set; }
        public List<int> shapesToBlow { get; private set; }
        public Coord Size { get; private set; }

        public event EventHandler<EventArgs> Changed;
        public event EventHandler<EventArgs> GameLost;
        public event EventHandler<BlowEventArgs> Blow;

        public void AddShape()
        {
            ShapeTag shapeTag = (ShapeTag)(_rand.Next() % SHAPECOUNT);
            int colorCode = _rand.Next() % COLORS;

            Position position = Position.UNDEFINED; 
            Shape shape = null;

            switch (shapeTag)
            {
                case ShapeTag.SQUARE:
                    shape = new Square(new Coord(_rand.Next() % Size.X, 0), Size, colorCode);
                    break;
                case ShapeTag.ROOF:
                    shape = new Roof(new Coord(_rand.Next() % Size.X, 0), Size, colorCode);
                    break;
                case ShapeTag.ROMBUS:
                    shape = new Rombus(new Coord(_rand.Next() % Size.X, 0), Size, colorCode);
                    break;
                case ShapeTag.LSHAPE:
                    shape = new LShape(new Coord(_rand.Next() % Size.X, 0), Size, colorCode);
                    break;
                case ShapeTag.LINE:
                    shape = new Line(new Coord(_rand.Next() % Size.X, 0), Size, colorCode);
                    break;
                default:
                    break;
            }

            do
            {
                position = (Position)(_rand.Next() % POSCOUNT);
            } while (!shape.Init(position, new Coord(_rand.Next() % Size.X, 0)));

            Shapes.Add(shape);
            shape.Drawn += Model_Drawn;
            Model_Drawn(shape, new DrawnEventArgs(true, Position.UNDEFINED));
        }


        private bool validator(Coord[] newCoords)
        {
            if (!newCoords.All<Coord>(p1 => p1.Y < Size.Y)) return false;
            for (int i = Shapes.Count -2; i >= 0; i--) //init this way, so as not to compare new shape with itself
            {
                foreach (Coord coord in Shapes[i].Coordinates) //compare each coordinates with any of the new shape's
                    if (newCoords.Any<Coord>(p1 => p1.Equals(coord))) return false;
            }
            
            return true;
        }

        private void Drop(int index)
        {
            
            for (int i = 0; i < Shapes.Count; i++)
            {

                for (int j = 0; j < Shapes[i].Coordinates.Count; j++)
                {
                    if (Shapes[i].Coordinates[j].Y < index)
                    {
                        rowComplete[Shapes[i].Coordinates[j].Y]--;
                        Shapes[i].Coordinates[j].Y++;
                        rowComplete[Shapes[i].Coordinates[j].Y]++;
                    }
                }
            }
        }

        private void ClearRow(int index)
        {
            rowComplete[index] = 0;

            int cleared = 0;
            bool top = index < LENGTH / 2;

            //optimised for loop to achieve cleared faster, then return
            for (int i = top ? Shapes.Count - 1 : 0; i != (top ? -1 : Shapes.Count); i = (top ? i - 1 : i + 1))
            {
                if (cleared == Size.X) return;
                for (int j = Shapes[i].Coordinates.Count-1; j>= 0; j--)
                {
                    if (Shapes[i].Coordinates[j].Y == index)
                    {
                        Shapes[i].Coordinates.RemoveAt(j);
                        cleared++;
                    }
                }
            }
        }

        private void BlowRows()
        {
            if (!rowComplete.Any<int>(a => a == Size.X)) return;

            for (int i = 0; i<rowComplete.Length; i++)
            {
                if (rowComplete[i] == Size.X)
                {
                    Blow?.Invoke(this, new BlowEventArgs(i));
                    ClearRow(i);
                    Drop(i);
                }
            }
        }

        private void Model_Drawn(object sender, DrawnEventArgs e)
        {
            Shape shape = sender as Shape;
            if (shape == null) throw new InvalidCastException();

            if (validator(shape.TempCoords))
            {

                if (e.Position != Position.UNDEFINED)
                {
                    shape.Position = shape.Position == Position.EAST ? Position.SOUTH : shape.Position + 1;
                }

                for (int i = 0; i < shape.TempCoords.Length; i++)
                {
                    shape.Coordinates[i] = shape.TempCoords[i];
                    shape.TempCoords[i] = null;
                }

                Changed?.Invoke(this, new DrawnEventArgs());
            }
            else
            {
                if (e.Falling)
                {
                    if (Shape.OverFlown(shape.TempCoords))
                    {
                        GameLost?.Invoke(this, new EventArgs());
                        return;
                    }

                    
                    foreach (Coord coord in shape.Coordinates)
                    {
                        rowComplete[coord.Y] += 1;
                    }
                    
                    BlowRows();

                    Changed?.Invoke(this, new StuckEventArgs());
                    
                }
            }

        }
    }
}
