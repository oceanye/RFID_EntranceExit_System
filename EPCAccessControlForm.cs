using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

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

        // 功率设置相关字段
        private byte fComAdr = 0xFF;
        private int frmcomportindex = 0;

        // 服务器配置 - API v3.6.6
        private string serverUrl = "http://175.24.178.44:8082";
        private string authHeader = "Basic cm9vdDpSb290cm9vdCE="; // root:Rootroot!
        private HttpClient httpClient;
        private List<string> statusOptions = new List<string>();
        
        // EPC和Assemble ID缓存
        private Dictionary<string, string> epcAssembleCache = new Dictionary<string, string>();
        // 改为高亮显示检测中状态，不自动清理

        public EPCAccessControlForm()
        {
            InitializeComponent();
            InitializeDataGridView();
            UpdateStats();
            UpdateAntennaStatus();
            InitializeHttpClient();
            InitializeStatusComboBoxes();
            
            // 设置默认功率值
            ComboBox_PowerDbm.SelectedIndex = 30; // 默认30dBm
            
            // 异步加载状态配置
            LoadStatusConfigurationAsync();
            
            // 添加窗口关闭事件处理
            this.FormClosing += EPCAccessControlForm_FormClosing;
        }

        private void EPCAccessControlForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 清理HTTP客户端资源
            if (httpClient != null)
            {
                httpClient.Dispose();
                httpClient = null;
            }
        }

        // 更新数据网格中指定EPC的待处理记录时间
        private void UpdatePendingRowInGrid(string epc, DateTime newTime)
        {
            for (int i = dataGridView1.Rows.Count - 1; i >= 0; i--)
            {
                var row = dataGridView1.Rows[i];
                if (row.Cells[0].Value?.ToString() == epc && 
                    row.Cells[1].Value?.ToString() == "检测中")
                {
                    // 只更新时间，保持Assemble ID不变
                    row.Cells[2].Value = newTime.ToString("yyyy-MM-dd HH:mm:ss");
                    
                    // 确保高亮显示样式保持
                    row.DefaultCellStyle.BackColor = Color.Yellow;
                    row.DefaultCellStyle.ForeColor = Color.Black;
                    row.DefaultCellStyle.Font = new Font(dataGridView1.Font, FontStyle.Bold);
                    
                    break; // 只更新最新的一条记录
                }
            }
        }
        private string RemovePendingRowFromGrid(string epc)
        {
            string assembleId = "";
            for (int i = dataGridView1.Rows.Count - 1; i >= 0; i--)
            {
                var row = dataGridView1.Rows[i];
                if (row.Cells[0].Value?.ToString() == epc && 
                    row.Cells[1].Value?.ToString() == "检测中")
                {
                    // 保存Assemble ID信息
                    if (dataGridView1.Columns.Count > 5 && row.Cells[5].Value != null)
                    {
                        assembleId = row.Cells[5].Value.ToString();
                    }
                    
                    dataGridView1.Rows.RemoveAt(i);
                    break; // 只移除最新的一条记录
                }
            }
            return assembleId; // 返回保存的Assemble ID
        }

        private void InitializeHttpClient()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", authHeader);
            httpClient.Timeout = TimeSpan.FromSeconds(10);
            
            // 添加User-Agent头部以符合API v3.6.6最佳实践
            httpClient.DefaultRequestHeaders.Add("User-Agent", "UHFReader288Demo/v3.6.6");
        }

        private void InitializeStatusComboBoxes()
        {
            // 设置默认状态选项
            statusOptions = new List<string>
            {
                "完成扫描录入",
                "构件录入",
                "钢构车间进场",
                "钢构车间出场",
                "混凝土车间进场",
                "混凝土车间出场"
            };
            
            UpdateStatusComboBoxes();
        }

        private void UpdateStatusComboBoxes()
        {
            cmbEntryStatus.Items.Clear();
            cmbExitStatus.Items.Clear();
            
            foreach (string status in statusOptions)
            {
                cmbEntryStatus.Items.Add(status);
                cmbExitStatus.Items.Add(status);
            }
            
            // 设置默认选择
            if (cmbEntryStatus.Items.Count > 0)
            {
                cmbEntryStatus.SelectedIndex = 0; // 默认选择第一个进入状态
            }
            if (cmbExitStatus.Items.Count > 1)
            {
                cmbExitStatus.SelectedIndex = 1; // 默认选择第二个退出状态
            }
        }

        private async void LoadStatusConfigurationAsync()
        {
            try
            {
                lblServerStatus.Text = "正在连接服务器...";
                lblServerStatus.ForeColor = Color.Orange;
                
                var response = await httpClient.GetAsync($"{serverUrl}/api/status-config");
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var serializer = new JavaScriptSerializer();
                    var statusConfig = serializer.Deserialize<StatusConfigResponse>(jsonContent);
                    
                    if (statusConfig.success && statusConfig.statuses != null)
                    {
                        statusOptions = statusConfig.statuses.ToList();
                        UpdateStatusComboBoxes();
                        lblServerStatus.Text = $"服务器连接正常 (API v3.6.6)";
                        lblServerStatus.ForeColor = Color.Green;
                    }
                    else
                    {
                        lblServerStatus.Text = "服务器配置格式错误";
                        lblServerStatus.ForeColor = Color.Orange;
                    }
                }
                else
                {
                    lblServerStatus.Text = $"服务器连接失败 ({response.StatusCode})";
                    lblServerStatus.ForeColor = Color.Red;
                }
            }
            catch (System.Net.Http.HttpRequestException httpEx)
            {
                lblServerStatus.Text = "网络连接异常";
                lblServerStatus.ForeColor = Color.Red;
                System.Diagnostics.Debug.WriteLine($"网络请求错误: {httpEx.Message}");
            }
            catch (TaskCanceledException tcEx)
            {
                lblServerStatus.Text = "连接超时";
                lblServerStatus.ForeColor = Color.Red;
                System.Diagnostics.Debug.WriteLine($"请求超时: {tcEx.Message}");
            }
            catch (Exception ex)
            {
                lblServerStatus.Text = "服务器连接异常";
                lblServerStatus.ForeColor = Color.Red;
                System.Diagnostics.Debug.WriteLine($"加载状态配置时发生错误: {ex.Message}");
            }
        }

        private async Task<bool> UploadToServerAsync(string epcId, string direction, string deviceName, string statusNote)
        {
            try
            {
                // 构建API v3.6.6兼容的请求数据
                var recordData = new
                {
                    epcId = epcId,
                    deviceId = deviceName,
                    statusNote = statusNote,
                    createTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    location = direction == "进入" ? "门禁进入" : "门禁离开",
                    rssi = "", // 可选字段
                    assembleId = "" // 可选字段
                };
                
                var serializer = new JavaScriptSerializer();
                var jsonContent = serializer.Serialize(recordData);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                
                var response = await httpClient.PostAsync($"{serverUrl}/api/epc-record", content);
                
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    // 记录API错误响应码以便调试
                    var errorContent = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"API上传失败: {response.StatusCode}, 响应: {errorContent}");
                    return false;
                }
            }
            catch (System.Net.Http.HttpRequestException httpEx)
            {
                // 网络相关错误
                System.Diagnostics.Debug.WriteLine($"网络请求错误: {httpEx.Message}");
                return false;
            }
            catch (TaskCanceledException tcEx)
            {
                // 超时错误
                System.Diagnostics.Debug.WriteLine($"请求超时: {tcEx.Message}");
                return false;
            }
            catch (Exception ex)
            {
                // 其他未知错误
                System.Diagnostics.Debug.WriteLine($"上传到服务器时发生未知错误: {ex.Message}");
                return false;
            }
        }

        // 状态配置响应类
        public class StatusConfigResponse
        {
            public bool success { get; set; }
            public string[] statuses { get; set; }
            public string timestamp { get; set; }
        }

        // EPC记录查询响应类
        public class EPCRecordsResponse
        {
            public bool success { get; set; }
            public EPCRecordData[] data { get; set; }
            public PaginationInfo pagination { get; set; }
        }

        public class EPCRecordData
        {
            public int id { get; set; }
            public string epc_id { get; set; }
            public string device_id { get; set; }
            public string status_note { get; set; }
            public string assemble_id { get; set; }
            public string location { get; set; }
            public string create_time { get; set; }
            public string upload_time { get; set; }
            public string rssi { get; set; }
            public string device_type { get; set; }
            public string app_version { get; set; }
        }

        public class PaginationInfo
        {
            public int total { get; set; }
            public int limit { get; set; }
            public int offset { get; set; }
            public int returned { get; set; }
        }

        /// <summary>
        /// 查询EPC对应的最新Assemble ID
        /// </summary>
        /// <param name="epcId">EPC标签ID</param>
        /// <returns>Assemble ID，如果未找到返回空字符串</returns>
        private async Task<string> QueryAssembleIdAsync(string epcId)
        {
            try
            {
                // 首先检查缓存
                if (epcAssembleCache.ContainsKey(epcId))
                {
                    System.Diagnostics.Debug.WriteLine($"从缓存获取EPC {epcId} 的Assemble ID: {epcAssembleCache[epcId]}");
                    return epcAssembleCache[epcId];
                }

                // 查询服务器获取该EPC的最新记录
                // API使用模糊匹配，为了精确匹配，我们获取更多记录然后筛选
                var queryUrl = $"{serverUrl}/api/epc-records?epcId={Uri.EscapeDataString(epcId)}&limit=50&offset=0";
                System.Diagnostics.Debug.WriteLine($"正在查询EPC记录: {queryUrl}");
                
                var response = await httpClient.GetAsync(queryUrl);
                System.Diagnostics.Debug.WriteLine($"API响应状态: {response.StatusCode}");
                
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"API响应内容: {jsonContent}");
                    
                    var serializer = new JavaScriptSerializer();
                    var epcRecordsResponse = serializer.Deserialize<EPCRecordsResponse>(jsonContent);
                    
                    if (epcRecordsResponse.success)
                    {
                        System.Diagnostics.Debug.WriteLine($"API返回成功，数据记录数: {epcRecordsResponse.data?.Length ?? 0}");
                        
                        if (epcRecordsResponse.data != null && epcRecordsResponse.data.Length > 0)
                        {
                            // 由于API使用模糊匹配，需要找到精确匹配的记录
                            var exactMatch = epcRecordsResponse.data
                                .Where(record => record.epc_id == epcId)
                                .OrderByDescending(record => record.create_time) // 按创建时间降序排列，获取最新的
                                .FirstOrDefault();

                            if (exactMatch != null)
                            {
                                var assembleId = exactMatch.assemble_id ?? "";
                                
                                System.Diagnostics.Debug.WriteLine($"找到精确匹配的EPC {epcId}，Assemble ID: '{assembleId}'");
                                
                                // 缓存结果
                                epcAssembleCache[epcId] = assembleId;
                                
                                return assembleId;
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine($"在返回的 {epcRecordsResponse.data.Length} 条记录中没有找到精确匹配的EPC: {epcId}");
                                // 调试：显示返回的EPC ID
                                foreach (var record in epcRecordsResponse.data.Take(5)) // 只显示前5个
                                {
                                    System.Diagnostics.Debug.WriteLine($"  返回的EPC: '{record.epc_id}'");
                                }
                            }
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"EPC {epcId} 没有找到任何记录");
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"API返回失败: success = false");
                    }
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"查询EPC记录失败: {response.StatusCode}, 响应: {errorContent}");
                }
            }
            catch (System.Net.Http.HttpRequestException httpEx)
            {
                System.Diagnostics.Debug.WriteLine($"查询EPC记录网络错误: {httpEx.Message}");
            }
            catch (TaskCanceledException tcEx)
            {
                System.Diagnostics.Debug.WriteLine($"查询EPC记录超时: {tcEx.Message}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"查询EPC记录时发生错误: {ex.Message}");
            }
            
            // 缓存空结果，避免重复查询
            epcAssembleCache[epcId] = "";
            System.Diagnostics.Debug.WriteLine($"EPC {epcId} 查询失败，缓存空结果");
            return "";
        }

        private void InitializeDataGridView()
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns.Clear();
            
            dataGridView1.Columns.Add("EPC", "EPC标签");
            dataGridView1.Columns.Add("Direction", "进出方向");
            dataGridView1.Columns.Add("Time", "时间");
            dataGridView1.Columns.Add("Status", "状态");
            dataGridView1.Columns.Add("Device", "设备");
            dataGridView1.Columns.Add("AssembleId", "组装件ID");
            
            dataGridView1.Columns[0].DataPropertyName = "EPC";
            dataGridView1.Columns[1].DataPropertyName = "Direction";
            dataGridView1.Columns[2].DataPropertyName = "Time";
            dataGridView1.Columns[3].DataPropertyName = "Status";
            dataGridView1.Columns[4].DataPropertyName = "Device";
            dataGridView1.Columns[5].DataPropertyName = "AssembleId";
            
            dataGridView1.Columns[0].Width = 120;
            dataGridView1.Columns[1].Width = 80;
            dataGridView1.Columns[2].Width = 120;
            dataGridView1.Columns[3].Width = 100;
            dataGridView1.Columns[4].Width = 80;
            dataGridView1.Columns[5].Width = 100;
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

                // 异步查询Assemble ID（如果尚未缓存）
                Task.Run(async () =>
                {
                    await QueryAssembleIdAsync(epc);
                });

                // 检查是否已有待处理记录
                if (pendingDetections.ContainsKey(epc))
                {
                    var pending = pendingDetections[epc];
                    
                    // 如果是同一天线的重复检测，更新时间
                    if (pending.Antenna == antenna)
                    {
                        pending.DetectionTime = detectionTime;
                        // 更新数据网格中的时间
                        UpdatePendingRowInGrid(epc, detectionTime);
                        return;
                    }
                    
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
                            // 获取当前设备名和状态选择
                            string deviceName = txtDeviceName.Text.Trim();
                            if (string.IsNullOrEmpty(deviceName))
                            {
                                deviceName = "Unknown_Device";
                            }
                            
                            string statusNote = "";
                            if (direction == "进入" && cmbEntryStatus.SelectedItem != null)
                            {
                                statusNote = cmbEntryStatus.SelectedItem.ToString();
                            }
                            else if (direction == "离开" && cmbExitStatus.SelectedItem != null)
                            {
                                statusNote = cmbExitStatus.SelectedItem.ToString();
                            }
                            
                            if (string.IsNullOrEmpty(statusNote))
                            {
                                statusNote = "完成扫描录入"; // 默认状态
                            }

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

                            // 更新UI - 移除原有的"检测中"记录并保留Assemble ID
                            string savedAssembleId = RemovePendingRowFromGrid(epc);
                            AddToGrid(epc, direction, detectionTime, statusNote, deviceName, savedAssembleId);
                            UpdateStats();
                            
                            // 异步上传到服务器
                            Task.Run(async () =>
                            {
                                bool success = await UploadToServerAsync(epc, direction, deviceName, statusNote);
                                if (!success)
                                {
                                    // 在UI线程中显示上传失败的提示
                                    this.Invoke(new Action(() =>
                                    {
                                        // 可以在这里添加上传失败的显示逻辑
                                    }));
                                }
                            });
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
                    AddToGrid(epc, "检测中", detectionTime, "天线" + antenna.ToString(), txtDeviceName.Text.Trim());
                    
                    // 更新统计信息以显示待处理状态
                    UpdateStats();
                }
            }));
        }

        private void AddToGrid(string epc, string direction, DateTime time, string status, string deviceName = "", string existingAssembleId = null)
        {
            int rowIndex = dataGridView1.Rows.Add();
            var row = dataGridView1.Rows[rowIndex];
            
            row.Cells[0].Value = epc;
            row.Cells[1].Value = direction;
            row.Cells[2].Value = time.ToString("yyyy-MM-dd HH:mm:ss");
            row.Cells[3].Value = status;
            if (dataGridView1.Columns.Count > 4)
            {
                row.Cells[4].Value = deviceName;
            }
            
            // 添加Assemble ID显示 - 优先使用已存在的ID
            if (dataGridView1.Columns.Count > 5)
            {
                string assembleId = "";
                
                // 如果提供了现有的Assemble ID，直接使用
                if (!string.IsNullOrEmpty(existingAssembleId))
                {
                    assembleId = existingAssembleId;
                }
                // 检查缓存中是否已有ID
                else if (epcAssembleCache.ContainsKey(epc))
                {
                    assembleId = string.IsNullOrEmpty(epcAssembleCache[epc]) ? "未找到" : epcAssembleCache[epc];
                }
                // 首次检测时才查询
                else if (direction == "检测中")
                {
                    assembleId = "查询中...";
                    // 异步查询，只在首次检测时进行
                    Task.Run(async () =>
                    {
                        var retrievedId = await QueryAssembleIdAsync(epc);
                        this.Invoke(new Action(() =>
                        {
                            // 更新所有该EPC的记录的Assemble ID
                            UpdateAssembleIdForEPC(epc, string.IsNullOrEmpty(retrievedId) ? "未找到" : retrievedId);
                        }));
                    });
                }
                // 其他状态如果没有缓存ID，显示"未查询"
                else
                {
                    assembleId = "未查询";
                }
                
                row.Cells[5].Value = assembleId;
            }
            
            // 如果是"检测中"状态，设置高亮显示
            if (direction == "检测中")
            {
                row.DefaultCellStyle.BackColor = Color.Yellow;  // 黄色背景
                row.DefaultCellStyle.ForeColor = Color.Black;   // 黑色字体
                row.DefaultCellStyle.Font = new Font(dataGridView1.Font, FontStyle.Bold); // 粗体
            }
            else
            {
                // 恢复默认样式
                row.DefaultCellStyle.BackColor = dataGridView1.DefaultCellStyle.BackColor;
                row.DefaultCellStyle.ForeColor = dataGridView1.DefaultCellStyle.ForeColor;
                row.DefaultCellStyle.Font = dataGridView1.DefaultCellStyle.Font;
            }
            
            // 自动滚动到最新记录
            dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.Rows.Count - 1;
        }

        /// <summary>
        /// 更新指定EPC的所有记录的Assemble ID显示
        /// </summary>
        /// <param name="epc">EPC标签ID</param>
        /// <param name="assembleId">Assemble ID</param>
        private void UpdateAssembleIdForEPC(string epc, string assembleId)
        {
            if (dataGridView1.Columns.Count <= 5) return;
            
            // 更新所有匹配EPC的行
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[0].Value?.ToString() == epc)
                {
                    row.Cells[5].Value = assembleId;
                }
            }
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
            epcAssembleCache.Clear(); // 清空Assemble ID缓存
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
                        writer.WriteLine("EPC标签,进出方向,时间,状态,设备,组装件ID");
                        
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            if (row.Cells[0].Value != null)
                            {
                                var epc = row.Cells[0].Value?.ToString() ?? "";
                                var direction = row.Cells[1].Value?.ToString() ?? "";
                                var time = row.Cells[2].Value?.ToString() ?? "";
                                var status = row.Cells[3].Value?.ToString() ?? "";
                                var device = row.Cells[4].Value?.ToString() ?? "";
                                var assembleId = dataGridView1.Columns.Count > 5 ? 
                                    (row.Cells[5].Value?.ToString() ?? "") : "";
                                
                                writer.WriteLine($"{epc},{direction},{time},{status},{device},{assembleId}");
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
            epcAssembleCache.Clear(); // 清空Assemble ID缓存
            MessageBox.Show("已重置所有EPC标签状态和缓存", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void BT_DBM_Click(object sender, EventArgs e)
        {
            try
            {
                byte powerDbm = (byte)ComboBox_PowerDbm.SelectedIndex;
                MessageBox.Show($"功率设置为: {ComboBox_PowerDbm.SelectedItem}", "功率设置", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("设置功率失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefreshStatus_Click(object sender, EventArgs e)
        {
            LoadStatusConfigurationAsync();
        }

        /// <summary>
        /// 测试按钮 - 手动查询Assemble ID
        /// </summary>
        private async void btnTestAssembleId_Click(object sender, EventArgs e)
        {
            string testEpc = "";
            
            // 获取测试用的EPC
            if (dataGridView1.Rows.Count > 0 && dataGridView1.Rows[0].Cells[0].Value != null)
            {
                testEpc = dataGridView1.Rows[0].Cells[0].Value.ToString();
            }
            else
            {
                MessageBox.Show("请先检测一些EPC标签再进行测试，或者先运行API诊断查看数据库中的记录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            // 清除缓存以强制查询
            if (epcAssembleCache.ContainsKey(testEpc))
            {
                epcAssembleCache.Remove(testEpc);
            }
            
            System.Diagnostics.Debug.WriteLine($"=== 开始测试查询EPC: {testEpc} ===");
            MessageBox.Show($"开始测试查询EPC: {testEpc}\n请查看Debug输出窗口获取详细信息", "测试查询", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            string result = await QueryAssembleIdAsync(testEpc);
            
            System.Diagnostics.Debug.WriteLine($"=== 查询完成，结果: '{result}' ===");
            MessageBox.Show($"查询结果: '{result}'\n\n详细调试信息请查看输出窗口", "查询结果", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 简单的API连接测试
        /// </summary>
        private async void btnTestConnection_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. 测试健康检查
                System.Diagnostics.Debug.WriteLine("=== 开始API诊断 ===");
                
                var healthUrl = $"{serverUrl}/health";
                System.Diagnostics.Debug.WriteLine($"1. 测试健康检查: {healthUrl}");
                
                var healthResponse = await httpClient.GetAsync(healthUrl);
                var healthContent = await healthResponse.Content.ReadAsStringAsync();
                
                System.Diagnostics.Debug.WriteLine($"健康检查状态: {healthResponse.StatusCode}");
                System.Diagnostics.Debug.WriteLine($"健康检查响应: {healthContent}");
                
                // 2. 测试获取最新的几条记录
                var testUrl = $"{serverUrl}/api/epc-records?limit=10&offset=0";
                System.Diagnostics.Debug.WriteLine($"2. 测试获取记录: {testUrl}");
                
                var response = await httpClient.GetAsync(testUrl);
                var content = await response.Content.ReadAsStringAsync();
                
                System.Diagnostics.Debug.WriteLine($"记录查询状态: {response.StatusCode}");
                System.Diagnostics.Debug.WriteLine($"记录查询响应: {content}");
                
                // 3. 尝试解析JSON看结构
                if (response.IsSuccessStatusCode)
                {
                    var serializer = new JavaScriptSerializer();
                    try
                    {
                        var epcRecordsResponse = serializer.Deserialize<EPCRecordsResponse>(content);
                        System.Diagnostics.Debug.WriteLine($"JSON解析成功: success={epcRecordsResponse.success}, 记录数={epcRecordsResponse.data?.Length ?? 0}");
                        
                        if (epcRecordsResponse.data != null && epcRecordsResponse.data.Length > 0)
                        {
                            var firstRecord = epcRecordsResponse.data[0];
                            System.Diagnostics.Debug.WriteLine($"第一条记录:");
                            System.Diagnostics.Debug.WriteLine($"  EPC ID: '{firstRecord.epc_id}'");
                            System.Diagnostics.Debug.WriteLine($"  Assemble ID: '{firstRecord.assemble_id}'");
                            System.Diagnostics.Debug.WriteLine($"  Device ID: '{firstRecord.device_id}'");
                            System.Diagnostics.Debug.WriteLine($"  Create Time: '{firstRecord.create_time}'");
                            
                            // 显示前几条记录的EPC和Assemble ID
                            for (int i = 0; i < Math.Min(5, epcRecordsResponse.data.Length); i++)
                            {
                                var record = epcRecordsResponse.data[i];
                                System.Diagnostics.Debug.WriteLine($"记录 {i+1}: EPC='{record.epc_id}', Assemble='{record.assemble_id ?? "NULL"}'");
                            }
                        }
                    }
                    catch (Exception jsonEx)
                    {
                        System.Diagnostics.Debug.WriteLine($"JSON解析失败: {jsonEx.Message}");
                    }
                }
                
                MessageBox.Show($"API诊断完成!\n健康检查: {healthResponse.StatusCode}\n记录查询: {response.StatusCode}\n\n详细信息请查看Debug输出窗口", "API诊断", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"API诊断失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine($"API诊断异常: {ex}");
            }
        }

        private void btnShowPending_Click(object sender, EventArgs e)
        {
            // 显示当前检测中的记录信息
            var count = pendingDetections.Count;
            if (count > 0)
            {
                var info = new System.Text.StringBuilder();
                info.AppendLine($"当前有 {count} 条检测中的记录：\n");
                
                foreach (var kvp in pendingDetections)
                {
                    var detection = kvp.Value;
                    var timeSpan = DateTime.Now - detection.DetectionTime;
                    info.AppendLine($"EPC: {kvp.Key}");
                    info.AppendLine($"天线: {detection.Antenna}");
                    info.AppendLine($"检测时间: {detection.DetectionTime:yyyy-MM-dd HH:mm:ss}");
                    info.AppendLine($"持续时间: {timeSpan.TotalMinutes:F1} 分钟");
                    info.AppendLine("---");
                }
                
                MessageBox.Show(info.ToString(), "检测中状态详情", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("当前没有检测中的记录", "检测中状态", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 获取返回码描述
        /// </summary>
        /// <param name="errCode">错误码</param>
        /// <returns>错误描述</returns>
        private string GetReturnCodeDesc(int errCode)
        {
            switch (errCode)
            {
                case 0x00:
                    return "操作成功";
                case 0x01:
                    return "返回前操作失败";
                case 0x02:
                    return "没有标签可操作";
                case 0x03:
                    return "发送数据有错误";
                case 0x04:
                    return "读写器不支持该命令";
                case 0x05:
                    return "读写器内部错误";
                case 0x06:
                    return "参数错误";
                case 0x08:
                    return "通讯繁忙";
                case 0x09:
                    return "电子标签返回错误码";
                case 0x0A:
                    return "电子标签未认证";
                case 0x0B:
                    return "电子标签被锁定";
                case 0x0C:
                    return "电子标签电源不足";
                case 0x0D:
                    return "电子标签不支持该操作";
                case 0x0E:
                    return "电子标签存储空间不足";
                case 0x0F:
                    return "电子标签存储空间被锁定";
                case 0x10:
                    return "密码错误";
                case 0x11:
                    return "电子标签已经被灭活";
                case 0x12:
                    return "电子标签不存在灭活密码";
                case 0x13:
                    return "电子标签不支持该错误码";
                case 0x14:
                    return "调整失败";
                case 0x15:
                    return "不能调整";
                case 0x16:
                    return "读写器不支持该功能";
                case 0x17:
                    return "读写器不支持该指令";
                case 0x18:
                    return "指令执行失败";
                case 0x19:
                    return "自定义指令执行失败";
                case 0xF8:
                    return "检测天线错误";
                case 0xF9:
                    return "检测温度错误";
                case 0xFA:
                    return "读写器地址错误";
                case 0xFB:
                    return "读写器波特率错误";
                case 0xFC:
                    return "读写器参数错误";
                case 0xFD:
                    return "读写器校验和错误";
                case 0xFE:
                    return "读写器数据长度错误";
                case 0xFF:
                    return "读写器命令错误";
                default:
                    return "未知错误";
            }
        }

    }
}
