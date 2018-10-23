using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeometryLib
{
    public class BoundingBoxBuilder
    {
        public static BoundingBox Union(BoundingBox ext1, BoundingBox ext2)
        {
            BoundingBox[] BoundingBox = new BoundingBox[]{ext1,ext2};         
            return Union(BoundingBox);
        }
        public static BoundingBox Union(BoundingBox[] BoundingBox)
        {
            BoundingBox ext = new BoundingBox();
            double xmax = double.MaxValue * -1;
            double ymax = double.MaxValue * -1;
            double zmax = double.MaxValue * -1;
            double xmin = double.MaxValue;
            double ymin = double.MaxValue;
            double zmin = double.MaxValue;
            foreach (BoundingBox extent in BoundingBox)
            {
                xmax = Math.Max(xmax, extent.Max.X);
                ymax = Math.Max(ymax, extent.Max.Y);
                zmax = Math.Max(zmax, extent.Max.Z);
                xmin = Math.Min(xmin, extent.Min.X);
                ymin = Math.Min(ymin, extent.Min.Y);
                zmin = Math.Min(zmin, extent.Min.Z);
            }
            ext.Max = new Vector3(xmax, ymax, zmax);
            ext.Min = new Vector3(xmin, ymin, zmin);
            return ext;
        }
        public static BoundingBox CubeFromPtArray(List<Vector3> points)
        {
            try
            {
                BoundingBox boundingBox = BoundingBoxBuilder.FromPtArray(points.ToArray());
                Vector3 center = boundingBox.Center;
                Vector3 width = boundingBox.Max - boundingBox.Min;
                double maxDim = .5 * Math.Max(Math.Abs(width.X), Math.Max(Math.Abs(width.Y), Math.Abs(width.Z)));

                Vector3 size = new Vector3(maxDim, maxDim, maxDim);
                boundingBox = new BoundingBox(center - size, center + size);
                return boundingBox;
            }
            catch (Exception)
            {
                throw;
            }

        }
        static public BoundingBox GetSearchBox(Vector3 searchPoint, double searchRadius)
        {
            BoundingBox searchBox = new BoundingBox(
                                          searchPoint.X - searchRadius,
                                          searchPoint.Y - searchRadius,
                                          searchPoint.Z - searchRadius,
                                          searchPoint.X + searchRadius,
                                          searchPoint.Y + searchRadius,
                                          searchPoint.Z + searchRadius);
            return searchBox;
        }
        static public BoundingBox GetSearchCylinder(BoundingBox boundingBox, Vector3 searchPoint, Vector3 axis, double searchRadius)
        {
            BoundingBox searchBox = new BoundingBox(
                                          searchPoint.X - Math.Abs(boundingBox.Min.X * axis.X - searchRadius),
                                          searchPoint.Y - Math.Abs(boundingBox.Min.Y * axis.Y - searchRadius),
                                          searchPoint.Z - Math.Abs(boundingBox.Min.Z * axis.Z - searchRadius),
                                          searchPoint.X + Math.Abs(boundingBox.Max.X * axis.X + searchRadius),
                                          searchPoint.Y + Math.Abs(boundingBox.Max.Y * axis.Y + searchRadius),
                                          searchPoint.Z + Math.Abs(boundingBox.Max.Z * axis.Z + searchRadius));
            return searchBox;
        }
        public static BoundingBox FromPtArray(Vector3[] pts)
        {
            BoundingBox ext = new BoundingBox();

            double xmax = double.MaxValue * -1;
            double ymax = double.MaxValue * -1;
            double zmax = double.MaxValue * -1;
            double xmin = double.MaxValue;
            double ymin = double.MaxValue;
            double zmin = double.MaxValue;

            if(pts.Length>0)
            {
                foreach (Vector3 pt in pts)
                {
                    xmax = Math.Max(xmax, pt.X);
                    ymax = Math.Max(ymax, pt.Y);
                    zmax = Math.Max(zmax, pt.Z);
                    xmin = Math.Min(xmin, pt.X);
                    ymin = Math.Min(ymin, pt.Y);
                    zmin = Math.Min(zmin, pt.Z);
                }

                ext.Max = new Vector3(xmax, ymax, zmax);
                ext.Min = new Vector3(xmin, ymin, zmin);

            }
            else
            {
                ext.Max = new Vector3(0, 0, 0);
                ext.Min = new Vector3(0, 0, 0);
            }
            
            return ext;
        }

    }
}
