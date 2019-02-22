using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolpathLib
{
    /// <summary>
    /// builds NC file from a modelPath or toolpath list
    /// </summary>
    public class NcFileBuilder
    {
        CNCMachineCode machine;
        int currentLineNumber;
        bool invertF;
        public NcFileBuilder(CNCMachineCode machine)
        {
            this.machine = machine;
            currentLineNumber = machine.StartingLineNumber;
        }
        
        public List<string>Build(ToolPath path,bool invertFeedrates,List<string>header)
        {
            invertF = invertFeedrates;
            var file = new List<string>();
            file.AddRange(header);
            foreach(PathEntity pe in path)
            {
                file.Add(buildLine(pe));
            }
            return file;
        }
        string buildLine(PathEntity pe)
        {
            string line = "";
            switch(pe.Type)
            {
                case BlockType.CCWArc:
                case BlockType.CWArc:
                    var ape = (ArcPathEntity)pe;
                    break;
                case BlockType.Command:
                    break;
                case BlockType.Comment:
                    break;
                case BlockType.Delay:
                    var dpe = (DelayPathEntity)pe;
                    line = addDelay(dpe.Delay);
                    break;
                case BlockType.FiveAxis:                  
                case BlockType.Linear:                   
                case BlockType.Rapid:
                    var lpe = (LinePathEntity)pe;

                    line = addMove(lpe);
                    break;
                case BlockType.Unknown:
                    break;
            }
            return line;
        }
        string addDelay(double delayinSeconds)
        {
            string delayString= machine.DelayAmountPrefix + (delayinSeconds * machine.DelayScaleFactor).ToString(machine.DelayStringFormat);
            return machine.DelayGcode + delayString;
        }
        string addComment(string comment)
        {
            if (comment.Length > machine.CommentMaxLength)
            {
                comment = comment.Substring(0, machine.CommentMaxLength);
            }
            return machine.ComStart + comment + machine.ComEnd;
             
        }
        string addMcode(string m,bool state)
        {
            //TODO return mcode from string and state
            return m;
           // return machine.getMcode(m, state);                        
        }
        List<string> addFooter()
        {
            List<string> footer = new List<string>();
            footer.Add(machine.EndofProg);
            return footer;
        }
        List<string> addHeader(string ProgName,string[] comments)
        {
            foreach(char badC in machine.ForbiddenChars)
            {
                ProgName.Replace(badC, '-');                
            }
            List<string> header = new List<string>();
            string line = machine.HeaderStart + ProgName + machine.HeaderEnd;
            header.Add(line);
           
            return header;
        }
        string miscCommand(string command)
        {
            foreach (char badC in machine.ForbiddenChars)
            {
                command.Replace(badC, '-');
            }
            return command;
        }
        void appendLineNumber(ref StringBuilder line)
        {
            line.Append(machine.N + machine.StartingLineNumber + machine.Sp);
        }
        void appendGCode(BlockType t, ref StringBuilder line)
        {
            if (invertF && (t== BlockType.Linear || t== BlockType.FiveAxis))
            {
                line.Append(machine.InverseFeedGcode + machine.Sp);
            }
            switch (t)
            {
                case BlockType.CCWArc:
                    line.Append(machine.CcwArcGcode + machine.Sp);
                    break;
                case BlockType.CWArc:
                    line.Append(machine.CwArcGcode + machine.Sp);
                    break;
                case BlockType.FiveAxis:
                case BlockType.Linear:
                    line.Append(machine.LinearMoveGcode + machine.Sp);
                    break;
                case BlockType.Rapid:
                    line.Append(machine.RapidGcode + machine.Sp);
                    break;               
            }          
        }
        void appendPositions(PathEntity pe,ref StringBuilder line)
        {
           
                    line.Append("X");
                    line.Append(pe.Position.X.ToString(machine.PFormat) + machine.Sp);
                    line.Append("Y");
                    line.Append(pe.Position.Y.ToString(machine.PFormat) + machine.Sp);
                    line.Append("Z");
                    line.Append(pe.Position.Z.ToString(machine.PFormat) + machine.Sp);
                    line.Append("B");
                    line.Append(pe.Position.Bdeg.ToString(machine.PFormat) + machine.Sp);
                    line.Append("C");
                    line.Append(pe.Position.Cdeg.ToString(machine.PFormat) + machine.Sp);
                   
           
        }
        void appendFeedrate(Feedrate f, ref StringBuilder line)
        {
            line.Append(machine.F + f.Value.ToString(machine.FFormat));
        }
        string addMove(PathEntity pe)
        {
            
            StringBuilder line = new StringBuilder();
            appendLineNumber(ref line);
            appendGCode(pe.Type,  ref line);
            appendPositions(pe, ref line);           
            if (pe.Type != BlockType.Rapid)
            {
                appendFeedrate(pe.Feedrate, ref line);
            }            
            
            return line.ToString();
        }       
    }
}
