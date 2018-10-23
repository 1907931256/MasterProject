using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolpathLib
{
    interface ICncFileParser
    {
        ToolPath ParsePath(List<string> file);
    }
}
