using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolpathLib;
using GeometryLib;
using SurfaceModel;
using System.Threading;
using AbMachModel;
using FileIOLib;
using DwgConverterLib;
namespace AbMachModel
{
    public class AbmachSimModel3D : IAbmachModel<AbmachPoint>
    {
        ISurface<AbmachPoint> surface;
        ISurface<AbmachPoint> initialSurface;
        ISurface<AbmachPoint> targetSurface;
        ModelPath path;
        AbMachParameters abmachParams;
        RunInfo runInfo;
        RemovalRate currentRemovalRate;
        DepthInfo depthInfo;
        double jetRadius;

        public ISurface<AbmachPoint> GetSurface()
        {
            return surface;
        }
        public ModelPath GetPath()
        {
            return path;
        }
        public RemovalRate GetRemovalRate()
        {
            return currentRemovalRate;
        }

        public AbmachSimModel3D(ISurface<AbmachPoint> initialSurface,ISurface<AbmachPoint>targetSurface,  ModelPath path, AbMachParameters parms)
        {
            surface = initialSurface;
            this.targetSurface = targetSurface;
            this.initialSurface = initialSurface.Clone();
            this.path = path;
            abmachParams = parms;
            runInfo = parms.RunInfo;
            jetRadius = abmachParams.AbMachJet.Diameter/2.0;
            currentRemovalRate = parms.RemovalRate;
            depthInfo = abmachParams.DepthInfo;
           
        }
        public void Run(CancellationToken ct, IProgress<int> progress)
        {
            try
            {
                double searchRadius = jetRadius + surface.MeshSize;

                int pcount = path.Count * runInfo.Runs * runInfo.Iterations;
                int count = 0;
                List<string> pointlist = new List<string>();
                pointlist.Add("run,Nx,Ny,Nz,x,z,pointradius,jetfactor,incidentangle,slopefactor");

                double spFactor = spacingFactor();
                double testJetRadius = jetRadius + surface.MeshSize;
                for (int iteration = 1; iteration <= runInfo.Iterations; iteration++)
                {
                    runInfo.CurrentIteration = iteration;


                    for (int run = 1; run <= runInfo.Runs; run++)
                    {
                        runInfo.CurrentRun = run;
                        int pathCount = 0;
                        foreach (ModelPathEntity mpe in path)
                        {
                            if(!ct.IsCancellationRequested)
                            {
                                double feedFactor = currentRemovalRate.DepthPerPass * spFactor * feedrateFactor(mpe.Feedrate.Value, currentRemovalRate);
                                pathCount++;
                                if (mpe.JetOn && feedFactor != 0)
                                {
                                        BoundingBox searchBox = BoundingBoxBuilder.GetSearchCylinder(surface.BoundingBox, mpe.PositionAsVector, mpe.JetVector, searchRadius);
                                        List<AbmachPoint> jetIntersectList = surface.GetPointsInsideBox(searchBox);
                                    
                                    Octree<AbmachPoint> localSurface = OctreeBuilder<AbmachPoint>.Build(jetIntersectList,  surface.MeshSize);// OctreeBuilder<AbmachPoint>.Build(jetIntersectList, searchBox, surface.MeshSize);
                                    
                                    var newPts = new List<AbmachPoint>();
                                        var mpeV = new Vector3(mpe.Position.X, mpe.Position.Y, 0);
                                        var jetRay = new Ray(mpe.PositionAsVector, mpe.JetVector);

                                        foreach (AbmachPoint jetPt in localSurface.GetAllPoints())
                                        {
                                            if(jetPt != null)
                                            {
                                                //var jetV = new Vector3(jetPt.Position.X, jetPt.Position.Y, 0);

                                                //double pointRadius = jetV.DistanceTo(mpeV);
                                                double pointRadius = Geometry.RayPointDistance(jetRay, jetPt.Position);
                                                if (pointRadius <= testJetRadius)
                                                {
                                                    double jFactor = jetFactor(pointRadius);
                                                    if (jFactor > 0)
                                                    {
                                                    Vector3 localNormal = new Vector3(0, 0, 1); // localSurface.GetNormalAt(jetPt.Position);
                                                        if (localNormal.Length == 0)
                                                            localNormal = new Vector3(mpe.JetVector);
                                                        double angle = incidentAngle(mpe.JetVector, localNormal);
                                                        double slFactor = slopeFactor(angle);
                                                        //debug 
                                                        

                                                         if (jetPt.Position.X >0.006 && jetPt.Position.X < .1 && jetPt.Position.Y>.072 && jetPt.Position.Y<.102)
                                                             pointlist.Add(run.ToString() + "," + jetPt.Position.X.ToString("f5") + "," +  jetPt.Position.Y.ToString("f5") + "," + jetPt.Position.Z.ToString("f5") + "," +pointRadius.ToString("f5")+","+ jFactor.ToString("F5")+","+ angle.ToString("F5")+","+ slFactor.ToString("F5") );
                                                        //debug 

                                                        Vector3 materialRemoved = (feedFactor * slFactor * jFactor) * mpe.JetVector;
                                                         
                                                        Vector3 newPosition = jetPt.Position - materialRemoved;
                                             
                                                        jetPt.Position = newPosition;
                                                        jetPt.Normal = localNormal;
                                                        jetPt.OriginalPosition = jetPt.OriginalPosition;
                                                        jetPt.JetHit = true;
                                                        newPts.Add(jetPt);
                                                    }

                                                }
                                                else
                                                {
                                                    newPts.Add(jetPt);
                                                }
                                            }//end foreach jetPt 
                                        }                                            
                                         surface.Insert(newPts);
                                                                    
                                }//endif jeton                            
                            }                                        
                            progress.Report(100 * ++count / pcount);
                        }//next path entity                    
                        if (runInfo.RunType == ModelRunType.NewMRR)
                        {
                            currentRemovalRate = newRemovalRate(path, runInfo, currentRemovalRate, depthInfo);
                        }

                    } //next run

                    if (runInfo.RunType == ModelRunType.NewFeedrates && runInfo.CurrentIteration < runInfo.Iterations)
                    {
                        path = newFeedrates(path, depthInfo);
                        resetSurface();
                    }

                }//next iteration
                FileIOLib.FileIO.Save(pointlist, "slopefactor.csv");
            }
            catch (Exception )
            {

                throw;
            }
        }
     
        private void resetSurface()
        {
            surface = initialSurface.Clone();
        }
        private ModelPathEntity nearestPathEntity(ModelPath path, Vector3 point)
        {
            var distances = new List<double>();
            if (path.Count > 0)
            {
                foreach (ModelPathEntity mpe in path)
                {
                    double distance = point.Distance2To(mpe.PositionAsVector);
                    distances.Add(distance);
                }
                var distArray = distances.ToArray();
                var pathArray = path.ToArray();
                Array.Sort(distArray, pathArray);
                return pathArray[0];
            }
            else
            {
                return new ModelPathEntity();
            }

        }
        private RemovalRate newRemovalRate(ModelPath path,RunInfo runInfo, RemovalRate oldRemovalRate, DepthInfo depthInfo)
        {
            var newRemovalRate = new RemovalRate();
            ModelPathEntity mpeDepth = nearestPathEntity(path, depthInfo.LocationOfDepthMeasure);
            double currentDepth = getDepth(mpeDepth, depthInfo);
            double currentDepthPerRun = currentDepth/runInfo.CurrentRun;
            double currentTargetDepthPerRun = depthInfo.TargetDepthAtLocation/runInfo.CurrentRun;
            depthInfo.CurrentDepthAtLocation.Add(currentDepth);
            
            if(currentDepth != 0)
            {
                double newMrr = oldRemovalRate.DepthPerPass * (currentTargetDepthPerRun / currentDepthPerRun);
                return newRemovalRate;
            }
            else
            {
                return oldRemovalRate;
            }
            
        }
        private void saveFootprint(List<AbmachPoint> ptlist,int runNumber)
        {
            var entityList = new List<DwgEntity>();
            foreach (AbmachPoint v in ptlist)
            {


                DXFPoint pt = new DXFPoint(v.Position);
                if (v.JetHit)
                {
                    pt.DxfColor = DxfColor.Cyan;
                    entityList.Add(pt);
                }
                else
                {
                    pt.DxfColor = DxfColor.Grey;
                    entityList.Add(pt);
                }

            }
            string fileName = "footprintRun." + runNumber.ToString() + ".dxf";
            DxfFileBuilder.Save(entityList, fileName);
        }
        private void saveDataFile(List<string>lines ,string fileName)
        {
            string time = DateTime.Now.Hour.ToString("d2") + DateTime.Now.Minute.ToString("d2") + DateTime.Now.Second.ToString("d2") + DateTime.Now.Millisecond.ToString("d3");
            FileIO.Save(lines, fileName + "-" + time + ".txt");
        }
        private void savePath(ModelPath path,string fileName)
        {
            List<string> lines = new List<string>();
            string title = "targetDepth,depth,feedrate";
            lines.Add(title);
            string line = "";
            string time = DateTime.Now.Hour.ToString("d2") +DateTime.Now.Minute.ToString("d2")+DateTime.Now.Second.ToString("d2") +DateTime.Now.Millisecond.ToString("d3");
            foreach (ModelPathEntity mpe in path)
            {
                line = mpe.TargetDepth.ToString("f6") + "," + mpe.Depth.ToString("f6") + "," + mpe.Feedrate.Value.ToString("f4");
                lines.Add(line);
            }
            FileIO.Save(lines, fileName+"-"+time+".txt");
        }
        private ModelPath newFeedrates(ModelPath path,DepthInfo depthInfo)
        {
           
            ModelPath newPath = new ModelPath();

            foreach (ModelPathEntity mpe in path)
            {
                ModelPathEntity mpNew = mpe.Clone();
                mpe.Depth = getDepth(mpe,depthInfo);
                if(mpe.Depth!=0)
                {                    
                    mpNew.Feedrate.Value = mpe.Feedrate.Value * (mpe.Depth / mpe.TargetDepth);
                    mpNew.Depth = mpe.Depth;                
                }
                newPath.Add(mpNew);
               
            }
            savePath(path, "oldfeedrates");
            savePath(newPath, "newFeedrates");
            return newPath;
        }
        double getDepth(ModelPathEntity mpe, DepthInfo depthInfo)
        {
            switch (depthInfo.SearchType)
            {
                case DepthSearchType.FindMaxDepth:
                    return getMaxDepth(mpe, depthInfo.SearchRadius);
                    
                case DepthSearchType.FindMinDepth:
                    return getMinDepth(mpe, depthInfo.SearchRadius);
                   
                case DepthSearchType.FindAveDepth:
                default:
                    return getAveDepth(mpe, depthInfo.SearchRadius);  
            }

        }
        double getMaxDepth(ModelPathEntity mpe, double searchRadius)
        {
            List<AbmachPoint> jetIntersectList = surface.GetPointsInsideBox(BoundingBoxBuilder.GetSearchBox(mpe.PositionAsVector, searchRadius));
            double maxDepth = 0;
            foreach (AbmachPoint pt in jetIntersectList)
            {
                double depth = getPointDepth(pt);
                if (depth > maxDepth)
                    maxDepth = depth;
            }
            return maxDepth;
        }
        double getMinDepth(ModelPathEntity mpe,double searchRadius)
        {
            List<AbmachPoint> jetIntersectList = surface.GetPointsInsideBox(BoundingBoxBuilder.GetSearchBox(mpe.PositionAsVector, searchRadius));
            double minDepth = 1e16;
            foreach(AbmachPoint pt in jetIntersectList)
            {
                double depth = getPointDepth(pt);
                if (depth < minDepth)
                    minDepth = depth;
            }
            return minDepth;
        }
        double getPointDepth(AbmachPoint pt)
        {
         return  (pt.OriginalPosition - pt.Position).Length;
        }
        double getAveDepth(ModelPathEntity mpe,double searchRadius)
        {
            double depth = 0;

            List<AbmachPoint> jetIntersectList = surface.GetPointsInsideBox(BoundingBoxBuilder.GetSearchBox(mpe.PositionAsVector, searchRadius));
            int pointCount = 0;
            foreach(AbmachPoint pt in jetIntersectList)
            {
                if (pt.JetHit)
                {
                    depth += getPointDepth(pt);
                    pointCount++;
                }
            }

            depth = pointCount > 0 ? depth / pointCount : 0;           

            return depth;

        }
        double spacingFactor()
        {
            double pathLength = 0;
            for (int i = 0; i < path.Count - 1; i++)
            {
                pathLength += (path[i + 1].PositionAsVector - path[i].PositionAsVector).Length;
            }
            double avePathLen = pathLength / path.Count;

            double result = 2 * avePathLen / jetRadius;
            return result;      
        }
        double jetFactor (double radius)
        {
            return abmachParams.AbMachJet.RemovalRateAt(radius);            
        }
        double feedrateFactor(double feedrate,  RemovalRate mrr)
        {
            if (feedrate == 0)
                feedrate = 1;
            return   mrr.NominalSurfaceSpeed / feedrate;
        }
        double incidentAngle(Vector3 jetVector, Vector3 surfaceNormal)
        {
            double incidentAngle = Math.Abs(jetVector.AngleTo(surfaceNormal));
            if (incidentAngle > Math.PI / 2)
                incidentAngle = Math.PI - incidentAngle;
            return incidentAngle;
        }
        double slopeFactor(double incidentAngle)
        {
            double angle = incidentAngle - abmachParams.Material.CriticalRemovalAngle;
            double sFactor = Math.Abs(Math.Cos(angle));
            if (double.IsNaN(sFactor))
                sFactor = 1;
            return sFactor;
        }
     
    }
}
