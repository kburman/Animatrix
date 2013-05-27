using System;
using System.Drawing;
using System.Windows.Forms;

namespace Animatrix.Animation
{
 public    interface IAnimation
    {
        /// <summary>
        /// Return True if  Animation is Started.
        /// Return False if Animation is not Started.
        /// </summary>
         Boolean  Started { get; set; }
        /// <summary>
        /// Return True if  Animation is Completed.
        /// Return False if Animation is not Completed.
        /// </summary>
         Boolean  Completed { get; set; }
        /// <summary>
        /// Frame No. at which animation should start.
        /// </summary>
         int startFrame { get; set; }
        /// <summary>
        /// current frame No.
        /// </summary>
         int currentFrame { get; set; }

        /// <summary>
        /// Dispose all the resource in use to free up memory used by this class
        /// It will end animation irespective of wheather it is completed or not.
        /// </summary>
         void cleanMemoryFootprint();
        /// <summary>
        /// Extra space required for animation
        /// </summary>
        /// <param name="hostSize">Size of the Control which is to be animated.</param>
        /// <returns></returns>
         Padding getPadding(Size hostSize);
        /// <summary>
        /// Compute and draw the next frame and the return it in form of bitmap.
        /// </summary>
        /// <returns>Animated Image of control</returns>
        /// <remarks>Size of the bitmap will be of Screener so there is no need
        /// to use g.Draw(x,y,image)
        /// you should use g.Draw(0,0,image)
        ///</remarks>
         Bitmap nextFrame(AnimationFrameArgs e);
    }
}
