using Animatrix.Animation;
using Animatrix.Animation.Slide;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Tester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Shown +=Form1_Shown;
        }

        void Form1_Shown(object sender, System.EventArgs e)
        {
            int startFrame = 10;
            // Menu
            SlideIn sl1 = new SlideIn();
            sl1.startFrame = startFrame;
            animator1.Add(panel1, sl1 );

            startFrame += 30;

            // Menu Pane
            SlideIn sl2 = new SlideIn();
            sl2.startFrame = startFrame;
            sl2.From = Direction.Right ;
            animator1.Add(panel2, sl2);

            startFrame += 30;

            // Menu Button Animation
            foreach (Control item in panel1.Controls )
            {
                SlideIn _sl = new SlideIn();
                _sl.From = Direction.Left;                
                _sl.startFrame = startFrame;
                animator1.Add(item, _sl);
                //animator1.AddReflection(item);
                /// Play with this number to load menu item in difftent style
                /// try 1,2,3,0
                startFrame += 3;
            }

            


        }

        private void Clear()
        {
            foreach (Control item in panel2.Controls )
            {
                try
                {
                    item.Dispose();
                }
                catch (Exception)
                {
                    
                    throw;
                }
            }
            panel2.Controls.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadReflection();
        }

        private void LoadReflection()
        {
            try
            {
                Clear();
                ReflectionExample  reflectionEx = getReflectionExample();
                reflectionEx.Visible = false;
                reflectionEx.BackColor = BackColor;
                panel2.Controls.Add(reflectionEx);
                //reflectionEx.Dock = DockStyle.Fill;
                reflectionEx.Location = Point.Empty;
                reflectionEx.Size = panel2.Size;

                SlideIn CstSl = new SlideIn();
                CstSl.From = Direction.Right;
                CstSl.Distance = 500;
                animator1.Add(reflectionEx, CstSl);
                /// Some Glitch 
                /// 
                reflectionEx.Visible = true ;
                reflectionEx.Visible = false;
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private void ShowError(String msg)
        {
            MessageBox.Show(msg);
        }



        private ReflectionExample getReflectionExample()
        {
            return new ReflectionExample();
        }

    }
}
