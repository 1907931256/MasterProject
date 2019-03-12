using System;
using GeometryLib;


namespace DataLib
{
    public enum COLORCODE
    {
        MONO,
        GREEN_RED,
        RAINBOW,
        MONO_RED,
        CONTOURS
    }
    public class ColorCoder
    {
        public static RGBColor MapMonoColor()
        {
            return new RGBColor(125, 125, 125, 255);
        }
        public static RGBColor MapGreenRedColor(double value,double minValue, double maxValue)
        {
            if (Math.Abs(value)<Math.Abs(minValue) || (Math.Abs(value) > Math.Abs(maxValue) ))
            {
                return new RGBColor(255, 125, 125, 255);
            }
            else
            {
                return new RGBColor(125, 255, 125, 255);
            }

        }
        public static RGBColor MapRainbowColor(double value, double min_value, double max_value)
        {
            // Convert into a value between 0 and 1023.
            int int_value = (int)(1023 * (value - min_value) / (max_value - min_value));
            byte red = 100;
            byte green = 100;
            byte blue = 100;
            // Map different color bands.
            if (int_value < 256)
            {
                // Red to yellow. (255, 0, 0) to (255, 255, 0).
                red = 255;
                green = (byte)int_value;
                blue = 0;
            }
            else if (int_value < 512)
            {
                // Yellow to green. (255, 255, 0) to (0, 255, 0).
                int_value -= 256;
                red = (byte)(255 - int_value);
                green = 255;
                blue = 0;
            }
            else if (int_value < 768)
            {
                // Green to aqua. (0, 255, 0) to (0, 255, 255).
                int_value -= 512;
                red = 0;
                green = 255;
                blue = (byte)int_value;
            }
            else
            {
                // Aqua to blue. (0, 255, 255) to (0, 0, 255).
                int_value -= 768;
                red = 0;
                green = (byte)(255 - int_value);
                blue = 255;
            }
            return new RGBColor(red, green, blue, 255);
        }
        public static RGBColor MapMonoRedColor(double value, double maxToleranceValue)
        {
            if (Math.Abs(value) > Math.Abs(maxToleranceValue))
            {
                return new RGBColor(255, 125, 125, 255);
            }
            else
            {
                return new RGBColor(125, 125, 125, 255);
            }
        }
        public static RGBColor MapContours(double value, double min_value, double max_value, double[] contours)
        {
            RGBColor c = new RGBColor();
            int int_value = (int)(1000 * (value - min_value) / (max_value - min_value));
            if (int_value % 100 < 5)
            {
                c = new RGBColor(0, 0, 0, 255);
            }
            else
            {
                c = new RGBColor(125, 255, 125, 255);
            }
            return c;
        }
    }
}
