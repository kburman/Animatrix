using Animatrix.Screener;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Animatrix
{
  public   class AnimationFrameArgs : IDisposable 
    {

      private const bool StaticImage = false;
      private IScreener sc;
      private Bitmap bck = null;
        /// <summary>
        /// Image of area coverd by the control whithout it.
        /// </summary>
        /// <value>Can be null</value>
        public Bitmap Background 
        { 
            get
            {
                if (sc == null) return null;
                if (StaticImage)
                {
                    if (bck == null) bck = sc.getBackground();
                    return bck;
                }
                else
                { return sc.getBackground(); }
                
            }
        }
        private Bitmap frg = null;
        /// <summary>
        /// Image of control.
        /// </summary>
        public Bitmap Forerground 
        {
            get
            {
                if (sc == null) return null ;
                if (StaticImage )
                {
                    if (frg == null) frg = sc.getForeground();
                     return frg; 
                }
                else
                {
                    return sc.getForeground();
                }
                //return sc.getForeground();
            }
        }
        /// <summary>
        /// Location of control.
        /// </summary>
        public Point Location 
        {
            get 
            {
                if (sc == null) return Point.Empty ;
                 return  sc.HostRelativeLocation;
            }
        }
        /// <summary>
        /// Size of screener which is hiding control.
        /// </summary>
        public Size ScreenerSize
        {
            get
            {
                if (sc == null) return Size.Empty ;
                return sc.getSize();
            }
        }
        public Graphics graphics { get; set; }

        public AnimationFrameArgs(ref IScreener screen)
        {
            if (screen == null) return;
            sc = screen;
        }


        public void Dispose()
        {
            if (sc != null) 
            { 
                sc.cleanMemoryFootprint();
                sc = null; 
            }
            if (graphics != null) 
            { 
                graphics.Dispose();
                graphics = null;
            }

        }
    }
}
