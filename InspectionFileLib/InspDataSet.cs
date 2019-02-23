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
    public class AxialDataSet:InspDataSet
    {
        public CylData CorrectedCylData { get; set; }
        public CylData UncorrectedCylData { get; set; }
        public AxialDataSet(Barrel barrel, string filename) : base(barrel, filename)
        {
            CorrectedCylData = new CylData(Filename);
            UncorrectedCylData = new CylData(Filename);
        }
    }
   
    public class SpiralDataSet:InspDataSet
    {
        public CylGridData CorrectedSpiralData { get; set; }
        public CylGridData UncorrectedSpiralData { get; set; }
        public SpiralDataSet(Barrel barrel, string filename) : base(barrel, filename)
        {
            CorrectedSpiralData = new CylGridData();
            UncorrectedSpiralData = new CylGridData();
        }
    }
    public class RingDataSet : InspDataSet
    {
        public double NominalMinDiam { get; set; }
        public CylData CorrectedCylData { get; set; }
        public CylData UncorrectedCylData { get; set; }
        public CylData RawLandPoints { get; set; }
        public CylData CorrectedLandPoints { get; set; }
        double getRVariation(CylData pts)
        {
            double maxR = double.MinValue;
            double minR = double.MaxValue;
            foreach (PointCyl pt in RawLandPoints)
            {
                if (pt.R > maxR)
                {
                    maxR = pt.R;
                }
                if (pt.R < minR)
                {
                    minR = pt.R;
                }
            }
            double range = maxR - minR;
            return range;
        }
        public double GetCorrectedLandVariation()
        {
            return getRVariation(CorrectedLandPoints);
        }
        public double GetRawLandVariation()
        {
            return getRVariation(RawLandPoints);
        }
        public RingDataSet(Barrel barrel, string filename) : base(barrel, filename)
        {
            CorrectedCylData = new CylData(Filename);
            UncorrectedCylData = new CylData(Filename);
            RawLandPoints = new CylData(filename);
            CorrectedLandPoints = new CylData(filename);
        }
    }
    public class CartDataSet:InspDataSet
    {
        public CartData CartData { get; set; }      
        
        public CartDataSet(Barrel barrel, string filename) : base(barrel, filename)
        {
            CartData = new CartData(filename);
           
        }
    }
    public class CartGridDataSet : InspDataSet
    {
        public CartGridData CartGridData { get; set; }
        public CartGridDataSet(Barrel barrel,string filename):base (barrel,filename)
        {
            CartGridData = new CartGridData();
        }
    }

    public class InspDataSet
    {
        public Barrel Barrel { get; protected set; }
        public ScanFormat DataFormat { get; protected set; }
        public string Filename { get; protected set; }
        public InspDataSet(Barrel barrel,string filename)
        {           
            Barrel = barrel;
            Filename = filename;
        }
    }
}
