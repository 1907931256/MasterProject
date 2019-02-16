using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbMachModel;
using GeometryLib;
using ToolpathLib;
using System.Drawing;
namespace GridTest
{
    class Program
    {
        class ErrorHistory
        {
            public Error Error;
            public double PosScaling;
            public double NegScaling;
            public double CritAngleDeg;
        }
        class Error
        {
            public double Max;
            public double Min;
            public double Ave;
        }
        static void Main(string[] args)

        {
            try
            {

                ModelRunType modelRunType = ModelRunType.NewFeedrates;
                var gridOrigin = new Vector2(-.32, 0);
                double gridWidth = .64;
                double gridDepth = .5;
                double meshSize = .001;
                byte startValue = 255;
                double alongLocation = 0.0;
                double jetCenterX = gridWidth / 2.0;
                double jetDiameter = .0558;
                double jetR = jetDiameter/2.0;
                
                double nominalFeedrate = 40;
                double baseMrr = .00234211;
                double maxMrr = .0025;
                double averagingWindow = .01;
                double critAngle = Math.PI * 0.0 / 180.0;
                double angExpEffect = .5;
                double inspectionLoc = 0;
                double targetDepth = .049;
                double particleRadius = .014;
                int runCount = 11;
                
                double feedrateEffectScaling =.4;
                //laptop
                string directory =" C:/Users/nickc/OneDrive/Documents/#729 155mm/Nick test files/";
                //work desktop
                directory = "C:/Users/nickc_000/OneDrive/Documents/#729 155mm/Nick test files/";
                Console.WriteLine("building grid");


                var profile = new XSectionProfile(gridOrigin, gridWidth, meshSize);
                
                var tempProfile = new XSectionProfile(gridOrigin, gridWidth, meshSize);
                var jetRayList = new List<Ray2>();
                //string targetProfileFilename = "190201_03 _TOP_33_60deg_out.csv";
                string targetProfileFilename = "190201-04 - TOP-33-BOT-10-COR-04 - 60deg_out.csv";
               // string targetDepthProfile = "190207-11 - 0.040Dm-60deg-Xpos-16pass_out-target.csv";
                var targetProfile  = new XSectionProfile(targetProfileFilename, 7,1);
                var startProfile = new XSectionProfile("top_channel_model_profile.csv", 0, 0);
                double startDepth = startProfile.GetValue(inspectionLoc);
                Console.WriteLine("startDepth: " + startDepth.ToString());

                string jetscanFilename = "dn005-dm040-60deg.csv";
                //jetscanFilename = "parabolaJet.csv";
                
                var jet = new XSecJet( jetscanFilename,jetDiameter);
                
                //string path  ="155_FLAT_TOP_33.csv";
                //string  pathCsvFilename = "singlepath-profile-toolpath.csv";
                //pathCsvFilename =  "BOT-profile-toolpath-depthmeasure-test-nick.csv";
                string   path = "bottom25.csv";
                //pathCsvFilename = "top35.csv";
                //build path from path entities 
                //path is list of path entities 
                //jet ray list is list of jet rays 
                //pathExOrder,xLocation(across channel),yLocation(along channel),feed, direction,Depth,TargetDepth"
                var pathList = new XSecPathList(path,1,0);
                int pathCount = pathList.Count;               
                int jetW = (int)(Math.Ceiling( jet.Diameter / meshSize));

                //double[,] feedArr = new double[pathCount, jetW];
                //build jet rays from path list


                Ray2[,] jetArr = BuildJetArray(pathList, jet,meshSize,nominalFeedrate);

                Console.WriteLine("running model");           
                var angleEffectList = new List<string>();
                var normalsList = new List<string>();
                var errorHistoryStr = new List<string>();
                var errorHistoryList = new List<ErrorHistory>();
                Error profileError = new Error();
                double maxError = .0005;
                int innerIterator = 0;
                int innerIterations = 4;
                int outerIterations = 1;
                double critAngleMax = 85;
                double critAngleMin = 0;
                double critAngleChange = 1;
                double critAngDeg = 19;
                string timeCode=  DateTime.Now.ToFileTimeUtc().ToString();
                double startPosScaling = .008;
                double startNegScaling = .001;
                double posScalingFactor=startPosScaling;
                double negScalingFactor= startNegScaling;
                int outerIterator = 0;
                errorHistoryStr.Add("CRITANG,NEGSCALING,POSSCALING,ERROR");
                double mrrAdjustFactor = 1;
                while (outerIterator < outerIterations)//pos scaling iteration
                {
                    negScalingFactor = startNegScaling;
                    innerIterator = 0;
                    while (innerIterator < innerIterations)//neg scaling iteration
                    {
                        Console.WriteLine("Iteration: " + (outerIterator+innerIterator).ToString());
                        critAngle = Math.PI * critAngDeg / 180.0;
                        profile = new XSectionProfile(startProfile);
                        int run = 0;
                        double inspectionDepth = 0;
                        while (run < runCount )
                        {
                            //index across jet locations in jet-path array 
                            for (int pathIndex = 0; pathIndex < jetArr.GetLength(0); pathIndex++)
                            {
                                tempProfile = new XSectionProfile(gridOrigin, gridWidth, meshSize);
                                for (int jetLocIndex = 0; jetLocIndex < jetArr.GetLength(1); jetLocIndex++)
                                {
                                    var jetRay = jetArr[pathIndex, jetLocIndex];
                                    double x = jetRay.Origin.X;
                                    var profileNormal = profile.GetNormalAngle(x, averagingWindow);
                                    var angleEffect = AngleEffect(profileNormal, critAngle);
                                    var curvature = CurvatureEffect(profile.GetCurvature(x, particleRadius), posScalingFactor, negScalingFactor);
                                    double materialRemoved = baseMrr * jetRay.Length * angleEffect * curvature;
                                    tempProfile.SetValue(materialRemoved, x);                                    
                                }
                                tempProfile.Smooth(jetR);
                                profile.AddProfile(tempProfile);
                                profile.Smooth(jetR);
                            }
                            if (run == runCount - 1)
                            {
                                angleEffectList.Clear();

                                angleEffectList.Add("X,D,TD,AE,N,feed,curvature");
                                foreach (var pt in profile)
                                {
                                    var n = profile.GetNormalAngle(pt.X, jetDiameter);
                                    var c = profile.GetCurvature(pt.X, averagingWindow);
                                    var td = targetProfile.GetValue(pt.X);
                                   // var f = feedProfile.GetValue(pt.X);
                                    angleEffectList.Add(pt.X.ToString() + "," + pt.Y.ToString() + "," + td.ToString() + ","
                                        + AngleEffect(n, critAngle).ToString() + "," + n.ToString() + "," + c.ToString());

                                }

                            }
                            Console.WriteLine("Run: " + run.ToString());
                            startDepth = startProfile.GetValue(inspectionLoc);
                            Console.WriteLine("StartDepth: " + startDepth.ToString());
                            inspectionDepth = profile.GetValue(inspectionLoc);
                            Console.WriteLine("Total Depth: " + inspectionDepth.ToString());
                            double targetDepthAtRun = (targetDepth - startDepth) * (run + 1) / runCount;
                            double depthAtRun = inspectionDepth - startDepth;
                            double targetTotalDepth = startDepth + targetDepthAtRun;
                            Console.WriteLine("TargetDepthAtRun: " + targetDepthAtRun.ToString());
                            Console.WriteLine("TargetTotalDepthAtRun: " + targetTotalDepth.ToString());                           
                            Console.WriteLine("mrrAdjustFactor: " + mrrAdjustFactor.ToString());                            
                            Console.WriteLine("");
                            run++;
                        }

                        profileError = CalcError(profile, targetProfile, runCount, runCount);

                        Console.WriteLine("CritAngle: " + critAngDeg + " baseMrr: " + baseMrr + " Error: " + profileError.ToString());
                        string paramString = critAngDeg.ToString() + "," + baseMrr.ToString() + "," +  profileError.ToString();
                        errorHistoryStr.Add(paramString);
                        var e = new ErrorHistory();
                        e.CritAngleDeg = critAngDeg;
                        e.NegScaling = negScalingFactor;
                        e.PosScaling = posScalingFactor;
                        e.Error = profileError;
                        errorHistoryList.Add(e);
                        Console.WriteLine("");
                        Console.WriteLine("saving model");
                        timeCode = System.DateTime.Now.ToFileTimeUtc().ToString();
                        angleEffectList.Add(paramString);
                        FileIOLib.FileIO.Save(angleEffectList, directory + "ProfileEffectList" + timeCode + ".csv");
                        string bitmapFilename = directory + "testgrid" + timeCode + ".bmp";
                        profile.SaveBitmap(bitmapFilename);
                        //critAngDeg += critAngleChange;
                        if (modelRunType == ModelRunType.NewMRR)
                        {
                            mrrAdjustFactor = Math.Abs(targetDepth / inspectionDepth);
                            baseMrr *= mrrAdjustFactor;
                            Console.WriteLine("new Mrr: " + baseMrr.ToString());
                        }
                        if(modelRunType==ModelRunType.AdjustCritAngle)
                        {
                            negScalingFactor += .001;
                        }
                        if(modelRunType == ModelRunType.NewFeedrates)
                        {
                            double MeasureWidth = .002;
                            DataLib.CartData depthData = new DataLib.CartData();
                            foreach(var pt in profile)
                            {
                                depthData.Add(new Vector3(pt.X,pt.Y,0));
                            }
                            DataLib.CartData targetData = new DataLib.CartData();
                            foreach(var pt in targetProfile)
                            {
                                targetData.Add(new Vector3(pt.X, pt.Y, 0));
                            }
                            pathList.AdjustFeedrates(depthData, targetData, MeasureWidth);
                            
                            jetArr = BuildJetArray(pathList, jet, meshSize, nominalFeedrate);
                            FileIOLib.FileIO.Save(pathList.AsCSVFile(), directory + "pathlist" + timeCode + ".csv");
                        }
                        innerIterator++;
                    }
                    if (modelRunType == ModelRunType.AdjustCritAngle)
                    {
                        posScalingFactor += .001;
                    }
                    outerIterator++;
                }
                FileIOLib.FileIO.Save(errorHistoryStr, directory + "Errorhistory" + timeCode + ".csv");
                Console.WriteLine("program complete");
                Console.ReadKey();

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message + ":" + ex.StackTrace);
                Console.ReadKey();
            }
        }
        static Ray2[,] BuildJetArray(XSecPathList pathList,XSecJet jet,double meshSize,double nominalFeedrate)
        {
            int jetW = (int)(Math.Ceiling(jet.Diameter / meshSize));
            double jetR = jet.Diameter / 2;
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
                    double jetMrr = jet.GetMrr(jetX);
                    double feedEffect = FeedrateEffect( pathList[pathIndex].Feedrate, nominalFeedrate);
                    double mrr = jetMrr * feedEffect;
                    var jetRay = new Ray2(origin, direction, mrr);
                    jetArr[pathIndex, jetLocIndex] = jetRay;
                   
                }
            }
            return jetArr;
        }
        static void AdjustValues(List<ErrorHistory> errorHistoryList)
        {
            int c = errorHistoryList.Count;
            var e1 = errorHistoryList[c - 1];
            var e0 = errorHistoryList[c - 2];
            double dErr = e1.Error.Ave - e0.Error.Ave;
            double dneg = e1.NegScaling - e0.NegScaling;
            double dpos = e1.PosScaling - e0.PosScaling;
            if(dErr>0)
            {

            }
        }
        static Error CalcError(XSectionProfile profile, XSectionProfile targetProfile,int run,int runTotal)
        {
            Error error = new Error();
            int i = 0;
            double min = double.MaxValue;
            double max = double.MinValue;
            double ave = 0;
            foreach(var pt in profile)
            {
                var modDepth = pt.Y;
                var targetDepth = targetProfile.GetValue(pt.X) * (run + 1) / runTotal;               
                double e = Math.Abs(Math.Abs(modDepth) - Math.Abs(targetDepth));
                min = Math.Min(min, e);
                max = Math.Max(max, e);
                ave += e;
                i++;
            }
            if(i!=0)
            {
                ave /= i;
            }
            error.Max = max;
            error.Min = min;
            error.Ave = ave;
            return error;
        }
        static double CurvatureEffect(double curvature,double negScalingFactor, double posScalingFactor)
        {
            double ce = 1;
            if (curvature < 0)
            {
                ce += negScalingFactor * curvature;
            }
            else
            {
                ce += posScalingFactor * curvature;
            }
            return ce;
        }
        static double AngleEffect(double normalAngle,double critAngle)
        {
            return Math.Abs(Math.Cos(Math.Abs(normalAngle)-critAngle)) ;            
        }
        static double FeedrateEffect( double feedrate, double nominalFeedrate)
        {
            return nominalFeedrate / feedrate;
        }
    }
        
}
