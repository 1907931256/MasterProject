using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Threading.Tasks;
using KeyenceLJLib;
using System.Runtime.InteropServices;

namespace ProbeController
{
    public interface ISensorComms
    {

        string GetProbeSettings(CancellationToken ct);        
        bool Connect();
        bool Disconnect();
        
    }
    public enum SendCommand
    {
        /// <summary>None</summary>
        None,
        /// <summary>Restart</summary>
        RebootController,
        /// <summary>Trigger</summary>
        Trigger,
        /// <summary>Start measurement</summary>
        StartMeasure,
        /// <summary>Stop measurement</summary>
        StopMeasure,
        /// <summary>Auto zero</summary>
        AutoZero,
        /// <summary>Timing</summary>
        Timing,
        /// <summary>Reset</summary>
        Reset,
        /// <summary>Program switch</summary>
        ChangeActiveProgram,
        /// <summary>Get measurement results</summary>
        GetMeasurementValue,

        /// <summary>Get profiles</summary>
        GetProfile,
        /// <summary>Get batch profiles (operation mode "high-speed (profile only)")</summary>
        GetBatchProfile,
        /// <summary>Get profiles (operation mode "advanced (with OUT measurement)")</summary>
        GetProfileAdvance,
        /// <summary>Get batch profiles (operation mode "advanced (with OUT measurement)").</summary>
        GetBatchProfileAdvance,

        /// <summary>Start storage</summary>
        StartStorage,
        /// <summary>Stop storage</summary>
        StopStorage,
        /// <summary>Get storage status</summary>
        GetStorageStatus,
        /// <summary>Manual storage request</summary>
        RequestStorage,
        /// <summary>Get storage data</summary>
        GetStorageData,
        /// <summary>Get profile storage data</summary>
        GetStorageProfile,
        /// <summary>Get batch profile storage data.</summary>
        GetStorageBatchProfile,

        /// <summary>Initialize USB high-speed data communication</summary>
        HighSpeedDataUsbCommunicationInitalize,
        /// <summary>Initialize Ethernet high-speed data communication</summary>
        HighSpeedDataEthernetCommunicationInitalize,
        /// <summary>Request preparation before starting high-speed data communication</summary>
        PreStartHighSpeedDataCommunication,
        /// <summary>Start high-speed data communication</summary>
        StartHighSpeedDataCommunication,
    }
    public class LJController
    {
        /// <summary>Ethernet settings structure </summary>
        private LJV7IF_ETHERNET_CONFIG _ethernetConfig;
        /// <summary>Measurement data list</summary>
        private List<MeasureData> _measureDatas;
        /// <summary>Current device ID</summary>
        private int _currentDeviceId;
        /// <summary>Send command</summary>
        private SendCommand _sendCommand;
        /// <summary>Callback function used during high-speed communication</summary>
        private HighSpeedDataCallBack _callback;
        /// <summary>Callback function used during high-speed communication (count only)</summary>
        private HighSpeedDataCallBack _callbackOnlyCount;

        /// The following are maintained in arrays to support multiple controllers.
        /// <summary>Array of profile information structures</summary>
        private LJV7IF_PROFILE_INFO[] _profileInfo;
        /// <summary>Array of controller information</summary>
        private DeviceData _deviceData;
        /// <summary>Array of labels that indicate the controller status</summary>

        bool _connected;
       

        public bool Connect( )
        {
            try
            {
                Rc rc = Rc.Ok;
                rc = (Rc)NativeMethods.LJV7IF_Initialize();
                if (CheckReturnValue(rc))
                {
                    rc = (Rc)NativeMethods.LJV7IF_UsbOpen(Define.DEVICE_ID);
                    if (CheckReturnValue(rc))
                    {
                        _connected = true;
                    }
                }
                return _connected;
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        public bool Connect(string ip1,string ip2,string ip3,string ip4, string port)
        {
            try
            {
                Rc rc = Rc.Ok;
                rc = (Rc)NativeMethods.LJV7IF_Initialize();
                if (CheckReturnValue(rc))
                {
                    _ethernetConfig.abyIpAddress = new byte[] {
                        Convert.ToByte(ip1),
                        Convert.ToByte(ip2),
                        Convert.ToByte(ip3),
                        Convert.ToByte(ip4)
                        };
                    _ethernetConfig.wPortNo = Convert.ToUInt16(port);
                    rc = (Rc)NativeMethods.LJV7IF_EthernetOpen(Define.DEVICE_ID, ref _ethernetConfig);
                    if (CheckReturnValue(rc))
                    {
                        _connected = true;
                    }
                }
                return _connected;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool Disconnect()
        {
            try
            {
               

                var rc = (Rc)NativeMethods.LJV7IF_CommClose(Define.DEVICE_ID);
                Thread.Sleep(100);
                if(CheckReturnValue(rc))
                {
                    rc = (Rc)NativeMethods.LJV7IF_Finalize();
                    Thread.Sleep(100);
                   
                }
                _connected = false;
                return CheckReturnValue(rc);
            }
            catch (Exception)
            {

                throw;
            }
        }
        bool SetSetting(LJSetting lJSetting)
        {
            try
            {                
                using (PinnedObject pin = new PinnedObject(lJSetting.Data))
                {
                    uint dwError = 0;
                    int rc = NativeMethods.LJV7IF_SetSetting(_currentDeviceId, lJSetting.Depth, lJSetting.TargetSetting,
                        pin.Pointer, (uint)lJSetting.Data.Length, ref dwError);
                    // @Point
                    // # There are three setting areas: a) the write settings area, b) the running area, and c) the save area.
                    //   * Specify a) for the setting level when you want to change multiple settings. However, to reflect settings in the LJ-V operations, you have to call LJV7IF_ReflectSetting.
                    //	 * Specify b) for the setting level when you want to change one setting but you don't mind if this setting is returned to its value prior to the change when the power is turned off.
                    //	 * Specify c) for the setting level when you want to change one setting and you want this new value to be retained even when the power is turned off.

                    // @Point
                    //  As a usage example, we will show how to use SettingForm to configure settings such that sending a setting, with SettingForm using its initial values,
                    //  will change the sampling period in the running area to "100 Hz."
                    //  Also see the GetSetting function.       
                    return CheckReturnValue(rc);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        byte[] GetSetting(LJSetting lJSetting)
        {
            try
            {
                LJV7IF_TARGET_SETTING targetSetting = lJSetting.TargetSetting;
                byte[] data = new byte[lJSetting.DataLength];
                using (PinnedObject pin = new PinnedObject(data))
                {
                    int rc = NativeMethods.LJV7IF_GetSetting(_currentDeviceId, lJSetting.Depth, targetSetting,
                        pin.Pointer, (uint)lJSetting.DataLength);

                }
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }
        bool SetTriggerType(LJTriggerType lJTriggerType)
        {
            try
            {
                byte category = 0x00;
                byte item = 0x01;
                byte target1 = 0x00;
                byte target2 = 0x00;
                byte target3 = 0x00;
                byte target4 = 0x00;
                byte[] data = new byte[1] { Convert.ToByte(lJTriggerType) };
                var setting = new LJSetting((byte)SettingDepth.Running, (byte)SettingType.Program00, category, item, target1, target2, target3, target4, data);
                return SetSetting(setting);
            }
            catch (Exception)
            {

                throw;
            }
        }
        LJSamplingFrequency GetSamplingFrequency()
        {
            try
            {               
                byte category = 0x00;
                byte item = 0x01;
                byte target1 = 0x00;
                byte target2 = 0x00;
                byte target3 = 0x00;
                byte target4 = 0x00;
                byte[] data = new byte[1] { Convert.ToByte(LJSamplingFrequency.F100Hz) };
                var setting = new LJSetting((byte)SettingDepth.Running, (byte)SettingType.Program00, category, item, target1, target2, target3, target4, data);
                data = GetSetting(setting);
                LJSamplingFrequency fOut = (LJSamplingFrequency)(Convert.ToUInt32(data[0]));
                return fOut;
            }
            catch (Exception)
            {

                throw;
            }
        }
        bool SetSamplingFrequency(LJSamplingFrequency lJSamplingFrequency)
        {
            try
            {
                byte category = 0x00;
                byte item = 0x02;
                byte target1= 0x00;
                byte target2 = 0x00;
                byte target3 = 0x00;
                byte target4 = 0x00;
                byte[] data = new byte[1] { Convert.ToByte(lJSamplingFrequency) };
                var setting = new LJSetting((byte)SettingDepth.Running, (byte)SettingType.Program00, category, item, target1, target2, target3, target4, data);
                return SetSetting(setting);
            }
            catch (Exception)
            {

                throw;
            }
        }
        bool SetBatchMode(bool batchModeOn)
        {
            try
            {
                byte category = 0x00;
                byte item = 0x03;
                byte target1 = 0x00;
                byte target2 = 0x00;
                byte target3 = 0x00;
                byte target4 = 0x00;
                byte data = Convert.ToByte(batchModeOn);
                var setting = new LJSetting((byte)SettingDepth.Running, (byte)SettingType.Program00, category, item, target1, target2, target3, target4, data);
                return SetSetting(setting);
            }
            catch (Exception)
            {
                throw;
            }
        }

        bool TriggerSingle()
        {
            try
            {
                var rc = (Rc)NativeMethods.LJV7IF_Trigger(_currentDeviceId);
                return CheckReturnValue(rc);
            }
            catch (Exception)
            {

                throw;
            }
        }
        bool CheckReturnValue(Rc rc)
        {
            try
            {
                if(rc==Rc.Ok)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        bool CheckReturnValue(int rc)
        {
            try
            {
                if (rc == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool StartStorage()
        {
            try
            {
                
                var rc= (Rc)NativeMethods.LJV7IF_StartStorage(_currentDeviceId);
                return CheckReturnValue(rc);
                 
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool StopStorage()
        {
            try
            {
                var rc = (Rc)NativeMethods.LJV7IF_StopStorage(_currentDeviceId);
                return CheckReturnValue(rc);
            }
            catch (Exception)
            {

                throw;
            }
        }
       
        uint GetStorageProfileCount()
        {
            try
            {
                LJV7IF_GET_STRAGE_STATUS_REQ req = new LJV7IF_GET_STRAGE_STATUS_REQ() { dwRdArea = 0 };
                // @Point
                // # dwReadArea is the target surface to read.
                //   The target surface to read indicates where in the internal memory usage area to read.
                // # The method to use in specifying dwReadArea varies depending on how internal memory is allocated.
                //   * Double buffer
                //      0 indicates the active surface, 1 indicates surface A, and 2 indicates surface B.
                //   * Entire area (overwrite)
                //      Fixed to 1
                //   * Entire area (do not overwrite)
                //      After a setting modification, data is saved in surfaces 1, 2, 3, and so on in order, and 0 is set as the active surface.
                // # For details, see "9.2.9.2 Internal memory."

                LJV7IF_GET_STRAGE_STATUS_RSP rsp = new LJV7IF_GET_STRAGE_STATUS_RSP();
                LJV7IF_STORAGE_INFO storageInfo = new LJV7IF_STORAGE_INFO();
                uint profileCount = 0;
                int rc = NativeMethods.LJV7IF_GetStorageStatus(_currentDeviceId, ref req, ref rsp, ref storageInfo);
                if (CheckReturnValue(rc))
                {
                    profileCount = storageInfo.dwStorageCnt;
                }
                
                return profileCount;
                // @Point
                // # Terminology	
                //  * Base time … time expressed with 32 bits (<- the time when the setting was changed)
                //  * Accumulated date and time	 … counter value that indicates the elapsed time, in units of 10 ms, from the base time
                // # The accumulated date and time are stored in the accumulated data.
                // # The accumulated time of read data is calculated as shown below.
                //   Accumulated time = "base time (stBaseTime of LJV7IF_GET_STORAGE_RSP)" + "accumulated date and time × 10 ms"
            }
            catch (Exception)
            {

                throw;
            }
        }
        
        ProbeSetup _probeSetup;
        bool _envelopeMode;
        /// <summary>
		/// Get the profile data size
		/// </summary>
		/// <returns>Data size of one profile (in units of bytes)</returns>
		private uint GetOneProfileDataSize()
        {
            try
            {
                // Buffer size (in units of bytes)
                uint retBuffSize = 0;
                _envelopeMode = true;
                // Basic size
                int basicSize = (int)Define.MEASURE_RANGE_FULL / (int)Define.RECEIVED_BINNING_OFF;
                basicSize /= (int)Define.COMPRESS_X_OFF;

                // Number of headers
                retBuffSize += (uint)basicSize * _probeSetup.ProbeCount;

                // Envelope setting
                retBuffSize *= (_envelopeMode ? 2U : 1U);

                //in units of bytes
                retBuffSize *= (uint)Marshal.SizeOf(typeof(uint));

                // Sizes of the header and footer structures
                LJV7IF_PROFILE_HEADER profileHeader = new LJV7IF_PROFILE_HEADER();
                retBuffSize += (uint)Marshal.SizeOf(profileHeader);
                LJV7IF_PROFILE_FOOTER profileFooter = new LJV7IF_PROFILE_FOOTER();
                retBuffSize += (uint)Marshal.SizeOf(profileFooter);

                return retBuffSize;
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        LJV7IF_STORAGE_INFO GetStorageStatus()
        {
            try
            {
                LJV7IF_GET_STRAGE_STATUS_REQ req = new LJV7IF_GET_STRAGE_STATUS_REQ() { dwRdArea = 0 };
                // @Point
                // # dwReadArea is the target surface to read.
                //   The target surface to read indicates where in the internal memory usage area to read.
                // # The method to use in specifying dwReadArea varies depending on how internal memory is allocated.
                //   * Double buffer
                //      0 indicates the active surface, 1 indicates surface A, and 2 indicates surface B.
                //   * Entire area (overwrite)
                //      Fixed to 1
                //   * Entire area (do not overwrite)
                //      After a setting modification, data is saved in surfaces 1, 2, 3, and so on in order, and 0 is set as the active surface.
                // # For details, see "9.2.9.2 Internal memory."

                LJV7IF_GET_STRAGE_STATUS_RSP rsp = new LJV7IF_GET_STRAGE_STATUS_RSP();
                LJV7IF_STORAGE_INFO storageInfo = new LJV7IF_STORAGE_INFO();

                int rc = NativeMethods.LJV7IF_GetStorageStatus(_currentDeviceId, ref req, ref rsp, ref storageInfo);
                // @Point
                // # Terminology	
                //  * Base time … time expressed with 32 bits (<- the time when the setting was changed)
                //  * Accumulated date and time	 … counter value that indicates the elapsed time, in units of 10 ms, from the base time
                // # The accumulated date and time are stored in the accumulated data.
                // # The accumulated time of read data is calculated as shown below.
                //   Accumulated time = "base time (stBaseTime of LJV7IF_GET_STORAGE_RSP)" + "accumulated date and time × 10 ms"

                return storageInfo;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<DataLib.CartData> GetAllStoredProfiles()
        {
            try
            {
                var storageInfo = GetStorageStatus();
                return GetStoredProfiles(storageInfo.dwStorageCnt);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<DataLib.CartData> GetStoredProfiles(uint profileCount)
        {
            try
            {
                _sendCommand = SendCommand.GetStorageProfile;
                _deviceData.ProfileData.Clear();
                _deviceData.MeasureData.Clear();
                _measureDatas.Clear();
                LJV7IF_GET_STORAGE_REQ req = new LJV7IF_GET_STORAGE_REQ() { dwDataCnt = profileCount, dwStartNo = 0, dwSurface = 0 };
                // @Point
                // # dwReadArea is the target surface to read.
                //   The target surface to read indicates where in the internal memory usage area to read.
                // # The method to use in specifying dwReadArea varies depending on how internal memory is allocated.
                //   * Double buffer
                //      0 indicates the active surface, 1 indicates surface A, and 2 indicates surface B.
                //   * Entire area (overwrite)
                //      Fixed to 1
                //   * Entire area (do not overwrite)
                //      After a setting modification, data is saved in surfaces 1, 2, 3, and so on in order, and 0 is set as the active surface.
                // # For details, see "9.2.9.2 Internal memory."

                LJV7IF_STORAGE_INFO storageInfo = new LJV7IF_STORAGE_INFO();
                LJV7IF_GET_STORAGE_RSP rsp = new LJV7IF_GET_STORAGE_RSP();
                LJV7IF_PROFILE_INFO profileInfo = new LJV7IF_PROFILE_INFO();

                uint oneDataSize = (uint)(Marshal.SizeOf(typeof(uint)) + (uint)Utility.GetByteSize(Utility.TypeOfStruct.MEASURE_DATA)
                    * (uint)NativeMethods.MeasurementDataCount * 2 + GetOneProfileDataSize());
                uint allDataSize = Math.Min(Define.READ_DATA_SIZE, oneDataSize * req.dwDataCnt);
                byte[] receiveData = new byte[allDataSize];
                var profDataList = new List<DataLib.CartData>();
                using (PinnedObject pin = new PinnedObject(receiveData))
                {
                    int rc = NativeMethods.LJV7IF_GetStorageProfile(_currentDeviceId, ref req, ref storageInfo, ref rsp, ref profileInfo, pin.Pointer, allDataSize);
                    // @Point
                    // # Terminology	
                    //  * Base time … time expressed with 32 bits (<- the time when the setting was changed)
                    //  * Accumulated date and time	 … counter value that indicates the elapsed time, in units of 10 ms, from the base time
                    // # The accumulated date and time are stored in the accumulated data.
                    // # The accumulated time of read data is calculated as shown below.
                    //   Accumulated time = "base time (stBaseTime of LJV7IF_GET_STORAGE_RSP)" + "accumulated date and time × 10 ms"

                    // @Point
                    // # When reading multiple profiles, the specified number of profiles may not be read.
                    // # To read the remaining profiles after the first set of profiles have been read,
                    //   set the number to start reading profiles from (dwStartNo) and the number of profiles to read (byDataCnt) 
                    //   to values that specify a range of profiles that have not been read to read the profiles in order.
                    // # For the basic code, see "btnGetBatchProfileEx_Click."

                    //AddLogResult(rc, Resources.SID_GET_STORAGE_PROFILE);
                    if (rc == (int)Rc.Ok)
                    {
                        //debugTextOutLines = new List<string>();
                        // Temporarily retain the get data.
                        int measureDataSize = MeasureData.GetByteSize();
                        //debugTextOutLines.Add("measureDataSize " + measureDataSize.ToString());
                        int profileDataSize = ProfileData.CalculateDataSize(profileInfo) * Marshal.SizeOf(typeof(int));
                        //debugTextOutLines.Add("profileDataSize " + profileDataSize.ToString());
                        int profileMeasureDataSize = Utility.GetByteSize(Utility.TypeOfStruct.MEASURE_DATA) * NativeMethods.MeasurementDataCount;
                        int byteSize = measureDataSize + profileDataSize + profileMeasureDataSize;
                        //debugTextOutLines.Add("byteSize " + byteSize.ToString());
                        // debugTextOutLines.Add("rsp.dwDataCnt " + rsp.dwDataCnt.ToString());
                        byte[] tempRecMeasureData = new byte[profileMeasureDataSize];
                        // textBoxDebugOut.Lines = debugTextOutLines.ToArray();

                        for (int i = 0; i < (int)rsp.dwDataCnt; i++)
                        {
                            _measureDatas.Add(new MeasureData(receiveData, byteSize * i));
                            _deviceData.ProfileData.Add(new ProfileData(receiveData, (measureDataSize + byteSize * i), profileInfo));
                            Buffer.BlockCopy(receiveData, (measureDataSize + profileDataSize + byteSize * i), tempRecMeasureData, 0, profileMeasureDataSize);
                            _deviceData.MeasureData.Add(new MeasureData(tempRecMeasureData));
                        }

                        foreach (var profile in _deviceData.ProfileData)
                        {
                            profDataList.Add(profile.AsCartData(_scalingMultiplier));
                        }
                    }                             
                    return profDataList;
                }
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public DataLib.CartData GetSingleProfile()
        {
            try
            {
                var data = new DataLib.CartData();
                if(_connected)
                {
                    _sendCommand = SendCommand.GetProfileAdvance;
                    _deviceData.ProfileData.Clear();
                    _deviceData.MeasureData.Clear();
                    _measureDatas.Clear();

                    // Set the command function
                    LJV7IF_PROFILE_INFO profileInfo = new LJV7IF_PROFILE_INFO();
                    uint dataSize = GetOneProfileDataSize();
                    int[] profileData = new int[dataSize / Marshal.SizeOf(typeof(int))];
                    LJV7IF_MEASURE_DATA[] measureData = new LJV7IF_MEASURE_DATA[NativeMethods.MeasurementDataCount];

                    using (PinnedObject pin = new PinnedObject(profileData))
                    {
                        // Send the command
                        int rc = NativeMethods.LJV7IF_GetProfileAdvance(_currentDeviceId, ref profileInfo, pin.Pointer, dataSize, measureData);

                        // Result output
                       
                        if (rc == (int)Rc.Ok)
                        {
                            // Response data display
                           

                            _measureDatas.Add(new MeasureData(0, measureData));
                            ExtractProfileData(1, ref profileInfo, profileData);
                        }
                    }
                }
                return FilterData( _deviceData.ProfileData[0].AsCartData(1));
                
            }
            catch (Exception)
            {

                throw;
            }
        }
        DataLib.CartData FilterData(DataLib.CartData cartData)
        {
            var outData = new DataLib.CartData();
            foreach(var pt in cartData)
            {
                if(Math.Abs(pt.Y)<1e6)
                {
                    outData.Add(pt);
                }
            }
            return outData;
        }
        /// <summary>
        /// AnalyzeProfileData
        /// </summary>
        /// <param name="profileCount">Number of profiles that were read</param>
        /// <param name="profileInfo">Profile information structure</param>
        /// <param name="profileData">Acquired profile data</param>
        private void ExtractProfileData(int profileCount, ref LJV7IF_PROFILE_INFO profileInfo, int[] profileData)
        {
            try
            {
                int dataUnit = ProfileData.CalculateDataSize(profileInfo);
                ExtractProfileData(profileCount, ref profileInfo, profileData, 0, dataUnit);
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        /// <summary>
        /// AnalyzeProfileData
        /// </summary>
        /// <param name="profileCount">Number of profiles that were read</param>
        /// <param name="profileInfo">Profile information structure</param>
        /// <param name="profileData">Acquired profile data</param>
        /// <param name="startProfileIndex">Start position of the profiles to copy</param>
        /// <param name="dataUnit">Profile data size</param>
        private void ExtractProfileData(int profileCount, ref LJV7IF_PROFILE_INFO profileInfo, int[] profileData, int startProfileIndex, int dataUnit)
        {
            try
            {
                int readPropfileDataSize = ProfileData.CalculateDataSize(profileInfo);
                int[] tempRecvieProfileData = new int[readPropfileDataSize];

                // Profile data retention
                for (int i = 0; i < profileCount; i++)
                {
                    Array.Copy(profileData, (startProfileIndex + i * dataUnit), tempRecvieProfileData, 0, readPropfileDataSize);

                    _deviceData.ProfileData.Add(new ProfileData(tempRecvieProfileData, profileInfo));
                }
            }
            catch (Exception)
            {

                throw;
            }
           
        }
        MeasurementUnit _outputUnit;
        double _scalingMultiplier;
        public static List<string> FrequencyList { get; private set; }
        public static List<string> TriggerTypeList { get; private set; }

        static LJController()
        {
            FrequencyList = new List<string>() { "10Hz", "20Hz", "50Hz", "100Hz", "200Hz", "500Hz", "1KHz", "2KHz", "4KHz", "4.13KHz", "8KHz", "32KHz", "64KHz" };
            TriggerTypeList = new List<string>() { "Continuous", "External", "Encoder" };
        }
        public LJController(ProbeSetup probeSetup,MeasurementUnit outputUnit)
        {
            _probeSetup = probeSetup;
            _outputUnit = outputUnit;
            _scalingMultiplier = Define.PROFILE_UNIT_MM / outputUnit.ConversionFactor;
            var _batchModeParms = new byte[8] {(byte)SettingDepth.Running,(byte)SettingType.Program00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x00 };
            _deviceData = new DeviceData();
            _measureDatas = new List<MeasureData>();
        }
    }
    public enum LJTriggerType
    {
        Continuous=0,
        External=1,
        Encode=2
    }
    public enum LJSamplingFrequency
    {
        F10Hz=0,
        F20Hz=1,
        F50Hz =2,
        F100Hz=3,
        F200Hz=4,
        F500Hz=5,
        F1KHz=6,
        F2KHz=7,
        F4KHz=8,
        F413KHz=9,
        F8KHz=10,
        F16KHz=11,
        F32KHz=12,
        F64KHz=13
    }

    public class SiController
    {

        //double[] GetAllData(CancellationToken ct)
        //{

        //}
        //double[] GetDataArray(int points, CancellationToken ct);
        //double GetSingleDataPoint(CancellationToken ct);
    }
    public class ProbeController:ISensorComms
    {
         
       
        protected int probeCount;
        
        
        protected double[] dataArray;
        
           
        public string GetProbeSettings(CancellationToken ct)
        {
            string settings = "";
            return settings;
        }
        public double[] GetAllData(CancellationToken ct)
        {
            var data = new List<double>();
            return data.ToArray();

        }
        public double[] GetDataArray(int points,CancellationToken ct)
        {
            var data = new List<double>();
            return data.ToArray();
        }
        public double GetSingleDataPoint(CancellationToken ct)
        {
            double pt = 0;
            return pt;
        }
        public bool Connect()
        {
            return false;
        }
        public bool Disconnect()
        {
            return true;
        }
         
        public ProbeController()
        {
                  
        }

    }
}
