using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

public struct Employeestruct
{
    private int _fooNumber;

    public Employeestruct(int fooNumber)
        : this()
    {
        _fooNumber = fooNumber;
    }

    public string Name { get; set; }

    public int Age { get; set; }

    public string GetMessage()
    {
        return string.Format("{0}--{1}", Name, Age);
    }
}

public struct AlarmInfo
{
    public int AlarmID { get; set; }

    public string AlarmName { get; set; }

    public int AlarmLevel { get; set; }

    public string HandleAlarmMode { get; set; }

    public int ServerWarningID { get; set; }


    public AlarmInfo(int Id)
        : this()
    {
        AlarmID = Id;
        XmlDocument XMLalarmInfo = new XmlDocument();

        string EXEPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        XMLalarmInfo.Load(EXEPath + "alarmInfoConfig.xml");
        XmlElement el = XMLalarmInfo.DocumentElement;
        XmlNodeList AlarmInfoNodes = el.GetElementsByTagName("AlarmInfo");

        int i = 0;
        foreach (XmlNode node in AlarmInfoNodes)
        {
            XmlNodeList AlarmNode = node.ChildNodes;
            XmlElement AlarmEle = (XmlElement)AlarmNode[0];
            if (AlarmID == Convert.ToInt32(AlarmEle.InnerText))
            {
                AlarmName = ((XmlElement)AlarmNode[1]).InnerText;
                string stAlarmLevel = ((XmlElement)AlarmNode[2]).InnerText;
                AlarmLevel = Convert.ToInt32(stAlarmLevel);
                HandleAlarmMode = ((XmlElement)AlarmNode[3]).InnerText;
                break;
            }
            i++;
        }

    }

}

namespace DTSLibrary
{
    public class DTSManager
    {
        private string serverIP = "http://127.0.0.1";
        public static string m_shift = "";//班次

        public static PostToSever PostSever;
        public static RunningDataDialog Runningdlg = new RunningDataDialog { StartPosition = FormStartPosition.CenterParent };
        public static AlarmDataDialog Alarmdlg = new AlarmDataDialog { StartPosition = FormStartPosition.CenterParent };
        private static ConsumableMain Consumabledlg1 = new ConsumableMain() { StartPosition = FormStartPosition.CenterParent };
        // 通过本地配置文件，初始化服务器连接信息等
        public DTSManager(string localConfigFilePath)
        {
            PostSever = new PostToSever(localConfigFilePath);
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
                string EXEPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                alarmInfoConfig.Save((EXEPath + "alarmInfoConfig.xml"));        
            }
            catch (WebException ex)
            {
                MessageBox.Show(ex.Message);
            }

            try
            {
                XmlDocument alarmInfoConfig = WebServiceHelper.QueryPostWebService(serverIP + "/Process.asmx", "GetConsumableInfoConfigration", new Hashtable());
                alarmInfoConfig.Save(Application.StartupPath + "\\ConsumableInfoConfig.xml");
            }
            catch (WebException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        // 设备启动
        public bool Start(string shift)
        {
            //1.查询数据库，是否创建数据库
            //2.和配置文件对比，是否需要更新数据表和字段
            //4.抓取配置文件
            //3.开始计时
            //数据库3张表，1、运行数据；2、报警信息；3、易损件信息
            m_shift = shift;
            Thread t1 = new Thread(new ParameterizedThreadStart(StartThread));
            t1.Start(shift);

            return true;
        }

        // 设备关闭
        public void Stop()
        {
            Thread t1 = new Thread(new ThreadStart(StopThread));
            t1.Start();
        }

        // 实时更新设备运行数据
        public bool UpdateRunningData(Hashtable data)
        {
            Thread thread = new Thread(new ParameterizedThreadStart(UpdateRunningDataThread));
            thread.Start(data);
            return true;
        }

        // 实时报告设备报警信息
        public bool ReportAlarmInfo(Hashtable Pars)
        {
            Thread thread = new Thread(new ParameterizedThreadStart(ReportAlarmInfoThread));
            thread.Start(Pars);

            return true;
        }

        // 设备易损件信息内部更新，不需要外部接口

        // 显示设备运行数据操作界面
        public void ShowRunningDataDialog()
        {
            Runningdlg.ShowDialog();
        }

        // 显示设备报警信息操作界面
        public void ShowAlarmInfoDialog()
        {
            Alarmdlg.ShowDialog();
        }

        // 显示设备易损件信息操作界面
        public void ShowConsumableInfoDialog()
        {
            Consumabledlg1.ShowDialog();
        }

        private static void StartThread(object obj)
        {
            string shift = obj as string;
            DatabaseHandle.CreateTable();
            bool ret = PostSever.PostRunningData(1, shift, 0);
            DatabaseHandle.RecordStartToDB(ret);
            Runningdlg.ThreadStart();
            Consumabledlg1.ThreadStart();
            Alarmdlg.ThreadStart();
        }
        public static void StopThread()
        {
            bool ret = PostSever.PostRunningData(3, m_shift, 0);
            DatabaseHandle.RecordStopToDB(ret);
            Runningdlg.Dispose();
            Consumabledlg1.Dispose();
            Alarmdlg.Dispose();
        }

        static void UpdateRunningDataThread(object obj)
        {
            Hashtable data = obj as Hashtable;

            if (data.Contains("TYPE") && data.Contains("Count"))
            {
                string Status = (string)data["TYPE"];
                int ProduceCount = (int)data["Count"];
                DatabaseHandle.RecordProduceToDB(ProduceCount);
            }         

        }

        static void ReportAlarmInfoThread(object obj)
        {
            Hashtable data = obj as Hashtable;

            if (data.Contains("TYPE") && data.Contains("AlarmID"))
            {
                string Status = (string)data["TYPE"];
                int nAlarmID = (int)data["AlarmID"];

                AlarmInfo AlaInfo = new AlarmInfo(nAlarmID);

                AlaInfo.ServerWarningID = PostSever.PostWarningData(nAlarmID);
                
                string stDBid = DatabaseHandle.RecordAlarmToDB(AlaInfo);
              
                if (!string.IsNullOrEmpty(stDBid))
                {
                    AlarmDetailForm dlg = new AlarmDetailForm(stDBid,1)
                    {
                        StartPosition = FormStartPosition.CenterParent
                    };
                    dlg.ShowDialog();

                    dlg.Dispose();
                }
            }

        }
    }
}
