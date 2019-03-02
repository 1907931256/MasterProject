using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeometryLib;
namespace ToolpathLib
{
    class LinePathEntity:PathEntity5Axis
    {
        public bool RapidMove { get; set; }
        public bool PlungeMove { get; set; }    
        
        
 
        
        public LinePathEntity(BlockType type)
        {
            base.Type = type;                            
            RapidMove = type==BlockType.Rapid;
            
        }

    }
}
