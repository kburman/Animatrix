using Animatrix.Animation;
using Animatrix.Projectors;
using Animatrix.Screener;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Tester
{
    public partial class Test : Form
    {
        private Control _aniObj = null;
        public Test()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Animate(_aniObj);            
        }

        private void Test_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            _aniObj = panel1    ;
            if (animator1.Started)
                button3.Text = "Pause";
            else
                button3.Text = "Start";
        }

        private void Animate(Control obj)
        {
            if (obj == null) return;
            ControlScreener sc = new ControlScreener(obj);
            IAnimation an ;
            an = new Debug();
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                   var slideer = new Animatrix.Animation.SlideIn(500, Direction.Left);
                  
                   an = slideer;
                   break;
                case 1: break;
            }
            Projector pj = new Projector();
            pj.Inti(sc, an, true);
            animator1.Add(pj);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (button3.Text == "Pause")
            { animator1.Pause(); button3.Text = "Start"; }
            else
            { animator1.Start(); button3.Text = "Pause"; }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            _aniObj.Visible = !_aniObj.Visible;
        }

    }
}
