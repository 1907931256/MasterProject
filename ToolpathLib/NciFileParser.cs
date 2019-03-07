using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolpathLib
{
    /// <summary>
    /// creates list of pathEnitiy objects from Mastercam NCI file
    /// </summary>
    class NciFileParser
    {
        ToolPath5Axis toolpath;
        private double posFeedrate;
        

        internal NciFileParser()
        {
            toolpath = new ToolPath5Axis();
        }
 
        internal ToolPath5Axis ParsePath(List<string> file)
        {
            
            int length = file.Count;
            int gBlock;
            string paramBlock = "";
            string[] paramArr;
            string[] splitter = new string[] {" "};
            PathEntity5Axis pe;
            for (int i = 0; i < length; i+=2)
            {
                if (file[i] != "")
                {
                    gBlock = Int32.Parse(file[i]);
                    paramBlock = file[i + 1];
                    paramArr = paramBlock.Split(splitter, StringSplitOptions.None);
                    switch (gBlock)
                    {
                        case 0:
                            pe = parseLine(BlockType.Rapid, paramArr);
                            AddEntity(pe);
                            break;
                        case 1:
                            pe = parseLine(BlockType.Linear, paramArr);
                            AddEntity(pe);
                            //linear move
                            break;
                        case 2:pe = parseArc(BlockType.CWArc, paramArr);
                            AddEntity(pe);//cw arc
                            break;
                        case 3:
                            pe = parseArc(BlockType.CCWArc, paramArr);
                            AddEntity(pe);//ccw arc move
                            break;
                        case 4:
                            pe = parseDelay(BlockType.Delay, paramArr);
                            AddEntity(pe);//delay dwell
                            break;
                        case 11:
                            pe = parseFiveAxis(BlockType.FiveAxis, paramArr);
                            AddEntity(pe);
                            //5axis move                            
                            break;
                    }
                    
                    switch (gBlock)
                    {                        
                        case 1000:
                        case 1001:
                        case 1002: parseToolChange(gBlock, paramArr);//tool change
                            break;
                        case 1012: parseMiscIntegers(paramArr);//misc integers
                            break;
                        case 1011: parseMiscReals(paramArr);//misc reals
                            break;
                        case 1003: eof(paramArr);//eof                             
                            break;
                        case 1050: //misc parameters
                            break;
                        default: //fallthrough 
                            break;

                    }//end switch      
                }
            }
            return toolpath;
        }
        private void AddEntity(PathEntity5Axis pe)
        {
            if(toolpath.Count>0)
            {
                pe.PrevPosition = new CNCLib.XYZBCMachPosition(toolpath.Last().Position);
            }
            else
            {
                pe.PrevPosition = new CNCLib.XYZBCMachPosition(pe.Position);
            }            
            toolpath.Add(pe);
        }
        private void parseMiscIntegers(string[] paramArr)
        {
            toolpath.MiscIntegerArr = new int[paramArr.Length];
           
            for (int i = 0; i < paramArr.Length; i++)
            {
                string trimmed = paramArr[i].Trim();
                toolpath.MiscIntegerArr[i] = int.Parse(trimmed);
            }
        }
        
        private void parseMiscReals(string[] paramArr)
        {
            toolpath.MiscRealArr = new double[paramArr.Length];            
            for(int i=0;i<paramArr.Length;i++)
            {
                string trimmed = paramArr[i].Trim();
                toolpath.MiscRealArr[i] = double.Parse(trimmed);
            }
        }
        
        private void parseToolChange(int gBlock,string[] paramArr)
        {
            if ((gBlock == 1001) || (gBlock == 1002))
            {
                toolpath.ProgNumber = int.Parse(paramArr[0]);
                toolpath.StartNumber = int.Parse(paramArr[1]);
                toolpath.SeqIncrement = int.Parse(paramArr[2]);
                toolpath.ToolNumber = int.Parse(paramArr[3]);
                toolpath.ToolDiamNumber = int.Parse(paramArr[4]);
                toolpath.ToolLengthNumber = int.Parse(paramArr[5]);
                toolpath.NomFeedrate = double.Parse(paramArr[8]);
                toolpath.XHome = double.Parse(paramArr[13]);
                toolpath.YHome = double.Parse(paramArr[14]);
                toolpath.ZHome = double.Parse(paramArr[15]);
            }
        }
        /// <summary>
        /// parse a linear move from NCI file
        /// </summary>
        /// <param name="type"></param>
        /// <param name="ParamArr"></param>
        /// <returns></returns>
        private PathEntity5Axis parseLine(BlockType type, string[] ParamArr)
        { 
            LinePathEntity entity = new LinePathEntity(type);
            entity.Type = type;
            entity.Ccomp=(CComp)(int.Parse(ParamArr[0]));
            entity.Position.X = double.Parse(ParamArr[1]);
            entity.Position.Y = double.Parse(ParamArr[2]);
            entity.Position.Z = double.Parse(ParamArr[3]);
            double xTop = double.Parse(ParamArr[1]);
            double yTop = double.Parse(ParamArr[2]);
            double zTop = double.Parse(ParamArr[3]);
            entity.Feedrate.Value = getPositiveF(double.Parse(ParamArr[4]));
            if (entity.Feedrate.Value == -2)
            {
                entity.Type = BlockType.Rapid;
                entity.RapidMove = true;
            }
            else
            {
                entity.RapidMove = false;
            }
            entity.SurfNormal= new GeometryLib.Vector3(0,0,1);
            entity.JetVector= new GeometryLib.Vector3(xTop - entity.Position.X,yTop- entity.Position.Y,zTop-entity.Position.Z);    
            entity.ControlFlag=(CtrlFlag)(int.Parse(ParamArr[5]));
            entity.JetOn = getJetOn(entity.Feedrate.Value, toolpath.NomFeedrate);
           
            entity.InputString = "X" + entity.Position.X.ToString() + 
                                  "Y"+ entity.Position.Y.ToString() + 
                                  "Z" + entity.Position.Z.ToString() + 
                                  "F" + entity.Feedrate.Value.ToString();

            return entity;
        }
        private bool getJetOn(double feedrate, double nomFeedrate)
        {
            if(feedrate == nomFeedrate)
                return true;
            else
                return false;

        }
      
        private PathEntity5Axis parseDelay(BlockType type, string[] ParamArr)
        {
            DelayPathEntity entity = new DelayPathEntity();
            entity.Type = type;
            entity.Delay = double.Parse(ParamArr[0]);//TODO check NCI file for params in delay block
            return entity;
        }
      
        private double getPositiveF(double feedrate)
        {
            double feedOut = 0;
            if (feedrate > 0)
            {
                 posFeedrate=feedrate;
                 feedOut = feedrate;
            }
            if (feedrate == -1)
            {
                feedrate = posFeedrate;
                feedOut = posFeedrate;
            }
            if(feedrate == -2)
            {
                feedOut = feedrate;
            }
            return feedOut;
        }
      
        private PathEntity5Axis parseArc(BlockType type, string[] ParamArr)
        {
            
            ArcPathEntity entity = new ArcPathEntity(type);
            entity.Type = type;
            entity.Ccomp = (CComp)(int.Parse(ParamArr[0]));
            entity.ArcPlane = (ArcPlane)(int.Parse(ParamArr[1]));
            if (entity.ArcPlane == ArcPlane.XY)
            {
                entity.Position.X = double.Parse(ParamArr[2]);
                entity.Position.Y = double.Parse(ParamArr[3]);
            }
            if (entity.ArcPlane == ArcPlane.XZ)
            {
                entity.Position.X = double.Parse(ParamArr[2]);
                entity.Position.Z = double.Parse(ParamArr[3]);
            }
            if (entity.ArcPlane == ArcPlane.YZ)
            {
                entity.Position.Y = double.Parse(ParamArr[2]);
                entity.Position.Z = double.Parse(ParamArr[3]);
            }
            entity.CenterPoint.X = double.Parse(ParamArr[4]);
            entity.CenterPoint.Y = double.Parse(ParamArr[5]);
            entity.CenterPoint.Z = double.Parse(ParamArr[6]);
            entity.Feedrate.Value = getPositiveF(double.Parse(ParamArr[7]));
            entity.ControlFlag = (CtrlFlag)int.Parse(ParamArr[8]);
            entity.ArcType = ArcSpecType.NCI;
            int IarcFlag = int.Parse(ParamArr[9]);
            if (IarcFlag == 0)
                entity.FullArcFlag = false;
            else
                entity.FullArcFlag = true;
            entity.JetOn = getJetOn(entity.Feedrate.Value, toolpath.NomFeedrate);
            return entity;

        }
       
        private PathEntity5Axis parseFiveAxis(BlockType type, string[] paramArr)
        {
            LinePathEntity entity = new LinePathEntity(type);
            entity.Type = type;
            entity.Position.X = double.Parse(paramArr[0]);
            entity.Position.Y = double.Parse(paramArr[1]);
            entity.Position.Z = double.Parse(paramArr[2]);
            double xTop = double.Parse(paramArr[3]);
            double yTop = double.Parse(paramArr[4]);
            double zTop = double.Parse(paramArr[5]);
            entity.Feedrate.Value = getPositiveF(double.Parse(paramArr[6]));
            if(entity.Feedrate.Value==-2)
            {
                entity.Type = BlockType.Rapid;
                entity.RapidMove = true;
            }
            else
            {
                entity.RapidMove = false;
            }            
            entity.SurfNormal = new GeometryLib.Vector3(double.Parse(paramArr[9]), double.Parse(paramArr[10]), double.Parse(paramArr[11]));
            entity.JetVector = new GeometryLib.Vector3(xTop - entity.Position.X, yTop - entity.Position.Y, zTop - entity.Position.Z);
            entity.ControlFlag = (CtrlFlag)(int.Parse(paramArr[8]));
            entity.JetOn = getJetOn(entity.Feedrate.Value, toolpath.NomFeedrate);
            entity.Position.Bdeg =GeometryLib.GeomUtilities.ToDegs( Math.Acos(entity.JetVector.Z / entity.JetVector.Length));
            entity.Position.Cdeg = GeometryLib.GeomUtilities.ToDegs(Math.Atan2(entity.JetVector.Y, entity.JetVector.X));
            return entity;
        }
        private void eof(string[] paramArr)
        {

        }
    }

}
