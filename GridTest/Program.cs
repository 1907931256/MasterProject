using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbMachModel;
using GeometryLib;
using System.Drawing;
namespace GridTest
{
    class Program
    {
        static void Main(string[] args)
        {
            double width = .15;
            double depth = .25;
            double meshSize = .0001;
            byte startValue = 255;
            double alongLocation = 0.0;
            Console.WriteLine("building grid");
            var grid = new GridXSection(width, depth, meshSize, startValue, alongLocation);
            var rays = new List<Ray2>();
            double jetCenterX = width/2.0;
            double jetR = .05;
            double endX = jetCenterX + jetR;
            double x = jetCenterX-jetR;
            int eqIndex = 2;
            var jet = new XSecJet(eqIndex, jetR * 2, meshSize);

            while (x <= endX)
            {
                var origin = new Vector2(x, 0);
                double angleDeg = 90;
                double angRad = Math.PI * (angleDeg / 180.0);
                var direction = new Vector2(Math.Cos(angRad), Math.Sin(angRad));
                double mrr = jet.GetMrr(x - jetCenterX);
                var ray = new Ray2(origin, direction, mrr);
                rays.Add(ray);
                x += meshSize;
            }

            var intersectList = new List<GridIntersect>();
            int runCount =2;
            Console.WriteLine("running model");
           
            var angleEffectList = new List<string>();
            var normalsList = new List<string>();
            double particleDiam = .002;

            for (int i = 0; i < runCount; i++)
            {
                intersectList = new List<GridIntersect>();

                foreach (Ray2 ray in rays)
                {
                    intersectList.Add(grid.GetXYIntersectOf(ray));
                }

                double critAngle = Math.PI * 70.0 / 180.0;
                double baseMrr = .005;

                foreach (GridIntersect intersect in intersectList)
                {
                    double angleEffect =  AngleEffect(intersect.IntersectRay, intersect.Normal, critAngle);
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
            double angle = normal.AngleTo(jet.Direction);
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
