using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ToolpathLib;
using GeometryLib;
using SurfaceModel;
using CNCLib;

namespace AbMachModel
{
    /// <summary>
    /// modeling engine
    /// </summary>
    public class AbmachSimModel2D
    {
        Abmach2DSurface _surface;
        Abmach2DSurface _initialSurf;
       
        Abmach2DSurface _tempSurf;
        ModelPath _path;
        AbMachParameters abmachParams;
        RunInfo runInfo;
        RemovalRate _currentRemovalRate;
        List<string> debugData;   
        int jetR;
        double jetRadius;
        double meshSize;
        public Abmach2DSurface GetSurface()
        {
            return _surface;
        }
        public ModelPath GetPath()
        {
            return _path;
        }
        public RemovalRate GetRemovalRate()
        {
            return _currentRemovalRate;
        }
        public AbmachSimModel2D(Abmach2DSurface surface, ModelPath path, AbMachParameters parms)
        {
            debugData = new List<string>();
            _surface = surface;
            _initialSurf = surface.Clone();
            _tempSurf = new Abmach2DSurface(surface.BoundingBox, surface.MeshSize, surface.Border);
            _path = path;
            abmachParams = parms;
            runInfo = parms.RunInfo;
            jetRadius = abmachParams.AbMachJet.Diameter / 2.0;
            jetR = (int)Math.Round(.5*abmachParams.AbMachJet.Diameter/ surface.MeshSize);
            meshSize = surface.MeshSize;
        }
        
        bool checkParms()
        {
            try{
                if (abmachParams == null)
                    throw new Exception("Parameters are null");
                if(abmachParams.RemovalRate.DepthPerPass <= 0)
                    throw new Exception("Removal Rate must be > zero");
                if(abmachParams.AbMachJet.Diameter<=0)
                    throw new Exception("Jet Diameter must be > zero");
                if (abmachParams.RunInfo.Runs <= 0)
                    throw new Exception("Runs must be > zero");
                if (abmachParams.RunInfo.Iterations <= 0)
                    throw new Exception("Iterations must be > zero");
                return true;
            }
            catch(Exception)
            {
                throw;
            }

        }
        bool checkSurface()
        {
            try
            {
                if (_surface == null)
                    throw new Exception("surface  is null");
              
                if (_surface.MeshSize == 0)
                    throw new Exception("mesh size must be > 0");
                return true;
            }
            catch (Exception )
            {
                throw ;
            }
        }
        bool checkPath()
        {
            try
            {
                if (_path == null)
                    throw new Exception("path is null");
                if (_path.Count <= 0)
                    throw new Exception("Pathlength must be > 0");
                return true;
            }
            catch (Exception)
            {                
                throw;
            }
        }  
       

        public void Run(CancellationToken ct,IProgress<int> progress)
        {
            try
            {
                if(checkParms() && checkPath() && checkSurface())
                {
                    double searchRadius = jetRadius + _surface.MeshSize;
                    _currentRemovalRate = abmachParams.RemovalRate;
                    int prevXIndex = _surface.Xindex(_path[0].Position.X);
                    int prevYIndex = _surface.Yindex(_path[0].Position.Y);

                    int currentRunTotal = 0;
                    int pathLength = _path.Count;
                    int progressTotal = runInfo.Iterations * runInfo.Runs;
                    double spFactor = spacingFactor();
                    abmachParams.DepthInfo.CurrentDepthAtLocation.Clear();
                    bool prevJetonState = false;
                    for (int iteration = 1; iteration <= runInfo.Iterations; iteration++)
                    {
                        runInfo.CurrentIteration = iteration;
                        if (iteration > 1)
                        {
                            resetSurface();
                        }
                        for (int run = 1; run <= runInfo.Runs; run++)
                        {
                            runInfo.CurrentRun = run;

                            foreach (ModelPathEntity mpe in _path)
                            {
                                if(!ct.IsCancellationRequested)
                                {
                                    if (mpe.JetOn && mpe.Feedrate.Value != 0)
                                    {
                                        int xIndex = _surface.Xindex(mpe.Position.X);
                                        int yIndex = _surface.Yindex(mpe.Position.Y);
                                        double deltaIndex = Math.Sqrt(Math.Pow(xIndex - prevXIndex, 2) + Math.Pow(yIndex - prevYIndex, 2));
                                        prevXIndex = xIndex;
                                        prevYIndex = yIndex;
                                        if (deltaIndex != 0 && prevJetonState)
                                        {
                                            int searchR = jetR + 1;
                                            int startXIndex = Math.Max(0, xIndex - searchR);
                                            int endXIndex = Math.Min(_surface.XSize - 1, xIndex + searchR);
                                            int startYIndex = Math.Max(0, yIndex - searchR);
                                            int endYIndex = Math.Min(_surface.YSize - 1, yIndex + searchR);
                                            double feedFactor = feedrateFactor(mpe.Feedrate.Value, deltaIndex, _currentRemovalRate);
                                            double removalConst = _currentRemovalRate.DepthPerPass * feedFactor * spFactor;
                                            subtractFootprint(mpe.JetVector, xIndex, yIndex, startXIndex, endXIndex, startYIndex, endYIndex, removalConst);
                                            //smoothFootprint(xIndex, yIndex,startXIndex, endXIndex, startYIndex, endYIndex);
                                            moveTempToModel(xIndex, yIndex, startXIndex, endXIndex, startYIndex, endYIndex);
                                        }

                                    }

                                }
                                prevJetonState = mpe.JetOn;

                            }

                            abmachParams.DepthInfo.CurrentDepthAtLocation.Add(getDepth(abmachParams.DepthInfo));
                            //adjust mrr after each run
                            if (abmachParams.RunInfo.RunType == ModelRunType.NewMRR)
                            {
                                double targetDepth = 0;
                                if(abmachParams.DepthInfo.ConstTargetDepth)
                                {
                                    targetDepth = abmachParams.DepthInfo.TargetDepth;
                                }
                                else
                                {
                                    targetDepth = abmachParams.DepthInfo.TargetDepthAtLocation;
                                }
                                abmachParams.RemovalRate = adjustMRR(abmachParams.DepthInfo.CurrentDepthAtLocation.Last(),
                                    _currentRemovalRate, runInfo,targetDepth);
                                _currentRemovalRate = abmachParams.RemovalRate;
                            }
                            currentRunTotal++;
                            progress.Report((int)100 * currentRunTotal / progressTotal);
                        }
                        //adjust feedrates after all runs are done
                        if (abmachParams.RunInfo.RunType == ModelRunType.NewFeedrates)
                        {
                            adjustFeedRates(abmachParams.DepthInfo.SearchRadius);
                        }
                    }
                    FileIOLib.FileIO.Save(debugData, "debugValues.csv");
                }
                
            }
            catch (Exception)
            {
                throw;
            }
           
        }
       
        private void resetSurface()
        {
            _surface =_initialSurf.Clone();
        }

        bool debugWrite;
        private void subtractFootprint(Vector3 jetVector, int xIndex, int yIndex,int startXIndex,int endXIndex,int startYIndex,
            int endYIndex,double removalConst)
        {
            try
            {   
                for (int surfX = startXIndex; surfX <= endXIndex; surfX++)
                {
                    int jetXIndex = xIndex - surfX;
                   
                    for (int surfY = startYIndex; surfY <= endYIndex; surfY++)
                    {                                                         
                        int jetYIndex = yIndex - surfY ;
                        double remRate = abmachParams.AbMachJet.RemovalRateAt(jetXIndex,jetYIndex);   
                        
                        if(remRate > 0)
                        {                            
                            Vector3 normal = _surface.GetNormal(surfX,surfY);
                            double slopeF =   slopeFactor(jetVector, normal);
                            double materialRemoved = removalConst * slopeF * remRate;                           
                            _tempSurf.SetDepth( surfX,surfY, materialRemoved);
                           
                        }
                        else
                        {
                            var pt = new Abmach2DPoint();
                            pt.JetHit = false;
                            _tempSurf.SetDepth( surfX,surfY,0);
                        }
                            
                    }
                }
                
            }
            catch (Exception)
            {
                throw;
            }            
        }
        void writeDebugData(List<double> data)
        {
            string line = "";
            foreach(double value in data)
            {
                line += value.ToString("f5") + ",";
            }
            debugData.Add(line);
        }
        double spacingFactor()
        {
            try
            {
                double pathLength = 0;
                for (int i = 0; i < _path.Count - 1; i++)
                {
                    pathLength += (_path[i + 1].PositionAsVector - _path[i].PositionAsVector).Length;
                }
                double avePathLen = pathLength / _path.Count;

                double result = 2 * avePathLen / jetRadius;
                return result;
            }
            catch (Exception)
            {

                throw;
            }
            
        }
       
        private void smoothFootprint(int xIndex, int yIndex, int startXIndex, int endXIndex, int startYIndex, int endYIndex)
        {
            try
            {
                
                for (int surfX = startXIndex; surfX <= endXIndex; surfX++)
                {
                    double x = _surface.XCoordinate(surfX);
                    for (int surfY = startYIndex; surfY <= endYIndex; surfY++)
                    {
                        double y = _surface.YCoordinate(surfY);                        
                        if (_tempSurf.IsPeak(surfX,surfY) || _tempSurf.IsValley(surfX,surfY))
                        {
                            _tempSurf.SmoothAt(surfX,surfY);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        private void moveTempToModel(int xIndex, int yIndex, int startXIndex, int endXIndex, int startYIndex, int endYIndex)
        {
            try
            {                
                for (int surfX = startXIndex; surfX <= endXIndex; surfX++)
                {                    
                    for (int surfY = startYIndex; surfY <= endYIndex; surfY++)
                    {                      
                        var depth =_surface.GetDepth(surfX,surfY)-_tempSurf.GetDepth(surfX,surfY);
                       _surface.SetDepth(surfX,surfY,depth);
                        
                    }
                }
            }
            catch (Exception)
            {

                throw;
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
        double jetFactor(double radius)
        {
            return 1;
        }
        double feedrateFactor(double feedrate, RemovalRate mrr)
        {
            if (feedrate == 0)
                feedrate = 1;
            return  mrr.NominalSurfaceSpeed / feedrate;
        }
        double feedrateFactor(double feedrate, double deltaIndex, RemovalRate mrr)
        {
            return deltaIndex *  mrr.NominalSurfaceSpeed / feedrate;
        }

        double slopeFactor(Vector3 jetVector, Vector3 surfaceNormal)
        {
            try
            {
                double incidentAngle = Math.Abs(jetVector.AngleTo(surfaceNormal));
                if (incidentAngle > Math.PI / 2)
                    incidentAngle = Math.PI - incidentAngle;
                double angle = incidentAngle - abmachParams.Material.CriticalRemovalAngle;
                double sFactor = Math.Abs(Math.Cos(angle));
                if (double.IsNaN(sFactor))
                    sFactor = 1;
                return sFactor;
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        void adjustFeedRates(double searchRadius)
        {
            try
            {
                int currentPathNumber = _path[0].PathNumber;
                double segmentDepth = 0;
                double segmentTargetDepth = 0;
                int segmentCount = 0;
                foreach (ModelPathEntity ent in _path)
                {
                    if (ent.JetOn)
                    {
                        while (currentPathNumber == ent.PathNumber)
                        {
                            segmentDepth += averageDepth(ent.Position.X, ent.Position.Y, searchRadius);
                            int i = _surface.Xindex(ent.PositionAsVector.X);
                            int j = _surface.Yindex(ent.PositionAsVector.Y);
                            segmentTargetDepth += _surface.GetTargetDepth(i,j);
                            segmentCount ++;
                        }

                        if (segmentCount > 0)
                        {
                            segmentTargetDepth /= segmentCount;
                            segmentDepth /= segmentCount;
                        }
                        ent.Depth = segmentDepth;
                        ent.TargetDepth = segmentTargetDepth;

                        currentPathNumber = ent.PathNumber;
                        segmentCount = 0;
                        segmentDepth = 0;
                        segmentTargetDepth = 0;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        double averageDepth(double xCenter, double yCenter, double searchRadius)
        {
            try
            {
                double result = 0;
                double xStart = xCenter - searchRadius;
                double yStart = yCenter - searchRadius;
                double xEnd = xCenter + searchRadius;
                double yEnd = yCenter + searchRadius;
                double x = xStart;
                double y = yStart;
                
                int count = 0;
                while (x <= xEnd)
                {
                    y = yStart;
                    int i = _surface.Xindex(x);
                    while (y <= yEnd)
                    {
                        int j = _surface.Yindex(y);
                        result += _surface.GetDepth(i,j);
                        y += _surface.MeshSize;
                        count++;
                    }
                    x += _surface.MeshSize;
                }
                if (count > 0)
                {
                    result /= count;
                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        RemovalRate adjustMRR(double currentDepth, RemovalRate currentMrr,RunInfo runInfo,double targetDepth)
        {
            //TODO calc adjust mrr
            double targetMrr =Math.Abs( targetDepth / runInfo.Runs);
            double actualMrr = Math.Abs(currentDepth / runInfo.CurrentRun);
            var newMrr = new RemovalRate(currentMrr.NominalSurfaceSpeed, currentMrr.DepthPerPass);
            if (actualMrr!= 0)
            {
                newMrr.DepthPerPass = currentMrr.DepthPerPass * targetMrr / actualMrr;
            }
            return newMrr;
        }

        double getDepth(DepthInfo depthInfo)
        {
            try
            {
                double depth = 0;
                if (depthInfo.SearchType == DepthSearchType.FindAveDepth)
                {
                    depth = averageDepth(depthInfo.LocationOfDepthMeasure.X, depthInfo.LocationOfDepthMeasure.Y, depthInfo.SearchRadius);
                }
                return depth;
            }
            catch (Exception)
            {

                throw;
            }
           
           
        }
    }
}
