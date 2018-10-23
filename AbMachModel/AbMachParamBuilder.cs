using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AWJModel;

namespace AbMachModel
{
    public class AbMachParamBuilder
    {
        public static AbMachParameters Build()
        {
            double meshSize = .001;
            AbMachOperation op = AbMachOperation.OTHER;
            RunInfo runInfo = new RunInfo();
            RemovalRate removalRate = new RemovalRate();
            Material mat = new Material();
            AbMachJet abmachJet= new AbMachJet();
            DepthInfo depthInfo = new DepthInfo();
            
            return new AbMachParameters(op, runInfo, removalRate, mat, abmachJet, depthInfo, meshSize);
        }
        public static AbMachParameters Build(AbMachOperation op, RunInfo runInfo, RemovalRate removalRate,
            Material mat, AbMachJet abmachJet, DepthInfo depthInfo, double meshSize)
        {
           return new AbMachParameters(op, runInfo, removalRate, mat, abmachJet, depthInfo, meshSize);
        }
    }
}
