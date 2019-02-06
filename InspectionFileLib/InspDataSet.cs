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
        public AxialDataSet(Barrel barrel):base(barrel)
        {
            CorrectedCylData = new CylData();
            UncorrectedCylData = new CylData();
        }
    }
   
    public class SpiralDataSet:InspDataSet
    {
        public CylGridData CorrectedSpiralData { get; set; }
        public CylGridData UncorrectedSpiralData { get; set; }
        public SpiralDataSet(Barrel barrel) : base(barrel)
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
        public PointCyl[] RawLandPoints { get; set; }
        public PointCyl[] CorrectedLandPoints { get; set; }
        double getRVariation(PointCyl[] pts)
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
        public RingDataSet(Barrel barrel) : base(barrel)
        {
            CorrectedCylData = new CylData();
            UncorrectedCylData = new CylData();
            RawLandPoints = new PointCyl[Barrel.DimensionData.GrooveCount];
            CorrectedLandPoints = new PointCyl[Barrel.DimensionData.GrooveCount];
        }
    }
    public class CartDataSet:InspDataSet
    {
        public CartData CartData { get; set; }      
        public CartData ModCartData { get; set; }
        public void FitToCircleKnownR(Vector3 pt1, Vector3 pt2,double radius)
        {           
            var centers = DataUtilities.FindCirclesofKnownR(pt1, pt2, radius);
            var center = new Vector3();
            if (centers[0].Y > centers[1].Y)
            {
                center = centers[0];
            }
            else
            {
                center = centers[1];
            }
            ModCartData.RotateDataToLine(pt1, pt2);
            ModCartData.Translate(new Vector3(-1.0 * center.X, -1.0 * center.Y, 0));
            double scaling = 1.0;
            ModCartData = DataConverter.UnrollCylinderRing( ModCartData, scaling, radius);
        }
        public CartDataSet(Barrel barrel) : base(barrel)
        {
            CartData = new CartData();
            ModCartData = new CartData();
        }
    }
    public class CartGridDataSet : InspDataSet
    {
        public CartGridData CartGridData { get; set; }
        public CartGridDataSet(Barrel barrel):base (barrel)
        {
            CartGridData = new CartGridData();
        }
    }

    public class InspDataSet
    {
        public Barrel Barrel { get; protected set; }
        public ScanFormat DataFormat { get; protected set; }
        public string Filename { get;  set; }
        public InspDataSet(Barrel barrel)
        {           
            Barrel = barrel;
        }
    }
}
