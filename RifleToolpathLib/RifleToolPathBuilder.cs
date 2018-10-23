using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FileIOLib;
using AWJModel;
using InspectionLib;

namespace RifleToolpathLib
{
    public class ToolpathBuildOptions
    {
        public string ProgramTitle;
        public string JetOnFilename;
        public string JetOffFilename;
        public string PreJetOffHeaderFilename;
        public string PreJetOnHeaderFilename;
        public string MoveFileName;
        public string ProgramGenFilename;        
        public string  RotAxisLabel;
        public string  LinAxisLabel;
        public string  AbsMoveLabel;
        public string   RelMoveLabel;
        public string   RapidMoveLabel;
        public string   LinMoveLabel;
        public string FeedRateLabel;
        public string InvertFeedLabel;
        public string LineNumLabel;
        public bool TaperedBarrel;
        public bool NominalStartingLandDiam;
        public bool NominalGrooveDepth;
        public bool LinearSpeedInterpol;
        public bool RasterMethod;
        public bool SimuAllGrooves;
        public bool ClampAtMaxRotarySpeed;
        public bool WarnAtMaxRotarySpeed;
        public bool InvertFeedrates;
        public bool RelativeMoves;
        
        public int DepthProfileCt;
        public int RasterPerGroove;
        public int TargetPassCt;
        public int InitPassCt;
        public int FirstLineNum;
        public int LineNumIndex;
        public int LineNumStart;
        public int[] GrooveList;

        public double MaxMachineRPM;
        public double InitDepthFraction;
        public double LinAxisMoveLen;
        public double Raster2EndAngle;
        public double Raster1StartAngle;
        public double GrooveStartLinLocation;
        public double GrooveEndLinLocation;

        public ToolpathBuildOptions()
        {
           
            JetOffFilename = "rifle_8055_jetoff.txt";
            JetOnFilename = "rifle_8055_jeton.txt";
            PreJetOffHeaderFilename = "rifle_8055_prejetoff_footer.txt";
            PreJetOnHeaderFilename = "rifle_8055_prejeton_footer.txt";
            MoveFileName = "rifle_8055_move.txt";
            ProgramGenFilename = "rifle_8055_progGen.txt";

            ProgramTitle = "50CAL_SN027";
            RotAxisLabel = "A";
            LinAxisLabel = "X";
            AbsMoveLabel = "G90 ";
            RelMoveLabel = "G91 ";
            RapidMoveLabel = "G00 ";
            LinMoveLabel = "G01 ";
            FeedRateLabel = "F";
            InvertFeedLabel = "G32 ";
            LineNumLabel = "N";

            TaperedBarrel = true;
            NominalStartingLandDiam = false;
            NominalGrooveDepth = false;
            LinearSpeedInterpol = true;
            RasterMethod = true;
            SimuAllGrooves = true;
            ClampAtMaxRotarySpeed = true; ;
            WarnAtMaxRotarySpeed = true; ;
            InvertFeedrates = true; ;
            RelativeMoves = true; ;

            DepthProfileCt =2;
            RasterPerGroove=16;
            TargetPassCt=4;
            InitPassCt=2;
            FirstLineNum=100;
            LineNumIndex=2;

            MaxMachineRPM=16;
            InitDepthFraction=0.5;
            LinAxisMoveLen=0.5;
            Raster2EndAngle=24.645;
            Raster1StartAngle=0.0;
            GrooveStartLinLocation=1.0;
            GrooveEndLinLocation=46.0;
            
        }
    }
    public class ToolpathFuncLib
    {
       
        ToolpathBuildOptions tpb;

        public string GrooveStartAngle()
        {
            return tpb.GrooveStartLinLocation.ToString("f4");
        }
        public string Time()
        {
            return System.DateTime.Now.ToShortTimeString();
        }
        public string Date()
        {
            return System.DateTime.Now.ToShortDateString();
        }
        public string  NextIndex()
        {
            _numCurrent += tpb.LineNumIndex;
            return tpb.LineNumLabel + _numCurrent.ToString();
        }
        public string ProgramTitle()
        {
            return tpb.ProgramTitle;
        }
        public string RotAxisLabel()
        {
            return tpb.RotAxisLabel;
        }
        public string LinAxisLabel()
        {
            return tpb.LinAxisLabel;
        }
        public string LinMoveLabel()
        {
            return tpb.LinMoveLabel;
        }
        public string RapidMoveLabel()
        {
            return tpb.RapidMoveLabel;
        }
        public string AbsMovelLabel()
        {
            return tpb.AbsMoveLabel;
        }
        public string RelMoveLabel()
        {
            return tpb.RelMoveLabel;
        }
        public string FeedRateLabel()
        {
            return tpb.FeedRateLabel;
        }
        public string InvFeedLabel()
        {
            return tpb.InvertFeedLabel;
        }
        public string Move()
        {
            return NextIndex();
        }
        int _numCurrent;
        void SetValues()
        {
            tpb = new ToolpathBuildOptions();
            _numCurrent = tpb.LineNumStart;
        }
        public ToolpathFuncLib()
        {
            SetValues();
        }
    }
    //class LinFitCoeffs
    //{
    //    public double Slope
    //    {
    //        get;set;
    //    }
    //    public double Intercept
    //    {
    //        get;set;
    //    }
    //    public LinFitCoeffs(double intercept,double slope)
    //    {
    //        Slope = slope;
    //        Intercept = intercept;
    //    }
         
    //}
    //class LinearFit
    //{       
    //    static public LinFitCoeffs GetCoeffs(double[] x, double[] y)
    //    {            
    //        var slopeIntercept = MathNet.Numerics.Fit.Line(x, y);            
    //        var lfc = new LinFitCoeffs(slopeIntercept.Item1,slopeIntercept.Item2);
    //        return lfc;
    //    }        
    //}
    public class RifleCNCFileBuilder
    {
        Dictionary<string, System.Reflection.MethodInfo> dictionary;

        void BuildFunctionDictionary()
        {

            dictionary = new Dictionary<string, System.Reflection.MethodInfo>();
            var m = typeof(ToolpathFuncLib).GetMethods();
            foreach (System.Reflection.MethodInfo mi in m)
            {
                var pi = mi.GetParameters();
                dictionary.Add(mi.Name.ToUpper(), mi);
            }
        }

        void ParseFile(string formatFlename, string outputFilename)
        {
            try
            {
                char[] delim = new char[2] { '$', '!' };
                var toolpathFuncLib = new ToolpathFuncLib();

                var lines = FileIO.ReadDataTextFile(formatFlename);
                var linesOut = new List<string>();
                foreach (string line in lines)
                {
                    string lineOut = "";
                    if (line.IndexOfAny(delim) >= 0)
                    {
                        var words = line.Split(delim);
                        foreach (string word in words)
                        {
                            System.Reflection.MethodInfo mi;
                            if (dictionary.TryGetValue(word, out mi))
                            {
                                string s = (string)mi.Invoke(toolpathFuncLib, new object[] { });
                                lineOut += s;
                            }
                            else
                            {
                                lineOut += word;
                            }
                        }
                    }
                    else
                    {
                        lineOut = line;
                    }

                    linesOut.Add(lineOut);
                }
                FileIO.Save(linesOut.ToArray(), outputFilename);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void BuildOutputFile(string formatFilename, string outputFilename)
        {
            ParseFile(formatFilename, outputFilename);
        }
        public RifleCNCFileBuilder()
        {
            BuildFunctionDictionary();
        }
    }
    public class RifleToolpathBuilder
    {
            List<MachineRasterSpeeds> _machSpeeds;
            List<GrooveDepthProfile> _depthMeasurements;
            BarrelProfile _barrelProfile;
            int _rasterCount;
            int _depthMeasCount;

            void CheckRasterCounts()
            {
                try
                {

                    foreach (var ms in _machSpeeds)
                    {
                        _rasterCount = ms.RasterCount;

                        foreach (var dm in _depthMeasurements)
                        {
                            _depthMeasCount = dm.RasterCount;
                            if (dm.RasterCount != _rasterCount)
                            {
                                throw new Exception("Raster Counts in depth measurements and machine speed files do not match.");
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }

            }

            void BuildNextSpeedSets()
            {
                try
                {

                    for (int d = 0; d < _depthMeasurements.Count; d++)
                    {
                        for (int i = 0; i < _rasterCount; i++)
                        {
                            var vNext = _depthMeasurements[d][i].Depth * _machSpeeds[d][i].CurrentSpeed / (_machSpeeds[d][i].TargetDepth - _depthMeasurements[d][i].Depth);
                            _machSpeeds[d][i].NextSpeed = vNext;
                        }
                    }

                }
                catch (Exception)
                {

                    throw;
                }
            }
        class RifleSpeed
        {
            public double Speed { get; set; }
            public double InvSpeed { get; set; }
            public double RPM { get; set; }
        }
        RifleSpeed[,] _speedArr;
            

        void BuildSpeedArray(double maxRPM, bool adjustSpeeds)
        {
                _speedArr = new RifleSpeed[_barrelProfile.Count, _rasterCount];
               
                var dRange = _barrelProfile[_barrelProfile.Count - 1].TargetGrooveDepth - _barrelProfile[0].TargetGrooveDepth;
                for (int xi = 0; xi < _barrelProfile.Count; xi++)
                {
                    for (int ri = 0; ri < _rasterCount; ri++)
                    {
                        var dV = _machSpeeds[1][ri].NextSpeed - _machSpeeds[0][ri].NextSpeed;
                        var dD = _barrelProfile[xi].TargetGrooveDepth - _barrelProfile[0].TargetGrooveDepth;
                        _speedArr[xi, ri].Speed = _machSpeeds[0][ri].NextSpeed + dD * dV / dRange;
                        if (_barrelProfile[xi].SegLen != 0)
                        {
                            _speedArr[xi, ri].InvSpeed = _speedArr[xi, ri].Speed / _barrelProfile[xi].SegLen;
                        }
                        double rpm = _speedArr[xi, ri].InvSpeed * _barrelProfile[xi].DeltaA / 360.0;
                        if(rpm > maxRPM && adjustSpeeds)
                        {
                            _speedArr[xi, ri].RPM = maxRPM;
                            _speedArr[xi, ri].InvSpeed = maxRPM * 360 / _barrelProfile[xi].DeltaA;
                            _speedArr[xi, ri].Speed = _speedArr[xi, ri].InvSpeed * _barrelProfile[xi].SegLen;
                        }
                        else
                        {
                            _speedArr[xi, ri].RPM = rpm;
                        }
                    }
                }

        }
       
        public void SaveAllArrays(string filename)
        {
                var speedList = new List<string>();
                var invSpeedList = new List<string>();
                var rpmList = new List<string>();
                speedList.Add("Speeds");
                invSpeedList.Add("Inverse Speeds");
                rpmList.Add("Rpms");

                for (int xi = 0; xi < _barrelProfile.Count; xi++)
                {
                    string speeds = xi.ToString() + ",";
                    string invSpeeds = xi.ToString() + ",";
                    string rpms = xi.ToString() + ",";
                    for (int ri = 0; ri < _rasterCount; ri++)
                    {
                        speeds += _speedArr[xi, ri].Speed.ToString("f3") + ",";
                        invSpeeds += _speedArr[xi, ri].InvSpeed.ToString("f3") + ",";
                        rpms += _speedArr[xi, ri].RPM.ToString("f3") + ",";
                    }
                    speedList.Add(speeds);
                    invSpeedList.Add(invSpeeds);
                    rpmList.Add(rpms);
                }
                var lines = new List<string>();
                lines.AddRange(speedList);
                lines.AddRange(invSpeedList);
                lines.AddRange(rpmList);
                FileIO.Save(lines.ToArray(), filename);
        }
        public void SaveSpeedArrays(string filename)
        {
                int rasterDirection = -1;
                var lines = new List<string>();
                for (int ri = 0; ri < _rasterCount; ri++)
                {
                    lines.Add(";START RASTER_" + (ri + 1).ToString());
                    if (rasterDirection > 0)
                    {
                        for (int xi = 0; xi < _barrelProfile.Count; xi++)
                        {
                            lines.Add("F" + _speedArr[xi, ri].InvSpeed.ToString("f3"));
                        }
                    }
                    else
                    {
                        for (int xi = _barrelProfile.Count - 1; xi >= 0; xi--)
                        {
                            lines.Add("F" + _speedArr[xi, ri].InvSpeed.ToString("f3"));
                        }
                    }
                    lines.Add(";END RASTER_" + (ri + 1).ToString());
                    rasterDirection *= -1;

                }
                FileIO.Save(lines.ToArray(), filename);
        }
        public void BuildPath(List<MachineRasterSpeeds> machineSpeedList, List<GrooveDepthProfile> depthMeasurements, BarrelProfile barrelProfile,double maxRPM,bool adjustSpeeds)
        {
                _machSpeeds = machineSpeedList;
                _depthMeasurements = depthMeasurements;
                _barrelProfile = barrelProfile;
                CheckRasterCounts();
                BuildNextSpeedSets();
                BuildSpeedArray(maxRPM,adjustSpeeds);

        }
    }
}

