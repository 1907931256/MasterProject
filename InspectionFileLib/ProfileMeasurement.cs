using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryLib;
using DataLib;
using FileIOLib;
using BarrelLib;

namespace InspectionLib
{
    /// <summary>
    /// list of profile depth measurement across several grooves
    /// </summary>
    public class BarrelXsectionProfile: List<GrooveDepthProfile>
    {

        double _axialLoc;
        Barrel _barrel;
        MachineRasterSpeeds _machineSpeedList;
        int _currentPassCt;
        int _grooveCount;
        double _nominalLandDiam;
        GrooveDepthProfile _averageDepths;

        public List<string> AsStringList()
        {
            var lines = new List<string>();
            lines.Add("Depth measurements");
            int g = 1;
            foreach (var groove in this)
            {
                foreach (var dm in groove)
                {
                    string s = "Groove: " + g.ToString() + ", Raster Order: " + dm.RasterOrder.ToString() + ", Depth: " + dm.Depth.ToString("f5");
                    lines.Add(s);
                }
                g++;
            }
            return lines;
        }
        GrooveDepthProfile _aveDepths;
        public void CalcGrooveStats(CylData data)
        {
            try
            {
                CalcAllGrooveDepths(data);
                int maxRadiusPtCount = 0;
                int minRadiusPtCount = 0;
                int totalRadiusPtCount = 0;
                foreach (var groove in this)
                {
                    foreach (var dm in groove)
                    {
                        totalRadiusPtCount++;
                        if (dm.Datum.R * 2.0 > _barrel.DimensionData.GrooveMaxDiam)
                        {
                            maxRadiusPtCount++;
                        }
                        if (dm.Datum.R * 2.0 < _barrel.DimensionData.GrooveMinDiam)
                        {
                            minRadiusPtCount++;
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public GrooveDepthProfile CalcAverageDepths(CylData data)
        {
            try
            {
                CalcAllGrooveDepths(data);
                double[] depthAveArr = new double[_machineSpeedList.RasterCount];
                _aveDepths = new GrooveDepthProfile();
                foreach (var groove in this)
                {
                    int rasterIndex = 0;
                    foreach (var depthMeasurement in groove)
                    {
                        depthAveArr[rasterIndex] += depthMeasurement.Depth;
                        rasterIndex++;
                    }
                }
                for (int i = 0; i < depthAveArr.Length; i++)
                {
                    double aveDepth = depthAveArr[i] / _grooveCount;

                    _aveDepths.Add(new DepthMeasurement(new PointCyl(), this[0][i].Theta, this[0][i].RasterOrder, aveDepth));
                }
                _aveDepths.CurrentPassCount = _currentPassCt;
                _aveDepths.TargetPassCount = _machineSpeedList.TargetPasses;
                _aveDepths.AxialLocation = _axialLoc;
                _aveDepths.Barrel = _barrel;
                return _aveDepths;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void SaveMeasurements(string outputFilename, string inputFilename, bool saveAll, bool saveAve)
        {
            try
            {
                var file = new List<string>();
                var newFilename = DataFileBuilder.BuildFileName(outputFilename, "_autoDepth",".csv");

                file.Add("InputFile: " + inputFilename);
                file.Add("Barrel Type: " + _barrel.Type.ToString());
                file.Add("Barrel S/N: " + _barrel.ManufactureData.SerialNumber);
                file.Add("Status: " + _barrel.ManufactureData.CurrentManufStep);
                file.Add("Axial Location: " + _axialLoc.ToString("f5"));
                file.Add("Current Pass Count: " + _currentPassCt.ToString());
                file.Add("Total Pass Target: " + _machineSpeedList.TargetPasses.ToString());

                if (saveAll)
                {
                    file.Add("GrooveNumber,RasterOrder,Theta(degs),Depth");
                    foreach (var groove in this)
                    {
                        foreach (var depthMeasurement in groove)
                        {
                            string s = groove.GrooveNumber.ToString() + "," + depthMeasurement.RasterOrder.ToString() + ","
                                + Geometry.ToDegs(depthMeasurement.Theta).ToString("f4") + "," + depthMeasurement.Depth.ToString("f5");
                            file.Add(s);
                        }
                    }
                }

                if (saveAve)
                {
                    file.Add("Average Depths");
                    file.Add("RasterOrder,Theta(degs),Depth");
                    for (int i = 0; i < _machineSpeedList.RasterCount; i++)
                    {
                        string s = (_aveDepths[i].RasterOrder).ToString() + "," + (Geometry.ToDegs(_machineSpeedList[i].ThetaRel)).ToString("f4") + "," + (_aveDepths[i].Depth).ToString("f5");
                        file.Add(s);
                    }
                }

                FileIO.Save(file.ToArray(), newFilename);
            }
            catch (Exception)
            {

                throw;
            }

        }
        bool FindIntersectionAt(double th, PointCyl p0, PointCyl p1, out PointCyl intersection)
        {
            try
            {
                intersection = new PointCyl();
                double th0 = p0.ThetaRad;
                double th1 = p1.ThetaRad;
                if (th < 0)
                    th += (Math.PI * 2.0);
                if (th0 < 0)
                    th0 += (Math.PI * 2.0);
                if (th1 < 0)
                    th1 += (Math.PI * 2.0);
                if (th > th0 && th <= th1)
                {
                    double dth = th1 - th0;
                    double dr = p1.R - p0.R;
                    double r = (th - th0) * (dr / dth) + p0.R;
                    intersection = new PointCyl(r, th, 0);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }

        }
        void CalcAllGrooveDepths(CylData data)
        {
            try
            {

                double depth = -1;
                double nomR = _nominalLandDiam / 2.0;
                PointCyl intersection = new PointCyl();
                foreach (var groove in this)
                {
                    foreach (var depthMeasurement in groove)
                    {
                        for (int i = 0; i < data.Count - 1; i++)
                        {
                            if (FindIntersectionAt(depthMeasurement.Theta, data[i], data[i + 1], out intersection))
                            {
                                depthMeasurement.Datum = intersection;
                                depth = intersection.R - nomR;
                                depthMeasurement.Depth = depth;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

        }


        void BuildMeasurementSet()
        {
            try
            {
                _grooveCount = _barrel.DimensionData.GrooveCount;


                double dt = (Math.PI * 2.0) / _grooveCount;

                for (int g = 0; g < _grooveCount; g++)
                {
                    var gv = new GrooveDepthProfile(g, _currentPassCt, _machineSpeedList.TargetPasses, _barrel);
                    for (int i = 0; i < _machineSpeedList.RasterCount; i++)
                    {
                        int rasterOrder = _machineSpeedList[i].RasterIndex;
                        double rasterAngle = (dt * g) + (_machineSpeedList[i].ThetaRel + _machineSpeedList.RasterOffsetAngle);
                        var dm = new DepthMeasurement(new PointCyl(), rasterAngle, rasterOrder);
                        gv.Add(dm);
                    }
                    this.Add(gv);
                }
            }
            catch (Exception)
            {

                throw;
            }


        }
        DataOutputOptions _options;
        public BarrelXsectionProfile(Barrel barrel, DataOutputOptions options, int currentPassCount, double axialLocation, string machineSpeedFileName)
        {
            _options = options;
            _currentPassCt = currentPassCount;

            _barrel = barrel;
            _axialLoc = axialLocation;

            _nominalLandDiam = _barrel.DimensionData.ActualLandDiam;
            
          
            _machineSpeedList = new MachineRasterSpeeds(machineSpeedFileName);

            BuildMeasurementSet();
        }
    }
}
