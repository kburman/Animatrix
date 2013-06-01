using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Animatrix.Screener
{
    public class Reflector : Control, IScreener
    {

        private bool ready = false;
        public  bool Ready
        {
            get { return ready; }
        }

        private Bitmap _imgToPaint = null;
        /// <summary>
        /// 
        /// </summary>
        public Bitmap imageToPaint
        {
            get
            {
                return _imgToPaint;
            }
            set
            {
                if (_imgToPaint == null)
                    _imgToPaint = value;
                else
                    lock (_imgToPaint )
                    {
                        _imgToPaint.Dispose();
                        _imgToPaint = null;
                        _imgToPaint = value;
                    }
            }
        }

        public bool HostVisible_afterAnimation { get; set; }

        private Point _rLoc = Point.Empty;
        public Point HostRelativeLocation
        {
            get
            {
                return _rLoc;
            }
        }

       

        public Point HostLocation
        {
            get
            {
                return _host.Location ;
            }
        }



        private Control _host = null;
        public Reflector(Control host)
        {
            if (host == null) return;
            if (host.IsDisposed) return;
            if (host.Parent == null) return;
            if (this.IsDisposed) return;
            _host = host;
            HostVisible_afterAnimation = true;
            setPadding(pad);
            _host.SizeChanged += _host_SizeChanged;
            _host.LocationChanged += _host_LocationChanged;
            this.Paint += ControlScreener_Paint;
            //this._host.Paint += _host_Paint;

            
        }

        //void _host_Paint(object sender, PaintEventArgs e)
        //{
        //    lock (foregroundImage)
        //    {
        //        foregroundImage.Dispose();
        //        foregroundImage = null;
        //    }

        //    lock (background)
        //    {
        //        background.Dispose();
        //        background = null;
        //    }
        //}


        void ControlScreener_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                if (_imgToPaint != null )
                {
                    lock (_imgToPaint )
                    {
                        e.Graphics.DrawImage(_imgToPaint, Point.Empty);
                    }
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        void _host_LocationChanged(object sender, EventArgs e)
        {
            setPadding(pad);
        }

        /// <summary>
        /// Size of the host has changed.
        /// so we have to also change our self.
        /// </summary>
        void _host_SizeChanged(object sender, EventArgs e)
        {
            setPadding(pad);
        }

        public void DrawAgain()
        {
            if (this.IsDisposed)
                return;
            this.Refresh();
        }

        public void coverTheHost()
        {
            if (this.IsDisposed)
                return;
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.Opaque, false);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.Selectable, false);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.UserPaint, true);
            this.DoubleBuffered = true;
             if (_host == null)
                return;
            if (_host.IsDisposed)
                return;
            this._host.Visible = true;
            int i = _host.Parent.Controls.GetChildIndex(_host);
            this.Parent = _host.Parent;
            _host.Parent.Controls.SetChildIndex(this, i);
            ready = true;

            Region rg = new Region();
            rg.Exclude(new Rectangle(HostRelativeLocation, _host.Size));
            this.Region = rg;
        }

        public void leaveScreen()
        {
            if (this.IsDisposed)
                return;
            this.Parent = null;

            if (_host == null)
                return;
            if (_host.IsDisposed)
                return;
            // Again Seting it bcz incase if it is changed after that.
            _host.Visible = this.HostVisible_afterAnimation;
        }

        private Padding pad = Padding.Empty;
        public void setPadding(Padding pad)
        {
            if (this.IsDisposed)
                return;
            if (_host == null)
                return ;
            if (_host.IsDisposed)
                return ;
            this.Location =new Point(_host.Left - pad.Left, _host.Top - pad.Top);
            this.Size = new Size(_host.Width + pad.Left + pad.Right,
                               _host.Height + pad.Top + pad.Bottom);
            _rLoc  = new Point(Math.Abs(this.Location.X - _host.Location.X),
                               Math.Abs(this.Location.Y - _host.Location.Y));
            BackColor = Color.Red ;
        }


         
        public Bitmap getBackground()
        {
            Bitmap background = null;
           if (_host == null)
                    return null;
                if (_host.IsDisposed)
                    return null;
                var bounds = this.Bounds; ;
                var w = bounds.Width;
                var h = bounds.Height;
                if (w == 0) w = 1;
                if (h == 1) h = 1;
                background = new Bitmap(w, h);
                Graphics g = Graphics.FromImage(background);
                for (int i = _host.Parent.Controls.Count - 1; i >= 0; i--)
                {
                    var c = _host.Parent.Controls[i];
                    if (c == _host) break;
                    if (c.Visible && !c.IsDisposed)
                        if (c.Bounds.IntersectsWith(bounds))
                        {
                            Bitmap tmpImg = new Bitmap(c.Width, c.Height);
                            c.DrawToBitmap(tmpImg, c.DisplayRectangle);
                            g.DrawImage(tmpImg, c.Left - bounds.Left, c.Top - bounds.Top, c.Width, c.Height);
                            tmpImg.Dispose();
                        }
                }
                g.Dispose();
               
          return background  ;
        }

        [DllImport("user32.dll")]
        public static extern bool PrintWindow(IntPtr hwnd, IntPtr hdcBlt, uint nFlags);
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);

        public System.Drawing.Bitmap CapWin(IntPtr hWnd)
        {
            System.Drawing.Rectangle rctForm = System.Drawing.Rectangle.Empty;
            using (System.Drawing.Graphics grfx = System.Drawing.Graphics.FromHdc(GetWindowDC(hWnd)))
            {
                rctForm = System.Drawing.Rectangle.Round(grfx.VisibleClipBounds);
            }
            System.Drawing.Bitmap pImage = new System.Drawing.Bitmap(rctForm.Width, rctForm.Height);
            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(pImage);
            IntPtr hDC = graphics.GetHdc();
            //paint control onto graphics using provided options        
            try
            {
                PrintWindow(hWnd, hDC, (uint)0);
            }
            finally
            {
                graphics.ReleaseHdc(hDC);
                graphics.Dispose();
            }
            return pImage;
        }

        private bool lastCapWinIsBalnk = false;
        public Bitmap getForeground()
        {
            if (lastCapWinIsBalnk )
            {
                return (Bitmap)CapWinBitBlt(_host.Handle);
            }
            var img =(Bitmap) CapWin(_host.Handle);
            if (IsBlank(img))
            {
                lastCapWinIsBalnk = true;
                return (Bitmap)CapWinBitBlt(_host.Handle);
            }
            else
                return img;
        }

        private bool IsBlank(Bitmap bmp)
        {
            // Lock the bitmap's bits.  
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = bmpData.Stride * bmp.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            // Scanning for non-zero bytes
            bool allBlack = true;
            for (int index = 0; index < rgbValues.Length; index++)
                if (rgbValues[index] != 0)
                {
                    allBlack = false;
                    break;
                }
            // Unlock the bits.
            bmp.UnlockBits(bmpData);
            return allBlack;
        }

        public Size getHostSize()
        {
            if (_host == null || _host.IsDisposed)
                return Size.Empty;
            else
                return _host.Size;
        }

        public Size getSize()
        {
            if (this.IsDisposed)
                return Size.Empty ;
            else
                return this.Size;
        }

 

        public void cleanMemoryFootprint()
        {
            if (this.IsDisposed)
                return;
            if (_host != null)
                _host = null;
            imageToPaint = null;
            pad = Padding.Empty;
            //if (foregroundImage != null )
            //{
            //    try
            //    { foregroundImage.Dispose(); }
            //    catch (Exception)
            //    { }
            //}
            //if (background  != null)
            //{
            //    try
            //    { background.Dispose(); }
            //    catch (Exception)
            //    { }
            //}
        }

        public object getHost()
        {
            return _host;
        }

        /// Helper class containing Gdi32 API functions
        private class GDI32
        {
            public int SRCCOPY = 0xcc0020;
            [DllImport("gdi32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
            // BitBlt dwRop parameter
            public static extern Int32 BitBlt(IntPtr hDestDC, Int32 x, Int32 y, Int32 nWidth, Int32 nHeight, IntPtr hSrcDC, Int32 xSrc, Int32 ySrc, Int32 dwRop);
            [DllImport("gdi32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

            public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, Int32 nWidth, Int32 nHeight);
            [DllImport("gdi32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

            public static extern IntPtr CreateCompatibleDC(IntPtr hdc);
            [DllImport("gdi32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

            public static extern Int32 DeleteDC(IntPtr hdc);
            [DllImport("gdi32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

            public static extern Int32 DeleteObject(IntPtr hObject);
            [DllImport("gdi32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

            public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hObject);
        }
        //GDI32
        /// Helper class containing User32 API functions
        public class User32
        {
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
            public struct RECT
            {
                public int left;
                public int top;
                public int right;
                public int bottom;
            }
            [DllImport("user32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
            //RECT

            public static extern IntPtr GetDesktopWindow();
            [DllImport("user32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

            public static extern IntPtr GetWindowDC(IntPtr hwnd);
            [DllImport("user32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

            public static extern Int32 ReleaseDC(IntPtr hwnd, IntPtr hdc);
            [DllImport("user32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

            public static extern Int32 GetWindowRect(IntPtr hwnd, ref RECT lpRect);

        }

        public Image CapWinBitBlt(IntPtr handle)
        {
            //int SRCCOPY = 0xcc0020;
            int SRCCOPY = 0x00EE0086;
            // get te hDC of the target window
            IntPtr hdcSrc = User32.GetWindowDC(handle);
            // get the size
            User32.RECT windowRect = new User32.RECT();
            User32.GetWindowRect(handle, ref windowRect);
            int width = windowRect.right - windowRect.left;
            int height = windowRect.bottom - windowRect.top;
            // create a device context we can copy to
            IntPtr hdcDest = GDI32.CreateCompatibleDC(hdcSrc);
            // create a bitmap we can copy it to,
            // using GetDeviceCaps to get the width/height
            IntPtr hBitmap = GDI32.CreateCompatibleBitmap(hdcSrc, width, height);
            // select the bitmap object
            IntPtr hOld = GDI32.SelectObject(hdcDest, hBitmap);
            // bitblt over
            GDI32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, SRCCOPY);
            // restore selection
            GDI32.SelectObject(hdcDest, hOld);
            // clean up
            GDI32.DeleteDC(hdcDest);
            User32.ReleaseDC(handle, hdcSrc);

            // get a .NET image object for it
            Image img = Image.FromHbitmap(hBitmap);
            // free up the Bitmap object
            GDI32.DeleteObject(hBitmap);

            return img;
        }
    }
}
