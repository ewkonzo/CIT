using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public  class ClientDispenser
{
    private byte[] m_dataBuffer = new byte[10];
    private IAsyncResult m_result;
    private AsyncCallback m_pfnCallBack;
    private Socket m_clientSocket;
    public String strRXServer = "";
    //cliant
    public event EventHandler OnDataReceivedFinish;
    private delegate void SetTextCallback(string text);

      public bool Connect2Server(string strIP, int port)
        {
            bool booResult = true;
            try
            {
                // Create the socket instance
                m_clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // Cet the remote IP address
                IPAddress ip = IPAddress.Parse(strIP);
                //int iPortNo = System.Convert.ToInt16(textBoxPort.Text);
                // Create the end point 
                IPEndPoint ipEnd = new IPEndPoint(ip, port);
                // Connect to the remote host
                m_clientSocket.Connect(ipEnd);
                if (m_clientSocket.Connected)
                {
                    WaitForDataC();
                }
            }
            catch (SocketException se)
            {
                booResult = false;
            }
            return booResult;
        }


        public bool Send2Server(string strSend)
        {
            bool booResult = true;
            try
            {
                Object objData = strSend + "\r";
                byte[] byData = System.Text.Encoding.ASCII.GetBytes(objData.ToString());
                if (m_clientSocket != null)
                {
                    m_clientSocket.Send(byData);
                }
            }
            catch (SocketException se)
            {
                //MessageBox.Show(se.Message);
                booResult = false;
            }
            return booResult;
        }

        private void WaitForDataC()
        {
            try
            {
                if (m_pfnCallBack == null)
                {
                    m_pfnCallBack = new AsyncCallback(OnDataReceivedC);
                }
                SocketPacket theSocPkt = new SocketPacket();
                theSocPkt.m_currentSocket = m_clientSocket;
                // Start listening to the data asynchronously
                m_result = m_clientSocket.BeginReceive(theSocPkt.dataBuffer,
                                                        0, theSocPkt.dataBuffer.Length,
                                                        SocketFlags.None,
                                                        m_pfnCallBack,
                                                        theSocPkt);
            }
            catch (SocketException se)
            {
                //MessageBox.Show(se.Message);
            }
        }

        private void OnDataReceivedC(IAsyncResult asyn)
        {
            try
            {
                SocketPacket theSockId = (SocketPacket)asyn.AsyncState;

                int iRx = theSockId.m_currentSocket.EndReceive(asyn);
                char[] chars = new char[iRx + 1];
                System.Text.Decoder d = System.Text.Encoding.UTF8.GetDecoder();
                int charLen = d.GetChars(theSockId.dataBuffer, 0, iRx, chars, 0);
                System.String szData = new System.String(chars);

                if (chars[0] == 13)
                    //SetText(strRXServer);
                    this.OnDataReceivedFinish(this, new EventArgs());
                else
                    try { strRXServer += szData.Remove(1, 1); }
                    catch (Exception) { }
              //  textBox1.AppendText(szData);
                WaitForDataC();
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

        public void CloseClient()
        {
            if (m_clientSocket != null)
            {
                m_clientSocket.Close();
                m_clientSocket = null;
            }
        }

        private class SocketPacket
        {
            public System.Net.Sockets.Socket m_currentSocket;
            public byte[] dataBuffer = new byte[1];
        }

        //private void btnConnect_Click(object sender, EventArgs e)
        //{
        //    label1.Text = Convert.ToString(Connect2Server(txtIP.Text, Convert.ToInt32(txtPort.Text)));
        //    this.OnDataReceivedFinish +=Form1_OnDataReceivedFinish;
        //}

        //private void Form1_OnDataReceivedFinish(object sender, EventArgs e)
        //{
        //    SetText1(strRXServer);
        //    strRXServer = "";
        //}

        //private void textBox2_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.KeyCode == Keys.Enter)
        //    {
        //        Send2Server(textBox2.Text);
        //        textBox2.Text = "";
        //    }
        //}

        //private void SetText1(string text)
        //{
        //    if (this.textBox1.InvokeRequired)
        //    {
        //        SetTextCallback d = new SetTextCallback(SetText1);
        //        this.Invoke(d, new object[] { text });
        //    }
        //    else
        //    {
        //        //this.lbACD1.Text += text + "\r\n";
        //        textBox1.AppendText(text + "\r\n");
        //    }
        //}
  
}