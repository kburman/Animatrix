using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Animatrix.Animation
{
    public class Debug :IAnimation 
    {
        private bool _start = false;
        public bool Started
        {
            get
            {
                return _start;
            }
            set
            {
                return;
            }
        }

        private bool _completed = false;
        public bool Completed
        {
            get
            {
                return _completed;
            }
            set
            {
                return;
            }
        }

        public int startFrame { get; set; }


        public int currentFrame { get; set; }

        public void cleanMemoryFootprint()
        {
            
        }

        public Padding getPadding(System.Drawing.Size hostSize)
        {
            return new Padding(10);
        }

        public void nextFrame(ref AnimationFrameArgs e)
        {
            _start = true;
            e.graphics.Clear(Color.Yellow);
            var bck = e.Background;
            var fr = e.Forerground;

            e.graphics.DrawImage(bck, Point.Empty);
            e.graphics.DrawImage(fr, e.Location);
            e.graphics.DrawString(currentFrame.ToString(), Form.DefaultFont, SystemBrushes.ActiveBorder, Point.Empty);

            currentFrame += 1;

          
        }
    }
}
