using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWJModel
{
    /// <summary>
    /// contains machining operation type
    /// </summary>
    public enum MachiningOpType
    {
        Unknown,
        RotaryMill,
        Turning,
        LinearMill,
        SingleChannel,
        VariableWidthChannel,
        BoreMill,
        Cutting,
        Drilling,

    }
}
