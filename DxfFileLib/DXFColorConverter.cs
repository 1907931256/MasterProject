using GeometryLib;

namespace DwgConverterLib
{
    public class DXFColorConverter:ColorConverter
    {
        public static DxfColor ToDxfColor(System.Drawing.Color c)
        {
            DxfColor dxfC = DxfColor.Cyan;
            
            if (c.R == 0 & c.G == 0 & c.B == 0)
                dxfC = DxfColor.Black;
            else if (c.R != 0 & c.G == 0 & c.B == 0)
                dxfC = DxfColor.Red;
             else if (c.R == 0 & c.G != 0 & c.B == 0)
                dxfC = DxfColor.Green;
             else if (c.R == 0 & c.G == 0 & c.B != 0)
                dxfC = DxfColor.Blue;           
             else if (c.R != 0 & c.G == 0 & c.B != 0)
                dxfC = DxfColor.Magenta;            
             else if (c.R != 0 & c.G != 0 & c.B == 0)
                dxfC = DxfColor.Yellow;
             else if (c.R == 0 & c.G != 0 & c.B != 0)
                dxfC = DxfColor.Cyan;
             else 
                dxfC = DxfColor.Grey;
            return dxfC;
        }
        
        public static DxfColor ToDxfColor(int c)
        {
            DxfColor dxfC = DxfColor.White;
            switch (c)
            {
                case 0:
                    dxfC = DxfColor.Black;
                    break;
                case 1:
                    dxfC = DxfColor.Red;
                    break;
                case 2:
                    dxfC = DxfColor.Yellow;
                    break;
                case 3:
                    dxfC = DxfColor.Green;
                    break;
                case 4:
                    dxfC = DxfColor.Cyan;
                    break;
                case 5:
                    dxfC= DxfColor.Blue;
                    break;
                case 6:
                    dxfC= DxfColor.Magenta;
                    break;               
                case 8:                
                    dxfC= DxfColor.Grey;
                    break;
                case 7:
                default:    
                    dxfC =DxfColor.White;
                    break;
            }
            return dxfC;
        }
   
    }
}
