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

namespace testServer
{
    public partial class Form1 : Form
    {
        //server
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
            label1.Text = this.OpenServer(8100)+" PORT:8100";
            this.OnDataReceivedFinish +=Form1_OnDataReceivedFinish;
        }

        private void Form1_OnDataReceivedFinish(object sender, EventArgs e)
        {
            SetText1(strRXServer);
            strRXServer = "";
        }

        public string OpenServer(int port)
        {
            string strResult = "Open OK";
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
                strResult = se.ToString();
            }
            return strResult;
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

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseServer();
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) {
                Send2Client(textBox2.Text);
                textBox2.Text = "";
            }
        }

        private void SetText1(string text)
        {
            if (this.textBox1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText1);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                //this.lbACD1.Text += text + "\r\n";
                textBox1.AppendText(text + "\r\n");
            }
        }
    }
}
