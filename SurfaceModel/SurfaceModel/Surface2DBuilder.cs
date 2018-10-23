using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryLib;

namespace SurfaceModel
{
   public enum Surface2DType
    {
        XZCurve,
        YZCurve,
        XYZSurface,
    }
    
    public class Surface2DBuilder<T> where T : SurfacePoint, new()
    {
       
       
        public static Surface2D<T> Build(List<T> Points,double meshSize)
        {
            
            var pointList = new List<Vector3>();
            foreach(T pt in Points)
            {
                
                if (pt != null)
                    pointList.Add(pt.Position);
            }
            return Build(pointList, meshSize);
        }
        public static Surface2D<T>Build (BoundingBox boundingBox,Surface2DType type,List<T> points,double meshSize)
        {
            switch(type)
            {
                case Surface2DType.XYZSurface:
                default:
                    return BuildXYZ(boundingBox, points, meshSize);
                    
                case Surface2DType.XZCurve:
                    return BuildXZ(boundingBox, points, meshSize);
                    
                case Surface2DType.YZCurve:
                    return BuildYZ(boundingBox, points, meshSize);
                    
            }
        }
        static Surface2D<T> BuildXZ(BoundingBox boundingBox, List<T> points, double meshSize)
        {
            var surf = new Surface2D<T>(boundingBox, meshSize);
            var xArr = new double[points.Count];
            var zArr = new double[points.Count];
            for (int j = 0; j < points.Count - 1; j++)
            {
                xArr[j] = points[j].Position.X;
                zArr[j] = points[j].Position.Z;
            }
            for (int k = 0; k < surf.XSize; k++)
            {
                double x = surf.GetPointAt(k,0).Position.X;
                double z = getZValue(x, xArr, zArr);
                for (int i = 0; i < surf.YSize; i++)
                {
                    var t = new T();
                    double y = surf.GetPointAt(k,i).Position.Y;
                    t.Position = new Vector3(x, y, z);
                    surf.SetValue(t, k,i);
                }
            }
            return surf;
        }
        static Surface2D<T> BuildXYZ(BoundingBox boundingBox, List<T> points, double meshSize)
        {
            var surf = new Surface2D<T>(boundingBox, meshSize);

            return surf;
        }
        static Surface2D<T>BuildYZ(BoundingBox boundingBox, List<T>points,double meshSize)
        {
            
            var surf = new Surface2D<T>(boundingBox, meshSize);
            var yArr = new double[points.Count];
            var zArr = new double[points.Count];
            for (int j = 0; j < points.Count - 1; j++)
            {
                yArr[j] = points[j].Position.Y;
                zArr[j] = points[j].Position.Z;
            }
            for (int k=0;k<surf.YSize;k++)
            {
                double y = surf.GetPointAt(0,k).Position.Y;
                double z = getZValue(y, yArr, zArr);
                for(int i=0; i<surf.XSize;i++)
                {
                    var t = new T();
                    double x = surf.GetPointAt(i, k).Position.X;
                    t.Position = new Vector3(x, y, z);
                    surf.SetValue(t, i, k);
                }
            }
            return surf;
            
        }
        static double getZValue(double input,double[] hSorted,double[] vSorted)
        {
            double hstart = hSorted[0];
            double hend = hSorted[hSorted.Length-1];
            double vstart = vSorted[0];
            double vend = vSorted[vSorted.Length-1];
            double result=0;
            if (input > hstart && input < hend)
            {
                for (int i = 0; i < hSorted.Length - 1; i++)
                {
                    if (hSorted[i] <= input && hSorted[i + 1] >= input)
                    {
                        hstart = hSorted[i];
                        hend = hSorted[i + 1];
                        vstart = vSorted[i];
                        vend = vSorted[i + 1];
                        break;
                    }
                }
                if (vstart != vend)
                {
                    result = (input - hstart) * (vend - vstart)/ (hend - hstart)  + hstart;
                }
                else
                {
                    result = vstart;
                }
            }
            else if(input <=hstart)
            {
                result = vstart;
            }
            else
            {
                result = vend;
            }
            return result;
        }
        public static Surface2D<T> Build(List<Vector3> Points,  double meshSize)
        {
            var boundingBox = BoundingBoxBuilder.FromPtArray(Points.ToArray());
            var surface = new Surface2D<T>(boundingBox, meshSize);
           
            return surface;
        }
        public static Surface2D<T> Build(BoundingBox _boundingBox, double meshSize)
        {
            
            var surface = new Surface2D<T>(_boundingBox, meshSize);

            return surface;
        }
        public static Surface2D<T> Build(BoundingBox _boundingBox,double initValue, double meshSize)
        {

            var surface = new Surface2D<T>(_boundingBox,initValue, meshSize);

            return surface;
        }
    }
}
