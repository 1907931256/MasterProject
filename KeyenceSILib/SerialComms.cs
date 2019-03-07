using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.InteropServices;
using System.IO.Ports;
namespace KeyenceSILib
{
    public class SerialComms
    {
        public enum FilterMode
        {
            Averaging = 0,
            LowPass = 1,
            HighPass = 2
        }
        public enum TriggerNum
        {
            Trig1 = 0,
            Trig2 = 1
        }
        public enum FilterAverging
        {
            Hz300_X1 = 0,
            Hz100_X4 = 1,
            Hz30_X16 = 2,
            Hz10_X64 = 3,
            Hz3_X256 = 4,
            Hz1_X1024 = 5,
            Hz03_X4096 = 6,
            Hz01_X16384 = 7,
            X65536 = 8,
            X262144 = 9
        }
        ControllerMode controllerMode;
        public enum ControllerMode
        {
            CommMode = 0,
            GenMode = 1
        }
        public enum StorageCyle
        {
            x1 = 0,
            x2 = 1,
            x5 = 2,
            x10 = 3,
            x20 = 4,
            x50 = 5,
            x100 = 6,
            x200 = 7,
            x500 = 8,
            x1000 = 9,
            SyncInput = 10
        }
        public enum SwitchStatus
        {
            OFF = 0,
            ON = 1
        }
        public enum StorageStatus
        {
            StorageStopped = 0,
            StorageActive = 1
        }
        public class DataStorageStats
        {
            public StorageStatus StorageStatus { get; set; }
            public int[] StoredDataCount { get; set; }
            public StorageCyle StorageCycle { get; set; }
        }
        Dictionary<int, string> ErrorDictionary;
        SerialPort serialPort;
        string comMode = "Q0";
        // Change to General Mode
        string genMode = "R0";
        // Data Storage Start
        string dataStoreStart = "AS";
        // Data Storage Stop
        string dataStoreStop = "AP";
        // Data Storage Initialization
        string dataStoreInit = "AQ";
        // Data Storage Output
        string dataStoreOut = "AO,01";
        //get one data point
        string dataGetOnePoint = "MS,01";
        int _headCount;
        int _maxPointsToStore;
        public SerialComms(string portName, int headCount)
        {
            try
            {
                BuildDictionary();
                serialPort = Open(portName);
                SetCommunicationMode();
                _headCount = 1;
                if (headCount > 0)
                {
                    _headCount = headCount;
                }

                _maxPointsToStore = 1200000 / _headCount;
            }
            catch (Exception)
            {

                throw;
            }

        }
        void BuildDictionary()
        {
            try
            {
                ErrorDictionary = new Dictionary<int, string>();
                ErrorDictionary.Add(0, "Head Connection Error ");
                ErrorDictionary.Add(1, "Head 01 Error ");
                ErrorDictionary.Add(2, "Head 02 Error ");
                ErrorDictionary.Add(13, "Controller Error ");
                ErrorDictionary.Add(15, "Controller SRAM Error ");
                ErrorDictionary.Add(16, "USB Error ");
                ErrorDictionary.Add(17, "Ethernet Error ");
                ErrorDictionary.Add(50, "Command Error ");
                ErrorDictionary.Add(51, "Status Error ");
                ErrorDictionary.Add(60, "Command Length Error ");
                ErrorDictionary.Add(61, "Parameter Count Error ");
                ErrorDictionary.Add(62, "Parameter Range Error ");
                ErrorDictionary.Add(88, "Timeout Error ");
                ErrorDictionary.Add(99, "Other Error ");
            }
            catch (Exception)
            {

                throw;
            }

        }

        public SerialPort Open(string portName)
        {
            try
            {
                serialPort = new SerialPort(portName);                
                serialPort.BaudRate = 115200; 
                serialPort.Parity = Parity.None; 
                serialPort.DataBits = 8; 
                serialPort.StopBits = StopBits.One; 
                serialPort.Handshake = Handshake.None;
                serialPort.ReadBufferSize = 8400000;    
                //serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                serialPort.ReadTimeout = 2500;
                serialPort.WriteTimeout = 2500;
                serialPort.Open();
                return serialPort;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public void SetTrigger(int outNo, TriggerNum triggerNum)
        {
            try
            {
                if (controllerMode == ControllerMode.GenMode)
                {
                    SetCommunicationMode();
                }
                int tn = (int)triggerNum;
                string command = "SW,OE,M," + outNo.ToString() + "," + tn.ToString("d1");
                Write(command, 100);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public void SetFilter(int outNo, FilterMode filterMode, FilterAverging filterAverging)
        {
            try
            {
                if (controllerMode == ControllerMode.GenMode)
                {
                    SetCommunicationMode();
                }
                int fm = (int)filterMode;
                int fa = (int)filterAverging;
                string command = "SW,OC," + outNo.ToString() + "," + fm.ToString("d1") + "," + fa.ToString("d1");
                Write(command, 100);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public void SetTiming(SwitchStatus switchStatus, int outNo)
        {
            try
            {
                if (controllerMode == ControllerMode.GenMode)
                {
                    SetCommunicationMode();
                }
                int sw = (int)switchStatus;
                string command = "TS," + sw.ToString() + "," + outNo.ToString("d2");
                Write(command, 100);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public void SetActiveHeadCount(int headCount)
        {
            try
            {
                if (controllerMode == ControllerMode.GenMode)
                {
                    SetCommunicationMode();
                }
                string command = "SW,EF," + headCount.ToString("d2");
                Write(command, 100);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public DataStorageStats GetStorageStatus()
        {
            try
            {
                string data = Write("AN", 100);
                var words = data.Split(',');
                var status = (StorageStatus)Enum.Parse(typeof(StorageStatus), words[1]);
                var pointCountList = new List<int>();
                for (int i = 2; i < words.Length; i++)
                {
                    int pc = int.Parse(words[i]);
                    pointCountList.Add(pc);
                }
                var dss = new DataStorageStats();
                dss.StorageStatus = status;
                dss.StoredDataCount = pointCountList.ToArray();
                return dss;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void SetStorage(int pointsToStore, StorageCyle storageCycle)
        {
            try
            {
                if (controllerMode == ControllerMode.GenMode)
                {
                    SetCommunicationMode();
                }
                if (pointsToStore > _maxPointsToStore)
                {
                    pointsToStore = _maxPointsToStore;
                }
                if (pointsToStore <= 0)
                {
                    throw new Exception("Points to Store must be >0 ;=" + pointsToStore.ToString());
                }

                int storCycle = (int)storageCycle;

                string command = "SW,CD," + pointsToStore.ToString("D7") + "," + storCycle.ToString("D2");
                Write(command, 100);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InitStorage()
        {
            try
            {
                if (controllerMode == ControllerMode.CommMode)
                {
                    SetGeneralMode();
                }
                Write(dataStoreInit, 100);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void StartStorage()
        {
            try
            {
                if (controllerMode == ControllerMode.CommMode)
                {
                    SetGeneralMode();
                }
                Write(dataStoreStart, 100);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void StopStorage()
        {
            try
            {
                if (controllerMode == ControllerMode.CommMode)
                {
                    SetGeneralMode();
                }
                Write(dataStoreStop, 100);
            }
            catch (Exception)
            {
                throw;
            }
        }
        string incomingData;
        // not used as of 3/4/19 
        //using CR at end of response to find end 
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                var cr = (char)13;
                SerialPort sp = (SerialPort)sender;
                string indata = sp.ReadExisting();
                if (indata.Contains(cr))
                {
                    incomingData = indata;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<double> GetStoredData()
        {
            try
            {
                if (controllerMode == ControllerMode.CommMode)
                {
                    SetGeneralMode();
                }
                var storeStats = new DataStorageStats();

                StorageStatus currentStatus = StorageStatus.StorageActive;
                while (currentStatus == StorageStatus.StorageActive)
                {
                    storeStats = GetStorageStatus();
                    currentStatus = storeStats.StorageStatus;
                }

                string dataOut = Write(dataStoreOut, 100);
                Write(dataStoreStop, 100);
                var stringList = GetData(dataOut);
                var outputList = new List<double>();
                foreach (int ptCt in storeStats.StoredDataCount)
                {
                    outputList.Add((double)ptCt);
                }
                for (int i = 0; i < stringList.Count; i++)
                {
                    double result = 0;
                    if (double.TryParse(stringList[i], out result))
                    {
                        outputList.Add(result);
                    }

                }
                return outputList;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void SetCommunicationMode()
        {
            try
            {
                var commandOut = BuildCommand(comMode);
                serialPort.Write(commandOut, 0, commandOut.Length);
                System.Threading.Thread.Sleep(50);
                controllerMode = ControllerMode.CommMode;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void SetGeneralMode()
        {
            try
            {
                var commandOut = BuildCommand(genMode);
                serialPort.Write(commandOut, 0, commandOut.Length);
                Thread.Sleep(600);
                controllerMode = ControllerMode.GenMode;
            }
            catch (Exception)
            {
                throw;
            }
        }
        char[] BuildCommand(string commandStr)
        {
            try
            {
                char[] cr = { (char)13 };
                char[] command = commandStr.ToCharArray();
                char[] commandOut = command.Concat(cr).ToArray();
                return commandOut;
            }
            catch (Exception)
            {

                throw;
            }

        }
        string Write(string commandStr, int responseMs)
        {
            try
            {
                var commandOut = BuildCommand(commandStr);
                serialPort.Write(commandOut, 0, commandOut.Length);
                System.Threading.Thread.Sleep(responseMs);
                return Read(commandStr);
            }
            catch (Exception)
            {

                throw;
            }

        }
        void CheckError(string commandSent, string response)
        {
            try
            {
                if (response.Contains("ER"))
                {
                    string[] words = response.Split(',');
                    string errorCodeStr = words[words.Length - 1];
                    int errorCode = 99;
                    string errorDesc = "Other Error";
                    if (int.TryParse(errorCodeStr, out errorCode))
                    {
                        ErrorDictionary.TryGetValue(errorCode, out errorDesc);
                        throw new Exception(errorDesc + ":" + commandSent + ":" + response);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        void CheckResponse(string commandSent, string response)
        {
            try
            {
                string[] commands;
                if (commandSent.Contains(","))
                {
                    commands = commandSent.Split(',');
                }
                else
                {
                    commands = new string[1] { commandSent };
                }
                if (!(response.Contains(commands[0])))
                {
                    throw new Exception("Invalid Response  " + commandSent + ";" + response);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        string Read(string commandSent)
        {
            try
            {
                string response = "";
                char[] data = new char[8400000];
                int offset = 0;
                char cr = (char)13;
                bool endfound = false;

                while (!endfound)
                {
                    int bytes = serialPort.BytesToRead;
                    serialPort.Read(data, offset, bytes);
                    offset += bytes;
                    if (data.Contains(cr))
                    {
                        endfound = true;
                    }
                }
                foreach (char c in data)
                {
                    if (c != ' ' && c != (char)0)
                    {
                        response += c.ToString();
                    }
                }
                CheckResponse(commandSent, response);
                CheckError(commandSent, response);
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        static List<string> GetData(string csvdata)
        {
            try
            {
                var stringList = new List<string>();
                string[] words = csvdata.Split(',');
                for (int i = 1; i < words.Length; i++)
                {
                    stringList.Add(words[i]);
                }
                return stringList;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
