using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolpathLib
{
    /// <summary>
    /// creates list of pathEnitiy objects from Mastercam NCI file
    /// </summary>
    class NCIFileParser
    {
       public int[] MiscIntegerArr;
       public double[] MiscRealArr;
       public int ProgNumber;
       public int SeqIncrement;
       public int StartNumber;
       public int ToolNumber;
       public int ToolDiamNumber;
       public int ToolLengthNumber;
       public double Nomfeedrate;
       public double XHome;
       public double YHome;
       public double ZHome;
       public double OffsetDist;
       public double ToolDiameter;
       public int OpCode;
       public int CutPathCount;
       public string FilePath;
       public string OutputFileName;
       public string InputFileName;
       public string Title;
       private double posFeedrate;
       bool eofFound;

        /// <summary>
        /// parse NCI into list of pathEntity objects
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        internal List<PathEntity> BuildPath(List<string> file)
        {
            List<PathEntity> path = new List<PathEntity>();
            int length = file.Count;
            int gBlock;
            string paramBlock = "";
            string[] paramArr;
            string[] splitter = new string[] {" "};
            for (int i = 0; i < length; i+=2)
            {
                if (file[i] != "")
                {
                    gBlock = Int32.Parse(file[i]);
                    paramBlock = file[i + 1];
                    paramArr = paramBlock.Split(splitter, StringSplitOptions.None);
                    switch (gBlock)
                    {
                        case 0: path.Add(linearMove(BlockType.Rapid, paramArr));
                            break;
                        case 1:path.Add(linearMove(BlockType.Linear, paramArr)); //linear move
                            break;
                        case 2: path.Add(arcMove(BlockType.CWArc, paramArr));//cw arc
                            break;
                        case 3: path.Add(arcMove(BlockType.CCWArc, paramArr));//ccw arc move
                            break;
                        case 4: path.Add(delay(BlockType.Delay, paramArr));//delay dwell
                            break;
                        case 11: path.Add(fiveAxisMove(BlockType.FiveAxis, paramArr));//5axis move
                            break;
                        case 1000:
                        case 1001:
                        case 1002: toolChange(gBlock, paramArr);//tool change
                            break;
                        case 1012: getMiscIntegers(paramArr);//misc integers
                            break;
                        case 1011: getMiscReals(paramArr);//misc reals
                            break;
                        case 1003: eof(paramArr);//eof
                            eofFound = true; 
                            break;
                        case 1050: //misc parameters
                            break;
                        default: //fallthrough 
                            break;

                    }//end switch      
                }
            }
            return path;
        }
        /// <summary>
        /// get misc integer values from NCI
        /// </summary>
        /// <param name="paramArr"></param>
        private void getMiscIntegers(string[] paramArr)
        {
            MiscIntegerArr = new int[paramArr.Length];
           
            for (int i = 0; i < paramArr.Length; i++)
            {
                string trimmed = paramArr[i].Trim();
                MiscIntegerArr[i] = int.Parse(trimmed);
            }
        }
        /// <summary>
        /// get misc real values from NCI
        /// </summary>
        /// <param name="paramArr"></param>
        private void getMiscReals(string[] paramArr)
        {
            MiscRealArr = new double[paramArr.Length];            
            for(int i=0;i<paramArr.Length;i++)
            {
                string trimmed = paramArr[i].Trim();
                MiscRealArr[i] = double.Parse(trimmed);
            }
        }
        /// <summary>
        /// parse a toolchange from NCI file
        /// </summary>
        /// <param name="gBlock"></param>
        /// <param name="paramArr"></param>
        private void toolChange(int gBlock,string[] paramArr)
        {
            if ((gBlock == 1001) || (gBlock == 1002))
            {
                this.ProgNumber = int.Parse(paramArr[0]);
                this.StartNumber = int.Parse(paramArr[1]);
                this.SeqIncrement = int.Parse(paramArr[2]);
                this.ToolNumber = int.Parse(paramArr[3]);
                this.ToolDiamNumber = int.Parse(paramArr[4]);
                this.ToolLengthNumber = int.Parse(paramArr[5]);
                this.Nomfeedrate = double.Parse(paramArr[8]);
                this.XHome = double.Parse(paramArr[13]);
                this.YHome = double.Parse(paramArr[14]);
                this.ZHome = double.Parse(paramArr[15]);
            }
        }
        /// <summary>
        /// parse a linear move from NCI file
        /// </summary>
        /// <param name="type"></param>
        /// <param name="ParamArr"></param>
        /// <returns></returns>
        private PathEntity linearMove(BlockType type, string[] ParamArr)
        { 
            LinePathEntity entity = new LinePathEntity(type);
            entity.Ccomp=(CComp)(int.Parse(ParamArr[0]));
            entity.EndPoint.X = double.Parse(ParamArr[1]);
            entity.EndPoint.Y = double.Parse(ParamArr[2]);
            entity.EndPoint.Z = double.Parse(ParamArr[3]);
            entity.JetTopPoint.X = double.Parse(ParamArr[1]);
            entity.JetTopPoint.Y = double.Parse(ParamArr[2]);
            entity.JetTopPoint.Z = double.Parse(ParamArr[3]);
            entity.Feedrate = positiveF(double.Parse(ParamArr[4]));
            entity.SurfNormal= new DrawingIO.Vector3(0,0,1);
            entity.JetVector= new DrawingIO.Vector3(0,0,1);    
            entity.ControlFlag=(CtrlFlag)(int.Parse(ParamArr[5]));
            
            

            return entity;
        }
        /// <summary>
        /// parse delay value from NCI file
        /// </summary>
        /// <param name="type"></param>
        /// <param name="ParamArr"></param>
        /// <returns></returns>
        private PathEntity delay(BlockType type, string[] ParamArr)
        {
            DelayPathEntity entity = new DelayPathEntity();
            entity.Delay = double.Parse(ParamArr[0]);//TODO check NCI file for params in delay block
            return entity;
        }
        /// <summary>
        /// output only positive feedrates
        /// </summary>
        /// <param name="feedrate"></param>
        /// <returns></returns>
        private double positiveF(double feedrate)
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
            return feedOut;
        }
        /// <summary>
        /// parse arc move from NCI file
        /// </summary>
        /// <param name="type"></param>
        /// <param name="ParamArr"></param>
        /// <returns></returns>
        private PathEntity arcMove(BlockType type, string[] ParamArr)
        {
            
            ArcPathEntity entity = new ArcPathEntity(type);
            entity.Ccomp = (CComp)(int.Parse(ParamArr[0]));
            entity.ArcPlane = (ArcPlane)(int.Parse(ParamArr[1]));
            if (entity.ArcPlane == ArcPlane.XY)
            {
                entity.EndPoint.X = double.Parse(ParamArr[2]);
                entity.EndPoint.Y = double.Parse(ParamArr[3]);
            }
            if (entity.ArcPlane == ArcPlane.XZ)
            {
                entity.EndPoint.X = double.Parse(ParamArr[2]);
                entity.EndPoint.Z = double.Parse(ParamArr[3]);
            }
            if (entity.ArcPlane == ArcPlane.YZ)
            {
                entity.EndPoint.Y = double.Parse(ParamArr[2]);
                entity.EndPoint.Z = double.Parse(ParamArr[3]);
            }
            entity.CenterPoint.X = double.Parse(ParamArr[4]);
            entity.CenterPoint.Y = double.Parse(ParamArr[5]);
            entity.CenterPoint.Z = double.Parse(ParamArr[6]);
            entity.Feedrate = positiveF(double.Parse(ParamArr[7]));
            entity.ControlFlag = (CtrlFlag)int.Parse(ParamArr[8]);
            int IarcFlag = int.Parse(ParamArr[9]);
            if (IarcFlag == 0)
                entity.FullArcFlag = false;
            else
                entity.FullArcFlag = true;
            return entity;

        }
        /// <summary>
        /// parse 5 axis move from NCI file
        /// </summary>
        /// <param name="type"></param>
        /// <param name="paramArr"></param>
        /// <returns></returns>
        private PathEntity fiveAxisMove(BlockType type, string[] paramArr)
        {
            LinePathEntity entity = new LinePathEntity(type);            
            entity.EndPoint.X = double.Parse(paramArr[0]);
            entity.EndPoint.Y = double.Parse(paramArr[1]);
            entity.EndPoint.Z = double.Parse(paramArr[2]);
            entity.JetTopPoint.X = double.Parse(paramArr[3]);
            entity.JetTopPoint.Y = double.Parse(paramArr[4]);
            entity.JetTopPoint.Z = double.Parse(paramArr[5]);
            entity.Feedrate = positiveF(double.Parse(paramArr[6]));
            entity.SurfNormal = new DrawingIO.Vector3(double.Parse(paramArr[9]), double.Parse(paramArr[10]), double.Parse(paramArr[11]));
            entity.JetVector = new DrawingIO.Vector3(entity.JetTopPoint.X - entity.EndPoint.X, entity.JetTopPoint.Y - entity.EndPoint.Y, entity.JetTopPoint.Z - entity.EndPoint.Z);
            entity.ControlFlag = (CtrlFlag)(int.Parse(paramArr[8]));          
            return entity;
        }
        private void eof(string[] paramArr)
        {

        }
    }

}
