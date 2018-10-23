using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ToolpathLib;
using DrawingIO;
using SurfaceModel;

namespace AbMachModel
{
    
    public class AbmachSimModel2D
    {
        
        AbmachSurface surf;        
        ModelPath path;
        AbMachParameters abmachParams;
        RunInfo runInfo;
        RemovalRate matRemRate;
        public AbmachSimModel2D(AbmachSurface modelSurf, ModelPath path, AbMachParameters parms)
        {
            this.surf = modelSurf;
            this.path = path;
            this.abmachParams = parms;
            this.runInfo = parms.RunInfo;
        }
        public void Initialize()
        {

        }
        public void SubtractSurface2D(CancellationToken ct,IProgress<int> progress)
        {
            int jetR = abmachParams.AbMachJet.JetRadius;
            matRemRate = abmachParams.RemovalRate;
            int prevXIndex = surf.Xindex(path.Entities[0].Position.X);
            int prevYIndex = surf.Xindex(path.Entities[0].Position.Y);
            for (int iteration = 0; iteration < runInfo.Iterations;iteration++ )// iterations
            {
                runInfo.CurrentIteration += 1;
                for (int run = 0; run < runInfo.Runs; run++)//runs
                {
                    runInfo.CurrentRun += 1;
                    foreach (ModelPathEntity ent in path.Entities)//path 
                    {
                        int xIndex = surf.Xindex(ent.Position.X);
                        int yIndex = surf.Yindex(ent.Position.Y);
                        double deltaIndex = Math.Sqrt(Math.Pow(xIndex - prevXIndex, 2) + Math.Pow(yIndex - prevYIndex, 2));
                        prevXIndex = xIndex;
                        prevYIndex = yIndex;
                        if (deltaIndex != 0)
                        {
                            double feedFactor = feedrateFactor(ent.Feedrate, deltaIndex, matRemRate);
                            //subtract jet footprint and put into temp surface
                            //temp surface so that slope calc is not affected by depth changes
                            for (int a = xIndex - jetR; a <= xIndex + jetR; a++) 
                            {
                                for (int b = yIndex - jetR; b <= yIndex + jetR; b++)
                                {
                                    double depth = feedFactor * slopeFactor(surf.Normal(a,b)) * surf.GetValue(a,b).MachIndex * abmachParams.AbMachJet.FootPrint(a - xIndex + jetR, b - yIndex + jetR);
                                    surf.SetValue(AbmachValType.Temp,depth, a, b);
                                }
                            }
                            //smooth surface and place in temp surface smooth spikes and pits
                            for (int a = xIndex - jetR; a <= xIndex + jetR; a++) 
                            {
                                for (int b = yIndex - jetR; b <= yIndex + jetR; b++)
                                {
                                   smoothValue(a, b);                                    
                                }
                            }
                            //replace model surface with smoothed surface
                            for (int a = xIndex - jetR; a <= xIndex + jetR; a++) 
                            {
                                for (int b = yIndex - jetR; b <= yIndex + jetR; b++)
                                {
                                    surf.SetValue(AbmachValType.Model,surf.GetValue(a, b).Temp, a, b);
                                }
                            }
                        }
                    }//next toolpath segment
                    //get depth at depth location
                    abmachParams.DepthInfo.DepthAtLocation = getDepth(abmachParams.DepthInfo.LocationOfDepthMeasure);
                    //adjust material removal rate if requested
                    if (abmachParams.RunInfo.RunType == ModelRunType.NewMRR)     
                    {                        
                        abmachParams.RemovalRate = adjustMRR(abmachParams.DepthInfo.DepthAtLocation, matRemRate, runInfo); 
                    }
                   
                }//next run
                if (abmachParams.RunInfo.RunType == ModelRunType.NewFeedrates)
                {
                    adjustFeedRates();
                }
            }//next iteration
        }
        
          
        void smoothValue( int a, int b)
        {
            if ( surf.IsPeak(a,b) || surf.IsValley(a,b))
            {
                surf.SmoothAt(a, b);
            }
           
        }
        /// <summary>
        /// calc change in jet surface speed due to B axis move
        /// </summary>
        /// <param name="depth"></param>
        /// <param name="ent"></param>
        /// <returns></returns>
        double deltaBAxisFactor(double depth, ModelPathEntity ent)
        {
            //TODO calc deltab

            return 1;
        }
        double feedrateFactor(double feedrate, double deltaIndex, RemovalRate mrr)
        {
            return deltaIndex * mrr.DepthPerPass * mrr.SurfaceSpeed / feedrate;
        }
        double slopeFactor(Vector3 surfaceNormal)        {           
           
            double incidentAngle = Math.Abs(Math.Acos(surfaceNormal.Z / surfaceNormal.Length));
            return Math.Abs(Math.Cos(incidentAngle - abmachParams.Material.CriticalRemovalAngle));
        }
        void adjustFeedRates()
        {
            foreach (ModelPathEntity ent in path.Entities)//path 
            {           
                ent.Depth = surf.GetValue(ent.Position.X, ent.Position.Y).Depth;
            }
        }
        RemovalRate adjustMRR(double currentDepth, RemovalRate currentMrr,RunInfo runInfo)
        {
            //TODO calc adjust mrr
            return currentMrr;
        }

        double getDepth(Vector3 depthLocation)
        {
            return surf.GetValue(depthLocation.X, depthLocation.Y).Depth;
        }
    }
}
