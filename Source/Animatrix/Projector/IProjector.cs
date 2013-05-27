using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Animatrix.Projectors
{
  public  interface IProjector
    {
         Boolean  Completed { get; set; }

       
       
         void NextFrame();
         void leaveScreen();
         void CleanMemoryFootprints();
         Object getHost();
         bool  Equals(IProjector other);
         
    }
}
