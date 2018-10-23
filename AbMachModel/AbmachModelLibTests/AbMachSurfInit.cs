using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbMachModel;
namespace AbmachModelLibTests
{
    class AbMachSurfInit
    {
        DrawingIO.Vector2 min;
        DrawingIO.Vector2 max;
        double meshSize;
        AbMachSurfaceBuilder abmachSurfBuilder;
        public void init()
        {
            min = new DrawingIO.Vector2(-1, -1);
            max = new DrawingIO.Vector2(1, 1);
            meshSize = .005;
            abmachSurfBuilder = new AbMachSurfaceBuilder(min, max, meshSize);
        }
    }
}
