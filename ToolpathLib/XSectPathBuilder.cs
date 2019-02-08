using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryLib;
namespace ToolpathLib
{
    class XSectPathBuilder : ModelPathBuilder
    {
        Plane _xSectPlane;

        public XSecPathList Build(ToolPath inputPath, double increment, Plane xsectionPlane)
        {
            try
            {
                _xSectPlane = xsectionPlane;
                if (inputPath.Count == 0)
                    throw new InvalidOperationException("inputpath length must be > 0");
                if (increment <= 0)
                    throw new InvalidOperationException("path increment must be > 0");

                return parsePath(inputPath, increment);

            }
            catch (Exception)
            {
                throw;
            }
        }
        private XSecPathList parsePath(ToolPath inputPath, double increment)
        {
            
            var mp = new XSecPathList();
            int j = 0;
            for (int i = 1; i < inputPath.Count; i++)
            {
               if( inputPath[i].JetOn)
               {
                    if(inputPath[i-1].Feedrate != inputPath[i].Feedrate)
                    {
                        var xpe = new XSectionPathEntity()
                        {
                            Feedrate = inputPath[i - 1].Feedrate.Value,                        
                            CrossLoc = inputPath[i - 1].PositionAsVector.Y,
                            PassExecOrder = j++
                        };
                        mp.Add(xpe);
                    }
               }
            }
            return mp;
        }
    }
}
