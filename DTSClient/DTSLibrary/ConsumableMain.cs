using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using SQLiteQueryBrowser;
using System.Xml;

namespace DTSLibrary
{
    public partial class ConsumableMain : Form
    {
        public ConsumableMain()
        {
            InitializeComponent();
        }

        public void ThreadStart()
        {
            SetTimerParam();
        }

        public System.Timers.Timer aTimer = new System.Timers.Timer();
        private bool m_bFormShow = false;
        private ConsumableChange ConsumableChangedlg = null;

        static object locker = new object();

        private void TimeThread(object source, System.Timers.ElapsedEventArgs e)
        {//定时查询数据库更新易损件的使用时间 
            string EXEPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string dbPath = EXEPath + "Demo.db3";
            SQLiteDBHelper db = new SQLiteDBHelper(dbPath);

            /*string sql2 = "UPDATE ConsumableLog SET WorkingTime = WorkingTime+1";// where id = " + stID;

                SQLiteParameter[] parameters = new SQLiteParameter[]{
                        new SQLiteParameter("@WorkingTime",nWorkingTime)
                };

                db.ExecuteNonQuery(sql2, parameters);*/

            XmlDocument XMLalarmInfo = new XmlDocument();
            XMLalarmInfo.Load(EXEPath + "ConsumableInfoConfig.xml");
            XmlElement el = XMLalarmInfo.DocumentElement;
            XmlNodeList ConsumableInfoNodes = el.GetElementsByTagName("ConsumableInfo");

            List<int> ids = new List<int>() {};
            List<int> times = new List<int>() {};

            foreach (XmlNode node in ConsumableInfoNodes)
            {
                XmlNodeList ConsumableNode = node.ChildNodes;
                string ConsumableID = ((XmlElement)ConsumableNode[0]).InnerText;
                
                string ConsumableName = ((XmlElement)ConsumableNode[1]).InnerText;
                //string stLifetime = ((XmlElement)ConsumableNode[3]).InnerText;

                string sql3 = "select max(id), WorkingTime, Lifetime from ConsumableLog where ConsumableID = " + ConsumableID;

                string stID = "";
                string stWorkingTime = "";
                string stLifetime = "";

                using (SQLiteDataReader reader = db.ExecuteReader(sql3, null))
                {
                    while (reader.Read())
                    {
                        stID = reader["max(id)"].ToString();
                        stWorkingTime = reader["WorkingTime"].ToString();
                        stLifetime = reader["Lifetime"].ToString();
                    }
                }
                int nWorkingTime = Convert.ToInt32(stWorkingTime)+1;
                //if(nWorkingTime%60 == 0)
                {
                    ids.Add(Convert.ToInt32(ConsumableID));
                    times.Add(nWorkingTime/60);
                }
                string sql2 = "UPDATE ConsumableLog SET WorkingTime = WorkingTime+1 where id = " + stID;

                db.ExecuteNonQuery(sql2, null);

                if(nWorkingTime >= Convert.ToInt32(stLifetime))
                {
                    if (ConsumableChangedlg == null)
                    { 
                        ConsumableChangedlg = new ConsumableChange(ConsumableID, null)
                        {
                            StartPosition = FormStartPosition.CenterParent
                        };
                        ConsumableChangedlg.ShowDialog();
                        ConsumableChangedlg.Dispose();
                        ConsumableChangedlg = null;
                    }
                }
            }

            ChangeListview(true);//刷新listview

            DTSManager.PostSever.PostConsumableData(ids, times);//发送易损件运行时间更新到server    

            //IsPostConsumable();//补发
        }

        public void IsPostConsumable()
        {//循环查询未发送到服务端的Consumable信息，补发
            List<string> IsPostIDList = new List<string>() { };
            string EXEPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string dbPath = EXEPath + "Demo.db3";
            string sql = "select * from ConsumableLog where IsPost = 0";
            string stID = "";
            string ChangePeopleName = "";
            string stConsumableID = "";
            string stWorkingTime = "";
            string stChangetime = "";
            SQLiteDBHelper db = new SQLiteDBHelper(dbPath);
            using (SQLiteDataReader reader = db.ExecuteReader(sql, null))
            {
                while (reader.Read())
                {
                    stID = reader["id"].ToString();
                    stConsumableID = reader["ConsumableID"].ToString();
                    stWorkingTime = reader["WorkingTime"].ToString();
                    ChangePeopleName = reader["ChangePeopleName"].ToString();
                    stChangetime = reader["Changetime"].ToString();


                    bool ret = false;
                    ret = DTSManager.PostSever.PostConsumableReplaceData(ChangePeopleName, Convert.ToInt32(stConsumableID), Convert.ToInt32(stWorkingTime), stChangetime);
 
                    if (ret)
                    {
                        IsPostIDList.Add(stID);
                    }
                }
            }
            foreach (string elm in IsPostIDList)
            {
                string sql2 = "UPDATE ConsumableLog SET IsPost = @IsPost where id = " + stID;
                SQLiteParameter[] parameters = new SQLiteParameter[]{
                                    new SQLiteParameter("@IsPost",1)
                            };
                int DBResult = db.ExecuteNonQuery(sql2, parameters);
            }

        }

        public void SetTimerParam()
        {
            //到时间的时候执行事件  
            aTimer.Elapsed += new System.Timers.ElapsedEventHandler(TimeThread);
            aTimer.Interval = 60 * 1000;
            aTimer.AutoReset = true;//执行一次 false，一直执行true  
            //是否执行System.Timers.Timer.Elapsed事件  
            aTimer.Enabled = true;
        }

        private void ConsumableMain_Shown(object sender, EventArgs e)
        {
            m_bFormShow = true;
            ConsumablelistView.Items.Clear();
            ChangeListview(false);
            
        }  

        private void ConsumableMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_bFormShow = false; 
            this.Hide();
        }

        private void ChangeListview(bool bRefresh)
        {//查询数据库，将信息显示到Listview上
            lock (locker)
            {                               
                string EXEPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                string dbPath = EXEPath + "Demo.db3";
                SQLiteDBHelper db = new SQLiteDBHelper(dbPath);

                XmlDocument XMLalarmInfo = new XmlDocument();
                XMLalarmInfo.Load(EXEPath + "ConsumableInfoConfig.xml");
                XmlElement el = XMLalarmInfo.DocumentElement;
                XmlNodeList ConsumableInfoNodes = el.GetElementsByTagName("ConsumableInfo");
                int Count = 0;
                foreach (XmlNode node in ConsumableInfoNodes)
                {
                    XmlNodeList ConsumableNode = node.ChildNodes;
                    string ConsumableID = ((XmlElement)ConsumableNode[0]).InnerText;
                    string ConsumableName = ((XmlElement)ConsumableNode[1]).InnerText;
                    string stLifetime = ((XmlElement)ConsumableNode[3]).InnerText;
                    stLifetime = stLifetime + "(" +(Convert.ToInt32(stLifetime)*60).ToString() + "分钟)";

                    string sql3 = "select max(id), WorkingTime, Changetime from ConsumableLog where ConsumableID = " + ConsumableID;

                    string stID = "";
                    string stWorkingTime = "";
                    string stChangeDate = "";
                    using (SQLiteDataReader reader = db.ExecuteReader(sql3, null))
                    {
                        while (reader.Read())
                        {
                            stID = reader["max(id)"].ToString();
                            stWorkingTime = reader["WorkingTime"].ToString();
                            stChangeDate = reader["Changetime"].ToString();
                        }
                    }
                    if (m_bFormShow)
                    {
                        if (bRefresh)
                        { 
                            ConsumablelistView.Items[Count].SubItems[4].Text = stWorkingTime;
                        }
                        else
                        {
                            ListViewItem item1 = new ListViewItem(ConsumableID);
                            item1.SubItems.Add(ConsumableName);
                            item1.SubItems.Add(stLifetime);
                            item1.SubItems.Add(stChangeDate);
                            item1.SubItems.Add(stWorkingTime);
                            ConsumablelistView.Items.Add(item1);
                        }
                    }
                    Count++;

                }
            }
        }

        private void ConsumablelistView_DoubleClick(object sender, EventArgs e)
        {
            string stConsumableID="";

            lock(locker)
            { 
                if(ConsumablelistView.Items.Count>0)
                {
                    stConsumableID = ConsumablelistView.SelectedItems[0].Text;
                }
                if (!string.IsNullOrEmpty(stConsumableID))
                {
                    ConsumableDetail dlg = new ConsumableDetail(stConsumableID)
                    {
                        StartPosition = FormStartPosition.CenterParent
                    };
                    dlg.ShowDialog();

                    dlg.Dispose();
                }
            }
        }

        private void ConsumableMain_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false; 
        }
    }
}
