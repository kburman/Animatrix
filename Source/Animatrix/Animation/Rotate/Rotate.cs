using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Animatrix.Animation
{
    public class Rotate : IAnimation 
    {

        public int angle = 0;
        public int deltaAngle = 20;
        private float middleX = float.NaN;
        private float middleY = float.NaN;
        private PointF middlePt = Point.Empty;
        private bool _invert = false;
        public bool rotateAtCenter = true;


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
            return new Padding(Math.Max(hostSize.Height,hostSize.Width));
        }

        public  void Invert() { _invert = !_invert; }

        Matrix mat=null ;
        public void nextFrame(ref AnimationFrameArgs e)
        {

            if (currentFrame >= startFrame )
            {
                var bck = e.Background;
                var frg = e.Forerground;

                // Draw Background
                e.graphics.DrawImageUnscaled(bck, Point.Empty);
                //

                // Determine center 
                if (middleX.Equals(float.NaN) || middleY.Equals(float.NaN))
                {
                    if (rotateAtCenter)
                    {
                        middleX = (e.ScreenerSize.Width / 2);
                        middleY = (e.ScreenerSize.Height / 2);
                        middlePt = new PointF(middleX, middleY);
                    }
                    else
                    {
                        middlePt = PointF.Empty;
                        middleX = 0;
                        middleY = 0;
                        deltaAngle = 10;
                        angle = 200;
                    }
                }


                if (mat == null )
                {
                    mat = new Matrix();
                }

                mat.Reset();

                mat.RotateAt(angle, middlePt);

                
                angle += deltaAngle;

                if (_invert) mat.Invert();



                e.graphics .Transform = mat;
                e.graphics .DrawImage(frg , e.Location);

            }
            currentFrame += 1;
            if (angle >= 360) _completed  = true;
        }
    }
}
