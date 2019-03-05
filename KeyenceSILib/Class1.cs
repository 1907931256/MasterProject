using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO.Ports;
namespace KeyenceSILib
{
    public enum RC
    {
        RC_OK = 0x0000,             // The operation is completed successfully.
                                    ///////////////////////////////////////////////
                                    // Communication error from controller notification
                                    //
        RC_NAK_FIRST = 0x1000,
        RC_NAK_COMMAND,         // Command error
        RC_NAK_COMMAND_LENGTH,      // Command length error
        RC_NAK_TIMEOUT,             // Timeout
        RC_NAK_CHECKSUM,            // Check sum error
        RC_NAK_INVALID_STATE,       // Status error
        RC_NAK_OTHER,               // Other error
        RC_NAK_PARAMETER,           // Parameter error
        RC_NAK_OUT_STAGE,           // OUT calculation count limitation error
        RC_NAK_OUT_HEAD_NUM,        // No. of used head/OUT over error
        RC_NAK_OUT_INVALID_CALC,    // OUT which cannot be used for calculation was specified for calculation.
        RC_NAK_OUT_VOID,            // OUT which specified for calculation is not found.
        RC_NAK_INVALID_CYCLE,       // Unavailable sampling cycle
        RC_NAK_CTRL_ERROR,          // Main unit error
        RC_NAK_SRAM_ERROR,          // Setting value error
                                    ///////////////////////////////////////////////
                                    // Communication DLL error notification
                                    //
        RC_ERR_OPEN_DEVICE = 0x2000,// Opening the device failed.
        RC_ERR_NO_DEVICE,           // The device is not open.
        RC_ERR_SEND,                // Command sending error
        RC_ERR_RECEIVE,             // Response receiving error
        RC_ERR_TIMEOUT,             // Timeout
        RC_ERR_NODATA,              // No data
        RC_ERR_NOMEMORY,            // No free memory

        RC_ERR_DISCONNECT,          // Cable disconnection suspected
        RC_ERR_UNKNOWN,             // Undefined error
    }
    public enum SIIF_FLOATRESULT {
        SIIF_FLOATRESULT_VALID,         // valid data
        SIIF_FLOATRESULT_RANGEOVER_P,   // over range at positive (+) side
        SIIF_FLOATRESULT_RANGEOVER_N,   // over range at negative (-) side
        SIIF_FLOATRESULT_WAITING,       // Wait for comparator result
        SIIF_FLOATRESULT_ALARM,         // alarm
        SIIF_FLOATRESULT_INVALID,       // Invalid (error, etc.)
    }
    // Set Measurement Mode
    public enum SIIF_MEASUREMODE
    {
        SIIF_MEASUREMODE_NORMAL = 0,                        // normal
        SIIF_MEASUREMODE_INTERFEROMETER,                // Interferometer
    }
   public enum SIIF_MEDIAN
    {
        SIIF_MEDIAN_OFF,                // OFF					
        SIIF_MEDIAN_7,                  // 7 point
        SIIF_MEDIAN_15,                 // 15 point
        SIIF_MEDIAN_31,                 // 31 point
    }
   public enum SIIF_LASER_CTRL_GROUP
    {
        SIIF_LASER_CTRL_GROUP_1,        // LASER CTRL 1
        SIIF_LASER_CTRL_GROUP_2,        // LASER CTRL 2
    }
    public enum SIIF_CALCMETHOD
    {
        SIIF_CALCMETHOD_HEAD = 0,   // head
        SIIF_CALCMETHOD_OUT,        // OUT
        SIIF_CALCMETHOD_ADD,        // ADD
        SIIF_CALCMETHOD_SUB,        // SUB
        SIIF_CALCMETHOD_AVE,        // AVE
        SIIF_CALCMETHOD_PP,         // P-P
        SIIF_CALCMETHOD_MAX,        // MAX
        SIIF_CALCMETHOD_MIN,        // MIN
    }
    
    public enum  SIIF_FILTERMODE
    {
        SIIF_FILTERMODE_MOVING_AVERAGE,         // moving average
        SIIF_FILTERMODE_LOWPASS,                // low pass filter
        SIIF_FILTERMODE_HIGHPASS,               // high pass filter
    }
    public enum SIIF_FILTERPARA 
    {
        SIIF_FILTERPARA_AVE_1 = 0,      // 1 time
        SIIF_FILTERPARA_AVE_4,          // 4 times
        SIIF_FILTERPARA_AVE_16,         // 16 times
        SIIF_FILTERPARA_AVE_64,         // 64 times
        SIIF_FILTERPARA_AVE_256,        // 256 times
        SIIF_FILTERPARA_AVE_1024,       // 1024 times
        SIIF_FILTERPARA_AVE_4096,       // 4096 times
        SIIF_FILTERPARA_AVE_16384,      // 16384 times
        SIIF_FILTERPARA_AVE_65536,      // 65536 times
        SIIF_FILTERPARA_AVE_262144,     // 262144 times

        SIIF_FILTERPARA_COFF_300 = 0,       // 300Hz
        SIIF_FILTERPARA_COFF_100,       // 100Hz
        SIIF_FILTERPARA_COFF_30,        // 30Hz
        SIIF_FILTERPARA_COFF_10,        // 10Hz
        SIIF_FILTERPARA_COFF_3,         // 3Hz
        SIIF_FILTERPARA_COFF_1,         // 1Hz
        SIIF_FILTERPARA_COFF_0_3,       // 0.3Hz
        SIIF_FILTERPARA_COFF_0_1,       // 0.1Hz
    }
    // Set Trigger Mode
    public enum SIIF_TRIGGERMODE
    {
        SIIF_TRIGGERMODE_EXT1,      // external trigger 1
        SIIF_TRIGGERMODE_EXT2,      // external trigger 2
    }
    

    // Set the Measurement Mode.
    public enum SIIF_CALCMODE
    {
        SIIF_CALCMODE_NORMAL,           // normal
        SIIF_CALCMODE_PEAKHOLD,         // peak hold
        SIIF_CALCMODE_BOTTOMHOLD,       // bottom hold
        SIIF_CALCMODE_PEAKTOPEAKHOLD,   // peak-to-peak hold
        SIIF_CALCMODE_SAMPLEHOLD,       // sample hold
        SIIF_CALCMODE_AVERAGEHOLD,      // average hold
    }
    

    // Set Minimum Display Unit
    public enum SIIF_DISPLAYUNIT
    {
        SIIF_DISPLAYUNIT_0000_01MM = 0, // 0.01mm
        SIIF_DISPLAYUNIT_000_001MM,     // 0.001mm
        SIIF_DISPLAYUNIT_00_0001MM,     // 0.0001mm
        SIIF_DISPLAYUNIT_0_00001MM,     // 0.00001mm
        SIIF_DISPLAYUNIT_00000_1UM,     // 0.1um
        SIIF_DISPLAYUNIT_0000_01UM,     // 0.01um
        SIIF_DISPLAYUNIT_000_001UM,     // 0.001um	
    }
    

    // Set measurement type.
    public enum SIIF_MEASURETYPE
    {
        SIIF_MEASURETYPE_DISPLACEMENT,  // Displacement
        SIIF_MEASURETYPE_LIGHT1,        // Light1 
        SIIF_MEASURETYPE_LIGHT2,        // Light2
    }
    

    // Specify OUT
    public enum SIIF_OUTNO
    {
        SIIF_OUTNO_01 = 0x0001,         // OUT1		
        SIIF_OUTNO_02 = 0x0002,         // OUT2		
        SIIF_OUTNO_03 = 0x0004,         // OUT3		
        SIIF_OUTNO_04 = 0x0008,         // OUT4		
        SIIF_OUTNO_05 = 0x0010,         // OUT5		
        SIIF_OUTNO_06 = 0x0020,         // OUT6		
        SIIF_OUTNO_07 = 0x0040,         // OUT7		
        SIIF_OUTNO_08 = 0x0080,         // OUT8		
        SIIF_OUTNO_09 = 0x0100,         // OUT9		
        SIIF_OUTNO_10 = 0x0200,         // OUT10		
        SIIF_OUTNO_11 = 0x0400,         // OUT11		
        SIIF_OUTNO_12 = 0x0800,         // OUT12		
        SIIF_OUTNO_ALL = 0x0FFF,        // All OUTs
    }
    

    // Set Storage (Target)  *Specify TRUE/FALSE for each OUT to set ON/OFF.
    public enum SIIF_TARGETOUT
    {
        SIIF_TARGETOUT_NONE,            // no target OUT
        SIIF_TARGETOUT_OUT1,            // OUT1
        SIIF_TARGETOUT_OUT2,            // OUT2
        SIIF_TARGETOUT_BOTH,            // OUT 1/2
    }
    

    // Set Storage (Cycle)
    public enum SIIF_STORAGECYCLE
    {
        SIIF_STORAGECYCLE_1,            // sampling rate x 1
        SIIF_STORAGECYCLE_2,            // sampling rate x 2
        SIIF_STORAGECYCLE_5,            // sampling rate x 5
        SIIF_STORAGECYCLE_10,           // sampling rate x 10
        SIIF_STORAGECYCLE_20,           // sampling rate x 20
        SIIF_STORAGECYCLE_50,           // sampling rate x 50
        SIIF_STORAGECYCLE_100,          // sampling rate x 100
        SIIF_STORAGECYCLE_200,          // sampling rate x 200
        SIIF_STORAGECYCLE_500,          // sampling rate x 500
        SIIF_STORAGECYCLE_1000,         // sampling rate x 1000
        SIIF_STORAGECYCLE_TIMING,       // Timing sync
    }
    

    // Set Comparator Output Format
    public enum SIIF_TOLERANCE_COMPARATOR_OUTPUT_FORMAT
    {
        SIIF_TOLERANCE_COMPARATOR_OUTPUT_FORMAT_NORMAL,     // normal
        SIIF_TOLERANCE_COMPARATOR_OUTPUT_FORMAT_HOLD,       // hold
        SIIF_TOLERANCE_COMPARATOR_OUTPUT_FORMAT_OFF_DELAY,  // off-delay
    }
    

    // Set Strobe Time
    public enum SIIF_STOROBETIME
    {
        SIIF_STOROBETIME_2MS,       // 2ms							
        SIIF_STOROBETIME_5MS,       // 5ms							
        SIIF_STOROBETIME_10MS,      // 10ms							
        SIIF_STOROBETIME_20MS,      // 20ms							
    }
    

    // Set alarm output form.
    public enum SIIF_ALARM_OUTPUT_FORM
    {
        SIIF_ALARM_OUTPUT_FORM_SYSTEM,  // System alarm
        SIIF_ALARM_OUTPUT_FORM_MEASURE, // Measurement alarm
        SIIF_ALARM_OUTPUT_FORM_BOTH,    // System alarm and measurement alarm
    }
    

    // Mode Switch
    public enum SIIF_MODE
    {
        SIIF_MODE_NORMAL,               // normal mode
        SIIF_MODE_COMMUNICATION,        // setting mode
    }

   [StructLayout(LayoutKind.Sequential)]
	public struct SIIF_FLOATVALUE_OUT
    {
        public int OutNo;          // OUT No(0-11)
        public SIIF_FLOATRESULT FloatResult;   // valid or invalid data
        public float Value;            // Measurement value
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct SIIF_FLOATVALUE
    {
        public SIIF_FLOATRESULT FloatResult;   // valid or invalid data
        public float Value;            // Measurement value
    }

    //// IP address
    //[StructLayout(LayoutKind.Sequential)]
    //public struct SIIF_OPENPARAM_ETHERNET
    //{
    //    IN_ADDR IPAddress;
    //} 
    public class NativeMethods
    {
        [DllImport("SIIF.dll" )]
        public static extern RC SIIF_OpenDeviceUsb();
        [DllImport("SIIF.dll")]
        public static extern RC SIIF_CloseDevice();
        [DllImport("SIIF.dll")]
        public static extern int SIIF_GetCalcDataSingle(long outNo, ref SIIF_FLOATVALUE_OUT calcData);
        [DllImport("SIIF.dll")]
        public static extern int SIIF_DataStorageInit();
        [DllImport("SIIF.dll")]
        public static extern int SIIF_DataStorageStart();      
        [DllImport("SIIF.dll")]
        public static extern int SIIF_DataStorageStop();
        [DllImport("SIIF.dll")]
        public static extern int SIIF_DataStorageGetData( long OutNo,  long NumOfBuffer, ref SIIF_FLOATVALUE[]  OutBuffer, ref long NumReceived);
        [DllImport("SIIF.dll")]
        public static extern int SIIF_StartMeasure();
        [DllImport("SIIF.dll")]
        public static extern int SIIF_StopMeasure();
        [DllImport("SIIF.dll")]
        public static extern int SIIF_GetMeasureMode(int headNo,ref SIIF_MEASUREMODE measureMode);

    }
    
   
    public class SerialComms
    {
        public enum FilterMode
        {
            Averaging=0,
            LowPass=1,
            HighPass=2
        }
        public enum TriggerNum
        {
            Trig1=0,
            Trig2=1
        }
        public enum FilterAverging
        {
            Hz300_X1=0,
            Hz100_X4=1,
            Hz30_X16=2,
            Hz10_X64=3,
            Hz3_X256=4,
            Hz1_X1024=5,
            Hz03_X4096=6,
            Hz01_X16384=7,
            X65536=8,
            X262144=9
        }
        ControllerMode controllerMode;
        public enum ControllerMode
        {
            CommMode=0,
            GenMode=1
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
        public SerialComms(string portName,int headCount)
        {
            try
            {
                BuildDictionary();
                serialPort = Open(portName);
                SetCommunicationMode();
                _headCount = 1;
                if(headCount>0)
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


                // Allow the user to set the appropriate properties.
                //serialPort.PortName = portName; /*SetPortName(_serialPort.PortName);*/
                serialPort.BaudRate = 115200; /*SetPortBaudRate(_serialPort.BaudRate);*/
                serialPort.Parity = Parity.None; /*SetPortParity(_serialPort.Parity);*/
                serialPort.DataBits = 8; /*SetPortDataBits(_serialPort.DataBits);*/
                serialPort.StopBits = StopBits.One; /*SetPortStopBits(_serialPort.StopBits);*/
                serialPort.Handshake = Handshake.None; /*SetPortHandshake(_serialPort.Handshake);*/
                serialPort.ReadBufferSize = 1000000;
                //
                //added to working code
                //serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                //end of add

                // Set the read/write timeouts
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
                int tn = (int)triggerNum;
                string command = "SW,OE,M," + outNo.ToString() + "," + tn.ToString("d1");
                Write(command);
            }
            catch (Exception)
            {

                throw;
            }
           
        }
        public void SetFilter(int outNo, FilterMode filterMode,FilterAverging filterAverging)
        {
            try
            {
                int fm = (int)filterMode;
                int fa = (int)filterAverging;
                string command = "SW,OC," + outNo.ToString() + "," + fm.ToString("d1") + "," + fa.ToString("d1");
                Write(command);
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
                int sw = (int)switchStatus;
                string command = "TS," + sw.ToString() + "," + outNo.ToString("d2");
                Write(command);
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
                string command = "SW,EF," + headCount.ToString("d2");
                Write(command);
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
                string data=  Write("AN");
                var words = data.Split(',');
                var status = (StorageStatus)Enum.Parse(typeof(StorageStatus), words[1]);
                var pointCountList = new List<int>();
                for(int i=2;i<words.Length;i++)
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
                if (pointsToStore > _maxPointsToStore)
                {
                    pointsToStore = _maxPointsToStore;
                }
                if (pointsToStore <= 0)
                {
                    throw new Exception("Points to Store must be >0 ;="+ pointsToStore.ToString());
                }
                int storCycle = (int)storageCycle;
                
                string command = "SW,CD," + pointsToStore.ToString("D7") + "," + storCycle.ToString("D2");
                Write(command);
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
                Write(dataStoreInit);               
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
                Write(dataStoreStart);
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
               Write(dataStoreStop);
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
        public List<double>GetStoredData()
        {
            try
            {
                var storeStats = new DataStorageStats();
                 
                StorageStatus currentStatus = StorageStatus.StorageActive;
                while (currentStatus == StorageStatus.StorageActive)
                {
                    storeStats = GetStorageStatus();
                    currentStatus = storeStats.StorageStatus;
                }
                
                string dataOut = Write(dataStoreOut);
                Write(dataStoreStop);
                var stringList = GetData(dataOut);
                var outputList = new List<double>();
                foreach(int ptCt in storeStats.StoredDataCount)
                {
                    outputList.Add((double)ptCt);
                }
                for(int i=0;i<stringList.Count;i++)
                {
                    double result = 0;
                    if (double.TryParse(stringList[i], out result))
                    {                      
                        outputList.Add(result);
                    }
                    else
                    {
                        Console.WriteLine(i.ToString()+","+ stringList[i]);
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
        string Write(string commandStr)
        {
            try
            {
                var commandOut = BuildCommand(commandStr);
                serialPort.Write(commandOut, 0, commandOut.Length);
                return Read(commandStr);
            }
            catch (Exception)
            {

                throw;
            }
                     
        }
        void CheckError(string commandSent,string response)
        {
            try
            {
                if(response.Contains("ER"))
                {
                    string[] words = response.Split(',');
                    string errorCodeStr = words[words.Length - 1];
                    int errorCode = 99;
                    string errorDesc = "Other Error";
                    if(int.TryParse(errorCodeStr,out errorCode))
                    {
                       ErrorDictionary.TryGetValue(errorCode, out errorDesc);
                       throw new Exception(errorDesc +":"+ commandSent +":"+ response);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        void CheckResponse(string commandSent,string response)
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
                if(!(response.Contains(commands[0])))
                {
                    throw new Exception("Invalid Response" + commandSent + ";"+response);
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
                //int i = 0;
                while (!endfound)
                {
                    int bytes = serialPort.BytesToRead;
                    serialPort.Read(data, offset, bytes);                    
                    offset += bytes;
                    if (data.Contains(cr))
                    {
                        endfound = true;
                    }
                    //Thread.Sleep(1);
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
