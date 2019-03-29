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
using DataLib;
namespace AbMachModel
{


    public class ChannelModel
    {
        XSecJet jet;
        XSecPathList path;
        RunInfo runInfo;
        XSecModelParams parameters;
        XSection profile;
        XSection targetProf;
        XSection startProf;
        XSection tempProf;

        
        //public ChannelModel(XSectionProfile targetProfile, XSecJet xSecJet, XSecPathList path,
        //     XSecModelParams parameters)
        //{
        //    jet = xSecJet;
        //    this.path = path;           
        //    this.parameters = parameters;
        //    targetProf = targetProfile;
        //}
        public ChannelModel(XSection targetProfile, XSection startProfile, XSecJet xSecJet, XSecPathList path,
              XSecModelParams parameters)
        {
            jet = xSecJet;
            this.path = path;
            this.parameters = parameters;
            targetProf = targetProfile;
            startProf = startProfile;
        }
        
        int totalModelRuns;
        int currentModelRun;
        public void Run(CancellationToken ct, IProgress<int> progress,int innerIterations, int outerIterations)
        {
            try
            {

                var meshSize = parameters.MeshSize;
                var gridOrigin = profile.Origin;
                var gridWidth = profile.Width;
                var jetArr = new XSecJetPath(jet, path, parameters.MeshSize, parameters.RemovalRate.NominalSurfaceSpeed);
                var baseMrr = parameters.RemovalRate.DepthPerPass;
                var mrr = baseMrr;
                var averagingWindow = parameters.SmoothingWindowWidth;
                var critAngle = parameters.Material.CriticalRemovalAngle;
                int outerIterator = 0;
                int innerIterator = 0;

                totalModelRuns = outerIterations * innerIterations * parameters.RunTotal;
                currentModelRun = 0;
                var depthInfo = new DepthInfo(parameters.DepthInfo);
                
                while (outerIterator < outerIterations)
                {

                   
                    mrr = baseMrr;
                    innerIterator = 0;
                    while (innerIterator < innerIterations)
                    {               
                        RunPath(ct,progress,jetArr, gridOrigin, gridWidth, averagingWindow, mrr, critAngle);
                        depthInfo = GetNewDepthInfo(depthInfo);
                        mrr= GetNewMrr(mrr, depthInfo);
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
        DepthInfo GetNewDepthInfo( DepthInfo oldDepthInfo)
        {
            var depthInfo = new DepthInfo();

            double inspectionLoc = oldDepthInfo.LocationOfDepthMeasure.X;
            depthInfo.StartDepth = startProf.GetValue(inspectionLoc);
            depthInfo.TargetDepth = targetProf.GetValue(inspectionLoc);
            depthInfo.CurrentDepth = profile.GetValue(inspectionLoc);
            return depthInfo;
        }
        double GetNewMrr(double baseMrr,DepthInfo depthInfo)
        {
            depthInfo.CurrentDepth = GetInspectionDepth();
            double  mrrAdjustFactor = Math.Abs(depthInfo.TargetDepth / depthInfo.CurrentDepth);
            double newMrr = baseMrr * mrrAdjustFactor;
            return newMrr;
        }
        XSecJetPath GetNewJetArrayFeedrates(double measureWidth, string directory,string timeCode)
        {
            path.AdjustFeedrates(profile.AsCartData(),startProf.AsCartData(), targetProf.AsCartData(), measureWidth);
            string filename = directory + "pathlist" + timeCode + ".csv";
            FileIOLib.FileIO.Save(path.AsCSVFile( filename, ""),filename);
            var jetArray = new XSecJetPath(jet, path, parameters.MeshSize, parameters.RemovalRate.NominalSurfaceSpeed);
            return jetArray;
        }
        double GetInspectionDepth()
        {
           return  profile.GetValue(parameters.DepthInfo.LocationOfDepthMeasure.X);
        }
        void RunPath(CancellationToken ct, IProgress<int> progress, XSecJetPath jetArr, Vector2 gridOrigin, double gridWidth,
            double averagingWindow,double mrr, double critAngle)
        {
            try
            {
                double curvatureSearchWindow = .014;
                int run = 0;
                profile = new XSection(startProf);
                while (run < parameters.RunTotal && !ct.IsCancellationRequested)
                {
                    //index acroos jet path locations in jet path array
                    for (int pathIndex = 0; pathIndex < jetArr.PathCount; pathIndex++)
                    {
                        //index across jet locations in jet-path array 
                        var tempProf = new XSection(gridOrigin, gridWidth, parameters.MeshSize);
                        for (int jetLocIndex = 0; jetLocIndex < jetArr.JetCount; jetLocIndex++)
                        {
                                var jetRay = jetArr.GetJetRay(pathIndex, jetLocIndex);
                                double x = jetRay.Origin.X;
                                var profileNormal = profile.GetNormalAngle(x, averagingWindow);
                                var angleEffect = AngleEffect(profileNormal, critAngle);
                                var curvatureEffect = parameters.CurvatureEffect.Factor(profile.GetCurvature(x, curvatureSearchWindow));
                                double materialRemoved = mrr * jetRay.Length * angleEffect * curvatureEffect;
                                tempProf.SetValue(materialRemoved, x);
                                
                        }                        
                        tempProf.Smooth(jet.Radius);
                        profile.AddProfile(tempProf);
                        profile.Smooth(jet.Radius);                       
                    }
                    int p = (int)(100 * currentModelRun++ / totalModelRuns);
                    progress.Report(p);
                    run++;
                }
            }
            catch (Exception)
            {

                throw;
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
