using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Animatrix
{
  public   class AnimationFrameArgs
    {
        /// <summary>
        /// Image of area coverd by the control whithout it.
        /// </summary>
        public Bitmap Background { get; set; }
        /// <summary>
        /// Image of control.
        /// </summary>
        public Bitmap Forerground { get; set; }
        /// <summary>
        /// This is imgToPaint varibale of IScreener interal use .
        /// This property is created to save some more space.
        /// </summary>
        public Bitmap imgToPaint { get; set; }
        /// <summary>
        /// Location of control.
        /// </summary>
        public Point Location { get; set; }
        /// <summary>
        /// Size of screener which is hiding control.
        /// </summary>
        public Size ScreenerSize { get; set; }


        internal void Dispose()
        {
            Background.Dispose();
            Forerground.Dispose();

        }
    }
}
