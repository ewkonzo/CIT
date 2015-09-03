using DispenserControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Media;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace Dispenser
{
    public partial class Form1 : Form
    {
        SoundPlayer player = new SoundPlayer();
        MifareReader mifaV;
        DispenserControllor dispenserControl;
        LEDDisplay ledDisplay;
        Image []active, deactive;
        Bitmap bmpPL = null;
        Bitmap bmpPD = null;
        DahuaSDK dahua = new DahuaSDK();
        string tmpFileD, tmpFileL;
        string strFileL, strFileD;
        static int CardLevel, RecordNo;
        string typeCard;
        uint intIDV = 0;
        bool ready = false;
        bool loopSpeaking = false;
        bool toggleDisplay = true;
        bool writeBlock = false;
        bool connectBoard = false;
        int readError = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            loadConfig();
            toggleControlDisplay();
            if (!db.Connect(txtDatabaseIP.Text))
            {
                MessageBox.Show("Cannot connect to database, please check IP Database and restart program");
            }

            player.Stop();
            MaximizeBox = false;
            MinimizeBox = false;
            progressBar1.Visible = true;
            active = new Image[6];
            deactive = new Image[6];
            for (int i = 0; i < 6; i++) {
                active[i] = Image.FromFile("Images/triangle-icon.png");
                deactive[i] = Image.FromFile("Images/dark-icon.png");
            }
           

            axVLCPlugin1.stop();
            axVLCPlugin1.playlistClear();
            axVLCPlugin1.addTarget("rtsp://admin:admin@" + txtCam1.Text + ":554/cam/realmonitor?channel=1&subtype=0", null, AXVLC.VLCPlaylistMode.VLCPlayListReplaceAndGo, 0);
            axVLCPlugin1.play();

            axVLCPlugin2.stop();
            axVLCPlugin2.playlistClear();
            axVLCPlugin2.addTarget("rtsp://admin:admin@" + txtCam2.Text + ":554/cam/realmonitor?channel=1&subtype=0", null, AXVLC.VLCPlaylistMode.VLCPlayListReplaceAndGo, 0);
            axVLCPlugin2.play();

            progressBar1.Visible = false;
            if (rdBack.Checked)
            {
                dahua.InitCamera(txtCam1.Text, txtCam2.Text, "", "");
                progressBar1.Visible = true;
                progressBar1.Value = 10;
                backgroundWorker1.WorkerReportsProgress = true;
                backgroundWorker1.RunWorkerAsync();
            }

            foreach (string s in SerialPort.GetPortNames())
            {
                cmbControlPort.Items.Add(s);
                cmbLEDPort.Items.Add(s);
                cmbReaderPort.Items.Add(s);
            }

            ledDisplay = new LEDDisplay(cmbLEDPort.Text);
            if (ledDisplay.openPort())
            {
                ledDisplay.setText("Creative Innovation Technology", 3);
            }
            else MessageBox.Show("Cannot connect to LED display.");

            mifaV = new MifareReader(false);
            if (mifaV.Open(cmbReaderPort.Text))
            {
                if (mifaV.Connect())
                {
                    dispenserControl = new DispenserControllor(cmbControlPort.Text);
                    if (dispenserControl.openPort())
                    {
                        dispenserControl.portHandle.Write("S\r");
                        dispenserControl.portHandle.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                        Thread.Sleep(700);
                        if (connectBoard)
                        {
                            timer1.Enabled = true;
                        }
                        else
                        {
                            MessageBox.Show("Cannot connect to control board.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Cannot connect to Reader.");
                }
            }

            

        }

        private void loadConfig()
        {
            XmlTextReader reader = new XmlTextReader("Setting.xml");
            while (reader.Read())
            {

                if (reader.NodeType == XmlNodeType.Element && reader.Name == "ServerDir")
                {
                    txtServerDir.Text = reader.ReadElementString();
                }

                if (reader.NodeType == XmlNodeType.Element && reader.Name == "BackupDir")
                {
                    txtBackupDir.Text = reader.ReadElementString();
                }

                if (reader.NodeType == XmlNodeType.Element && reader.Name == "Database")
                {
                    txtDatabaseIP.Text = reader.ReadElementString();
                }

                if (reader.NodeType == XmlNodeType.Element && reader.Name == "ControlPort")
                {
                    cmbControlPort.Text = reader.ReadElementString();
                }

                if (reader.NodeType == XmlNodeType.Element && reader.Name == "LedPort")
                {
                    cmbLEDPort.Text = reader.ReadElementString();
                }

                if (reader.NodeType == XmlNodeType.Element && reader.Name == "ReaderPort")
                {
                    cmbReaderPort.Text = reader.ReadElementString();
                }

                if (reader.NodeType == XmlNodeType.Element && reader.Name == "CamIp1")
                {
                    txtCam1.Text = reader.ReadElementString();
                }

                if (reader.NodeType == XmlNodeType.Element && reader.Name == "CamIp2")
                {
                    txtCam2.Text = reader.ReadElementString();
                }

                if (reader.NodeType == XmlNodeType.Element && reader.Name == "CaptureMode")
                {
                    string mode = reader.ReadElementString();
                    if (mode == "Fore")
                        rdFore.Checked = true;
                    if (mode == "Back")
                        rdBack.Checked = true;
                }

                if (reader.NodeType == XmlNodeType.Element && reader.Name == "SendData") 
                {
                    txtSendData.Text = reader.ReadElementString();
                }
            }
            reader.Close();
        }

        private void saveConfig()
        {
            XmlWriter xmlWriter = XmlWriter.Create("Setting.xml");
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("DispenserControl");
            xmlWriter.WriteStartElement("ServerDir");
            xmlWriter.WriteString(txtServerDir.Text);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("BackupDir");
            xmlWriter.WriteString(txtBackupDir.Text);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("Database");
            xmlWriter.WriteString(txtDatabaseIP.Text);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("ControlPort");
            xmlWriter.WriteString(cmbControlPort.Text);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("LedPort");
            xmlWriter.WriteString(cmbLEDPort.Text);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("ReaderPort");
            xmlWriter.WriteString(cmbReaderPort.Text);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("CamIp1");
            xmlWriter.WriteString(txtCam1.Text);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("CamIp2");
            xmlWriter.WriteString(txtCam2.Text);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("CaptureMode");
            if (rdBack.Checked)
                xmlWriter.WriteString("Back");
            else xmlWriter.WriteString("Fore");
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("SendData");
            xmlWriter.WriteString(txtSendData.Text);
            xmlWriter.WriteEndElement();
            xmlWriter.Close();
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            int dataInt = sp.ReadByte();
            string dataString = Convert.ToString(dataInt, 2).PadLeft(8, '0');

            if (dataString[0] == '1')
            {
                connectBoard = true;
            }

            if (dataString[2] == '1') picLoop.Image = active[0];//Loop
            else picLoop.Image = deactive[0];

            if (dataString[3] == '1') picRed.Image = active[1];//Red Btn
            else picRed.Image = deactive[1];

            if (dataString[4] == '1') picGreen.Image = active[2];//Green Btn
            else picGreen.Image = deactive[2];

            if (dataString[5] == '1') picReadOk.Image = active[3];// Read OK
            else picReadOk.Image = deactive[3];

            if (dataString[6] == '1') picLow.Image = active[4];//Low
            else picLow.Image = deactive[4];

            if (dataString[7] == '1') picEmpty.Image = active[5];//Empty
            else picEmpty.Image = deactive[5];

            if (dataString[1] == '1')
            {//Ready State

                playSound(4);
                if (rdBack.Checked)
                    takePhotoBack();
                if (rdFore.Checked)
                    takePhotoFore();
               if(ledDisplay.sendData) ledDisplay.setText("PullCard", 3);
                ready = true;
                Thread.Sleep(1000);
                //   SetText7("บัตรพร้อมหยิบ");
            }
            Console.WriteLine(dataString);
            Client.StartClient(dataString, txtSendData.Text);

            if (dataString[5] == '0' && ready)
            {
                //ดึงบัตร
                //Save Data
                if (CarInRecord(intIDV.ToString(), "0", "NO", "", strFileD, strFileL))
                {
                    string sql = "UPDATE card" + typeCard + " SET no =" + RecordNo2.ToString();
                    sql += " WHERE name=" + intIDV.ToString();
                    db.SaveData(sql);
                    CarInRecordB(intIDV.ToString(), "0", "NO", "", strFileD, strFileL, txtBackupDir.Text);
                    ready = false;
                    if (ledDisplay.sendData) ledDisplay.setText("Welcome :)", 3);
                    playSound(1);
                    dispenserControl.liftOut();
                    dispenserControl.portHandle.Write("W\r");
                    Thread.Sleep(3000);
                    writeBlock = false;
                    if (ledDisplay.sendData) ledDisplay.setText("Creative Innovation Technology", 3);
                }
                //  label13.Text = "ReadOK ";
            }

            if (dataString[2] == '1')
            {
                if (!loopSpeaking)
                {
                    playSound(2);
                    if (ledDisplay.sendData) ledDisplay.setText("Press Green Button", 3);
                    loopSpeaking = true;
                }
            }
            else
            {
                if (loopSpeaking)
                {
                    if (ledDisplay.sendData) ledDisplay.setText("Creative Innovation Technology", 3);
                }
                loopSpeaking = false;
            }



        }
        /// <summary>
        /// default hello
        /// </summary>
        /// <param name="sound">
        /// 1. hello
        /// 2. pressGreen
        /// 3. pressRed
        /// 4. pullCard
        /// 5. thankyou
        /// </param>
        void playSound(int sound = 1)
        {
            switch (sound)
            {
                case 1: player.SoundLocation = "Sounds/hello.wav"; break;
                case 2: player.SoundLocation = "Sounds/pressGreen.wav"; break;
                case 3: player.SoundLocation = "Sounds/pressRed.wav"; break;
                case 4: player.SoundLocation = "Sounds/pullCard.wav"; break;
                case 5: player.SoundLocation = "Sounds/thankyou.wav"; break;
                default: player.SoundLocation = "Sounds/hello.wav"; break;
            }
            player.Play();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!writeBlock)
            {
                if (mifaV.chkCard())
                {
                    string strNum = mifaV.Init1();
                    if (strNum != "")
                    {
                        intIDV = Convert.ToUInt32(strNum, 16);
                        mifaV.Init2();

                        if (mifaV.Login(4))
                        {
                            if (mifaV.WriteBlock(5, "V000000000000000"))
                            {
                                if (checkCard(intIDV))
                                {
                                    readError = 0;
                                    dispenserControl.portHandle.Write("R\r");//Ready for Green Button
                                    Console.WriteLine("ID CARD :" + intIDV.ToString());
                                    label13.Text = "ReadOK " + intIDV.ToString();
                                    writeBlock = true;
                                    Thread.Sleep(1000);
                                }
                                else
                                {
                                    label13.Text = "ReadOK ";
                                    //    db.SaveData("INSERT INTO cardmf (name,level) VALUES("+intIDV.ToString()+",1)");
                                    readError = 0;
                                    dispenserControl.callBack();
                                    Thread.Sleep(3000);
                                }
                            }
                        }
                        else
                        {
                            readError++;
                            if (readError > 8)
                            {
                                label13.Text = "ReadOK ";
                                readError = 0;
                                dispenserControl.callBack();
                                Thread.Sleep(3000);
                            }
                        }

                    }
                    else
                    {
                        readError++;
                        if (readError > 8)
                        {
                            label13.Text = "ReadOK ";
                            readError = 0;
                            dispenserControl.callBack();
                            Thread.Sleep(3000);
                        }
                    }
                }
                else
                {
                    readError++;
                    if (readError > 8)
                    {
                        label13.Text = "ReadOK ";
                        readError = 0;
                        dispenserControl.callBack();
                        Thread.Sleep(3000);
                    }
                }
            }
        }

        private void takePhotoFore()
        {
            try
            {
                int x = axVLCPlugin1.ClientRectangle.Width;
                int y = axVLCPlugin1.ClientRectangle.Height;
                bmpPL = new Bitmap(x, y);
                Graphics gfxScreenshot = Graphics.FromImage(bmpPL);
                System.Drawing.Size imgSize = new System.Drawing.Size(x, y);
                Point ps;
                axVLCPlugin1.Invoke(new MethodInvoker(delegate
                {
                    ps = PointToScreen(new Point(axVLCPlugin1.Bounds.X, axVLCPlugin1.Bounds.Y));
                    gfxScreenshot.CopyFromScreen(ps.X + groupBox1.Location.X, ps.Y + groupBox1.Location.Y, 0, 0, imgSize, CopyPixelOperation.SourceCopy);
                }));
                x = axVLCPlugin2.ClientRectangle.Width;
                y = axVLCPlugin2.ClientRectangle.Height;
                bmpPD = new Bitmap(x, y);
                gfxScreenshot = Graphics.FromImage(bmpPD);
                imgSize = new System.Drawing.Size(x, y);
                axVLCPlugin2.Invoke(new MethodInvoker(delegate
                {
                    ps = PointToScreen(new Point(axVLCPlugin2.Bounds.X, axVLCPlugin2.Bounds.Y));
                    gfxScreenshot.CopyFromScreen(ps.X + groupBox1.Location.X, ps.Y + groupBox1.Location.Y, 0, 0, imgSize, CopyPixelOperation.SourceCopy);
                }));
                picCam1.Image = bmpPD;
                picCam2.Image = bmpPL;
            }
            catch (Exception e)
            {
            }
            SaveImage(bmpPD, bmpPL, txtServerDir.Text, "VI");
            SaveImage(bmpPD, bmpPL, txtBackupDir.Text, "VI");
        }

        private void takePhotoBack()
        {
            SaveImageDahua(ref bmpPD, ref bmpPL, txtServerDir.Text, "VI");
            Thread.Sleep(1500);
            SaveImageDahua(ref bmpPD, ref bmpPL, txtBackupDir.Text, "VI");
            picCam1.Image = bmpPL;
            picCam2.Image = bmpPD;
        }


        public void SaveImageDahua(ref Bitmap bmpPD, ref Bitmap bmpPL, string strDir, string strMode)
        {

            DateTime now = DateTime.Now;
            string strFile = now.ToString("ddMMyyyy_HHmmss") + ".jpg";
            string folder = now.Month.ToString();
            strFileL = strDir + "\\" + folder;

            if (!Directory.Exists(strFileL))
            {
                Directory.CreateDirectory(strFileL);
            }
            folder = now.Day.ToString();
            strFileL = strFileL + "\\" + folder;
            if (!Directory.Exists(strFileL))
            {
                Directory.CreateDirectory(strFileL);
            }
            strFileD = "";

            strFileD = strFileL;
            strFileD = strFileD + "\\" + strMode + "D" + strFile;
            strFileL += "\\" + strMode + "L" + strFile;

            if (strDir.IndexOf("server") > -1 || strDir.IndexOf("Server") > -1)
            {
                if (strMode == "VI")
                    dahua.capture(strFileL, strFileD, "", "");
                else dahua.capture("", "", strFileL, strFileD);
                tmpFileD = strFileD;
                tmpFileL = strFileL;
            }
            else
            {
                try
                {
                    Bitmap bD = (Bitmap)Image.FromFile(tmpFileD);
                    Bitmap varBmp = new Bitmap(bD);
                    Bitmap newBitmap = new Bitmap(varBmp);
                    varBmp.Save(strFileD, ImageFormat.Jpeg);
                    varBmp.Dispose();
                    varBmp = null;
                    /* if (strMode == "MI") bmpPD = bD;
                     else*/
                    bmpPD = bD;
                }
                catch (Exception) { }

                try
                {
                    Bitmap bL = (Bitmap)Image.FromFile(tmpFileL);
                    Bitmap varBmp = new Bitmap(bL);
                    Bitmap newBitmap = new Bitmap(varBmp);
                    varBmp.Save(strFileL, ImageFormat.Jpeg);
                    varBmp.Dispose();
                    varBmp = null;
                    /* if (io == "IN") pic2.Image = bL;
                     else*/
                    bmpPL = bL;
                }
                catch (Exception) { }
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            dahua.LoginCam();
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            for (int i = 2; i <= 20; i++)
            {
                progressBar1.Value = i * 5;
                Thread.Sleep(100);
            }

            progressBar1.Visible = false;
        }



        static public bool CarInRecord(string intID, string strCarType, string strLicense, string strRandKey, string strImageD, string strImageL)// Bitmap bmL, Bitmap bmD
        {
            try
            {
                getRecordNo();
                RecordNo2++;
                string sql = "INSERT INTO recordin (no,id,cartype,license,rankey,picdiv,piclic,datein,userin) VALUES (";//
                strImageL = strImageL.Replace("\\", "\\\\");
                strImageD = strImageD.Replace("\\", "\\\\");
                sql += RecordNo2.ToString() + "," + intID + "," + strCarType + ",'" + strLicense + "','" + strRandKey + "','" + strImageD + "','" + strImageL + "',NOW()," + 1 + ")";//
                if (db.SaveData(sql) == "")
                    return true;
                else return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        static public bool CarInRecordMember(string intID, string strCarType, string strLicense, string strRandKey, string strImageD, string strImageL, string guardhouse)// Bitmap bmL, Bitmap bmD
        {
            try
            {
                getRecordNo();
                RecordNo2++;
                string sql = "INSERT INTO recordin (no,id,cartype,license,rankey,picdiv,piclic,datein,userin,guardhouse) VALUES (";//
                strImageL = strImageL.Replace("\\", "\\\\");
                strImageD = strImageD.Replace("\\", "\\\\");
                sql += RecordNo2.ToString() + "," + intID + "," + strCarType + ",(SELECT license FROM member WHERE cardid = " + intID + "),'" + strRandKey + "','" + strImageD + "','" + strImageL + "',NOW()," + 1 + ",'" + guardhouse + "')";//
                if (db.SaveData(sql) == "")
                    return true;
                else return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static void CarInRecordB(string intID, string strCarType, string strLicense, string strRandKey, string strImageD, string strImageL, string BackupDirectory)
        {
            DateTime now = DateTime.Now;
            string folder = now.Month.ToString();
            string strFile = BackupDirectory + "\\" + folder;
            if (!Directory.Exists(strFile))
            {
                Directory.CreateDirectory(strFile);
            }
            strFile = strFile + "\\InBackup_" + now.ToString("ddMMyyyy") + ".csv";
            string strHeader = "sql,no,id,cartype,license,rankey,picdiv,piclic,datein,userin";
            string sql = "INSERT INTO recordin VALUES (";//
            strImageL = strImageL.Replace("\\", "\\\\");
            strImageD = strImageD.Replace("\\", "\\\\");
            //sql += RecordNo.ToString() + "," + intID.ToString() + "," + strCarType + ",'" + strLicense + "','" + strRandKey + "','" + strImageD + "','" + strImageL + "',NOW()," + user.ID + "," + proID.ToString() + ");"; //Mac 2014/10/13
            sql += RecordNo2.ToString() + "," + intID + "," + strCarType + ",'" + strLicense + "','" + strRandKey + "','" + strImageD + "','" + strImageL + "'," + now.ToString("yyyy-MM-dd HH:mm:ss") + "," + 1 + "," + 0 + ");"; //Mac 2014/10/13
            //string strRecord = RecordNo.ToString() + "," + intID.ToString() + "," + strCarType + "," + strLicense + "," + strRandKey + "," + strImageD + "," + strImageL + "," + now.ToString() + "," + user.ID + ")";//
            if (!File.Exists(strFile))
            {
                StreamWriter sw = File.CreateText(strFile);
                sw.WriteLine(strHeader);
                sw.WriteLine(sql);
                sw.Flush();
                sw.Close();
            }
            else
            {
                FileStream MyFileStream = new FileStream(strFile, FileMode.Append, FileAccess.Write, FileShare.Read);
                StreamWriter sw = new StreamWriter(MyFileStream);
                sw.WriteLine(sql);
                sw.Close();
                MyFileStream.Close();
            }
        }

        public static void CarInRecordBMember(string intID, string strCarType, string strLicense, string strRandKey, string strImageD, string strImageL, string BackupDirectory, string guardhouse)
        {
            DateTime now = DateTime.Now;
            string folder = now.Month.ToString();
            string strFile = BackupDirectory + "\\" + folder;
            if (!Directory.Exists(strFile))
            {
                Directory.CreateDirectory(strFile);
            }
            strFile = strFile + "\\InBackup_" + now.ToString("ddMMyyyy") + ".csv";
            string strHeader = "sql,no,id,cartype,license,rankey,picdiv,piclic,datein,userin,guardhouse";
            string sql = "INSERT INTO recordin VALUES (";//
            strImageL = strImageL.Replace("\\", "\\\\");
            strImageD = strImageD.Replace("\\", "\\\\");
            //sql += RecordNo.ToString() + "," + intID.ToString() + "," + strCarType + ",'" + strLicense + "','" + strRandKey + "','" + strImageD + "','" + strImageL + "',NOW()," + user.ID + "," + proID.ToString() + ");"; //Mac 2014/10/13
            sql += RecordNo2.ToString() + "," + intID + "," + strCarType + ",(SELECT license FROM member WHERE cardid = " + intID + "),'" + strRandKey + "','" + strImageD + "','" + strImageL + "'," + now.ToString("yyyy-MM-dd HH:mm:ss") + "," + 1 + "," + 0 + ",'" + guardhouse + "');"; //Mac 2014/10/13
            //string strRecord = RecordNo.ToString() + "," + intID.ToString() + "," + strCarType + "," + strLicense + "," + strRandKey + "," + strImageD + "," + strImageL + "," + now.ToString() + "," + user.ID + ")";//
            if (!File.Exists(strFile))
            {
                StreamWriter sw = File.CreateText(strFile);
                sw.WriteLine(strHeader);
                sw.WriteLine(sql);
                sw.Flush();
                sw.Close();
            }
            else
            {
                FileStream MyFileStream = new FileStream(strFile, FileMode.Append, FileAccess.Write, FileShare.Read);
                StreamWriter sw = new StreamWriter(MyFileStream);
                sw.WriteLine(sql);
                sw.Close();
                MyFileStream.Close();
            }
        }

        static int RecordNo2 = 0;
        static public void getRecordNo()
        {
            string sql = "SELECT MAX(no) FROM recordin";
            DataTable dt = db.LoadData(sql);
            try
            {
                if (dt.Rows.Count > 0)
                {
                    RecordNo2 = Convert.ToInt32(dt.Rows[0].ItemArray[0]);
                }
            }
            catch (Exception)
            {
                RecordNo2 = 0;
            }

        }

        private bool checkCard(uint cardid)
        {
            bool result = false;
            try
            {

                string sql = "";
                //Check Card Prox
                sql = "SELECT * FROM cardpx ";
                sql += " WHERE name=" + cardid;
                DataTable dt = db.LoadData(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    typeCard = "px";
                    CardLevel = Convert.ToInt32(dt.Rows[0].ItemArray[1]);
                    RecordNo = Convert.ToInt32(dt.Rows[0].ItemArray[2]);
                    if (CardLevel > 0 && RecordNo == 0)
                        result = true;
                }
                else
                {
                    sql = "SELECT * FROM cardmf ";
                    sql += " WHERE name=" + cardid;
                    dt = db.LoadData(sql);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        typeCard = "mf";
                        CardLevel = Convert.ToInt32(dt.Rows[0].ItemArray[1]);
                        RecordNo = Convert.ToInt32(dt.Rows[0].ItemArray[2]);
                        if (CardLevel > 0 && RecordNo == 0)
                            result = true;
                    }
                }

            }
            catch (Exception e)
            {
            }
            return result;
        }

        private void SaveImage(Bitmap bmpPD, Bitmap bmpPL, string strDir, string strMode)
        {
            try
            {
                DateTime now = DateTime.Now;
                string strFile = now.ToString("ddMMyyyy_HHmmss") + ".jpg";
                string folder = now.Month.ToString();
                strFileL = strDir + "\\" + folder;

                try //Mac 2014/11/25
                {

                    if (!Directory.Exists(strFileL))
                    {
                        Directory.CreateDirectory(strFileL);
                    }
                    folder = now.Day.ToString();
                    strFileL = strFileL + "\\" + folder;
                    if (!Directory.Exists(strFileL))
                    {
                        Directory.CreateDirectory(strFileL);
                    }
                    strFileD = "";

                    strFileD = strFileL;
                    strFileD = strFileD + "\\" + strMode + "D" + strFile;
                    Bitmap varBmp = new Bitmap(bmpPD);
                    Bitmap newBitmap = new Bitmap(varBmp);
                    varBmp.Save(strFileD, ImageFormat.Jpeg);
                    varBmp.Dispose();
                    varBmp = null;
                }
                catch (Exception) { }

                try
                {
                    strFileL += "\\" + strMode + "L" + strFile;
                    Bitmap varBmpL = new Bitmap(bmpPL);
                    Bitmap newBitmapL = new Bitmap(varBmpL);
                    varBmpL.Save(strFileL, ImageFormat.Jpeg);
                    varBmpL.Dispose();
                    varBmpL = null;
                }
                catch (Exception)
                {
                }
            }
            catch (Exception e)
            {
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            toggleControlDisplay();
            if (!toggleDisplay)
            {
                saveConfig();
                MessageBox.Show("Saved! restart to Load Config");
                Application.Exit();
            }
            else {
                timer1.Enabled = false;
            }
        }

        private void toggleControlDisplay()
        {
            if (toggleDisplay)
            {
                toggleDisplay = false;
                btnEdit.Text = "Edit";
            }
            else
            {
                toggleDisplay = true;
                btnEdit.Text = "Save";
            }

            txtBackupDir.Enabled = toggleDisplay;
            txtServerDir.Enabled = toggleDisplay;
            txtDatabaseIP.Enabled = toggleDisplay;
            txtCam1.Enabled = toggleDisplay;
            txtCam2.Enabled = toggleDisplay;
            rdBack.Enabled = toggleDisplay;
            rdFore.Enabled = toggleDisplay;
            cmbControlPort.Enabled = toggleDisplay;
            cmbLEDPort.Enabled = toggleDisplay;
            cmbReaderPort.Enabled = toggleDisplay;
            txtSendData.Enabled = toggleDisplay;

        }

    }
}
