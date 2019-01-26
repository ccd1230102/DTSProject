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

namespace DTSLibrary
{
    public partial class RunningDataDialog : Form
    {
        private List<RunData> m_RunDataList = new List<RunData>();
        public RunningDataDialog()
        {
            InitializeComponent();        
        }

        public void ThreadStart()
        {
            SetTimerParam();
        }

        public void Threadstop()
        {
            aTimer.Enabled = false;
        }

        public System.Timers.Timer aTimer = new System.Timers.Timer();

        private void TimeThread(object source, System.Timers.ElapsedEventArgs e)
        {//循环发送最新的生产数到server，补发未发送的start和stop
            CreatRunDatalist();
            RunData run = m_RunDataList[m_RunDataList.Count()-1];
            DTSManager.PostSever.PostRunningData(2, DTSManager.m_shift, run.count);
            //IsPostRunData();
        }

        public void IsPostRunData()
        {//循环查询未发送到服务端的start和stop信息，补发
            List<string> IsPostIDList = new List<string>() { };
            string EXEPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string dbPath = EXEPath + "Demo.db3";
            string sql = "select * from RunningLog where IsPost = 0 and (Type=='start' or Type=='stop')";
            string stID = "";
            SQLiteDBHelper db = new SQLiteDBHelper(dbPath);
            using (SQLiteDataReader reader = db.ExecuteReader(sql, null))
            {
                while (reader.Read())
                {
                    stID = reader["id"].ToString();
                    string stdatetime = reader["datetime"].ToString();
                    string stType = reader["Type"].ToString();
                    string stcount = reader["count"].ToString();
                    string stRelateID = reader["RelateID"].ToString();
                    string IsPost = reader["IsPost"].ToString();

                    bool ret = false;
                    if (stType.CompareTo("start")==0) 
                        ret = DTSManager.PostSever.PostRunningData(1, DTSManager.m_shift, 0);
                    else
                        ret = DTSManager.PostSever.PostRunningData(3, DTSManager.m_shift, 0);
                    
                    if(ret)
                    {
                        IsPostIDList.Add(stID);                       
                    }
                }
            }
            foreach (string elm in IsPostIDList)
            {
                string sql2 = "UPDATE RunningLog SET IsPost = @IsPost where id = " + stID;
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
            aTimer.Interval = 1 * 60 * 1000;
            aTimer.AutoReset = true;//执行一次 false，一直执行true  
            //是否执行System.Timers.Timer.Elapsed事件  
            aTimer.Enabled = true;
        }

        private void RunningDataDialog_Shown(object sender, EventArgs e)
        {
            CreatRunDatalist();
            RunDatalistView.Items.Clear();
            foreach (RunData run in m_RunDataList)
            {
                ListViewItem item1 = new ListViewItem(run.Starttime);
                item1.SubItems.Add(run.Starttime);
                item1.SubItems.Add(run.Stoptime);
                item1.SubItems.Add(run.count.ToString());
                RunDatalistView.Items.Add(item1);
            }
        }

        public void CreatRunDatalist()
        {//建立班次和生产数的list
            m_RunDataList.Clear();
            string EXEPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string dbPath = EXEPath + "Demo.db3";
            string sql = "select * from RunningLog";
            SQLiteDBHelper db = new SQLiteDBHelper(dbPath);

            string stStarttime = "";
            string stStoptime = "";
            int ProduceCount = 0;
            string TypeStart = "start";
            string TypeStop = "stop";
            string TypeCount = "Produce";
            
            using (SQLiteDataReader reader = db.ExecuteReader(sql, null))
            {
                while (reader.Read())
                {
                    string stID = reader["id"].ToString();
                    string stdatetime = reader["datetime"].ToString();
                    string stType = reader["Type"].ToString();
                    string stcount = reader["count"].ToString();
                    string stRelateID = reader["RelateID"].ToString();
                    string IsPost = reader["IsPost"].ToString();

                    if (stType.Equals(TypeStart) && string.IsNullOrEmpty(stStarttime))
                    {
                        stStarttime = stdatetime;
                        RunData run = new RunData(stStarttime, stStoptime, ProduceCount);
                        m_RunDataList.Add(run);
                    }

                    if (stType.Equals(TypeCount) && !string.IsNullOrEmpty(stStarttime))
                    {
                        ProduceCount += Convert.ToInt32(stcount);
                        int nListLenght = m_RunDataList.Count();
                        m_RunDataList[nListLenght - 1].count = ProduceCount;
                    }

                    if (stType.Equals(TypeStop) && !string.IsNullOrEmpty(stStarttime))
                    {
                        stStoptime = stdatetime;
                        int nListLenght = m_RunDataList.Count();
                        m_RunDataList[nListLenght - 1].Stoptime = stStoptime;
                        stStarttime = "";
                        ProduceCount = 0;
                        stStoptime = "";
                    }
                }
            }
        }

        class RunData
        {
            public string Starttime { get; set; } //
            public string Stoptime { get; set; } //
            public int count { get; set; } //

            public RunData(string _Starttime, string _Stoptime, int _count)
            {
                this.Starttime = _Starttime;
                this.Stoptime = _Stoptime;
                this.count = _count;
            }
        }
    }
}
