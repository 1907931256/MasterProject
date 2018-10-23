using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolpathLib
{
    public class NCFileParser
    {
        
        string[] g0;
        string[] g1;
        string[] g2;
        string[] g3;
        string[] g4;

        string n;
        string g32;
        string g;
        string x;
        string y;
        string z;
        string i;
        string j;
        string k;
        string a;
        string b;
        string c;
        string m;
        string f;

        string[] splitters;
        double toolDiam;
        int toolOffsetNumber;
        int toolNumber;
        string[] pathGCodes;
        public NCFileParser()
        {
  
            splitters = new string[] {" "};
            pathGCodes = new string[] { "G0", "G00", "G1", "G01", "G2", "G02", "G3", "G03", "G4", "G04" };
            n="N";
            g32="G32";
            g="G";
            x="X";
            y="Y";
            z="Z";
            i="I";
            j="J";
            k="K";
            a="A";
            b="B";
            c="C";
            m="M";
            f="F";
        }
        /// <summary>
        /// return list of path entities from file as string array
        /// </summary>
        /// <param name="file">string array of file lines</param>
        /// <returns>list of path entities</returns>
        internal List<PathEntity> BuildPath(List<string> file)
        {
            List<PathEntity> path = new List<PathEntity>();


            foreach (string line in file)
            {
                BlockType blockT = BlockType.Unknown;
                if (isPath(line, out blockT))
                {
                    switch (blockT)
                    {
                        case BlockType.Rapid:
                            path.Add(parseLine(line, blockT));
                            break;

                        case BlockType.Linear:
                            path.Add(parseLine(line, blockT));
                            break;

                        case BlockType.CCWArc:
                            path.Add(parseArc(line, blockT));
                            break;

                        case BlockType.CWArc:
                            path.Add(parseArc(line, blockT));
                            break;
                    }
                }

            }

            return path;
        }
        bool containsPathCode(string s, out string gcode)
        {
            s = s.ToUpper();
            gcode = "";
            foreach (string code in pathGCodes)
            {
                if (s.Contains(code))
                {
                    gcode = code;
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// return true and blocktype if line is a g-code line
        /// </summary>
        /// <param name="ncLine">line from program</param>
        /// <param name="blockT">block type</param>
        /// <returns>true if line is part of path</returns>
        private bool isPath(string ncLine, out BlockType blockT)
        {
            int iResult;
            bool isAPath = false;
            string[] splitLine = ncLine.Split(splitters, StringSplitOptions.None);
            string trimStr = "";
            blockT = BlockType.Unknown;
            foreach (string splitStr in splitLine)
            {
                if (containsPathCode(splitStr, out g))
                {
                    trimStr = splitStr.Substring(splitStr.IndexOf(g) + 1);
                    trimStr = trimStr.Trim();
                    if (Int32.TryParse(trimStr, out iResult))
                    {
                        switch (iResult)
                        {
                            case 0:
                                isAPath = true;
                                blockT = BlockType.Rapid;
                                break;

                            case 1:
                                isAPath = true;
                                blockT = BlockType.Linear;
                                break;

                            case 2:
                                isAPath = true;
                                blockT = BlockType.CWArc;
                                break;

                            case 3:
                                isAPath = true;
                                blockT = BlockType.CCWArc;
                                break;

                            case 4:
                                isAPath = true;
                                blockT = BlockType.Delay;
                                break;
                        }
                    }
                }
            }
            return isAPath;
        }
        /// <summary>
        /// parse string line from nc file into path ARC entity
        /// </summary>
        /// <param name="ncLine">line from NC file</param>
        /// <param name="blockT">Block type</param>
        /// <returns>ArcPathEntity</returns>
        ArcPathEntity parseArc(string ncLine, BlockType blockT)
        {
            ArcPathEntity ent = new ArcPathEntity(blockT);
            string[] splitLine = ncLine.Split(splitters, StringSplitOptions.None);

            foreach (string str in splitLine)
            {
                if (str.Contains(n))
                {
                    ent.PathNumber = parseInt(str, n);
                }
                if (str.Contains(x))
                {
                    ent.EndPoint.X = parseDouble(str, x);
                }
                if (str.Contains(y))
                {
                    ent.EndPoint.Y = parseDouble(str, y);
                }
                if (str.Contains(z))
                {
                    ent.EndPoint.Z = parseDouble(str, z);
                }
                if (str.Contains(i))
                {
                    ent.I = parseDouble(str, i);
                }
                if (str.Contains(j))
                {
                    ent.J = parseDouble(str, j);
                }
                if (str.Contains(k))
                {
                    ent.K = parseDouble(str, k);
                }
                if (str.Contains(f))
                {
                    ent.Feedrate = parseDouble(str, f);
                }
            }
            ent.Type = blockT;
            return ent;
        }
        int parseInt(string line,string word)
        {
            int r = 0;

            if (Int32.TryParse(line.Substring((line.IndexOf(word) + 1)), out r))
                return r;
            else
                return 0;
        }
        double parseDouble(string line, string word)
        {
            double r = 0;

            if (double.TryParse(line.Substring((line.IndexOf(word) + 1)), out r))
                return r;
            else
                return 0;
        }
        /// <summary>
        /// parse string line from nc file into path LINE entity
        /// </summary>
        /// <param name="ncLine">line from NC file</param>
        /// <param name="blockT">Block type</param>
        /// <returns>LinePathEntity</returns>
        LinePathEntity parseLine(string ncLine, BlockType blockT)
        {
            LinePathEntity ent = new LinePathEntity(blockT);
            string[] splitLine = ncLine.Split(splitters, StringSplitOptions.None);
            foreach (string str in splitLine)
            {
                if (str.Contains(n))
                {                    
                        ent.PathNumber = parseInt(str,n);
                }
                if (str.Contains(x))
                {                   
                        ent.EndPoint.X = parseDouble(str,x);
                }
                if (str.Contains(y))
                {                 
                        ent.EndPoint.Y = parseDouble(str, y);
                }
                if (str.Contains(z))
                {
                    ent.EndPoint.Z = parseDouble(str, z);
                }
                if (str.Contains(a))
                {                   
                        ent.A = parseDouble(str,a);
                        ent.Type = BlockType.FiveAxis;                   
                }
                if (str.Contains(b))
                {                  
                        ent.B = parseDouble(str,b);
                        ent.Type = BlockType.FiveAxis;                    
                }
                if (str.Contains(c))
                {                   
                        ent.C = parseDouble(str,c);
                        ent.Type = BlockType.FiveAxis;                    
                }
                if (str.Contains(f))
                {                  
                        ent.Feedrate = parseDouble(str,f);                    
                }


            }

            if (ent.Type == BlockType.FiveAxis)
            {
                DrawingIO.Vector3 pt = new DrawingIO.Vector3(0, 0, 1);
                DrawingIO.Vector3 origin = new DrawingIO.Vector3(0, 0, 0);
                pt.RotateXY(origin, ent.B);
            }
            else
            {
                ent.JetVector = DrawingIO.Vector3.ZAxis;
            }
            ent.Type = blockT;
            return ent;
        }
    }
    public class NCFile
    {
        
        
    }
}
