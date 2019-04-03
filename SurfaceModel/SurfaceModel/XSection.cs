using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DwgConverterLib;
using GeometryLib;
using DataLib;
using System.Drawing;
using FileIOLib;
namespace SurfaceModel
{
    public class XSection:List<Vector2>
    {

        public List<DwgEntity> Entities { get; protected set; }
        public BoundingBox BoundingBox { get { return _boundingBox; } }
        public double MeshSize { get; protected set; }
        public Vector2 Origin { get; protected set; }
        public double Width { get; protected set; }


        protected BoundingBox _boundingBox;
         
        protected DisplayData cylDisplayData;
        protected DisplayData cartDisplayData;

        public DisplayData AsCartDisplayData()
        {
            return cartDisplayData;
        }

        public DisplayData AsTrimmedCartDisplayData(RectangleF window)
        {
            return cartDisplayData.TrimTo(window);
        }
        public DisplayData AsCylDisplayData()
        {
            return cylDisplayData;
        }
        public DisplayData AsTrimmedCylDisplayData(RectangleF window)
        {
            return cylDisplayData.TrimTo(window);
        }
       

        /// <summary>
        /// return list of entities rotated at current positon
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public List<DwgEntity> rotateEntities(double thetaRadians)
        {

            List<DwgEntity> rotEntities = new List<DwgEntity>();
            Vector3 origin = new Vector3(0, 0, 0);
            foreach (DwgEntity entity in Entities)
            {
                if (entity is Arc)
                {
                    Arc arc = entity as Arc;
                    rotEntities.Add(arc.RotateZ(origin, thetaRadians));
                }
                if (entity is Line)
                {
                    Line line = entity as Line;
                    rotEntities.Add(line.RotateZ(origin, thetaRadians));
                }
            }
            return rotEntities;
        }
        BoundingBox getBoundingBox(List<DwgEntity> entities)
        {
            BoundingBox ext = new BoundingBox();
            var boundingBoxList = new List<BoundingBox>();
            foreach (DwgEntity entity in entities)
            {
                if (entity is Line)
                {
                    Line line = entity as Line;

                    boundingBoxList.Add(line.BoundingBox());
                }
                if (entity is Arc)
                {
                    Arc arc = entity as Arc;

                    boundingBoxList.Add(arc.BoundingBox());
                }
            }
            ext = BoundingBoxBuilder.Union(boundingBoxList.ToArray());
            return ext;
        }

      

        protected int GetIndex(double xLocation)
        {
            int xi = (int)Math.Round((xLocation - Origin.X) / MeshSize);
            return xi;
        }
        protected bool CheckIndex(int index)
        {
            if (index >= 0 && index < this.Count)
                return true;
            else
                return false;
        }
        public double GetNormalAngle(double xLocation, double aveWindow)
        {
            int windowHalfW = (int)(.5 * aveWindow / MeshSize);
            int i0 = GetIndex(xLocation);
            var x = new List<double>();
            var y = new List<double>();
            for (int i = i0 - windowHalfW; i <= i0 + windowHalfW; i++)
            {
                if (CheckIndex(i))
                {
                    x.Add(this[i].X);
                    y.Add(this[i].Y);
                }
            }
            var mb = MathNet.Numerics.Fit.Line(x.ToArray(), y.ToArray());
            double a = Math.Atan(mb.Item2);
            return a;
        }
        protected Tuple<double, double> GetMinMax()
        {
            double max = double.MinValue;
            double min = double.MaxValue;
            for (int i = 0; i < Count; i++)
            {
                if (this[i].Y > max)
                {
                    max = this[i].Y;
                }
                if (this[i].Y < min)
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
                int yLength = (int)Math.Ceiling(Math.Abs(minmax.Item2 - minmax.Item1) / MeshSize);
                int xLength = Count;
                bitmap = new Bitmap(xLength, yLength);
                Size s = new Size(bitmap.Width, bitmap.Height);

                // Create graphics 
                for (int i = 0; i < xLength; i++)
                {
                    int j = Math.Min(yLength - 1, Math.Max(0, (int)Math.Floor((this[i].Y - minmax.Item1) / MeshSize)));
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
        public  CartData AsCartData()
        {
            var cartData = new DataLib.CartData();
            foreach (var pt in this)
            {
                cartData.Add(new Vector3(pt.X, pt.Y, 0));
            }
            return cartData;
        }
        public List<Vector2> ParseFile(string scanFilename, double minYValue, double maxYValue)
        {
            var stringArr = CSVFileParser.ParseFile(scanFilename);
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
                for (int i = 0; i < Count; i++)
                {

                    string pointStr = this[i].X.ToString() + "," + this[i].Y.ToString();
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
            for (int i = 0; i <= Count - aveWindow; i += aveWindow)
            {
                var x = new List<double>();
                var y = new List<double>();
                for (int j = i; j < i + aveWindow; j++)
                {
                    if (CheckIndex(j))
                    {
                        x.Add(this[j].X);
                        y.Add(this[j].Y);
                    }
                }
                //  var fitFunc = MathNet.Numerics.Fit.LineFunc(x.ToArray(), y.ToArray());
                var fitFunc = MathNet.Numerics.Fit.PolynomialFunc(x.ToArray(), y.ToArray(), 4);

                for (int j = 0; j < x.Count; j++)
                {
                    var yFit = fitFunc(x[j]);
                    smoothProfile.Add(new Vector2(x[j], yFit));
                }
            }
            Clear();
            foreach (var pt in smoothProfile)
            {
                Add(new Vector2(pt.X, pt.Y));
            }
        }
        public void Smooth()
        {
            var smoothProfile = new List<Vector2>();
            smoothProfile.Add(new Vector2(this[0].X, this[0].Y));

            for (int i = 1; i < Count - 1; i++)
            {
                if (((this[i].Y > this[i - 1].Y) && (this[i].Y > this[i + 1].Y))
                    || ((this[i].Y < this[i - 1].Y) && (this[i].Y < this[i + 1].Y)))
                {
                    var ySmoothed = (this[i - 1].Y + this[i + 1].Y) / 2.0;
                    smoothProfile.Add(new Vector2(this[i].X, ySmoothed));
                }
                else
                {
                    smoothProfile.Add(new Vector2(this[i].X, this[i].Y));
                }
            }
            smoothProfile.Add(new Vector2(this[Count - 1].X, this[Count - 1].Y));
            Clear();
            foreach (var pt in smoothProfile)
            {
                Add(new Vector2(pt.X, pt.Y));
            }


        }
        public void SetValue(double value, double radius, double location)
        {
            int j = GetIndex(location);
            int jstart = GetIndex(location - radius);
            int jEnd = GetIndex(location + radius);
            double r = radius / MeshSize;
            double r2 = Math.Pow(r, 2);
            for (int i = jstart; i <= jEnd; i++)
            {
                double y = value;// * Math.Sqrt(Math.Abs(r2 - Math.Pow(i - j, 2)))/r;
                if (CheckIndex(i))
                {
                    this[i].Y = y;
                }
            }

        }
        public void SetValue(double value, double location)
        {
            int j = GetIndex(location);
            if (CheckIndex(j))
                this[j].Y = value;
        }
        public double GetValue(double location)
        {
            int j = GetIndex(location);
            if (CheckIndex(j))
                return this[j].Y;
            else
                return 0;
        }
        public double GetCurvature(double location, double aveWindowWidth)
        {
            int aveWindow = (int)Math.Round(.5 * aveWindowWidth / MeshSize);
            var smoothProfile = new List<Vector2>();

            var x = new List<double>();
            var y = new List<double>();

            int i = GetIndex(location);
            var fitFuncArr = new double[] { 0, 0 };
            int minPointCount = 6;
            int indexStep = (int)(2 * aveWindow / minPointCount);
            if (CheckIndex(i))
            {
                for (int j = i - aveWindow; j < i + aveWindow; j += indexStep)
                {
                    if (CheckIndex(j))
                    {
                        x.Add(this[j].X);
                        y.Add(this[j].Y);
                    }
                }
            }
            fitFuncArr = MathNet.Numerics.Fit.Polynomial(x.ToArray(), y.ToArray(), 2);
            double curvatureFacture = 1;
            double minCurvatureFacture = 1;
            double minCurvature = minCurvatureFacture * (1 / aveWindowWidth);
            double curvature = fitFuncArr[2];
            //if(curvature > minCurvature)
            //{
            //    curvatureFacture = 1 + (Math.Sign(fitFuncArr[2])*(curvature- minCurvature) / minCurvature);
            //}
            return curvature;
        }
       


        void SortByX()
        {
            var xList = new List<double>();
            foreach (var pt in this)
            {
                xList.Add(pt.X);
            }
            var xarr = xList.ToArray();
            var ptArr = this.ToArray();
            Array.Sort(xarr, ptArr);
            this.Clear();
            this.AddRange(ptArr.ToList());
        }
        public void AddProfile(XSection  profile)
        {
            int maxCount = Math.Min(Count, profile.Count);
            if (profile.MeshSize > MeshSize)
            {
                var indexList = new List<int>();
                for (int i = 0; i < profile.Count - 1; i++)
                {
                    double x1 = profile[i].X;
                    double x2 = profile[i + 1].X;
                    double y1 = profile[i].Y;
                    double y2 = profile[i + 1].Y;
                    double m = (y2 - y1) / (x2 - x1);
                    double x = x1;
                    while (x < x2)
                    {
                        int j = GetIndex(x);
                        if (CheckIndex(j) && !indexList.Contains(j))
                        {
                            double y = (x - x1) * m + y1;
                            this[j].Y += y;
                            indexList.Add(j);
                        }
                        x += MeshSize;
                    }
                }
            }
            else
            {
                foreach (var pt in profile)
                {
                    int i = GetIndex(pt.X);
                    if (CheckIndex(i))
                    {
                        this[i].Y += pt.Y;
                    }
                }
            }
        }
        protected void BuildDefProfile()
        {
            double x = Origin.X;
            while (x < Origin.X + Width)
            {
                Add(new Vector2(x, Origin.Y));
                x += MeshSize;
            }
        }
        protected void BuildFromDXF(string dxfFilename)
        {
            Entities = DwgConverterLib.DxfFileParser.Parse(dxfFilename);
            cartDisplayData = DwgConverterLib.DxfFileParser.AsDisplayData(Entities, MeshSize, ViewPlane.XY);
            cartDisplayData.FileName = dxfFilename;
            cylDisplayData = new DisplayData(dxfFilename);
            foreach (var pt in cartDisplayData)
            {
                PointCyl ptc = new PointCyl(new Vector3(pt.X, pt.Y, 0));
                PointF ptf = new PointF((float)ptc.ThetaDeg, (float)ptc.R);
                cylDisplayData.Add(ptf);
            }
            cylDisplayData.FileName = dxfFilename;
        }
        public XSection (Vector2 origin, double width, double meshSize)
        {
            Origin = origin;
            MeshSize = meshSize;
            Width = width;
            BuildDefProfile();
        }
       
        public XSection(string XsectionCsvFile, int firstDataRow, int firstDataColumn)
        {
            double[,] pointArr = CSVFileParser.ToArray(XsectionCsvFile, firstDataRow);

            for (int i = firstDataRow; i < pointArr.GetLength(0); i++)
            {
                double x = pointArr[i, firstDataColumn];
                double y = Math.Abs(pointArr[i, firstDataColumn + 1]);
                Add(new Vector2(x, y));
            }
            SortByX();
            Origin = this[0];
            MeshSize = this[1].X - this[0].X;
            Width = this[Count - 1].X - Origin.X;
        }
        public XSection (XSection  startingProfile)
        {
            Origin = startingProfile.Origin;
            MeshSize = startingProfile.MeshSize;
            Width = startingProfile.Width;
            foreach (var pt in startingProfile)
            {
                Add(new Vector2(pt.X, pt.Y));
            }
            SortByX();
        }
        public XSection(string DXFFilename,double meshSize)
        {
            MeshSize = meshSize;
            BuildFromDXF(DXFFilename);
        }
    }  

}
