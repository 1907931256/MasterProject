using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
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
        public BoreProfile(DimensionData dd)
        {            
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
        M240_762mm,
        M_50mm,
        UNKNOWN
    }
    /// <summary>
    /// contains gun barrel definition filenames, twist profile and cross-section profiles
    /// </summary>
    public class Barrel
    {
        public BarrelType Type { get; private set; }
        public BoreDiameterType BoreDiameterType { get; set; }

        //public MachiningData MachiningData { get; set; }
        //public ManufacturingData ManufactureData { get; set; }
        //public LifetimeData LifetimeData { get; set; } 
        public DimensionData DimensionData{ get; set; }
        public BoreProfile BoreProfile { get; set; }      
        public TwistProfile TwistProfile{ get; set; }
        public BarrelProfile MinProfile{ get { return _minProfile; } }
        public BarrelProfile NomProfile { get { return _nomProfile; } }
        public BarrelProfile MaxProfile { get { return _maxProfile; } }
        public string MinProfileFilename
        {
            get
            {
                return _minProfName;
            }
            set
            {
                _minProfName = value;
                _minProfile =  BuildProfile(_minProfName, BarrelProfileType.Min);
                if (_minProfile!= null && _minProfile.Entities != null)
                {
                    _containsMinProfile = true;
                }
                else
                {
                    _containsMinProfile = false;
                }
            }
        }
        public string MaxProfileFilename
        {
            get
            {
                return _maxProfName;
            }
            set
            {
                _maxProfName = value;
                _maxProfile = BuildProfile(_maxProfName, BarrelProfileType.Max);        
                if(_maxProfile!= null && _maxProfile.Entities!=null)
                {
                    _containsMaxProfile = true;
                }
                else
                {
                    _containsMaxProfile = false;
                }

            }
        }
        
        public string NomProfileFilename
        {
            get
            {
                return _nomProfName;
            }
            set
            {
                _nomProfName = value;
                _nomProfile = BuildProfile(_nomProfName, BarrelProfileType.Nominal);
                if (_nomProfile != null && _nomProfile.Entities != null)
                {
                    _containsNomProfile = true;
                }
                else
                {
                    _containsNomProfile = false;
                }
            }
        }
        BarrelProfile BuildProfile(string dxfFileName, BarrelProfileType xSectionType )
        {
            BarrelProfile profile;
            if (dxfFileName != "" && System.IO.File.Exists(dxfFileName))
            {
                profile = new BarrelProfile(Type, dxfFileName, xSectionType, _meshSize);
            }
            else
            {
                profile = null;
            }
            return profile;
        }
        public bool ContainsMinProfile { get { return _containsMinProfile; } }
        public bool ContainsMaxProfile { get { return _containsMaxProfile; } }
        public bool ContainsNomProfile { get { return _containsNomProfile; } }
        BarrelProfile _minProfile;
        BarrelProfile _maxProfile;
        BarrelProfile _nomProfile;
        string _minProfName;
        string _maxProfName;
        string _nomProfName;
        
        
        //public List<DwgEntity> MinEntitiesAt(double thetaRad)
        //{
        //        return MinProfile.rotateEntities(thetaRad);
        //}
        //public List<DwgEntity> MaxEntitiesAt(double thetaRad) 
        //{           
        //        return MaxProfile.rotateEntities(thetaRad);           
        //}
        //public List<DwgEntity> NomEntitiesAt(double thetaRad) 
        //{           
        //        return NomProfile.rotateEntities(thetaRad);            
        //}

        public int GetGrooveNumber(double z,double thetaRad)
        {
            return DimensionData.GetGrooveNumber(z, thetaRad);
        }
       
        public double MaxRadius(double z, double thetaRad)
        {
            double r =  0;
            if (_containsMaxProfile)
                r = _maxProfile.RadiusAt(z, thetaRad);
            else
                r = DimensionData.MaxRadiusAt(z, thetaRad);
            return r;
        } 
        public double NomRadius(double z, double thetaRad)
        {
            double r = 0;
            if (_containsNomProfile)
                r = _nomProfile.RadiusAt(z, thetaRad);
            else
                r = DimensionData.NomRadiusAt(z, thetaRad);
            return r;
        }
        public double MinRadius(double z, double thetaRad)
        {
            double r = 0;
            if (_containsMinProfile)
                r = _minProfile.RadiusAt(z, thetaRad);
            else
                r = DimensionData.MinRadiusAt(z, thetaRad);
            return r;
        }
       
        public double TwistRad(double z)
        {
            return TwistProfile.ThetaRadAt(z);
        }

       
        //private 
       
        
        public static BarrelType GetBarrelType(string name)
        {
            try
            {
                BarrelType bt = BarrelType.UNKNOWN;
                var barrelTypes = Enum.GetNames(typeof(BarrelType));
                foreach (var type in barrelTypes)
                {
                    if (name.Contains(type))
                    {
                        bt = (BarrelType)Enum.Parse(typeof(BarrelType), type);
                        break;
                    }
                }
                return bt;
            }
            catch (Exception)
            {

                throw;
            }
           
        }
        double _meshSize;
        void Initialize( )
        {
            try
            {
                //ManufactureData = new ManufacturingData();
                //LifetimeData = new LifetimeData();
                DimensionData = new DimensionData(Type);
                TwistProfile = new TwistProfile(Type);
                BoreProfile = new BoreProfile(DimensionData);
                //MachiningData = new MachiningData();
                _meshSize = .0005;
                switch (Type)
                {
                    case BarrelType.M2_50_Cal:
                        BuildM2_50_Cal();
                        break;
                    case BarrelType.M_50mm:
                        break;
                    case BarrelType.M240_762mm:
                        break;
                    case BarrelType.M242_25mm:
                        break;
                    case BarrelType.M284_155mm:
                        BuildM284_155mm();
                        break;
                }
               
            }
            catch (Exception)
            {

                throw;
            }
            
        }
      
        bool _containsMaxProfile;
        bool _containsMinProfile;
        bool _containsNomProfile;

        void BuildM284_155mm()
        {
            MinProfileFilename = barrelFolderName + "155mm_Profile_Min.dxf";
            NomProfileFilename = barrelFolderName + "155mm_Profile_Nom.dxf";
            MaxProfileFilename = barrelFolderName + "155mm_Profile_Max.dxf";           
        }
        void BuildM2_50_Cal()
        {
            MinProfileFilename = barrelFolderName + "50Cal_Profile_Min.dxf";
            NomProfileFilename = "";
            MaxProfileFilename = barrelFolderName + "50Cal_Profile_Max.dxf";
            
        }
         
        string barrelFolderName;
        public Barrel(BarrelType type)
        {
            barrelFolderName = "Barrel_Profiles/";
            Type = type;
            Initialize();
        }
    }
}
