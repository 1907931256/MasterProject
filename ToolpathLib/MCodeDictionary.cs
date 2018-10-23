using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
namespace ToolpathLib
{
    public class MCode
    {
        string _onCode;
        string _offCode;
        string _name;
        bool _state;

        internal bool State { get { return _state; } set { _state = value; } }
        internal string OnCode { get { return _onCode; } }
        internal string OffCode {get {return _offCode; } }
        internal string Name { get { return _name; } }

        internal string Code(bool state)
        {
            return state ? _onCode : _offCode;
        }

        internal MCode(string name, string onCode, string offCode)
        {
            _name = name;
            _onCode = onCode;
            _offCode = offCode;            
        }
        internal MCode()
        {
            _name = "";
            _onCode = "";     
            _offCode = "";
            _state = false;
        }
    }
    public class MCodeDictionary:Dictionary<string,MCode>
    {        
        // exposing dictionary as public makes serializer fault switched to method 
        Dictionary<string, MCode> _mcodeDictionary;
        Dictionary<string, MCode> _mcodeReverseDictionary;

        static string defaultFilename = "MCodes.xml";

        public string GetCode(string mcodeName, bool state)
        {
            MCode mc = new MCode();
            _mcodeDictionary.TryGetValue(mcodeName, out mc);
            return mc.Code(state);
        }
        public string GetName(string mCode)
        {            
            MCode mc = new MCode();
            _mcodeReverseDictionary.TryGetValue(mCode, out mc);
            return mc.Name;            
        }
        public bool GetState(string mCode)
        {
            MCode mc = new MCode();
            bool state = false;
            if(_mcodeReverseDictionary.TryGetValue(mCode, out mc))
            { 
                if(mCode == mc.OnCode)
                {
                    state = true;
                }
            }
            return state;
        }
        private void loadMcodes(string fileName)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);
            XmlNodeList mcodeList = doc.SelectNodes("MCodes/MCode");
            _mcodeDictionary = new Dictionary<string, MCode>();
            _mcodeReverseDictionary = new Dictionary<string, MCode>();
            foreach (XmlNode mcodeNode in mcodeList)
            {
                string name = mcodeNode.Attributes["name"].Value.ToString();
                string mcodeOnStr = mcodeNode.Attributes["oncode"].Value.ToString();
                string mcodeOffStr = mcodeNode.Attributes["offcode"].Value.ToString();
                MCode mCode = new MCode(name, mcodeOnStr, mcodeOffStr);
                _mcodeDictionary.Add(name, mCode);
                _mcodeReverseDictionary.Add(mcodeOnStr, mCode);
                _mcodeReverseDictionary.Add(mcodeOffStr, mCode);
            }

        }
        public MCodeDictionary()
        {

            loadMcodes(defaultFilename);

        }
        public MCodeDictionary(string filename)
        {
            loadMcodes(filename);
        }
    }
    
  
}
