namespace MonitorDispenser
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
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
            this.lbLocal = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.mynotifyicon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picEmpty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picReadOk)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGreen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLoop)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
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
            this.groupBox3.Location = new System.Drawing.Point(10, 39);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(279, 175);
            this.groupBox3.TabIndex = 32;
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
            // lbLocal
            // 
            this.lbLocal.AutoSize = true;
            this.lbLocal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lbLocal.Location = new System.Drawing.Point(24, 13);
            this.lbLocal.Name = "lbLocal";
            this.lbLocal.Size = new System.Drawing.Size(137, 13);
            this.lbLocal.TabIndex = 33;
            this.lbLocal.Text = "RECEIVE STATUS IP :";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // mynotifyicon
            // 
            this.mynotifyicon.ContextMenuStrip = this.contextMenuStrip1;
            this.mynotifyicon.Icon = ((System.Drawing.Icon)(resources.GetObject("mynotifyicon.Icon")));
            this.mynotifyicon.Text = "notifyIcon1";
            this.mynotifyicon.Visible = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showToolStripMenuItem,
            this.hideToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(153, 70);
            // 
            // showToolStripMenuItem
            // 
            this.showToolStripMenuItem.Name = "showToolStripMenuItem";
            this.showToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.showToolStripMenuItem.Text = "&Show";
            this.showToolStripMenuItem.Click += new System.EventHandler(this.showToolStripMenuItem_Click);
            // 
            // hideToolStripMenuItem
            // 
            this.hideToolStripMenuItem.Name = "hideToolStripMenuItem";
            this.hideToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.hideToolStripMenuItem.Text = "&Hide";
            this.hideToolStripMenuItem.Click += new System.EventHandler(this.hideToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(301, 229);
            this.Controls.Add(this.lbLocal);
            this.Controls.Add(this.groupBox3);
            this.Name = "Form1";
            this.Text = "Monitor";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picEmpty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picReadOk)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGreen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLoop)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
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
        private System.Windows.Forms.Label lbLocal;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.NotifyIcon mynotifyicon;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem showToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hideToolStripMenuItem;
    }
}

