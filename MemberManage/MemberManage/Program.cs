using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.Net;
using System.IO;
using System.Timers;

namespace MemberManage
{
    class Program
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;
        //DISPENSER_CMD
        public const byte DISPENSER_RESET = (0x31);
        public const byte DISPENSER_ISSUE_1 = (0x32);
        public const byte DISPENSER_ISSUE_2 = (0x33);
        public const byte DISPENSER_CALL_BACK = (0x34);
        //COLLECTOR_CMD
        public const byte COLLECTOR_RESET = (0x35);
        public const byte COLLECTOR_RECEIVE = (0x36);
        public const byte COLLECTOR_RECEDE = (0x37);
        //LIGHT CMD
        public const byte LIGHT_OPEN = (0x01);
        public const byte LIGHT_CLOSE = (0x00);
        //BARRIER CMD
        public const byte BARRIER_CLOSE = (0x00);
        public const byte BARRIER_OPEN = (0x01);
        public const byte BARRIER_STOP = (0x02);
        //RELAY_CMD
        public const byte RELAY1_OFF = (0x10);
        public const byte RELAY1_ON = (0x11);
        public const byte RELAY2_OFF = (0x20);
        public const byte RELAY2_ON = (0x21);
        public const byte RELAY3_OFF = (0x30);
        public const byte RELAY3_ON = (0x31);
        public const byte RELAY4_OFF = (0x40);
        public const byte RELAY4_ON = (0x41);

        //STRUCT
        [StructLayout(LayoutKind.Sequential)]
        public struct CARD_NUMBER_MSG
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] ControllerIP;
            public byte CommandSeq;
            public byte ReaderID;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] CardID;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct IC_CARD_MSG
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] ControllerIP;
            public byte CommandSeq;
            public byte ReaderID;
            public byte ReadState;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] CardID;
            public byte SectorNum;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 48)]
            public byte[] SectorData;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct WRITE_IC_RETURN
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] ControllerIP;
            public uint CommandSeq;
            public uint ReaderID;
            public uint WriteState;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] CardID;
            public uint SectorNum;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct ENTERED_IC_CARD_MSG
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] ControllerIP;
            public uint CommandSeq;
            public uint ReaderID;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] CardID;
            public uint CardType;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
            public byte[] EnterTime;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct EXITED_IC_CARD_MSG
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] ControllerIP;
            public uint CommandSeq;
            public uint ReaderID;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] CardID;
            public uint CardType;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
            public byte[] EnterTime;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
            public byte[] ExitTime;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct ID_CARD_RECORD_MSG
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] ControllerIP;
            public uint CommandSeq;
            public uint Seq_Num;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] CardID;
            public uint CardType;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
            public byte[] EnterTime;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct BAR_CODE_MSG
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] ControllerIP;
            public uint CommandSeq;
            public uint DataLen;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
            public byte[] BarData;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct IO_STATE_MSG
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] ControllerIP;
            public uint CommandSeq;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] IO_Data;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] ChangeBitBytes;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct RS232_RECEIVED_DATA_MSG
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] ControllerIP;
            public uint CommandSeq;
            public uint ReaderNum;
            public uint DataLen;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
            public byte[] RevData;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CALLBACK_FUN
        {
            public ReadedCardNumber p_ReadedCardNumberCallBack;
            public ReadedICCard p_ReadedICCardCallBack;
            public WriteICReturn p_WriteICReturnCallBack;
            public EnteredICCard p_EnterICCardCallBack;
            public ExitedICCard p_ExitedICCardCallBack;
            public CtrUploadIDRecord p_CtrUploadIDRecordCallBack;
            public CtrUploadBarCode p_CtrUploadBarCodeCallBack;
            public CtrUploadIOState p_CtrUploadIOStateCallBack;
            public CtrUploadRS232Data p_CtrUploadRS232DataCallBack;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct STRUCT_SYS_CONFIG_INFO
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] TermIP;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] Gatewate;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] SubNetMask;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] ServerIP;
            public uint Wg1WorkMode;
            public uint Wg2WorkMode;
            public uint Com1WorkMode;
            public uint Com2WorkMode;

        }
        [StructLayout(LayoutKind.Sequential)]
        public struct STRUCT_SET_TIME
        {
            public ushort Year;
            public byte Month;
            public byte Date;
            public byte Hour;
            public byte Minute;
            public byte Second;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct STRUCT_RD_IC
        {
            public byte ReaderNum;
            public byte SectorNum;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public byte[] Key;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct STRUCT_WR_IC
        {
            public byte ReaderNum;
            public uint CardID;
            public byte SectorNum;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public byte[] Key;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] Block0Data;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] Block1Data;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] Block2Data;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct STRUCT_LED_DISPLAY
        {
            public byte DisDataLen;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
            public byte[] DisData;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct STRUCT_PLAY_VOICE
        {
            public byte VoiceNum;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] VoiceBuff;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct STRUCT_PLAY_CHARGE
        {
            public byte CarType;
            public ushort Charge;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct STRUCT_PLAY_PERIOD
        {
            public ushort Year;
            public byte Month;
            public byte Day;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct STRUCT_RS485_TRANS
        {
            public byte Translen;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
            public byte[] TransData;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct STRUCT_RS232_TRANS
        {
            public byte ComNumber;
            public byte Translen;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
            public byte[] TransData;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct STRUCT_IO_STATE
        {
            public byte IOStateData1;
            public byte IOStateData2;
            public byte IOStateData3;
            public byte IOStateData4;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct STRUCT_CARD_LIST
        {
            public ushort Sequence;
            public uint CardID;
            public byte CardType;
            public ushort StopEffectTimeYear;
            public byte StopEffectTimeMonth;
            public byte StopEffectTimeDay;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public byte[] PlateNo;
        }


        //CALLBACK
        public delegate void ReadedCardNumber(uint dwUser, ref CARD_NUMBER_MSG p_card_msg);
        public delegate void ReadedICCard(uint dwUser, ref IC_CARD_MSG p_ic_card_msg);
        public delegate void WriteICReturn(uint dwUser, ref WRITE_IC_RETURN p_write_ic_return);
        public delegate void EnteredICCard(uint dwUser, ref ENTERED_IC_CARD_MSG p_entered_ic_msg);
        public delegate void ExitedICCard(uint dwUser, ref EXITED_IC_CARD_MSG p_exited_ic_msg);
        public delegate void CtrUploadIDRecord(uint dwUser, ref ID_CARD_RECORD_MSG p_id_record_msg);
        public delegate void CtrUploadBarCode(uint dwUser, ref BAR_CODE_MSG p_bar_code_msg);
        public delegate void CtrUploadIOState(uint dwUser, ref IO_STATE_MSG p_io_state_msg);
        public delegate void CtrUploadRS232Data(uint dwUser, ref RS232_RECEIVED_DATA_MSG p_rs232_msg);

        [DllImport("TCP102_SDK.dll", EntryPoint = "?TCP102_InitDll@@YGEK@Z")]
        public unsafe static extern byte TCP102_InitDll(uint dwUser);
        [DllImport("TCP102_SDK.dll", EntryPoint = "?TCP102_GetDllVersion@@YGEPAE@Z")]
        public unsafe static extern byte TCP102_GetDllVersion(uint* version);
        [DllImport("TCP102_SDK.dll", EntryPoint = "?TCP102_InitCallBack@@YGEKPAU_CALLBACK_FUN@@@Z")]
        public unsafe static extern byte TCP102_InitCallBack(uint dwUser, ref CALLBACK_FUN p_call_back);
        [DllImport("TCP102_SDK.dll", EntryPoint = "?TCP102_ResetController@@YGEPADEE@Z")]
        public unsafe static extern byte TCP102_ResetController(string ctr_ip, uint sequence, uint time_out);
        [DllImport("TCP102_SDK.dll", EntryPoint = "?TCP102_ReadControllerVersion@@YGEPADEEPAE@Z")]
        public unsafe static extern byte TCP102_ReadControllerVersion(string ctr_ip, uint sequence, uint time_out, byte[] ver_data);
        [DllImport("TCP102_SDK.dll", EntryPoint = "?TCP102_DownLoadConfigInfo@@YGEPADEEPAUSTRUCT_SYS_CONFIG_INFO@@@Z")]
        public unsafe static extern byte TCP102_DownLoadConfigInfo(string ctr_ip, uint sequence, uint time_out, ref STRUCT_SYS_CONFIG_INFO cofig_info);
        [DllImport("TCP102_SDK.dll", EntryPoint = "?TCP102_SetTime@@YGEPADEEPAUSTRUCT_SET_TIME@@@Z")]
        public unsafe static extern byte TCP102_SetTime(string ctr_ip, uint sequence, uint time_out, ref STRUCT_SET_TIME time);
        [DllImport("TCP102_SDK.dll", EntryPoint = "?TCP102_ReadICCard@@YGEPADEEPAUSTRUCT_RD_IC@@@Z")]
        public unsafe static extern byte TCP102_ReadICCard(string ctr_ip, uint sequence, uint time_out, ref STRUCT_RD_IC struct_rd_ic);
        [DllImport("TCP102_SDK.dll", EntryPoint = "?TCP102_WriteICCard@@YGEPADEEPAUSTRUCT_WR_IC@@@Z")]
        public unsafe static extern byte TCP102_WriteICCard(string ctr_ip, uint sequence, uint time_out, ref STRUCT_WR_IC struct_wr_ic);
        [DllImport("TCP102_SDK.dll", EntryPoint = "?TCP102_LedDisplay@@YGEPADEEPAUSTRUCT_LED_DISPLAY@@@Z")]
        public unsafe static extern byte TCP102_LedDisplay(string ctr_ip, uint sequence, uint time_out, ref STRUCT_LED_DISPLAY led_dis);
        [DllImport("TCP102_SDK.dll", EntryPoint = "?TCP102_PlayVoice@@YGEPADEEPAUSTRUCT_PLAY_VOICE@@@Z")]
        public unsafe static extern byte TCP102_PlayVoice(string ctr_ip, uint sequence, uint time_out, ref STRUCT_PLAY_VOICE voice);
        [DllImport("TCP102_SDK.dll", EntryPoint = "?TCP102_PlayCharge@@YGEPADEEPAUSTRUCT_PLAY_CHARGE@@@Z")]
        public unsafe static extern byte TCP102_PlayCharge(string ctr_ip, uint sequence, uint time_out, ref STRUCT_PLAY_CHARGE charge);
        [DllImport("TCP102_SDK.dll", EntryPoint = "?TCP102_PlayPeriod@@YGEPADEEPAUSTRUCT_PLAY_PERIOD@@@Z")]
        public unsafe static extern byte TCP102_PlayPeriod(string ctr_ip, uint sequence, uint time_out, ref STRUCT_PLAY_PERIOD period);
        [DllImport("TCP102_SDK.dll", EntryPoint = "?TCP102_PlayPeriodDays@@YGEPADEEG@Z")]
        public unsafe static extern byte TCP102_PlayPeriodDays(string ctr_ip, uint sequence, uint time_out, ushort days);
        [DllImport("TCP102_SDK.dll", EntryPoint = "?TCP102_RS485Transmit@@YGEPADEEPAUSTRUCT_RS485_TRANS@@@Z")]
        public unsafe static extern byte TCP102_RS485Transmit(string ctr_ip, uint sequence, uint time_out, ref STRUCT_RS485_TRANS trans);
        [DllImport("TCP102_SDK.dll", EntryPoint = "?TCP102_RS232Transmit@@YGEPADEEPAUSTRUCT_RS232_TRANS@@@Z")]
        public unsafe static extern byte TCP102_RS232Transmit(string ctr_ip, uint sequence, uint time_out, ref STRUCT_RS232_TRANS rs232_trans);
        [DllImport("TCP102_SDK.dll", EntryPoint = "?TCP102_CardDispenserControl@@YGEPADEEW4DISPENSER_CMD@@@Z")]
        public unsafe static extern byte TCP102_CardDispenserControl(string ctr_ip, uint sequence, uint time_out, byte dispenser_cmd);
        [DllImport("TCP102_SDK.dll", EntryPoint = "?TCP102_CardCollectorControl@@YGEPADEEW4COLLECTOR_CMD@@@Z")]
        public unsafe static extern byte TCP102_CardCollectorControl(string ctr_ip, uint sequence, uint time_out, byte collerctor_cmd);
        [DllImport("TCP102_SDK.dll", EntryPoint = "?TCP102_WriteIO@@YGEPADEEE@Z")]
        public unsafe static extern byte TCP102_WriteIO(string ctr_ip, uint sequence, uint time_out, byte io_data);
        [DllImport("TCP102_SDK.dll", EntryPoint = "?TCP102_ReadIO@@YGEPADEEPAUSTRUCT_IO_STATE@@@Z")]
        public unsafe static extern byte TCP102_ReadIO(string ctr_ip, uint sequence, uint time_out, ref STRUCT_IO_STATE io_state);
        [DllImport("TCP102_SDK.dll", EntryPoint = "?TCP102_LightControl@@YGEPADEEW4LIGHT_CMD@@@Z")]
        public unsafe static extern byte TCP102_LightControl(string ctr_ip, uint sequence, uint time_out, byte light_cmd);
        [DllImport("TCP102_SDK.dll", EntryPoint = "?TCP102_BarrierControl@@YGEPADEEW4BARRIER_CMD@@@Z")]
        public unsafe static extern byte TCP102_BarrierControl(string ctr_ip, uint sequence, uint time_out, byte barrier_cmd);
        [DllImport("TCP102_SDK.dll", EntryPoint = "?TCP102_RelayControl@@YGEPADEEW4RELAY_CMD@@@Z")]
        public unsafe static extern byte TCP102_RelayControl(string ctr_ip, uint sequence, uint time_out, byte relay_cmd);
        [DllImport("TCP102_SDK.dll", EntryPoint = "?TCP102_LoadCardList@@YGEPADEEPAUSTRUCT_CARD_LIST@@@Z")]
        public unsafe static extern byte TCP102_LoadCardList(string ctr_ip, uint sequence, uint time_out, ref STRUCT_CARD_LIST card_list);
        [DllImport("TCP102_SDK.dll", EntryPoint = "?TCP102_ClearCardList@@YGEPADEE@Z")]
        public unsafe static extern byte TCP102_ClearCardList(string ctr_ip, uint sequence, uint time_out);
        [DllImport("TCP102_SDK.dll", EntryPoint = "?TCP102_ClearRecordList@@YGEPADEE@Z")]
        public unsafe static extern byte TCP102_ClearRecordList(string ctr_ip, uint sequence, uint time_out);
        [DllImport("TCP102_SDK.dll", EntryPoint = "?TCP102_ReadCardList@@YGEPADEEGPAUSTRUCT_CARD_LIST@@@Z")]
        public unsafe static extern byte TCP102_ReadCardList(string ctr_ip, uint sequence, uint time_out, ushort list_seq, ref STRUCT_CARD_LIST card_list);
        [DllImport("TCP102_SDK.dll", EntryPoint = "?TCP102_DllExit@@YGEXZ")]
        public unsafe static extern byte TCP102_DllExit();

        static int num;
        static MemberManagement.Form1 frm = new MemberManagement.Form1();
        static string ctr_ip1, ctr_ip2;
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                CALLBACK_FUN fun = new CALLBACK_FUN();
                fun.p_ReadedCardNumberCallBack = fReadedCardNumber;
                fun.p_CtrUploadIOStateCallBack = fCtrUploadIOState;
                fun.p_ReadedICCardCallBack = fReadICCard;
                byte a = TCP102_InitDll(1);
                byte b = TCP102_InitCallBack(1, ref fun);
                if (a != 0)
                {
                    Console.WriteLine("Init DLL Error code : " + a.ToString() + "\nPress any key to exit...");
                    Console.ReadKey();
                    return;
                }
                if (b != 0)
                {
                    Console.WriteLine("Init CALLBACK Error code : " + b.ToString() + "\nPress any key to exit...");
                    Console.ReadKey();
                    return;
                }
                frm.loadConfig();
                ctr_ip1 = frm.ctrlIP1;
                ctr_ip2 = frm.ctrlIP2;

                var handle = GetConsoleWindow();
                //Hide
                ShowWindow(handle, SW_HIDE);
                Thread.Sleep(400);
                frm.ShowDialog();
                // Show
                ShowWindow(handle, SW_SHOW);
                TCP102_DllExit();
            }
            catch (Exception) { }


        }

        static void fReadedCardNumber(uint dwUser, ref CARD_NUMBER_MSG p_card_msg)
        {
            num = (int)(p_card_msg.CardID[3] << 24);
            num |= (int)(p_card_msg.CardID[2] << 16);
            num |= (int)(p_card_msg.CardID[1] << 8);
            num |= (int)(p_card_msg.CardID[0]);

            string ip = p_card_msg.ControllerIP[0] + "."
                + p_card_msg.ControllerIP[1] + "."
                + p_card_msg.ControllerIP[2] + "."
                + p_card_msg.ControllerIP[3];

            Console.WriteLine("From " + ip + " : " + num);

            if (ip.Equals(ctr_ip1))
            { //Member IN
                string str = "ID CARD: " + num.ToString();
                if (frm.checkCard(num.ToString()))
                {
                    if (frm.getCardLevel() > 1)
                    {
                        frm.takePhotoFore1();
                        frm.SaveImage("MI", "server", "IN");
                        Thread.Sleep(100);
                        frm.saveMember(num.ToString());

                        TCP102_RelayControl(ip, 0, 1, RELAY4_ON);
                        
                        STRUCT_PLAY_VOICE voice;
                        voice.VoiceNum = 1;
                        voice.VoiceBuff = new byte[32];
                        voice.VoiceBuff[0] = 9;
                        TCP102_PlayVoice(ctr_ip1, 1, 5, ref voice);
                        
                        Thread.Sleep(800);
                        TCP102_RelayControl(ip, 0, 1, RELAY4_OFF);

                        str += "\n\nสมาชิกรหัส: " + num.ToString() + " เข้าจอด";
                        frm.SaveImage("MI", "backup", "IN");
                        frm.saveMemberBackup(num.ToString());
                    }
                    else
                    {
                        str += "\n\nไม่ใช่การ์ดสมาชิก";
                        STRUCT_PLAY_VOICE voice;
                        voice.VoiceNum = 1;
                        voice.VoiceBuff = new byte[32];
                        voice.VoiceBuff[0] = 7;
                        TCP102_PlayVoice(ctr_ip1, 1, 5, ref voice);
                    }
                }
                else
                {
                    if (frm.getRecordNo2() > 0)
                    {
                        str += "\n\nการ์ดยังไม่ได้ทาบออก";
                        STRUCT_PLAY_VOICE voice;
                        voice.VoiceNum = 1;
                        voice.VoiceBuff = new byte[32];
                        voice.VoiceBuff[0] = 4;
                        TCP102_PlayVoice(ctr_ip1, 1, 5, ref voice);
                    }
                    else {
                        str += "\n\nไม่มีบัตรในระบบ";
                        STRUCT_PLAY_VOICE voice;
                        voice.VoiceNum = 1;
                        voice.VoiceBuff = new byte[32];
                        voice.VoiceBuff[0] = 1;
                        TCP102_PlayVoice(ctr_ip1, 1, 5, ref voice);
                    }
                }

                frm.setTextDetail1(str);
            }

            if (ip.Equals(ctr_ip2))
            { // Member OUT 
                 string str = "ID CARD: " + num.ToString();
                 if (!frm.checkCard(num.ToString()))
                 {
                     if (frm.getCardLevel() > 1)
                     {
                         frm.takePhotoFore2();
                         frm.SaveImage("MO", "server", "OUT");
                         if (frm.MemberOut(num.ToString()))
                         {
                             TCP102_RelayControl(ip, 0, 1, RELAY4_ON);
                             Thread.Sleep(800);
                             TCP102_RelayControl(ip, 0, 1, RELAY4_OFF);
                             str += "\n\nสมาชิกรหัส: " + num.ToString() + " ออก";
                             STRUCT_PLAY_VOICE voice;
                             voice.VoiceNum = 1;
                             voice.VoiceBuff = new byte[32];
                             voice.VoiceBuff[0] = 10;
                             TCP102_PlayVoice(ctr_ip2, 1, 5, ref voice);
                         }
                         else
                         {
                             str += "\n\nบันทึกไม่สำเร็จ";
                         }

                         frm.takePhotoFore2();
                         frm.SaveImage("MO", "backup", "OUT");
                         frm.MemberOutB();


                     }
                     else
                     {
                         str += "\n\nไม่ใช่การ์ดสมาชิก";
                         STRUCT_PLAY_VOICE voice;
                         voice.VoiceNum = 1;
                         voice.VoiceBuff = new byte[32];
                         voice.VoiceBuff[0] = 7;
                         TCP102_PlayVoice(ctr_ip2, 1, 5, ref voice);
                     }
                 }
                 else
                 {
                     if (frm.getRecordNo2() < 1)
                     {
                         str += "\n\nการ์ดยังไม่ได้ทาบเข้า";
                         STRUCT_PLAY_VOICE voice;
                         voice.VoiceNum = 1;
                         voice.VoiceBuff = new byte[32];
                         voice.VoiceBuff[0] = 3;
                         TCP102_PlayVoice(ctr_ip2, 1, 5, ref voice);
                     }
                 }
                 frm.setTextDetail2(str);
            }


        }

        static void fCtrUploadIOState(uint dwUser, ref IO_STATE_MSG p_io_state_msg)
        {
        }

        static void fReadICCard(uint dwUser, ref IC_CARD_MSG p_ic_card_msg)
        {

        }

        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }




    }
}
