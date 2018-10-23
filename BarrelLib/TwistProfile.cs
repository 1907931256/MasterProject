using System;
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
        public int Direction { get { return _direction; } }
        TwistType _type;
        TwistValue[] _twist;
        public static string Name = "Twist_Profile";
        public static string EndName = "End_Twist";

        public double ThetaRadAt(double z)
        {
            double theta = 0;
            switch(_type)
            {
                case TwistType.Constant:
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
            double th =_direction * Math.PI * 2 * (z / _inchPerRev);
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
            TwistValue tw0 = new TwistValue();
            TwistValue tw1 = new TwistValue();
            double theta = 0;
            bool zFound = false;

            for (int i = 0; i < _twist.Length - 1; i++)
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

            foreach (TwistValue tw in _twist)
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
            
            List<TwistValue> twistList = new List<TwistValue>();

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
                        TwistValue twist = new TwistValue();

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

                            twistList.Add(twist);
                        }

                    }
                }
                _twist = twistList.ToArray();

            }
            else
            {
                _type = TwistType.Progressive;
                _twist = new TwistValue[0];
            }

        }
        public TwistProfile(string twistFileName)
        {
            OpenTXT(twistFileName);
        }
        double _length;
        double _inchPerRev;
        int _direction;
        public TwistProfile(double length, double inchPerRevolution,int direction)
        {
            _type = TwistType.Constant;
            _inchPerRev = inchPerRevolution;
            _direction = direction;
            _length = length;
            int len = (int)Math.Ceiling(_length * 1000);
            double dl = _length / 1000;
            double dt = _direction* (Math.PI * 2) / (inchPerRevolution * 1000);
            _twist = new TwistValue[len];
            for(int i=0;i<len;i++)
            {
               _twist[i] = new TwistValue(i * dl, i * dt);
            }
        }
        public TwistProfile(BarrelType type)
        {
            _type = TwistType.Constant;
            _twist = new TwistValue[0];
            switch(type)
            {
                case BarrelType.M2_50_Cal:
                    setM2Twist();
                    break;
                case BarrelType.M242_25mm:
                    setM242Twist();
                    break;                
                case BarrelType.M284_155mm:
                    setM284Twist();
                    break;
                case BarrelType.M240_762mm:
                    setM240Twist();
                    break;
            }
           
        }
        public TwistProfile()
        {
            _type = TwistType.Constant;
            _twist = new TwistValue[0];
            setM2Twist();
        }
        void setM284Twist()
        {
            _type = TwistType.Constant;
            _length = 240;
            _inchPerRev = 122;
            _direction = -1;
        }
        void setM242Twist()
        {
            _type = TwistType.Progressive;
            _length = 78;
            _inchPerRev = 40;
            _direction = -1;
        }
        void setM240Twist()
        {
            _type = TwistType.Constant;
            _length = 30;
            _inchPerRev = 12;
            _direction = -1;
        }
        void setM2Twist()
        {
            _type = TwistType.Constant;
            _length = 48;
            _inchPerRev = 15;
            _direction = -1;
        }
        public TwistProfile(double[] z, double[] theta, TwistType type)
        {
            int l = Math.Min(z.Length, theta.Length);
            _twist = new TwistValue[l];
            _direction = Math.Sign(theta[theta.Length - 1] - theta[0]);

            for (int i = 0; i < _twist.Length - 1; i++)
            {
                _twist[i].Z = z[i];
                _twist[i].ThetaRad = theta[i];
            }
            _type = type;

        }
    }
   
}
