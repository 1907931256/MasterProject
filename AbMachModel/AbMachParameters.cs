using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AWJModel;

namespace AbMachModel
{  
    public class AbMachParameters
    {
         public AbMachOperation Operation { get;  set; }
         public RunInfo RunInfo { get;  set;}
         public RemovalRate RemovalRate { get; set;} 
         public Material Material { get;  set;}
         public AbMachJet AbMachJet { get;  set;}
         public DepthInfo DepthInfo { get;  set;}
         public double SmoothingWindowWidth { get; set; }
         public double MeshSize { get { return _meshSize; } set { _meshSize = value; } }
         public double MrrNormalizeCoeff { get { return _mrrNormalizeCoeff; } }
        
        public static string FileExt = ".prx";
        public static string FileFilter = "XML Param files (*.prx)|*.prx";
        private double _mrrNormalizeCoeff;
        double _meshSize;
        public void calcNormalCoeff(double _meshSize)
        {
            double rmax = AbMachJet.Diameter/2;
            double r = 0;

            double dr;
            if (_meshSize > 0)
            {
                dr = _meshSize;
            }
            else
            {
                dr = rmax / 50;
            }
            double mrrSum = 0;
            
            var mrrValues=new List<double>();
            while(r<=rmax)
            {
                 mrrValues.Add (AbMachJet.RemovalRateAt(r));                
                r += dr;               
            }
            for (int i = 0; i < mrrValues.Count; i++)
            {
                mrrSum += mrrValues[i];
            }
            _mrrNormalizeCoeff =  (mrrSum * 2);
        }
        internal AbMachParameters(AbMachOperation op, RunInfo runInfo, RemovalRate removalRate, Material mat, AbMachJet abmachJet,DepthInfo depthInfo,double meshSize)
        {
           
            Operation = op;             
            RunInfo = runInfo;
            RemovalRate = removalRate;
            Material = mat;
            AbMachJet = abmachJet;
            DepthInfo = depthInfo;
            _meshSize = meshSize;
            calcNormalCoeff(_meshSize);
        }
        internal AbMachParameters()
        {
            _meshSize = .005;
            AbMachJet = new AbMachJet();
            RunInfo = new RunInfo();
            RemovalRate = new RemovalRate();
            Material = new Material();
            Operation = AbMachOperation.OTHER;
            DepthInfo = new DepthInfo();
            
            calcNormalCoeff(_meshSize);
        }

    }
  
   
    
  
    
    
}
