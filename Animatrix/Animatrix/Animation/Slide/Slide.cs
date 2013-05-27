using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Animatrix.Animation
{
    public enum Direction
    {
        Left = 0,
        Top = 1,
        Right = 2,
        Bottom = 3
    }
   public  class SlideIn : IAnimation 
    {
        public bool Started { get; set; }
        public bool Completed { get; set; }
        public int startFrame { get; set; }
        public int currentFrame { get; set; }
        public Direction From { get; set; }
        public int Distance { get; set; }
       /// <summary>
       /// Speed With with animation will move.
       /// </summary>
        public int Speed = 10;
       /// <summary>
       /// Change in Speed after every frame.
       /// </summary>
        public int DeltaSpeed = 5;

        public int EaseIn = 50;

        public SlideIn(int distance,Direction dir)
        {
            this.Started = false;
            this.Completed = false;
            this.startFrame = 0;
            this.currentFrame = 0;
            this.Distance = distance ;
            this.From = dir ;
        }
        public SlideIn()
        {
            this.Started = false;
            this.Completed = false;
            this.startFrame = 0;
            this.currentFrame = 0;
            this.Distance = 200;
            this.From =  Direction.Left ; 
        }


        public void cleanMemoryFootprint()
        {
            return;
        }

        public Padding getPadding(System.Drawing.Size hostSize)
        {
            switch (this.From)
            {
                case Direction.Left: return new Padding(this.Distance, 0, 0, 0);
                case Direction.Top: return new Padding(0, this.Distance, 0, 0);
                case Direction.Right: return new Padding(0, 0, this.Distance, 0);
                case Direction.Bottom: return new Padding(0, 0, 0, this.Distance);
                default: return Padding.Empty;
            }
        }

        public System.Drawing.Bitmap nextFrame(AnimationFrameArgs e)
        {
            Bitmap bit = new Bitmap(e.ScreenerSize.Width, e.ScreenerSize.Height);
            Graphics g = Graphics.FromImage(bit);
            g.DrawImage(e.Background , Point.Empty);
            if (this.Distance <= this.EaseIn) { this.DeltaSpeed =0 ; this.Speed = 10; }

            if (this.currentFrame >= this.startFrame)
            {
                Matrix mat = new Matrix();
                switch (this.From)
                { 
                    case Direction.Left :
                        mat.Translate(-this.Distance, 0);
                        break;
                    case Direction.Top:
                         mat.Translate(0,-this.Distance);
                        break;
                    case Direction.Right:
                        mat.Translate(this.Distance,0);
                        break;
                    case Direction.Bottom:
                        mat.Translate(0,this.Distance);
                        break;
                }
                g.Transform = mat;
                this.Distance -= this.Speed;
                this.Speed += this.DeltaSpeed;
                g.DrawImage(e.Forerground, e.Location);
                
            }


            if (this.Distance <= 0) this.Completed = true;
            
            g.Dispose();
            this.currentFrame += 1;
            return bit;
        }
    }
}
