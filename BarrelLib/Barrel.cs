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
       
       
        public BoreProfile(Barrel barrel)
        {
           
            var bd = new PointCyl(barrel.DimensionData.LandNominalDiam/2.0, 0, 0);
            Add(bd);
            bd = new PointCyl(barrel.DimensionData.LandNominalDiam / 2.0, 0, barrel.DimensionData.Length);
            Add(bd);
        }
        public BoreProfile(double radius,Barrel barrel )
        {             
            var bd = new PointCyl(radius, 0, 0);
            Add(bd);
            bd = new PointCyl(radius, 0, barrel.DimensionData.Length);
            Add(bd);
        }
       
    }
   
    /// <summary>
    /// contains gun barrel definition filenames, twist profile and cross-section profiles
    /// </summary>
    public class Barrel
    {

        public static List<string> BarrelNameList { get; set; }
        public string Name { get; set; }
        
        public BoreDiameterType BoreDiameterType { get; set; }
        public DimensionData DimensionData{ get; set; }
        public BoreProfile BoreProfile { get; set; }      
       
        public BarrelProfile MinProfile{ get { return _minProfile; } }
        public BarrelProfile NomProfile { get { return _nomProfile; } }
        public BarrelProfile MaxProfile { get { return _maxProfile; } }
        string _dimensionFilename;
        public string DimensionFileName
        {
            get
            {
                return _dimensionFilename;
            }
            set
            {
                _dimensionFilename = value;
                DimensionData = BuildDimData(_dimensionFilename);

            }
        }
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
        DimensionData BuildDimData(string fileName)
        {
            try
            {
                DimensionData dd;
                if (fileName != "" && System.IO.File.Exists(fileName))
                {
                    dd = new DimensionData(this, fileName);
                }
                else
                {
                    dd = null;
                }
                return dd;
            }
            catch (Exception)
            {

                throw;
            }
           
        }
        BarrelProfile BuildProfile(string dxfFileName, BarrelProfileType xSectionType )
        {
            try
            {
                BarrelProfile profile;
                if (dxfFileName != "" && System.IO.File.Exists(dxfFileName))
                {
                    profile = new BarrelProfile(this, dxfFileName, xSectionType, _meshSize);
                }
                else
                {
                    profile = null;
                }
                return profile;
            }
            catch (Exception)
            {

                throw;
            }
            
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
   
        public double MaxRadius(double z, double thetaRad)
        {
            try
            {
                double r = 0;
                if (_containsMaxProfile)
                    r = _maxProfile.RadiusAt(z, thetaRad);
                return r;
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        public double NomRadius(double z, double thetaRad)
        {
            try
            {
                double r = 0;
                if (_containsNomProfile)
                    r = _nomProfile.RadiusAt(z, thetaRad);
                return r;
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        public double MinRadius(double z, double thetaRad)
        {
            try
            {
                double r = 0;
                if (_containsMinProfile)
                    r = _minProfile.RadiusAt(z, thetaRad);
                return r;
            }
            catch (Exception)
            {

                throw;
            }
           
        }

       
       
        //private 
       
        
       
        double _meshSize;
        void Initialize( )
        {
            try
            {
                //ManufactureData = new ManufacturingData();
                //LifetimeData = new LifetimeData();                                
                
                //MachiningData = new MachiningData();
                _meshSize = .0005;
                if(Name=="50Cal" )
                    Build50Cal();
                if (Name=="155mm")
                    Build155mm();
                if (Name == "25mm")
                    Build25mm();
                if (Name == "7.62mm")
                    Build762mm();
                if (Name == "50mm")
                    Build50mm();
                if (Name == "30mm")
                    Build30mm();
                BoreProfile = new BoreProfile(this);
            }
            catch (Exception)
            {
                throw;
            }
            
        }
      
        bool _containsMaxProfile;
        bool _containsMinProfile;
        bool _containsNomProfile;
        void Build25mm()
        {
            DimensionFileName = barrelFolderName + "25mm_Dimensions.txt";
            MinProfileFilename = barrelFolderName + "25mm_Profile_Min.dxf";
            NomProfileFilename = null;
            MaxProfileFilename = barrelFolderName + "25mm_Profile_Max.dxf";
            
        }
        void Build762mm()
        {
            DimensionFileName = barrelFolderName + "762mm_Dimensions.txt";
            MinProfileFilename = barrelFolderName + "762mm_Profile_Min.dxf";
            NomProfileFilename = null;
            MaxProfileFilename = barrelFolderName + "762mm_Profile_Max.dxf";
           
        }
        void Build30mm()
        {
            DimensionFileName = barrelFolderName + "30mm_Dimensions.txt";
            MinProfileFilename = barrelFolderName + "30mm_Profile_Min.dxf";
            NomProfileFilename = barrelFolderName + "30mm_Profile_Nom.dxf";
            MaxProfileFilename = barrelFolderName + "30mm_Profile_Max.dxf";
            
        }
        void Build50mm()
        {
            DimensionFileName = barrelFolderName + "50mm_Dimensions.txt";
            MinProfileFilename = barrelFolderName + "50mm_Profile_Min.dxf";
            NomProfileFilename = null;
            MaxProfileFilename = barrelFolderName + "50mm_Profile_Max.dxf";
            
        }
        void Build155mm()
        {
            DimensionFileName = barrelFolderName + "155mm_Dimensions.txt";
            MinProfileFilename = barrelFolderName + "155mm_Profile_Min.dxf";
            NomProfileFilename = barrelFolderName + "155mm_Profile_Nom.dxf";
            MaxProfileFilename = barrelFolderName + "155mm_Profile_Max.dxf";
            
        }
        void Build50Cal()
        {
            DimensionFileName = barrelFolderName + "50Cal_Dimensions.txt";
            MinProfileFilename = barrelFolderName + "50Cal_Profile_Min.dxf";
            NomProfileFilename = null;
            MaxProfileFilename = barrelFolderName + "50Cal_Profile_Max.dxf";
            

        }
         
        string barrelFolderName;
        static Barrel()
        {
            BarrelNameList = new List<string>() { "50Cal", "25mm", "155mm", "7.62mm", "50mm", "30mm", "Flat Plate" };
        }
        public Barrel(string name)
        {            
            barrelFolderName = "Barrel_Profiles/";
            Name = name;
            Initialize();
        }
    }
}
