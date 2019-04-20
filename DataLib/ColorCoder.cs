using System;
using GeometryLib;


namespace DataLib
{
    public enum COLORCODE
    {
        MONO,
        GREEN_RED,
        RAINBOW,        
    }
    public class ColorCoder
    {
        public static System.Drawing.Color MapMonoColor()
        {
            return  System.Drawing.Color.FromArgb(125, 125, 125);
        }
        public static System.Drawing.Color MapGreenRedColor(double value, double maxValue)
        {
            if (Math.Abs(value) > Math.Abs(maxValue) )
            {
                return System.Drawing.Color.FromArgb(255, 125, 125);
            }
            else
            {
                return System.Drawing.Color.FromArgb(125, 255, 125);
            }

        }
        public static System.Drawing.Color MapRainbowColor(double value, double min_value, double max_value)
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
            return  System.Drawing.Color.FromArgb(red, green, blue);
        }
        public static System.Drawing.Color MapMonoRedColor(double value, double maxToleranceValue)
        {
            if (Math.Abs(value) > Math.Abs(maxToleranceValue))
            {
                return System.Drawing.Color.FromArgb(255, 125, 125);
            }
            else
            {
                return System.Drawing.Color.FromArgb(125, 125, 125);
            }
        }
        
    }
}
