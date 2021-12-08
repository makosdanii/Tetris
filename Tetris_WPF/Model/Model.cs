using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris_WPF.Shape_utils;
using Tetris_WPF.Shape_events;

namespace Tetris_WPF
{
    enum ShapeTag
    {
        SQUARE, ROOF, ROMBUS, LSHAPE, LINE
    }

    public class Model
    {
        private IPersistence _persistence;
        private Random _rand;
        private string path;

        private static int SHAPECOUNT = 5;
        private static int POSCOUNT = 4;
        private static int LENGTH = 16;
        private static int COLORS = 5;

        private int[] rowComplete;
        public List<Shape> Shapes { get; private set; }
        public Coord Size { get; private set; }

        public event EventHandler Changed;
        public event EventHandler GameLost;

        public Model(IPersistence _persistence, int width)
        {
            Shapes = new List<Shape>();
            rowComplete = new int[LENGTH];

            _rand = new Random();
            this._persistence = _persistence;
            path = String.Empty;

            this.Size = new Coord(width, LENGTH);
        }

        public Model(IPersistence _persistence, string path)
        {
            Shapes = new List<Shape>();
            rowComplete = new int[LENGTH];
            this.path = path;

            this._persistence = _persistence;
            _rand = new Random();
        }



        public void Save(String path)
        {
            List<String> data = new List<string>();
            StringBuilder line = new StringBuilder();
            for (int i = 0; i < Shapes.Count - 1; i++) //currently falling will be emitted
            {

                foreach (Coord coord in Shapes[i].Coordinates)
                {
                    line.Append(coord.X);
                    line.Append(' ');
                    line.Append(coord.Y);
                    line.Append(' ');
                }

                if (line.Length > 0)
                {
                    line.Remove(line.Length - 1, 1);
                    data.Add(line.ToString());
                    line.Clear();

                }
            }

            if (data.Count > 0)
            {
                _persistence.Write(path, data.ToArray(), Size.X);
                data.Clear();
            }

        }

        public async Task LoadAsync()
        {
            String[] source = await _persistence.ReadAsync(path);

            try
            {
                Size = new Coord(Int32.Parse(source[0]), LENGTH);

                for (int j = 1; j < source.Length; j++)
                {
                    if (source[j] == "") continue;

                    String[] _coords = source[j].Split(' ');
                    List<Coord> coords = new List<Coord>();

                    for (int i = 0; i < _coords.Length; i+= 2)
                    {
                        coords.Add(new Coord(Int32.Parse(_coords[i]), Int32.Parse(_coords[i + 1])));
                        rowComplete[Int32.Parse(_coords[i + 1])]++;
                    }

                    Shapes.Add(new Polymorph(coords, Size, _rand.Next() % COLORS));
                    Shapes[^1].Drawn += Model_Drawn;
                }
            }
            catch
            {
                Size = new Coord(4, LENGTH);

                Shapes = new List<Shape>();
                rowComplete = new int[LENGTH];
            }
        }


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
                        Changed?.Invoke(this, new DrawnEventArgs(i));
                    }
                }
            }
        }

        private void ClearRow(int index)
        {
            rowComplete[index] = 0;

            int cleared = 0;

            for (int i = Shapes.Count - 1; i >= 0; i--)
            {
                if (cleared == Size.X) return;
                for (int j = Shapes[i].Coordinates.Count-1; j>= 0; j--)
                {
                    if (Shapes[i].Coordinates[j].Y == index)
                    {
                        Shapes[i].Coordinates.RemoveAt(j);
                        Changed?.Invoke(this, new DrawnEventArgs(i, j));
                        cleared++;
                    }
                }

                if (Shapes[i].Coordinates.Count == 0) Shapes.RemoveAt(i);
            }
        }

        private void BlowRows()
        {
            if (!rowComplete.Any<int>(a => a >= Size.X)) return;

            for (int i = 0; i<rowComplete.Length; i++)
            {
                if (rowComplete[i] >= Size.X)
                {
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

                Changed?.Invoke(this, new DrawnEventArgs(Shapes.Count - 1));
            }
            else
            {
                if (e.Falling)
                {
                    if (Shape.OverFlown(shape.TempCoords))
                    {
                        GameLost?.Invoke(this, EventArgs.Empty);
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
