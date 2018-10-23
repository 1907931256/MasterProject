using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeometryLib;
namespace SurfaceModel
{
    public class QuadTreePoint
    {
        Vector3 position;
       
        public Vector3 Position { get { return position; } set { position = value; } }
        
        public QuadTreePoint(Vector3 pt)
        {
            position = pt;
            
        }
        public QuadTreePoint(QuadTreePoint pt)
        {
            position = new Vector3(pt.Position);
          
        }
    }
}
