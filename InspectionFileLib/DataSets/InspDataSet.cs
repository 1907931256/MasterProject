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
    
    public class SpiralDataSet:InspDataSet
    {
        public CylGridData SpiralData { get; set; }
        public CylGridData UncorrectedSpiralData { get; set; }
        public SpiralDataSet(string filename) : base( filename)
        {
            SpiralData = new CylGridData();
            UncorrectedSpiralData = new CylGridData();
        }
        public SpiralDataSet()
        { 
            SpiralData = new CylGridData();
            UncorrectedSpiralData = new CylGridData();
        }
    }
    public class RingDataSet : CylDataSet
    {
        public double NominalMinDiam { get; set; }
        //public CylData CylData { get; set; }
        
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
        public RingDataSet( string filename) : base( filename)
        {          
            
            RawLandPoints = new CylData(filename);
            CorrectedLandPoints = new CylData(filename);
        }
        public RingDataSet()
        {

            RawLandPoints = new CylData();
            CorrectedLandPoints = new CylData();
        }
    }
    public class CylDataSet :InspDataSet
    {
        public CylData CylData { get; set; }
        public CylData UncorrectedCylData { get; set; }
        public CylDataSet(string filename) : base( filename)
        {
            CylData = new CylData(filename);
            UncorrectedCylData = new CylData(FileName);
        }
        public CylDataSet()
        {
            CylData = new CylData();
            UncorrectedCylData = new CylData();
        }
    }
    public class CartDataSet:InspDataSet
    {
        public CartData CartData { get; set; }      
        
        public CartDataSet( string filename) : base( filename)
        {
            CartData = new CartData(filename);           
        }
        public CartDataSet()
        {
            CartData = new CartData();
        }


    }
    public class CartGridDataSet : InspDataSet
    {
        public CartGridData CartGridData { get; set; }
        public CartGridDataSet(string filename):base (filename)
        {
            CartGridData = new CartGridData();
        }
    }

    public class InspDataSet
    {        
        public ScanFormat DataFormat { get;  set; }
        public string FileName { get;  set; }
        public string OutputFileName { get; set; }
        public InspDataSet()
        {

        }
        public InspDataSet(string filename)
        {  
            FileName = filename;
        }
    }
}
