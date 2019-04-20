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
    public class ProfileBuilder 
    {
       
        
       
       
        static BarrelInspProfile BuildFromRings(List<InspDataSet> inspDataSets,int grooveCount)
        {
            try
            {
                var profile = new BarrelInspProfile();
                var groovePointList = new List<CylData>();
                var aveLandPointList = new List<CylData>();               
                var aveGrooveProfile = new CylData(inspDataSets[0].FileName);
                var minLandProfile = new CylData(inspDataSets[0].FileName);
                var aveLandProfile = new CylData(inspDataSets[0].FileName);
                
                
                foreach(InspDataSet dataset in inspDataSets)
                {
                    if(dataset is RingDataSet ringData)
                    {
                        var inspData =ringData.CylData;

                        var groovePoints = new CylData(inspDataSets[0].FileName);
                        var landPoints = new CylData(inspDataSets[0].FileName);                        
                        int pointCt = ringData.CylData.Count;
                        int deltaIndex = pointCt / grooveCount;

                        int[] grooveIndices = new int[grooveCount];
                        int[] landIndices = new int[grooveCount];
                        //get land and groove indices
                        int grooveIndex = 0;


                        int landIndex = 0;
                        for (int j = 0; j < grooveCount; j++)
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
                   // profile.Barrel = inspDataSets[0].Barrel;
                }
                    
                return profile;
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        static BarrelInspProfile BuildFromAxial(List<InspDataSet> inspDataSets)
        {
            try
            {
                var profile = new BarrelInspProfile();
                var grooveProfile = new List<PointCyl>();
                var landProfile = new List<PointCyl>();

                int minLength = int.MaxValue;
                foreach(InspDataSet dataset in inspDataSets)
                {
                    if(dataset  is CylDataSet cylData)
                    {
                        int len = cylData.CylData.Count;
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
                        if (dataset is CylDataSet cylData)
                        {
                            z = cylData.CylData[j].Z;
                            th = cylData.CylData[j].ThetaRad;
                           // if (cylData.DataFormat == ScanFormat.LAND)
                            {
                                landRadius += cylData.CylData[j].R;
                                landCount++;
                            }
                           // if (cylData.DataFormat == ScanFormat.GROOVE)
                            {
                                grooveRadius += cylData.CylData[j].R;
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
                //profile.Barrel = inspDataSets[0].Barrel;
                return profile;
            }
            catch (Exception)
            {

                throw;
            }
        }
        static public BarrelInspProfile Build(List<InspDataSet> inspDataSets,int grooveCount)
        {
            try
            {
                var barrelProfile = new BarrelInspProfile();
                if (inspDataSets != null && inspDataSets.Count > 0)
                {
                    ScanFormat format = inspDataSets[0].DataFormat;
                    switch (format)
                    {
                        case ScanFormat.RING:
                            barrelProfile = BuildFromRings(inspDataSets,grooveCount);
                            break;
                        case ScanFormat.AXIAL:
                        //case ScanFormat.GROOVE:
                        //case ScanFormat.LAND:
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
        public ProfileBuilder()
        {

        }

    }
}
