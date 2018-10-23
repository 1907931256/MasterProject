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

        public ModelPath Build(ToolPath inputPath, double increment, Plane xsectionPlane)
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
        private ModelPath parsePath(ToolPath inputPath, double increment)
        {
            BoundingBox ext = getBoundingBox(inputPath);
            BoundingBox jetOnBox = getJetOnBoundingBox(inputPath);

            ModelPath mp = new ModelPath(ext, jetOnBox);
            for (int i = 1; i < inputPath.Count; i++)
            {
                
            }
            return mp;
        }
    }
}
