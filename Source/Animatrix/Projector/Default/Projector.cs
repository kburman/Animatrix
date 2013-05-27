
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
  public   class Projector : IProjector 
    {
      public bool Completed { get; set; }
      IAnimation _animation;
      IScreener _screener;
      bool hvisible;
      /// <summary>
      /// Signal when Screener is removed.
      /// </summary>
    public   ManualResetEvent wait = new ManualResetEvent(false);




    public ManualResetEvent  Inti(IScreener sc, IAnimation ani, bool Show)
    {
        this._animation = ani;
        this._screener = sc;
        this.hvisible = Show;
        //feeding
        _screener.setPadding(_animation.getPadding(this._screener.getHostSize()));
        _screener.Background = _screener.getBackground();
        return wait;
    }
  

        public void NextFrame()
        {
            if (_animation.Completed) return;
            if (_screener.Ready)
            {
                Bitmap nxtfrm;
                //_screener.update();
                AnimationFrameArgs e = new AnimationFrameArgs();
                e.Background = _screener.Background;
                e.Forerground = _screener.Foreground;
                e.Location = _screener.HostRelativeLocation;
                e.ScreenerSize = _screener.getSize();
                nxtfrm = _animation.nextFrame(e);
                e = null;
                _screener.imageToPaint = nxtfrm;
                _screener.DrawAgain();
                nxtfrm.Dispose();
                if (_animation.Completed)
                {
                    this.Completed = true;
                }
            }
            else
            {
                _screener.HostVisible_afterAnimation = this.hvisible;
                _screener.coverTheHost();
                
            }
            
        }


        public void CleanMemoryFootprints()
        {
            this.leaveScreen();
            _animation.cleanMemoryFootprint();
            _screener.cleanMemoryFootprint(); 
            
        }


        public void leaveScreen()
        {
            _screener.leaveScreen();
        }


        public bool  Equals(IProjector other)
        {
            Object host = other.getHost();
            if (host == null) return false;
            if (!(host.GetType().Equals(this.getHost().GetType()))) return false; // Check if both are Button or PictureBox etc.
            return this.getHost().Equals(other.getHost());
        }


        public object getHost()
        {
            return _screener.getHost();
        }
    }
}
