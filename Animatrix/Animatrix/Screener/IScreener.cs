using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Animatrix.Screener
{
 public    interface IScreener
    {
        /// <summary>
        /// Screener is in Postion and ready to display animation.
        /// </summary>
         Boolean Ready { get; set; }
        /// <summary>
        /// Image which is being currently displayed on the screener.
        /// </summary>
         Bitmap imageToPaint { get; set; }
        /// <summary>
        /// Should the host should be visible after the animation is completed.
        /// </summary>
         Boolean HostVisible_afterAnimation { get; set; }
        /// <summary>
        /// 
        /// </summary>
         Point HostRelativeLocation { get; set; }
        /// <summary>
        /// 
        /// </summary>
         Point  HostLocation { get; set; }
        /// <summary>
        /// Image of area coverd by the control whithout it.
        /// </summary>
         Bitmap Foreground { get; set; }
        /// <summary>
        /// Image of control.
        /// </summary>
         Bitmap Background { get; set; }

        /// <summary>
        /// call the Refresh() method of Screener.
        /// </summary>
         void DrawAgain();
        /// <summary>
        /// Screener take the postion in front of host to show the animation.
        /// </summary>
         void coverTheHost();
        /// <summary>
        /// After the animation is completed this called so that screener leave the 
        /// control.Parent.
        /// </summary>
         void leaveScreen();
        /// <summary>
        /// set extra surrounding space around the host which is required to show animation
        /// such Blink animation required extra space around.
        /// </summary>
         void setPadding(Padding pad);
        /// <summary>
        /// returns a fresh background image.
        /// </summary>
        /// <returns></returns>
         Bitmap getBackground();
        /// <summary>
        /// returns a fresh foreground image.
        /// </summary>
        /// <returns></returns>
         Bitmap getForeground();
        /// <summary>
        /// returns the size of control which is being animated.
        /// </summary>
        /// <returns></returns>
         Size getHostSize();
        /// <summary>
        /// retuns the size of screener.
        /// </summary>
        /// <returns></returns>
         Size getSize();

        /// <summary>
        /// Similar to dispose() method;
        /// </summary>
         void cleanMemoryFootprint();

         Object getHost();



    }
}
