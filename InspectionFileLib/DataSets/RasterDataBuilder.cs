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
    public class RasterDataBuilder : DataBuilder
    {

        InspDataSet BuildRasterPoints(RasterInspScript script, double[] data)
        {
            try
            {
                var points = new CylData(script.InputDataFileName);
                double theta = script.StartLocation.Adeg;
                double z = script.StartLocation.X;
                double r = 0;
                double nextZ = script.StartLocation.X + script.AxialIncrement;
                double direction = 1;
                var dataSet = new RasterDataSet(script.InputDataFileName);
                for (int i = 0; i < data.Length; i++)
                {

                    theta = i * script.AngleIncrement + script.StartLocation.Adeg; ;

                    if (theta >= script.EndLocation.Adeg && z < nextZ)
                    {
                        direction = -1;
                        z = i * script.AxialIncrement + script.StartLocation.X;

                    }
                    if (theta <= script.EndLocation.Adeg && z < nextZ)
                    {
                        direction = 1;
                        z = i * script.AxialIncrement + script.StartLocation.X;

                    }
                    if (theta < script.EndLocation.Adeg && theta > script.StartLocation.Adeg)
                    {
                        if (direction > 0)
                            theta = direction * i * script.AngleIncrement + script.StartLocation.Adeg;
                        if (direction < 0)
                            theta = direction * i * script.AngleIncrement + script.EndLocation.Adeg;
                        if (z >= nextZ)
                        {
                            nextZ += script.AxialIncrement;
                        }
                    }

                    r = data[i];
                    var pt = new PointCyl(r, GeomUtilities.ToRadians(theta), z, i);
                    dataSet.CorrectedCylData.Add(pt);
                }
                return dataSet;
            }
            catch (Exception)
            {

                throw;
            }
        }
       
        public InspDataSet BuildDataAsync(CancellationToken ct, IProgress<int> progress, RasterInspScript script, double[] rawDataSet, DataOutputOptions options)
        {
            try
            {
                //Init(options);
                var sw = new Stopwatch();
                progress.Report(sw.Elapsed.Seconds);

                var dataSet = BuildRasterPoints(script, rawDataSet );

                return dataSet;
            }
            catch (Exception)
            {

                throw;
            }

        }
        ScanFormat _scanformat;
        public RasterDataBuilder()
        {
            _scanformat = ScanFormat.RASTER;
        }
    }
}
