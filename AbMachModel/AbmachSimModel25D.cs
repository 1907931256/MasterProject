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
    public class GridIntersect
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Vector2 Normal { get; set; }
        public Ray2 IntersectRay { get; set; }
        public double Mrr { get; set; }
    }
    public class GridXSection
    {
        public int Index { get; set; }
        public double AlongLocation { get; private set; }
        byte[,] grid;
        double _meshSize;
        byte _critFillValue;
        double yHeight;
        double xWidth;
        int xLength;
        int yLength;

        Point GetIndices(double xModel, double yModel)
        {
           
            int i =Math.Max(0,Math.Min(xLength, (int)Math.Round(xModel / _meshSize)));
            int j =Math.Max(0,Math.Min(yLength, (int)Math.Round(yModel / _meshSize)));
            return new Point(i, j);
        }
        void SetValue(int xi, int yj,byte value)
        {
            int i = Math.Min(Math.Max(xi,0), grid.GetUpperBound(0) - 1);
            int j = Math.Min(Math.Max(yj, 0), grid.GetUpperBound(1) - 1);
            grid[i, j] = value;

        }
        Vector2 GetNormal(Point loc)
        {
            int searchR = 8;
            var BoundaryPts = new List<Point>();

            int startX = Math.Max(1, loc.X - searchR);
            int startY = Math.Max(1, loc.Y - searchR);
            int endX = Math.Min(grid.GetUpperBound(0)-1, loc.X + searchR);
            int endY = Math.Min(grid.GetUpperBound(1)-1, loc.Y + searchR);

            for (int i = startX; i < endX; i++)
            {
                for (int j = startY; j < endY; j++)
                {
                    if (grid[i, j] > _critFillValue)
                    {
                        if (grid[i - 1, j] <= _critFillValue || grid[i + 1, j] <= _critFillValue || grid[i, j - 1] <= _critFillValue || grid[i, j + 1] <= _critFillValue)
                        {
                            BoundaryPts.Add(new Point(i, j));
                        }
                    }
                }
            }
            var normal = new Vector2();
            if(BoundaryPts.Count>=2)
            {
                double[] xfit = new double[BoundaryPts.Count];
                double[] yfit = new double[BoundaryPts.Count];
                for (int k = 0; k < BoundaryPts.Count; k++)
                {
                    xfit[k] = BoundaryPts[k].X;
                    yfit[k] = BoundaryPts[k].Y;
                }
                var coeffs = MathNet.Numerics.Fit.Line(xfit, yfit);
                var slope = Math.Atan(coeffs.Item2) + (Math.PI / 2.0); 
                if(slope<0)
                {
                    slope += Math.PI;
                }
                normal = new Vector2(Math.Cos(slope),Math.Sin(slope));
            }
            else
            {
                normal = new Vector2(0, 1);
            }
           
            return normal;
        }
        public GridIntersect GetXYIntersectOf(Ray2 ray)
        {
            var origin = GetIndices(ray.Origin.X, ray.Origin.Y);
            double dx = _meshSize / ray.DirectionTan;
            double dy = _meshSize;            
            byte cellValue = GetValueAt(origin);
            double x = ray.Origin.X;
            double y = ray.Origin.Y;
            var loc = new Point(origin.X,origin.Y);
            if(cellValue<=_critFillValue)
            {
                while (x < xWidth && y < yHeight)
                {
                    x += dx;
                    y += dy;
                    loc = GetIndices(x, y);
                    cellValue = GetValueAt(loc);
                    if (cellValue > _critFillValue)
                    {
                        break;
                    }
                }
            }           
            var normal = GetNormal(loc);
            return new GridIntersect { X = loc.X, Y = loc.Y, IntersectRay = ray,Normal = normal };
        }
        public void SetValuesAt(GridIntersect loc, double width,double height, byte value)
        {           
            int indexW = (int)Math.Round(width / _meshSize);
            int indexH = (int)Math.Round(height / _meshSize);           
            int startX = Math.Max(loc.X - indexW, 0);
            int startY = loc.Y;
            int endX = Math.Min(loc.X + indexW, xLength - 1);
            int endY = Math.Min(loc.Y + indexH, yLength - 1);

            for (int xi = startX; xi <= endX; xi++)
            {
                for (int yi = startY; yi <= endY; yi++)
                {
                   grid[xi,yi]= value;
                }
            }
        }
        public void SetRadialValuesAt(GridIntersect loc, double radius,byte value)
        {
            int indexR = (int)Math.Round(radius / _meshSize);
            int indexR2 = (int)Math.Pow(indexR, 2.0);
            int startX = Math.Max(loc.X - indexR, 0);
            int startY = loc.Y;
            int endX = Math.Min(loc.X + indexR, xLength-1);
            int endY = Math.Min(loc.Y+ indexR, yLength-1);
            for(int xi = startX;xi<=endX;xi++)
            {
                for(int yi=startY;yi<=endY;yi++)
                {
                    double r2 = Math.Pow(xi - loc.X, 2.0) + Math.Pow(yi - loc.Y, 2.0);
                    if(r2<=indexR2)
                    {
                        SetValue(xi, yi, value);
                    }
                }
            }
        }
        public void SetRadialValuesAt(GridIntersect loc, double width,double height, byte value)
        {
            int indexWidth = (int)Math.Round(width / _meshSize);           
            int indexHeight = (int)Math.Round(height / _meshSize);
            int startY = Math.Max(0, loc.Y);
            double radius = width / 2.0;
            int endY = startY;

            if (indexHeight > 0)
            {
               endY= Math.Max(0,Math.Min(loc.Y + indexHeight , yLength - 1));
            }
            for(int yi = startY;yi<=endY;yi++)
            {
                var circleCenter = new GridIntersect { X = loc.X, Y = yi };
                SetRadialValuesAt(circleCenter, radius, value);
            }
        }
        private byte GetValueAt(Point location)
        {
            return grid[location.X, location.Y];
        }
        public void SetValueAt(GridIntersect loc, byte value)
        {
            SetValueAt(loc.X, loc.Y,value);
        }
        public void SetValueAt(double x,double y, byte value)
        {
            var loc = GetIndices(x, y);
            grid[loc.X, loc.Y] = value;
        }
        double halfWidth;
        public void SaveCSV(string filename)
        {
            var file = new List<string>();
            for (int j = 0; j < yLength; j++)
            {
                string line = "";
                for (int i = 0; i < xLength; i++)
                {
                    line += grid[i, j].ToString() + ",";
                }
                file.Add(line);
            }
            FileIO.Save(file, filename);
        }
        public void SaveBitmap(string filename)
        {
            try
            {
                Bitmap bitmap;
                bitmap = new Bitmap(xLength,yLength);
                Size s = new Size(bitmap.Width, bitmap.Height);

                // Create graphics 
                for(int i=0;i<xLength;i++)
                {
                    for(int j=0;j<yLength;j++)
                    {
                        Color c =  Color.FromArgb(10, 10, grid[i, j]);
                        bitmap.SetPixel(i, j, c);
                    }
                }
                
                Graphics memoryGraphics = Graphics.FromImage(bitmap);                              
                bitmap.Save(filename);
                
            }
            catch (Exception)
            {

                throw;
            }
        }
        public GridXSection(double width,double depth, double meshSize,byte startValue, double alongLocation)
        {
            xLength = (int)Math.Round(width / meshSize);
            yLength = (int)Math.Round(depth / meshSize);
            halfWidth = width / 2.0;
            xWidth = width;
            yHeight = depth;
            grid = new byte[xLength,yLength];
            for(int i=0;i<xLength;i++)
            {
                for(int j=0;j<yLength;j++)
                {
                    grid[i, j] = startValue;
                }
            }
            _meshSize = meshSize;
            AlongLocation = alongLocation;
        }
       
    }
    public class ChannelModel
    {
        XSecJet _xSecJet;
        ChannelPath _path;
        RunInfo _runInfo;
        AbMachParameters _abMachParams;
        

        public ChannelModel(XSecJet xSecJet,ChannelPath path,RunInfo runInfo,AbMachParameters abMachParameters)
        {
            _xSecJet = xSecJet;
            _path = path;
            _runInfo = runInfo;
            _abMachParams = abMachParameters;
           
        }
        public void Run(CancellationToken ct, IProgress<int> progress)
        {
            try
            {
                foreach(ChannelXSection cspath in _path)
                {
                   
                    for(int i=0;i<cspath.Count;i++)
                    {
                       
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        double AngleRemoval(Vector2 angle,Vector2 jetAngle)
        {
            double result = 1;// Math.Cos(angle - _abMachParams.Material.CriticalRemovalAngle);
            return result;
        }
    }
}
