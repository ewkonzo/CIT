﻿using NetSDK;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Dispenser
{
    public class DahuaSDK
    {
        private int m_nLoginID;     //Device Login ID
        private bool m_bInit;       //Whether the Library is Initialized successfully
        private int m_nChannelNum;  //Device Channel Number
        private bool m_bJSON;       //
        private bool m_bEnable;
        private bool m_bEnable2;

        private int m_nLoginID2;
        private int m_nLoginID3;
        private int m_nLoginID4; //Device Login ID    
        private bool m_bJSON2;
        private bool m_bJSON3;
        private bool m_bJSON4; //

        private int m_snapmode;

        private string ipCamGlobal1, ipCamGlobal2, ipCamGlobal3, ipCamGlobal4;
        private string camPath1, camPath2, camPath3, camPath4;

        private Dictionary<string, int> m_dicPix;//Resolution
        private Dictionary<string, int> m_dicQuality;//Image Quality
        private Dictionary<string, int> m_dicSnapSpace;//Snap Space
        private Dictionary<string, int> m_dicSnapMode;//Snap Mode

        private NET_SNAP_ATTR_EN m_stuSnapAttr;
        private NET_SNAP_ATTR_EN_EX m_stuSnapAttrEx;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        private NETDEV_SNAP_CFG[] m_stuSnapCfg;

        // Entrust
        private fDisConnect m_disConnect;
        private fSnapRev m_SnapRecv;

        public struct NET_SNAP_ATTR_EN_EX
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public Int32[] m_bQueried;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public CFG_SNAPCAPINFO_INFO[] m_stuSnapAttrEx;
        };

        //private string ReadParam(string strParam)
        //{

        //    string str = "";
        //    string sql = "SELECT * FROM param WHERE name='" + strParam + "'";
        //    DataTable dt;
        //    try
        //    {
        //        dt = db.LoadData(sql);
        //        str = dt.Rows[0].ItemArray[1].ToString();
        //    }
        //    catch (Exception) { }
        //    return str;
        //}



        public bool InitCamera(string ipCam1, string ipCam2, string ipCam3, string ipCam4)
        {
            //try
            //{
            //    if (!Convert.ToBoolean(ReadParam("dahua")))
            //        return false;
            //}
            //catch (Exception)
            //{
            //    return false;
            //}
            m_bEnable = false;
            m_bEnable2 = false;

            m_nLoginID = 0;
            m_nLoginID2 = 0;
            m_nLoginID3 = 0;
            m_nLoginID4 = 0;
            m_bInit = false;

            ipCamGlobal1 = ipCam1;
            ipCamGlobal2 = ipCam2;
            ipCamGlobal3 = ipCam3;
            ipCamGlobal4 = ipCam4;

            m_stuSnapCfg = new NETDEV_SNAP_CFG[32];
            for (int i = 0; i < 32; ++i)
            {
                m_stuSnapCfg[i].struSnapEnc = new NET_VIDEOENC_OPT[32];
            }

            m_stuSnapAttr = new NET_SNAP_ATTR_EN();
            m_stuSnapAttr.stuSnap = new NET_QUERY_SNAP_INFO[16];

            m_stuSnapAttrEx = new NET_SNAP_ATTR_EN_EX();
            m_stuSnapAttrEx.m_bQueried = new Int32[32];
            m_stuSnapAttrEx.m_stuSnapAttrEx = new CFG_SNAPCAPINFO_INFO[32];

            //Resolution
            m_dicPix = new Dictionary<string, int>();
            m_dicPix["D1"] = 0;
            m_dicPix["HD1"] = 1;
            m_dicPix["BCIF"] = 2;
            m_dicPix["CIF"] = 3;
            m_dicPix["QCIF"] = 4;
            m_dicPix["VGA"] = 5;
            m_dicPix["QVGA"] = 6;
            m_dicPix["SVCD"] = 7;
            m_dicPix["QQVGA"] = 8;
            m_dicPix["SVGA"] = 9;
            m_dicPix["XVGA"] = 10;
            m_dicPix["WXGA"] = 11;
            m_dicPix["SXGA"] = 12;
            m_dicPix["WSXGA"] = 13;
            m_dicPix["UXGA"] = 14;
            m_dicPix["WUXGA"] = 15;
            m_dicPix["LTF"] = 16;
            m_dicPix["720p"] = 17;
            m_dicPix["1080p"] = 18;
            m_dicPix["1.3M"] = 19;
            m_dicPix["NR"] = 20;

            m_dicQuality = new Dictionary<string, int>();
            m_dicSnapSpace = new Dictionary<string, int>();
            m_dicSnapMode = new Dictionary<string, int>();

            return InitSDK();
        }

        private bool InitSDK()
        {
            m_disConnect = new fDisConnect(DisConnectEvent);
            m_SnapRecv = new fSnapRev(SnapRev);
            m_bInit = NETClient.NETInit(m_disConnect, IntPtr.Zero);
            if (!m_bInit)
            {
                //  MessageBox.Show("Initialization Failure ");
                return false;
            }
            else
            {
                NETClient.NETSetSnapRevCallBack(m_SnapRecv, 0);
                return true;
            }

        }

        private void DisConnectEvent(int lLoginID, StringBuilder pchDVRIP, int nDVRPort, IntPtr dwUser)
        {
            //MessageBox.Show("Device Disconnected!");
        }

        private void SnapRev(Int32 lLoginID, IntPtr pBuf, UInt32 RevLen, UInt32 EncodeType, UInt32 CmdSerial, UInt32 dwUser)
        {
            byte[] buf = new byte[RevLen];
            Marshal.Copy(pBuf, buf, 0, (int)RevLen);
            Thread.Sleep(500);
            string strName = camPath1;
            if (lLoginID == m_nLoginID2)
                strName = camPath2;
            if (lLoginID == m_nLoginID3)
                strName = camPath3;
            if (lLoginID == m_nLoginID4)
                strName = camPath4;
            try
            {
                // Create the file.
                using (System.IO.FileStream fs = System.IO.File.Create(strName))
                {
                    //Marshal.read
                    fs.Write(buf, 0, (int)RevLen);
                    fs.Close();
                    //    MessageBox.Show("Picture Obtained is in the Working Directory,Picture Name is "+strName);
                }
            }
            catch (Exception) { }
            Thread.Sleep(2000);
            return;
        }

        public bool LoginCam()
        {
            if (!m_bInit)
            {
                // MessageBox.Show("Library Initialization Failed");
                return false;
            }

            //Obtain Device User Information
            NET_DEVICEINFO deviceInfo = new NET_DEVICEINFO();
            int error = 0;
            m_nLoginID = NETClient.NETLogin(ipCamGlobal1, ushort.Parse("37777"),
                        "admin", "admin", out deviceInfo, out error);

            if (m_nLoginID > 0)
            {
                m_nChannelNum = deviceInfo.byChanNum;
                //query json ability.
                Int32 dwRetLen = 0;
                IntPtr pDevEnable = new IntPtr();
                pDevEnable = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NET_DEV_ENABLE_INFO)));
                bool bRet = NETClient.NETQuerySystemInfo(m_nLoginID, NET_SYS_ABILITY.ABILITY_DEVALL_INFO, pDevEnable,
                                                Marshal.SizeOf(typeof(NET_DEV_ENABLE_INFO)), ref dwRetLen, 1000);
                if (bRet == false)
                {
                    //MessageBox.Show(ConvertString("Query device ability failed."));
                    // MessageBox.Show("Query device ability failed");
                    return false;
                }


                NET_DEV_ENABLE_INFO devEnable = new NET_DEV_ENABLE_INFO();
                devEnable = (NET_DEV_ENABLE_INFO)Marshal.PtrToStructure(pDevEnable, typeof(NET_DEV_ENABLE_INFO));
                m_bJSON = devEnable.IsFucEnable[(Int32)NET_FUN_SUPPORT.EN_JSON_CONFIG] > 0 ? true : false;

                if (m_bJSON == false)
                {
                    int nRetLen = 0;
                    IntPtr pStuSnapAttr = new IntPtr();
                    pStuSnapAttr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NET_SNAP_ATTR_EN)));
                    bool nRet = NETClient.NETQueryDevState(m_nLoginID, (int)NETClient.NET_DEVSTATE_SNAP
                                                       , pStuSnapAttr, Marshal.SizeOf(typeof(NET_SNAP_ATTR_EN)), ref nRetLen, 1000);
                    if (nRet == false || nRetLen != Marshal.SizeOf(typeof(NET_SNAP_ATTR_EN)))
                    {
                        //   MessageBox.Show("Get Snap Capability Set Failed!");
                        return false;
                    }
                    else
                    {
                        m_stuSnapAttr = (NET_SNAP_ATTR_EN)Marshal.PtrToStructure(pStuSnapAttr, typeof(NET_SNAP_ATTR_EN));
                    }
                }
                else//json
                {
                    InitSnapConfigExUI(0);
                }

                IntPtr pSnapCfg = new IntPtr();
                pSnapCfg = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NETDEV_SNAP_CFG)) * 32);
                UInt32 dwRetConfig = 0;
                bRet = NETClient.NETGetDevConfig(m_nLoginID, CONFIG_COMMAND.NET_DEV_SNAP_CFG, -1, pSnapCfg, (UInt32)Marshal.SizeOf(typeof(NETDEV_SNAP_CFG)) * 32, ref dwRetConfig, 1000);
                if (!bRet)
                {
                    //  MessageBox.Show("Get Snap Configuration Failed!");
                    return false;
                }
                else
                {
                    for (int i = 0; i < 32; ++i)
                    {
                        m_stuSnapCfg[i] = (NETDEV_SNAP_CFG)Marshal.PtrToStructure((IntPtr)((UInt32)pSnapCfg + i * Marshal.SizeOf(typeof(NETDEV_SNAP_CFG)))
                                                                   , typeof(NETDEV_SNAP_CFG));
                    }
                }
            }

            NET_DEVICEINFO deviceInfo2 = new NET_DEVICEINFO();
            int error2 = 0;
            m_nLoginID2 = NETClient.NETLogin(ipCamGlobal2, ushort.Parse("37777"),
                        "admin", "admin", out deviceInfo2, out error2);
            if (m_nLoginID2 > 0)
            {
                //query json ability.
                Int32 dwRetLen = 0;
                IntPtr pDevEnable = new IntPtr();
                pDevEnable = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NET_DEV_ENABLE_INFO)));
                bool bRet = NETClient.NETQuerySystemInfo(m_nLoginID2, NET_SYS_ABILITY.ABILITY_DEVALL_INFO, pDevEnable,
                                                Marshal.SizeOf(typeof(NET_DEV_ENABLE_INFO)), ref dwRetLen, 1000);
                if (bRet == false)
                {
                    //MessageBox.Show(ConvertString("Query device ability failed."));
                    //MessageBox.Show("Query device ability failed");
                    return false;
                }


                NET_DEV_ENABLE_INFO devEnable = new NET_DEV_ENABLE_INFO();
                devEnable = (NET_DEV_ENABLE_INFO)Marshal.PtrToStructure(pDevEnable, typeof(NET_DEV_ENABLE_INFO));
                m_bJSON2 = devEnable.IsFucEnable[(Int32)NET_FUN_SUPPORT.EN_JSON_CONFIG] > 0 ? true : false;

                if (m_bJSON2 == false)
                {
                    int nRetLen = 0;
                    IntPtr pStuSnapAttr = new IntPtr();
                    pStuSnapAttr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NET_SNAP_ATTR_EN)));
                    bool nRet = NETClient.NETQueryDevState(m_nLoginID2, (int)NETClient.NET_DEVSTATE_SNAP
                                                       , pStuSnapAttr, Marshal.SizeOf(typeof(NET_SNAP_ATTR_EN)), ref nRetLen, 1000);
                    if (nRet == false || nRetLen != Marshal.SizeOf(typeof(NET_SNAP_ATTR_EN)))
                    {
                        //   MessageBox.Show("Get Snap Capability Set Failed!");
                        return false;
                    }
                    else
                    {
                        m_stuSnapAttr = (NET_SNAP_ATTR_EN)Marshal.PtrToStructure(pStuSnapAttr, typeof(NET_SNAP_ATTR_EN));
                    }
                }
                else//json
                {
                    InitSnapConfigExUI(0);
                }

                IntPtr pSnapCfg = new IntPtr();
                pSnapCfg = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NETDEV_SNAP_CFG)) * 32);
                UInt32 dwRetConfig = 0;
                bRet = NETClient.NETGetDevConfig(m_nLoginID2, CONFIG_COMMAND.NET_DEV_SNAP_CFG, -1, pSnapCfg, (UInt32)Marshal.SizeOf(typeof(NETDEV_SNAP_CFG)) * 32, ref dwRetConfig, 1000);
                if (!bRet)
                {
                    // MessageBox.Show("Get Snap Configuration Failed!");
                    return false;
                }
                else
                {
                    for (int i = 0; i < 32; ++i)
                    {
                        m_stuSnapCfg[i] = (NETDEV_SNAP_CFG)Marshal.PtrToStructure((IntPtr)((UInt32)pSnapCfg + i * Marshal.SizeOf(typeof(NETDEV_SNAP_CFG)))
                                                                   , typeof(NETDEV_SNAP_CFG));
                    }
                }//else
            }//if login


            NET_DEVICEINFO deviceInfo3 = new NET_DEVICEINFO();
            int error3 = 0;
            m_nLoginID3 = NETClient.NETLogin(ipCamGlobal3, ushort.Parse("37777"),
                        "admin", "admin", out deviceInfo3, out error3);
            if (m_nLoginID3 > 0)
            {
                //query json ability.
                Int32 dwRetLen = 0;
                IntPtr pDevEnable = new IntPtr();
                pDevEnable = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NET_DEV_ENABLE_INFO)));
                bool bRet = NETClient.NETQuerySystemInfo(m_nLoginID3, NET_SYS_ABILITY.ABILITY_DEVALL_INFO, pDevEnable,
                                                Marshal.SizeOf(typeof(NET_DEV_ENABLE_INFO)), ref dwRetLen, 1000);
                if (bRet == false)
                {
                    //MessageBox.Show(ConvertString("Query device ability failed."));
                    //MessageBox.Show("Query device ability failed");
                    return false;
                }


                NET_DEV_ENABLE_INFO devEnable = new NET_DEV_ENABLE_INFO();
                devEnable = (NET_DEV_ENABLE_INFO)Marshal.PtrToStructure(pDevEnable, typeof(NET_DEV_ENABLE_INFO));
                m_bJSON3 = devEnable.IsFucEnable[(Int32)NET_FUN_SUPPORT.EN_JSON_CONFIG] > 0 ? true : false;

                if (m_bJSON3 == false)
                {
                    int nRetLen = 0;
                    IntPtr pStuSnapAttr = new IntPtr();
                    pStuSnapAttr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NET_SNAP_ATTR_EN)));
                    bool nRet = NETClient.NETQueryDevState(m_nLoginID3, (int)NETClient.NET_DEVSTATE_SNAP
                                                       , pStuSnapAttr, Marshal.SizeOf(typeof(NET_SNAP_ATTR_EN)), ref nRetLen, 1000);
                    if (nRet == false || nRetLen != Marshal.SizeOf(typeof(NET_SNAP_ATTR_EN)))
                    {
                        //   MessageBox.Show("Get Snap Capability Set Failed!");
                        return false;
                    }
                    else
                    {
                        m_stuSnapAttr = (NET_SNAP_ATTR_EN)Marshal.PtrToStructure(pStuSnapAttr, typeof(NET_SNAP_ATTR_EN));
                    }
                }
                else//json
                {
                    InitSnapConfigExUI(0);
                }

                IntPtr pSnapCfg = new IntPtr();
                pSnapCfg = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NETDEV_SNAP_CFG)) * 32);
                UInt32 dwRetConfig = 0;
                bRet = NETClient.NETGetDevConfig(m_nLoginID3, CONFIG_COMMAND.NET_DEV_SNAP_CFG, -1, pSnapCfg, (UInt32)Marshal.SizeOf(typeof(NETDEV_SNAP_CFG)) * 32, ref dwRetConfig, 1000);
                if (!bRet)
                {
                    // MessageBox.Show("Get Snap Configuration Failed!");
                    return false;
                }
                else
                {
                    for (int i = 0; i < 32; ++i)
                    {
                        m_stuSnapCfg[i] = (NETDEV_SNAP_CFG)Marshal.PtrToStructure((IntPtr)((UInt32)pSnapCfg + i * Marshal.SizeOf(typeof(NETDEV_SNAP_CFG)))
                                                                   , typeof(NETDEV_SNAP_CFG));
                    }
                }//else
            }//if login

            NET_DEVICEINFO deviceInfo4 = new NET_DEVICEINFO();
            int error4 = 0;
            m_nLoginID4 = NETClient.NETLogin(ipCamGlobal4, ushort.Parse("37777"),
                        "admin", "admin", out deviceInfo4, out error4);
            if (m_nLoginID4 > 0)
            {
                //query json ability.
                Int32 dwRetLen = 0;
                IntPtr pDevEnable = new IntPtr();
                pDevEnable = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NET_DEV_ENABLE_INFO)));
                bool bRet = NETClient.NETQuerySystemInfo(m_nLoginID4, NET_SYS_ABILITY.ABILITY_DEVALL_INFO, pDevEnable,
                                                Marshal.SizeOf(typeof(NET_DEV_ENABLE_INFO)), ref dwRetLen, 1000);
                if (bRet == false)
                {
                    //MessageBox.Show(ConvertString("Query device ability failed."));
                    //MessageBox.Show("Query device ability failed");
                    return false;
                }


                NET_DEV_ENABLE_INFO devEnable = new NET_DEV_ENABLE_INFO();
                devEnable = (NET_DEV_ENABLE_INFO)Marshal.PtrToStructure(pDevEnable, typeof(NET_DEV_ENABLE_INFO));
                m_bJSON4 = devEnable.IsFucEnable[(Int32)NET_FUN_SUPPORT.EN_JSON_CONFIG] > 0 ? true : false;

                if (m_bJSON4 == false)
                {
                    int nRetLen = 0;
                    IntPtr pStuSnapAttr = new IntPtr();
                    pStuSnapAttr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NET_SNAP_ATTR_EN)));
                    bool nRet = NETClient.NETQueryDevState(m_nLoginID4, (int)NETClient.NET_DEVSTATE_SNAP
                                                       , pStuSnapAttr, Marshal.SizeOf(typeof(NET_SNAP_ATTR_EN)), ref nRetLen, 1000);
                    if (nRet == false || nRetLen != Marshal.SizeOf(typeof(NET_SNAP_ATTR_EN)))
                    {
                        //   MessageBox.Show("Get Snap Capability Set Failed!");
                        return false;
                    }
                    else
                    {
                        m_stuSnapAttr = (NET_SNAP_ATTR_EN)Marshal.PtrToStructure(pStuSnapAttr, typeof(NET_SNAP_ATTR_EN));
                    }
                }
                else//json
                {
                    InitSnapConfigExUI(0);
                }

                IntPtr pSnapCfg = new IntPtr();
                pSnapCfg = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NETDEV_SNAP_CFG)) * 32);
                UInt32 dwRetConfig = 0;
                bRet = NETClient.NETGetDevConfig(m_nLoginID4, CONFIG_COMMAND.NET_DEV_SNAP_CFG, -1, pSnapCfg, (UInt32)Marshal.SizeOf(typeof(NETDEV_SNAP_CFG)) * 32, ref dwRetConfig, 1000);
                if (!bRet)
                {
                    // MessageBox.Show("Get Snap Configuration Failed!");
                    return false;
                }
                else
                {
                    for (int i = 0; i < 32; ++i)
                    {
                        m_stuSnapCfg[i] = (NETDEV_SNAP_CFG)Marshal.PtrToStructure((IntPtr)((UInt32)pSnapCfg + i * Marshal.SizeOf(typeof(NETDEV_SNAP_CFG)))
                                                                   , typeof(NETDEV_SNAP_CFG));
                    }
                }//else
            }//if login

            return true;
        }//Method

        private void InitSnapConfigExUI(int nChn)
        {
            if (m_stuSnapAttrEx.m_bQueried[nChn] == 0)
            {
                object result = new object();
                bool bRet = bRet = NETClient.NETGetNewDevConfig(m_nLoginID, 0,
                    NETClient.CFG_CMD_SNAPCAPINFO, ref result, typeof(CFG_SNAPCAPINFO_INFO), 0, 3000);
                if (bRet == false)
                {
                    //  MessageBox.Show("Get Device Capability Set Failed");
                    return;
                }

                m_stuSnapAttrEx.m_stuSnapAttrEx[nChn] = (CFG_SNAPCAPINFO_INFO)result;
                m_stuSnapAttrEx.m_bQueried[nChn] = 1;
            }

            if (m_stuSnapAttrEx.m_bQueried[nChn] == 0)
            {
                return;
            }

            CFG_SNAPCAPINFO_INFO stuSnapInfo = m_stuSnapAttrEx.m_stuSnapAttrEx[nChn];
            int i = 0;

            // this.comboBoxSnapMode.Items.Clear();
            if ((stuSnapInfo.dwMode & 0x00000001) > 0)
            {
                //  this.comboBoxSnapMode.Items.Add("Timing Trigger Snap");
                m_dicPix["Timing Trigger Snap"] = 0;
            }
            if ((stuSnapInfo.dwMode & 0x00000002) > 0)
            {
                //   this.comboBoxSnapMode.Items.Add("Manual Trigger Snap");
                m_dicPix["Manual Trigger Snap"] = 1;
            }

            //
            string csFrame = "";
            // this.comboBoxSnapSpace.Items.Clear();
            for (i = 0; i < stuSnapInfo.dwFramesPerSecNum; i++)
            {
                if (stuSnapInfo.nFramesPerSecList[i] > 0)
                {
                    csFrame = string.Format("{0:n0} Second1Frame", stuSnapInfo.nFramesPerSecList[i]);
                }
                else if (stuSnapInfo.nFramesPerSecList[i] < 0)
                {
                    csFrame = string.Format("{0:n0} Second1Frame", Math.Abs(stuSnapInfo.nFramesPerSecList[i]));
                }
                m_dicSnapSpace[csFrame] = stuSnapInfo.nFramesPerSecList[i];
            }
            // 
            // 		IMAGE_SIZE_D1,								// 704*576(PAL)  704*480(NTSC)
            // 		IMAGE_SIZE_HD1,								// 352*576(PAL)  352*480(NTSC)
            // 		IMAGE_SIZE_BCIF,							// 704*288(PAL)  704*240(NTSC)
            // 		IMAGE_SIZE_CIF,								// 352*288(PAL)  352*240(NTSC)
            // 		IMAGE_SIZE_QCIF,							// 176*144(PAL)  176*120(NTSC)
            // 		IMAGE_SIZE_VGA,								// 640*480
            // 		IMAGE_SIZE_QVGA,							// 320*240
            // 		IMAGE_SIZE_SVCD,							// 480*480
            // 		IMAGE_SIZE_QQVGA,							// 160*128
            // 		IMAGE_SIZE_SVGA,							// 800*592
            // 		IMAGE_SIZE_XVGA,							// 1024*768
            // 		IMAGE_SIZE_WXGA,							// 1280*800
            // 		IMAGE_SIZE_SXGA,							// 1280*1024  
            // 		IMAGE_SIZE_WSXGA,							// 1600*1024  
            // 		IMAGE_SIZE_UXGA,							// 1600*1200
            // 		IMAGE_SIZE_WUXGA,							// 1920*1200
            // 		IMAGE_SIZE_LTF,								// 240*192
            // 		IMAGE_SIZE_720,								// 1280*720
            // 		IMAGE_SIZE_1080,							// 1920*1080
            // 		IMAGE_SIZE_1_3M,							// 1280*960
            // 		IMAGE_SIZE_NR  

            //
            UInt32 dwMask = 0x0001;
            // this.comboBoxRe.Items.Clear();
            for (i = 0; i < 32; i++)
            {
                if ((m_stuSnapAttr.stuSnap[nChn].dwVideoStandardMask & dwMask) > 0)
                {
                    foreach (KeyValuePair<string, int> temp in m_dicPix)
                    {
                        if (temp.Value == i)
                        {
                            //  this.comboBoxRe.Items.Add(temp.Key);
                        }
                    }
                }
                dwMask <<= 1;
            }

            //
            //IMAGE_QUALITY_Q10 = 1,							// Image Quality 10%
            //IMAGE_QUALITY_Q30,								// Image Quality 30%
            //IMAGE_QUALITY_Q50,								// Image Quality 50%
            //IMAGE_QUALITY_Q60,								// Image Quality 60%
            //IMAGE_QUALITY_Q80,								// Image Quality 80%
            //IMAGE_QUALITY_Q100,								// Image Quality 100%
            int[] nMapQuality = { 0, 10, 30, 50, 60, 80, 100 };
            string csQuality = "";
            //   this.comboBoxQuality.Items.Clear();
            for (i = 0; i < stuSnapInfo.dwQualityMun; i++)
            {
                if (stuSnapInfo.emQualityList[i] > 0 && (int)(stuSnapInfo.emQualityList[i]) < 7)
                {
                    csQuality = string.Format("{0:n0}%", nMapQuality[(int)(stuSnapInfo.emQualityList[i])]);
                }

                //     this.comboBoxQuality.Items.Add(csQuality);
                m_dicQuality[csQuality] = m_stuSnapAttr.stuSnap[nChn].PictureQuality[i];
            }

            if (m_stuSnapAttrEx.m_bQueried[nChn] == 0)
            {
                object result = new object();
                bool bRet = bRet = NETClient.NETGetNewDevConfig(m_nLoginID2, 0,
                    NETClient.CFG_CMD_SNAPCAPINFO, ref result, typeof(CFG_SNAPCAPINFO_INFO), 0, 3000);
                if (bRet == false)
                {
                    //  MessageBox.Show("Get Device Capability Set Failed");
                    return;
                }

                m_stuSnapAttrEx.m_stuSnapAttrEx[nChn] = (CFG_SNAPCAPINFO_INFO)result;
                m_stuSnapAttrEx.m_bQueried[nChn] = 1;
            }

            if (m_stuSnapAttrEx.m_bQueried[nChn] == 0)
            {
                return;
            }

            stuSnapInfo = m_stuSnapAttrEx.m_stuSnapAttrEx[nChn];
            i = 0;

            //   this.comboBoxSnapMode.Items.Clear();
            if ((stuSnapInfo.dwMode & 0x00000001) > 0)
            {
                //       this.comboBoxSnapMode.Items.Add("Timing Trigger Snap");
                m_dicPix["Timing Trigger Snap"] = 0;
            }
            if ((stuSnapInfo.dwMode & 0x00000002) > 0)
            {
                //       this.comboBoxSnapMode.Items.Add("Manual Trigger Snap");
                m_dicPix["Manual Trigger Snap"] = 1;
            }

            //
            csFrame = "";
            //   this.comboBoxSnapSpace.Items.Clear();
            for (i = 0; i < stuSnapInfo.dwFramesPerSecNum; i++)
            {
                if (stuSnapInfo.nFramesPerSecList[i] > 0)
                {
                    csFrame = string.Format("{0:n0} Second1Frame", stuSnapInfo.nFramesPerSecList[i]);
                }
                else if (stuSnapInfo.nFramesPerSecList[i] < 0)
                {
                    csFrame = string.Format("{0:n0} Second1Frame", Math.Abs(stuSnapInfo.nFramesPerSecList[i]));
                }
                m_dicSnapSpace[csFrame] = stuSnapInfo.nFramesPerSecList[i];
            }
            // 
            // 		IMAGE_SIZE_D1,								// 704*576(PAL)  704*480(NTSC)
            // 		IMAGE_SIZE_HD1,								// 352*576(PAL)  352*480(NTSC)
            // 		IMAGE_SIZE_BCIF,							// 704*288(PAL)  704*240(NTSC)
            // 		IMAGE_SIZE_CIF,								// 352*288(PAL)  352*240(NTSC)
            // 		IMAGE_SIZE_QCIF,							// 176*144(PAL)  176*120(NTSC)
            // 		IMAGE_SIZE_VGA,								// 640*480
            // 		IMAGE_SIZE_QVGA,							// 320*240
            // 		IMAGE_SIZE_SVCD,							// 480*480
            // 		IMAGE_SIZE_QQVGA,							// 160*128
            // 		IMAGE_SIZE_SVGA,							// 800*592
            // 		IMAGE_SIZE_XVGA,							// 1024*768
            // 		IMAGE_SIZE_WXGA,							// 1280*800
            // 		IMAGE_SIZE_SXGA,							// 1280*1024  
            // 		IMAGE_SIZE_WSXGA,							// 1600*1024  
            // 		IMAGE_SIZE_UXGA,							// 1600*1200
            // 		IMAGE_SIZE_WUXGA,							// 1920*1200
            // 		IMAGE_SIZE_LTF,								// 240*192
            // 		IMAGE_SIZE_720,								// 1280*720
            // 		IMAGE_SIZE_1080,							// 1920*1080
            // 		IMAGE_SIZE_1_3M,							// 1280*960
            // 		IMAGE_SIZE_NR  

            //
            dwMask = 0x0001;
            //   this.comboBoxRe.Items.Clear();
            for (i = 0; i < 32; i++)
            {
                if ((m_stuSnapAttr.stuSnap[nChn].dwVideoStandardMask & dwMask) > 0)
                {
                    foreach (KeyValuePair<string, int> temp in m_dicPix)
                    {
                        if (temp.Value == i)
                        {
                            //                   this.comboBoxRe.Items.Add(temp.Key);
                        }
                    }
                }
                dwMask <<= 1;
            }

            //
            //IMAGE_QUALITY_Q10 = 1,							// Image Quality 10%
            //IMAGE_QUALITY_Q30,								// Image Quality 30%
            //IMAGE_QUALITY_Q50,								// Image Quality 50%
            //IMAGE_QUALITY_Q60,								// Image Quality 60%
            //IMAGE_QUALITY_Q80,								// Image Quality 80%
            //IMAGE_QUALITY_Q100,								// Image Quality 100%
            nMapQuality = new int[] { 0, 10, 30, 50, 60, 80, 100 };
            csQuality = "";
            //       this.comboBoxQuality.Items.Clear();
            for (i = 0; i < stuSnapInfo.dwQualityMun; i++)
            {
                if (stuSnapInfo.emQualityList[i] > 0 && (int)(stuSnapInfo.emQualityList[i]) < 7)
                {
                    csQuality = string.Format("{0:n0}%", nMapQuality[(int)(stuSnapInfo.emQualityList[i])]);
                }

                //         this.comboBoxQuality.Items.Add(csQuality);
                m_dicQuality[csQuality] = m_stuSnapAttr.stuSnap[nChn].PictureQuality[i];
            }
        }//Method

        public void capture(string path1, string path2, string path3, string path4)
        {
            camPath1 = path1;
            camPath2 = path2;
            camPath3 = path3;
            camPath4 = path4;

            if (0 != m_nLoginID)
            {
                if (m_snapmode == -1)
                {
                    //MessageBox(ConvertString("please select snap mode!"), ConvertString("prompt"));
                    return;
                }

                //Fill in request structure 
                SNAP_PARAMS snapparams = new SNAP_PARAMS();
                snapparams.Channel = (UInt32)0;
                snapparams.mode = 0;//m_snapmode>0?0:1;
                snapparams.CmdSerial = UInt32.Parse("1");

                if (snapparams.mode == 1)
                {
                    //Time interval for scheduled snapshot. Use snapshot setup to configure.
                    snapparams.InterSnap = (UInt32)0;
                }

                if (path1 != "")
                {
                    bool bRet = NETClient.NETSnapPicture(m_nLoginID, snapparams);
                    if (!bRet)
                    {
                        //  MessageBox.Show("Snap Start-up failed");
                    }
                    else
                    {
                        //   MessageBox.Show("Snap Start-up successfully");
                    }
                }

                if (path2 != "")
                {
                    bool bRet2 = NETClient.NETSnapPicture(m_nLoginID2, snapparams);
                    if (!bRet2)
                    {
                        //   MessageBox.Show("Snap Start-up failed");
                    }
                    else
                    {
                        //   MessageBox.Show("Snap Start-up successfully");
                    }
                }

                if (path3 != "")
                {
                    bool bRet3 = NETClient.NETSnapPicture(m_nLoginID3, snapparams);
                    if (!bRet3)
                    {
                        //   MessageBox.Show("Snap Start-up failed");
                    }
                    else
                    {
                        //   MessageBox.Show("Snap Start-up successfully");
                    }
                }

                if (path4 != "")
                {
                    bool bRet4 = NETClient.NETSnapPicture(m_nLoginID4, snapparams);
                    if (!bRet4)
                    {
                        //   MessageBox.Show("Snap Start-up failed");
                    }
                    else
                    {
                        //   MessageBox.Show("Snap Start-up successfully");
                    }
                }
            }
        }
    }
}
