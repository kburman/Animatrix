using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Animatrix.Animation
{

    public class Reflection : IAnimation
    {
        public bool Started { get; set; }
        public bool Completed { get; set; }
        public int startFrame { get; set; }
        public int currentFrame { get; set; }
        private Color _color = Color.Black;
        private Size _hostSize = Size.Empty;
        private float _angle = float.NaN;

        /// <summary>
        /// Where will be reflection is to be Displayed.
        /// Default is at Bottom
        /// </summary>
        public Direction Position { get { return _where; } set { _where = value; } }
        private Direction _where = Direction.Bottom;

        /// <summary>
        /// Distance Between the Reflection and actual control.
        /// </summary>
        public int Distance { get { return _distance; } set { _distance = value; } }
        private int _distance = 0;

        /// <summary>
        /// Set the Opacity of Reflected Image.
        /// </summary>
        public int Opacity { get { return _reflectance; } set { _reflectance = value; } }
        private int _reflectance = 100;

  


        /// <summary>
        /// Create a new instance of Reflection animation
        /// </summary>
        /// <param name="Backcolor">background color of host.parent</param>
        public Reflection(Color Backcolor)
        {
            this.Started = true ;
            this.Completed = false;
            this.startFrame = 0;
            this.currentFrame = 0;
            this._angle = 0F;
            this._color = Backcolor;
        }

        /// <summary>
        /// Clean the memory used by this class.  
        /// </summary>
        /// <remarks >
        /// Since it not uses to much vari
        /// </remarks>
        public void cleanMemoryFootprint()
        {
            _color = Color.Empty;
            _hostSize = Size.Empty;
            mat = null;
            lbrush.Dispose();
            lbrush = null;
            imgrectangle = Rectangle.Empty;
        }

        /// <summary>
        /// Used by Animation Manger .
        /// </summary>
        /// <param name="hostSize"></param>
        /// <returns></returns>
        public Padding getPadding(System.Drawing.Size hostSize)
        {
            _hostSize = hostSize;
            switch (this.Position)
            {
                case Direction.Left:
                    return new Padding(hostSize.Width+Distance , 0, 0, 0);
                    break;
                case Direction.Top:
                    return new Padding(0, hostSize.Height + Distance, 0, 0);
                    break;
                case Direction.Right:
                    return new Padding(0, 0, hostSize.Width + Distance, 0);
                    break;
                case Direction.Bottom:
                    return new Padding(0, 0, 0, hostSize.Height + Distance);
                    break;
                default:
                    return new Padding(20);
                    break;
            }
        }

        Matrix mat = null;
        Rectangle imgrectangle = Rectangle.Empty;
        LinearGradientBrush lbrush = null;
        public void nextFrame(ref AnimationFrameArgs e)
        {
            var fr = e.Forerground;

            // Inti Variables.
            if (this.currentFrame == 0)
            {
                mat = new Matrix();
                switch (this.Position)
                {
                    case Direction.Left:
                        mat.Translate(-_hostSize.Width, 0);
                        break;
                    case Direction.Top:
                        mat.Translate(0, -_hostSize.Height);
                        break;
                    case Direction.Right:
                        mat.Translate(_hostSize.Width, 0);
                        _angle = 270;
                        break;
                    case Direction.Bottom:
                        mat.Dispose();
                        mat = null;
                        this._angle = 90F;
                        break;
                    default:
                        break;
                }
                imgrectangle = new Rectangle(0, 0, _hostSize.Width, _hostSize.Height);
                lbrush = new LinearGradientBrush(imgrectangle, _color, Color.FromArgb(255 - this.Opacity, _color), this._angle);
            }


            this.currentFrame += 1;

            switch (this.Position)
            {
                case Direction.Left:
                case Direction.Top:
                case Direction.Right:
                    e.graphics.Transform = mat;
                    e.graphics.DrawImage(fr , Point.Empty);
                    break;
                case Direction.Bottom:
                    e.graphics.ScaleTransform(1F, -1F);
                    e.graphics.TranslateTransform(0, -(e.ScreenerSize.Height + _distance));
                    e.graphics.DrawImage(fr, e.Location);
                    break;
            }
            e.graphics.FillRectangle(lbrush, imgrectangle);

            fr.Dispose();
            fr = null;
        }



    }
}
