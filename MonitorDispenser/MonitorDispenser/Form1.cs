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
        public Form1()
        {
            InitializeComponent();
        }
        // Incoming data from the client.
        public static string data = null;
        Image[] active, deactive;
        bool msgLow = false;
        bool msgEmpty = false;
        private void Form1_Load(object sender, EventArgs e)
        {
            
            
            MaximizeBox = false;
            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            lbLocal.Text += ipAddress.ToString();
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.RunWorkerAsync();
            active = new Image[6];
            deactive = new Image[6];
            for (int i = 0; i < 6; i++)
            {
                active[i] = Image.FromFile("Images/triangle-icon.png");
                deactive[i] = Image.FromFile("Images/dark-icon.png");
            }
        }

        public void StartListening()
        {
            // Data buffer for incoming data.
            byte[] bytes = new Byte[1024];

            // Establish the local endpoint for the socket.
            // Dns.GetHostName returns the name of the 
            // host running the application.
            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

            // Create a TCP/IP socket.
            Socket listener = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and 
            // listen for incoming connections.
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);

                // Start listening for connections.
                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");
                    // Program is suspended while waiting for an incoming connection.
                    Socket handler = listener.Accept();
                    data = null;

                    // An incoming connection needs to be processed.
                    while (true)
                    {
                        bytes = new byte[1024];
                        int bytesRec = handler.Receive(bytes);
                        data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                        if (data.IndexOf("<EOF>") > -1)
                        {
                            string dataTemp = data.Replace("<EOF>","");

                            if (dataTemp[2] == '1') picLoop.Image = active[0];
                            else picLoop.Image = deactive[0];

                            if (dataTemp[3] == '1') picRed.Image = active[1];
                            else picRed.Image = deactive[1];

                            if (dataTemp[4] == '1') picGreen.Image = active[2];
                            else picGreen.Image = deactive[2];

                            if (dataTemp[5] == '1') picReadOk.Image = active[3];
                            else picReadOk.Image = deactive[3];

                            if (dataTemp[6] == '1')
                            {
                                if (!msgLow) MessageBox.Show(this,"การ์ดเหลือน้อยกรุณาเติมบัตร");
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

                            if (dataTemp[7] == '1') { 
                                picEmpty.Image = active[5];
                                picLow.Image = active[4];
                                mynotifyicon.BalloonTipText = "Card Empty...";
                                mynotifyicon.BalloonTipTitle = "Monitor Dispenser";
                                mynotifyicon.Visible = true;
                                mynotifyicon.ShowBalloonTip(500);
                            }
                            else{ 
                                picEmpty.Image = deactive[5];
                                mynotifyicon.Visible = true;
                            }

                            break;
                        }
                    }

                    // Show the data on the console.
                    Console.WriteLine("Text received : {0}", data);

                    // Echo the data back to the client.
                    byte[] msg = Encoding.ASCII.GetBytes(data);

                    handler.Send(msg);
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            StartListening();
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
