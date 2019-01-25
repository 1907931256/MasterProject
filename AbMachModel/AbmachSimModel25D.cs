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
    public class XSectionProfile
    {
        double[] profile;
        
        public double MeshSize { get; private set; }
        public Vector2 Origin { get; private set; }
        public int XLength { get; private set; }

        private int GetIndex(double xLocation)
        {
            int xi = (int)Math.Round((xLocation - Origin.X) / MeshSize);            
            return GetIndex(xi);
        }
        private int GetIndex(int index)
        {
            int i = Math.Min(Math.Max(index, 0), XLength - 1);
            return i;
        }
        public double GetNormalAngle(double xLocation)
        {
            int windowHalfW = 5;
            int i0 = GetIndex(xLocation);
            double m1 = 0;
            double y0 = profile[i0];
            int count = 0;
            for(int i=i0-windowHalfW;i<i0;i++)
            {
                count++;
                m1 += (y0 - profile[GetIndex(i)])/(MeshSize*(i0-i));
            }
            m1 /= count;
            count = 0;
            double m2 = 0;
            for(int i=i0+1;i<i0+windowHalfW;i++)
            {
                count++;
                m2 += (profile[GetIndex(i)] - y0) / (MeshSize * (i - i0));
            }
            m2 /= count;

            double m = (m1 + m2 ) / 2.0;
            double angle = Math.Atan(m) ;
            return angle;
        }
        double GetMaxValue()
        {
            double max = double.MinValue;
            for (int i = 0; i < profile.Length; i++)
            {
                var absVal = Math.Abs(profile[i]);
                if (absVal > max)
                {
                    max = absVal;
                }
            }
            return max;
        }
        public void SaveBitmap(string filename)
        {
            try
            {
                Bitmap bitmap;
                int yLength = (int)Math.Ceiling(GetMaxValue() / MeshSize);
                int xLength = profile.Length;
                bitmap = new Bitmap(xLength, yLength);
                Size s = new Size(bitmap.Width, bitmap.Height);

                // Create graphics 
                for (int i = 0; i < xLength; i++)
                {
                    int j = (int)Math.Floor(profile[i] / MeshSize);
                    if (j >= yLength)
                    {
                        j = yLength - 1;
                    }
                    Color c = Color.FromArgb(10, 10, 10);
                    bitmap.SetPixel(i, j, c);
                }

                Graphics memoryGraphics = Graphics.FromImage(bitmap);
                bitmap.Save(filename);

            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<Vector2> ParseFile(string scanFilename, double minYValue, double maxYValue)
        {
            var stringArr = FileIOLib.CSVFileParser.ParseFile(scanFilename);
            var vectorList = new List<Vector2>();
            for (int i = 0; i < stringArr.GetLength(0); i++)
            {
                double x = 0;
                double y = maxYValue * 2;
                if (double.TryParse(stringArr[i, 0], out x) && double.TryParse(stringArr[i, 1], out y))
                {
                    if (y >= minYValue && y <= maxYValue)
                    {
                        var v = new Vector2(x, y);
                        vectorList.Add(v);
                    }
                }

            }
            return vectorList;
        }
        //public List<Vector2> TranslateScan(List<Vector2> scan, Vector2 translation)
        //{

        //}
        //public List<Vector2> MirrorScan(List<Vector2> scan, Vector2 mirrorLine)
        //{

        //}

        //public List<double> GetDifferenceProfile(List<Vector2> scan1, List<Vector2> scan2)
        //{
        //    double error = 0;
        //    var vectorList = ParseFile(targetCsvProfile,minY,maxY);
            
           
        //    for(int j= 0;j<XLength;j++)
        //    {
        //        double xprof = Origin.X + (j * MeshSize);

        //    }
        //    return error;
        //}
        public void SaveCSV(string filename)
        {
            try
            {
                var pointList = new List<string>();
                for(int i=0;i<profile.Length;i++)
                {
                    double x = i * MeshSize + Origin.X;
                    string pointStr = x.ToString() + "," + (-1.0*profile[i]).ToString();
                    pointList.Add(pointStr);
                }
                FileIO.Save(pointList, filename);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public void Smooth(double rollingAveWidth)
        {
            int aveWindow = (int)Math.Round(rollingAveWidth / MeshSize);
            double[] smoothProfile = new double[XLength];
            smoothProfile[0] = profile[0];
            smoothProfile[XLength - 1] = profile[XLength - 1];

            for (int i = 0; i < profile.Length - aveWindow-1; i+=aveWindow)
            {
                double sum = 0;
                for(int j= i;j<i+ aveWindow;j++)
                {
                    sum += profile[j];
                }
                sum /= aveWindow;
                for (int j = i; j < i + aveWindow; j++)
                {
                    smoothProfile[j] =sum;
                }
            }
            for (int i = 1; i < profile.Length - 1; i++)
            {
                profile[i] = smoothProfile[i];
            }

        }
        public void Smooth()
        {
            double[] smoothProfile = new double[XLength];
            smoothProfile[0] = profile[0];
            smoothProfile[XLength-1] = profile[XLength-1];

            for (int i=1;i<profile.Length-1;i++)
            {
                if(((profile[i]>profile[i-1])&&(profile[i]>profile[i+1]))|| ((profile[i] > profile[i - 1]) && (profile[i] > profile[i + 1])))
                {
                    smoothProfile[i] = (profile[i - 1] + profile[i + 1]) / 2.0;
                }
                else
                {
                    smoothProfile[i] = profile[i];
                }
            }
            for (int i = 1; i < profile.Length - 1; i++)
            {
                profile[i] = smoothProfile[i];
            }

        }
        public void SetValue (double value, double location)
        {
            profile[GetIndex(location)] = value;
        }
        public double GetValue(double location)
        {
            return profile[GetIndex(location)];
        }
        public XSectionProfile(Vector2 origin, double width, double meshSize)
        {
            Origin = origin;
            MeshSize = meshSize;
            XLength = (int)Math.Ceiling(width / meshSize);
            profile = new double[XLength];

        }
        public XSectionProfile(string XsectionCsvFile)
        {
            string[,] pointArr = CSVFileParser.ParseFile(XsectionCsvFile);
            double x = 0;
            double y = 0;
            double.TryParse(pointArr[0, 0], out x);
            double.TryParse(pointArr[0, 1], out y);
            Origin = new Vector2(x, y);
            var pointList = new List<double>();
            double x1 = 0;
            double.TryParse(pointArr[1, 0], out x1);
            MeshSize = Math.Abs(x1 - x);

            for (int i=0;i<pointArr.GetLength(0);i++)
            {               
                if(double.TryParse(pointArr[i, 1], out y))
                {
                    pointList.Add(y);
                }                
            }
            profile = pointList.ToArray();
            XLength = profile.Length;
        }
        public XSectionProfile(XSectionProfile startingProfile)
        {
            Origin = startingProfile.Origin;
            XLength = startingProfile.XLength;
            
            MeshSize = startingProfile.MeshSize;
            profile = new double[XLength];
            for (int i=0;i<profile.Length;i++)
            {
                double xLocation = (i * MeshSize) + Origin.X;
                profile[i] = startingProfile.GetValue(xLocation);
            }
        }
    }
    public class GridIntersect
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Vector2 Normal { get; set; }
        public Ray2 IntersectRay { get; set; }
        public double Mrr { get; set; }
    }
    public class XSectionGrid
    {
        public int Index { get; set; }
        public Vector2 Origin { get; private set; }
        public double AlongLocation { get; private set; }
        byte[,] grid;
        double _meshSize;
        byte _critFillValue;
        double yHeight;
        double xWidth;
        int xLength;
        int yLength;

        private Point GetIndices(double xModel, double yModel)
        {
           
            int i =Math.Max(0,Math.Min(xLength, (int)Math.Round((xModel-Origin.X) / _meshSize)));
            int j =Math.Max(0,Math.Min(yLength, (int)Math.Round((yModel-Origin.Y) / _meshSize)));
            return new Point(i, j);
        }
        private void SetValue(int xi, int yj,byte value)
        {
            int i = Math.Min(Math.Max(xi,0), grid.GetUpperBound(0) - 1);
            int j = Math.Min(Math.Max(yj, 0), grid.GetUpperBound(1) - 1);
            grid[i, j] = value;

        }
        private Vector2 GetNormal(Point loc)
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
        public void SmoothValues()
        {
            var intersectArr = new int[xLength];
            var smoothArr = new int[xLength];
            for (int xi = 0; xi<xLength;xi++)
            {
                int yi  = 0;
                byte cellValue = _critFillValue;
                while(yi <yLength && grid[xi, yi] <= _critFillValue)
                {                    
                    yi++;
                }
                intersectArr[xi] = yi;
            }
            smoothArr[0] = intersectArr[0];
            smoothArr[xLength-1] = intersectArr[xLength-1];
            for (int xi=1;xi<xLength-1;xi++)
            {
                if((intersectArr[xi]>intersectArr[xi-1] && intersectArr[xi]>intersectArr[xi+1])||
                    (intersectArr[xi] > intersectArr[xi - 1] && intersectArr[xi] > intersectArr[xi + 1]))
                {
                    smoothArr[xi] = intersectArr[xi - 1] + intersectArr[xi + 1];
                }
                else
                {
                    smoothArr[xi] = intersectArr[xi];
                }
            }
            for(int xi=0;xi<xLength;xi++)
            {
                for(int yi=0;yi<=smoothArr[xi];yi++)
                {
                    grid[xi, yi] = 0;
                }
                for(int yi= smoothArr[xi];yi<yLength;yi++)
                {
                    grid[xi, yi] = 255;
                }
            }
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
        public XSectionGrid(double width,double depth, double meshSize,byte startValue, double alongLocation)
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
            _critFillValue = 0;
            Origin = new Vector2(0, 0);
        }
        public XSectionGrid(Vector2 origin, double width, double depth, double meshSize, byte startValue, double alongLocation)
        {
            xLength = (int)Math.Round(width / meshSize);
            yLength = (int)Math.Round(depth / meshSize);
            halfWidth = width / 2.0;
            xWidth = width;
            yHeight = depth;
            grid = new byte[xLength, yLength];
            for (int i = 0; i < xLength; i++)
            {
                for (int j = 0; j < yLength; j++)
                {
                    grid[i, j] = startValue;
                }
            }
            _meshSize = meshSize;
            AlongLocation = alongLocation;
            _critFillValue = 0;
            Origin = origin;
        }
    }
    public class ChannelModel
    {
        XSecJet _xSecJet;
        XSecPathList _path;
        RunInfo _runInfo;
        AbMachParameters _abMachParams;
        

        public ChannelModel(XSecJet xSecJet, XSecPathList path,RunInfo runInfo,AbMachParameters abMachParameters)
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
                foreach(XSectionPathEntity cspath in _path)
                {
                   
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
