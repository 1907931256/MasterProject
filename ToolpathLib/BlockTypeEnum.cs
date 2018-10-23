using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolpathLib
{
    public enum BlockType
    {
        Rapid,
        Linear,
        CWArc,
        CCWArc,
        Delay,
        FiveAxis,
        Command,
        Comment,
        Unknown,
    }
}
