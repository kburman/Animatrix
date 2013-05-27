using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
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
        public Direction Position = Direction.Bottom;
        /// <summary>
        /// Sepration bettween Control and reflection.
        /// </summary>
        private int distance = 2;
        /// <summary>
        /// Opacity of Reflected Image
        /// </summary>
        public int Reflectance = 230;
        private  int Length = 50;
        public bool AutoSize = true;
        private Color _color;
        public Reflection(Color BackColor,int len)
        {
            this.Started = false;
            this.Completed = false;
            this.startFrame = 0;
            this.currentFrame = 0;
            this._color = BackColor;
            this.Length = len;
        }
        public Reflection(Color BackColor)
        {
            this.Started = false;
            this.Completed = false;
            this.startFrame = 0;
            this.currentFrame = 0;
            this._color = BackColor;
        }
        public void cleanMemoryFootprint()
        {
            return;
        }

        private Size _hostSize = Size.Empty ;
        public Padding getPadding(System.Drawing.Size hostSize)
        {
            _hostSize = hostSize;
            if (AutoSize)
                switch (Position)
                {
                    case Direction.Left:
                        return new Padding(hostSize.Width , 0, 0, 0);
                        break;
                    case Direction.Top:
                        return new Padding(0, hostSize.Height , 0, 0);
                        break;
                    case Direction.Right:
                        return new Padding(0, 0, hostSize.Width , 0);
                        break;
                    case Direction.Bottom:
                        return new Padding(0, 0, 0, hostSize.Height);
                        break;
                    default:
                        return new Padding(Length);
                        break;
                }
            else
            switch (Position )
            {
                case Direction.Left:
                    return new Padding(Length, 0, 0, 0);
                    break;
                case Direction.Top:
                    return new Padding(0, Length, 0, 0);
                    break;
                case Direction.Right:
                    return new Padding(0, 0, Length, 0);
                    break;
                case Direction.Bottom:
                    return new Padding(0, 0, 0, Length);
                    break;
                default:
                    return new Padding(Length);
                    break;
            }
        }

        private float angle = 90F; // 90F
        public System.Drawing.Bitmap nextFrame(AnimationFrameArgs e)
        {
            Bitmap bit = new Bitmap(e.ScreenerSize.Width, e.ScreenerSize.Height);
            Graphics g = Graphics.FromImage(bit);
            g.DrawImage(e.Background  ,Point.Empty );
            Point pt=Point.Empty ;
            Matrix mat = new Matrix();
            Rectangle imageRectangle = Rectangle.Empty  ;
            LinearGradientBrush lbrush=null;
            
            switch (Position )
            {
                case Direction.Left:
                     mat.Translate(-_hostSize.Width ,0);
                    g.Transform = mat;
                     // Draw Gradient
                    imageRectangle = new Rectangle(0, 0, e.Forerground.Width, e.Forerground.Height);
                    lbrush = new LinearGradientBrush(imageRectangle, _color  , Color.FromArgb(Reflectance,_color)  , 0F);
                    g.DrawImage(e.Forerground, Point.Empty);
                    
                    g.FillRectangle(lbrush, 0, 0, e.Forerground.Width, e.Forerground.Height);
                    break;
                case Direction.Top:
                    mat.Translate(0,-_hostSize.Height);
                    g.Transform = mat;
                     // Draw Gradient
                    imageRectangle = new Rectangle(0, 0, e.Forerground.Width, e.Forerground.Height);
                    lbrush = new LinearGradientBrush(imageRectangle, _color  , Color.FromArgb(Reflectance,_color)  , 90F);
                    g.DrawImage(e.Forerground, Point.Empty);
                    g.FillRectangle(lbrush, imageRectangle );
                    break;
                case Direction.Right:
                    mat.Translate(_hostSize.Width  ,0);
                    g.Transform = mat;
                     // Draw Gradient
                    imageRectangle = new Rectangle(0, 0, e.Forerground.Width, e.Forerground.Height);
                    lbrush = new LinearGradientBrush(imageRectangle, _color  , Color.FromArgb(Reflectance,_color)  , 180F);
                    g.DrawImage(e.Forerground, Point.Empty);
                    g.FillRectangle(lbrush, 0, 0, e.Forerground.Width, e.Forerground.Height);
                    break;
                    break;
                case Direction.Bottom:
                    
                    g.ScaleTransform(1F, -1F);
                    g.TranslateTransform(0, -(e.ScreenerSize.Height + distance   )   );
                    g.DrawImage(e.Forerground, e.Location);

                    imageRectangle = new Rectangle(0, 0, e.Forerground.Width, e.Forerground.Height);
                    lbrush = new LinearGradientBrush(imageRectangle, _color  , Color.FromArgb(255-Reflectance,_color)  , 90F);
                    g.FillRectangle(lbrush, imageRectangle );








                    //pt = new Point(0 , _hostSize.Height);
                    //mat.RotateAt(180, pt);
                    //mat.Translate(-e.Forerground.Width ,0 );
                    
                    //g.Transform = mat;
                    //g.ScaleTransform(1, -1);
                    //// Draw Gradient
                    // imageRectangle = new Rectangle(0, 0, e.Forerground.Width, e.Forerground.Height);
                    // lbrush = new LinearGradientBrush(imageRectangle, _color  , Color.FromArgb(Reflectance,_color)  , 90F);
                    //g.DrawImage(e.Forerground, Point.Empty);
                    //g.FillRectangle(lbrush, 0, 0, e.Forerground.Width, e.Forerground.Height);
                    break;
                default:
                    break;
            }

            imageRectangle = Rectangle.Empty ;
            pt = Point.Empty;
            lbrush.Dispose();
            lbrush = null;

            g.Flush();
            g.Dispose();
            this.currentFrame += 1;
            return bit;
        }


  

      
    }
}
