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
   

    public class InspDataSet
    {
        public CartData CartData { get; set; }
        public CartGridData CartGridData { get; set; }
        public CylData CorrectedCylData { get; set; }
        public CylData UncorrectedCylData { get; set; }
        public CylGridData CorrectedSpiralData { get; set; }
        public CylGridData UncorrectedSpiralData { get; set; }
        public PointCyl[] RawLandPoints { get; set; }
        public PointCyl[] CorrectedLandPoints { get; set; }
        public Barrel Barrel { get; set; }
        public ScanFormat DataFormat { get; set; }
        public string Filename { get; set; }

        public double GetRVariation(PointCyl[] points)
        {
            double maxR = double.MinValue;
            double minR = double.MaxValue;
            foreach(PointCyl pt in points)
            {
                if(pt.R>maxR)
                {
                    maxR = pt.R;
                }
                if(pt.R<minR)
                {
                    minR = pt.R;
                }
            }
            double range = maxR - minR;
            return range;
        }
        void Init()
        {
            CorrectedCylData = new CylData();
            UncorrectedCylData = new CylData();
            CorrectedSpiralData = new CylGridData();
            UncorrectedSpiralData = new CylGridData();
            DataFormat = ScanFormat.RING;            
            Barrel = new Barrel();
            RawLandPoints = new PointCyl[Barrel.DimensionData.GrooveCount];
            CorrectedLandPoints = new PointCyl[Barrel.DimensionData.GrooveCount];
        }
        public InspDataSet(string filename)
        {
            Filename = filename;
            Init();
        }
        public InspDataSet()
        {
            Filename = "";
            Init();
        }
    }
}
