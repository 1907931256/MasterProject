using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryLib
{
       
      
     
    public class ColorConverter
    {/// <summary>
    /// Convert to RGB Color from DXF color
    /// </summary>
    /// <param name="c">DXF Color 0-7</param>
    /// <returns></returns>
        public static System.Drawing.Color ToColor(int c)
        {
            System.Drawing.Color color = new System.Drawing.Color();
            switch (c)
            {
                case 0://Black = 0,
                    color = System.Drawing.Color.Black;
                    break;
                case 1://      Red = 1,
                    color = System.Drawing.Color.Red;
                    break;
                case 2: //      Yellow = 2,
                    color = System.Drawing.Color.Yellow;
                    break;
                case 3://      Green = 3,
                    color = System.Drawing.Color.Green;
                    break;
                case 4://      Cyan = 4,
                    color = System.Drawing.Color.Cyan;
                    break;
                case 5://      Blue = 5,
                    color = System.Drawing.Color.Blue;
                    break;
                case 6://      Magenta = 6,
                    color = System.Drawing.Color.Magenta;
                    break;
                case 7://      White = 7,
                    color = System.Drawing.Color.White;
                    break;
                case 8://      Grey = 8,   
                default:
                    color = System.Drawing.Color.Gray;
                    break;
            }
            return color;            
        }
        public static System.Drawing.Color ToColor(byte red,byte green, byte blue)
        {
            return System.Drawing.Color.FromArgb(red, green, blue);
        }       
       
    }
}
