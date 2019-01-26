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
    public partial class AlarmDetailForm : Form
    {
        private string m_DBid;
        private int m_Mode;
        public AlarmDetailForm()
        {
            InitializeComponent();
        }
        public AlarmDetailForm(string ID,int Mode)
        {
            InitializeComponent();
            m_DBid = ID;
            m_Mode = Mode;
            if(m_Mode == 1)
            {
                HandleTimetextBox.Visible = false;
                label8.Visible = false;

                TimetextBox.Enabled = false;
                IDtextBox.Enabled = false;
                NametextBox.Enabled = false;
                LeveltextBox.Enabled = false;
                HandlerichTextBox.Enabled = false;
            }
            if(m_Mode == 2)
            {
                TimetextBox.Enabled = false;
                IDtextBox.Enabled = false;
                NametextBox.Enabled = false;
                LeveltextBox.Enabled = false;
                HandlerichTextBox.Enabled = false;
                HandleTimetextBox.Enabled = false;
                button1.Text = "修改报警处理";
            }
        }

        private void AlarmDetailForm_Shown(object sender, EventArgs e)
        {
            string EXEPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string dbPath = EXEPath + "Demo.db3";
            string sql = "select * from AlarmLog where id = " + m_DBid;
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

                    TimetextBox.Text = stdatetime;
                    IDtextBox.Text = stAlarmID;
                    NametextBox.Text = stAlarmName;
                    LeveltextBox.Text = stAlarmLevel;
                    HandlerichTextBox.Text = stHandleAlarmMode;
                    PeopletextBox.Text = stHandlePeopleName;
                    HandleTimetextBox.Text = stHandleTime;
                    ResultrichTextBox.Text = stHandleResult;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string stPeople = PeopletextBox.Text;
            string stResult = ResultrichTextBox.Text;
            if((string.IsNullOrEmpty(stPeople))||(string.IsNullOrEmpty(stResult)))
            {
                MessageBox.Show("请输入处理人员和处理结果！");
                return;
            }                
            
            string EXEPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string dbPath = EXEPath + "Demo.db3";
            SQLiteDBHelper db = new SQLiteDBHelper(dbPath);
            string sql2 = "UPDATE AlarmLog SET HandleTime = @HandleTime,HandlePeopleName = @HandlePeopleName,HandleResult = @HandleResult where id = " + m_DBid;

            SQLiteParameter[] parameters = new SQLiteParameter[]{
                    new SQLiteParameter("@HandleTime",DateTime.Now),
                    new SQLiteParameter("@HandlePeopleName",stPeople),
                    new SQLiteParameter("@HandleResult",stResult)
            };
            int DBResult = db.ExecuteNonQuery(sql2, parameters);

            if (DBResult>0)
                MessageBox.Show("报警处理完成！");

            string ServerWarningID = "";
            string stdatetime="";
            string stHandleTime="";
            string stHandleResult = "";
            string sql = "select * from AlarmLog where id = " + m_DBid;;
            using (SQLiteDataReader reader = db.ExecuteReader(sql, null))
            {
                while (reader.Read())
                {
                    string stID = reader["id"].ToString();
                    stdatetime = reader["datetime"].ToString();
                    string stAlarmID = reader["AlarmID"].ToString();
                    string stAlarmName = reader["AlarmName"].ToString();
                    string stAlarmLevel = reader["AlarmLevel"].ToString();               
                    string stHandleAlarmMode = reader["HandleAlarmMode"].ToString();
                    ServerWarningID = reader["ServerWarningID"].ToString();
                    stHandleTime = reader["HandleTime"].ToString();
                    string stHandlePeopleName = reader["HandlePeopleName"].ToString();
                    stHandleResult = reader["HandleResult"].ToString();
                }
            }

            DateTime Handletime = Convert.ToDateTime(stHandleTime);
            DateTime datetime = Convert.ToDateTime(stdatetime);
            TimeSpan Span = Handletime - datetime;
            int nDuration = Span.Seconds;
            DTSManager.PostSever.PostWarningFixedData(ServerWarningID, stHandleTime, stHandleResult, nDuration);

            this.Close();          
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
