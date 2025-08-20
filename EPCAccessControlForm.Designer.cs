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
            this.txtDeviceName = new System.Windows.Forms.TextBox();
            this.lblDeviceName = new System.Windows.Forms.Label();
            this.cmbEntryStatus = new System.Windows.Forms.ComboBox();
            this.lblEntryStatus = new System.Windows.Forms.Label();
            this.cmbExitStatus = new System.Windows.Forms.ComboBox();
            this.lblExitStatus = new System.Windows.Forms.Label();
            this.btnRefreshStatus = new System.Windows.Forms.Button();
            this.lblServerStatus = new System.Windows.Forms.Label();
            this.btnClearPending = new System.Windows.Forms.Button();
            this.btnTestAssembleId = new System.Windows.Forms.Button();
            this.btnTestConnection = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(660, 300);
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
            // txtDeviceName
            // 
            this.txtDeviceName.Location = new System.Drawing.Point(12, 450);
            this.txtDeviceName.Name = "txtDeviceName";
            this.txtDeviceName.Size = new System.Drawing.Size(120, 20);
            this.txtDeviceName.TabIndex = 15;
            this.txtDeviceName.Text = "PDA_001";
            // 
            // lblDeviceName
            // 
            this.lblDeviceName.AutoSize = true;
            this.lblDeviceName.Location = new System.Drawing.Point(12, 430);
            this.lblDeviceName.Name = "lblDeviceName";
            this.lblDeviceName.Size = new System.Drawing.Size(55, 13);
            this.lblDeviceName.TabIndex = 16;
            this.lblDeviceName.Text = "设备名称";
            // 
            // cmbEntryStatus
            // 
            this.cmbEntryStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEntryStatus.FormattingEnabled = true;
            this.cmbEntryStatus.Location = new System.Drawing.Point(150, 450);
            this.cmbEntryStatus.Name = "cmbEntryStatus";
            this.cmbEntryStatus.Size = new System.Drawing.Size(120, 21);
            this.cmbEntryStatus.TabIndex = 17;
            // 
            // lblEntryStatus
            // 
            this.lblEntryStatus.AutoSize = true;
            this.lblEntryStatus.Location = new System.Drawing.Point(150, 430);
            this.lblEntryStatus.Name = "lblEntryStatus";
            this.lblEntryStatus.Size = new System.Drawing.Size(67, 13);
            this.lblEntryStatus.TabIndex = 18;
            this.lblEntryStatus.Text = "进入时状态";
            // 
            // cmbExitStatus
            // 
            this.cmbExitStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbExitStatus.FormattingEnabled = true;
            this.cmbExitStatus.Location = new System.Drawing.Point(290, 450);
            this.cmbExitStatus.Name = "cmbExitStatus";
            this.cmbExitStatus.Size = new System.Drawing.Size(120, 21);
            this.cmbExitStatus.TabIndex = 19;
            // 
            // lblExitStatus
            // 
            this.lblExitStatus.AutoSize = true;
            this.lblExitStatus.Location = new System.Drawing.Point(290, 430);
            this.lblExitStatus.Name = "lblExitStatus";
            this.lblExitStatus.Size = new System.Drawing.Size(67, 13);
            this.lblExitStatus.TabIndex = 20;
            this.lblExitStatus.Text = "离开时状态";
            // 
            // btnRefreshStatus
            // 
            this.btnRefreshStatus.Location = new System.Drawing.Point(430, 450);
            this.btnRefreshStatus.Name = "btnRefreshStatus";
            this.btnRefreshStatus.Size = new System.Drawing.Size(90, 23);
            this.btnRefreshStatus.TabIndex = 21;
            this.btnRefreshStatus.Text = "刷新状态配置";
            this.btnRefreshStatus.UseVisualStyleBackColor = true;
            this.btnRefreshStatus.Click += new System.EventHandler(this.btnRefreshStatus_Click);
            // 
            // lblServerStatus
            // 
            this.lblServerStatus.AutoSize = true;
            this.lblServerStatus.Location = new System.Drawing.Point(430, 430);
            this.lblServerStatus.Name = "lblServerStatus";
            this.lblServerStatus.Size = new System.Drawing.Size(67, 13);
            this.lblServerStatus.TabIndex = 22;
            this.lblServerStatus.Text = "服务器状态";
            // 
            // btnClearPending
            // 
            this.btnClearPending.Location = new System.Drawing.Point(430, 480);
            this.btnClearPending.Name = "btnClearPending";
            this.btnClearPending.Size = new System.Drawing.Size(90, 23);
            this.btnClearPending.TabIndex = 23;
            this.btnClearPending.Text = "检测中状态";
            this.btnClearPending.UseVisualStyleBackColor = true;
            this.btnClearPending.Click += new System.EventHandler(this.btnShowPending_Click);
            // 
            // btnTestAssembleId
            // 
            this.btnTestAssembleId.Location = new System.Drawing.Point(530, 450);
            this.btnTestAssembleId.Name = "btnTestAssembleId";
            this.btnTestAssembleId.Size = new System.Drawing.Size(90, 23);
            this.btnTestAssembleId.TabIndex = 24;
            this.btnTestAssembleId.Text = "测试查询ID";
            this.btnTestAssembleId.UseVisualStyleBackColor = true;
            this.btnTestAssembleId.Click += new System.EventHandler(this.btnTestAssembleId_Click);
            // 
            // btnTestConnection
            // 
            this.btnTestConnection.Location = new System.Drawing.Point(530, 480);
            this.btnTestConnection.Name = "btnTestConnection";
            this.btnTestConnection.Size = new System.Drawing.Size(90, 23);
            this.btnTestConnection.TabIndex = 25;
            this.btnTestConnection.Text = "API诊断";
            this.btnTestConnection.UseVisualStyleBackColor = true;
            this.btnTestConnection.Click += new System.EventHandler(this.btnTestConnection_Click);
            // 
            // EPCAccessControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 520);
            this.Controls.Add(this.btnTestConnection);
            this.Controls.Add(this.btnTestAssembleId);
            this.Controls.Add(this.btnClearPending);
            this.Controls.Add(this.lblServerStatus);
            this.Controls.Add(this.btnRefreshStatus);
            this.Controls.Add(this.lblExitStatus);
            this.Controls.Add(this.cmbExitStatus);
            this.Controls.Add(this.lblEntryStatus);
            this.Controls.Add(this.cmbEntryStatus);
            this.Controls.Add(this.lblDeviceName);
            this.Controls.Add(this.txtDeviceName);
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
        private System.Windows.Forms.TextBox txtDeviceName;
        private System.Windows.Forms.Label lblDeviceName;
        private System.Windows.Forms.ComboBox cmbEntryStatus;
        private System.Windows.Forms.Label lblEntryStatus;
        private System.Windows.Forms.ComboBox cmbExitStatus;
        private System.Windows.Forms.Label lblExitStatus;
        private System.Windows.Forms.Button btnRefreshStatus;
        private System.Windows.Forms.Label lblServerStatus;
        private System.Windows.Forms.Button btnClearPending;
        private System.Windows.Forms.Button btnTestAssembleId;
        private System.Windows.Forms.Button btnTestConnection;
    }
}
