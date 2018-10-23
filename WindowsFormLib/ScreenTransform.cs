using System;
using System.Drawing;
using GeometryLib;

namespace WinFormsLib
{
    public class ScreenTransform
    {
        BoundingBox _mRect;
        double _xScale;
        double _yScale;
        RectangleF _scrRect;
       

        public bool StretchToFit { get { return _stretch; } }
        public BoundingBox Model { get { return _mRect; } }
        public RectangleF Screen { get { return _scrRect; } }

        public PointF GetModelCoords(Point screenPt)
        {
            float x = (float)((screenPt.X - _xScrnPad) / _xScale + _mRect.Min.X);
            float y = (float)((_scrRect.Height - screenPt.Y - _yScrnPad) / _yScale + _mRect.Min.Y);
            var ptf = new PointF(x, y);
            return ptf;
        }
        public PointF GetModelCoords(PointF screenPt)
        {
            float x = (float)((screenPt.X - _xScrnPad) / _xScale + _mRect.Min.X);
            float y = (float)((_scrRect.Height - screenPt.Y - _yScrnPad) / _yScale + _mRect.Min.Y);
            var ptf = new PointF(x, y);
            return ptf;
        }
        public PointF GetScreenCoords(double x, double y)
        {
            float xScreen = (float)((x - _mRect.Min.X) * _xScale);
            float yScreen = (float)(_scrRect.Height - ((y - _mRect.Min.Y) * _yScale));
            var ptScreen = new PointF(xScreen+_xScrnPad, yScreen-_yScrnPad);
            return ptScreen;
        }
        void CalcTransform()
        {
            if (_modelRect != null)
            {
                _mRect = _modelRect;
            }
            else
            {
                _mRect = new BoundingBox(0, 0, 0, 1, 1, 0);
            }
            if (_screenRect != null)
            {
                _scrRect = _screenRect;
            }
            else
            {
                _scrRect = new RectangleF(0, 0, 1, 1);
            }

            
            double xtemp = (float)(_scrRect.Width / _mRect.Size.X);
            double ytemp = (float)(_scrRect.Height / _mRect.Size.Y);
            if (_stretch)
            {
                _xScale = xtemp;
                _yScale = ytemp;
            }
            else
            {
                _xScale = Math.Min(xtemp, ytemp);
                _yScale = _xScale;
            }
            _xScale *= _padFraction;
            _yScale *= _padFraction;
            _xScrnPad = (float)(((1.0-_padFraction) * _screenRect.Width) / 2.0);
            _yScrnPad = (float)(((1.0-_padFraction) * _screenRect.Height) / 2.0);

        }
        BoundingBox _modelRect;
        RectangleF _screenRect;
        double _padFraction;
        
        bool _stretch;
        float _xScrnPad;
        float _yScrnPad;

        public ScreenTransform(BoundingBox modelRect, RectangleF screenRect,double padFractionSize, bool strechToFit)
        {
            _modelRect = modelRect;
            _screenRect = screenRect;           
            _stretch = strechToFit;
            _padFraction = padFractionSize;
            CalcTransform();
        }
        public ScreenTransform(BoundingBox modelRect, RectangleF screenRect, bool stretchToFit)
        {
            _modelRect = modelRect;
            _screenRect = screenRect;            
            _stretch = stretchToFit;
            _padFraction = 1.0;
            CalcTransform();

        }
    }
}
