﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolpathLib
{
    interface IModelPathBuilder
    {
        ModelPath Build(ToolPath5Axis _inputPath, double increment);
    }
}
