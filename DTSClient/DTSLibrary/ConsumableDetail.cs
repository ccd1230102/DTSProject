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
    public partial class ConsumableDetail : Form
    {
        public ConsumableDetail()
        {
            InitializeComponent();
        }
        private string m_stConsumableID;
        public ConsumableDetail(string stConsumableID)
        {
            InitializeComponent();
            m_stConsumableID = stConsumableID;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(m_stConsumableID))
            {
                ConsumableChange dlg = new ConsumableChange(m_stConsumableID, this)
                {
                    StartPosition = FormStartPosition.CenterParent
                };
                dlg.ShowDialog();

                dlg.Dispose();
            }
            
        }

        private void ConsumableDetail_Shown(object sender, EventArgs e)
        {
            ReflashData();
        }
        public void ReflashData()
        {
            ConsumableDetaillistView.Items.Clear();
            string EXEPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string dbPath = EXEPath + "Demo.db3";
            SQLiteDBHelper db = new SQLiteDBHelper(dbPath);

            string sql3 = "select * from ConsumableLog where ConsumableID = " + m_stConsumableID;

            string stID = "";
            string stdatetime = "";
            m_stConsumableID = "";
            string stConsumableName = "";
            string stConsumableInfo = "";
            string stLifetime = "";
            string stWorkingTime = "";
            string stChangetime = "";
            string stChangePeopleName = "";
            string stRemarks = "";
            string stIsPost = "";
            using (SQLiteDataReader reader = db.ExecuteReader(sql3, null))
            {
                while (reader.Read())
                {
                    stID = reader["id"].ToString();
                    stdatetime = reader["datetime"].ToString();
                    m_stConsumableID = reader["ConsumableID"].ToString();
                    stConsumableName = reader["ConsumableName"].ToString();
                    stConsumableInfo = reader["ConsumableInfo"].ToString();
                    stLifetime = reader["Lifetime"].ToString();
                    stLifetime = (Convert.ToInt32(stLifetime) / 60).ToString() + "小时(" + stLifetime + "分钟)";
                    stWorkingTime = reader["WorkingTime"].ToString() + "分钟";
                    stChangetime = reader["Changetime"].ToString();
                    stChangePeopleName = reader["ChangePeopleName"].ToString();
                    stRemarks = reader["Remarks"].ToString();
                    stIsPost = reader["IsPost"].ToString();

                    ListViewItem item1 = new ListViewItem(m_stConsumableID);
                    item1.SubItems.Add(stConsumableName);
                    item1.SubItems.Add(stLifetime);
                    item1.SubItems.Add(stChangetime);
                    item1.SubItems.Add(stWorkingTime);
                    item1.SubItems.Add(stChangePeopleName);
                    item1.SubItems.Add(stRemarks);
                    ConsumableDetaillistView.Items.Add(item1);
                }
            }
        }
    }
}
