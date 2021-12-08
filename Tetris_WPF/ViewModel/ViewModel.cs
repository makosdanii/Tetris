using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Tetris_WPF
{
    public enum Size
    {
        SMALL = 4, MEDIUM = 8, LARGE = 16
    }

    class ViewModel : ViewModelBase
    {
        #region fields
        private static readonly String[] myColors = new string[] { "Coral", "LimeGreen", "SlateBlue", "Teal", "DarkOrchid" };

        private Model _model;
        private DispatcherTimer _timer;

        private List<int> indexCatalog;

        private double _unit;
        private int tableSize;
        private bool isStarted;
        private bool next;
        #endregion

        #region Props
        public double Unit { 
            get { return _unit; }
            set { _unit = value; OnPropertyChanged("Unit"); }
        }
        public ObservableCollection<ShapeField> UIShapes { get; set; }
        #endregion

        #region Commands
        public DelegateCommand SaveGame { get; private set; }
        public DelegateCommand LoadGame { get; private set; }
        public DelegateCommand SetGame { get; private set; }
        public DelegateCommand StartGame { get; private set; }
        public DelegateCommand InputKey { get; private set; }
        #endregion

        #region Events
        public event EventHandler SaveFile;
        public event EventHandler LoadFile;
        public event EventHandler GameOver;
        #endregion

        public ViewModel()
        {
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1.0),
            };
            _timer.Tick += Timer_Tick;

            UIShapes = new ObservableCollection<ShapeField>();
            Unit = 50.0;

            tableSize = 0;
            isStarted = false;
            next = false;
            indexCatalog = new List<int>(new int[] { 0 });

            StartGame = new DelegateCommand(_ => ToggleState(), _ => tableSize > 0);
            SaveGame = new DelegateCommand(_ => SaveFile?.Invoke(this, EventArgs.Empty), _ => tableSize > 0);
            LoadGame = new DelegateCommand(_ => LoadFile?.Invoke(this, EventArgs.Empty));

            SetGame = new DelegateCommand(size =>
                                            {
                                                SetTableAsync(Int16.Parse((String)size));
                                            }, Size => !isStarted);

            InputKey = new DelegateCommand(key => Navigate(((string)key)[0]), key => isStarted);

        }


        #region Event handling
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (next)
            {
                next = false;
                _model.AddShape();
            }
            else Navigate('S');
        }

        public void Model_GameLost(object sender, EventArgs e)
        {
            ToggleState();
            GameOver?.Invoke(this, EventArgs.Empty); //improve by sending playtime            
        }

        public void Model_Changed(object sender, EventArgs e)
        {
            if (e as Shape_events.StuckEventArgs != null)
            {
                next = true;
            }
            else
            {
                UpdateCollection((e as Shape_events.DrawnEventArgs).ShapeNo, (e as Shape_events.DrawnEventArgs).LostNo);
            }
        }
        #endregion

        private bool ToggleState()
        {
            isStarted = !isStarted;

            if (isStarted)
            {
                _timer.Start();
            }
            else
            {
                _timer.Stop();
            }

            return isStarted;
        }

        private void Navigate(Char key)
        {
            switch (key)
            {
                case 'W': _model.Shapes[^1].Rotate(); break;
                case 'S': _model.Shapes[^1].Move(Direction.DOWN); break;
                case 'A': _model.Shapes[^1].Move(Direction.LEFT); break;
                case 'D': _model.Shapes[^1].Move(Direction.RIGHT); break;
                default:
                    break;
            }

        }

        private void UpdateCollection(int shapeNo, int LostNo = -1)
        {
            if (shapeNo >= indexCatalog.Count - 1)
            {
                indexCatalog.Add(indexCatalog[^1] + _model.Shapes[shapeNo].Coordinates.Count);

                string color = myColors[_model.Shapes[shapeNo].ColorCode];
                foreach (Coord coord in _model.Shapes[shapeNo].Coordinates)
                {
                    UIShapes.Add(new ShapeField
                    {
                        Top = (int)(Unit) * coord.Y,
                        Left = (int)Unit * coord.X,
                        Color = color,
                        Prev = 0
                    });
                }
                return;
            }

            if (LostNo > -1)
            {
                UIShapes.RemoveAt(indexCatalog[shapeNo] + LostNo);

                for (int i = shapeNo + 1; i < indexCatalog.Count; i++)
                    indexCatalog[i]--;

                if (indexCatalog[shapeNo + 1] == indexCatalog[shapeNo]) indexCatalog.RemoveAt(shapeNo + 1);

                return;
            }

            for (int i = 0; i < _model.Shapes[shapeNo].Coordinates.Count; i++)
            {
                UIShapes[indexCatalog[shapeNo] + i].Prev = UIShapes[indexCatalog[shapeNo] + i].Top;
                UIShapes[indexCatalog[shapeNo]+i].Left = _model.Shapes[shapeNo].Coordinates[i].X * (int)Unit;
                UIShapes[indexCatalog[shapeNo]+i].Top = _model.Shapes[shapeNo].Coordinates[i].Y * (int)Unit;
            }

        }

        public async Task SetTableAsync(int i = 0, string path = null)
        {
            if (path != null)
            {
                _model = new Model(new TextPersistence(), path);
                await _model.LoadAsync();
            }
            else
            {
                _model = new Model(new TextPersistence(), i == 0 ? tableSize : i);
            }
            if (i != 0) tableSize = i;

            _model.Changed += Model_Changed;
            _model.GameLost += Model_GameLost;

            next = true;

            UIShapes.Clear();
            indexCatalog.Clear();
            indexCatalog.Add(0);
        }

        public void Save(string path)
        {
            _model.Save(path);
        }

        public async void LoadAsync(string path)
        {
            await SetTableAsync((int)Size.SMALL, path);
            Enumerable.Range(0, _model.Shapes.Count).ToList().ForEach(x => UpdateCollection(x));
        }
    }
}
