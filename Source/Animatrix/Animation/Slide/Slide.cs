using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Animatrix.Animation.Slide
{
    public class SlideIn : IAnimation
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

        public SlideIn(int distance, Direction dir)
        {
            this.Started = false;
            this.Completed = false;
            this.startFrame = 0;
            this.currentFrame = 0;
            this.Distance = distance;
            this.From = dir;
        }
        public SlideIn()
        {
            this.Started = false;
            this.Completed = false;
            this.startFrame = 0;
            this.currentFrame = 0;
            this.Distance = 200;
            this.From = Direction.Left;
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

        Matrix mat;
        public void  nextFrame(ref AnimationFrameArgs e)
        {
            if (currentFrame >= startFrame)
            {
                Started = true;
                var bck = e.Background;
                var frg = e.Forerground;
                e.graphics.DrawImageUnscaled(bck, Point.Empty);
                if (this.Distance <= this.EaseIn) { this.DeltaSpeed = 0; this.Speed = 10; }
                mat = new Matrix();
                switch (this.From)
                {
                    case Direction.Left:
                        mat.Translate(-this.Distance, 0);
                        break;
                    case Direction.Top:
                        mat.Translate(0, -this.Distance);
                        break;
                    case Direction.Right:
                        mat.Translate(this.Distance, 0);
                        break;
                    case Direction.Bottom:
                        mat.Translate(0, this.Distance);
                        break;
                }
                e.graphics .Transform = mat;
                this.Distance -= this.Speed;
                this.Speed += this.DeltaSpeed;
                e.graphics .DrawImage(frg , e.Location);
                mat.Dispose();

            }

            if (this.Distance <= 0) this.Completed = true;

            this.currentFrame += 1;
        }
    }
}
