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
                var gridOrigin = new Vector2(-.32, 0);
                double gridWidth = .64;
                double gridDepth = .5;
                double meshSize = .0001;
                byte startValue = 255;
                double alongLocation = 0.0;
                double jetCenterX = gridWidth / 2.0;
                double jetDiameter = .042;
                double jetR = jetDiameter/2.0;
                
                double nominalFeedrate = 40;
                double baseMrr = .0048;
                double averagingWindow = .01;
                double critAngle = Math.PI * 15.0 / 180.0;
                double angExpEffect = .5;
                double inspectionLoc = 0;
                double targetDepth = .038;
                int runCount = 16;
                int iterCount = 1;
                //laptop
                string directory =" C:/Users/nickc/OneDrive/Documents/#729 155mm/Nick test files/";
                //work desktop
                //string directory = "C:/Users/nickc_000/OneDrive/Documents/#729 155mm/Nick test files/";
                Console.WriteLine("building grid");
                
                var grid = new XSectionGrid(gridOrigin, gridWidth, gridDepth, meshSize, startValue, alongLocation);
                var profile = new XSectionProfile(gridOrigin, gridWidth, meshSize);
                var tempProfile = new XSectionProfile(gridOrigin, gridWidth, meshSize);
                var jetRayList = new List<Ray2>();


                string jetscanFilename = "dn005-dm040-60deg.csv";
                //jetscanFilename = "parabolaJet.csv";
                //jetscanFilename = "halfcirclejet.csv";
                var jet = new XSecJet( jetscanFilename,jetDiameter);
                
                string pathCsvFilename =  "singlepath-profile-toolpath.csv";
                //pathCsvFilename =  "BOT-profile-toolpath-depthmeasure-test-nick.csv";
                //pathCsvFilename = "bottom25.csv";
                //pathCsvFilename = "top35.csv";
                //build path from path entities 
                //path is list of path entities 
                //jet ray list is list of jet rays 
                //pathExOrder,xLocation(across channel),yLocation(along channel),feed, direction,Depth,TargetDepth"
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
                        double jetX = x - pathList[pathIndex].CrossLoc;
                        double mrr =  jet.GetMrr(jetX)* nominalFeedrate/ pathList[pathIndex].Feedrate;                        
                        var jetRay = new Ray2(origin, direction, mrr);                       
                        jetArr[pathIndex, jetLocIndex] = jetRay; 
                    }
                }

                var intersectList = new List<GridIntersect>();
                Console.WriteLine("running model");           
                var angleEffectList = new List<string>();
                var normalsList = new List<string>();
                double[] inspectionDepthArr = new double[runCount];
                string timeCode=  DateTime.Now.ToFileTimeUtc().ToString();
                for (int iter = 0; iter < iterCount; iter++)
                {
                    profile = new XSectionProfile(gridOrigin, gridWidth, meshSize);                    
                    for (int i = 0; i < runCount; i++)
                    {
                        
                        for (int pathIndex = 0; pathIndex < jetArr.GetLength(0); pathIndex++)
                        {
                            tempProfile = new XSectionProfile(gridOrigin, gridWidth, meshSize);
                            for (int jetLocIndex = 0; jetLocIndex < jetArr.GetLength(1); jetLocIndex++)
                            {
                                var jetRay = jetArr[pathIndex, jetLocIndex];
                                var profileNormal = profile.GetNormalAngle(jetRay.Origin.X,averagingWindow);
                                var angleEffect = 1; AngleEffect(jetRay, profileNormal, critAngle, angExpEffect);
                                if (i == runCount - 1)
                                {
                                    angleEffectList.Add(jetRay.Origin.X.ToString() + "," + angleEffect.ToString());
                                }
                                double materialRemoved = baseMrr * jetRay.Length * angleEffect;
                                double currentDepth = profile.GetValue(jetRay.Origin.X);
                                double newDepth = currentDepth + materialRemoved;
                                tempProfile.SetValue(newDepth, jetRay.Origin.X);
                            }
                            tempProfile.Smooth();
                            profile = new XSectionProfile(tempProfile);
                            
                           // profile.SaveBitmap(directory +"testgrid" + timeCode +"-iter"+iter.ToString()+ "-run" + i.ToString()+ ".bmp");
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
                            for (int profIndex = 0; profIndex < profile.Count; profIndex++)
                            {
                                double x = profIndex * profile.MeshSize + profile.Origin.X;
                                var n = profile.GetNormalAngle(x,averagingWindow);
                                normalsList.Add(x.ToString()+","+n.ToString());

                            }
                        }

                    }
                }
                
                Console.WriteLine("saving model");
                timeCode = System.DateTime.Now.ToFileTimeUtc().ToString();
                FileIOLib.FileIO.Save(normalsList, directory+"normalList"+timeCode +".csv");
                FileIOLib.FileIO.Save(angleEffectList, directory+"angleEffectList"+timeCode +".csv");
               
                string csvFilename =directory+ "testprofile-" + timeCode+".csv";
                profile.SaveCSV(csvFilename);
                string bitmapFilename = directory + "testgrid" + timeCode + ".bmp";
                profile.SaveBitmap(bitmapFilename);
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
            
            double a = 1.5 -   Math.Abs(Math.Cos(Math.Abs(normalAngle)-critAngle)) ;
            double angleEffect = a;
            //angleEffect = Math.Pow(Math.Abs(a),.5);
            //angleEffect = Math.Exp(-1.0 * expF* Math.Pow(a, 2.0));
           // angleEffect = .32 - .0729 * a + 1.826 * Math.Pow(a, 2.0)-4.9745 * Math.Pow(a, 3.0) + 5.8245 * Math.Pow(a, 4.0) -2.2085 * Math.Pow(a, 5.0);
           
            return angleEffect;
        }
    }
        
}
