using Animatrix.Animation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Tester.Demo
{
    public partial class AnimatedImageDisplay : Form
    {
        public AnimatedImageDisplay()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Rotate _animation = new Rotate();
            //animator1.Add(pictureBox1, _animation);
        }
    }
}
