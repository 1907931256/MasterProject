using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryLib;
using System.Diagnostics;
using System.Threading;
using DataLib;
using BarrelLib;

namespace InspectionLib
{
    //public class RasterDataBuilder : DataBuilder
    //{
        
    //    CylData BuildRasterPoints(CylInspScript script, double[] data)
    //    {
    //        try
    //        {
    //            var points = new CylData(_scanformat);
    //            double theta = script.StartThetaRad;
    //            double z = script.StartZ;
    //            double r = 0;
    //            double nextZ = script.StartZ + script.AxialIncrement;
    //            double direction = 1;
    //            for (int i = 0; i < data.Length; i++)
    //            {

    //                theta = i * script.AngleIncrement + script.StartThetaRad;

    //                if (theta >= script.EndThetaRad && z < nextZ)
    //                {
    //                    direction = -1;
    //                    z = i * script.AxialIncrement + script.StartZ;

    //                }
    //                if (theta <= script.StartThetaRad && z < nextZ)
    //                {
    //                    direction = 1;
    //                    z = i * script.AxialIncrement + script.StartZ;

    //                }
    //                if (theta < script.EndThetaRad && theta > script.StartThetaRad)
    //                {
    //                    if (direction > 0)
    //                        theta = direction * i * script.AngleIncrement + script.StartThetaRad;
    //                    if (direction < 0)
    //                        theta = direction * i * script.AngleIncrement + script.EndThetaRad;
    //                    if (z >= nextZ)
    //                    {
    //                        nextZ += script.AxialIncrement;
    //                    }
    //                }

    //                r = data[i];
    //                var pt = new PointCyl(r, theta, z, i);
    //                points.Add(pt);
    //            }
    //            return points;
    //        }
    //        catch (Exception)
    //        {

    //            throw;
    //        }
    //    }
    //    /// <summary>
    //    /// build Raster set from raw data
    //    /// 
    //    /// </summary>
    //    /// <param name="script"></param>
    //    /// <param name="rawInputData"></param>
    //    void BuildRasterFromRadialData(CylInspScript script, KeyenceSiDataSet rawInputData)
    //    {
    //        try
    //        {
    //            Debug.WriteLine("building data from raster inspection");
    //            var dataSet = new InspDataSet();
    //            var data = rawInputData.GetData();
    //            dataSet.UncorrectedCylData = BuildRasterPoints(script, data);

    //        }
    //        catch (Exception)
    //        {

    //            throw;
    //        }
    //    }
    //    ScanFormat _scanformat;
    //    public RasterDataBuilder(Barrel barrel) : base(barrel)
    //    {
    //        _scanformat = ScanFormat.GRID;
    //    }
    //}
}
