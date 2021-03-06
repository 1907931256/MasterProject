﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileIOLib;

namespace BarrelLib
{
    
    /// <summary>
    /// contains barrel twist dat as array of twistValue types
    /// </summary>
    public class TwistProfile
    {
        public enum TwistType
        {
            Uniform,
            Progressive
        }
        public enum DirectionEnum
        {
            RIGHT_HAND=0,
            LEFT_HAND=1
        }
        public int DirectionSign {
            get
            {
                if (Direction == DirectionEnum.LEFT_HAND)
                    return 1;
                else
                    return -1;
            }
        }
        public DirectionEnum Direction { get; set; }
        TwistType _type;
        List<GeometryLib.PointCyl> _twist;
        public static string Name = "Twist_Profile";
        public static string EndName = "End_Twist";

        public double ThetaRadAt(double z)
        {
            double theta = 0;
            switch(_type)
            {
                case TwistType.Uniform:
                    theta= ThetaRadforconstantTwist(z);
                    break;
                case TwistType.Progressive:
                    theta= ThetaRadAtForNonConstant(z);
                    break;
            }
            return theta;
        }
        double ThetaRadforconstantTwist(double z)
        {
            double th =_twistDirection * Math.PI * 2 * (z / _inchPerRev);
            //th %= (Math.PI * 2);
            //if (th < 0)
             //   th += (Math.PI * 2);
            return th;
        }
        /// <summary>
        /// return theta for a given z value
        /// </summary>
        /// <param name="z"></param>
        /// <returns></returns>
        double ThetaRadAtForNonConstant(double z)
        {
            var tw0 = new GeometryLib.PointCyl();
            var tw1 = new GeometryLib.PointCyl();
            double theta = 0;
            bool zFound = false;

            for (int i = 0; i < _twist.Count - 1; i++)
            {
                if ((z >= _twist[i].Z) && (z <= _twist[i + 1].Z))
                {
                    tw0 = _twist[i];
                    tw1 = _twist[i + 1];
                    zFound = true;

                }
                if (zFound)
                    break;
            }
            if (zFound)
            {
                if ((tw1.Z - tw0.Z) < float.Epsilon)
                {
                    theta = tw1.ThetaRad;
                }
                else
                {
                    theta = (z - tw0.Z) * (tw1.ThetaRad - tw0.ThetaRad) / (tw1.Z - tw0.Z) + tw0.ThetaRad;
                }
            }
            return theta;

        }
        public List<string> ToStringList()
        {
            List<string> file = new List<string>();
            file.Add(TwistProfile.Name);//0
            file.Add(_type.ToString());//1            
            file.Add("z theta");//2
            file.Add("inch degrees");//3

            foreach (GeometryLib.PointCyl tw in _twist)
            {
                StringBuilder stb = new StringBuilder();
                stb.Append(tw.Z);
                stb.Append(FileIO.Splitter);
                stb.Append(tw.ThetaRad);
                file.Add(stb.ToString());
            }
            file.Add(TwistProfile.EndName);
            return file;
        }
        void OpenTXT(string fileName)
        {
            List<string> file = FileIO.ReadDataTextFile(fileName);

            _twist = new List<GeometryLib.PointCyl>();

            if (file.Count >= 2)
            {
                string[] splitter = FileIO.Splitter;
                if (file[0].Trim() == TwistProfile.Name)
                {
                    TwistType tempEnum = TwistType.Progressive;
                    if (Enum.TryParse<TwistType>(file[1], true, out tempEnum))
                        _type = tempEnum;
                    else
                        _type = TwistType.Progressive;

                    for (int i = 4; i < file.Count; i++)
                    {
                        string[] splitLine = file[i].Split(splitter, StringSplitOptions.RemoveEmptyEntries);
                        var twist = new GeometryLib.PointCyl();

                        if (splitLine.Length > 1)
                        {
                            double result = 0;
                            if (double.TryParse(splitLine[0].Trim(), out result))
                            {
                                twist.Z = result;
                            }
                            result = 0;
                            if (double.TryParse(splitLine[1].Trim(), out result))
                            {
                                twist.ThetaRad= result;
                            }

                            _twist.Add(twist);
                        }

                    }
                }             

            }          

        }
        
        double _inchPerRev;
        int _twistDirection;
       
        void Build155mm()
        {
            _type = TwistType.Uniform;
            
            _inchPerRev = 122;
            _twistDirection = -1;
        }
        void Build25mm()
        {
            _type = TwistType.Progressive;             
            _inchPerRev = 40;
            _twistDirection = -1;
        }
        void Build762mm()
        {
            _type = TwistType.Uniform;
            
            _inchPerRev = 12;
            _twistDirection = -1;
        }
        void Build50Cal()
        {
            _type = TwistType.Uniform;
             
            _inchPerRev = 15;
            _twistDirection = -1;
        }
        void Build50mm()
        {

        }
        void Build30mm()
        {

        }
        void Init()
        {
            if (Name == "50Cal")
                Build50Cal();
            if (Name == "155mm")
                Build155mm();
            if (Name == "25mm")
                Build25mm();
            if (Name == "7.62mm")
                Build762mm();
            if (Name == "50mm")
                Build50mm();
            if (Name == "30mm")
                Build30mm();
        }
        public TwistProfile(Barrel barrel)
        {           
            Init();
        }
       
    }
   
}
