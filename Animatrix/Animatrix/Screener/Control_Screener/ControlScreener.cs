using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Animatrix.Screener
{
  public   class ControlScreener :Control,IScreener
    {

        private Control _host = null;


        public bool Ready { get; set; }
        public System.Drawing.Bitmap imageToPaint { get; set; }
        public bool HostVisible_afterAnimation { get; set; }
        /// <summary>
        /// It will get value after execution of setPadding(pad) method.
        /// By default when Constructor of this class is called padding is setPadding(0) is called;
        /// </summary>
        public Point HostRelativeLocation { get; set; }
        public Point HostLocation { get; set; }
        public Bitmap Foreground { get; set; }
        public Bitmap Background { get; set; }


        /// <summary>
        /// Create a new Control Screener 
        /// </summary>
        /// <param name="Host"> Control for which screener is to be created.</param>
        public  ControlScreener(Control Host)
        {
            if (Host == null | Host.IsDisposed) return;
            if (Host.Parent == null) return;
            this._host = Host;
            this.setPadding(new Padding(0));
            this.Foreground = this.getForeground();
            this.Background  = this.getBackground();
            this.imageToPaint = null;
            this.HostRelativeLocation = Point.Empty;
            this.Ready = false;
            this.Paint += ControlScreener_Paint;
        }

        void ControlScreener_Paint(object sender, PaintEventArgs e)
        {
            if (this.imageToPaint!=null) e.Graphics.DrawImageUnscaled(this.imageToPaint, Point.Empty);
        }

        /// <summary>
        /// This will hide the host by placing screener over it.
        /// </summary>
        public void coverTheHost()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.FixedHeight, true);
            SetStyle(ControlStyles.FixedWidth, true);
            SetStyle(ControlStyles.Opaque, false);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.Selectable, false);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.UserPaint, true);
            this.DoubleBuffered = true;
            int i = _host.Parent.Controls.GetChildIndex(_host);
            this.Parent = _host.Parent;
            _host.Parent .Controls.SetChildIndex(this, i);
            _host.Visible = this.HostVisible_afterAnimation;
            this.Visible = true;
            this.Ready = true;
        }
        
        /// <summary>
        /// Leave the host.parent and host be visible depending upon the value
        /// of this.HostVisible_afterAnimation
        /// </summary>
        public void leaveScreen()
        {
            this.Parent = null;            
        }

        /// <summary>
        /// Set the required space around the screener.
        /// </summary>
        /// <param name="pad"></param>
        public void setPadding(Padding pad)
        {
            Point pt = new Point(_host.Left - pad.Left, _host.Top - pad.Top);
            Size sz = new Size(_host.Width + pad.Left + pad.Right,
                               _host.Height + pad.Top + pad.Bottom);
            this.Location = pt;
            this.Size = sz; 

            int x = this.Location.X - _host.Location.X;
            int y = this.Location.Y - _host.Location.Y;
            this.HostRelativeLocation = new Point(Math.Abs( x),Math.Abs (y));
        }

        public System.Drawing.Bitmap getBackground(Rectangle area)
        {
            var bounds = area ;
            var w = bounds.Width;
            var h = bounds.Height;
            if (w == 0) w = 1;
            if (h == 1) h = 1;
            Bitmap bmp = new Bitmap(w, h);

            var clientRect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            PaintEventArgs ea = new PaintEventArgs(Graphics.FromImage(bmp), clientRect);

            //ea.Graphics.Clear(ctrl.Parent.BackColor);

            for (int i = _host.Parent.Controls.Count - 1; i >= 0; i--)
            {
                var c = _host.Parent.Controls[i];
                if (c == _host ) break;
                if (c.Visible && !c.IsDisposed)
                    if (c.Bounds.IntersectsWith(bounds))
                    {
                        using (Bitmap cb = new Bitmap(c.Width, c.Height))
                        {
                            c.DrawToBitmap(cb, new Rectangle(0, 0, c.Width, c.Height));
                            /*if (c == ctrl)
                                ea.Graphics.SetClip(clipRect);*/
                            ea.Graphics.DrawImage(cb, c.Left - bounds.Left, c.Top - bounds.Top, c.Width, c.Height);
                        }
                    }
                if (c == _host) break;
            }

            ea.Graphics.Dispose();

            return bmp;
        }

        public System.Drawing.Bitmap getBackground()
        {
            return getBackground(this.Bounds);
        }


        public System.Drawing.Bitmap getForeground()
        {
            Bitmap bmp = new Bitmap(this.Width, this.Height);

            if (!_host.IsDisposed)
                _host.DrawToBitmap(bmp, new Rectangle(_host.Left - Left, _host.Top - Top, _host.Width, _host.Height));

            return bmp;
        }

        public System.Drawing.Size getHostSize()
        {
            return _host.Size;
        }

        public System.Drawing.Size getSize()
        {
            return this.Size;
        }



        public void DrawAgain()
        {
            this.Refresh();
        }


        public void cleanMemoryFootprint()
        {
            this.Foreground = null;
            this.Background = null;
            this.imageToPaint = null;
            this.Dispose();
        }


        public object getHost()
        {
            return _host;
        }
    }
}
