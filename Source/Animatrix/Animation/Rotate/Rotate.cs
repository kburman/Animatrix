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
        public bool Started { get; set; }
        public bool Completed { get; set; }
        public int startFrame { get; set; }
        public int currentFrame { get; set; }
        public  int angle = 0;
        public  int deltaAngle = 20;
        private float middleX=float.NaN ;
        private float middleY=float.NaN ;
        private PointF middlePt = Point.Empty;
        private bool _invert = false;
        public  bool rotateAtCenter = true;

        public Rotate()
        {
            this.Started = false;
            this.Completed = false;
            this.startFrame = 0;
            this.currentFrame = 0;
        }
        public void cleanMemoryFootprint()
        {
            return;
        }

        public Padding getPadding(System.Drawing.Size hostSize)
        {
            if (rotateAtCenter)
                return new Padding(Math.Max(hostSize.Height, hostSize.Width));
            else
                //return new Padding(hostSize.Width, hostSize.Height, 0, hostSize.Height);
                return new Padding(0);
        }

        private void Invert() { _invert = !_invert; }

        public System.Drawing.Bitmap nextFrame(AnimationFrameArgs e)
        {
            Bitmap bit = new Bitmap(e.ScreenerSize.Width, e.ScreenerSize.Height);
            Graphics g = Graphics.FromImage(bit);
            g.DrawImage(e.Background, Point.Empty);
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
                    //Console.WriteLine("SET");
                }
            }
            
            if (this.currentFrame >= this.startFrame)
            {
                Matrix mat = new Matrix();
                mat.RotateAt(angle, middlePt);
                if (angle >= 360) Completed = true;
                angle += deltaAngle;
                //Console.WriteLine(angle + " = " + deltaAngle);
                if (_invert) mat.Invert();

                
              
                g.Transform = mat;
                g.DrawImage(e.Forerground, e.Location);

            }

            g.Flush();
            g.Dispose();
            this.currentFrame += 1;
            return bit;
        }
    }
}
