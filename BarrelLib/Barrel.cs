﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeometryLib;
using ToolpathLib;
using CNCLib;
using DwgConverterLib;
using FileIOLib;
namespace BarrelLib
{
  
    public enum BoreDiameterType
    {
        DEFAULT,
        USER_DEFINED,
        PROFILE
    }
   
    public class BoreProfile:List<PointCyl>
    {
        public double GetRadiusAt(double axialPosition)
        {
            double r = 0;
            for(int i=0;i<this.Count-1;i++)
            {
                if(axialPosition >=this[i].Z && axialPosition<this[i+1].Z)
                {
                    double deltaR = (this[i + 1].R - this[i].R);
                    if (deltaR != 0)
                    {
                        r = ((this[i + 1].Z - this[i].Z) / deltaR) *(axialPosition - this[i].Z) + this[i].R;
                    }
                    else
                    {
                        r = this[i].R;
                    }
                    break;
                }
            }
            return r;
        }
        public BoreProfile()
        {
            var dd = new DimensionData();
            var bd = new PointCyl(dd.LandNominalDiam / 2.0, 0, 0);
            Add(bd);
            bd = new PointCyl(dd.LandNominalDiam / 2.0, 0, dd.Length);
            Add(bd);
        }
        public BoreProfile(BarrelType barrelType)
        {
            var dd = new DimensionData(barrelType);
            var bd = new PointCyl(dd.LandNominalDiam/2.0, 0, 0);
            Add(bd);
            bd = new PointCyl(dd.LandNominalDiam / 2.0, 0, dd.Length);
            Add(bd);
        }
        public BoreProfile(double radius,BarrelType barrelType)
        {
            var dd = new DimensionData(barrelType);
            var bd = new PointCyl(radius, 0, 0);
            Add(bd);
            bd = new PointCyl(radius, 0, dd.Length);
            Add(bd);
        }
        public BoreProfile(string filename)
        {
            
            var lines = FileIO.ReadDataTextFile(filename);
           

            for(int i=3;i<lines.Count;i++)
            {
                var words = FileIO.Split(lines[i]);
                if (double.TryParse(words[2], out double r) && double.TryParse(words[3], out double z))
                {
                    var bd = new PointCyl(r, 0, z);
                    Add(bd);
                }
            }
        }
    }
    public enum BarrelType
    {
        M2_50_Cal,
        M242_25mm,
        M284_155mm,
        M240_762mm
    }
    /// <summary>
    /// contains gun barrel definition filenames, twist profile and cross-section profiles
    /// </summary>
    public class Barrel
    {
        public BarrelType Type {get{ return _type;} set{ _type = value; }}
        BarrelType _type;
        BoreDiameterType _diameterType;
        public BoreDiameterType BoreDiameterType { get { return _diameterType; } set { _diameterType = value; } }
        LifetimeData _lifedata;
        MachiningData _machData;
        public MachiningData MachiningData { get { return _machData; }set { _machData = value; } }
        public ManufacturingData ManufactureData { get { return _manData; } set { _manData = value; } }
        public LifetimeData LifetimeData { get { return _lifedata; } set { _lifedata = value; }}
        public DimensionData DimensionData{ get{ return _dimensionData; } set{ _dimensionData = value; }}
        public BoreProfile BoreProfile { get { return _boreProfile; } set { _boreProfile = value; } }      
        public TwistProfile TwistProfile{get{return _twistProfile;} set{ _twistProfile = value;}}
        public XSectionProfile MinProfile{get {return _minProfile; }  set { _minProfile = value; } }
        public XSectionProfile NomProfile { get  {return _nomProfile;} set { _nomProfile = value; } }
        public XSectionProfile MaxProfile { get{ return _maxProfile; } set { _maxProfile = value; } }
        //public BoundingBox BoundingBox {get { return _boundingBox; }      }
        public List<DwgEntity> MinEntitiesAt(double thetaRad)
        {
                return _minProfile.rotateEntities(thetaRad);
        }
        public List<DwgEntity> MaxEntitiesAt(double thetaRad) 
        {           
                return _maxProfile.rotateEntities(thetaRad);           
        }
        public List<DwgEntity> NomEntitiesAt(double thetaRad) 
        {           
                return _nomProfile.rotateEntities(thetaRad);            
        }
      
        public int GetGrooveNumber(double z,double thetaRad)
        {
            return _dimensionData.GetGrooveNumber(z, thetaRad);
        }
       
        public double MaxRadius(double z, double thetaRad)
        {
            double r =  0;
            //if (_containsProfiles)
            //    r = _maxProfile.RadiusAt(z, thetaRad);
            //else
                r = _dimensionData.MaxRadiusAt(z, thetaRad);
            return r;
        } 
        public double NomRadius(double z, double thetaRad)
        {
            double r = 0;
            //if (_containsProfiles)
            //    r = _nomProfile.RadiusAt(z, thetaRad);
            //else
                r = _dimensionData.NomRadiusAt(z, thetaRad);
            return r;
        }
        public double MinRadius(double z, double thetaRad)
        {
            double r = 0;
            //if (_containsProfiles)
            //    r = _minProfile.RadiusAt(z, thetaRad);
            //else
                r = _dimensionData.MinRadiusAt(z, thetaRad);
            return r;
        }
       
        public double TwistRad(double z)
        {
            return _twistProfile.ThetaRadAt(z);
        }

       
        //private 
        XSectionProfile _minProfile;
        XSectionProfile _maxProfile;
        XSectionProfile _nomProfile;
        DimensionData _dimensionData;
        TwistProfile _twistProfile;
        BoreProfile _boreProfile;
        ManufacturingData _manData;
        //BoundingBox _boundingBox;
        //List<BoundingBox> _boundingBoxList;
        //bool _containsProfiles;

        public static BarrelType GetBarrelType(string name)
        {
            BarrelType bt = BarrelType.M2_50_Cal;
            

            if (name == "M2_50_Cal")
                bt = BarrelType.M2_50_Cal;
            if (name == "M242_25mm")
                bt = BarrelType.M242_25mm;
            if (name == "M284_155mm")
                bt = BarrelType.M284_155mm;
            if (name == "M240_762mm")
                bt = BarrelType.M240_762mm;
            return bt;
        }
        public Barrel()
        {
            _manData = new ManufacturingData();
            _lifedata = new LifetimeData();
            _dimensionData = new DimensionData(BarrelType.M2_50_Cal);
            _twistProfile = new TwistProfile(BarrelType.M2_50_Cal);           
            _boreProfile = new BoreProfile(BarrelType.M2_50_Cal);
        }
        public Barrel(BarrelType type)
        {
            _manData = new ManufacturingData();
            _machData = new MachiningData();
            _lifedata = new LifetimeData();
            _dimensionData = new DimensionData(type);            
            _twistProfile = new TwistProfile(type);
            _boreProfile = new BoreProfile(type);
            
        }
    }
}
