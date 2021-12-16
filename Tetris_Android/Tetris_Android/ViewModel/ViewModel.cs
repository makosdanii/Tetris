using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Tetris_Android
{
    /// <summary>
    /// Tetris nézetmodell típusa.
    /// </summary>

    public class Level
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public Level(string name, int value)
        {
            Name = name;
            Value = value;
        }
    }

    public class ViewModel : ViewModelBase
    {
        #region Fields
        private static readonly String[] myColors = new string[] 
        { "Coral", "LimeGreen", "SlateBlue", "Teal", "DarkOrchid" };

        private GameModel _model; // modell

        private List<int> indexCatalog;

        private string _status;
        private int _unit;
        private int _tableSize;
        #endregion

        #region Properties
        public DelegateCommand SetGameCommand { get; private set; }
        /// <summary>
        /// Új játék kezdése parancs lekérdezése.
        /// </summary>
        public DelegateCommand ToggleGameCommand { get; private set; }

        /// <summary>
        /// Játék betöltése parancs lekérdezése.
        /// </summary>
        public DelegateCommand LoadGameCommand { get; private set; }

        /// <summary>
        /// Játék mentése parancs lekérdezése.
        /// </summary>
        public DelegateCommand SaveGameCommand { get; private set; }

        /// <summary>
        /// Kilépés parancs lekérdezése.
        /// </summary>
        public DelegateCommand ExitCommand { get; private set; }
        public ObservableCollection<ShapeField> UIShapes { get; set; }

        public string Status
        {
            get { return _status; }
            set
            {
                _status = _status == "Start" ? "Stop" : "Start";
                OnPropertyChanged("Status");
            }
        }

        public int Unit
        {
            get { return _unit; }
            set { _unit = value; OnPropertyChanged("Unit"); }
        }
        public int TableSize
        {
            get { return _tableSize; }
            set { _tableSize = value; OnPropertyChanged("TableSize"); }
        }
        public List<Level> Levels { get; set; }

        #endregion

        #region Events
        public event EventHandler<SingleIntEventArg> SetGame;
        /// <summary>
        /// Új játék eseménye.
        /// </summary>
        public event EventHandler ToggleGame;

        /// <summary>
        /// Játék betöltésének eseménye.
        /// </summary>
        public event EventHandler LoadGame;

        /// <summary>
        /// Játék mentésének eseménye.
        /// </summary>
        public event EventHandler SaveGame;

        /// <summary>
        /// Játékból való kilépés eseménye.
        /// </summary>
        public event EventHandler ExitGame;

        #endregion

        #region Constructors

        /// <summary>
        /// Sudoku nézetmodell példányosítása.
        /// </summary>
        /// <param name="model">A modell típusa.</param>
        public ViewModel(GameModel model)
        {
            _model = model;
            Status = "Start";
            Unit = 50;
            TableSize = model.Size.X;
            UIShapes = new ObservableCollection<ShapeField>();
            indexCatalog = new List<int>();


            SetGameCommand = new DelegateCommand(level => OnSet((level as Level).Value));
            ToggleGameCommand = new DelegateCommand(_ => OnToggleGame());
            LoadGameCommand = new DelegateCommand(_ => OnLoadGame());
            SaveGameCommand = new DelegateCommand( _ => OnSaveGame());
            ExitCommand = new DelegateCommand(_ => Status == "Start", _ => OnExitGame());

            Levels = new List<Level>
            {
                new Level("Small", 4),
                new Level("Middle", 8),
                new Level("Large", 12)

            };
        }

        #endregion
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
                        Top = coord.Y,
                        Left = coord.X,
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
                UIShapes[indexCatalog[shapeNo] + i].Left = _model.Shapes[shapeNo].Coordinates[i].X;
                UIShapes[indexCatalog[shapeNo] + i].Top = _model.Shapes[shapeNo].Coordinates[i].Y;
            }

        }

        public void RefreshCollection()
        {
            TableSize = _model.Size.X;
            UIShapes.Clear();
            indexCatalog.Clear();
            indexCatalog.Add(0);
            if (_model.Shapes.Count == 0)
                return;
            Enumerable.Range(0, _model.Shapes.Count).ToList().ForEach(x => UpdateCollection(x));
        }

        #region Game event handlers
        public void Model_Drawn(object sender, Shape_events.DrawnEventArgs e)
        {
                UpdateCollection(e.ShapeNo, e.LostNo);
        }

        #endregion

        #region Event methods
        private void OnSet(int size)
        {
            SetGame?.Invoke(this, new SingleIntEventArg(size));
        }
        /// <summary>
        /// Új játék indításának eseménykiváltása.
        /// </summary>
        private void OnToggleGame()
        {
            Status = String.Empty;
            if (ToggleGame != null)
                ToggleGame(this, EventArgs.Empty);
        }

        /// <summary>
        /// Játék betöltése eseménykiváltása.
        /// </summary>
        private void OnLoadGame()
        {
            if (LoadGame != null)
                LoadGame(this, EventArgs.Empty);
        }

        /// <summary>
        /// Játék mentése eseménykiváltása.
        /// </summary>
        private void OnSaveGame()
        {
            if (SaveGame != null)
                SaveGame(this, EventArgs.Empty);
        }

        /// <summary>
        /// Játékból való kilépés eseménykiváltása.
        /// </summary>
        private void OnExitGame()
        {
            if (ExitGame != null)
                ExitGame(this, EventArgs.Empty);
        }

        #endregion
    }
}
