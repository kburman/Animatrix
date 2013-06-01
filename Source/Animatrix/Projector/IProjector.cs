using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Animatrix.Projectors
{
  public  interface IProjector
    {
         /// <summary>
         /// True if Work of IProjector  is completed.
         /// </summary>
         Boolean  Completed { get;   }
         /// <summary>
         /// Create next fram of animation
         /// </summary>     
         void NextFrame();
         /// <summary>
         /// Leave the screen
         /// </summary>
         void leaveScreen();
         /// <summary>
         /// Dispose
         /// </summary>
         void CleanMemoryFootprints();
         /// <summary>
         /// returns the Host that is covered.
         /// </summary>
         /// <returns>Control or null</returns>
         Object getHost();
         bool  Equals(ref IProjector other);
         
    }
}
