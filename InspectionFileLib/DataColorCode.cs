using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryLib;
using System.Diagnostics;
using DataLib;
using BarrelLib;
namespace InspectionLib
{
    public class DataColorCode
    {
        static public CylGridData ColorCodeData(Barrel barrel, DataOutputOptions options, CylGridData correctedRingList, COLORCODE colorOption)
        {
            try
            {
                var resultGrid = new CylGridData();
                foreach (var cylstrip in correctedRingList)
                {
                    var resultStrip = new CylData(correctedRingList[0].FileName);
                    foreach (PointCyl pt in cylstrip)
                    {
                        var c = new RGBColor(1f, 1f, 1f);
                        switch (colorOption)
                        {
                            case COLORCODE.GREEN_RED:
                                c = MapGreenRedColor(barrel, pt);
                                break;
                            case COLORCODE.RAINBOW:
                                c = MapRainbowColor(barrel, pt);
                                break;
                            case COLORCODE.MONO_RED:
                                c = ColorCoder.MapMonoColor();
                                break;
                        }
                        pt.Col = c;
                        resultStrip.Add(new PointCyl(pt.R, pt.ThetaRad, pt.Z, c, pt.ID));
                    }
                    resultGrid.Add(resultStrip);
                }
                return resultGrid;
            }
            catch (Exception)
            {
                Debug.WriteLine(" exception in colorCodeData");
                throw;
            }
        }
        static RGBColor MapGreenRedColor(Barrel barrel, PointCyl pt)
        {
            return ColorCoder.MapGreenRedColor(pt.R,  barrel.MaxRadius(pt.Z, pt.ThetaRad));
        }
        
        static RGBColor MapRainbowColor(Barrel barrel, PointCyl pt)
        {
            return ColorCoder.MapRainbowColor(pt.R, barrel.MinRadius(pt.Z, pt.ThetaRad), barrel.MaxRadius(pt.Z, pt.ThetaRad));            
        }

        static RGBColor ColorbyGroove(Barrel barrel, PointCyl pt)
        {
            RGBColor c;
            int gn = barrel.GetGrooveNumber(pt.Z, pt.ThetaRad);
            if (gn >= 0)
            {
                c = new RGBColor(0.0f, 1.0f, 0.0f);
            }
            else
            {
                c = new RGBColor(0.0f, 0.0f, 1.0f);
            }

            return c;
        }

    }
}
