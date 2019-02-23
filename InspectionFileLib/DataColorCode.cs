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
                                c = ColorbyGroove(barrel, pt);
                                break;
                            case COLORCODE.RAINBOW:
                                c = ColorAbsRadialError(barrel, pt);
                                break;
                            case COLORCODE.MONO_RED:
                                c = ColorRelRadialError(barrel, options, pt);
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
        static RGBColor ColorRelRadialError(Barrel barrel, DataOutputOptions options, PointCyl pt)
        {
            RGBColor c;
            double rGMax = barrel.DimensionData.GrooveMaxDiam / 2.0;
            double rGMin = barrel.DimensionData.GrooveMinDiam / 2.0;
            double rLMax = barrel.DimensionData.LandMaxDiam / 2.0;
            double rLMin = barrel.DimensionData.LandMinDiam / 2.0;
           
                var drLand = (rLMax - rLMin) / 2.0;
                var drMax = rGMax - rLMax;
                var drMin = rGMin - rLMin;
           rLMin = (barrel.DimensionData.ActualLandDiam / 2.0) - drLand;
           rLMax = (barrel.DimensionData.ActualLandDiam / 2.0) + drLand;

           
            double rGErr = rGMax - pt.R;
            if (pt.R > rGMax)
            {
                c = new RGBColor(1.0f, .0f, 0.0f);
            }
            else if (pt.R >= rLMin && pt.R <= rLMax)
            {
                c = new RGBColor(0.0f, 1.0f, 0.0f);
            }
            else
            {
                float red = (float)(0.4 * (pt.R - rLMax) / (rGMax - rLMax));
                float green = 1.0f - red;
                c = new RGBColor(red, green, 0.0f);
            }

            return c;

        }
        static RGBColor ColorAbsRadialError(Barrel barrel, PointCyl pt)
        {
            RGBColor c;
            if (pt.R > barrel.MaxRadius(pt.Z,pt.ThetaRad))
            {
                c = new RGBColor(1.0f, 0f, 0f);
            }
            else if (pt.R < barrel.MaxRadius(pt.Z, pt.ThetaRad))
            {
                c = new RGBColor(0.0f, 0.0f, 1.0f);
            }
            else
            {
                c = new RGBColor(0.0f, 1.0f, 0.0f);
            }
            return c;
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
