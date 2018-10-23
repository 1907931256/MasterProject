using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeometryLib;
namespace SurfaceModel
{
 
     public class Surface2D<T> : ISurface<T> where T : SurfacePoint, new()
    {
        protected T[,] _values;
        protected double _meshSize;
        protected int _xSize;
        protected int _ySize;
        protected double _initValue;
        protected BoundingBox _boundingBox;
        public Double InitValue { get { return _initValue; } set { _initValue = value; } }
        public SurfaceType Type { get; }
        public BoundingBox BoundingBox { get { return _boundingBox; } }
        public Vector3 Min { get { return _boundingBox.Min; } }
        public Vector3 Max { get { return _boundingBox.Max; } }
        public Vector3 Size { get { return _boundingBox.Size; } }
        public double MeshSize { get { return _meshSize; } }
        public int XSize { get { return _xSize; } }
        public int YSize { get { return _ySize; } }
       
        
        public void SetValue(T value,int i,int j)
        {
            try
            {
                if (i >= 0 && i < _xSize && j >= 0 && j < _ySize)
                {                    
                    _values[i, j] = value;
                }
            }
            catch (Exception)
            {

                throw;
            }      
        }
        public void SetValue(T value, Vector3 pt)
        {
            int i = Xindex(pt.X);
            int j = Yindex(pt.Y);
            SetValue(value, i, j);

        }
        public void InitValues(T value)
        {
            try
            {
                for (int i = 0; i < _xSize; i++)
                {
                    for (int j = 0; j < _ySize; j++)
                    {
                        SetValue(value, i, j);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
           
        }
        public T GetPointAt(int i, int j)
        {
            try
            {
                if (i < _xSize && j < _ySize && i >= 0 && j >= 0)
                    return _values[i, j];
                else
                    return new T();
            }
            catch (Exception)
            {

                throw;
            }
          
        }
        public T GetPointAt(Vector3 pt)
        {
            int i = Xindex(pt.X);
            int j = Yindex(pt.Y);
            return GetPointAt(i, j);

        }
        public double Xposition(int xIndex)
        {
            return xIndex * _meshSize + _boundingBox.Min.X;
        }
        public double Yposition(int yIndex)
        {
            return yIndex * _meshSize + _boundingBox.Min.Y;
        }
        public int Xindex(double val)
        {
            try
            {
                int index = -1;
                index = (int)Math.Round((_xSize-1) * (val - _boundingBox.Min.X) / (_boundingBox.Max.X - _boundingBox.Min.X));
                if (index >= _xSize) index = _xSize - 1;
                if (index < 0) index = 0;
                return index;
            }
            catch (Exception)
            {

                throw;
            }
           
        }
        public int Yindex(double val)
        {
            try
            {
                int index = -1;
                index = (int)Math.Round((_ySize-1) * (val - _boundingBox.Min.Y) / (_boundingBox.Max.Y - _boundingBox.Min.Y));
                if (index >= _ySize) index =_ySize - 1;
                if (index < 0) index = 0;

                return index;
            }
            catch (Exception)
            {

                throw;
            }
           
        }
        public Vector3 GetNormalAt(int xI,int yI)
        {
            try
            {
                Vector3 v1 = new Vector3();
                Vector3 v2 = new Vector3();
                if (xI == 0)
                {
                    v1 = new Vector3(_values[xI + 1, yI].Position.X - _values[xI, yI].Position.X, 0, _values[xI + 1, yI].Position.Z - _values[xI, yI].Position.Z);
                }
                else if (xI == _xSize - 1)
                {
                    v1 = new Vector3(_values[xI, yI].Position.X - _values[xI - 1, yI].Position.X, 0, _values[xI, yI].Position.Z - _values[xI - 1, yI].Position.Z);
                }
                else
                {
                    v1 = new Vector3(_values[xI + 1, yI].Position.X - _values[xI - 1, yI].Position.X, 0, _values[xI + 1, yI].Position.Z - _values[xI - 1, yI].Position.Z);
                }

                if (yI == 0)
                {

                    v2 = new Vector3(0, _values[xI, yI + 1].Position.Y - _values[xI, yI].Position.Y, _values[xI, yI + 1].Position.Z - _values[xI, yI].Position.Z);
                }
                else if (yI == _ySize - 1)
                {

                    v2 = new Vector3(0, _values[xI, yI].Position.Y - _values[xI, yI - 1].Position.Y, _values[xI, yI].Position.Z - _values[xI, yI - 1].Position.Z);
                }
                else
                {

                    v2 = new Vector3(0, _values[xI, yI + 1].Position.Y - _values[xI, yI - 1].Position.Y, _values[xI, yI + 1].Position.Z - _values[xI, yI - 1].Position.Z);
                }


                Vector3 n = v1.Cross(v2);
                n.Normalize();
                return n;
            }
            catch (Exception)
            {

                throw;
            }
           
        }
        public Vector3 GetNormalAt(Vector3 pt)
        {
            try
            {
                int xI = Xindex(pt.X);
                int yI = Yindex(pt.Y);
                return GetNormalAt(xI, yI);
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        //public List<int> GetPtIndicesInBox(BoundingBox box)
        //{
        //    try
        //    {
        //        var results = new List<int>();
        //        int ximin = Xindex(box.Min.X);
        //        int yimin = Xindex(box.Min.Y);
        //        int ximax = Xindex(box.Max.X);
        //        int yimax = Xindex(box.Max.Y);
        //        for (int i = ximin; i <= ximax; i++)
        //        {
        //            for (int j = yimin; j <= yimax; j++)
        //            {
        //                results.Add(_values[i, j].Index);
        //            }
        //        }
        //        return results;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
           
        //}
    
        public List<T> GetPointsInsideBox(BoundingBox box)
        {
            try
            {
                var results = new List<T>();
                int xMin = Xindex(box.Min.X);
                int yMin = Yindex(box.Min.Y);
                int xMax = Xindex(box.Max.X);
                int yMax = Yindex(box.Max.Y);
                for (int xi = xMin; xi <= xMax; xi++)
                {
                    for (int yi = yMin; yi <= yMax; yi++)
                    {
                        results.Add(_values[xi, yi]);
                    }
                }

                return results;
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        public List<T> GetAllPoints()
        {
            List<T> pointList = new List<T>();
            for (int i = 0; i < _xSize; i++)
            {
                for (int j = 0; j < _ySize; j++)
                {
                    pointList.Add(_values[i, j]);
                }
            }
            return pointList;
        }
        public List<T> CutPointsInsideBox(BoundingBox box)
        {
            try
            {
                var results = new List<T>();
                int xMin = Xindex(box.Min.X);
                int yMin = Yindex(box.Min.Y);
                int xMax = Xindex(box.Max.X);
                int yMax = Yindex(box.Max.Y);
                for (int xi = xMin; xi <= xMax; xi++)
                {
                    for (int yi = yMin; yi < yMax; yi++)
                    {
                        results.Add(_values[xi, yi]);
                        _values[xi, yi] = null;
                    }
                }

                return results;
            }
            catch (Exception)
            {

                throw;
            }
            

        }
        public bool IsPeak(int i, int j)
        {
            var ptest = GetPointAt(i, j).Position.Z;
            var p1 = GetPointAt(i + 1, j).Position.Z;
            var p2 = GetPointAt(i - 1, j).Position.Z;
            var p3 = GetPointAt(i, j + 1).Position.Z;
            var p4 = GetPointAt(i, j - 1).Position.Z;
            if(ptest>p1 && ptest>p2 && ptest>p3 && ptest>p4)
            {
                return true;
            }
            return false;
        }
        public bool IsValley(int i, int j)
        {

            var ptest = GetPointAt(i, j).Position.Z;
            var p1 = GetPointAt(i + 1, j).Position.Z;
            var p2 = GetPointAt(i - 1, j).Position.Z;
            var p3 = GetPointAt(i, j + 1).Position.Z;
            var p4 = GetPointAt(i, j - 1).Position.Z;
            if (ptest < p1 && ptest < p2 && ptest < p3 && ptest < p4)
            {
                return true;
            }
            return false;
        }
        public void SmoothAt(int i,int j)
        {
            var p1 = GetPointAt(i + 1, j).Position.Z;
            var p2 = GetPointAt(i - 1, j).Position.Z;
            var p3 = GetPointAt(i, j + 1).Position.Z;
            var p4 = GetPointAt(i, j - 1).Position.Z;
            var pCenter = GetPointAt(i, j);
            //var pAve = (p1 + p2 + p3 + p4 + pCenter.Position.Z) / 5;
            var pAve = (p1 + p2 + p3 + p4 ) / 4;
            var position = new Vector3(pCenter.Position.X, pCenter.Position.Y, pAve);            

            pCenter.Position = position;
            SetValue(pCenter, i, j);
        }
        public ISurface<T> Clone()
        {
            var surface =  Surface2DBuilder<T>.Build(_boundingBox, _meshSize);
            foreach(T pt in _values)
            {
                surface.SetValue(pt, pt.Position);
            }
            return surface;
        }
        public void Insert(List<T> points)
        {
            foreach(T point in points)
            {
                Insert(point);
            }
        }
        public void Insert(T point)
        {
            try
            {
                int xI = Xindex(point.Position.X);
                int yI = Yindex(point.Position.Y);
                SetValue(point, xI, yI);
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        public IntersectionRecord GetIntersection(Ray ray)
        {
            var ir = new IntersectionRecord(ray.Origin, false);
            if (_boundingBox.Contains(ray.Origin))
                ir.Intersects = true;
            return ir;
        }
        private void calcSize()
        {
            try
            {
                _xSize = (int)(Math.Ceiling((_boundingBox.Max.X - _boundingBox.Min.X) / _meshSize))+1;
                _ySize = (int)(Math.Ceiling((_boundingBox.Max.Y - _boundingBox.Min.Y) / _meshSize))+1;
                _boundingBox.Max.X = _xSize * _meshSize + _boundingBox.Min.X;
                _boundingBox.Max.Y = YSize * _meshSize + _boundingBox.Min.Y;
            }
            catch (Exception)
            {
                throw;
            }
        }       
        private void fillArray(double initialZ)
        {
            try
            {
                double x = _boundingBox.Min.X;
                double y = _boundingBox.Min.Y;
                double z = 0;
                _values = new T[_xSize, _ySize];
                for (int i = 0; i < _xSize; i++)
                {
                    x = i * _meshSize + _boundingBox.Min.X;
                    for (int j = 0; j < _ySize; j++)
                    {
                        _values[i, j] = new T();
                        y = j * _meshSize + _boundingBox.Min.Y;                        
                        _values[i, j].Normal = new Vector3(0, 0, 1);
                        _values[i, j].Position = new Vector3(x, y, z);                        
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
           
        }

        public Surface2D(BoundingBox _boundingBoxIn,double meshSizeIn)
        {
            Type = SurfaceType.Array2D;
            _meshSize = meshSizeIn;
            _boundingBox = _boundingBoxIn;
            calcSize();
        }
    }
}
