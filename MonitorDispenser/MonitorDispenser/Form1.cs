using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace MonitorDispenser
{
    public partial class Form1 : Form
    {

        // Incoming data from the client.
        public static string data = null;
        Image[] active, deactive;
        bool msgLow = false;
        bool msgEmpty = false;
        private AsyncCallback pfnWorkerCallBack;
        private Socket m_mainSocket;
        private Socket[] m_workerSocket = new Socket[10];
        private int m_clientCount = 0;
        public string strRXServer = "";
        //cliant
        public event EventHandler OnDataReceivedFinish;

        private delegate void SetTextCallback(string text);

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MaximizeBox = false;
            active = new Image[6];
            deactive = new Image[6];
            for (int i = 0; i < 6; i++)
            {
                active[i] = Image.FromFile("Images/triangle-icon.png");
                deactive[i] = Image.FromFile("Images/dark-icon.png");
            }

            if (OpenServer(8100))
            {
                lbLocal.Text = "RECEIVE STATUS IP : " + GetLocalIPAddress();
                OnDataReceivedFinish += Form1_OnDataReceivedFinish;
            }

        }

        public string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }

        private void Form1_OnDataReceivedFinish(object sender, EventArgs e)
        {
            string dataTemp = strRXServer;
            strRXServer = "";
            if (dataTemp.Length == 8)
            {
                if (dataTemp[2] == '1') picLoop.Image = active[0];
                else picLoop.Image = deactive[0];

                if (dataTemp[3] == '1')
                {
                    picRed.Image = active[1];
                    this.Invoke(new Action(() =>
                    {
                        MessageBox.Show(new Form() { TopMost = true }, "มีการกดปุ่มขอความช่วยเหลือ!!!!!!",
                            "คำเตือน!",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1);
                    }));
                    picLow.Image = active[4];
                    mynotifyicon.BalloonTipText = "Help Button...";
                    mynotifyicon.BalloonTipTitle = "Monitor Dispenser";
                    mynotifyicon.Visible = true;
                    mynotifyicon.ShowBalloonTip(500);

                }
                else
                    picRed.Image = deactive[1];

                if (dataTemp[4] == '1') picGreen.Image = active[2];
                else picGreen.Image = deactive[2];

                if (dataTemp[5] == '1') picReadOk.Image = active[3];
                else picReadOk.Image = deactive[3];

                if (dataTemp[6] == '1')
                {
                    if (!msgLow)
                        this.Invoke(new Action(() =>
                        {
                            MessageBox.Show(new Form() { TopMost = true }, "บัตรในเครื่องจ่ายเหลือน้อยกรุณาเติมบัตร",
                                "คำเตือน!",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning,
                            MessageBoxDefaultButton.Button1);
                        }));
                    picLow.Image = active[4];
                    mynotifyicon.BalloonTipText = "Card Low...";
                    mynotifyicon.BalloonTipTitle = "Monitor Dispenser";
                    mynotifyicon.Visible = true;
                    mynotifyicon.ShowBalloonTip(500);
                    msgLow = true;
                }
                else
                {
                    mynotifyicon.Visible = false;
                    picLow.Image = deactive[4];
                    msgLow = false;
                }

                if (dataTemp[7] == '1')
                {
                    if (!msgEmpty)
                        this.Invoke(new Action(() =>
                        {
                            MessageBox.Show(new Form() { TopMost = true }, "บัตรในเครื่องจ่ายหมด!!\n กรุณาเติมบัตร",
                                "คำเตือน!",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1);
                        }));
                    picEmpty.Image = active[5];
                    picLow.Image = active[4];
                    mynotifyicon.BalloonTipText = "Card Empty...";
                    mynotifyicon.BalloonTipTitle = "Monitor Dispenser";
                    mynotifyicon.Visible = true;
                    mynotifyicon.ShowBalloonTip(500);
                }
                else
                {
                    picEmpty.Image = deactive[5];
                    mynotifyicon.Visible = true;
                }
            }//if check length
        }

        public bool OpenServer(int port)
        {
            bool result = true;
            try
            {
                // Create the listening socket...
                m_mainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint ipLocal = new IPEndPoint(IPAddress.Any, port);
                // Bind to local IP Address...
                m_mainSocket.Bind(ipLocal);
                // Start listening...
                m_mainSocket.Listen(5);
                // Create the call back for any client connections...
                m_mainSocket.BeginAccept(new AsyncCallback(OnClientConnect), null);
            }
            catch (SocketException se)
            {
                result = false;
                Console.WriteLine(se.ToString());
            }
            return result;
        }

        private void OnClientConnect(IAsyncResult asyn)
        {
            try
            {
                m_workerSocket[m_clientCount] = m_mainSocket.EndAccept(asyn);
                WaitForDataS(m_workerSocket[m_clientCount]);
                // Now increment the client count
                //  ++m_clientCount;
                m_mainSocket.BeginAccept(new AsyncCallback(OnClientConnect), null);
            }
            catch (ObjectDisposedException)
            {
                System.Diagnostics.Debugger.Log(0, "1", "\n OnClientConnection: Socket has been closed\n");
            }
            //catch (SocketException se)
            //{
            //    //MessageBox.Show(se.Message);
            //}
        }

        private void WaitForDataS(Socket soc)
        {
            try
            {
                if (pfnWorkerCallBack == null)
                {
                    pfnWorkerCallBack = new AsyncCallback(OnDataReceivedS);
                }
                SocketPacket theSocPkt = new SocketPacket();
                theSocPkt.m_currentSocket = soc;
                // Start receiving any data written by the connected client
                // asynchronously
                soc.BeginReceive(theSocPkt.dataBuffer, 0,
                                   theSocPkt.dataBuffer.Length,
                                   SocketFlags.None,
                                   pfnWorkerCallBack,
                                   theSocPkt);
            }
            catch (SocketException se)
            {
                //MessageBox.Show(se.Message);
            }

        }

        public void Send2Client(string strSend)
        {
            try
            {
                Object objData = strSend + "\r";
                byte[] byData = System.Text.Encoding.ASCII.GetBytes(objData.ToString());
                for (int i = 0; i < m_clientCount; i++)
                {
                    if (m_workerSocket[i] != null)
                    {
                        if (m_workerSocket[i].Connected)
                        {
                            m_workerSocket[i].Send(byData);
                        }
                    }
                }

            }
            catch (SocketException se)
            {
                //MessageBox.Show(se.Message);
            }
        }
        private void OnDataReceivedS(IAsyncResult asyn)
        {
            try
            {
                SocketPacket socketData = (SocketPacket)asyn.AsyncState;

                int iRx = 0;
                // Complete the BeginReceive() asynchronous call by EndReceive() method
                // which will return the number of characters written to the stream 
                // by the client
                iRx = socketData.m_currentSocket.EndReceive(asyn);
                char[] chars = new char[iRx + 1];
                System.Text.Decoder d = System.Text.Encoding.UTF8.GetDecoder();
                int charLen = d.GetChars(socketData.dataBuffer,
                                         0, iRx, chars, 0);
                System.String szData = new System.String(chars);

                if (chars[0] == 13)
                    //SetText(strRXServer);
                    this.OnDataReceivedFinish(this, new EventArgs());
                else
                    strRXServer += szData.Remove(1, 1);
                //txtResult.AppendText(szData);
                //  SetText1(strRXServer);
                // Continue the waiting for data on the Socket
                WaitForDataS(socketData.m_currentSocket);
            }
            catch (ObjectDisposedException)
            {
                System.Diagnostics.Debugger.Log(0, "1", "\nOnDataReceived: Socket has been closed\n");
            }
            catch (SocketException se)
            {
                //MessageBox.Show(se.Message);
            }
        }

        public void CloseServer()
        {
            if (m_mainSocket != null)
            {
                m_mainSocket.Close();
            }
            for (int i = 0; i < m_clientCount; i++)
            {
                if (m_workerSocket[i] != null)
                {
                    m_workerSocket[i].Close();
                    m_workerSocket[i] = null;
                }
            }
        }

        private class SocketPacket
        {
            public System.Net.Sockets.Socket m_currentSocket;
            public byte[] dataBuffer = new byte[1];
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                mynotifyicon.BalloonTipText = "Monitor Dispenser Running...";
                mynotifyicon.BalloonTipTitle = "Monitor Dispenser";
                mynotifyicon.Visible = true;
                mynotifyicon.ShowBalloonTip(500);
                this.Hide();
            }
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void hideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
