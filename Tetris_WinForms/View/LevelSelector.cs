using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris_WPF
{
    public partial class LevelSelector : Form
    {
        public event EventHandler<SelectedEventAgrs> Selected;
        public LevelSelector()
        {
            InitializeComponent();
        }

        public SelectedEventAgrs SelectedEventAgrs
        {
            get => default;
            set
            {
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Selected?.Invoke(this, new SelectedEventAgrs(comboBox1.SelectedIndex));
            Close();
        }

        private void LevelSelector_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
        }
    }
}
