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
            double width = .4;
            double depth = .25;
            double meshSize = .0001;
            byte startValue = 255;
            double alongLocation = 0.0;
            Console.WriteLine("building grid");
            var gridOrigin = new Vector2(-.2, 0);
            var grid = new XSectionGrid(gridOrigin,width, depth, meshSize, startValue, alongLocation);
            var profile = new XSectionProfile(gridOrigin, width, meshSize);

            var jetRayList = new List<Ray2>();
            double jetCenterX = width/2.0;
            double jetR = .022;
            int eqIndex = 1;
            double nominalFeedrate = 40;
            var jet = new XSecJet(eqIndex, jetR * 2, meshSize);
            string pathCsvFilename = "path155mm_test.csv";
            //build path from path entities 

            //path is list of path entities 
            //jet ray list is list of jet rays 
            var pathList = new XSecPathList(pathCsvFilename, 1);
            int pathCount = pathList.Count;
            double baseMrr = .005;
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
                    double mrr = baseMrr * jet.GetMrr(x - pathList[pathIndex].CrossLoc)* nominalFeedrate/ pathList[pathIndex].Feedrate;
                    //if(mrr>0)
                    //{
                        var jetRay = new Ray2(origin, direction, mrr);
                        //jetRayList.Add(jetRay);
                        
                   // }
                    
                    
                }
            }
            

            var intersectList = new List<GridIntersect>();
            int runCount =5;
            Console.WriteLine("running model");
           
            var angleEffectList = new List<string>();
            var normalsList = new List<string>();
            double particleDiam = .002;

            for (int i = 0; i < runCount; i++)
            {
                Console.WriteLine("run " + i.ToString());
                intersectList = new List<GridIntersect>();
                for(int locIndex=0; locIndex < jetArr.GetLength(0); locIndex++)
                {
                    for(int jet)
                }
                //foreach (Ray2 ray in jetRayList)
                //{
                //    if(ray.Length>0)
                //    {
                //        intersectList.Add(grid.GetXYIntersectOf(ray));
                //    }
                    
                //}

                double critAngle = Math.PI * 70.0 / 180.0;
                

                foreach (GridIntersect intersect in intersectList)
                {
                    double angleEffect = 1;//AngleEffect(intersect.IntersectRay, intersect.Normal, critAngle);
                    if(i == runCount-1)
                    {
                        angleEffectList.Add(intersect.IntersectRay.Origin.X + "," + angleEffect.ToString());                       
                        normalsList.Add(intersect.IntersectRay.Origin.X + "," + intersect.Normal.X.ToString() + "," + intersect.Normal.Y.ToString());
                    }
                    double mrr = baseMrr * intersect.IntersectRay.Length * angleEffect;
                    if (mrr > 0)
                    {
                       grid.SetRadialValuesAt(intersect, particleDiam, mrr, 0);
                       // grid.SetValuesAt(intersect,particleDiam, mrr, 0);
                    }
                }
                grid.SmoothValues();
            }

            FileIOLib.FileIO.Save(angleEffectList, "angleEffect.csv");
            FileIOLib.FileIO.Save(normalsList, "normalList.csv");

            Console.WriteLine("saving grid");
            grid.SaveBitmap("testgrid.bmp");
        }
        static double piOver2 = Math.PI / 2.0;
        static double piOver4 = Math.PI / 4.0;
        static double AngleEffect(Ray2 jet, Vector2 normal,double critAngle)
        {
            double angle = Math.Abs(normal.AngleTo(jet.Direction));
            angle %= piOver4;
            if( angle>piOver4)
            {
                angle -= piOver2;
            }
            double angleCenter =Math.Abs(angle) - critAngle;

            double angleEffect = Math.Exp(-1 * Math.Pow(angleCenter, 2.0));
            return angleEffect;
        }
    }
        
}
