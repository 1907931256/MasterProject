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
        static void Main(string[] args)

        {
            try
            {

                ModelRunType modelRunType = ModelRunType.NewMRR;
                var gridOrigin = new Vector2(-.1, 0);
                double width = .2;
                double depth = .25;
                double meshSize = .0005;
                byte startValue = 255;
                double alongLocation = 0.0;
                double jetCenterX = width / 2.0;
                double jetR = .041;
                int eqIndex = 1;
                double nominalFeedrate = 40;
                double baseMrr = .00709;
                double averagingWindow = .0005;
                double critAngle = Math.PI * 75.0 / 180.0;
                double angExpEffect = .5;
                double inspectionLoc = 0;
                double targetDepth = .05;
                int runCount = 10;
                int iterCount = 1;

                Console.WriteLine("building grid");
                
                var grid = new XSectionGrid(gridOrigin,width, depth, meshSize, startValue, alongLocation);
                var profile = new XSectionProfile(gridOrigin, width, meshSize);
                var tempProfile = new XSectionProfile(gridOrigin, width, meshSize);
                var jetRayList = new List<Ray2>();



                var jet = new XSecJet(eqIndex, jetR * 2, meshSize);
                string pathCsvFilename = "singlepath-profile-toolpath.csv";

                //build path from path entities 
                //path is list of path entities 
                //jet ray list is list of jet rays 
                var pathList = new XSecPathList(pathCsvFilename, 1);
                int pathCount = pathList.Count;               
                int jetW = (int)(Math.Ceiling(jetR * 2 / meshSize));
                Ray2[,] jetArr = new Ray2[pathCount, jetW];

                //build jet rays from path list
                for (int pathIndex = 0; pathIndex < pathCount; pathIndex++)
                {               
                    double endX = pathList[pathIndex].CrossLoc + jetR;                
                    for(int jetLocIndex =0;jetLocIndex<jetW;jetLocIndex++)
                    {
                        double x = (pathList[pathIndex].CrossLoc - jetR) + (meshSize * jetLocIndex);
                        var origin = new Vector2(x, 0);
                        double angleDeg = 90;
                        double angRad = Math.PI * (angleDeg / 180.0);
                        var direction = new Vector2(Math.Cos(angRad), Math.Sin(angRad));
                        double mrr =  jet.GetMrr(x - pathList[pathIndex].CrossLoc)* nominalFeedrate/ pathList[pathIndex].Feedrate;                        
                        var jetRay = new Ray2(origin, direction, mrr);                       
                        jetArr[pathIndex, jetLocIndex] = jetRay; 
                    }
                }

                var intersectList = new List<GridIntersect>();
                Console.WriteLine("running model");           
                var angleEffectList = new List<string>();
                var normalsList = new List<string>();
                double[] inspectionDepthArr = new double[runCount];
                for (int iter = 0; iter < iterCount; iter++)
                {
                    for (int i = 0; i < runCount; i++)
                    {
                        intersectList = new List<GridIntersect>();
                        for (int pathIndex = 0; pathIndex < jetArr.GetLength(0); pathIndex++)
                        {
                            for (int jetLocIndex = 0; jetLocIndex < jetArr.GetLength(1); jetLocIndex++)
                            {
                                var jetRay = jetArr[pathIndex, jetLocIndex];
                                var profileNormal = profile.GetNormalAngle(jetRay.Origin.X);
                                var angleEffect = AngleEffect(jetRay, profileNormal, critAngle, angExpEffect);
                                if (i == runCount - 1)
                                {
                                    angleEffectList.Add(jetRay.Origin.X.ToString() + "," + jetLocIndex.ToString() + "," + angleEffect.ToString());
                                }
                                double mrr = baseMrr * jetRay.Length * angleEffect;
                                double currentDepth = profile.GetValue(jetRay.Origin.X);
                                double newDepth = currentDepth + mrr;
                                tempProfile.SetValue(newDepth, jetRay.Origin.X);
                            }
                            tempProfile.Smooth(averagingWindow);
                            profile = new XSectionProfile(tempProfile);
                        }
                        double inspectionDepth = profile.GetValue(inspectionLoc);
                        Console.WriteLine("Run: " + i.ToString() + " Depth: " + inspectionDepth.ToString());
                        double targetDepthAtRun = targetDepth * (i + 1) / runCount;
                        Console.WriteLine("targetDepthAtRun: " + targetDepthAtRun.ToString());
                        double mrrAdjustFactor = targetDepthAtRun / inspectionDepth;
                        Console.WriteLine("mrrAdjustFactor: " + mrrAdjustFactor.ToString());
                        if (modelRunType == ModelRunType.NewMRR)
                        {
                            baseMrr *= mrrAdjustFactor;
                            Console.WriteLine("new Mrr: " + baseMrr.ToString());
                        }
                        Console.WriteLine("");

                        if (i == runCount - 1)
                        {
                            for (int profIndex = 0; profIndex < profile.XLength; profIndex++)
                            {
                                double x = profIndex * profile.MeshSize + profile.Origin.X;
                                var n = profile.GetNormalAngle(x);
                                normalsList.Add(x.ToString()+","+n.ToString());

                            }
                        }

                    }
                }

                Console.WriteLine("saving model");
                string timeCode = System.DateTime.Now.ToFileTimeUtc().ToString();
                FileIOLib.FileIO.Save(normalsList, "normalList"+timeCode +".csv");
                FileIOLib.FileIO.Save(angleEffectList, "angleEffectList"+timeCode +".csv");
                profile.SaveBitmap("testgrid"+timeCode +".bmp");
                string csvFilename = "testprofile-" + timeCode+".csv";
                profile.SaveCSV(csvFilename);
                Console.WriteLine("program complete");
                Console.ReadKey();
               
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message + ":" + ex.StackTrace);
                Console.ReadKey();
            }
        }
       
        
        static double AngleEffect(Ray2 jet, double normalAngle,double critAngle,double expF)
        {
            
            double a =Math.Abs(Math.Abs(normalAngle) - critAngle);
            double angleEffect = 1;

            //double angleEffect = Math.Abs(Math.Pow(Math.Cos(angleCenter),2));
            //angleEffect = Math.Exp(-1.0 * expF* Math.Pow(a, 2.0));
            angleEffect = .32 - .0729 * a + 1.826 * Math.Pow(a, 2.0)-4.9745 * Math.Pow(a, 3.0) + 
                5.8245 * Math.Pow(a, 4.0) -2.2085 * Math.Pow(a, 5.0);
            if (angleEffect < 0)
            {
                angleEffect = 0;
            }
               
            return angleEffect;
        }
    }
        
}
