using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tetris_WinForms.Shape_events;

namespace Tetris_WinForms
{
    public partial class View : Form
    {
        private static readonly Dictionary<int, Color> myColors = new Dictionary<int, Color>
        {
            { 0, Color.Teal },
            { 1, Color.DarkOrange },
            { 2, Color.Crimson },
            { 3, Color.ForestGreen },
            { 4, Color.RoyalBlue },
        };

        private static readonly Dictionary<int, int> Columns = new Dictionary<int, int>
        {
            { 0, 4 },
            { 1, 8 },
            { 2, 12 },
        };

        private static readonly Dictionary<int, int> Resolution = new Dictionary<int, int>
        {
            { 4, 240},
            { 8, 480},
            { 12, 720},
        };

        private const int ROWS = 16;
        private int size;
        private bool started;
        private bool getShape;

        System.Windows.Forms.Timer _timer;

        Model gamemodel;
        LevelSelector selectorform;
        public View()
        {
            InitializeComponent();
            size = -1;
            started = false;
            getShape = true;
            _timer = new System.Windows.Forms.Timer();
            _timer.Tick += _timer_Tick;
            _timer.Interval = 1000;
            _timer.Enabled = false;
        }


        #region Form event handling
        private void View_Shown(object sender, EventArgs e)
        {
            selectorform = new LevelSelector();
            selectorform.Selected += Selectorform_Selected;
            selectorform.Show();
        }
        private void View_Load(object sender, EventArgs e)
        {
            gamemodel = new Model(size);
        }

        private void View_ResizeEnd(object sender, EventArgs e)
        {
            Size = adjustSize();
        }

        private void Selectorform_Selected(object sender, SelectedEventAgrs e)
        {
            size = Columns[e.Index];
            Size = adjustSize(e.Index);

            gamemodel = new Model(size);
            gamemodel.Changed += _gamemodel_Changed;
            gamemodel.GameLost += Gamemodel_GameLost;
            gamemodel.Blow += Gamemodel_Blow;
            
            button1.Enabled = true;

            panel1.CreateGraphics().Clear(BackColor);
            Refresh();

            getShape = true;
        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            DrawModel();

        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            if (getShape)
            {
                getShape = !getShape;
                //_timer.Stop();
                gamemodel.AddShape();
            }
            else
            {
                View_KeyDown_1(this, new KeyEventArgs(Keys.Down));
                //_timer.Enabled = true;

            }
        }

        private Size adjustSize(int index = -1)
        {

            if (index != -1)
            {
                int width = Resolution[Columns[index]];
                MinimumSize = new Size(width, width / size);
            }
            return new Size(panel1.Width, (panel1.Width / size) * (ROWS+2));
        }
        #endregion

        #region Game model event handling
        private void _gamemodel_Changed(object sender, EventArgs e)
        {
            if (e as StuckEventArgs != null)
            {
                getShape = true;
            }
            Refresh();
            _timer.Enabled = true;
        }

        private void Gamemodel_GameLost(object sender, EventArgs e)
        {
            button1_Click(this, new EventArgs());
            Selectorform_Selected(this, new SelectedEventAgrs(0));
            Refresh();
            MessageBox.Show("Game lost, try again!");
        }
        private void Gamemodel_Blow(object sender, BlowEventArgs e)
        {
            DrawModel(e.Index);
        }

        private Point CoordToPoint(Coord c, int scaleX)
        {
            return new Point(c.X*scaleX, c.Y*scaleX);
        }

        private void DrawModel(int blowRow = -1)
        {
            Graphics g = panel1.CreateGraphics();
            g.Clear(BackColor);
            int scaleX = panel1.Width / size;
            Pen _pen = new Pen(Color.Gray, 5);

            for (int j = 0; j < gamemodel.Shapes.Count; j++)
            {
                for (int i = 0; i < gamemodel.Shapes[j].Coordinates.Count; i++)
                {
                    if (blowRow > -1 && gamemodel.Shapes[j].Coordinates[i].Y == blowRow)
                    {
                        _pen.Color = Color.Gray;
                        _pen.DashStyle = DashStyle.Dash;
                    }
                    else
                    {
                        _pen.Color = myColors[gamemodel.Shapes[j].ColorCode];
                        _pen.DashStyle = DashStyle.Solid;
                    }

                    g.DrawRectangle(_pen, new(
                        CoordToPoint(gamemodel.Shapes[j].Coordinates[i], scaleX), new Size(scaleX, scaleX)));
                }
            }

            if (blowRow > -1)
            {
                _timer.Stop();
                Thread.Sleep(250);
            }
        }


        #endregion

        #region Controllers event handling
        private void button1_Click(object sender, EventArgs e)
        {
            started = !started;
            button1.Text = started ? "Pause" : "Start";
            button2.Enabled = !started;
            KeyPreview = started;

            _timer.Enabled = started;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            selectorform = new LevelSelector();
            selectorform.Selected += Selectorform_Selected;
            selectorform.Show();
        }

        private void View_KeyDown_1(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Left:
                    gamemodel.Shapes[^1].Move(Direction.LEFT);
                    break;
                case Keys.Right:
                    gamemodel.Shapes[^1].Move(Direction.RIGHT);
                    break;
                case Keys.Up:
                    gamemodel.Shapes[^1].Rotate();
                    break;
                case Keys.Down:
                    gamemodel.Shapes[^1].Move(Direction.DOWN);
                    break;
                case Keys.Space:
                    button1_Click(this, new EventArgs());
                     break;
            }

        }

        #endregion

    }
}
