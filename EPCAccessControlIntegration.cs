using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UHFReader288Demo
{
    /// <summary>
    /// EPC门禁系统集成类
    /// 用于集成EPC门禁功能到主应用程序
    /// </summary>
    public class EPCAccessControlIntegration
    {
        private static EPCAccessControlForm accessControlForm;
        private static bool isInitialized = false;

        /// <summary>
        /// 初始化EPC门禁系统
        /// </summary>
        public static void Initialize()
        {
            if (!isInitialized)
            {
                accessControlForm = new EPCAccessControlForm();
                isInitialized = true;
            }
        }

        /// <summary>
        /// 显示EPC门禁管理窗口
        /// </summary>
        public static void ShowAccessControlForm()
        {
            // 检查窗口是否已释放，如果是则重新创建
            if (accessControlForm == null || accessControlForm.IsDisposed)
            {
                accessControlForm = new EPCAccessControlForm();
                isInitialized = true;
            }

            if (accessControlForm != null)
            {
                accessControlForm.Show();
                if (accessControlForm.WindowState == System.Windows.Forms.FormWindowState.Minimized)
                {
                    accessControlForm.WindowState = System.Windows.Forms.FormWindowState.Normal;
                }
                accessControlForm.BringToFront();
            }
        }

        /// <summary>
        /// 隐藏EPC门禁管理窗口
        /// </summary>
        public static void HideAccessControlForm()
        {
            if (isInitialized && accessControlForm != null && !accessControlForm.IsDisposed)
            {
                accessControlForm.Hide();
            }
        }

        /// <summary>
        /// 处理标签检测事件
        /// </summary>
        /// <param name="epc">EPC标签</param>
        /// <param name="antenna">天线编号</param>
        /// <param name="detectionTime">检测时间</param>
        public static void ProcessTagDetection(string epc, int antenna, DateTime detectionTime)
        {
            // 确保窗口可用，如果已释放则重新创建
            if (accessControlForm == null || accessControlForm.IsDisposed)
            {
                accessControlForm = new EPCAccessControlForm();
                isInitialized = true;
            }
            
            if (accessControlForm != null)
            {
                accessControlForm.ProcessTagDetection(epc, antenna, detectionTime);
            }
        }

        /// <summary>
        /// 设置天线状态
        /// </summary>
        /// <param name="antenna">天线编号</param>
        /// <param name="online">是否在线</param>
        public static void SetAntennaStatus(int antenna, bool online)
        {
            // 确保窗口可用，如果已释放则重新创建
            if (accessControlForm == null || accessControlForm.IsDisposed)
            {
                accessControlForm = new EPCAccessControlForm();
                isInitialized = true;
            }
            
            if (accessControlForm != null)
            {
                accessControlForm.SetAntennaStatus(antenna, online);
            }
        }

        /// <summary>
        /// 获取调试信息
        /// </summary>
        /// <returns>调试信息字符串</returns>
        public static string GetDebugInfo()
        {
            // 确保窗口可用，如果已释放则重新创建
            if (accessControlForm == null || accessControlForm.IsDisposed)
            {
                return "EPC门禁系统窗口已关闭";
            }
            
            return accessControlForm.GetDebugInfo();
        }

        /// <summary>
        /// 检查EPC门禁窗口是否已打开
        /// </summary>
        public static bool IsAccessControlFormOpen
        {
            get
            {
                return isInitialized && accessControlForm != null && !accessControlForm.IsDisposed && accessControlForm.Visible;
            }
        }

        /// <summary>
        /// 获取EPC门禁窗口实例
        /// </summary>
        public static EPCAccessControlForm AccessControlForm
        {
            get { return accessControlForm; }
        }
    }
}
