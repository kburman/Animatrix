using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Animatrix.Animation
{
   public  class Debug : IAnimation
    {
        public bool Started { get; set; }
        public bool Completed { get; set; }
        public int startFrame { get; set; }
        public int currentFrame { get; set; }

        public Debug()
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

        private Size _hostSize = Size.Empty ;
        public Padding getPadding(System.Drawing.Size hostSize)
        {
            _hostSize = hostSize;
            return new Padding(20);
        }


        public System.Drawing.Bitmap nextFrame(AnimationFrameArgs e)
        {
            Bitmap bit = new Bitmap(e.ScreenerSize.Width, e.ScreenerSize.Height);
            Graphics g = Graphics.FromImage(bit);
            g.Clear(Color.Red);
            g.DrawImage(e.Background  ,Point.Empty );

            g.DrawImage(e.Forerground, e.Location);

            //g.DrawImage(e.Forerground, e.Location);
            g.DrawString(this.currentFrame.ToString(), Form.DefaultFont, SystemBrushes.GrayText, PointF.Empty);
            g.Dispose();
            this.currentFrame += 1;
            return bit;
        }
    }
}
