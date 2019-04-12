//----------------------------------------------------------------------------- 
// <copyright file="NativeMethods.cs.cs" company="KEYENCE">
//	 Copyright (c) 2013 KEYENCE CORPORATION.  All rights reserved.
// </copyright>
//----------------------------------------------------------------------------- 
using System;
using System.Runtime.InteropServices;

namespace KeyenceLJLib
{
	#region Enum

	/// <summary>
	/// Return value definition
	/// </summary>
	public enum Rc
	{
		/// <summary>Normal termination</summary>
		Ok = 0x0000,
		/// <summary>Failed to open the device</summary>
		ErrOpenDevice = 0x1000,
		/// <summary>Device not open</summary>
		ErrNoDevice = 0x1001,
		/// <summary>Command send error</summary>
		ErrSend = 0x1002,
		/// <summary>Response reception error</summary>
		ErrReceive = 0x1003,
		/// <summary>Timeout</summary>
		ErrTimeout = 0x1004,
		/// <summary>No free space</summary>
		ErrNomemory = 0x1005,
		/// <summary>Parameter error</summary>
		ErrParameter = 0x1006,
		/// <summary>Received header format error</summary>
		ErrRecvFmt = 0x1007,

		/// <summary>Not open error (for high-speed communication)</summary>
		ErrHispeedNoDevice = 0x1009,
		/// <summary>Already open error (for high-speed communication)</summary>
		ErrHispeedOpenYet = 0x100A,
		/// <summary>Already performing high-speed communication error (for high-speed communication)</summary>
		ErrHispeedRecvYet = 0x100B,
		/// <summary>Insufficient buffer size</summary>
		ErrBufferShort = 0x100C,
	}
    public class ErrorCode
    {
        static public Rc GetRc(byte returnCode)
        {
            Rc rc = (Rc)returnCode;
            return rc;
        }
        static public Rc GetRc(int returnCode)
        {
            Rc rc = (Rc)returnCode;
            return rc;
        }
        static public string GetErrorMessage(int rc)
        {
            try
            {
                return GetErrorMessage(GetRc(rc));
            }
            catch (Exception)
            {

                throw;
            }
        }
        static public string GetErrorMessage(byte rc)
        {
            try
            {
                return GetErrorMessage(GetRc(rc));
            }
            catch (Exception)
            {

                throw;
            }
        }
        static public string GetErrorMessage(Rc rc)
        {
            try
            {
                switch (rc)
                {
                              
                    case Rc.ErrOpenDevice:
                        return "Failed to open the device";
                    case Rc.ErrNoDevice:
                        return "Device not open";
                    case Rc.ErrSend:
                        return "Command send error";
                    case Rc.ErrReceive:
                        return "Response reception error";
                    case Rc.ErrTimeout:
                        return "Timeout";
                    case Rc.ErrNomemory:
                        return "No free space";
                    case Rc.ErrParameter:
                        return "Parameter error";
                    case Rc.ErrRecvFmt :
                        return "Received header format error";
                    case Rc.ErrHispeedNoDevice:
                        return "Not open error (for high-speed communication)";
                    case Rc.ErrHispeedOpenYet:
                        return "Already open error (for high-speed communication)";
                    case Rc.ErrHispeedRecvYet:
                        return "Already performing high-speed communication error (for high-speed communication)";
                    case Rc.ErrBufferShort:
                        return "Insufficient buffer size";
                    case Rc.Ok:
                        return " Normal termination";
                    default:
                        return " Unknown Error Return Code: " +string.Format(" 0x{0,8:x}", rc);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
	/// <summary>
	/// Definition that indicates the validity of a measurement value
	/// </summary>
	public enum LJV7IF_MEASURE_DATA_INFO
	{
		LJV7IF_MEASURE_DATA_INFO_VALID = 0x00,	// Valid
		LJV7IF_MEASURE_DATA_INFO_ALARM = 0x01,	// Alarm value
		LJV7IF_MEASURE_DATA_INFO_WAIT = 0x02	// Judgment wait value
	}
    public enum LJV7IF_OP_MODE
    {
        HIGH_SPEED,
        ADVANCED
    }
    public enum LJV7IF_MEM_ALLOCATION
    {
        DOUBLE_BUFFER,
        OVERWRITE,
        NO_OVERWRITE
    }
    public enum LJV7IF_FULL_MEM_OP
    {
        OVERWRITE,
        STOP
    }
    public enum LJV7IF_BATCH_MODE
    {
        OFF,
        ON
    }
    public enum LJV7IF_FREQUENCY
    {
        F10HZ,
        F20HZ,
        F50HZ,
        F100HZ,
        F200HZ,
        F500HZ,
        F1KHZ,
        F2KHZ,
        F4KHZ,
        F413KHZ,
        F8KHZ,
        F16KHZ,
        F32KHZ,
        F64KHZ
    }
    /// <summary>
    /// Definition that indicates the tolerance judgment result of the measurement
    /// </summary>
    public enum LJV7IF_JUDGE_RESULT
	{
		LJV7IF_JUDGE_RESULT_HI = 0x01,	// HI
		LJV7IF_JUDGE_RESULT_GO = 0x02,	// GO
		LJV7IF_JUDGE_RESULT_LO = 0x04	// LO
	}
    public enum LJV7IF_TRIGGER_MODE
    {
        CONTINUOUS,
        EXTERNAL,
        ENCODER
    }
	/// Get batch profile position specification method designation
	public enum BatchPos : byte
	{
		/// <summary>From current</summary>
		Current = 0x00,
		/// <summary>Specify position</summary>
		Spec = 0x02,
		/// <summary>From current after commitment</summary>
		Commited = 0x03,
		/// <summary>Current only</summary>
		CurrentOnly = 0x04,
	};

	/// Setting value storage level designation
	public enum SettingDepth : byte
	{
		/// <summary>Settings write area</summary>
		Write = 0x00,
		/// <summary>Active measurement area</summary>
		Running = 0x01,
		/// <summary>Save area</summary>
		Save = 0x02,
	};

	/// Definition that indicates the "setting type" in LJV7IF_TARGET_SETTING structure.
	public enum SettingType : byte
	{
		/// <summary>Environment setting</summary>
		Environment = 0x01,
		/// <summary>Common measurement setting</summary>
		Common = 0x02,
		/// <summary>Measurement Program setting</summary>
		Program00 = 0x10,
		Program01,
		Program02,
		Program03,
		Program04,
		Program05,
		Program06,
		Program07,
		Program08,
		Program09,
		Program10,
		Program11,
		Program12,
		Program13,
		Program14,
		Program15,
	};

	/// Get profile target buffer designation
	public enum ProfileBank : byte
	{
		/// <summary>Active surface</summary>
		Active = 0x00,
		/// <summary>Inactive surface</summary>	
		Inactive = 0x01,
	};

	/// Get profile position specification method designation
	public enum ProfilePos : byte
	{
		/// <summary>From current</summary>
		Current = 0x00,
		/// <summary>From oldest</summary>
		Oldest = 0x01,
		/// <summary>Specify position</summary>
		Spec = 0x02,
	};

	#endregion

	#region Structure
	/// <summary>
	/// Ethernet settings structure
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LJV7IF_ETHERNET_CONFIG
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public byte[] abyIpAddress;
		public ushort wPortNo;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public byte[] reserve;

	};

	/// <summary>
	/// Date and time structure
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LJV7IF_TIME
	{
		public byte byYear;
		public byte byMonth;
		public byte byDay;
		public byte byHour;
		public byte byMinute;
		public byte bySecond;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public byte[] reserve;
	};

	/// <summary>
	/// Setting item designation structure
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LJV7IF_TARGET_SETTING
	{
		public byte byType;
		public byte byCategory;
		public byte byItem;
		public byte reserve;
		public byte byTarget1;
		public byte byTarget2;
		public byte byTarget3;
		public byte byTarget4;
	};

	/// <summary>
	/// Measurement results structure
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LJV7IF_MEASURE_DATA
	{
		public byte byDataInfo;
		public byte byJudge;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public byte[] reserve;
		public float fValue;
	};

	/// <summary>
	/// Profile information structure
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LJV7IF_PROFILE_INFO
	{
		public byte byProfileCnt;
		public byte byEnvelope;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public byte[] reserve;
		public short wProfDataCnt;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public byte[] reserve2;
		public int lXStart;
		public int lXPitch;
	};

	/// <summary>
	/// Profile header information structure
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LJV7IF_PROFILE_HEADER
	{
		public uint reserve;
		public uint dwTriggerCnt;
		public uint dwEncoderCnt;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
		public uint[] reserve2;
	};

	/// <summary>
	/// Profile footer information structure
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LJV7IF_PROFILE_FOOTER
	{
		public uint reserve;
	};

	/// <summary>
	/// High-speed mode get profile request structure (batch measurement: off)
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LJV7IF_GET_PROFILE_REQ
	{
		public byte byTargetBank;
		public byte byPosMode;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public byte[] reserve;
		public uint dwGetProfNo;
		public byte byGetProfCnt;
		public byte byErase;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public byte[] reserve2;
	};

	/// <summary>
	/// High-speed mode get profile request structure (batch measurement: on)
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LJV7IF_GET_BATCH_PROFILE_REQ
	{
		public byte byTargetBank;
		public byte byPosMode;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public byte[] reserve;
		public uint dwGetBatchNo;
		public uint dwGetProfNo;
		public byte byGetProfCnt;
		public byte byErase;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public byte[] reserve2;
	};

	/// <summary>
	/// Advanced mode get profile request structure (batch measurement: on)
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LJV7IF_GET_BATCH_PROFILE_ADVANCE_REQ
	{
		public byte byPosMode;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
		public byte[] reserve;
		public uint dwGetBatchNo;
		public uint dwGetProfNo;
		public byte byGetProfCnt;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
		public byte[] reserve2;
	};

	/// <summary>
	/// High-speed mode get profile response structure (batch measurement: off)
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LJV7IF_GET_PROFILE_RSP
	{
		public uint dwCurrentProfNo;
		public uint dwOldestProfNo;
		public uint dwGetTopProfNo;
		public byte byGetProfCnt;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
		public byte[] reserve;
	};

	/// <summary>
	/// High-speed mode get profile response structure (batch measurement: on)
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LJV7IF_GET_BATCH_PROFILE_RSP
	{
		public uint dwCurrentBatchNo;
		public uint dwCurrentBatchProfCnt;
		public uint dwOldestBatchNo;
		public uint dwOldestBatchProfCnt;
		public uint dwGetBatchNo;
		public uint dwGetBatchProfCnt;
		public uint dwGetBatchTopProfNo;
		public byte byGetProfCnt;
		public byte byCurrentBatchCommited;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public byte[] reserve;
	};

	/// <summary>
	/// Advanced mode get profile response structure (batch measurement: on)
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LJV7IF_GET_BATCH_PROFILE_ADVANCE_RSP
	{
		public uint dwGetBatchNo;
		public uint dwGetBatchProfCnt;
		public uint dwGetBatchTopProfNo;
		public byte byGetProfCnt;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
		public byte[] reserve;
	};

	/// <summary>
	/// Storage status request structure
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LJV7IF_GET_STRAGE_STATUS_REQ
	{
		public uint dwRdArea;		// Target surface to read
	};

	/// <summary>
	/// Storage status response structure
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LJV7IF_GET_STRAGE_STATUS_RSP
	{
		public uint dwSurfaceCnt;		// Storage surface number
		public uint dwActiveSurface;	// Active storage surface
	};

	/// <summary>
	/// Storage information structure
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LJV7IF_STORAGE_INFO
	{
		public byte byStatus;
		public byte byProgramNo;
		public byte byTarget;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
		public byte[] reserve;
		public uint dwStorageCnt;
	};

	/// <summary>
	/// Get storage data request structure
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LJV7IF_GET_STORAGE_REQ
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public byte[] reserve;
		public uint dwSurface;
		public uint dwStartNo;
		public uint dwDataCnt;
	};

	/// <summary>
	/// Get batch profile storage request structure
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LJV7IF_GET_BATCH_PROFILE_STORAGE_REQ
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public byte[] reserve;
		public uint dwSurface;
		public uint dwGetBatchNo;
		public uint dwGetBatchTopProfNo;
		public byte byGetProfCnt;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
		public byte[] reserved;
	};

	/// <summary>
	/// Get storage data response structure
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LJV7IF_GET_STORAGE_RSP
	{
		public uint dwStartNo;
		public uint dwDataCnt;
		public LJV7IF_TIME stBaseTime;
	};
	/// <summary>
	/// Get batch profile storage response structure
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LJV7IF_GET_BATCH_PROFILE_STORAGE_RSP
	{
		public uint dwGetBatchNo;
		public uint dwGetBatchProfCnt;
		public uint dwGetBatchTopProfNo;
		public byte byGetProfCnt;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
		public byte[] reserve;
		public LJV7IF_TIME stBaseTime;
	};

	/// <summary>
	/// High-speed communication start preparation request structure
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LJV7IF_HIGH_SPEED_PRE_START_REQ
	{
		public byte bySendPos;		// Send start position
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
		public byte[] reserve;		// Reservation 
	};

	#endregion

	#region Method
	/// <summary>
	/// Callback function for high-speed communication
	/// </summary>
	/// <param name="buffer">Received profile data pointer</param>
	/// <param name="size">Size in units of bytes of one profile</param>
	/// <param name="count">Number of profiles</param>
	/// <param name="notify">Finalization condition</param>
	/// <param name="user">Thread ID</param>
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void HighSpeedDataCallBack(IntPtr buffer, uint size, uint count, uint notify, uint user);

    public class LJSetting
    {
        #region Field
        /// <summary>
        /// Specify to what level the sent settings will be reflected (LJV7IF_SETTING_DEPTH).
        /// </summary>
        private byte _depth;

        /// <summary>
        /// Identify the item that is the target to send.
        /// </summary>
        private LJV7IF_TARGET_SETTING _targetSetting;

        /// <summary>
        /// Specify the buffer that stores the setting data to send.
        /// </summary>
        private byte[] _data;
        #endregion

        #region Property
        /// <summary>
        /// Specify to what level the sent settings will be reflected (LJV7IF_SETTING_DEPTH).
        /// </summary>
        public byte Depth
        {
            get { return   _depth; }
        }

        /// <summary>
        /// Identify the item that is the target to send.
        /// </summary>
        public LJV7IF_TARGET_SETTING TargetSetting
        {
            get { return _targetSetting; }
        }
        public int DataLength
        {
            get { return Convert.ToInt32(_data.Length); }
        }
        /// <summary>
        /// Specify the buffer that stores the setting data to send.
        /// </summary>
        public byte[] Data
        {
            get { return _data; }
        }
        #endregion

        static public void SetOpMode(int deviceId, LJV7IF_OP_MODE data)
        {
            try
            {
                byte category = 0;
                byte item = 0;
                var setting = new LJSetting((byte)SettingDepth.Running, (byte)SettingType.Common, category, item,
                    (byte)0, (byte)0, (byte)0, (byte)0, (byte)data);
                SetSetting(deviceId, setting);
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        static public LJV7IF_OP_MODE GetOpMode(int deviceId)
        {
            try
            {
                byte category = 0;
                byte item = 0;
                byte[] data = new byte[1];
                var setting = new LJSetting((byte)SettingDepth.Running, (byte)SettingType.Common, category, item,
                    (byte)0, (byte)0, (byte)0, (byte)0, data);
                data =  GetSetting(deviceId, setting);
                return (LJV7IF_OP_MODE)(data[0]);
            }
            catch (Exception)
            {

                throw;
            }
        }
        static public void SetMemAllocation(int deviceId, LJV7IF_MEM_ALLOCATION data)
        {
            try
            {
                byte category = 0;
                byte item = 1;
                var setting = new LJSetting((byte)SettingDepth.Running, (byte)SettingType.Common, category, item,
                    (byte)0, (byte)0, (byte)0, (byte)0, (byte)data);
                SetSetting(deviceId, setting);
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        static public void SetMemFullOp(int deviceId, LJV7IF_FULL_MEM_OP data)
        {
            try
            {
                byte category = 0;
                byte item = 2;
                var setting = new LJSetting((byte)SettingDepth.Running, (byte)SettingType.Common, category, item,
                    (byte)0, (byte)0, (byte)0, (byte)0, (byte)data);
                SetSetting(deviceId, setting);
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        static public void SetTriggerMode(int deviceId, SettingType settingType, LJV7IF_TRIGGER_MODE data)
        {
            try
            {
                byte category = 0;
                byte item = 1;
                var setting = new LJSetting((byte)SettingDepth.Running, (byte)settingType, category, item,
                    (byte)0, (byte)0, (byte)0, (byte)0, (byte)data);
                SetSetting(deviceId, setting);
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        static public void SetTriggerFreq(int deviceId, SettingType settingType, LJV7IF_FREQUENCY data)
        {
            try
            {
                byte category = 0;
                byte item = 2;
                var setting = new LJSetting((byte)SettingDepth.Running, (byte)settingType, category, item,
                    (byte)0, (byte)0, (byte)0, (byte)0, (byte)data);
                SetSetting(deviceId, setting);
            }
            catch (Exception)
            {

                throw;
            }
           
        }
        static public void SetBatchMode(int deviceId, SettingType settingType, LJV7IF_BATCH_MODE data)
        {
            try
            {
                byte category = 0;
                byte item = 3;
                var setting = new LJSetting((byte)SettingDepth.Running, (byte)settingType, category, item,
                    (byte)0, (byte)0, (byte)0, (byte)0, (byte)data);
                SetSetting(deviceId, setting);
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        static byte[] GetSetting(int deviceId, LJSetting lJSetting)
        {
            try
            {
                LJV7IF_TARGET_SETTING targetSetting = lJSetting.TargetSetting;
                byte[] data = new byte[lJSetting.DataLength];
                using (PinnedObject pin = new PinnedObject(data))
                {
                    int rc = NativeMethods.LJV7IF_GetSetting(deviceId, lJSetting.Depth, targetSetting,
                        pin.Pointer, (uint)lJSetting.DataLength);

                }
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }
        static void SetSetting(int deviceId, LJSetting lJSetting)
        {
            try
            {
                using (PinnedObject pin = new PinnedObject(lJSetting.Data))
                {
                    uint dwError = 0;
                    int rc = NativeMethods.LJV7IF_SetSetting(deviceId, lJSetting.Depth, lJSetting.TargetSetting,
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
                    if(ErrorCode.GetRc(rc) != Rc.Ok)
                    {
                        throw new Exception(ErrorCode.GetErrorMessage(rc));
                    }
                    
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public LJSetting(byte depth, byte type, byte category, byte item, byte target1, byte target2, byte target3, byte target4, params byte[] data)
        {
            _targetSetting = new LJV7IF_TARGET_SETTING();
            _depth = depth ;
            _targetSetting.byType =(byte) (type);
            _targetSetting.byCategory = category;
            _targetSetting.byItem = item;
            _targetSetting.byTarget1 = target1;
            _targetSetting.byTarget2 = target2;
            _targetSetting.byTarget3 = target3;
            _targetSetting.byTarget4 = target4;
            var dataList = new System.Collections.Generic.List<byte>();
            foreach (var val in data)
            {
                dataList.Add(val);
            }
            _data = dataList.ToArray();
        }
    }
    /// <summary>
    /// Function definitions
    /// </summary>
    public class NativeMethods
	{
		/// <summary>
		/// Get measurement results (the data of all 16 OUTs, including those that are not being measured, is stored).
		/// </summary>
		public static int MeasurementDataCount
		{
			get { return 16; }
		}

		/// <summary>
		/// Number of connectable devices
		/// </summary>
		public static int DeviceCount
		{
			get { return 6; }
		}
		
		/// <summary>
		/// Fixed value for the bytes of environment settings data 
		/// </summary>
		public static UInt32 EnvironmentSettingSize
		{
			get { return 60; }
		}

		/// <summary>
		/// Fixed value for the bytes of common measurement settings data 
		/// </summary>
		public static UInt32 CommonSettingSize
		{
			get { return 12; }
		}

		/// <summary>
		/// Fixed value for the bytes of program settings data 
		/// </summary>
		public static UInt32 ProgramSettingSize
		{
			get { return 10932; }
		}

		[DllImport("LJV7_IF.dll")]
		public static extern int LJV7IF_Initialize();

		[DllImport("LJV7_IF.dll")]
		public static extern int LJV7IF_Finalize();

		[DllImport("LJV7_IF.dll")]
		public static extern uint LJV7IF_GetVersion();

		[DllImport("LJV7_IF.dll")]
        public static extern int LJV7IF_UsbOpen(int lDeviceId);
        public static Rc UsbOpen(int lDeviceId)
        {
            return  (Rc)LJV7IF_UsbOpen(lDeviceId);
            
        }
		[DllImport("LJV7_IF.dll")]
		public static extern int LJV7IF_EthernetOpen(int lDeviceId, ref LJV7IF_ETHERNET_CONFIG ethernetConfig);

		[DllImport("LJV7_IF.dll")]
		public static extern int LJV7IF_CommClose(int lDeviceId);

		[DllImport("LJV7_IF.dll")]
		public static extern int LJV7IF_RebootController(int lDeviceId);

		[DllImport("LJV7_IF.dll")]
		public static extern int LJV7IF_RetrunToFactorySetting(int lDeviceId);

		[DllImport("LJV7_IF.dll")]
		public static extern int LJV7IF_GetError(int lDeviceId, byte byRcvMax, ref byte pbyErrCnt, IntPtr pwErrCode);

		[DllImport("LJV7_IF.dll")]
		public static extern int LJV7IF_ClearError(int lDeviceId, short wErrCode);

		[DllImport("LJV7_IF.dll")]
		public static extern int LJV7IF_Trigger(int lDeviceId);

		[DllImport("LJV7_IF.dll")]
		public static extern int LJV7IF_StartMeasure(int lDeviceId);

		[DllImport("LJV7_IF.dll")]
		public static extern int LJV7IF_StopMeasure(int lDeviceId);

		[DllImport("LJV7_IF.dll")]
		public static extern int LJV7IF_AutoZero(int lDeviceId, byte byOnOff, uint dwOut);

		[DllImport("LJV7_IF.dll")]
		public static extern int LJV7IF_Timing(int lDeviceId, byte byOnOff, uint dwOut);

		[DllImport("LJV7_IF.dll")]
		public static extern int LJV7IF_Reset(int lDeviceId, uint dwOut);

		[DllImport("LJV7_IF.dll")]
		public static extern int LJV7IF_ClearMemory(int lDeviceId);

		[DllImport("LJV7_IF.dll")]
		public static extern int LJV7IF_SetSetting(int lDeviceId, byte byDepth, LJV7IF_TARGET_SETTING TargetSetting, IntPtr pData, uint dwDataSize, ref uint pdwError);

		[DllImport("LJV7_IF.dll")]
		public static extern int LJV7IF_GetSetting(int lDeviceId, byte byDepth, LJV7IF_TARGET_SETTING TargetSetting, IntPtr pData, uint dwDataSize);

		[DllImport("LJV7_IF.dll")]
		public static extern int LJV7IF_InitializeSetting(int lDeviceId, byte byDepth, byte byTarget);

		[DllImport("LJV7_IF.dll")]
		public static extern int LJV7IF_ReflectSetting(int lDeviceId, byte byDepth, ref uint pdwError);

		[DllImport("LJV7_IF.dll")]
		public static extern int LJV7IF_RewriteTemporarySetting(int lDeviceId, byte byDepth);

		[DllImport("LJV7_IF.dll")]
		public static extern int LJV7IF_CheckMemoryAccess(int lDeviceId, ref byte pbyBusy);

		[DllImport("LJV7_IF.dll")]
		public static extern int LJV7IF_SetTime(int lDeviceId, ref LJV7IF_TIME time);

		[DllImport("LJV7_IF.dll")]
		public static extern int LJV7IF_GetTime(int lDeviceId, ref LJV7IF_TIME time);

		[DllImport("LJV7_IF.dll")]
		public static extern int LJV7IF_ChangeActiveProgram(int lDeviceId, byte byProgNo);

		[DllImport("LJV7_IF.dll")]
		public static extern int LJV7IF_GetActiveProgram(int lDeviceId, ref byte pbyProgNo);

		[DllImport("LJV7_IF.dll")]
		public static extern int LJV7IF_GetMeasurementValue(int lDeviceId, [Out]LJV7IF_MEASURE_DATA[] pMeasureData);

		[DllImport("LJV7_IF.dll")]
		public static extern int LJV7IF_GetProfile(int lDeviceId, ref LJV7IF_GET_PROFILE_REQ pReq,
		ref LJV7IF_GET_PROFILE_RSP pRsp, ref LJV7IF_PROFILE_INFO pProfileInfo, IntPtr pdwProfileData, uint dwDataSize);

		[DllImport("LJV7_IF.dll")]
		public static extern int LJV7IF_GetBatchProfile(int lDeviceId, ref LJV7IF_GET_BATCH_PROFILE_REQ pReq,
		ref LJV7IF_GET_BATCH_PROFILE_RSP pRsp, ref LJV7IF_PROFILE_INFO pProfileInfo,
		IntPtr pdwBatchData, uint dwDataSize);

		[DllImport("LJV7_IF.dll")]
		public static extern int LJV7IF_GetProfileAdvance(int lDeviceId, ref LJV7IF_PROFILE_INFO pProfileInfo,
		IntPtr pdwProfileData, uint dwDataSize, [Out]LJV7IF_MEASURE_DATA[] pMeasureData);

		[DllImport("LJV7_IF.dll")]
		public static extern int LJV7IF_GetBatchProfileAdvance(int lDeviceId, ref LJV7IF_GET_BATCH_PROFILE_ADVANCE_REQ pReq,
		ref LJV7IF_GET_BATCH_PROFILE_ADVANCE_RSP pRsp, ref LJV7IF_PROFILE_INFO pProfileInfo,
		IntPtr pdwBatchData, uint dwDataSize, [Out]LJV7IF_MEASURE_DATA[] pBatchMeasureData, [Out]LJV7IF_MEASURE_DATA[] pMeasureData);

		[DllImport("LJV7_IF.dll")]
		public static extern int LJV7IF_StartStorage(int lDeviceId);

		[DllImport("LJV7_IF.dll")]
		public static extern int LJV7IF_StopStorage(int lDeviceId);

		[DllImport("LJV7_IF.dll")]
		public static extern int LJV7IF_GetStorageStatus(int lDeviceId, ref LJV7IF_GET_STRAGE_STATUS_REQ pReq,
		ref LJV7IF_GET_STRAGE_STATUS_RSP pRsp, ref LJV7IF_STORAGE_INFO pStorageInfo);

		[DllImport("LJV7_IF.dll")]
		public static extern int LJV7IF_GetStorageData(int lDeviceId, ref LJV7IF_GET_STORAGE_REQ pReq,
		ref LJV7IF_STORAGE_INFO pStorageInfo, ref LJV7IF_GET_STORAGE_RSP pRsp, IntPtr pdwData, uint dwDataSize);

		[DllImport("LJV7_IF.dll")]
		public static extern int LJV7IF_GetStorageProfile(int lDeviceId, ref LJV7IF_GET_STORAGE_REQ pReq,
		ref LJV7IF_STORAGE_INFO pStorageInfo, ref LJV7IF_GET_STORAGE_RSP pRes,
		ref LJV7IF_PROFILE_INFO pProfileInfo, IntPtr pdwData, uint dwDataSize);

		[DllImport("LJV7_IF.dll")]
		public static extern int LJV7IF_GetStorageBatchProfile(int lDeviceId,
		ref LJV7IF_GET_BATCH_PROFILE_STORAGE_REQ pReq, ref LJV7IF_STORAGE_INFO pStorageInfo,
		ref LJV7IF_GET_BATCH_PROFILE_STORAGE_RSP pRes, ref LJV7IF_PROFILE_INFO pProfileInfo,
		IntPtr pdwData, uint dwDataSize, ref uint pTimeOffset, [Out]LJV7IF_MEASURE_DATA[] pMeasureData);

		[DllImport("LJV7_IF.dll")]
		public static extern int LJV7IF_HighSpeedDataUsbCommunicationInitalize(int lDeviceId, HighSpeedDataCallBack pCallBack,
		uint dwProfileCnt, uint dwThreadId);

		[DllImport("LJV7_IF.dll")]
		public static extern int LJV7IF_HighSpeedDataEthernetCommunicationInitalize(
		int lDeviceId, ref LJV7IF_ETHERNET_CONFIG pEthernetConfig, ushort wHighSpeedPortNo,
		HighSpeedDataCallBack pCallBack, uint dwProfileCnt, uint dwThreadId);

		[DllImport("LJV7_IF.dll")]
		public static extern int LJV7IF_PreStartHighSpeedDataCommunication(
		int lDeviceId, ref LJV7IF_HIGH_SPEED_PRE_START_REQ pReq,
		ref LJV7IF_PROFILE_INFO pProfileInfo);

		[DllImport("LJV7_IF.dll")]
		public static extern int LJV7IF_StartHighSpeedDataCommunication(int lDeviceId);

		[DllImport("LJV7_IF.dll")]
		public static extern int LJV7IF_StopHighSpeedDataCommunication(int lDeviceId);

		[DllImport("LJV7_IF.dll")]
		public static extern int LJV7IF_HighSpeedDataCommunicationFinalize(int lDeviceId);
	}
	#endregion

}
