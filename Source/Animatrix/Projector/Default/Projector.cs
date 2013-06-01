using Animatrix.Animation;
using Animatrix.Screener;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;

namespace Animatrix.Projectors
{
  public   class Projector : IProjector ,IDisposable 
    {
        private IScreener _screen = null;
        private IAnimation _animation = null;
        private ManualResetEvent _wait = null;

        public ManualResetEvent Wait
        {
            get
            {
                if (_wait == null)
                    _wait = new ManualResetEvent(false);
                return _wait;
            }
        }
        public bool Completed
        {
            get
            {
                if (_animation != null) 
                    return _animation.Completed;
                else 
                    return false ;
            }
        }
       
        
        public Projector(IScreener screen,IAnimation animation)
        {
            // Check if we don't have null value.
            if (screen == null || animation == null) return;
            _screen = screen;
            _animation = animation;
            _screen.setPadding(_animation.getPadding(_screen.getHostSize()));
             _wait = new ManualResetEvent(false);
        }




        private AnimationFrameArgs e;
        public void NextFrame()
        {
            // check if animation is completed then 
            // ask IScreener to leave screen.
            //
            //else
            //
            // if screener is not in position then ask to do it.
            // now compute nextframe with animation and pass
            // it to screen and ask to draw again.

            if (_animation.Completed) return;
            if ( _animation.Started )
            {               
                _screen.coverTheHost();
            }
            if (true)
            {

                Bitmap nxtfrm;
                Size sz = _screen.getSize();
                nxtfrm = new Bitmap(sz.Width, sz.Height);
                Graphics g = Graphics.FromImage(nxtfrm);
                if (e == null)
                {
                    e = new AnimationFrameArgs(ref _screen);
                }
               
                e.graphics = g;
                _animation.nextFrame(ref e);
                _screen.DrawAgain();
                g.Dispose();
                g = null;
                sz = Size.Empty;

            
               
                // Check if disposing nxtfrm dispose imagetoPaint also or not.
                // if (_screen.imageToPaint != null)  _screen.imageToPaint.Dispose();
                _screen.imageToPaint = nxtfrm ;
            }
        }

        public void leaveScreen()
        {
            _screen.leaveScreen();
            if(e != null)
             e.Dispose();
             e = null;
        }

        public void CleanMemoryFootprints()
        {
            if (_screen != null) 
            { 
                _screen.cleanMemoryFootprint();
                _screen = null;
            }
           if (_animation != null)
           {
               _animation.cleanMemoryFootprint();
               _animation = null;
           }
           if (_wait != null) 
           { 
               _wait  = null;
           }
           if (e != null)
           { 
               e.Dispose(); 
               e = null; 
           }

        }

        public object getHost()
        {
            if (_screen != null)
                return _screen.getHost();
            else
                return null;
        }

        public bool Equals(ref IProjector other)
        {
            if (other == null) return false ;

            var otherhost = other.getHost();
            if (otherhost == null) return false;

            var thisHost = this.getHost();
            if (thisHost == null) return false;

            bool match = thisHost.Equals(otherhost);

            thisHost = null;
            otherhost = null;
            return match;
            

        }


        public void Dispose()
        {
            this.CleanMemoryFootprints();
            Wait.Close();
        }

    }
}
