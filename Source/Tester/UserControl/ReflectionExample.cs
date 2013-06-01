using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Animatrix.Animation;
using Animatrix.Animation.Slide;

namespace Tester
{
    public partial class ReflectionExample : UserControl
    {
        public ReflectionExample()
        {
            InitializeComponent();
            this.textBox1.MouseMove += textBox1_MouseMove;
        }

        private bool done = false;
        void textBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!done)
            {
                Reflection re = new Reflection(BackColor);
                animator1.AddReflection(textBox1, re);
                done = true;
            }
        }

        private void ReflectionExample_Load(object sender, EventArgs e)
        {
          

            Reflection re1 = new Reflection(BackColor);
            re1.Position = Direction.Right;
            animator1.AddReflection(pictureBox1, re1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            animator1.AddReflection(button1);
        }

    }
}
