using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolpathLib
{
    interface ICncFileParser
    {
        ToolPath5Axis ParsePath(List<string> file);
    }
}
