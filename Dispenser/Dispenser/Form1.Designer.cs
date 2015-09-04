namespace Dispenser
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txtSendData = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.btnEdit = new System.Windows.Forms.Button();
            this.cmbReaderPort = new System.Windows.Forms.ComboBox();
            this.cmbLEDPort = new System.Windows.Forms.ComboBox();
            this.cmbControlPort = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtDatabaseIP = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtBackupDir = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtServerDir = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label15 = new System.Windows.Forms.Label();
            this.picEmpty = new System.Windows.Forms.PictureBox();
            this.label14 = new System.Windows.Forms.Label();
            this.picLow = new System.Windows.Forms.PictureBox();
            this.label13 = new System.Windows.Forms.Label();
            this.picReadOk = new System.Windows.Forms.PictureBox();
            this.label11 = new System.Windows.Forms.Label();
            this.picGreen = new System.Windows.Forms.PictureBox();
            this.label10 = new System.Windows.Forms.Label();
            this.picRed = new System.Windows.Forms.PictureBox();
            this.label9 = new System.Windows.Forms.Label();
            this.picLoop = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.picCam2 = new System.Windows.Forms.PictureBox();
            this.rdFore = new System.Windows.Forms.RadioButton();
            this.picCam1 = new System.Windows.Forms.PictureBox();
            this.rdBack = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.axVLCPlugin1 = new AxAXVLC.AxVLCPlugin();
            this.label12 = new System.Windows.Forms.Label();
            this.axVLCPlugin2 = new AxAXVLC.AxVLCPlugin();
            this.txtCam2 = new System.Windows.Forms.TextBox();
            this.txtCam1 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picEmpty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picReadOk)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGreen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLoop)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCam2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCam1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axVLCPlugin1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axVLCPlugin2)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.txtSendData);
            this.groupBox4.Controls.Add(this.label16);
            this.groupBox4.Controls.Add(this.btnEdit);
            this.groupBox4.Controls.Add(this.cmbReaderPort);
            this.groupBox4.Controls.Add(this.cmbLEDPort);
            this.groupBox4.Controls.Add(this.cmbControlPort);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.txtDatabaseIP);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.txtBackupDir);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.txtServerDir);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Location = new System.Drawing.Point(717, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(279, 380);
            this.groupBox4.TabIndex = 32;
            this.groupBox4.TabStop = false;
            // 
            // txtSendData
            // 
            this.txtSendData.Location = new System.Drawing.Point(143, 100);
            this.txtSendData.Name = "txtSendData";
            this.txtSendData.Size = new System.Drawing.Size(126, 20);
            this.txtSendData.TabIndex = 18;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(34, 103);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(102, 13);
            this.label16.TabIndex = 17;
            this.label16.Text = "SEND STATUS IP :";
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(189, 341);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 16;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // cmbReaderPort
            // 
            this.cmbReaderPort.FormattingEnabled = true;
            this.cmbReaderPort.Location = new System.Drawing.Point(142, 179);
            this.cmbReaderPort.Name = "cmbReaderPort";
            this.cmbReaderPort.Size = new System.Drawing.Size(121, 21);
            this.cmbReaderPort.TabIndex = 15;
            // 
            // cmbLEDPort
            // 
            this.cmbLEDPort.FormattingEnabled = true;
            this.cmbLEDPort.Location = new System.Drawing.Point(143, 152);
            this.cmbLEDPort.Name = "cmbLEDPort";
            this.cmbLEDPort.Size = new System.Drawing.Size(121, 21);
            this.cmbLEDPort.TabIndex = 14;
            // 
            // cmbControlPort
            // 
            this.cmbControlPort.FormattingEnabled = true;
            this.cmbControlPort.Location = new System.Drawing.Point(143, 125);
            this.cmbControlPort.Name = "cmbControlPort";
            this.cmbControlPort.Size = new System.Drawing.Size(121, 21);
            this.cmbControlPort.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(45, 182);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(91, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "READER PORT :";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(69, 155);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "LED PORT :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 128);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(120, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "CONTROLLOR PORT :";
            // 
            // txtDatabaseIP
            // 
            this.txtDatabaseIP.Location = new System.Drawing.Point(142, 74);
            this.txtDatabaseIP.Name = "txtDatabaseIP";
            this.txtDatabaseIP.Size = new System.Drawing.Size(126, 20);
            this.txtDatabaseIP.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(53, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "DATABASE IP :";
            // 
            // txtBackupDir
            // 
            this.txtBackupDir.Location = new System.Drawing.Point(143, 48);
            this.txtBackupDir.Name = "txtBackupDir";
            this.txtBackupDir.Size = new System.Drawing.Size(125, 20);
            this.txtBackupDir.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "BACKUP DIRECTORY :";
            // 
            // txtServerDir
            // 
            this.txtServerDir.Location = new System.Drawing.Point(143, 22);
            this.txtServerDir.Name = "txtServerDir";
            this.txtServerDir.Size = new System.Drawing.Size(125, 20);
            this.txtServerDir.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "SERVER DIRECTORY :";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.picEmpty);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.picLow);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.picReadOk);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.picGreen);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.picRed);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.picLoop);
            this.groupBox3.Location = new System.Drawing.Point(717, 409);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(279, 175);
            this.groupBox3.TabIndex = 31;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "STATUS";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(45, 153);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(36, 13);
            this.label15.TabIndex = 11;
            this.label15.Text = "Empty";
            // 
            // picEmpty
            // 
            this.picEmpty.Image = ((System.Drawing.Image)(resources.GetObject("picEmpty.Image")));
            this.picEmpty.Location = new System.Drawing.Point(17, 149);
            this.picEmpty.Name = "picEmpty";
            this.picEmpty.Size = new System.Drawing.Size(22, 22);
            this.picEmpty.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picEmpty.TabIndex = 10;
            this.picEmpty.TabStop = false;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(45, 127);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(27, 13);
            this.label14.TabIndex = 9;
            this.label14.Text = "Low";
            // 
            // picLow
            // 
            this.picLow.Image = ((System.Drawing.Image)(resources.GetObject("picLow.Image")));
            this.picLow.Location = new System.Drawing.Point(17, 123);
            this.picLow.Name = "picLow";
            this.picLow.Size = new System.Drawing.Size(22, 22);
            this.picLow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picLow.TabIndex = 8;
            this.picLow.TabStop = false;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(45, 101);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(48, 13);
            this.label13.TabIndex = 7;
            this.label13.Text = "ReadOK";
            // 
            // picReadOk
            // 
            this.picReadOk.Image = ((System.Drawing.Image)(resources.GetObject("picReadOk.Image")));
            this.picReadOk.Location = new System.Drawing.Point(17, 97);
            this.picReadOk.Name = "picReadOk";
            this.picReadOk.Size = new System.Drawing.Size(22, 22);
            this.picReadOk.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picReadOk.TabIndex = 6;
            this.picReadOk.TabStop = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(45, 75);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(67, 13);
            this.label11.TabIndex = 5;
            this.label11.Text = "GreenButton";
            // 
            // picGreen
            // 
            this.picGreen.Image = ((System.Drawing.Image)(resources.GetObject("picGreen.Image")));
            this.picGreen.Location = new System.Drawing.Point(17, 71);
            this.picGreen.Name = "picGreen";
            this.picGreen.Size = new System.Drawing.Size(22, 22);
            this.picGreen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picGreen.TabIndex = 4;
            this.picGreen.TabStop = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(45, 49);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(58, 13);
            this.label10.TabIndex = 3;
            this.label10.Text = "RedButton";
            // 
            // picRed
            // 
            this.picRed.Image = ((System.Drawing.Image)(resources.GetObject("picRed.Image")));
            this.picRed.Location = new System.Drawing.Point(17, 45);
            this.picRed.Name = "picRed";
            this.picRed.Size = new System.Drawing.Size(22, 22);
            this.picRed.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picRed.TabIndex = 2;
            this.picRed.TabStop = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(45, 23);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(31, 13);
            this.label9.TabIndex = 1;
            this.label9.Text = "Loop";
            // 
            // picLoop
            // 
            this.picLoop.Image = ((System.Drawing.Image)(resources.GetObject("picLoop.Image")));
            this.picLoop.Location = new System.Drawing.Point(17, 19);
            this.picLoop.Name = "picLoop";
            this.picLoop.Size = new System.Drawing.Size(22, 22);
            this.picLoop.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picLoop.TabIndex = 0;
            this.picLoop.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.progressBar1);
            this.groupBox1.Controls.Add(this.picCam2);
            this.groupBox1.Controls.Add(this.rdFore);
            this.groupBox1.Controls.Add(this.picCam1);
            this.groupBox1.Controls.Add(this.rdBack);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.axVLCPlugin1);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.axVLCPlugin2);
            this.groupBox1.Controls.Add(this.txtCam2);
            this.groupBox1.Controls.Add(this.txtCam1);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(699, 572);
            this.groupBox1.TabIndex = 29;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "CAMERA";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(371, 301);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(301, 13);
            this.progressBar1.TabIndex = 28;
            this.progressBar1.Visible = false;
            // 
            // picCam2
            // 
            this.picCam2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.picCam2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picCam2.Location = new System.Drawing.Point(352, 320);
            this.picCam2.Name = "picCam2";
            this.picCam2.Size = new System.Drawing.Size(320, 240);
            this.picCam2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picCam2.TabIndex = 2;
            this.picCam2.TabStop = false;
            // 
            // rdFore
            // 
            this.rdFore.AutoSize = true;
            this.rdFore.Location = new System.Drawing.Point(263, 297);
            this.rdFore.Name = "rdFore";
            this.rdFore.Size = new System.Drawing.Size(102, 17);
            this.rdFore.TabIndex = 27;
            this.rdFore.TabStop = true;
            this.rdFore.Text = "FOREGROUND";
            this.rdFore.UseVisualStyleBackColor = true;
            // 
            // picCam1
            // 
            this.picCam1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.picCam1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picCam1.Location = new System.Drawing.Point(17, 320);
            this.picCam1.Name = "picCam1";
            this.picCam1.Size = new System.Drawing.Size(320, 240);
            this.picCam1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picCam1.TabIndex = 1;
            this.picCam1.TabStop = false;
            // 
            // rdBack
            // 
            this.rdBack.AutoSize = true;
            this.rdBack.Location = new System.Drawing.Point(143, 297);
            this.rdBack.Name = "rdBack";
            this.rdBack.Size = new System.Drawing.Size(101, 17);
            this.rdBack.TabIndex = 26;
            this.rdBack.TabStop = true;
            this.rdBack.Text = "BACKGROUND";
            this.rdBack.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 297);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(102, 13);
            this.label5.TabIndex = 25;
            this.label5.Text = "CAPTURE MODE : ";
            // 
            // axVLCPlugin1
            // 
            this.axVLCPlugin1.Enabled = true;
            this.axVLCPlugin1.Location = new System.Drawing.Point(17, 19);
            this.axVLCPlugin1.Name = "axVLCPlugin1";
            this.axVLCPlugin1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axVLCPlugin1.OcxState")));
            this.axVLCPlugin1.Size = new System.Drawing.Size(320, 240);
            this.axVLCPlugin1.TabIndex = 19;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(349, 268);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(71, 13);
            this.label12.TabIndex = 24;
            this.label12.Text = "IP CAMERA2";
            // 
            // axVLCPlugin2
            // 
            this.axVLCPlugin2.Enabled = true;
            this.axVLCPlugin2.Location = new System.Drawing.Point(352, 19);
            this.axVLCPlugin2.Name = "axVLCPlugin2";
            this.axVLCPlugin2.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axVLCPlugin2.OcxState")));
            this.axVLCPlugin2.Size = new System.Drawing.Size(320, 240);
            this.axVLCPlugin2.TabIndex = 20;
            // 
            // txtCam2
            // 
            this.txtCam2.Location = new System.Drawing.Point(426, 265);
            this.txtCam2.Name = "txtCam2";
            this.txtCam2.Size = new System.Drawing.Size(200, 20);
            this.txtCam2.TabIndex = 23;
            // 
            // txtCam1
            // 
            this.txtCam1.Location = new System.Drawing.Point(91, 265);
            this.txtCam1.Name = "txtCam1";
            this.txtCam1.Size = new System.Drawing.Size(200, 20);
            this.txtCam1.TabIndex = 21;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(14, 268);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(71, 13);
            this.label8.TabIndex = 22;
            this.label8.Text = "IP CAMERA1";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 598);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DispenserControl";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picEmpty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picReadOk)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGreen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLoop)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCam2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCam1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axVLCPlugin1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axVLCPlugin2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox txtDatabaseIP;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtBackupDir;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtServerDir;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox picCam2;
        private System.Windows.Forms.RadioButton rdFore;
        private System.Windows.Forms.PictureBox picCam1;
        private System.Windows.Forms.RadioButton rdBack;
        private System.Windows.Forms.Label label5;
        private AxAXVLC.AxVLCPlugin axVLCPlugin1;
        private System.Windows.Forms.Label label12;
        private AxAXVLC.AxVLCPlugin axVLCPlugin2;
        private System.Windows.Forms.TextBox txtCam2;
        private System.Windows.Forms.TextBox txtCam1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmbReaderPort;
        private System.Windows.Forms.ComboBox cmbLEDPort;
        private System.Windows.Forms.ComboBox cmbControlPort;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.PictureBox picEmpty;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.PictureBox picLow;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.PictureBox picReadOk;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.PictureBox picGreen;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.PictureBox picRed;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.PictureBox picLoop;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.TextBox txtSendData;
        private System.Windows.Forms.Label label16;

    }
}

