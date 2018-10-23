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
        public static RGBColor ToColor(int c)
        {
            RGBColor color = new RGBColor();
            switch (c)
            {
                case 0://Black = 0,
                    color.Red = 0;
                    color.Green  = 0;
                    color.Blue = 0;
                    break;
                case 1://      Red = 1,
                    color.Red = 250;
                    color.Green = 0;
                    color.Blue = 0;
                    break;
                case 2: //      Yellow = 2,
                    color.Red = 250;
                    color.Green = 250;
                    color.Blue = 0;
                    break;
                case 3://      Green = 3,
                    color.Red = 0;
                    color.Green = 250;
                    color.Blue = 0;
                    break;
                case 4://      Cyan = 4,
                    color.Red = 0;
                    color.Green = 250;
                    color.Blue = 250;
                    break;
                case 5://      Blue = 5,
                    color.Red = 0;
                    color.Green = 0;
                    color.Blue = 250;
                    break;
                case 6://      Magenta = 6,
                    color.Red = 240;
                    color.Green = 0;
                    color.Blue = 240;
                    break;
                case 7://      White = 7,
                    color.Red = 250;
                    color.Green = 250;
                    color.Blue = 250;
                    break;
                case 8://      Grey = 8,   
                default: 
                    color.Red = 127;
                    color.Green = 127;
                    color.Blue = 127;
                    break;
            }
            return color;            
        }
        public static RGBColor ToColor(uint red,uint green, uint blue)
        {
            return new RGBColor(red, green, blue);
        }       
       
    }
}
