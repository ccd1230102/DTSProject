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
    public partial class AlarmDataDialog : Form
    {
        public AlarmDataDialog()
        {
            InitializeComponent();
        }

        public void ThreadStart()
        {
            SetTimerParam();
        }

        public System.Timers.Timer aTimer = new System.Timers.Timer();
        private void TimeThread(object source, System.Timers.ElapsedEventArgs e)
        {//循环发送最新的生产数到server，补发未发送的alram
            //IsPostAlarmData();
            //UnHandleAlarm();
        }

        public void UnHandleAlarm()
        {//未处理的报警将其强制曝出
            string EXEPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string dbPath = EXEPath + "Demo.db3";
            string sql = "select * from AlarmLog where HandleTime is null";
            string stID = "";
            string stAlarmID = "";
            SQLiteDBHelper db = new SQLiteDBHelper(dbPath);
            using (SQLiteDataReader reader = db.ExecuteReader(sql, null))
            {
                while (reader.Read())
                {
                    stID = reader["id"].ToString();
                    stAlarmID = reader["AlarmID"].ToString();
                    AlarmDetailForm dlg = new AlarmDetailForm(stID, 1)
                    {
                        StartPosition = FormStartPosition.CenterParent
                    };
                    dlg.ShowDialog();

                    dlg.Dispose();
                }
            }
        }

        public void IsPostAlarmData()
        {//循环查询未发送到服务端的alram，补发
            List<string> IsPostIDList = new List<string>() { };

            string EXEPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string dbPath = EXEPath + "Demo.db3";
            string sql = "select * from AlarmLog where IsPost = 0";
            string stID = "";
            string stAlarmID = "";
            SQLiteDBHelper db = new SQLiteDBHelper(dbPath);
            using (SQLiteDataReader reader = db.ExecuteReader(sql, null))
            {
                while (reader.Read())
                {
                    stID = reader["id"].ToString();
                    stAlarmID = reader["AlarmID"].ToString();
                    int nWarningID = DTSManager.PostSever.PostWarningData(Convert.ToInt32(stAlarmID));

                    if (nWarningID!=0)
                    {
                        string sql2 = "UPDATE AlarmLog SET IsPost = @IsPost, ServerWaringID = @ServerWaringID where id = " + stID;
                        SQLiteParameter[] parameters = new SQLiteParameter[]{
                                    new SQLiteParameter("@IsPost",1),
                                    new SQLiteParameter("@ServerWaringID",nWarningID)
                            };
                        int DBResult = db.ExecuteNonQuery(sql2, parameters);
                    }
                }
            }
        }

        public void SetTimerParam()
        {
            //到时间的时候执行事件  
            aTimer.Elapsed += new System.Timers.ElapsedEventHandler(TimeThread);
            aTimer.Interval = 1 * 10 * 1000;
            aTimer.AutoReset = true;//执行一次 false，一直执行true  
            //是否执行System.Timers.Timer.Elapsed事件  
            aTimer.Enabled = true;
        }

        private void AlarmDataDialog_Shown(object sender, EventArgs e)
        {
            string EXEPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string dbPath = EXEPath + "Demo.db3";
            string sql = "select * from AlarmLog";
            SQLiteDBHelper db = new SQLiteDBHelper(dbPath);

            using (SQLiteDataReader reader = db.ExecuteReader(sql, null))
            {
                while (reader.Read())
                {
                    string stID = reader["id"].ToString();
                    string stdatetime = reader["datetime"].ToString();
                    string stAlarmID = reader["AlarmID"].ToString();
                    string stAlarmName = reader["AlarmName"].ToString();
                    string stAlarmLevel = reader["AlarmLevel"].ToString();
                    string stHandleAlarmMode = reader["HandleAlarmMode"].ToString();
                    string stHandleTime = reader["HandleTime"].ToString();
                    string stHandlePeopleName = reader["HandlePeopleName"].ToString();
                    string stHandleResult = reader["HandleResult"].ToString();

                    ListViewItem item1 = new ListViewItem(stID);
                    item1.SubItems.Add(stdatetime);
                    item1.SubItems.Add(stAlarmID);
                    item1.SubItems.Add(stAlarmName);
                    item1.SubItems.Add(stAlarmLevel);
                    item1.SubItems.Add(stHandleAlarmMode);
                    item1.SubItems.Add(stHandleTime);
                    item1.SubItems.Add(stHandlePeopleName);
                    if (string.IsNullOrEmpty(stHandleResult))
                        item1.SubItems.Add("未处理");
                    else
                        item1.SubItems.Add(stHandleResult);
                    AlarmlistView.Items.Add(item1);
                }
            }

        }

        private void AlarmlistView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string stID = AlarmlistView.SelectedItems[0].Text;

            if (!string.IsNullOrEmpty(stID))
            {
                AlarmDetailForm dlg = new AlarmDetailForm(stID, 2)
                {
                    StartPosition = FormStartPosition.CenterParent
                };
                dlg.ShowDialog();

                dlg.Dispose();
            }
            
        }

    }
}
