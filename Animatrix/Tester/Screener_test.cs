using System;
using System.Windows.Forms;
using System.Drawing;
using Animatrix;
using Animatrix.Screener;

namespace Tester
{
    public partial class Screener_test : Form
    {
        ControlScreener sc;

        public Screener_test()
        {
            InitializeComponent();
            sc = new ControlScreener(button3);
            sc.BackColor = Color.Yellow;
            sc.HostVisible_afterAnimation = true;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = sc.getForeground ();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = sc.getBackground();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            sc.coverTheHost();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            sc.leaveScreen();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            sc.imageToPaint = sc.getBackground ();
            sc.Refresh();
        }
    }
}
