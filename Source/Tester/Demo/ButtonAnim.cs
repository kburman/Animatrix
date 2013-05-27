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
    public partial class ButtonAnim : Form
    {
        public ButtonAnim()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Rotate _animation = new Rotate();
            animator1.Add((Button)sender, _animation);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SlideIn _animation = new SlideIn();
            animator1.Add ((Button)sender , _animation);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SlideIn _animation = new SlideIn();
            _animation.From  = Direction.Top;
            animator1.Add((Button)sender, _animation);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SlideIn _animation = new SlideIn();
            _animation.From = Direction.Bottom;
            animator1.Add((Button)sender, _animation);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Rotate _animation = new Rotate();
            _animation.rotateAtCenter = false;
            animator1.Add((Button)sender, _animation);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            animator1.AddReflection((Button)sender);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            animator1.Add(button7, new Debug());
        }

        private void ButtonAnim_Load(object sender, EventArgs e)
        {
            animator1.AddReflection(textBox1);
            animator1.AddReflection(pictureBox1);
            animator1.AddReflection(progressBar1);
        } 
    }
}
