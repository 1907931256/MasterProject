using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
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
        int OutNo;          // OUT No(0-11)
        SIIF_FLOATRESULT FloatResult;   // valid or invalid data
        float Value;            // Measurement value
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct SIIF_FLOATVALUE
    {
        SIIF_FLOATRESULT FloatResult;   // valid or invalid data
        float Value;            // Measurement value
    }

    //// IP address
    //[StructLayout(LayoutKind.Sequential)]
    //public struct SIIF_OPENPARAM_ETHERNET
    //{
    //    IN_ADDR IPAddress;
    //} 
    public class NativeMethods
    {
        [DllImport("SIIF.dll")]
        public static extern int SIIF_OpenDeviceUsb();
        [DllImport("SIIF.dll")]
        public static extern int SIIF_CloseDevice();
        [DllImport("SIIF.dll")]
        public static extern int SIIF_GetCalcDataSingle(long outNo, ref SIIF_FLOATVALUE_OUT calcData);
    }
}
