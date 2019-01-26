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
    public partial class ConsumableChange : Form
    {
        public ConsumableChange()
        {
            InitializeComponent();
        }

        private string m_stConsumableID = "";
        private string m_stConsumableName = "";
        private string m_stLifetime = "";
        private string m_stWorkingTime = "";
        private string m_stChangetime = "";
        private ConsumableDetail m_DetailDlg;
        public ConsumableChange(string stConsumableID,ConsumableDetail DetailDlg)
        {
            InitializeComponent();

            m_stConsumableID = stConsumableID;
            m_DetailDlg = DetailDlg;

            IDtextBox.Enabled = false;
            NametextBox.Enabled = false;
            LifttimetextBox.Enabled = false;
            LastChangetimetextBox.Enabled = false;
            WorkingtimetextBox.Enabled = false;

            string EXEPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string dbPath = EXEPath + "Demo.db3";
            SQLiteDBHelper db = new SQLiteDBHelper(dbPath);

            string sql3 = "select max(id),ConsumableName,Lifetime,WorkingTime,Changetime from ConsumableLog where ConsumableID = " + stConsumableID;

            using (SQLiteDataReader reader = db.ExecuteReader(sql3, null))
            {
                while (reader.Read())
                {
                    m_stConsumableName = reader["ConsumableName"].ToString();
                    m_stLifetime = reader["Lifetime"].ToString();
                    m_stWorkingTime = reader["WorkingTime"].ToString();
                    m_stChangetime = reader["Changetime"].ToString();

                    IDtextBox.Text = stConsumableID;
                    NametextBox.Text = m_stConsumableName;
                    LifttimetextBox.Text = m_stLifetime;
                    LastChangetimetextBox.Text = m_stChangetime;
                    WorkingtimetextBox.Text = m_stWorkingTime;
                }
            }

            if (DetailDlg==null)
            {
                this.Text = m_stConsumableName + "易损件工作时间已经超过使用寿命，请及时更换！";
            }
            else
            {
                this.Text = m_stConsumableName + "易损件更换";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string stPeople = PeopletextBox.Text;
            if ((string.IsNullOrEmpty(stPeople)))
            {
                MessageBox.Show("请输入更换人员！");
                return;
            }

            string EXEPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            XmlDocument XMLalarmInfo = new XmlDocument();
            XMLalarmInfo.Load(EXEPath + "ConsumableInfoConfig.xml");
            XmlElement el = XMLalarmInfo.DocumentElement;
            XmlNodeList ConsumableInfoNodes = el.GetElementsByTagName("ConsumableInfo");

            foreach (XmlNode node in ConsumableInfoNodes)
            {

                XmlNodeList ConsumableNode = node.ChildNodes;
                string stConsumableID = ((XmlElement)ConsumableNode[0]).InnerText;
                if (0 == stConsumableID.CompareTo(m_stConsumableID))
                {
                    m_stConsumableName = ((XmlElement)ConsumableNode[1]).InnerText;
                    m_stLifetime = ((XmlElement)ConsumableNode[3]).InnerText;
                    break;
                }
            }
            
            string dbPath = EXEPath + "Demo.db3";
            SQLiteDBHelper db = new SQLiteDBHelper(dbPath);

            string sql2 = "INSERT INTO ConsumableLog(datetime,ConsumableID,ConsumableName,Lifetime,WorkingTime,Changetime,ChangePeopleName,Remarks)" +
                  "values(@datetime,@ConsumableID,@ConsumableName,@Lifetime,@WorkingTime,@Changetime,@ChangePeopleName,@Remarks)";

            DateTime CurrentDatetime = DateTime.Now;
            SQLiteParameter[] parameters = new SQLiteParameter[]{
                        new SQLiteParameter("@datetime",DateTime.Now),
                        new SQLiteParameter("@ConsumableID",m_stConsumableID),
                        new SQLiteParameter("@ConsumableName",m_stConsumableName),
                        new SQLiteParameter("@Lifetime",m_stLifetime),
                        new SQLiteParameter("@WorkingTime","0"),
                        new SQLiteParameter("@Changetime",CurrentDatetime),
                        new SQLiteParameter("@ChangePeopleName",PeopletextBox.Text),
                        new SQLiteParameter("@Remarks",ResultrichTextBox.Text)
                };

            int DBResult = db.ExecuteNonQuery(sql2, parameters);
            if (DBResult > 0)
                MessageBox.Show("易损件更换完成！");
            this.Close();

            DTSManager.PostSever.PostConsumableReplaceData(PeopletextBox.Text, Convert.ToInt32(m_stConsumableID), 0, CurrentDatetime.ToString());

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
