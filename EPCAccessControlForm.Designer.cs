namespace UHFReader288Demo
{
    partial class EPCAccessControlForm
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnResetEPC = new System.Windows.Forms.Button();
            this.lblTotalEntry = new System.Windows.Forms.Label();
            this.lblTotalExit = new System.Windows.Forms.Label();
            this.lblCurrentInside = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblAntenna1 = new System.Windows.Forms.Label();
            this.lblAntenna8 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.ComboBox_PowerDbm = new System.Windows.Forms.ComboBox();
            this.BT_DBM = new System.Windows.Forms.Button();
            this.lblPower = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(560, 300);
            this.dataGridView1.TabIndex = 0;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(12, 330);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "开始监控";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(93, 330);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 2;
            this.btnStop.Text = "停止监控";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(174, 330);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "清空记录";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(255, 330);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 4;
            this.btnExport.Text = "导出记录";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnResetEPC
            // 
            this.btnResetEPC.Location = new System.Drawing.Point(336, 330);
            this.btnResetEPC.Name = "btnResetEPC";
            this.btnResetEPC.Size = new System.Drawing.Size(75, 23);
            this.btnResetEPC.TabIndex = 5;
            this.btnResetEPC.Text = "重置EPC";
            this.btnResetEPC.UseVisualStyleBackColor = true;
            this.btnResetEPC.Click += new System.EventHandler(this.btnResetEPC_Click);
            // 
            // lblTotalEntry
            // 
            this.lblTotalEntry.AutoSize = true;
            this.lblTotalEntry.Location = new System.Drawing.Point(12, 370);
            this.lblTotalEntry.Name = "lblTotalEntry";
            this.lblTotalEntry.Size = new System.Drawing.Size(53, 13);
            this.lblTotalEntry.TabIndex = 6;
            this.lblTotalEntry.Text = "总进入: 0";
            // 
            // lblTotalExit
            // 
            this.lblTotalExit.AutoSize = true;
            this.lblTotalExit.Location = new System.Drawing.Point(120, 370);
            this.lblTotalExit.Name = "lblTotalExit";
            this.lblTotalExit.Size = new System.Drawing.Size(53, 13);
            this.lblTotalExit.TabIndex = 7;
            this.lblTotalExit.Text = "总离开: 0";
            // 
            // lblCurrentInside
            // 
            this.lblCurrentInside.AutoSize = true;
            this.lblCurrentInside.Location = new System.Drawing.Point(228, 370);
            this.lblCurrentInside.Name = "lblCurrentInside";
            this.lblCurrentInside.Size = new System.Drawing.Size(65, 13);
            this.lblCurrentInside.TabIndex = 8;
            this.lblCurrentInside.Text = "当前在场: 0";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(12, 400);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(91, 13);
            this.lblStatus.TabIndex = 9;
            this.lblStatus.Text = "监控状态: 已停止";
            // 
            // lblAntenna1
            // 
            this.lblAntenna1.AutoSize = true;
            this.lblAntenna1.Location = new System.Drawing.Point(120, 400);
            this.lblAntenna1.Name = "lblAntenna1";
            this.lblAntenna1.Size = new System.Drawing.Size(53, 13);
            this.lblAntenna1.TabIndex = 10;
            this.lblAntenna1.Text = "天线1: 离线";
            // 
            // lblAntenna8
            // 
            this.lblAntenna8.AutoSize = true;
            this.lblAntenna8.Location = new System.Drawing.Point(228, 400);
            this.lblAntenna8.Name = "lblAntenna8";
            this.lblAntenna8.Size = new System.Drawing.Size(53, 13);
            this.lblAntenna8.TabIndex = 11;
            this.lblAntenna8.Text = "天线8: 离线";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // ComboBox_PowerDbm
            // 
            this.ComboBox_PowerDbm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBox_PowerDbm.FormattingEnabled = true;
            this.ComboBox_PowerDbm.Items.AddRange(new object[] {
            "0 dBm",
            "1 dBm",
            "2 dBm",
            "3 dBm",
            "4 dBm",
            "5 dBm",
            "6 dBm",
            "7 dBm",
            "8 dBm",
            "9 dBm",
            "10 dBm",
            "11 dBm",
            "12 dBm",
            "13 dBm",
            "14 dBm",
            "15 dBm",
            "16 dBm",
            "17 dBm",
            "18 dBm",
            "19 dBm",
            "20 dBm",
            "21 dBm",
            "22 dBm",
            "23 dBm",
            "24 dBm",
            "25 dBm",
            "26 dBm",
            "27 dBm",
            "28 dBm",
            "29 dBm",
            "30 dBm"});
            this.ComboBox_PowerDbm.Location = new System.Drawing.Point(450, 330);
            this.ComboBox_PowerDbm.Name = "ComboBox_PowerDbm";
            this.ComboBox_PowerDbm.Size = new System.Drawing.Size(70, 21);
            this.ComboBox_PowerDbm.TabIndex = 12;
            // 
            // BT_DBM
            // 
            this.BT_DBM.Location = new System.Drawing.Point(450, 360);
            this.BT_DBM.Name = "BT_DBM";
            this.BT_DBM.Size = new System.Drawing.Size(70, 23);
            this.BT_DBM.TabIndex = 13;
            this.BT_DBM.Text = "设置功率";
            this.BT_DBM.UseVisualStyleBackColor = true;
            this.BT_DBM.Click += new System.EventHandler(this.BT_DBM_Click);
            // 
            // lblPower
            // 
            this.lblPower.AutoSize = true;
            this.lblPower.Location = new System.Drawing.Point(450, 310);
            this.lblPower.Name = "lblPower";
            this.lblPower.Size = new System.Drawing.Size(55, 13);
            this.lblPower.TabIndex = 14;
            this.lblPower.Text = "功率设置";
            // 
            // EPCAccessControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 441);
            this.Controls.Add(this.lblPower);
            this.Controls.Add(this.BT_DBM);
            this.Controls.Add(this.ComboBox_PowerDbm);
            this.Controls.Add(this.lblAntenna8);
            this.Controls.Add(this.lblAntenna1);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblCurrentInside);
            this.Controls.Add(this.lblTotalExit);
            this.Controls.Add(this.lblTotalEntry);
            this.Controls.Add(this.btnResetEPC);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.dataGridView1);
            this.Name = "EPCAccessControlForm";
            this.Text = "EPC门禁进出管理";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnResetEPC;
        private System.Windows.Forms.Label lblTotalEntry;
        private System.Windows.Forms.Label lblTotalExit;
        private System.Windows.Forms.Label lblCurrentInside;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblAntenna1;
        private System.Windows.Forms.Label lblAntenna8;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ComboBox ComboBox_PowerDbm;
        private System.Windows.Forms.Button BT_DBM;
        private System.Windows.Forms.Label lblPower;
    }
}
