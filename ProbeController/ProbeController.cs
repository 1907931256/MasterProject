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
                 
                rc = (Rc)NativeMethods.LJV7IF_Initialize();
                CheckReturnValue(rc);
                rc = (Rc)NativeMethods.LJV7IF_UsbOpen(Define.DEVICE_ID);
                CheckReturnValue(rc);
                _connected = true;
                return _connected;
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        Rc rc;
        public bool Connect(string ip1,string ip2,string ip3,string ip4, string port)
        {
            try
            {
                 
                rc = (Rc)NativeMethods.LJV7IF_Initialize();
                CheckReturnValue(rc);
                _ethernetConfig.abyIpAddress = new byte[] {
                        Convert.ToByte(ip1),
                        Convert.ToByte(ip2),
                        Convert.ToByte(ip3),
                        Convert.ToByte(ip4)
                };
                _ethernetConfig.wPortNo = Convert.ToUInt16(port);
                rc = (Rc)NativeMethods.LJV7IF_EthernetOpen(Define.DEVICE_ID, ref _ethernetConfig);
                CheckReturnValue(rc);
                                
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
                CheckReturnValue(rc);               
                 
                rc = (Rc)NativeMethods.LJV7IF_Finalize();
                CheckReturnValue(rc); 
                
                _connected = false;                
                return _connected;
            }
            catch (Exception)
            {

                throw;
            }
        }
        
        
        void SetTriggerType(LJV7IF_TRIGGER_MODE lJTriggerType)
        {
            try
            {
                LJSetting.SetTriggerMode(_currentDeviceId, SettingType.Program00, lJTriggerType);
            }
            catch (Exception)
            {

                throw;
            }
        } 
        void SetSamplingFrequency(LJV7IF_FREQUENCY lJSamplingFrequency)
        {
            try
            {
                LJSetting.SetTriggerFreq(_currentDeviceId, SettingType.Program00, lJSamplingFrequency);
            }
            catch (Exception)
            {

                throw;
            }
        }
        
        void TriggerSingle()
        {
            try
            {
                rc = (Rc)NativeMethods.LJV7IF_Trigger(_currentDeviceId);
                CheckReturnValue(rc);
            }
            catch (Exception)
            {

                throw;
            }
        }
        void CheckReturnValue(Rc rc)
        {
            try
            {
                if (rc != Rc.Ok)
                {
                    throw new Exception(ErrorCode.GetErrorMessage(rc));
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        void CheckReturnValue(int rc)
        {
            try
            {
                if (rc != 0)
                {
                    throw new Exception(ErrorCode.GetErrorMessage(rc));
                }
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
                rc = (Rc)NativeMethods.LJV7IF_ClearMemory(_currentDeviceId);
                Thread.Sleep(100);
                CheckReturnValue(rc); 
                
                rc = (Rc)NativeMethods.LJV7IF_StartStorage(_currentDeviceId);                                
                CheckReturnValue(rc);
                 
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
                rc = (Rc)NativeMethods.LJV7IF_StopStorage(_currentDeviceId);
                CheckReturnValue(rc);
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
                 rc = (Rc)NativeMethods.LJV7IF_GetStorageStatus(_currentDeviceId, ref req, ref rsp, ref storageInfo);
                CheckReturnValue(rc);
                
                profileCount = storageInfo.dwStorageCnt;
                
                
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

                rc =(Rc)NativeMethods.LJV7IF_GetStorageStatus(_currentDeviceId, ref req, ref rsp, ref storageInfo);
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
        public List<DataLib.CartData> GetStoredProfiles()
        {
            try
            {
                var profDataList = new List< DataLib.CartData > ();
                LJV7IF_GET_PROFILE_REQ req = new LJV7IF_GET_PROFILE_REQ();
                req.byTargetBank = (byte)ProfileBank.Active;
                req.byPosMode = (byte)ProfilePos.Current;
                req.dwGetProfNo = 0;
                req.byGetProfCnt = 10;
                req.byErase = 0;

                LJV7IF_GET_PROFILE_RSP rsp = new LJV7IF_GET_PROFILE_RSP();
                LJV7IF_PROFILE_INFO profileInfo = new LJV7IF_PROFILE_INFO();

                int profileDataSize = Define.MAX_PROFILE_COUNT +
                    (Marshal.SizeOf(typeof(LJV7IF_PROFILE_HEADER)) + Marshal.SizeOf(typeof(LJV7IF_PROFILE_FOOTER))) / Marshal.SizeOf(typeof(int));
                int[] receiveBuffer = new int[profileDataSize * req.byGetProfCnt];


                Rc rc;
                    // Get profiles.
                using (PinnedObject pin = new PinnedObject(receiveBuffer))
                {
                        rc = (Rc)NativeMethods.LJV7IF_GetProfile(Define.DEVICE_ID, ref req, ref rsp, ref profileInfo, pin.Pointer,
                            (uint)(receiveBuffer.Length * Marshal.SizeOf(typeof(int))));
                       
                }
               // CheckReturnValue(rc);
                
                    // Output the data of each profile
                List<ProfileData> profileDatas = new List<ProfileData>();
                int unitSize = ProfileData.CalculateDataSize(profileInfo);
                for (int i = 0; i < rsp.byGetProfCnt; i++)
                {
                        profileDatas.Add(new ProfileData(receiveBuffer, unitSize * i, profileInfo));
                }
                foreach (var profile in profileDatas)
                {
                      profDataList.Add(profile.GetCartData(_scalingMultiplier));
                }
                
                return profDataList;
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        bool _highSpeedCommsActive;
        System.Timers.Timer _timerHighSpeed;
        DeviceStatus _deviceStatus;
        public void GetProfilesHighSpeed()
        {
            try
            {
                
                var ethernetConfig = new LJV7IF_ETHERNET_CONFIG();
                uint profileCount = 10;
                var deviceStatus = DeviceStatus.UsbFast;
                LJSetting.SetOpMode(_currentDeviceId, LJV7IF_OP_MODE.HIGH_SPEED);
                // Stop and finalize high-speed data communication.
                NativeMethods.LJV7IF_StopHighSpeedDataCommunication(Define.DEVICE_ID);
                NativeMethods.LJV7IF_HighSpeedDataCommunicationFinalize(Define.DEVICE_ID);

                // Initialize the data.
                ThreadSafeBuffer.Clear(Define.DEVICE_ID);
                Rc rc = Rc.Ok;

                // Initialize the high-speed communication path
                // High-speed communication start preparations
                LJV7IF_HIGH_SPEED_PRE_START_REQ req = new LJV7IF_HIGH_SPEED_PRE_START_REQ();
                try
                {
                    uint frequency = 50;
                    uint threadId = (uint)Define.DEVICE_ID;

                    //if (deviceStatus == DeviceStatus.UsbFast) 
                    //{
                        // Initialize USB high-speed data communication
                     rc = (Rc)NativeMethods.LJV7IF_HighSpeedDataUsbCommunicationInitalize(Define.DEVICE_ID, _callback, frequency, threadId);
                    //}
                    //else
                    //{
                    // Generate the settings for Ethernet communication.
                    //    ushort highSpeedPort = 0;
                    //    _ethernetConfig.abyIpAddress = new byte[] {
                    //    Convert.ToByte(_txtIpFirstSegment.Text),
                    //    Convert.ToByte(_txtIpSecondSegment.Text),
                    //    Convert.ToByte(_txtIpThirdSegment.Text),
                    //    Convert.ToByte(_txtIpFourthSegment.Text)
                    //};
                    //    _ethernetConfig.wPortNo = Convert.ToUInt16(_txtCommandPort.Text);
                    //    highSpeedPort = Convert.ToUInt16(_txtHighSpeedPort.Text);

                    //    // Initialize Ethernet high-speed data communication
                    //    rc = (Rc)NativeMethods.LJV7IF_HighSpeedDataEthernetCommunicationInitalize(Define.DEVICE_ID, ref _ethernetConfig,
                    //        highSpeedPort, _callback, frequency, threadId);
                    //}
                    //if (!CheckReturnCode(rc)) return;
                    req.bySendPos = (byte)0;
                }
                catch(Exception)
                {
                    throw;
                }

                // High-speed data communication start preparations
                LJV7IF_PROFILE_INFO profileInfo = new LJV7IF_PROFILE_INFO();
                rc = (Rc)NativeMethods.LJV7IF_PreStartHighSpeedDataCommunication(Define.DEVICE_ID, ref req, ref profileInfo);
                CheckReturnValue(rc);

                // Start high-speed data communication.
                rc = (Rc)NativeMethods.LJV7IF_StartHighSpeedDataCommunication(Define.DEVICE_ID);
                CheckReturnValue(rc);

                //_lblReceiveProfileCount.Text = "0";
                _timerHighSpeed.Start();
            }
            catch (Exception)
            {

                throw;
            }
        }
        void FinalizeHighSpeedComms()
        {
            try
            {
                rc = (Rc)NativeMethods.LJV7IF_HighSpeedDataCommunicationFinalize(_currentDeviceId);
                CheckReturnValue(rc);               
            }
            catch (Exception)
            {

                throw;
            }
        }
        void StopHighSpeedComms()
        {
            try
            {
               rc = (Rc)NativeMethods.LJV7IF_StopHighSpeedDataCommunication(_currentDeviceId);
               CheckReturnValue(rc);
            }
            catch (Exception)
            {

                throw;
            }
        }
        int _receivedProfiles;
        void StartHighSpeedComms()
        {
            try
            {
                ThreadSafeBuffer.ClearBuffer(_currentDeviceId);
                _receivedProfiles=0;
                rc = (Rc)NativeMethods.LJV7IF_StartHighSpeedDataCommunication(_currentDeviceId);
                CheckReturnValue(rc);
                // @Point
                //  If the LJ-V does not measure the profile for 30 seconds from the start of transmission,
                //  "0x00000008" is stored in dwNotify of the callback function.
            }
            catch (Exception)
            {

                throw;
            }
        }
        void PreStartHighSpeedComms()
        {
            try
            {
                LJV7IF_HIGH_SPEED_PRE_START_REQ req = new LJV7IF_HIGH_SPEED_PRE_START_REQ();
                req.bySendPos = (byte)0;

                // @Point
                // # SendPos is used to specify which profile to start sending data from during high-speed communication.
                // # When "Overwrite" is specified for the operation when memory full and 
                //   "0: From previous send complete position" is specified for the send start position,
                //    if the LJ-V continues to accumulate profiles, the LJ-V memory will become full,
                //    and the profile at the previous send complete position will be overwritten with a new profile.
                //    In this situation, because the profile at the previous send complete position is not saved, an error will occur.

                LJV7IF_PROFILE_INFO profileInfo = new LJV7IF_PROFILE_INFO();

                rc =(Rc) NativeMethods.LJV7IF_PreStartHighSpeedDataCommunication(_currentDeviceId, ref req, ref profileInfo);
                CheckReturnValue(rc);
            }
            catch (Exception)
            {

                throw;
            }
        }
        void InitHighSpeedEthernetComms(LJV7IF_ETHERNET_CONFIG ethernetConfig,uint profileCount)
        {
            try
            {
                _deviceData.ProfileData.Clear();  //Clear the retained profile data.
                _deviceData.MeasureData.Clear();
                LJV7IF_ETHERNET_CONFIG config = ethernetConfig;
                ushort portNo =0;
                uint profileCnt = 10;                
                var rc = (Rc)NativeMethods.LJV7IF_HighSpeedDataEthernetCommunicationInitalize(_currentDeviceId, ref config,
                    portNo, _callback, profileCnt, (uint)_currentDeviceId);
            }
            catch (Exception)
            {

                throw;
            }
        }
        void InitHighSpeedUSBComms(uint profileCount)
        {
            try
            {
                _deviceData.ProfileData.Clear();  // Clear the retained profile data.
                _deviceData.MeasureData.Clear();

                var rc =(Rc) NativeMethods.LJV7IF_HighSpeedDataUsbCommunicationInitalize(_currentDeviceId, _callback, profileCount, (uint)_currentDeviceId);
                CheckReturnValue(rc);
                // @Point
                // # When the frequency of calls is low, the callback function may not be called once per specified number of profiles.
                // # The callback function is called when both of the following conditions are met.
                //   * There is one packet of received data.
                //   * The specified number of profiles have been received by the time the call frequency has been met.               

                if (rc == Rc.Ok) _deviceData.Status = DeviceStatus.UsbFast;
            }
            catch (Exception)
            {

                throw;
            }
        }
        void InitHighSpeedComms(DeviceStatus deviceStatus, LJV7IF_ETHERNET_CONFIG ethernetConfig, uint profileCount)
        {
            try
            {
               switch(deviceStatus )
               {
                    case DeviceStatus.UsbFast:
                        InitHighSpeedUSBComms(profileCount);
                        break;
                    case DeviceStatus.EthernetFast:
                        InitHighSpeedEthernetComms(ethernetConfig, profileCount);
                        break;
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
                        rc = (Rc)NativeMethods.LJV7IF_GetProfileAdvance(_currentDeviceId, ref profileInfo, pin.Pointer, dataSize, measureData);
                        CheckReturnValue(rc);
                       
                        // Response data display 
                        _measureDatas.Add(new MeasureData(0, measureData));
                        ExtractProfileData(1, ref profileInfo, profileData);
                    }
                }
                return _deviceData.ProfileData[0].GetCartData(_scalingMultiplier);
                
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
        public static void ReceiveHighSpeedData(IntPtr buffer, uint size, uint count, uint notify, uint user)
        {
            // @Point
            // Take care to only implement storing profile data in a thread save buffer in the callback function.
            // As the thread used to call the callback function is the same as the thread used to receive data,
            // the processing time of the callback function affects the speed at which data is received,
            // and may stop communication from being performed properly in some environments.

            uint profileSize = (uint)(size / Marshal.SizeOf(typeof(int)));
            List<int[]> receiveBuffer = new List<int[]>();
            int[] bufferArray = new int[profileSize * count];
            Marshal.Copy(buffer, bufferArray, 0, (int)(profileSize * count));

            // Profile data retention
            for (int i = 0; i < count; i++)
            {
                int[] oneProfile = new int[profileSize];
                Array.Copy(bufferArray, i * profileSize, oneProfile, 0, profileSize);
                receiveBuffer.Add(oneProfile);
            }

            ThreadSafeBuffer.Add((int)user, receiveBuffer, notify);
        }
        public static void CountProfileReceive(IntPtr buffer, uint size, uint count, uint notify, uint user)
        {
            // @Point
            // Take care to only implement storing profile data in a thread save buffer in the callback function.
            // As the thread used to call the callback function is the same as the thread used to receive data,
            // the processing time of the callback function affects the speed at which data is received,
            // and may stop communication from being performed properly in some environments.

            ThreadSafeBuffer.AddCount((int)user, count, notify);
        }
        static LJController()
        {
            FrequencyList = new List<string>() { "10Hz", "20Hz", "50Hz", "100Hz", "200Hz", "500Hz", "1KHz", "2KHz", "4KHz", "4.13KHz", "8KHz", "32KHz", "64KHz" };
            TriggerTypeList = new List<string>() { "Continuous", "External", "Encoder" };
        }


        public LJController(ProbeSetup probeSetup,MeasurementUnit outputUnit)
        {
            _probeSetup = probeSetup;
            _outputUnit = outputUnit;
            _scalingMultiplier = 5e-7;// outputUnit.ConversionFactor * Define.PROFILE_UNIT_MM;
            var _batchModeParms = new byte[8] {(byte)SettingDepth.Running,(byte)SettingType.Program00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x00 };
            _deviceData = new DeviceData();
            _measureDatas = new List<MeasureData>();
            _callback = new HighSpeedDataCallBack(ReceiveHighSpeedData);
            _callbackOnlyCount = new HighSpeedDataCallBack(CountProfileReceive);
        }
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
