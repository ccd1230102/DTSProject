using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace DTSLibrary
{
    public class DTSManager
    {
        private string serverIP = "http://127.0.0.1";

        // 通过本地配置文件，初始化服务器连接信息等
        public DTSManager(string localConfigFilePath)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(localConfigFilePath);

            XmlElement root = xml.DocumentElement;

            XmlNodeList serverNodes = root.GetElementsByTagName("Server");
            if(serverNodes.Count > 0)
            {
                XmlElement serverEle = (XmlElement)serverNodes[0];
                serverIP = serverEle.InnerText;
            }

            try
            {
                XmlDocument alarmInfoConfig = WebServiceHelper.QueryPostWebService(serverIP + "/Process.asmx", "GetAlarmInfoConfigration", new Hashtable());
            }
            catch (WebException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // 设备启动
        public bool Start()
        {
            return true;
        }

        // 设备关闭
        public void Stop()
        {

        }

        // 实时更新设备运行数据
        public bool UpdateRunningData(Hashtable data)
        {
            bool ret = false;

            try
            {
                XmlDocument xml = WebServiceHelper.QuerySoapWebService(serverIP + "/Process.asmx", "PostRunningData", "data", data);

                ret = xml.InnerText == "true";
            }
            catch (WebException ex)
            {

            }

            return ret;
        }

        // 实时报告设备报警信息
        public bool ReportAlarmInfo(Hashtable Pars)
        {
            return true;
        }

        // 设备易损件信息内部更新，不需要外部接口

        // 显示设备运行数据操作界面
        public void ShowRunningDataDialog()
        {
            RunningDataDialog dlg = new RunningDataDialog
            {
                StartPosition = FormStartPosition.CenterParent
            };
            dlg.ShowDialog();

            dlg.Dispose();
        }

        // 显示设备报警信息操作界面
        public void ShowAlarmInfoDialog()
        {
        }

        // 显示设备易损件信息操作界面
        public void ShowConsumableInfoDialog()
        {
        }
    }
}
