using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryLib
{
    /// <summary>
    /// contains color RGB values
    /// </summary>
    public class RGBColor
    {
        public byte Red { get; set; }
        public byte Green { get; set; }
        public byte Blue { get; set; }
        public byte Alpha { get; set; }
        /// <summary>
        /// new color set to white
        /// </summary>
        public RGBColor()
        {
            Red = 255;
            Green = 255;
            Blue = 255;
            Alpha = 255;
        }
        /// <summary>
        /// new color
        /// </summary>
        /// <param name="red">Red component 0-255</param>
        /// <param name="green">Green component 0-255</param>
        /// <param name="blue">Blue component 0-255</param>
        
        public RGBColor(byte red, byte green, byte blue,byte alpha)
        {
            Red = red;
            Green = green;
            Blue = blue;
            Alpha = alpha;
        }
        public RGBColor(float red, float green, float blue)
        {
            Red = (byte)(255 * Math.Abs(red));
            Green = (byte)(255 *Math.Abs(green));
            Blue = (byte)(255 *Math.Abs(blue));
            Alpha = 255;
        }
        public RGBColor(double red, double green, double blue)
        {
            Red = (byte)(255 * Math.Abs(red));
            Green = (byte)(255 * Math.Abs(green));
            Blue = (byte)(255 * Math.Abs(blue));
            Alpha = 255;
        }
    }
}
