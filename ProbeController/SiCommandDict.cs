using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SICommands
{
    public class SiErrorCode
    {
        
        int _code;
        string errorDescription;
        bool systemError;
    }
    
    public class SiCommandDict:Dictionary<string,SiCommand>
    {
        public SiCommandDict()
        {

            Add( "CommMode" , new SiCommand("CommMode", "Q0",new int[]{51}));
            Add("GeneralMode" ,new SiCommand("GeneralMode", "R0", new int[] { 51,63,65,66 }));
            Add("MeasureSingle" , new SiCommand("MeasureSingle", "MS","a","h", new int[] { 64 }));
            Add("MeasureMulti" , new SiCommand(  "MeasureMulti","MM","i","h", new int[] { 62,64 }));
            Add("MeasureAll " , new SiCommand("MeasureAll", "MA","h", new int[] { 0 }));
            Add("TimingSingle " , new SiCommand("TimingSingle", "TS","p","a", new int[] { 62,64}));
            Add("TimingMulti " , new SiCommand("TimingMulti", "TM","p","i", new int[] { 62,64}));
            Add("TimingSyncOFF " , new SiCommand("TimingSyncOFF", "T0", new int[] {62}));
            Add("TimingSyncOn " , new SiCommand("TimingSyncOn", "T1", new int[] {62 }));
            Add("AutoZeroOnSingle " , new SiCommand("AutoZeroOnSingle", "VS","a", new int[] { 64 }));
            Add("AutoZeroOnMulti " , new SiCommand("AutoZeroOnMulti", "VM","i", new int[] { 64 }));
            Add("AutoZeroOnSync " , new SiCommand("AutoZeroOnSync", "VA", new int[] { 62 }));
            Add("AutoZeroOffSingle " , new SiCommand("AutoZeroOffSingle", "WS","a", new int[] { 64 }));
            Add("AutoZeroOffMulti " , new SiCommand("AutoZeroOffMulti", "WM","i", new int[] { 64 }));
            Add("AutoZeroOffSync " , new SiCommand("AutoZeroOffSync", "WA", new int[] {62 }));
            Add("MeasuredValResetSingle " , new SiCommand("MeasuredValResetSingle", "DS",new int[] { 64 }));
            Add("MeasuredValResetMulti " , new SiCommand("MeasuredValResetMulti", "DM", new int[] { 64 }));
            Add("MeasuredValResetSync " , new SiCommand("MeasuredValResetSync", "DA", new int[] { 0 }));            
             Add("ProgramSwitch " , new SiCommand("ProgramSwitch", "PW", new int[] { 62 }));
             Add("ProgramConfirm " , new SiCommand("ProgramConfirm", "PR", "o", new int[] { 51 }));
             Add("DataStoreStart " , new SiCommand("DataStoreStart", "AS", new int[] { 51 }));
             Add("DataStoreStop " , new SiCommand("DataStoreStop", "AP", new int[] { 51 }));
             Add("DataStoreInit " , new SiCommand("DataStoreInit", "AQ", new int[] { 51 }));
             Add("DataStoreOut " , new SiCommand("DataStoreOut", "AO","h", new int[] { 51 }));
             Add("DataStoreStatus " , new SiCommand("DataStoreStatus", "AN","s","d", new int[] { 0 }));
          
        }
    }

   
    public class SiCommand
    {
        protected string _command;       
        protected char[] output;
        protected string _description;
      
        protected char[] _commandOut;
        char[] cr;
        int[] _errorCodes;
        protected string _param1Format;
        protected string _param2Format;
        protected List<int> errorCodes;

        public char[] Response { get { return _response; } set { _response = value; } }
        char[] _response;

        public char[] Command()
        {
            return _commandOut;
        }
        public char[] Command(int param)
        {
            
            string output = _command + "," + param;
            char[] comMode = output.ToCharArray();
            _commandOut = comMode.Concat(cr).ToArray();
            return _commandOut;
        }

        public char[] Command(string param1,string param2)
        {
            string output = _command + "," + param1 + "," + param2;
            char[] comMode = output.ToCharArray();
            _commandOut = comMode.Concat(cr).ToArray();
            return _commandOut;
        }
        
        public SiCommand (string description,string command,int[] errorCodes)
        {
            _command = command;
            _param2Format = null;
            _param1Format = null;
            _description = description;
            cr = new char[]{ (char)13 };
            char[] comArr = command.ToCharArray();
            _commandOut = comArr.Concat(cr).ToArray();
        }
        public SiCommand(string description, string command, string param1Format, int[] errorCodes)
        {
            _command = command;
           
            _param2Format = null;
            _param1Format = param1Format;
            _description = description;
            cr = new char[] { (char)13 };
            char[] comArr = command.ToCharArray();
            _commandOut = comArr.Concat(cr).ToArray();
        }
        public SiCommand(string description, string command, string param1Format,string param2Format, int[] errorCodes)
        {
            _command = command;            
            _param1Format = param1Format;
            _param2Format = param2Format;     
            _description = description;
            cr = new char[] { (char)13 };
            char[] comArr = command.ToCharArray();
            _commandOut = comArr.Concat(cr).ToArray();
        }






    }
}
