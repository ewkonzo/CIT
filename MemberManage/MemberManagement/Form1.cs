using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Drawing.Imaging;

namespace MemberManagement
{
    public partial class Form1 : Form
    {
        public string ctrlIP1, ctrlIP2;
        private Bitmap bmpPL = null;
        private Bitmap bmpPD = null;
        private Bitmap bmpPL2 = null;
        private Bitmap bmpPD2 = null;
        static string strFileL = " ", strFileD = " ";
        private int CardLevel = 0, RecordNo = 0;
        private string typeCard;
        DahuaSDK dahua = new DahuaSDK();
        Member member = new Member();
        private delegate void SetTextCallback(string text);



        public Form1()
        {
            InitializeComponent();
        }

        public void loadDahua()
        {
            dahua.InitCamera(txtCamIP1.Text, txtCamIP2.Text,txtCamIP3.Text, txtCamIP4.Text);
            dahua.LoginCam();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MaximizeBox = false;
            loadConfig();
            if (!db.Connect(txtDBIP.Text, txtDBName.Text)) {
                MessageBox.Show(this, "Cannot connect database!\nchange your IP and Name database and click SAVE ALL", "Waring!", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
            }
            if (chDahua.Checked)
            {
                lbLogin.Visible = true;
                backgroundWorker1.WorkerReportsProgress = true;
                backgroundWorker1.RunWorkerAsync();
            }
            axVLCPlugin1.stop();
            axVLCPlugin1.playlistClear();
            axVLCPlugin1.addTarget("rtsp://admin:admin@" + txtCamIP1.Text + ":554/cam/realmonitor?channel=1&subtype=0", null, AXVLC.VLCPlaylistMode.VLCPlayListReplaceAndGo, 0);
            axVLCPlugin1.play();

            axVLCPlugin2.stop();
            axVLCPlugin2.playlistClear();
            axVLCPlugin2.addTarget("rtsp://admin:admin@" + txtCamIP2.Text + ":554/cam/realmonitor?channel=1&subtype=0", null, AXVLC.VLCPlaylistMode.VLCPlayListReplaceAndGo, 0);
            axVLCPlugin2.play();

            axVLCPlugin3.stop();
            axVLCPlugin3.playlistClear();
            axVLCPlugin3.addTarget("rtsp://admin:admin@" + txtCamIP3.Text + ":554/cam/realmonitor?channel=1&subtype=0", null, AXVLC.VLCPlaylistMode.VLCPlayListReplaceAndGo, 0);
            axVLCPlugin3.play();

            axVLCPlugin4.stop();
            axVLCPlugin4.playlistClear();
            axVLCPlugin4.addTarget("rtsp://admin:admin@" + txtCamIP4.Text + ":554/cam/realmonitor?channel=1&subtype=0", null, AXVLC.VLCPlaylistMode.VLCPlayListReplaceAndGo, 0);
            axVLCPlugin4.play();
        }

        public void loadConfig()
        {
            try
            {
                var lines = File.ReadAllLines("config.txt");
                foreach (var line in lines)
                {
                    if (line.Split(';')[0] == "IPCAM1")
                        txtCamIP1.Text = line.Split(';')[1];
                    if (line.Split(';')[0] == "IPCAM2")
                        txtCamIP2.Text = line.Split(';')[1];
                    if (line.Split(';')[0] == "IPCAM3")
                        txtCamIP3.Text = line.Split(';')[1];
                    if (line.Split(';')[0] == "IPCAM4")
                        txtCamIP4.Text = line.Split(';')[1];

                    if (line.Split(';')[0] == "CAM1ENABLE")
                        chCam1.Checked = Convert.ToBoolean(line.Split(';')[1]);
                    if (line.Split(';')[0] == "CAM2ENABLE")
                        chCam2.Checked = Convert.ToBoolean(line.Split(';')[1]);
                    if (line.Split(';')[0] == "CAM3ENABLE")
                        chCam3.Checked = Convert.ToBoolean(line.Split(';')[1]);
                    if (line.Split(';')[0] == "CAM4ENABLE")
                        chCam4.Checked = Convert.ToBoolean(line.Split(';')[1]);
                    if (line.Split(';')[0] == "USEDAHUA")
                        chDahua.Checked = Convert.ToBoolean(line.Split(';')[1]);


                    if (line.Split(';')[0] == "SERDIR")
                        txtServerDir.Text = line.Split(';')[1];
                    if (line.Split(';')[0] == "BACDIR")
                        txtBackupDir.Text = line.Split(';')[1];
                    if (line.Split(';')[0] == "BASEIP")
                        txtDBIP.Text = line.Split(';')[1];
                    if (line.Split(';')[0] == "BASENAME")
                        txtDBName.Text = line.Split(';')[1];

                    if (line.Split(';')[0] == "CONIP1")
                        txtControlIP1.Text = line.Split(';')[1];
                    if (line.Split(';')[0] == "CONIP2")
                        txtControlIP2.Text = line.Split(';')[1];
                }
            }
            catch (Exception)
            {

                txtControlIP1.Text = "192.168.1.98";
                txtControlIP2.Text = "192.168.1.99";
                txtCamIP1.Text = "192.168.1.109";
                txtCamIP2.Text = "192.168.1.111";
                txtCamIP3.Text = "192.168.1.109";
                txtCamIP4.Text = "192.168.1.111";

                txtServerDir.Text = "Z:\\carpark\\server";
                txtBackupDir.Text = "Z:\\carpark\\backup";
                txtDBIP.Text = "192.168.1.131";
                txtDBName.Text = "carpark2";

                chCam1.Checked = true;
                chCam2.Checked = true;
                chCam3.Checked = true;
                chCam4.Checked = true;
                chDahua.Checked = true;

                string createText;
                createText = "IPCAM1;" + txtCamIP1.Text + Environment.NewLine;
                createText += "IPCAM2;" + txtCamIP2.Text + Environment.NewLine;
                createText += "IPCAM3;" + txtCamIP3.Text + Environment.NewLine;
                createText += "IPCAM4;" + txtCamIP4.Text + Environment.NewLine;
                //CAM1ENABLE
                createText += "CAM1ENABLE;" + chCam1.Checked.ToString() + Environment.NewLine;
                createText += "CAM2ENABLE;" + chCam2.Checked.ToString() + Environment.NewLine;
                createText += "CAM3ENABLE;" + chCam3.Checked.ToString() + Environment.NewLine;
                createText += "CAM4ENABLE;" + chCam4.Checked.ToString() + Environment.NewLine;

                createText += "SERDIR;" + txtServerDir.Text + Environment.NewLine;
                createText += "BACDIR;" + txtBackupDir.Text + Environment.NewLine;
                createText += "CONIP1;" + txtControlIP1.Text + Environment.NewLine;
                createText += "CONIP2;" + txtControlIP2.Text + Environment.NewLine;
                createText += "BASEIP;" + txtDBIP.Text + Environment.NewLine;
                createText += "BASENAME;" + txtDBName.Text + Environment.NewLine;
                createText += "USEDAHUA;" + chDahua.Checked.ToString() + Environment.NewLine;

                File.WriteAllText("config.txt", createText);
            }
            ctrlIP1 = txtControlIP1.Text;
            ctrlIP2 = txtControlIP2.Text;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (txtControlIP1.Text == txtControlIP2.Text)
            {
                MessageBox.Show(this, "Please difference IP controllor!", "Waring!", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
                return;
            }

            string createText;
            createText = "IPCAM1;" + txtCamIP1.Text + Environment.NewLine;
            createText += "IPCAM2;" + txtCamIP2.Text + Environment.NewLine;
            createText += "IPCAM3;" + txtCamIP3.Text + Environment.NewLine;
            createText += "IPCAM4;" + txtCamIP4.Text + Environment.NewLine;
            //CAM1ENABLE
            createText += "CAM1ENABLE;" + chCam1.Checked.ToString() + Environment.NewLine;
            createText += "CAM2ENABLE;" + chCam2.Checked.ToString() + Environment.NewLine;
            createText += "CAM3ENABLE;" + chCam3.Checked.ToString() + Environment.NewLine;
            createText += "CAM4ENABLE;" + chCam4.Checked.ToString() + Environment.NewLine;

            createText += "SERDIR;" + txtServerDir.Text + Environment.NewLine;
            createText += "BACDIR;" + txtBackupDir.Text + Environment.NewLine;
            createText += "CONIP1;" + txtControlIP1.Text + Environment.NewLine;
            createText += "CONIP2;" + txtControlIP2.Text + Environment.NewLine;
            createText += "BASEIP;" + txtDBIP.Text + Environment.NewLine;
            createText += "BASENAME;" + txtDBName.Text + Environment.NewLine;
            createText += "USEDAHUA;" + chDahua.Checked.ToString() + Environment.NewLine;

            File.WriteAllText("config.txt", createText);
            Thread.Sleep(1500);
            MessageBox.Show("Restart program to load config!", "Saved!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Application.Exit();
        }

        public void setTextDetail1(string text)
        {
            if (this.lbDetail1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(setTextDetail1);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                lbDetail1.Text = text;
            }
        }

        public void setTextDetail2(string text)
        {
            if (this.lbDetail2.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(setTextDetail2);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                lbDetail2.Text = text;
            }
        }

        public void takePhotoFore1()
        {
            try
            {
                if (!chDahua.Checked)
                {
                    int x = axVLCPlugin1.ClientRectangle.Width;
                    int y = axVLCPlugin1.ClientRectangle.Height;
                    bmpPD = new Bitmap(x, y);
                    Graphics gfxScreenshot = Graphics.FromImage(bmpPD);
                    System.Drawing.Size imgSize = new System.Drawing.Size(x, y);
                    Point ps;
                    axVLCPlugin1.Invoke(new MethodInvoker(delegate
                    {
                        ps = PointToScreen(new Point(axVLCPlugin1.Bounds.X, axVLCPlugin1.Bounds.Y));
                        gfxScreenshot.CopyFromScreen(ps.X + groupBox1.Location.X, ps.Y + groupBox1.Location.Y, 0, 0, imgSize, CopyPixelOperation.SourceCopy);
                    }));
                    x = axVLCPlugin2.ClientRectangle.Width;
                    y = axVLCPlugin2.ClientRectangle.Height;
                    bmpPL = new Bitmap(x, y);
                    gfxScreenshot = Graphics.FromImage(bmpPL);
                    imgSize = new System.Drawing.Size(x, y);
                    axVLCPlugin2.Invoke(new MethodInvoker(delegate
                    {
                        ps = PointToScreen(new Point(axVLCPlugin2.Bounds.X, axVLCPlugin2.Bounds.Y));
                        gfxScreenshot.CopyFromScreen(ps.X + groupBox1.Location.X, ps.Y + groupBox1.Location.Y, 0, 0, imgSize, CopyPixelOperation.SourceCopy);
                    }));
                    pic1.Image = bmpPD;
                    pic2.Image = bmpPL;
                }
            }
            catch (Exception e)
            {
            }
        }

        public void takePhotoFore2()
        {
            try
            {
                if (!chDahua.Checked)
                {
                    int x = axVLCPlugin3.ClientRectangle.Width;
                    int y = axVLCPlugin3.ClientRectangle.Height;
                    bmpPD2 = new Bitmap(x, y);
                    Graphics gfxScreenshot = Graphics.FromImage(bmpPD2);
                    System.Drawing.Size imgSize = new System.Drawing.Size(x, y);
                    Point ps;
                    axVLCPlugin3.Invoke(new MethodInvoker(delegate
                    {
                        ps = PointToScreen(new Point(axVLCPlugin3.Bounds.X, axVLCPlugin3.Bounds.Y));
                        gfxScreenshot.CopyFromScreen(ps.X + groupBox2.Location.X, ps.Y + groupBox2.Location.Y, 0, 0, imgSize, CopyPixelOperation.SourceCopy);
                    }));
                    x = axVLCPlugin4.ClientRectangle.Width;
                    y = axVLCPlugin4.ClientRectangle.Height;
                    bmpPL2 = new Bitmap(x, y);
                    gfxScreenshot = Graphics.FromImage(bmpPL2);
                    imgSize = new System.Drawing.Size(x, y);
                    axVLCPlugin4.Invoke(new MethodInvoker(delegate
                    {
                        ps = PointToScreen(new Point(axVLCPlugin4.Bounds.X, axVLCPlugin4.Bounds.Y));
                        gfxScreenshot.CopyFromScreen(ps.X + groupBox2.Location.X, ps.Y + groupBox2.Location.Y, 0, 0, imgSize, CopyPixelOperation.SourceCopy);
                    }));
                    pic3.Image = bmpPD2;
                    pic4.Image = bmpPL2;
                }
            }
            catch (Exception e)
            {
            }
        }

        string tmpFileD = "", tmpFileL = "";
        public void SaveImage(string strMode, string bs, string io)
        {
            if (!chDahua.Checked)
            {
                if (io == "IN")
                {
                    if (bs == "server") SaveImage(bmpPD, bmpPL, txtServerDir.Text, strMode);
                    if (bs == "backup") SaveImage(bmpPD, bmpPL, txtBackupDir.Text, strMode);
                }
                else
                {
                    if (bs == "server") SaveImage(bmpPD2, bmpPL2, txtServerDir.Text, strMode);
                    if (bs == "backup") SaveImage(bmpPD2, bmpPL2, txtBackupDir.Text, strMode);
                }
            }
            else {
                string strDir = "";
                if (bs == "server") strDir = txtServerDir.Text;
                if (bs == "backup") strDir = txtBackupDir.Text;
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

                if (bs == "server")
                {
                    if (io == "IN")
                        dahua.capture(strFileD, strFileL,"","");
                    else dahua.capture("","",strFileD, strFileL);
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
                        if (io == "IN") pic1.Image = bD;
                        else pic3.Image = bD;
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
                        if (io == "IN") pic2.Image = bL;
                        else pic4.Image = bL;
                    }
                    catch (Exception) { }
                }
            }
        }

        public void SaveImage(Bitmap bmpPD, Bitmap bmpPL, string strDir, string strMode)
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
                    if ((chCam1.Checked && strMode == "MI") || (chCam3.Checked && strMode == "MO"))
                    {
                        Bitmap varBmp = new Bitmap(bmpPD);
                        Bitmap newBitmap = new Bitmap(varBmp);
                        varBmp.Save(strFileD, ImageFormat.Jpeg);
                        varBmp.Dispose();
                        varBmp = null;
                    }
                    else
                    {
                        strFileD = "NULL";
                    }
                }
                catch (Exception) { }

                try
                {
                    strFileL += "\\" + strMode + "L" + strFile;
                    if ((chCam2.Checked && strMode == "MI") || (chCam4.Checked && strMode == "MO"))
                    {
                        Bitmap varBmpL = new Bitmap(bmpPL);
                        Bitmap newBitmapL = new Bitmap(varBmpL);
                        varBmpL.Save(strFileL, ImageFormat.Jpeg);
                        varBmpL.Dispose();
                        varBmpL = null;
                    }
                    else
                    {
                        strFileL = "NULL";
                    }
                }
                catch (Exception)
                {
                }
            }
            catch (Exception e)
            {
            }
        }


        public static bool CarInRecord(string intID, string strCarType, string strLicense, string strRandKey, string strImageD, string strImageL)// Bitmap bmL, Bitmap bmD
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

        public bool saveMember(string cardid)
        {
            /*
                Get Cartype Member
             * 
             */
            member.LoadData(Convert.ToUInt32(cardid), false);
            if (CarInRecordMember(cardid,member.TypeID.ToString(), "NO", "", strFileD, strFileL, ""))
            {
                string sql = "UPDATE card" + typeCard + " SET no =" + RecordNo2.ToString();
                sql += " WHERE name=" + cardid;
                db.SaveData(sql);
                return true;
            }
            else
            {
                MessageBox.Show("บันทึกไม่สำเร็จ!");
                return false;
            }
        }



        public void saveMemberBackup(string cardid)
        {
            CarInRecordBMember(cardid, "200", "NO", "", strFileD, strFileL, txtBackupDir.Text, "");
        }

        public bool checkCard(string cardid)
        {
            bool result = false;
            try
            {

                string sql = "";
                //Check Card Prox
                sql = "SELECT * FROM cardpx ";
                sql += " WHERE name=" + cardid;
                DataTable dt = db.LoadData(sql);
                CardLevel = 0;
                RecordNo = 0;
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

        public int getCardLevel() { return CardLevel; }

        public int getRecordNo2() { return RecordNo; }

        public string getTypeCard() { return typeCard; }

        public bool MemberOut(string cardid)
        {
            bool result = false;
            if (CarOutRecord(getRecordNo2(), "0", "", "0", "0", strFileD, strFileL, 0, 0, "", ""))
            {
                string sql = "UPDATE card" + typeCard + " SET no = 0";
                sql += " WHERE name=" + cardid;
                db.SaveData(sql);
                result = true;
            }
            return result;
        }

        public void MemberOutB()
        {
            CarOutRecordB(getRecordNo2(), "0", "", "0", "0", strFileD, strFileL, 0, 0, 0, txtBackupDir.Text, "", "");
        }

        public static bool CarOutRecord(int recordno, string srtPrice, string strDtocar, string srtDiscount, string srtProID, string strImageD, string strImageL, int PriceCardLoss, int PriceOverDate, string GuardHouse, string PosID) //Mac 2015/01/26
        {

            //intPriceCardLoss = 0;
            //intPriceOverDate = 0;

            bool result = false;
            strImageL = strImageL.Replace("\\", "\\\\");
            strImageD = strImageD.Replace("\\", "\\\\");

            string sql = "INSERT INTO recordout (no,picdiv,piclic,dateout,proid,price,discount,userout,userno,losscard,overdate) VALUES(";//
            if (GuardHouse.Trim().Length > 0) //Mac 2015/01/26
                sql = "INSERT INTO recordout (no,picdiv,piclic,dateout,proid,price,discount,userout,userno,losscard,overdate,guardhouse) VALUES(";
            if (PosID.Trim().Length > 0) //Mac 2015/02/26
            {
                sql = "INSERT INTO recordout (no,picdiv,piclic,dateout,proid,price,discount,userout,userno,losscard,overdate,posid) VALUES(";
                if (GuardHouse.Trim().Length > 0)
                    sql = "INSERT INTO recordout (no,picdiv,piclic,dateout,proid,price,discount,userout,userno,losscard,overdate,guardhouse,posid) VALUES(";
            }

            //sql += recordno.ToString() + ",'" + strImageD + "','" + strImageL + "',NOW()," + srtProID + "," + srtPrice + "," + srtDiscount + "," + user.ID + "," + user.WorkID.ToString() + "," + PriceCardLoss.ToString() + "," + PriceOverDate.ToString() + ")";
            if (PosID.Trim().Length > 0) //Mac 2015/02/26
            {
                if (GuardHouse.Trim().Length > 0) //Mac 2015/01/26
                    sql += recordno.ToString() + ",'" + strImageD + "','" + strImageL + "',NOW()," + srtProID + "," + srtPrice + "," + srtDiscount + ",1,0," + PriceCardLoss.ToString() + "," + PriceOverDate.ToString() + ",'" + GuardHouse + "','" + PosID + "')"; //Mac 2015/01/26
                else
                    sql += recordno.ToString() + ",'" + strImageD + "','" + strImageL + "',NOW()," + srtProID + "," + srtPrice + "," + srtDiscount + ",1,0," + PriceCardLoss.ToString() + "," + PriceOverDate.ToString() + ",'" + PosID + "')"; //Mac 2014/08/08
            }
            else
            {
                if (GuardHouse.Trim().Length > 0) //Mac 2015/01/26
                    sql += recordno.ToString() + ",'" + strImageD + "','" + strImageL + "',NOW()," + srtProID + "," + srtPrice + "," + srtDiscount + ",1,0," + PriceCardLoss.ToString() + "," + PriceOverDate.ToString() + ",'" + GuardHouse + "')"; //Mac 2015/01/26
                else
                    sql += recordno.ToString() + ",'" + strImageD + "','" + strImageL + "',NOW()," + srtProID + "," + srtPrice + "," + srtDiscount + ",0,0," + PriceCardLoss.ToString() + "," + PriceOverDate.ToString() + ")"; //Mac 2014/08/08
            }
            if (db.SaveData(sql) == "")
            {
                result = true;
            }

            return result;
        }

        public static void CarOutRecordB(int recordno, string srtPrice, string strDtocar, string srtDiscount, string srtProID, string strImageD, string strImageL, int intPrintno, int PriceCardLoss, int PriceOverDate, string BackupDirectory, string GuardHouse, string PosID) //Mac 2015/01/26
        {
            DateTime now = DateTime.Now;
            string folder = now.Month.ToString();
            string strFile = BackupDirectory + "\\" + folder;
            if (!Directory.Exists(strFile))
            {
                Directory.CreateDirectory(strFile);
            }

            strFile = strFile + "\\OutBackup_" + now.ToString("ddMMyyyy") + ".csv";
            //string strHeader = "sql,no,picdiv,piclic,dateout,proid,price,discount,userout,userno,losscard,overdate"; 
            string strHeader = "sql,no,picdiv,piclic,dateout,proid,price,discount,userout,userno,printno,losscard,overdate"; //Mac 2014/09/18
            if (GuardHouse.Trim().Length > 0) //Mac 2015/01/26
                strHeader = "sql,no,picdiv,piclic,dateout,proid,price,discount,userout,userno,printno,losscard,overdate,guardhouse"; //Mac 2015/01/26
            if (PosID.Trim().Length > 0) //Mac 2015/02/26
            {
                strHeader = "sql,no,picdiv,piclic,dateout,proid,price,discount,userout,userno,printno,losscard,overdate,posid";
                if (GuardHouse.Trim().Length > 0) //Mac 2015/01/26
                    strHeader = "sql,no,picdiv,piclic,dateout,proid,price,discount,userout,userno,printno,losscard,overdate,guardhouse,posid";
            }
            strImageL = strImageL.Replace("\\", "\\\\");
            strImageD = strImageD.Replace("\\", "\\\\");
            string sql = "INSERT INTO recordout VALUES (";//
            //sql += recordno.ToString() + ",'" + strImageD + "','" + strImageL + "',NOW()," + srtProID + "," + srtPrice + "," + srtDiscount + "," + user.ID + "," + user.WorkID + "," + PriceCardLoss + "," + PriceOverDate + ");";
            //sql += recordno.ToString() + ",'" + strImageD + "','" + strImageL + "',NOW()," + srtProID + "," + srtPrice + "," + srtDiscount + "," + user.ID + "," + user.WorkID + "," + PriceCardLoss + "," + PriceOverDate + ");"; //Mac 2014/08/08
            if (PosID.Trim().Length > 0) //Mac 2015/02/26
            {
                if (GuardHouse.Trim().Length > 0) //Mac 2015/01/26
                    sql += recordno.ToString() + ",'" + strImageD + "','" + strImageL + "','" + now.ToString("yyyy-MM-dd HH:mm:ss") + "'," + srtProID + "," + srtPrice + "," + srtDiscount + ",1,0," + intPrintno + "," + PriceCardLoss + "," + PriceOverDate + ",'" + GuardHouse + "','" + PosID + "');"; //Mac 2014/11/08
                else
                    sql += recordno.ToString() + ",'" + strImageD + "','" + strImageL + "','" + now.ToString("yyyy-MM-dd HH:mm:ss") + "'," + srtProID + "," + srtPrice + "," + srtDiscount + ",1,0," + intPrintno + "," + PriceCardLoss + "," + PriceOverDate + ",'" + PosID + "');"; //Mac 2014/11/08
            }
            else
            {
                if (GuardHouse.Trim().Length > 0) //Mac 2015/01/26
                    sql += recordno.ToString() + ",'" + strImageD + "','" + strImageL + "','" + now.ToString("yyyy-MM-dd HH:mm:ss") + "'," + srtProID + "," + srtPrice + "," + srtDiscount + ",1,0," + intPrintno + "," + PriceCardLoss + "," + PriceOverDate + ",'" + GuardHouse + "');"; //Mac 2014/11/08
                else
                    sql += recordno.ToString() + ",'" + strImageD + "','" + strImageL + "','" + now.ToString("yyyy-MM-dd HH:mm:ss") + "'," + srtProID + "," + srtPrice + "," + srtDiscount + ",1,0," + intPrintno + "," + PriceCardLoss + "," + PriceOverDate + ");"; //Mac 2014/11/08
            }
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

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            loadDahua();
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            lbLogin.Visible = false;
            // MessageBox.Show(this, "Ready", "Camera Init", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
        }

        private void chDahua_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chDahua_Click(object sender, EventArgs e)
        {
            if (chDahua.Checked)
            {
                if (txtControlIP1.Text == txtControlIP2.Text)
                {
                    MessageBox.Show(this, "Please difference IP controllor!", "Waring!", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
                    return;
                }

                string createText;
                createText = "IPCAM1;" + txtCamIP1.Text + Environment.NewLine;
                createText += "IPCAM2;" + txtCamIP2.Text + Environment.NewLine;
                createText += "IPCAM3;" + txtCamIP3.Text + Environment.NewLine;
                createText += "IPCAM4;" + txtCamIP4.Text + Environment.NewLine;
                //CAM1ENABLE
                createText += "CAM1ENABLE;" + chCam1.Checked.ToString() + Environment.NewLine;
                createText += "CAM2ENABLE;" + chCam2.Checked.ToString() + Environment.NewLine;
                createText += "CAM3ENABLE;" + chCam3.Checked.ToString() + Environment.NewLine;
                createText += "CAM4ENABLE;" + chCam4.Checked.ToString() + Environment.NewLine;

                createText += "SERDIR;" + txtServerDir.Text + Environment.NewLine;
                createText += "BACDIR;" + txtBackupDir.Text + Environment.NewLine;
                createText += "CONIP1;" + txtControlIP1.Text + Environment.NewLine;
                createText += "CONIP2;" + txtControlIP2.Text + Environment.NewLine;
                createText += "BASEIP;" + txtDBIP.Text + Environment.NewLine;
                createText += "BASENAME;" + txtDBName.Text + Environment.NewLine;
                createText += "USEDAHUA;" + chDahua.Checked.ToString() + Environment.NewLine;

                File.WriteAllText("config.txt", createText);
                Thread.Sleep(1500);
                MessageBox.Show("Restart program to load config!", "Saved!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
            }
        }

        //public static void CardOutMember(string cardid) {
        //    return CarOutRecord(recordno, string srtPrice, string strDtocar, string srtDiscount, string srtProID, string strImageD, string strImageL, int PriceCardLoss, int PriceOverDate, string GuardHouse, string PosID);
        //}
    }
}
