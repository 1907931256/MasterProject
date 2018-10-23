using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbMachModel
{
    /// <summary>
    /// used by abmachsurface to identify what type of surface is in start,target, & miIndex
    /// </summary>
    public enum SurfaceInputType
    {
        Constant,
        CSV,
        STL,        
        Abmach
    }
}
