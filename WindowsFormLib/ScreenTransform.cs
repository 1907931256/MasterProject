using System;
using System.Drawing;
using GeometryLib;
using System.Collections.Generic;
namespace WinFormsLib
{
    
    public class ScreenTransform
    {
        RectangleF _mRect;
        double _xScale;
        double _yScale;
        RectangleF _scrRect;       
        public RectangleF ScreenRectangle { get { return _scrRect; } }
        public RectangleF ModelRectangle { get { return _mRect; } }
        public RectangleF BoundingRectangle { get { return new RectangleF(new PointF(_mRect.Left, _mRect.Bottom), _mRect.Size); } }
        public PointF GetModelCoords(Point screenPt)
        {
            float x = (float)((screenPt.X - _xScrnPad) / _xScale + _mRect.Left);
            float y = (float)((_scrRect.Height - screenPt.Y - _yScrnPad) / _yScale + _mRect.Top);
            var ptf = new PointF(x, y);
            return ptf;
        }
        public PointF GetModelCoords(PointF screenPt)
        {
            float x = (float)((screenPt.X - _xScrnPad) / _xScale + _mRect.Left);
            float y = (float)((_scrRect.Height - screenPt.Y - _yScrnPad) / _yScale + _mRect.Top);
            var ptf = new PointF(x, y);
            return ptf;
        }
        public PointF GetScreenCoords(double x, double y)
        {
            float xScreen = (float)((x - _mRect.Left) * _xScale);
            float yScreen = (float)(_scrRect.Height - ((y - _mRect.Top) * _yScale));
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
                _mRect = new RectangleF(0, 0, 1, 1);
            }
            if (_screenRect != null)
            {
                _scrRect = _screenRect;
            }
            else
            {
                _scrRect = new RectangleF(0, 0, 1, 1);
            }

            double xtemp = 1;           
            double ytemp = 1;           
            xtemp= (float)(_scrRect.Width / _mRect.Width);
            ytemp= (float)(_scrRect.Height / _mRect.Height);       

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

            _xScale *= 1-2*_borderFraction;
            _yScale *= 1-2*_borderFraction;
            _xScrnPad = (float)(_borderFraction * _screenRect.Width );
            _yScrnPad = (float)(_borderFraction * _screenRect.Height );

        }

        RectangleF _modelRect;
        RectangleF _screenRect;
        double _borderFraction;
        
        bool _stretch;
        float _xScrnPad;
        float _yScrnPad;

       
        public ScreenTransform(RectangleF modelRect, RectangleF screenRect,double borderFraction, bool strechToFit)
        {
            _modelRect = modelRect;
            _screenRect = screenRect;           
            _stretch = strechToFit;
            _borderFraction = borderFraction;
            CalcTransform();
        }
        public ScreenTransform(RectangleF modelRect, RectangleF screenRect, bool stretchToFit)
        {
            _modelRect = modelRect;
            _screenRect = screenRect;            
            _stretch = stretchToFit;
            _borderFraction = 0.0;
            CalcTransform();

        }
    }
}
