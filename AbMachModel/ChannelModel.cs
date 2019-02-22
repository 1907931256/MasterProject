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
using System.Drawing;
namespace AbMachModel
{


    public class ChannelModel
    {
        XSecJet jet;
        XSecPathList pathList;
        RunInfo runInfo;
        AbMachParameters parameters;
        XSectionProfile profile;
        XSectionProfile targetProf;
        XSectionProfile startProf;
        XSectionProfile tempProf;

        double meshSize;
        double jetR;
        double nominalFeedrate;
        public ChannelModel(XSectionProfile targetProfile, XSecJet xSecJet, XSecPathList path, RunInfo runInfo, AbMachParameters abMachParameters)
        {
            jet = xSecJet;
            pathList = path;
            this.runInfo = runInfo;
            parameters = abMachParameters;
            targetProf = targetProfile;
        }
        Ray2[,] GetJetArray()
        {
            int jetW = (int)(Math.Ceiling(jetR * 2 / meshSize));
            Ray2[,] jetArr = new Ray2[pathList.Count, jetW];
            for (int pathIndex = 0; pathIndex < pathList.Count; pathIndex++)
            {
                double endX = pathList[pathIndex].CrossLoc + jetR;
                for (int jetLocIndex = 0; jetLocIndex < jetW; jetLocIndex++)
                {
                    double x = (pathList[pathIndex].CrossLoc - jetR) + (meshSize * jetLocIndex);
                    var origin = new Vector2(x, 0);
                    double angleDeg = 90;
                    double angRad = Math.PI * (angleDeg / 180.0);
                    var direction = new Vector2(Math.Cos(angRad), Math.Sin(angRad));
                    double jetX = x - pathList[pathIndex].CrossLoc;
                    double mrr = jet.GetMrr(jetX) * nominalFeedrate / pathList[pathIndex].Feedrate;
                    var jetRay = new Ray2(origin, direction, mrr);
                    jetArr[pathIndex, jetLocIndex] = jetRay;
                }
            }
            return jetArr;
        }
        public void Run(CancellationToken ct, IProgress<int> progress)
        {
            try
            {

                var meshSize = parameters.MeshSize;
                var gridOrigin = profile.Origin;
                var gridWidth = profile.Width;
                var jetArr = GetJetArray();
                var tempProfile = new XSectionProfile(gridOrigin, gridWidth, meshSize);
                var jetRayList = new List<Ray2>();
                var baseMrr = parameters.RemovalRate.DepthPerPass;
                var averagingWindow = parameters.SmoothingWindowWidth;
                var critAngle = parameters.Material.CriticalRemovalAngle;
                for (int iter = 0; iter < runInfo.Iterations; iter++)
                {
                    profile = new XSectionProfile(gridOrigin, gridWidth, meshSize);
                    for (int i = 0; i < runInfo.Runs; i++)
                    {



                    }
                }
            }
            catch
            {
                throw;
            }
        }
        void SaveProfileParams(double critAngle,double averagingWindow)
        {
            var paramList = new List<string>();            

            paramList.Add("X,D,TD,AE,N,Curvature");
            foreach (var pt in profile)
            {
                var n = profile.GetNormalAngle(pt.X, jet.Diameter);
                var c = profile.GetCurvature(pt.X, averagingWindow);
                var td = targetProf.GetValue(pt.X);                 
                paramList.Add(pt.X.ToString() + "," + pt.Y.ToString() + "," + td.ToString() + ","
                    + AngleEffect(n, critAngle).ToString() + "," + n.ToString() + "," + c.ToString());

            }
        }
        DepthInfo GetDepthInfo(int run, RunInfo runInfo)
        {
            var depthInfo = new DepthInfo();
            double inspectionLoc = parameters.DepthInfo.LocationOfDepthMeasure.X;

            //Console.WriteLine("Run: " + run.ToString());             
            depthInfo.StartDepth = startProf.GetValue(inspectionLoc);
            depthInfo.TargetDepth = targetProf.GetValue(inspectionLoc);
            //Console.WriteLine("StartDepth: " + parameters.DepthInfo.StartDepth.ToString());             
            depthInfo.CurrentDepth = profile.GetValue(inspectionLoc);
            //Console.WriteLine("Total Depth: " + currentDepth.ToString());
            //Console.WriteLine("TargetDepthAtRun: " + targetDepthAtRun.ToString());
            //Console.WriteLine("TargetTotalDepthAtRun: " + targetTotalDepth.ToString());
            return depthInfo;
        }
        double AdjustMrr(double baseMrr,DepthInfo depthInfo)
        {
            double  mrrAdjustFactor = Math.Abs(depthInfo.TargetDepth / depthInfo.CurrentDepth);
            double newMrr = baseMrr * mrrAdjustFactor;
            return newMrr;
        }
        void RunPath(Ray2[,] jetArr, Vector2 gridOrigin, double gridWidth, double averagingWindow,double curvatureSearchWindow, double baseMrr, double critAngle)
        {
            int run = parameters.RunInfo.CurrentRun;
            while (parameters.RunInfo.CurrentRun < parameters.RunInfo.Runs)
            {
                //index across jet locations in jet-path array 
                for (int pathIndex = 0; pathIndex < jetArr.GetLength(0); pathIndex++)
                {
                    var tempProf = new XSectionProfile(gridOrigin, gridWidth, meshSize);
                    for (int jetLocIndex = 0; jetLocIndex < jetArr.GetLength(1); jetLocIndex++)
                    {
                        var jetRay = jetArr[pathIndex, jetLocIndex];
                        double x = jetRay.Origin.X;
                        var profileNormal = profile.GetNormalAngle(x, averagingWindow);
                        var angleEffect = AngleEffect(profileNormal, critAngle);
                        var curvature =parameters.CurvatureEffect.Factor(profile.GetCurvature(x, curvatureSearchWindow));
                        double materialRemoved = baseMrr * jetRay.Length * angleEffect * curvature;
                        tempProf.SetValue(materialRemoved, x);
                    }
                    tempProf.Smooth(jetR);
                    profile.AddProfile(tempProf);
                    profile.Smooth(jetR);
                }
                if (run == parameters.RunInfo.Runs - 1)
                {
                    

                }
                run++;
            }

            double inspectionDepth = profile.GetValue(parameters.DepthInfo.LocationOfDepthMeasure.X);
            Console.WriteLine("Run: " + run.ToString() + " Depth: " + inspectionDepth.ToString());
            double targetDepthAtRun = parameters.DepthInfo.TargetDepth * (run) / runInfo.Runs;
            Console.WriteLine("targetDepthAtRun: " + targetDepthAtRun.ToString());
            double mrrAdjustFactor = targetDepthAtRun / inspectionDepth;
            Console.WriteLine("mrrAdjustFactor: " + mrrAdjustFactor.ToString());
            if (parameters.RunInfo.RunType == ModelRunType.NewMRR)
            {
                baseMrr *= mrrAdjustFactor;
                Console.WriteLine("new Mrr: " + baseMrr.ToString());
            }
            Console.WriteLine("");
            var angleEffectList = new List<string>();
            var normalsList = new List<string>();
            if (run == parameters.RunInfo.Runs - 1)
            {
                for (int profIndex = 0; profIndex < profile.Count; profIndex++)
                {
                    double x = profIndex * profile.MeshSize + profile.Origin.X;
                    var n = profile.GetNormalAngle(x, averagingWindow);
                    normalsList.Add(x.ToString() + "," + n.ToString());

                }
            }
        }
    
        double AngleEffect(double normalAngle, double critAngle)
        {
            return Math.Abs(Math.Cos(Math.Abs(normalAngle) - critAngle));
        }
        double FeedrateEffect(double feedrate, double nominalFeedrate)
        {
            return nominalFeedrate / feedrate;
        }
    }
}
