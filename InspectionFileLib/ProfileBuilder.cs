using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarrelLib;
using DataLib;
using GeometryLib;
namespace InspectionLib
{
    public class ProfileBuilder:DataBuilder
    {
       
        public BarrelInspProfile Build(List<InspDataSet> inspDataSets)
        {
            try
            {
                var barrelProfile = new BarrelInspProfile();
                if(inspDataSets != null && inspDataSets.Count>0)
                {                   
                    ScanFormat format = inspDataSets[0].DataFormat;                  
                    switch(format)
                    {
                        case ScanFormat.RING:
                            barrelProfile = BuildFromRings(inspDataSets );
                            break;
                        case ScanFormat.AXIAL:                            
                        case ScanFormat.GROOVE:                           
                        case ScanFormat.LAND:
                            barrelProfile = BuildFromAxial(inspDataSets);
                            break;
                    }


                }
                return barrelProfile;
            }
            catch (Exception)
            {

                throw;
            }
        }
       
       
        BarrelInspProfile BuildFromRings(List<InspDataSet> inspDataSets)
        {
            try
            {
                var profile = new BarrelInspProfile();
                var groovePointList = new List<CylData>();
                var aveLandPointList = new List<CylData>();               
                var aveGrooveProfile = new CylData();
                var minLandProfile = new CylData();
                var aveLandProfile = new CylData();
                
                
                foreach(InspDataSet dataset in inspDataSets)
                {
                    if(dataset is RingDataSet ringData)
                    {
                        var inspData =ringData.CorrectedCylData;

                        var groovePoints = new CylData( );
                        var landPoints = new CylData( );
                        int grooveCt = ringData.Barrel.DimensionData.GrooveCount;
                        int pointCt = ringData.CorrectedCylData.Count;
                        int deltaIndex = pointCt / grooveCt;

                        int[] grooveIndices = new int[grooveCt];
                        int[] landIndices = new int[grooveCt];
                        //get land and groove indices
                        int grooveIndex = 0;


                        int landIndex = 0;
                        for (int j = 0; j < grooveCt; j++)
                        {
                            grooveIndex = (int)(j * deltaIndex);
                            landIndex = (int)(j * deltaIndex + deltaIndex / 2);
                            grooveIndices[j] = grooveIndex;
                            landIndices[j] = landIndex;
                        }

                        double rGrooveAve = 0;
                        double rLandAve = 0;
                        var pt0 = inspData[0];
                        //average groove values
                        for (int k = 0; k < grooveIndices.Length; k++)
                        {
                            var groovePt = inspData[grooveIndices[k]];
                            rGrooveAve += groovePt.R;
                            var landPt = inspData[landIndices[k]];
                            rLandAve += landPt.R;
                        }
                        rLandAve /= grooveIndices.Length;
                        rGrooveAve /= grooveIndices.Length;
                        aveGrooveProfile.Add(new PointCyl(rGrooveAve, pt0.ThetaRad, pt0.Z));
                        aveLandProfile.Add(new PointCyl(rLandAve, pt0.ThetaRad, pt0.Z));
                        //use min or average land values

                        double minR = double.MaxValue;
                        foreach (PointCyl pt in inspData)
                        {
                            if (pt.R < minR)
                            {
                                minR = pt.R;
                            }
                        }
                        var ptMin = new PointCyl(minR, pt0.ThetaRad, pt0.Z);
                        minLandProfile.Add(ptMin);
                    }
                    profile.AveGrooveProfile = aveGrooveProfile;
                    profile.AveLandProfile = aveLandProfile;
                    profile.MinLandProfile = minLandProfile;
                    profile.Barrel = inspDataSets[0].Barrel;
                }
                    
                return profile;
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        BarrelInspProfile BuildFromAxial(List<InspDataSet> inspDataSets)
        {
            try
            {
                var profile = new BarrelInspProfile();
                var grooveProfile = new List<PointCyl>();
                var landProfile = new List<PointCyl>();

                int minLength = int.MaxValue;
                foreach(InspDataSet dataset in inspDataSets)
                {
                    if(dataset  is AxialDataSet axialData)
                    {
                        int len = axialData.CorrectedCylData.Count;
                        if (len < minLength)
                        {
                            minLength = len;
                        }
                    }
                  
                }
                for(int j=0; j< minLength;j++)
                {
                    double landRadius = 0;
                    double grooveRadius = 0;
                    int grooveCount = 0;
                    int landCount = 0;
                    double z = 0;
                    double th = 0;
                    foreach (InspDataSet dataset in inspDataSets)
                    {
                        if (dataset is AxialDataSet axialData)
                        {
                            z = axialData.CorrectedCylData[j].Z;
                            th = axialData.CorrectedCylData[j].ThetaRad;
                            if (axialData.DataFormat == ScanFormat.LAND)
                            {
                                landRadius += axialData.CorrectedCylData[j].R;
                                landCount++;
                            }
                            if (axialData.DataFormat == ScanFormat.GROOVE)
                            {
                                grooveRadius += axialData.CorrectedCylData[j].R;
                                grooveCount++;
                            }
                        }
                    }
                    grooveRadius /= grooveCount;
                    landRadius /= landCount;
                    var groovePt = new PointCyl(grooveRadius, th, z);
                    var landPt = new PointCyl(landRadius, th, z);
                    grooveProfile.Add(groovePt);
                    landProfile.Add(landPt);
                }
                profile.AveGrooveProfile.AddRange(grooveProfile);
                profile.AveLandProfile.AddRange(landProfile);
                profile.Barrel = inspDataSets[0].Barrel;
                return profile;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ProfileBuilder(Barrel barrel):base(barrel)
        {

        }

    }
}
