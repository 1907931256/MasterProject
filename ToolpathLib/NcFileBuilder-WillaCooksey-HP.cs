using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolpathLib
{
    
    public class NCFileBuilder
    {
        CNCMachineCode machine;

        public NCFileBuilder(CNCMachineCode machine)
        {
            this.machine = machine;
        }
        

        public string Delay(double delayinSeconds)
        {
            string delayString= machine.DelayAmountPrefix + (delayinSeconds * machine.DelayScaleFactor).ToString(machine.DelayStringFormat);
            return machine.DelayGcode + delayString;
        }
        public string Comment(string comment)
        {
            if (comment.Length > machine.CommentMaxLength)
            {
                comment = comment.Substring(0, machine.CommentMaxLength);
            }
            return machine.ComStart + comment + machine.ComEnd;
             
        }
        public string Mcode(MCode m)
        {
            return machine.getMcode(m);                        
        }
        public List<string> Footer()
        {
            List<string> footer = new List<string>();
            footer.Add(machine.EndofProg);
            return footer;
        }
        public List<string> Header(string ProgName,string[] comments)
        {
            foreach(char badC in machine.ForbiddenChars)
            {
                ProgName.Replace(badC, '-');                
            }
            List<string> header = new List<string>();
            string line = machine.HeaderStart + ProgName + machine.HeaderEnd;
            header.Add(line);
            foreach (string comment in comments)
            {
                header.Add(Comment(comment));
            }
            return header;
        }
        public string MiscCommand(string command)
        {
            foreach (char badC in machine.ForbiddenChars)
            {
                command.Replace(badC, '-');
            }
            return command;
        }
        public string LinearMove(bool invertFeed,bool rapid, double f, params double[] positions)
        {
            StringBuilder line = new StringBuilder();
            if (positions.Length == machine.AxisCount)
            {
                line.Append(machine.N + machine.StartingLineNumber + machine.Sp);
                if (rapid)
                {
                    line.Append(machine.RapidGcode + machine.Sp);
                }
                else
                {
                    if (invertFeed)
                    {
                        line.Append(machine.InverseFeedGcode + machine.Sp + machine.LinearMoveGcode + machine.Sp);
                    }
                    else
                    {
                        line.Append(machine.LinearMoveGcode + machine.Sp);
                    }
                }
                for (int i = 0; i < positions.Length; i++)
                {
                    line.Append(machine.AxisNames[i] + positions[i].ToString(machine.PFormat) + machine.Sp);
                }
                if (!rapid)
                {
                    line.Append(machine.F + f.ToString(machine.FFormat));
                }
            }
            machine.StartingLineNumber += machine.LineNIndex;
            return line.ToString();
        }
    }
}
