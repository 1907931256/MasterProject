using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryLib;

namespace AbMachModel
{
    public class Abmach2DSurface
    {
        Abmach2DPoint[,] _values;
        BoundingBox _boundingBox;
        double _meshSize;
        int _xSize;
        int _ySize;
        double _xMin;
        double _xMax;
        double _yMin;
        double _yMax;
        double _jetDiameter;
        public BoundingBox BoundingBox { get { return _boundingBox; } }
        public double MeshSize { get { return _meshSize; } }
        public Abmach2DPoint[,] Values { get { return _values; } }
        public double Border { get { return _borderWidth; } }

        public int Xindex(double x)
        {
            return getXIndex(x);
        }
        public int Yindex(double y)
        {
            return getYIndex(y);
        }
        public double XCoordinate(int i)
        {
            double x = i * _meshSize + _boundingBox.Min.X;
            return x;
        }
        public double YCoordinate(int j)
        {
            double y = j * _meshSize + _boundingBox.Min.Y;
            return y;
        }
        public int XSize { get { return _xSize; } }
        public int YSize { get { return _ySize; } }
        public Vector3 GetNormal(int i,int j)
        {         
                   
            try
            {
              
                Vector3 v1 = new Vector3();
                Vector3 v2 = new Vector3();                
                if (i== 0)
                {
                    v1 = new Vector3(_meshSize*2, 0, _values[i + 1, j].Depth - _values[i, j].Depth);
                }
                else if (i == _xSize - 1)
                {
                    v1 = new Vector3(_meshSize*2, 0, _values[i,j].Depth - _values[i - 1,j].Depth);
                }
                else
                {
                    v1 = new Vector3(_meshSize * 2, 0, _values[i + 1, j].Depth - _values[i - 1,j].Depth);
                }

                if (j == 0)
                {

                    v2 = new Vector3(0, _meshSize*2, _values[i, j + 1].Depth - _values[i, j].Depth);
                }
                else if (j == _ySize - 1)
                {

                    v2 = new Vector3(0, _meshSize * 2, _values[i, j].Depth - _values[i, j - 1].Depth);
                }
                else
                {

                    v2 = new Vector3(0, _meshSize * 2, _values[i, j + 1].Depth - _values[i, j - 1].Depth);
                }

                return v1.Cross(v2);               
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool IsPeak(int i,int j)
        {
            try
            {
               
                var ptest = GetDepth(i,j);
                var p1 = GetDepth(i + 1, j);
                var p2 = GetDepth(i - 1, j);
                var p3 = GetDepth(i, j + 1);
                var p4 = GetDepth(i, j - 1);
                return (ptest > p1 && ptest > p2 && ptest > p3 && ptest > p4);                
            }
            catch (Exception)
            {
                throw;
            }

        }

        public bool IsValley(int i,int j)
        {
            try
            {
               
                var ptest = GetDepth(i,j);
                var p1 = GetDepth(i + 1, j);
                var p2 = GetDepth(i - 1, j);
                var p3 = GetDepth(i, j + 1);
                var p4 = GetDepth(i, j - 1);
                return (ptest < p1 && ptest < p2 && ptest < p3 && ptest < p4);
             

            }
            catch (Exception)
            {

                throw;
            }


        }

        public void SmoothAt(int i,int j)
        {
            try
            {
                
                var p1 = GetDepth(i+1,j);
                var p2 = GetDepth(i-1,j);
                var p3 = GetDepth(i,j+1);
                var p4 = GetDepth(i,j-1);
                var pCenter = GetDepth(i,j);
              
                var pAve = (p1 + p2 + p3 + p4) / 4;               
                SetDepth(i,j,pAve);

            }
            catch (Exception)
            {

                throw;
            }

        }
        public Abmach2DSurface Clone()
        {
            try
            {
                var surf = new Abmach2DSurface(_boundingBox, _meshSize, _borderWidth);
                for (int i = 0; i < _xSize; i++)
                {
                    for (int j = 0; j < _ySize; j++)
                    {
                        surf.SetValue(i, j, _values[i, j]);
                    }
                }
                return surf;
            }
            catch (Exception)
            {

                throw;
            }
           
        }
       
        public void SetValue(int i,int j, Abmach2DPoint pt)
        {
           
            _values[i, j].JetHit = pt.JetHit;
            _values[i, j].Normal = pt.Normal;
            _values[i, j].StartDepth = pt.StartDepth;
            _values[i, j].TargetDepth = pt.TargetDepth;
            _values[i, j].Depth = pt.Depth;
            _values[i, j].DepthTolerance = pt.DepthTolerance;
        }
        
        public Abmach2DPoint GetValue(int i,int j)
        {            
            return _values[i, j];
        }
       
        public double GetDepth(int i,int j)
        {           
            return _values[i, j].Depth;
        }
        public double GetDepthTolerance(int i,int j)
        {
            return _values[i, j].DepthTolerance;
        }
        public double GetStartDepth(int i,int j)
        {            
            return _values[i, j].StartDepth;
        }
        public double GetTargetDepth(int i,int j)
        {          
            return _values[i, j].TargetDepth;
        }

        public void SetDepth(int i,int j, double value)
        {           
            _values[i, j].Depth = value;
            _values[i, j].JetHit = true;
        }
        public void SetStartDepth(int i,int j, double value)
        {         
            _values[i, j].StartDepth = value;
        }
        public void SetTargetDepth(int i,int j, double value)
        {
           
            _values[i, j].TargetDepth = value;
        }
        public void SetDepthTolerance(int i,int j, double value)
        {
            _values[i, j].DepthTolerance = value;
        }
        int getXIndex(double x)
        {
            int i = (int)Math.Round((x - _xMin) / _meshSize, 0);// + _matrixBorderWidth;
            return i >= 0 && i < _xSize ? i : 0;
        }
        
        int getYIndex(double y)
        {
            int j = (int)Math.Round((y - _yMin) / _meshSize, 0);// + _matrixBorderWidth;
            return j >= 0 && j < _ySize ? j : 0;
        }
        int checkXIndex(int i)
        {
           return i >= 0 && i < _xSize ? i : 0;
        }
        int checkYIndex(int j)
        {
            return j >= 0 && j < _ySize ? j : 0;
        }
        double _borderWidth;
        int _matrixBorderWidth; 
        void buildSurface(BoundingBox boundingBox, double meshSize, double border)
        {
            _meshSize = meshSize;
            _borderWidth = border;
            _matrixBorderWidth = (int)Math.Ceiling(_borderWidth / _meshSize);

            _xMin = boundingBox.Min.X - _borderWidth;
            _xMax = boundingBox.Max.X + _borderWidth;
            _yMin = boundingBox.Min.Y - _borderWidth;
            _yMax = boundingBox.Max.Y + _borderWidth;
            _boundingBox = new BoundingBox(_xMin, _yMin, boundingBox.Min.Z, _xMax, _yMax, boundingBox.Max.Z);
            _xSize = (int)Math.Ceiling(boundingBox.Size.X / _meshSize) + 2 * _matrixBorderWidth;
            _ySize = (int)Math.Ceiling(boundingBox.Size.Y / _meshSize) + 2 * _matrixBorderWidth;
            _values = new Abmach2DPoint[_xSize, _ySize];
           
        }
        void initValues()
        {
            for (int i = 0; i < _xSize; i++)
            {
                for (int j = 0; j < _ySize; j++)
                {
                    _values[i, j] = new Abmach2DPoint();                    
                }
            }
        }
        void initValues(double targetValue)
        {
            for (int i = 0; i < _xSize; i++)
            {
                for (int j = 0; j < _ySize; j++)
                {
                    _values[i, j] = new Abmach2DPoint();
                    _values[i, j].TargetDepth = targetValue;
                }
            }
        }
        void initValues(double targetValue,double startValue)
        {
            for (int i = 0; i < _xSize; i++)
            {
                for (int j = 0; j < _ySize; j++)
                {
                    _values[i, j] = new Abmach2DPoint();
                    _values[i, j].TargetDepth = targetValue;
                    _values[i, j].StartDepth = startValue;
                }
            }
        }
        public Abmach2DSurface(BoundingBox boundingBox, double meshSize, double border, double targetValue,double startValue)
        {
            buildSurface(boundingBox, meshSize, border);
            initValues(targetValue, startValue);
        }
        public Abmach2DSurface( BoundingBox boundingBox,double meshSize,double border, double targetValue)
        {
            buildSurface(boundingBox, meshSize, border);
            initValues(targetValue);
        }
        public Abmach2DSurface(BoundingBox boundingBox, double meshSize, double border)
        {
            buildSurface(boundingBox, meshSize, border);
            initValues();
        }
    }
}
