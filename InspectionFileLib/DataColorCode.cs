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
                        var c = System.Drawing.Color.LightGray;
                        switch (colorOption)
                        {
                            case COLORCODE.GREEN_RED:
                                c = MapGreenRedColor(barrel, pt);
                                break;
                            case COLORCODE.RAINBOW:
                                c = MapRainbowColor(barrel, pt);
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
        static System.Drawing.Color MapGreenRedColor(Barrel barrel, PointCyl pt)
        {
            return ColorCoder.MapGreenRedColor(pt.R,  barrel.MaxRadius(pt.Z, pt.ThetaRad));
        }
        
        static System.Drawing.Color MapRainbowColor(Barrel barrel, PointCyl pt)
        {
            return ColorCoder.MapRainbowColor(pt.R, barrel.MinRadius(pt.Z, pt.ThetaRad), barrel.MaxRadius(pt.Z, pt.ThetaRad));            
        }
    }
}
