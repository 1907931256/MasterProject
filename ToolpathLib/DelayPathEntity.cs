using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolpathLib
{
    class DelayPathEntity:PathEntity5Axis
    {
        public double Delay{get;set;}
        public DelayPathEntity()
        { 
            base.Type = BlockType.Delay;
        }
    }
}
