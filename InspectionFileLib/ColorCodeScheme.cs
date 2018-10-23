using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryLib;
namespace InspectionLib
{
   public enum ColorCodeOptions
    {
        GROOVEHIGHLIGHT,
        ABSRADIALERROR,
        RELRADIALERROR,
        NONE

    }
    public abstract class ColorScale<T>
    {
        public abstract RGBColor GetColor(T value);
    }
    public class HighLightColorValue: ColorScale<bool>
    {
        protected RGBColor _colorTrue;
        protected RGBColor _colorFalse;

       
        public RGBColor ColorTrue
        {
            get
            {
                return _colorTrue;
            }
        }
        public RGBColor ColorFalse
        {
            get
            {
                return _colorFalse;
            }
        }
        override public  RGBColor GetColor(bool value)
        {
            if(value  )
            {
                return _colorTrue;
            }
            else
            {
                return _colorFalse;
            }

        }
    }

    public class ColorScaleValue:ColorScale<double>
    {
        public double LowerLimit
        {
            get
            {
                return _lower;
            }
        }
        public double UpperLimit
        {
            get
            {
                return _upper;
            }

        }
        public RGBColor UpperColor
        {
            get
            {
                return _upperColor;
            }
        }
        public RGBColor LowerColor
        {
            get
            {
                return _lowerColor;
            }
        }
        //public bool Blend
        //{
        //    get
        //    {
        //        return _blend;
        //    }
        //}
        protected double _lower;
        protected double _upper;
       
        RGBColor _upperColor;
        RGBColor _lowerColor;
       // bool _blend;
        int dRed;
        int dGreen;
        int dBlue;
        double dValue;
        bool blend;
        override public RGBColor GetColor(double value)
        {
            var c = new RGBColor();
            if(value>= _lower && value < _upper)
            {
                if(blend)
                {
                    double dv = (value - _lower) / dValue;
                    double r = (dv * dRed) + _lowerColor.Red;
                    double g = (dv * dGreen) + _lowerColor.Green;
                    double b = (dv * dBlue) + _lowerColor.Blue;
                    c = new RGBColor(r, g, b);
                }
                else
                {
                    c = _lowerColor;
                }
            }
            return c;
        }
        public ColorScaleValue(double lower, double upper, RGBColor lowerColor, RGBColor upperColor)
        {
            _lower = lower;
            _upper = upper;
            if(_upper == _lower )
            {
                _upper = _lower * 1.01;
            }
            _lowerColor = lowerColor;
            _upperColor = upperColor;
            dValue = _upper - _lower;
            dRed = (int)(_upperColor.Red - _lowerColor.Red);
            dGreen = (int)(_upperColor.Green - _lowerColor.Green);
            dBlue = (int)(_upperColor.Blue - _lowerColor.Blue);
            blend = true;
        }
        public ColorScaleValue(double lower,double upper, RGBColor color)
        {
            _lower = lower;
            _upper = upper;
            if (_upper == _lower)
            {
                _upper = _lower * 1.01;
            }
            _lowerColor = color;
            _upperColor = color;
            blend = false;
        }

    }
    public abstract class ColorCoder<T>
    {
        protected ColorCodeOptions _colorOptions;
        public abstract RGBColor GetColor(T value);
    }
    public class HighLightColorCoder:ColorCoder<bool>
    {
        HighLightColorValue _colorValue;
        override public RGBColor GetColor(bool value)
        {
            var c = new RGBColor();
            if(value)
            {
                c = _colorValue.ColorTrue;
            }
            else
            {
                c = _colorValue.ColorFalse;
            }
            return c;
        }
        public HighLightColorCoder(HighLightColorValue colorValue, ColorCodeOptions options)
        {
            _colorValue = colorValue;
            _colorOptions = options;
        }
    }
    public class ScaleColorCoder:ColorCoder<double>
    {
        List<ColorScaleValue> _colorScale;
        
        override public RGBColor GetColor(double value)
        {
            var c = new RGBColor();
            foreach(ColorScaleValue csval in _colorScale)
            {
                if(value<csval.UpperLimit && value>= csval.LowerLimit)
                {
                    c = csval.GetColor(value);
                    break;
                }
            }
            return c;
        }
        public ScaleColorCoder(List<ColorScaleValue>colorScale, ColorCodeOptions options)
        {
            _colorScale = colorScale;
            _colorOptions = options;

        }
    }


}
