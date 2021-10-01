using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris_WinForms
{
    public partial class View : Form
    {
        private static Dictionary<int, Color> myColors = new Dictionary<int, Color>
        {
            { 0, Color.Teal },
            { 1, Color.DarkOrange },
            { 2, Color.Crimson },
            { 3, Color.ForestGreen },
            { 4, Color.RoyalBlue },
        };

        private const int ROWS = 16;
        private int SIZE;
        Model gamemodel;
        LevelSelector selectorform;
        public View()
        {
            InitializeComponent();
            SIZE = -1;
        }
        private void View_Shown(object sender, EventArgs e)
        {
            selectorform = new LevelSelector();
            selectorform.Selected += Selectorform_Selected;
            selectorform.Show();
        }
        private void View_Load(object sender, EventArgs e)
        {
            gamemodel = new Model(SIZE);
        }

        private void View_ResizeEnd(object sender, EventArgs e)
        {
            adjustSize();
        }

        private void Selectorform_Selected(object sender, SelectedEventAgrs e)
        {
            SIZE = e.Index == 2 ? 12 : (e.Index + 1) * 4;
            adjustSize();

            gamemodel = new Model(SIZE);
            gamemodel.Drawn += _gamemodel_Drawn;
            
            button1.Enabled = true;

            panel1.CreateGraphics().Clear(BackColor);
            Refresh();
        }

        private void _gamemodel_Drawn(object sender, Shape_events.DrawnEventArgs e)
        {
            Refresh();
        }

        private Point CoordToPoint(Coord c, int scaleX)
        {
            return new Point(c.X*scaleX, c.Y*scaleX);
        }

        private void adjustSize()
        {
            Size = new Size(panel1.Width, (panel1.Width / SIZE) * (ROWS+1));
        }

        #region
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = panel1.CreateGraphics();
            g.Clear(BackColor);
            int scaleX = panel1.Width / SIZE;

            Rectangle[] rects = new Rectangle[4];

            foreach(Shape shape in gamemodel.Shapes)
            {
                for (int i = 0; i < rects.Length; i++)
                {
                    rects[i] = new Rectangle(
                        CoordToPoint(shape.Coordinates[i], scaleX), new Size(scaleX, scaleX));
                }

                g.DrawRectangles(new Pen(myColors[shape.ColorCode], 5), rects);
            }

        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            MessageBox.Show($"x:{e.X}, y:{e.Y}");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            gamemodel.AddShape();
            KeyPreview = KeyPreview ? true : true;
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
                     break;
            }

        }

        #endregion

    }
}
