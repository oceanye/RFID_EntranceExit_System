using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

namespace UHFReader288Demo
{
    public partial class EPCAccessControlForm : Form
    {
        // 进出记录类
        private class AccessRecord
        {
            public string EPC { get; set; }
            public DateTime FirstDetectionTime { get; set; }
            public DateTime SecondDetectionTime { get; set; }
            public string Direction { get; set; } // "进入" 或 "离开"
            public bool IsCompleted { get; set; }
            public int FirstAntenna { get; set; }
            public int SecondAntenna { get; set; }
        }

        // 临时检测记录
        private class DetectionRecord
        {
            public string EPC { get; set; }
            public int Antenna { get; set; }
            public DateTime DetectionTime { get; set; }
        }

        private Dictionary<string, AccessRecord> accessRecords = new Dictionary<string, AccessRecord>();
        private Dictionary<string, DetectionRecord> pendingDetections = new Dictionary<string, DetectionRecord>();
        
        private bool isMonitoring = false;
        private int totalEntry = 0;
        private int totalExit = 0;
        private bool antenna1Online = false;
        private bool antenna8Online = false;

        public EPCAccessControlForm()
        {
            InitializeComponent();
            InitializeDataGridView();
            UpdateStats();
            UpdateAntennaStatus();
        }

        private void InitializeDataGridView()
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns.Clear();
            
            dataGridView1.Columns.Add("EPC", "EPC标签");
            dataGridView1.Columns.Add("Direction", "进出方向");
            dataGridView1.Columns.Add("Time", "时间");
            dataGridView1.Columns.Add("Status", "状态");
            
            dataGridView1.Columns[0].DataPropertyName = "EPC";
            dataGridView1.Columns[1].DataPropertyName = "Direction";
            dataGridView1.Columns[2].DataPropertyName = "Time";
            dataGridView1.Columns[3].DataPropertyName = "Status";
            
            dataGridView1.Columns[0].Width = 150;
            dataGridView1.Columns[1].Width = 80;
            dataGridView1.Columns[2].Width = 150;
            dataGridView1.Columns[3].Width = 80;
        }

        // 处理标签检测
        public void ProcessTagDetection(string epc, int antenna, DateTime detectionTime)
        {
            if (!isMonitoring) return;

            this.Invoke(new Action(() =>
            {
                // 只处理天线1和天线8
                if (antenna != 1 && antenna != 8) return;

                // 如果已完成，不再处理
                if (accessRecords.ContainsKey(epc) && accessRecords[epc].IsCompleted)
                    return;

                // 检查是否已有待处理记录
                if (pendingDetections.ContainsKey(epc))
                {
                    var pending = pendingDetections[epc];
                    
                    // 确保是不同天线
                    if (pending.Antenna != antenna)
                    {
                        // 判断进出方向
                        string direction = "";
                        if (pending.Antenna == 1 && antenna == 8)
                        {
                            direction = "进入";
                            totalEntry++;
                        }
                        else if (pending.Antenna == 8 && antenna == 1)
                        {
                            direction = "离开";
                            totalExit++;
                        }

                        if (!string.IsNullOrEmpty(direction))
                        {
                            // 创建完整记录
                            var record = new AccessRecord
                            {
                                EPC = epc,
                                FirstDetectionTime = pending.DetectionTime,
                                SecondDetectionTime = detectionTime,
                                Direction = direction,
                                IsCompleted = true,
                                FirstAntenna = pending.Antenna,
                                SecondAntenna = antenna
                            };

                            accessRecords[epc] = record;
                            pendingDetections.Remove(epc);

                            // 更新UI
                            AddToGrid(epc, direction, detectionTime, "已完成");
                            UpdateStats();
                        }
                    }
                }
                else
                {
                    // 创建新的待处理记录
                    pendingDetections[epc] = new DetectionRecord
                    {
                        EPC = epc,
                        Antenna = antenna,
                        DetectionTime = detectionTime
                    };

                    // 显示待处理状态
                    AddToGrid(epc, "检测中", detectionTime, "天线" + antenna.ToString());
                    
                    // 更新统计信息以显示待处理状态
                    UpdateStats();
                }
            }));
        }

        private void AddToGrid(string epc, string direction, DateTime time, string status)
        {
            int rowIndex = dataGridView1.Rows.Add();
            dataGridView1.Rows[rowIndex].Cells[0].Value = epc;
            dataGridView1.Rows[rowIndex].Cells[1].Value = direction;
            dataGridView1.Rows[rowIndex].Cells[2].Value = time.ToString("yyyy-MM-dd HH:mm:ss");
            dataGridView1.Rows[rowIndex].Cells[3].Value = status;
            
            // 自动滚动到最新记录
            dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.Rows.Count - 1;
        }

        private void UpdateStats()
        {
            lblTotalEntry.Text = "总进入: " + totalEntry.ToString();
            lblTotalExit.Text = "总离开: " + totalExit.ToString();
            lblCurrentInside.Text = "当前在场: " + (totalEntry - totalExit).ToString();
            
            // 强制刷新UI
            lblTotalEntry.Refresh();
            lblTotalExit.Refresh();
            lblCurrentInside.Refresh();
        }

        private void UpdateAntennaStatus()
        {
            // 更新天线状态显示
            lblAntenna1.ForeColor = antenna1Online ? Color.Green : Color.Red;
            lblAntenna1.Text = "天线1: " + (antenna1Online ? "在线" : "离线");
            
            lblAntenna8.ForeColor = antenna8Online ? Color.Green : Color.Red;
            lblAntenna8.Text = "天线8: " + (antenna8Online ? "在线" : "离线");
        }

        // 设置天线状态
        public void SetAntennaStatus(int antenna, bool online)
        {
            if (antenna == 1)
                antenna1Online = online;
            else if (antenna == 8)
                antenna8Online = online;
            
            this.Invoke(new Action(() => UpdateAntennaStatus()));
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            isMonitoring = true;
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            lblStatus.Text = "监控状态: 运行中";
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            isMonitoring = false;
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            lblStatus.Text = "监控状态: 已停止";
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            accessRecords.Clear();
            pendingDetections.Clear();
            dataGridView1.Rows.Clear();
            totalEntry = 0;
            totalExit = 0;
            UpdateStats();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV文件|*.csv";
            saveFileDialog.Title = "导出进出记录";
            
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName))
                    {
                        writer.WriteLine("EPC标签,进出方向,时间,状态");
                        
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            if (row.Cells[0].Value != null)
                            {
                                writer.WriteLine(row.Cells[0].Value + "," + row.Cells[1].Value + "," + row.Cells[2].Value + "," + row.Cells[3].Value);
                            }
                        }
                    }
                    
                    MessageBox.Show("导出成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("导出失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnResetEPC_Click(object sender, EventArgs e)
        {
            accessRecords.Clear();
            pendingDetections.Clear();
            MessageBox.Show("已重置所有EPC标签状态", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // 定时器事件处理 - 可以用于定期更新UI或检查状态
            // 当前实现为空，可以根据需要添加功能
        }

        /// <summary>
        /// 调试方法：获取当前统计信息
        /// </summary>
        public string GetDebugInfo()
        {
            return $"总进入: {totalEntry}, 总离开: {totalExit}, 当前在场: {totalEntry - totalExit}, " +
                   $"待处理记录: {pendingDetections.Count}, 已完成记录: {accessRecords.Count}";
        }
    }
}
