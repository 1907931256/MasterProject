using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SurfaceFileLib
{
    /// <summary>
    /// contains header info for ply file
    /// </summary>
    public class PlyHeader
    {
        public PlyHeader()
        {
            Elements = new List<PlyElement>();
        }
        public IList<PlyElement> Elements { get; set; }
    }
}
