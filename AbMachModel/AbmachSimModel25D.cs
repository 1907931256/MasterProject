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
    public class XSectionProfile:List<Vector2>
    {       
        
        public double MeshSize { get; private set; }
        public Vector2 Origin { get; private set; }
        public double Width { get; private set; }

        private int GetIndex(double xLocation)
        {
            int xi = (int)Math.Round((xLocation - Origin.X) / MeshSize);            
            return GetIndex(xi);
        }
        private int GetIndex(int index)
        {
            int i = Math.Min(Math.Max(index, 0), Count - 1);
            return i;
        }
        public double GetNormalAngle(double xLocation,double aveWindow)
        {
            int windowHalfW = (int)(.5 * aveWindow / MeshSize);
            int i0 = GetIndex(xLocation);            
            var x = new List<double>();
            var y = new List<double>();
            for(int i=i0-windowHalfW;i<=i0+windowHalfW;i++)
            {
                x.Add(this[GetIndex(i)].X);
                y.Add(this[GetIndex(i)].Y);
            }
            var mb = MathNet.Numerics.Fit.Line(x.ToArray(), y.ToArray());
            double a = Math.Atan(mb.Item2);
            return a;
        }
        Tuple<double,double> GetMinMax()
        {
            double max = double.MinValue;
            double min = double.MaxValue;
            for (int i = 0; i < Count; i++)
            {
               if(this[i].Y>max)
                {
                    max = this[i].Y;
                }
               if(this[i].Y<min)
                {
                    min = this[i].Y;
                }
            }
            return new Tuple<double, double>(min, max);
        }
        public void SaveBitmap(string filename)
        {
            try
            {
                Bitmap bitmap;
                var minmax = GetMinMax(); 
                int yLength = (int)Math.Ceiling(Math.Abs(minmax.Item2-minmax.Item1) / MeshSize);
                int xLength = Count;
                bitmap = new Bitmap(xLength, yLength);
                Size s = new Size(bitmap.Width, bitmap.Height);

                // Create graphics 
                for (int i = 0; i < xLength; i++)
                {
                    int j = Math.Min(yLength-1,Math.Max(0,(int)Math.Floor((this[i].Y-minmax.Item1)/ MeshSize)));                  
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
        public void SaveCSV(string filename)
        {
            try
            {
                var pointList = new List<string>();
                for(int i=0;i<Count;i++)
                {
                   
                    string pointStr = this[i].X.ToString() + "," + (-1.0*this[i].Y).ToString();
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
            var smoothProfile = new List<Vector2>();
           



            for (int i = 0; i <= Count - aveWindow; i+= aveWindow)
            {
                var x = new List<double>();
                var y = new List<double>();
                for (int j = i; j < i + aveWindow; j++)
                {
                    x.Add(this[j].X);
                    y.Add(this[j].Y);
                   
                }
              //  var fitFunc = MathNet.Numerics.Fit.LineFunc(x.ToArray(), y.ToArray());
               var fitFunc =  MathNet.Numerics.Fit.PolynomialFunc(x.ToArray(), y.ToArray(),4);
                
               for (int j = 0; j < x.Count; j++)
               {
                    var yFit = fitFunc(x[j]);
                    smoothProfile.Add(new Vector2(x[j],yFit ));
               }
            }
            Clear();
            AddRange(smoothProfile);
        }
        public void Smooth()
        {
            var smoothProfile = new List<Vector2>();
            smoothProfile[0] = this[0];
           
            for (int i=1;i<Count-1;i++)
            {
                if(((this[i].Y>this[i-1].Y)&&(this[i].Y>this[i+1].Y))
                    || ((this[i].Y > this[i - 1].Y) && (this[i].Y > this[i + 1].Y)))
                {
                    var ySmoothed = (this[i - 1].Y + this[i + 1].Y) / 2.0;
                    smoothProfile.Add(new Vector2(this[i].X, ySmoothed));
                }
                else
                {
                    smoothProfile.Add(this[i]);
                }
            }
            var temp = this[Count - 1];
            Clear();
            AddRange(smoothProfile);
            smoothProfile.Add(temp);
        }
        public void SetValue (double value, double location)
        {
            this[GetIndex(location)].Y = value;
        }
        public double GetValue(double location)
        {
            return this[GetIndex(location)].Y;
        }

        void BuildProfile()
        {
            double x = Origin.X;
            while(x<Origin.X+Width)
            {
                Add(new Vector2(x, Origin.Y));
                x += MeshSize;
            }
        }
       
        public XSectionProfile(Vector2 origin, double width, double meshSize)
        {
            Origin = origin;
            MeshSize = meshSize;
            Width = width;
            BuildProfile();

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
                if(double.TryParse(pointArr[i, 0], out x) && double.TryParse(pointArr[i, 1], out y))
                {
                    Add(new Vector2(x, y));
                }                
            }
        }
        public XSectionProfile(XSectionProfile startingProfile)
        {
            Origin = startingProfile.Origin;
               
            MeshSize = startingProfile.MeshSize;
            AddRange(startingProfile);
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
        XSecJet jet;
        XSecPathList pathList;
        RunInfo _runInfo;
        AbMachParameters _abMachParams;
        XSectionProfile profile;
        double meshSize;
        double jetR;
        double nominalFeedrate;
        public ChannelModel(XSectionProfile profile, XSecJet xSecJet, XSecPathList path,RunInfo runInfo,AbMachParameters abMachParameters)
        {
            jet = xSecJet;
            pathList = path;
            _runInfo = runInfo;
            _abMachParams = abMachParameters;
            this.profile = profile;
        }
        Ray2[,]GetJetArray()
        {
            int jetW = (int)(Math.Ceiling(jetR * 2 / meshSize));
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
                    double mrr = jet.GetMrr(jetX) * nominalFeedrate / pathList[pathIndex].Feedrate;
                    var jetRay = new Ray2(origin, direction, mrr);
                    jetArr[pathIndex, jetLocIndex] = jetRay;
                }
            }
            return jetArr;
        }
        public void Run(CancellationToken ct, IProgress<int> progress)
        {
            try
            {

                var meshSize = _abMachParams.MeshSize;
                var gridOrigin = profile.Origin;
                var gridWidth = profile.Width;
                var jetArr = GetJetArray();
                var tempProfile = new XSectionProfile(gridOrigin, gridWidth, meshSize);
                var jetRayList = new List<Ray2>();
                var baseMrr = _abMachParams.RemovalRate.DepthPerPass;
                var averagingWindow = _abMachParams.SmoothingWindowWidth;
                var critAngle = _abMachParams.Material.CriticalRemovalAngle;
                for (int iter = 0; iter < _runInfo.Iterations; iter++)
                {
                    profile = new XSectionProfile(gridOrigin, gridWidth, meshSize);
                    for (int i = 0; i < _runInfo.Runs; i++)
                    {

                        for (int pathIndex = 0; pathIndex < jetArr.GetLength(0); pathIndex++)
                        {
                            tempProfile = new XSectionProfile(gridOrigin, gridWidth, meshSize);
                            for (int jetLocIndex = 0; jetLocIndex < jetArr.GetLength(1); jetLocIndex++)
                            {
                                var jetRay = jetArr[pathIndex, jetLocIndex];
                                var profileNormal = profile.GetNormalAngle(jetRay.Origin.X, averagingWindow);
                                var angleEffect = AngleEffect( profileNormal, critAngle);
                                //if (i == _runInfo.Runs - 1)
                                //{
                                //    angleEffectList.Add(jetRay.Origin.X.ToString() + "," + angleEffect.ToString());
                                //}
                                double materialRemoved = baseMrr * jetRay.Length * angleEffect;
                                double currentDepth = profile.GetValue(jetRay.Origin.X);
                                double newDepth = currentDepth + materialRemoved;
                                tempProfile.SetValue(newDepth, jetRay.Origin.X);
                            }
                            tempProfile.Smooth();
                            profile = new XSectionProfile(tempProfile);

                            // profile.SaveBitmap(directory +"testgrid" + timeCode +"-iter"+iter.ToString()+ "-run" + i.ToString()+ ".bmp");
                        }
                        double inspectionDepth = profile.GetValue(_abMachParams.DepthInfo.LocationOfDepthMeasure.X);                     Console.WriteLine("Run: " + i.ToString() + " Depth: " + inspectionDepth.ToString());
                        double targetDepthAtRun = _abMachParams.DepthInfo.TargetDepth * (i + 1) / _runInfo.Runs;
                        Console.WriteLine("targetDepthAtRun: " + targetDepthAtRun.ToString());
                        double mrrAdjustFactor = targetDepthAtRun / inspectionDepth;
                        Console.WriteLine("mrrAdjustFactor: " + mrrAdjustFactor.ToString());
                        if (_abMachParams.RunInfo.RunType == ModelRunType.NewMRR)
                        {
                            baseMrr *= mrrAdjustFactor;
                            Console.WriteLine("new Mrr: " + baseMrr.ToString());
                        }
                        Console.WriteLine("");
                        var angleEffectList = new List<string>();
                        var normalsList = new List<string>();
                        if (i == _abMachParams.RunInfo.Runs - 1)
                        {
                            for (int profIndex = 0; profIndex < profile.Count; profIndex++)
                            {
                                double x = profIndex * profile.MeshSize + profile.Origin.X;
                                var n = profile.GetNormalAngle(x, averagingWindow);
                                normalsList.Add(x.ToString() + "," + n.ToString());

                            }
                        }

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
        double AngleEffect( double normalAngle, double critAngle)
        {

            double a = 1.5 - Math.Abs(Math.Cos(Math.Abs(normalAngle) - critAngle));
            double angleEffect = a;
            //angleEffect = Math.Pow(Math.Abs(a),.5);
            //angleEffect = Math.Exp(-1.0 * expF* Math.Pow(a, 2.0));
            // angleEffect = .32 - .0729 * a + 1.826 * Math.Pow(a, 2.0)-4.9745 * Math.Pow(a, 3.0) + 5.8245 * Math.Pow(a, 4.0) -2.2085 * Math.Pow(a, 5.0);

            return angleEffect;
        }
    }
}
