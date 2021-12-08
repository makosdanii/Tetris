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
        private const int ROWS = 16;

        private int SIZE;
        Model gamemodel;
        LevelSelector selectorform;
        public View()
        {
            InitializeComponent();
            
        }

        private void Selectorform_Selected(object sender, SelectedEventAgrs e)
        {
            SIZE = e.Index == 2 ? 12 : (e.Index + 1) * 4;
            gamemodel = new Model(SIZE);
            gamemodel.Drawn += _gamemodel_Drawn;
            button1.Enabled = true;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            //Graphics g = panel1.CreateGraphics();
            //g.Clear(BackColor);
            //g.DrawLine(new Pen(Color.DarkOrange, 10), new Point(Left, Top), new Point(Right, Bottom));
        }

        private void View_Shown(object sender, EventArgs e)
        {
            selectorform = new LevelSelector();
            selectorform.Selected += Selectorform_Selected;
            selectorform.Show();
        }

        private void _gamemodel_Drawn(object sender, Shape_events.DrawnEventArgs e)
        {
            Graphics g = panel1.CreateGraphics();
            int scaleX = panel1.Width / SIZE;

            g.Clear(BackColor);
            Rectangle[] rects = new Rectangle[4];

            for (int i = 0; i < rects.Length; i++)
            {
                rects[i] = new Rectangle(
                    CoordToPoint(gamemodel.Shapes[e.Index].Coordinates[i], scaleX), new Size(scaleX, scaleX));
            }

            g.DrawRectangles(new Pen(Color.DarkOrange, 5), rects);
        }

        private Point CoordToPoint(Coord c, int scaleX)
        {
            return new Point(c.x*scaleX, c.y*scaleX);
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            MessageBox.Show($"x:{e.X}, y:{e.Y}");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            gamemodel.AddShape();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            selectorform = new LevelSelector();
            selectorform.Selected += Selectorform_Selected;
            selectorform.Show();
        }
    }
}
