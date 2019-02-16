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
         
        
        public static string FileExt = ".prx";
        public static string FileFilter = "XML Param files (*.prx)|*.prx";
       
        double _meshSize;
        
        internal AbMachParameters(AbMachOperation op, RunInfo runInfo, RemovalRate removalRate, Material mat, AbMachJet abmachJet,DepthInfo depthInfo,double meshSize)
        {
           
            Operation = op;             
            RunInfo = runInfo;
            RemovalRate = removalRate;
            Material = mat;
            AbMachJet = abmachJet;
            DepthInfo = depthInfo;
            _meshSize = meshSize;
            
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
            
        }

    }
  
   
    
  
    
    
}
